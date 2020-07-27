using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreUsageUploadTempFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeUsageUploadTempFlexReferenceVolumeDataAPIId;

        public StoreUsageUploadTempFlexReferenceVolumeDataController(ILogger<StoreUsageUploadTempFlexReferenceVolumeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempFlexReferenceVolumeDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempFlexReferenceVolumeDataAPI);
            storeUsageUploadTempFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempFlexReferenceVolumeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempFlexReferenceVolumeData/Store")]
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
                    storeUsageUploadTempFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreUsageUploadTempFlexReferenceVolumeDataAPI, storeUsageUploadTempFlexReferenceVolumeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Flex Reference Volume data from Customer Data Upload
                var flexReferenceVolumeDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Reference Volumes']");

                foreach(var row in flexReferenceVolumeDictionary.Keys)
                {
                    var values = flexReferenceVolumeDictionary[row];
                    var dateFrom = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));
                    var dateTo = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[2])));

                    //Insert flex reference volume data into [Temp.Customer].[FlexReferenceVolume]
                    _tempCustomerMethods.FlexReferenceVolume_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

