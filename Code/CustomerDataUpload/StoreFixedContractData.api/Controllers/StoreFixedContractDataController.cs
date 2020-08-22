﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StoreFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFixedContractDataController : ControllerBase
    {
        private readonly ILogger<StoreFixedContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeFixedContractDataAPIId;

        public StoreFixedContractDataController(ILogger<StoreFixedContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreFixedContractDataAPI, _systemAPIPasswordEnums.StoreFixedContractDataAPI);
            storeFixedContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFixedContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeFixedContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFixedContractData/Store")]
        public void Store([FromBody] object data)
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
                    storeFixedContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFixedContractDataAPI, storeFixedContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Fixed Contract data from Customer Data Upload
                var fixedContractDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Fixed Contracts']");
                var columns = new List<string>
                {
                    "StandingCharge", "CapacityCharge", "Rate1", "Rate2", "Rate3", "Rate4", "Rate5", "Rate6", "Rate7", "Rate8", "Rate9", "Rate10"
                };

                foreach(var row in fixedContractDictionary.Keys)
                {
                    var values = fixedContractDictionary[row];
                    var contractStartDate = _methods.GetDateTimeSqlParameterFromDateTimeString(values[3]);
                    var contractEndDate = _methods.GetDateTimeSqlParameterFromDateTimeString(values[4]);

                    for(var rateCount = 7; rateCount < values.Count(); rateCount++)
                    {
                        //Insert fixed contract data into [Temp.CustomerDataUpload].[FlexContract]
                        _tempCustomerDataUploadMethods.FixedContract_Insert(processQueueGUID, row, values[0], values[1], values[2], contractStartDate, contractEndDate, values[5], values[6], columns[rateCount - 7], values[rateCount]);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

