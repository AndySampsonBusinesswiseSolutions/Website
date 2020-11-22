﻿using Microsoft.AspNetCore.Mvc;
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
        #region Variables
        private readonly ILogger<CleanUpCustomerDataUploadTempDataController> _logger;
        private readonly Int64 cleanUpCustomerDataUploadTempDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CleanUpCustomerDataUploadTempDataController(ILogger<CleanUpCustomerDataUploadTempDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CleanUpCustomerDataUploadTempDataAPI, password);
            cleanUpCustomerDataUploadTempDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CleanUpCustomerDataUploadTempDataAPI);
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(cleanUpCustomerDataUploadTempDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/Clean")]
        public void Clean([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

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
                    cleanUpCustomerDataUploadTempDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CleanUpCustomerDataUploadTempDataAPI, cleanUpCustomerDataUploadTempDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                var deleteMethodList = new List<Action>
                {
                    () => new Methods.TempSchema.CustomerDataUpload.MeterUsage().MeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.SubMeterUsage().SubMeterUsage_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.Customer().Customer_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.FixedContract().FixedContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.FlexContract().FlexContract_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.FlexTrade().FlexTrade_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.Meter().Meter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.MeterExemption().MeterExemption_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.Site().Site_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID),
                    () => new Methods.TempSchema.CustomerDataUpload.SubMeter().SubMeter_DeleteByProcessQueueGUID(customerDataUploadProcessQueueGUID)
                };

                //Cleanup temp data
                Parallel.ForEach(deleteMethodList, new ParallelOptions{MaxDegreeOfParallelism = 5}, deleteMethod => {
                    deleteMethod();
                });

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, cleanUpCustomerDataUploadTempDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}