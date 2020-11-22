using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterExemptionDataController> _logger;
        private readonly Int64 storeMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterExemptionDataController(ILogger<StoreMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterExemptionDataAPI, password);
            storeMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/Store")]
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
                    storeMeterExemptionDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI, storeMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterExemptionDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadMeterExemptionMethods = new Methods.TempSchema.CustomerDataUpload.MeterExemption();

                //Get Meter Exemption data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var meterExemptionDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Meter Exemptions']");

                foreach(var row in meterExemptionDictionary.Keys)
                {
                    var values = meterExemptionDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert meter exemption data into [Temp.CustomerDataUpload].[MeterExemption]
                    tempCustomerDataUploadMeterExemptionMethods.MeterExemption_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3], values[4]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}