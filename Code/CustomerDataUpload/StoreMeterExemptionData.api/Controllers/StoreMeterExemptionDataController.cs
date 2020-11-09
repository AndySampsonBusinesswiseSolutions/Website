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
        private readonly ILogger<StoreMeterExemptionDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeMeterExemptionDataAPIId;
        private readonly string hostEnvironment;

        public StoreMeterExemptionDataController(ILogger<StoreMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.StoreMeterExemptionDataAPI, password);
            storeMeterExemptionDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreMeterExemptionDataAPI, storeMeterExemptionDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterExemptionDataAPIId);

                //Get Meter Exemption data from Customer Data Upload
                var meterExemptionDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Meter Exemptions']");

                foreach(var row in meterExemptionDictionary.Keys)
                {
                    var values = meterExemptionDictionary[row];
                    var dateFrom = _methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = _methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert meter exemption data into [Temp.CustomerDataUpload].[MeterExemption]
                    _tempCustomerDataUploadMethods.MeterExemption_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3], values[4]);
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