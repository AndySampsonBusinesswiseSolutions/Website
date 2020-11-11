using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Basket.Attribute _customerBasketAttributeEnums = new Enums.CustomerSchema.Basket.Attribute();
        private readonly Enums.CustomerSchema.Trade.Attribute _customerTradeAttributeEnums = new Enums.CustomerSchema.Trade.Attribute();
        private readonly Enums.InformationSchema.RateUnit _informationRateUnitEnums = new Enums.InformationSchema.RateUnit();
        private readonly Enums.InformationSchema.TradeDirection _informationTradeDirectionEnums = new Enums.InformationSchema.TradeDirection();
        private readonly Enums.InformationSchema.VolumeUnit _informationVolumeUnitEnums = new Enums.InformationSchema.VolumeUnit();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFlexTradeDataController(ILogger<CommitFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexTradeDataAPI, password);
            commitFlexTradeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexTradeData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFlexTradeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexTradeDataAPI, commitFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexTradeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] where CanCommit = 1
                var flexTradeEntities = new Methods.Temp.CustomerDataUpload.FlexTrade().FlexTrade_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexTradeEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(flexTradeEntities);

                if(!commitableFlexTradeEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, false, null);
                    return;
                }

                //Setup AttributeId Dictionary
                var attributeIdDictionary = new Dictionary<string, long>
                {
                    {_customerBasketAttributeEnums.BasketReference, _customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference)},
                    {_customerTradeAttributeEnums.TradeReference, _customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(_customerTradeAttributeEnums.TradeReference)},
                    {_customerTradeAttributeEnums.TradeDate, _customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(_customerTradeAttributeEnums.TradeDate)},
                    {_customerTradeAttributeEnums.TradeVolume, _customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(_customerTradeAttributeEnums.TradeVolume)},
                    {_customerTradeAttributeEnums.TradePrice, _customerMethods.TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(_customerTradeAttributeEnums.TradePrice)},
                    {_informationVolumeUnitEnums.MegaWatt, _informationMethods.VolumeUnit_GetVolumeUnitIdByVolumeUnitDescription(_informationVolumeUnitEnums.MegaWatt)},
                    {_informationRateUnitEnums.PoundPerMegaWattHour, _informationMethods.RateUnit_GetRateUnitIdByRateUnitDescription(_informationRateUnitEnums.PoundPerMegaWattHour)},
                };

                var baskets = commitableFlexTradeEntities.Select(cfte => cfte.BasketReference).Distinct()
                    .ToDictionary(b => b, b => _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(attributeIdDictionary[_customerBasketAttributeEnums.BasketReference], b));

                var tradeProducts = commitableFlexTradeEntities.Select(cfte => cfte.TradeProduct).Distinct()
                    .ToDictionary(tp => tp, tp => _informationMethods.TradeProduct_GetTradeProductIdByTradeProductDescription(tp));

                var tradeDirections = commitableFlexTradeEntities.Select(cfte => GetTradeDirection(cfte.Direction)).Distinct()
                    .ToDictionary(td => td, td => _informationMethods.TradeDirection_GetTradeDirectionIdByTradeDirectionDescription(td));

                foreach(var flexTradeEntity in commitableFlexTradeEntities.Where(cfte => baskets[cfte.BasketReference] > 0))
                {
                    //If TradeReference is empty, create new trade
                    //Else update existing trade
                    var tradeDetails = new Dictionary<long, string>
                    {
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeReference], flexTradeEntity.TradeReference},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeDate], flexTradeEntity.TradeDate},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeVolume], flexTradeEntity.Volume},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradePrice], flexTradeEntity.Price},
                    };

                    var tradeId = 0L;
                    var tradeReference = tradeDetails[attributeIdDictionary[_customerTradeAttributeEnums.TradeReference]];

                    if(string.IsNullOrWhiteSpace(tradeReference))
                    {
                        tradeId = _customerMethods.InsertNewTrade(createdByUserId, sourceId);
                        tradeDetails[attributeIdDictionary[_customerTradeAttributeEnums.TradeReference]] = _customerMethods.Trade_GetTradeGUIDByTradeId(tradeId);

                        //Insert into [Customer].[TradeDetail]
                        foreach(var tradeDetail in tradeDetails)
                        {
                            _customerMethods.TradeDetail_Insert(createdByUserId, sourceId, tradeId, tradeDetail.Key, tradeDetail.Value);
                        }
                    }
                    else
                    {
                        //Get TradeId from [Customer].[TradeDetail] by TradeReference
                        tradeId = _customerMethods.TradeDetail_GetTradeIdByTradeAttributeIdAndTradeDetailDescription(attributeIdDictionary[_customerTradeAttributeEnums.TradeReference], tradeReference);

                        //Get TradeDetails from [Customer].[TradeDetail] by TradeId
                        var currentTradeDetails = _customerMethods.TradeDetail_GetListByTradeId(tradeId);

                        //Check details against upload values and update any changes
                        foreach(var tradeDetail in tradeDetails)
                        {
                            var currentTradeDetail = currentTradeDetails.First(r => r.Field<long>("TradeAttributeId") == tradeDetail.Key);
                            if(tradeDetail.Value != currentTradeDetail["TradeDetailDescription"].ToString())
                            {
                                _customerMethods.TradeDetail_DeleteByTradeDetailId(Convert.ToInt64(currentTradeDetail["TradeDetailDescription"].ToString()));
                                _customerMethods.TradeDetail_Insert(createdByUserId, sourceId, tradeId, tradeDetail.Key, tradeDetail.Value);
                            }
                        }
                    }

                    //Insert into [Mapping].[BasketToTrade]
                    var basketToTradeId = _mappingMethods.BasketToTrade_GetBasketToTradeIdByBasketIdAndTradeId(baskets[flexTradeEntity.BasketReference], tradeId);
                    if(basketToTradeId == 0)
                    {
                        _mappingMethods.BasketToTrade_Insert(createdByUserId, sourceId, baskets[flexTradeEntity.BasketReference], tradeId);
                    }

                    //Get TradeDetailId by TradeId and TradeVolumeTradeAttributeId
                    var tradeDetailId = _customerMethods.TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(tradeId, attributeIdDictionary[_customerTradeAttributeEnums.TradeVolume]);

                    //Insert into [Mapping].[TradeDetailToVolumeUnit]
                    var tradeDetailToVolumeUnitId = _mappingMethods.TradeDetailToVolumeUnit_GetTradeDetailToVolumeUnitIdByTradeDetailIdAndVolumeUnitId(tradeDetailId, attributeIdDictionary[_informationVolumeUnitEnums.MegaWatt]);
                    if(tradeDetailToVolumeUnitId == 0)
                    {
                        _mappingMethods.TradeDetailToVolumeUnit_Insert(createdByUserId, sourceId, tradeDetailId, attributeIdDictionary[_informationVolumeUnitEnums.MegaWatt]);
                    }

                    //Get TradeDetailId by TradeId and TradePriceTradeAttributeId
                    tradeDetailId = _customerMethods.TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(tradeId, attributeIdDictionary[_customerTradeAttributeEnums.TradePrice]);

                    //Insert into [Mapping].[RateUnitToTradeDetail]
                    var rateUnitToTradeDetailId = _mappingMethods.RateUnitToTradeDetail_GetRateUnitToTradeDetailIdByRateUnitIdAndTradeDetailId(tradeDetailId, attributeIdDictionary[_informationRateUnitEnums.PoundPerMegaWattHour]);
                    if(rateUnitToTradeDetailId == 0)
                    {
                        _mappingMethods.RateUnitToTradeDetail_Insert(createdByUserId, sourceId, attributeIdDictionary[_informationRateUnitEnums.PoundPerMegaWattHour], tradeDetailId);
                    }

                    //Get TradeProductId from [Information].[TradeProduct] by TradeProductDescription
                    var tradeProductId = tradeProducts[flexTradeEntity.TradeProduct];

                    //Insert into [Mapping].[TradeToTradeProduct]
                    var tradeToTradeProductId = _mappingMethods.TradeToTradeProduct_GetTradeToTradeProductIdByTradeIdAndTradeProductId(tradeId, tradeProductId);
                    if(tradeToTradeProductId == 0)
                    {
                        _mappingMethods.TradeToTradeProduct_Insert(createdByUserId, sourceId, tradeId, tradeProductId);
                    }

                    //Get TradeDirectionId from [Information].[TradeDirection] by TradeDirectionDescription
                    var tradeDirection = GetTradeDirection(flexTradeEntity.Direction);
                    var tradeDirectionId = tradeDirections[tradeDirection];

                    //Insert into [Mapping].[TradeToTradeDirection]
                    var tradeToTradeDirectionId = _mappingMethods.TradeToTradeDirection_GetTradeToTradeDirectionIdByTradeIdAndTradeDirectionId(tradeId, tradeDirectionId);
                    if(tradeToTradeDirectionId == 0)
                    {
                        _mappingMethods.TradeToTradeDirection_Insert(createdByUserId, sourceId, tradeId, tradeDirectionId);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private string GetTradeDirection(string direction)
        {
            return direction.StartsWith("B")
                ? _informationTradeDirectionEnums.Buy
                : _informationTradeDirectionEnums.Sell;
        }
    }
}