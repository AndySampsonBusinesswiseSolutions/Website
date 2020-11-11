using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFlexReferenceVolumeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 storeFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexReferenceVolumeDataController(ILogger<StoreFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexReferenceVolumeDataAPI, password);
            storeFlexReferenceVolumeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/Store")]
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
                    storeFlexReferenceVolumeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFlexReferenceVolumeDataAPI, storeFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexReferenceVolumeMethods = new Methods.Temp.CustomerDataUpload.FlexReferenceVolume();

                //Get Flex Reference Volume data from Customer Data Upload
                var flexReferenceVolumeDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Reference Volumes']");

                foreach(var row in flexReferenceVolumeDictionary.Keys)
                {
                    var values = flexReferenceVolumeDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex reference volume data into [Temp.CustomerDataUpload].[FlexReferenceVolume]
                    tempCustomerDataUploadFlexReferenceVolumeMethods.FlexReferenceVolume_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}