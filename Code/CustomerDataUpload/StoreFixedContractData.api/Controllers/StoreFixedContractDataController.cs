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

namespace StoreFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFixedContractDataController> _logger;
        private readonly Int64 storeFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFixedContractDataController(ILogger<StoreFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFixedContractDataAPI, password);
            storeFixedContractDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(storeFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFixedContractData/Store")]
        public void Store([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    storeFixedContractDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI, storeFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFixedContractDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFixedContractMethods = new Methods.Temp.CustomerDataUpload.FixedContract();

                //Get Fixed Contract data from Customer Data Upload
                var fixedContractDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Fixed Contracts']");
                var columns = new List<string>
                {
                    "StandingCharge", "CapacityCharge", "Rate1", "Rate2", "Rate3", "Rate4", "Rate5", "Rate6", "Rate7", "Rate8", "Rate9", "Rate10"
                };

                foreach(var row in fixedContractDictionary.Keys)
                {
                    var values = fixedContractDictionary[row];
                    var contractStartDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[3]);
                    var contractEndDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[4]);

                    for(var rateCount = 7; rateCount < values.Count(); rateCount++)
                    {
                        //Insert fixed contract data into [Temp.CustomerDataUpload].[FlexContract]
                        tempCustomerDataUploadFixedContractMethods.FixedContract_Insert(processQueueGUID, row, values[0], values[1], values[2], contractStartDate, contractEndDate, values[5], values[6], columns[rateCount - 7], values[rateCount]);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}