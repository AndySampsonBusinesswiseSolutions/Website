using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace StoreUsageUploadTempSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeUsageUploadTempSubMeterUsageDataAPIId;

        public StoreUsageUploadTempSubMeterUsageDataController(ILogger<StoreUsageUploadTempSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempSubMeterUsageDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempSubMeterUsageDataAPI);
            storeUsageUploadTempSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempSubMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSubMeterUsageData/Store")]
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
                    storeUsageUploadTempSubMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreUsageUploadTempSubMeterUsageDataAPI, storeUsageUploadTempSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get SubMeter Usage data from Customer Data Upload
                var subMeterUsageDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['SubMeter HH Data']");

                //TODO: Make into BulkInsert
                foreach(var row in subMeterUsageDictionary.Keys)
                {
                    var values = subMeterUsageDictionary[row];
                    var subMeterIdentifier = values[0];
                    var date = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));

                    for(var timePeriod = 2; timePeriod < values.Count(); timePeriod++)
                    {
                        var timePeriodString = _methods.ConvertIntegerToHalfHourTimePeriod(timePeriod);

                        //Insert submeter usage data into [Temp.Customer].[SubMeterUsage]
                        _tempCustomerMethods.SubMeterUsage_Insert(processQueueGUID, row, subMeterIdentifier, date, timePeriodString, values[timePeriod]);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

