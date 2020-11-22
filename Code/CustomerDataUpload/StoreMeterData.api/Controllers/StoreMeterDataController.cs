using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterDataController> _logger;
        private readonly Int64 storeMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterDataController(ILogger<StoreMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterDataAPI, password);
            storeMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterData/Store")]
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
                    storeMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreMeterDataAPI, storeMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterDataAPIId);

                var tempCustomerDataUploadMeterMethods = new Methods.TempSchema.CustomerDataUpload.Meter();

                //Get Meter data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var meterDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Meters");

                foreach(var row in meterDictionary.Keys)
                {
                    var values = meterDictionary[row];

                    //Insert meter data into [Temp.CustomerDataUpload].[Meter]
                    tempCustomerDataUploadMeterMethods.Meter_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}