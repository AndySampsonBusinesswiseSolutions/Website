using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace CleanUpCustomerDataUploadTempData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CleanUpCustomerDataUploadTempDataController : ControllerBase
    {
        private readonly ILogger<CleanUpCustomerDataUploadTempDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 cleanUpCustomerDataUploadTempDataAPIId;

        public CleanUpCustomerDataUploadTempDataController(ILogger<CleanUpCustomerDataUploadTempDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CleanUpCustomerDataUploadTempDataAPI, _systemAPIPasswordEnums.CleanUpCustomerDataUploadTempDataAPI);
            cleanUpCustomerDataUploadTempDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CleanUpCustomerDataUploadTempDataAPI);
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(cleanUpCustomerDataUploadTempDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/Clean")]
        public void Clean([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    cleanUpCustomerDataUploadTempDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CleanUpCustomerDataUploadTempDataAPI, cleanUpCustomerDataUploadTempDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId);

                //Cleanup temp data
                _tempCustomerDataUploadMethods.Customer_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.FixedContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.FlexContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.FlexReferenceVolume_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.FlexTrade_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.Meter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.MeterExemption_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.MeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.Site_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.SubMeter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                _tempCustomerDataUploadMethods.SubMeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}