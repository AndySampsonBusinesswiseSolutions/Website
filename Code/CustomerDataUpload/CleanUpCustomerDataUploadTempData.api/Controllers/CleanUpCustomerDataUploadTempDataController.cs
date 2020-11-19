using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CleanUpCustomerDataUploadTempData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CleanUpCustomerDataUploadTempDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CleanUpCustomerDataUploadTempDataController> _logger;
        private readonly Entity.System.API.CleanUpCustomerDataUploadTempData.Configuration _configuration;
        #endregion

        public CleanUpCustomerDataUploadTempDataController(ILogger<CleanUpCustomerDataUploadTempDataController> logger, Entity.System.API.CleanUpCustomerDataUploadTempData.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(_configuration.APIId, _configuration.HostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/Clean")]
        public void Clean([FromBody] object data)
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
                    _configuration.APIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(_configuration.APIGUID, _configuration.APIId, _configuration.HostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, _configuration.APIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                var deleteMethodList = new List<Action>
                {
                    () => new Methods.Temp.CustomerDataUpload.MeterUsage().MeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.SubMeterUsage().SubMeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.Customer().Customer_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.FixedContract().FixedContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.FlexContract().FlexContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.FlexTrade().FlexTrade_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.Meter().Meter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.MeterExemption().MeterExemption_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.Site().Site_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID)
                };

                //Cleanup temp data
                Parallel.ForEach(deleteMethodList, new ParallelOptions{MaxDegreeOfParallelism = 5}, deleteMethod => {
                    deleteMethod();
                });

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}