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

namespace CommitProfiledUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitProfiledUsageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitProfiledUsageController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private readonly Enums.InformationSchema.UsageType _informationUsageTypeEnums = new Enums.InformationSchema.UsageType();
        private readonly Int64 commitProfiledUsageAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitProfiledUsageController(ILogger<CommitProfiledUsageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitProfiledUsageAPI, password);
            commitProfiledUsageAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitProfiledUsageAPI);
        }

        [HttpPost]
        [Route("CommitProfiledUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsyncAndDoNotAwaitResult(commitProfiledUsageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitProfiledUsage/Commit")]
        public void Commit([FromBody] object data)
        {

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
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
                    commitProfiledUsageAPIId);

                //Launch GetProfile process and wait for response
                var APIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileAPI);
                var result = _systemAPIMethods.PostAsJsonAsyncAndAwaitResult(APIId, _systemAPIGUIDEnums.CommitProfiledUsageAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitProfiledUsageAPIId);

                var profileString = JsonConvert.DeserializeObject(result).ToString();

                //No profile found so empty dictionary returned
                if (profileString == "{}")
                {
                    return;
                }

                var periodicUsageDictionary = new Methods().DeserializePeriodicUsage(profileString);

                if (!periodicUsageDictionary.Any())
                {
                    return;
                }

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId
                var meterId = GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString());

                //Insert new Periodic Usage into LoadedUsage tables
                var usageTypeId = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(_informationUsageTypeEnums.Profile);
                new Methods.Supply().InsertLoadedUsage(createdByUserId, sourceId, meterId, meterType, usageTypeId, periodicUsageDictionary);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitProfiledUsageAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitProfiledUsageAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var customerMethods = new Methods.Customer();
            var customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
            var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}