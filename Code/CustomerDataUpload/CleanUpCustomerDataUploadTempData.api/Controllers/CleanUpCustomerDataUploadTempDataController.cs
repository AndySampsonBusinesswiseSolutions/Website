using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

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
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 cleanUpCustomerDataUploadTempDataAPIId;
        private readonly string hostEnvironment;

        public CleanUpCustomerDataUploadTempDataController(ILogger<CleanUpCustomerDataUploadTempDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CleanUpCustomerDataUploadTempDataAPI, password);
            cleanUpCustomerDataUploadTempDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CleanUpCustomerDataUploadTempDataAPI);
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(cleanUpCustomerDataUploadTempDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                var processList = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};

                //Cleanup temp data
                Parallel.ForEach(processList, new ParallelOptions{MaxDegreeOfParallelism = 5}, process => {
                    if(process == 1)
                    {
                        _tempCustomerDataUploadMethods.MeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 2)
                    {
                        _tempCustomerDataUploadMethods.SubMeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 3)
                    {
                        _tempCustomerDataUploadMethods.Customer_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 4)
                    {
                        _tempCustomerDataUploadMethods.FixedContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 5)
                    {
                        _tempCustomerDataUploadMethods.FlexContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 6)
                    {
                        _tempCustomerDataUploadMethods.FlexReferenceVolume_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 7)
                    {
                        _tempCustomerDataUploadMethods.FlexTrade_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 8)
                    {
                        _tempCustomerDataUploadMethods.Meter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 9)
                    {
                        _tempCustomerDataUploadMethods.MeterExemption_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 10)
                    {
                        _tempCustomerDataUploadMethods.Site_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                    else if(process == 11)
                    {
                        _tempCustomerDataUploadMethods.SubMeter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                    }
                });

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