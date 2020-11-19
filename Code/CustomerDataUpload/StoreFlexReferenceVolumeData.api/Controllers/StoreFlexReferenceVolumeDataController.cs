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
        private readonly Int64 storeFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexReferenceVolumeDataController(ILogger<StoreFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexReferenceVolumeDataAPI, password);
            storeFlexReferenceVolumeDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(storeFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/Store")]
        public void Store([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    storeFlexReferenceVolumeDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI, storeFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexReferenceVolumeMethods = new Methods.Temp.CustomerDataUpload.FlexReferenceVolume();

                //Get Flex Reference Volume data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var flexReferenceVolumeDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Reference Volumes']");

                foreach(var row in flexReferenceVolumeDictionary.Keys)
                {
                    var values = flexReferenceVolumeDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex reference volume data into [Temp.CustomerDataUpload].[FlexReferenceVolume]
                    tempCustomerDataUploadFlexReferenceVolumeMethods.FlexReferenceVolume_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}