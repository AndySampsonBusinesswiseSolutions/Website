using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreCustomerDataController> _logger;
        private readonly Int64 storeCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreCustomerDataController(ILogger<StoreCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreCustomerDataAPI, password);
            storeCustomerDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI);
        }

        [HttpPost]
        [Route("StoreCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(storeCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreCustomerData/Store")]
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
                    storeCustomerDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI, storeCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeCustomerDataAPIId);

                var tempCustomerDataUploadCustomerMethods = new Methods.Temp.CustomerDataUpload.Customer();

                //Get Customer data from Customer Data Upload
                var customerDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Customers");

                foreach(var row in customerDictionary.Keys)
                {
                    var values = customerDictionary[row];

                    //Insert customer data into [Temp.CustomerDataUpload].[Customer]
                    //TODO: Make into BulkInsert
                    tempCustomerDataUploadCustomerMethods.Customer_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeCustomerDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}