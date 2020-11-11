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
        #region Variables
        private readonly ILogger<CommitSubMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSubMeterDataController(ILogger<CommitSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubMeterDataAPI, password);
            commitSubMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubMeterDataAPI, commitSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var tempCustomerDataUploadSubMeterMethods = new Methods.Temp.CustomerDataUpload.SubMeter();
                var subMeterEntities = tempCustomerDataUploadSubMeterMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
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

                foreach(var subMeterEntity in commitableSubMeterEntities)
                {
                    foreach(var attribute in attributes)
                    {
                        detailDictionary[attribute.Key] = subMeterEntity.GetType().GetProperty(attribute.Value).GetValue(subMeterEntity).ToString();
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
                            var currentDetailEntity = _customerMethods.SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId(subMeterId, detail.Key);
                            var currentDetail = currentDetailEntity.Field<string>("SubMeterDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var subMeterDetailId = currentDetailEntity.Field<int>("SubMeterDetailId");
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