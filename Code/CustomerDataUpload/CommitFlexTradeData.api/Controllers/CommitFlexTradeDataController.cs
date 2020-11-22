using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommitFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexTradeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitFlexTradeDataController> _logger;
        private readonly Int64 commitFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFlexTradeDataController(ILogger<CommitFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexTradeDataAPI, password);
            commitFlexTradeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexTradeData/Commit")]
        public void Commit([FromBody] object data)
        {
            var informationMethods = new Methods.InformationSchema();
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFlexTradeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitFlexTradeDataAPI, commitFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexTradeDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] where CanCommit = 1
                var flexTradeEntities = new Methods.TempSchema.CustomerDataUpload.FlexTrade().FlexTrade_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexTradeEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(flexTradeEntities);

                if(!commitableFlexTradeEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, false, null);
                    return;
                }

                var informationVolumeUnitEnums = new Enums.InformationSchema.VolumeUnit();
                var informationRateUnitEnums = new Enums.InformationSchema.RateUnit();
                var customerTradeAttributeEnums = new Enums.CustomerSchema.Trade.Attribute();
                var customerBasketAttributeEnums = new Enums.CustomerSchema.Basket.Attribute();
                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                //Setup AttributeId Dictionary
                var attributeIdDictionary = new Dictionary<string, long>
                {
                    {customerBasketAttributeEnums.BasketReference, customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(customerBasketAttributeEnums.BasketReference)},
                    {customerTradeAttributeEnums.TradeReference, customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(customerTradeAttributeEnums.TradeReference)},
                    {customerTradeAttributeEnums.TradeDate, customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(customerTradeAttributeEnums.TradeDate)},
                    {customerTradeAttributeEnums.TradeVolume, customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(customerTradeAttributeEnums.TradeVolume)},
                    {customerTradeAttributeEnums.TradePrice, customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(customerTradeAttributeEnums.TradePrice)},
                    {informationVolumeUnitEnums.MegaWatt, informationMethods.VolumeUnit_GetVolumeUnitIdByVolumeUnitDescription(informationVolumeUnitEnums.MegaWatt)},
                    {informationRateUnitEnums.PoundPerMegaWattHour, informationMethods.RateUnit_GetRateUnitIdByRateUnitDescription(informationRateUnitEnums.PoundPerMegaWattHour)},
                };

                var baskets = commitableFlexTradeEntities.Select(cfte => cfte.BasketReference).Distinct()
                    .ToDictionary(b => b, b => customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(attributeIdDictionary[customerBasketAttributeEnums.BasketReference], b));

                var tradeProducts = commitableFlexTradeEntities.Select(cfte => cfte.TradeProduct).Distinct()
                    .ToDictionary(tp => tp, tp => informationMethods.TradeProduct_GetTradeProductIdByTradeProductDescription(tp));

                var tradeDirections = commitableFlexTradeEntities.Select(cfte => informationMethods.GetTradeDirection(cfte.Direction)).Distinct()
                    .ToDictionary(td => td, td => informationMethods.TradeDirection_GetTradeDirectionIdByTradeDirectionDescription(td));

                foreach(var flexTradeEntity in commitableFlexTradeEntities.Where(cfte => baskets[cfte.BasketReference] > 0))
                {
                    //If TradeReference is empty, create new trade
                    //Else update existing trade
                    var tradeDetails = new Dictionary<long, string>
                    {
                        {attributeIdDictionary[customerTradeAttributeEnums.TradeReference], flexTradeEntity.TradeReference},
                        {attributeIdDictionary[customerTradeAttributeEnums.TradeDate], flexTradeEntity.TradeDate},
                        {attributeIdDictionary[customerTradeAttributeEnums.TradeVolume], flexTradeEntity.Volume},
                        {attributeIdDictionary[customerTradeAttributeEnums.TradePrice], flexTradeEntity.Price},
                    };

                    var tradeId = 0L;
                    var tradeReference = tradeDetails[attributeIdDictionary[customerTradeAttributeEnums.TradeReference]];

                    if(string.IsNullOrWhiteSpace(tradeReference))
                    {
                        tradeId = customerMethods.InsertNewTrade(createdByUserId, sourceId);
                        tradeDetails[attributeIdDictionary[customerTradeAttributeEnums.TradeReference]] = customerMethods.Trade_GetTradeGUIDByTradeId(tradeId);

                        //Insert into [Customer].[TradeDetail]
                        foreach(var tradeDetail in tradeDetails)
                        {
                            customerMethods.TradeDetail_Insert(createdByUserId, sourceId, tradeId, tradeDetail.Key, tradeDetail.Value);
                        }
                    }
                    else
                    {
                        //Get TradeId from [Customer].[TradeDetail] by TradeReference
                        tradeId = customerMethods.TradeDetail_GetTradeIdByTradeAttributeIdAndTradeDetailDescription(attributeIdDictionary[customerTradeAttributeEnums.TradeReference], tradeReference);

                        //Get TradeDetails from [Customer].[TradeDetail] by TradeId
                        var currentTradeDetails = customerMethods.TradeDetail_GetListByTradeId(tradeId);

                        //Check details against upload values and update any changes
                        foreach(var tradeDetail in tradeDetails)
                        {
                            var currentTradeDetail = currentTradeDetails.First(ctd => ctd.TradeAttributeId == tradeDetail.Key);
                            if(tradeDetail.Value != currentTradeDetail.TradeDetailDescription)
                            {
                                customerMethods.TradeDetail_DeleteByTradeDetailId(currentTradeDetail.TradeDetailId);
                                customerMethods.TradeDetail_Insert(createdByUserId, sourceId, tradeId, tradeDetail.Key, tradeDetail.Value);
                            }
                        }
                    }

                    //Insert into [Mapping].[BasketToTrade]
                    var basketToTradeId = mappingMethods.BasketToTrade_GetBasketToTradeIdByBasketIdAndTradeId(baskets[flexTradeEntity.BasketReference], tradeId);
                    if(basketToTradeId == 0)
                    {
                        mappingMethods.BasketToTrade_Insert(createdByUserId, sourceId, baskets[flexTradeEntity.BasketReference], tradeId);
                    }

                    //Get TradeDetailId by TradeId and TradeVolumeTradeAttributeId
                    var tradeDetailId = customerMethods.TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(tradeId, attributeIdDictionary[customerTradeAttributeEnums.TradeVolume]);

                    //Insert into [Mapping].[TradeDetailToVolumeUnit]
                    var tradeDetailToVolumeUnitId = mappingMethods.TradeDetailToVolumeUnit_GetTradeDetailToVolumeUnitIdByTradeDetailIdAndVolumeUnitId(tradeDetailId, attributeIdDictionary[informationVolumeUnitEnums.MegaWatt]);
                    if(tradeDetailToVolumeUnitId == 0)
                    {
                        mappingMethods.TradeDetailToVolumeUnit_Insert(createdByUserId, sourceId, tradeDetailId, attributeIdDictionary[informationVolumeUnitEnums.MegaWatt]);
                    }

                    //Get TradeDetailId by TradeId and TradePriceTradeAttributeId
                    tradeDetailId = customerMethods.TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(tradeId, attributeIdDictionary[customerTradeAttributeEnums.TradePrice]);

                    //Insert into [Mapping].[RateUnitToTradeDetail]
                    var rateUnitToTradeDetailId = mappingMethods.RateUnitToTradeDetail_GetRateUnitToTradeDetailIdByRateUnitIdAndTradeDetailId(tradeDetailId, attributeIdDictionary[informationRateUnitEnums.PoundPerMegaWattHour]);
                    if(rateUnitToTradeDetailId == 0)
                    {
                        mappingMethods.RateUnitToTradeDetail_Insert(createdByUserId, sourceId, attributeIdDictionary[informationRateUnitEnums.PoundPerMegaWattHour], tradeDetailId);
                    }

                    //Get TradeProductId from [Information].[TradeProduct] by TradeProductDescription
                    var tradeProductId = tradeProducts[flexTradeEntity.TradeProduct];

                    //Insert into [Mapping].[TradeToTradeProduct]
                    var tradeToTradeProductId = mappingMethods.TradeToTradeProduct_GetTradeToTradeProductIdByTradeIdAndTradeProductId(tradeId, tradeProductId);
                    if(tradeToTradeProductId == 0)
                    {
                        mappingMethods.TradeToTradeProduct_Insert(createdByUserId, sourceId, tradeId, tradeProductId);
                    }

                    //Get TradeDirectionId from [Information].[TradeDirection] by TradeDirectionDescription
                    var tradeDirection = informationMethods.GetTradeDirection(flexTradeEntity.Direction);
                    var tradeDirectionId = tradeDirections[tradeDirection];

                    //Insert into [Mapping].[TradeToTradeDirection]
                    var tradeToTradeDirectionId = mappingMethods.TradeToTradeDirection_GetTradeToTradeDirectionIdByTradeIdAndTradeDirectionId(tradeId, tradeDirectionId);
                    if(tradeToTradeDirectionId == 0)
                    {
                        mappingMethods.TradeToTradeDirection_Insert(createdByUserId, sourceId, tradeId, tradeDirectionId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}