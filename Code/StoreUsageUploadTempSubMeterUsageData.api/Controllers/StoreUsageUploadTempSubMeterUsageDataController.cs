using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Text;

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
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
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
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempSubMeterUsageDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSubMeterUsageData/Store")]
        public void Store([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeUsageUploadTempSubMeterUsageDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadTempSubMeterUsageDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get SubMeter Usage data from Customer Data Upload
                var subMeterUsageDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "SubMeter HH Data");

                //TODO: Make into BulkInsert
                foreach(var row in subMeterUsageDictionary.Keys)
                {
                    var values = subMeterUsageDictionary[row];
                    var subMeterIdentifier = values[0];
                    var date = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));

                    for(var timePeriod = 2; timePeriod < values.Count(); timePeriod++)
                    {
                        var time = DateTime.Today.AddMinutes(30 * (timePeriod - 1));
                        var timePeriodString = $"{time.Hour.ToString().PadLeft(2, '0')}:{time.Minute.ToString().PadLeft(2,'0')}";

                        //Insert submeter usage data into [Temp.Customer].[SubMeterUsage]
                        _tempCustomerMethods.SubMeterUsage_Insert(processQueueGUID, subMeterIdentifier, date, timePeriodString, values[timePeriod]);
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

