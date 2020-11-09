using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitSubMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitSubMeterDataAPIId;
        private readonly string hostEnvironment;

        public CommitSubMeterDataController(ILogger<CommitSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CommitSubMeterDataAPI, password);
            commitSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubMeterData/Commit")]
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
                    commitSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubMeterDataAPI, commitSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, false, null);
                    return;
                }

                //For each column, get SubMeterAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {_customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier), _customerDataUploadValidationEntityEnums.SubMeterIdentifier},
                    {_customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SerialNumber), _customerDataUploadValidationEntityEnums.SerialNumber},
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

                    //Get SubMeterId by MPXN
                    var subMeterId = _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value);

                    if(subMeterId == 0)
                    {
                        subMeterId = _customerMethods.InsertNewSubMeter(createdByUserId, sourceId);

                        //Insert into [Customer].[SubMeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            _customerMethods.SubMeterDetail_Insert(createdByUserId, sourceId, subMeterId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[SubMeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailDataRow = _customerMethods.SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId(subMeterId, detail.Key);
                            var currentDetail = currentDetailDataRow.Field<string>("SubMeterDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var subMeterDetailId = currentDetailDataRow.Field<int>("SubMeterDetailId");
                                _customerMethods.SubMeterDetail_DeleteBySubMeterDetailId(subMeterDetailId);
                                _customerMethods.SubMeterDetail_Insert(createdByUserId, sourceId, subMeterId, detail.Key, detail.Value);
                            }
                        }
                    }

                    //Create SubMeter tables
                    var meterType = "SubMeter";
                    var schemaName = $"Supply.SubMeter{subMeterId}";

                    _supplyMethods.CreateMeterTables(schemaName, subMeterId, meterType);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}