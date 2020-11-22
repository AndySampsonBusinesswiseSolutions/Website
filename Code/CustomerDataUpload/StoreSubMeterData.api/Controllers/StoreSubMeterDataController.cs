using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSubMeterDataController> _logger;
        private readonly Int64 storeSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSubMeterDataController(ILogger<StoreSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSubMeterDataAPI, password);
            storeSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterData/Store")]
        public void Store([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

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
                    storeSubMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI, storeSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSubMeterDataAPIId);

                var tempCustomerDataUploadSubMeterMethods = new Methods.TempSchema.CustomerDataUpload.SubMeter();

                //Get SubMeter data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var subMeterDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.SubMeters");

                foreach(var row in subMeterDictionary.Keys)
                {
                    var values = subMeterDictionary[row];

                    //Insert submeter data into [Temp.CustomerDataUpload].[SubMeter]
                    tempCustomerDataUploadSubMeterMethods.SubMeter_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}