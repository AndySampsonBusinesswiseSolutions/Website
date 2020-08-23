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

namespace CommitMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterDataAPIId;

        public CommitMeterDataController(ILogger<CommitMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterDataAPI, _systemAPIPasswordEnums.CommitMeterDataAPI);
            commitMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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
                    commitMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterDataAPI, commitMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterDataAPIId, false, null);
                    return;
                }

                //For each column, get MeterAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier), _customerDataUploadValidationEntityEnums.MPXN},
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.SupplyCapacity), _customerDataUploadValidationEntityEnums.Capacity},
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.StandardOfftakeQuantity), _customerDataUploadValidationEntityEnums.StandardOfftakeQuantity},
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.AnnualUsage), _customerDataUploadValidationEntityEnums.AnnualUsage},
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterSerialNumber), _customerDataUploadValidationEntityEnums.MeterSerialNumber},
                    {_customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.ImportExport), _customerDataUploadValidationEntityEnums.ImportExport},
                };

                var detailDictionary = new Dictionary<long, string>();

                foreach(var attribute in attributes)
                {
                    detailDictionary.Add(attribute.Key, string.Empty);
                }

                foreach(var dataRow in commitableDataRows)
                {
                    foreach(var attribute in attributes)
                    {
                        detailDictionary[attribute.Key] = dataRow.Field<string>(attribute.Value);
                    }

                    //Get MeterId by MPXN
                    var meterId = _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value).First();

                    if(meterId == 0)
                    {
                        meterId = _customerMethods.InsertNewMeter(createdByUserId, sourceId);

                        //Insert into [Customer].[MeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            _customerMethods.MeterDetail_Insert(createdByUserId, sourceId, meterId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[MeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailDataRow = _customerMethods.MeterDetail_GetByMeterIdAndMeterAttributeId(meterId, detail.Key);
                            var currentDetail = currentDetailDataRow.Field<string>("MeterDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var meterDetailId = currentDetailDataRow.Field<int>("MeterDetailId");
                                _customerMethods.MeterDetail_DeleteByMeterDetailId(meterDetailId);
                                _customerMethods.MeterDetail_Insert(createdByUserId, sourceId, meterId, detail.Key, detail.Value);
                            }
                        }
                    }

                    //Create Meter tables
                    var meterType = "Meter";
                    var schemaName = $"Supply.Meter{meterId}";

                    _supplyMethods.CreateMeterTables(schemaName, meterId, meterType);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

