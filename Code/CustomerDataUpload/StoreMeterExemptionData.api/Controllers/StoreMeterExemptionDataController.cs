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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 storeMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterExemptionDataController(ILogger<StoreMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterExemptionDataAPI, password);
            storeMeterExemptionDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/Store")]
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
                    storeMeterExemptionDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreMeterExemptionDataAPI, storeMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterExemptionDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadMeterExemptionMethods = new Methods.Temp.CustomerDataUpload.MeterExemption();

                //Get Meter Exemption data from Customer Data Upload
                var meterExemptionDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Meter Exemptions']");

                foreach(var row in meterExemptionDictionary.Keys)
                {
                    var values = meterExemptionDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert meter exemption data into [Temp.CustomerDataUpload].[MeterExemption]
                    tempCustomerDataUploadMeterExemptionMethods.MeterExemption_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3], values[4]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}