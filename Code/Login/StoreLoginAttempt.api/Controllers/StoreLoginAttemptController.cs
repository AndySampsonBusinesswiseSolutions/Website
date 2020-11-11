using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreLoginAttempt.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreLoginAttemptController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreLoginAttemptController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 storeLoginAttemptAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreLoginAttemptController(ILogger<StoreLoginAttemptController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreLoginAttemptAPI, password);
            storeLoginAttemptAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreLoginAttemptAPI);
        }

        [HttpPost]
        [Route("StoreLoginAttempt/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeLoginAttemptAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreLoginAttempt/Store")]
        public void Store([FromBody] object data)
        {
            var administrationLoginMethods = new Methods.Administration.Login();
            var administrationUserMethods = new Methods.Administration.User();
            var informationMethods = new Methods.Information();

            var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();                    

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeLoginAttemptAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreLoginAttemptAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());
                string errorMessage = erroredPrerequisiteAPIs.Any() ? $"Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeLoginAttemptAPIId);

                //Get User Id
                var userId = administrationUserMethods.GetUserIdByEmailAddress(jsonObject);

                if(userId != 0)
                {
                    //Store login attempt
                    administrationLoginMethods.Login_Insert(userId, sourceId, !erroredPrerequisiteAPIs.Any(), processQueueGUID);

                    //Get Login Id
                    var loginId = administrationLoginMethods.Login_GetLoginIdByProcessArchiveGUID(processQueueGUID);

                    //Store mapping between login attempt and user
                    var systemUserId = administrationUserMethods.GetSystemUserId();

                    var mappingMethods = new Methods.Mapping();
                    mappingMethods.LoginToUser_Insert(systemUserId, 
                        sourceId, 
                        loginId, 
                        userId);
                }
                
                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeLoginAttemptAPIId, erroredPrerequisiteAPIs.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeLoginAttemptAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}