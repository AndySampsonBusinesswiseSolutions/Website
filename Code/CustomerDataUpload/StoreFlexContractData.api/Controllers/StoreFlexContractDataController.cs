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

namespace StoreFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexContractDataController : ControllerBase
    {
        private readonly ILogger<StoreFlexContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeFlexContractDataAPIId;
        private readonly string hostEnvironment;

        public StoreFlexContractDataController(ILogger<StoreFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.StoreFlexContractDataAPI, password);
            storeFlexContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFlexContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexContractData/Store")]
        public void Store([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
                    storeFlexContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFlexContractDataAPI, storeFlexContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexContractDataAPIId);

                //Get Flex Contract data from Customer Data Upload
                var flexContractDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Contracts']");
                var columns = new List<string>
                {
                    "StandingCharge", "ShapeFee", "AdminFee", "ImbalanceFee", "RiskFee", "GreenPremium", "OptimisationBenefit"
                };

                foreach(var row in flexContractDictionary.Keys)
                {
                    var values = flexContractDictionary[row];
                    var contractStartDate = _methods.GetDateTimeSqlParameterFromDateTimeString(values[4]);
                    var contractEndDate = _methods.GetDateTimeSqlParameterFromDateTimeString(values[5]);

                    for(var rateCount = 7; rateCount < values.Count(); rateCount++)
                    {
                        //Insert fixed contract data into [Temp.CustomerDataUpload].[FlexContract]
                        _tempCustomerDataUploadMethods.FlexContract_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], contractStartDate, contractEndDate, values[6], columns[rateCount - 7], values[rateCount]);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}