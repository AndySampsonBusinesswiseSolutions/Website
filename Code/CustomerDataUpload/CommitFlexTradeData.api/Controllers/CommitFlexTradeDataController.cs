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

namespace CommitFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private readonly Enums.Customer.Trade.Attribute _customerTradeAttributeEnums = new Enums.Customer.Trade.Attribute();
        private readonly Enums.Information.RateUnit _informationRateUnitEnums = new Enums.Information.RateUnit();
        private readonly Enums.Information.TradeDirection _informationTradeDirectionEnums = new Enums.Information.TradeDirection();
        private readonly Enums.Information.VolumeUnit _informationVolumeUnitEnums = new Enums.Information.VolumeUnit();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitFlexTradeDataAPIId;

        public CommitFlexTradeDataController(ILogger<CommitFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexTradeDataAPI, _systemAPIPasswordEnums.CommitFlexTradeDataAPI);
            commitFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexTradeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexTradeData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFlexTradeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexTradeDataAPI, commitFlexTradeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] where CanCommit = 1
                var customerDataRows = _tempCustomerMethods.FlexTrade_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(customerDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexTradeDataAPIId, false, null);
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

                foreach(var dataRow in commitableDataRows)
                {
                    //Get BasketId from [Customer].[BasketDetail] by BasketReference
                    var basketReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference);
                    var basketId = _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(attributeIdDictionary[_customerBasketAttributeEnums.BasketReference], basketReference);

                    //If TradeReference is empty, create new trade
                    //Else update existing trade
                    var tradeDetails = new Dictionary<long, string>
                    {
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeReference], dataRow.Field<string>(_customerDataUploadValidationEntityEnums.TradeReference)},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeDate], dataRow.Field<string>(_customerDataUploadValidationEntityEnums.TradeDate)},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradeVolume], dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Volume)},
                        {attributeIdDictionary[_customerTradeAttributeEnums.TradePrice], dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Price)},
                    };

                    var tradeId = 0L;
                    var tradeReference = tradeDetails[attributeIdDictionary[_customerTradeAttributeEnums.TradeReference]];

                    if(string.IsNullOrWhiteSpace(tradeReference))
                    {
                        //Create new TradeGUID
                        var tradeGUID = Guid.NewGuid().ToString();
                        tradeReference = Guid.NewGuid().ToString();

                        //Insert into [Customer].[Trade]
                        _customerMethods.Trade_Insert(createdByUserId, sourceId, tradeGUID);
                        tradeId = _customerMethods.Trade_GetTradeIdByTradeGUID(tradeGUID);

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
                    var basketToTradeId = _mappingMethods.BasketToTrade_GetBasketToTradeIdByBasketIdAndTradeId(basketId, tradeId);
                    if(basketToTradeId == 0)
                    {
                        _mappingMethods.BasketToTrade_Insert(createdByUserId, sourceId, basketId, tradeId);
                    }

                    //Get TradeDetailId by TradeId and TradeVolumeTradeAttributeId
                    var tradeDetailId = _customerMethods.TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(tradeId, attributeIdDictionary[_customerTradeAttributeEnums.TradeVolume]);

                    //Insert into [Mapping].[TradeDetailToVolumeUnit]
                    var tradeDetailToVolumeUnitId = _mappingMethods.TradeDetailToVolumeUnit_GetTradeDetailToVolumeUnitIdByTradeDetailIdAndVolumeUnitId(tradeDetailId, attributeIdDictionary[_informationVolumeUnitEnums.MegaWatt]);
                    if(tradeDetailToVolumeUnitId == 0)
                    {
                        _mappingMethods.TradeDetailToVolumeUnit_Insert(createdByUserId, sourceId, attributeIdDictionary[_informationVolumeUnitEnums.MegaWatt], tradeDetailId);
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
                    var tradeProductId = _informationMethods.TradeProduct_GetTradeProductIdByTradeProductDescription(dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Product));

                    //Insert into [Mapping].[TradeToTradeProduct]
                    var tradeToTradeProductId = _mappingMethods.TradeToTradeProduct_GetTradeToTradeProductIdByTradeIdAndTradeProductId(tradeId, tradeProductId);
                    if(tradeToTradeProductId == 0)
                    {
                        _mappingMethods.TradeToTradeProduct_Insert(createdByUserId, sourceId, tradeId, tradeProductId);
                    }

                    //Get TradeDirectionId from [Information].[TradeDirection] by TradeDirectionDescription
                    var tradeDirection = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Direction).StartsWith("B")
                        ? _informationTradeDirectionEnums.Buy
                        : _informationTradeDirectionEnums.Sell;
                    var tradeDirectionId = _informationMethods.TradeDirection_GetTradeDirectionIdByTradeDirectionDescription(tradeDirection);

                    //Insert into [Mapping].[TradeToTradeDirection]
                    var tradeToTradeDirectionId = _mappingMethods.TradeToTradeDirection_GetTradeToTradeDirectionIdByTradeIdAndTradeDirectionId(tradeId, tradeDirectionId);
                    if(tradeToTradeDirectionId == 0)
                    {
                        _mappingMethods.TradeToTradeDirection_Insert(createdByUserId, sourceId, tradeId, tradeDirectionId);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

