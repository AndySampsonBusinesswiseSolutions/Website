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
        #region Variables
        private readonly ILogger<StoreFlexContractDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 storeFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexContractDataController(ILogger<StoreFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexContractDataAPI, password);
            storeFlexContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFlexContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFlexContractDataAPI, storeFlexContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexContractDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexContract = new Methods.Temp.CustomerDataUpload.FlexContract();

                //Get Flex Contract data from Customer Data Upload
                var flexContractDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Contracts']");
                var columns = new List<string>
                {
                    "StandingCharge", "ShapeFee", "AdminFee", "ImbalanceFee", "RiskFee", "GreenPremium", "OptimisationBenefit"
                };

                foreach(var row in flexContractDictionary.Keys)
                {
                    var values = flexContractDictionary[row];
                    var contractStartDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[4]);
                    var contractEndDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[5]);

                    for(var rateCount = 7; rateCount < values.Count(); rateCount++)
                    {
                        //Insert fixed contract data into [Temp.CustomerDataUpload].[FlexContract]
                        tempCustomerDataUploadFlexContract.FlexContract_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], contractStartDate, contractEndDate, values[6], columns[rateCount - 7], values[rateCount]);
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