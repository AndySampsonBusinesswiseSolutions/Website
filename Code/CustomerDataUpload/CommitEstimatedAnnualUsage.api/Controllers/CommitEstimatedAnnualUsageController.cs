using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CommitEstimatedAnnualUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitEstimatedAnnualUsageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitEstimatedAnnualUsageController> _logger;
        private readonly Int64 commitEstimatedAnnualUsageAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitEstimatedAnnualUsageController(ILogger<CommitEstimatedAnnualUsageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitEstimatedAnnualUsageAPI, password);
            commitEstimatedAnnualUsageAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitEstimatedAnnualUsageAPI);
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitEstimatedAnnualUsageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var informationMethods = new Methods.InformationSchema();
            var systemAPIMethods = new Methods.SystemSchema.API();
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    commitEstimatedAnnualUsageAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI, commitEstimatedAnnualUsageAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId);

                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
                var supplyMethods = new Methods.SupplySchema();
                var customerMethods = new Methods.CustomerSchema();

                //Get mpxn
                var mpxn = jsonObject[systemAPIRequiredDataKeyEnums.MPXN].ToString();

                //Get MeterIdentifierMeterAttributeId
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);

                //Get MeterId
                var meterId = customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();

                //Get MeterType
                var meterType = jsonObject[systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get Estimated Annual Usage
                var estimatedAnnualUsage = Convert.ToDecimal(jsonObject[systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage]);

                //End date existing Estimated Annual Usage
                supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                //Insert new Estimated Annual Usage
                supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);

                //Get HasPeriodicUsage
                var hasPeriodicUsage = Convert.ToBoolean(jsonObject[systemAPIRequiredDataKeyEnums.HasPeriodicUsage]);

                //Since the entity has periodic usage, don't bother creating a profiled version of the estimated annual usage
                if(hasPeriodicUsage)
                {
                    //Update Process Queue
                   systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, false, null);

                    return;
                }

                //Launch GetProfile process and wait for response
                var APIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.GetProfileAPI);
                var API = systemAPIMethods.PostAsJsonAsync(APIId, systemAPIGUIDEnums.CommitProfiledUsageAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Get profile from Profiling API
                var profileString = JsonConvert.DeserializeObject(result.Result.ToString()).ToString();
                var periodicUsageDictionary = new Methods().DeserializePeriodicUsage(profileString);

                //Insert new Periodic Usage into LoadedUsage tables
                var usageTypeId = informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(new Enums.InformationSchema.UsageType().CustomerEstimated);
                new Methods.SupplySchema().InsertLoadedUsage(createdByUserId, sourceId, meterId, meterType, usageTypeId, periodicUsageDictionary);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Update Process GUID to Create Forecast Usage Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, new Enums.SystemSchema.Process.GUID().CreateForecastUsage);

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}