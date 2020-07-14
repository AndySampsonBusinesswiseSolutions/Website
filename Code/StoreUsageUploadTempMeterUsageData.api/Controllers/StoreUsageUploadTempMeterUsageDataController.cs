using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace StoreUsageUploadTempMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 storeUsageUploadTempMeterUsageDataAPIId;

        public StoreUsageUploadTempMeterUsageDataController(ILogger<StoreUsageUploadTempMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempMeterUsageDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempMeterUsageDataAPI);
            storeUsageUploadTempMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempMeterUsageDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempMeterUsageData/Store")]
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
                    storeUsageUploadTempMeterUsageDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadTempMeterUsageDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempMeterUsageDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //TODO: API Logic

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

