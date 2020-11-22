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
        private readonly Int64 storeLoginAttemptAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreLoginAttemptController(ILogger<StoreLoginAttemptController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreLoginAttemptAPI, password);
            storeLoginAttemptAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreLoginAttemptAPI);
        }

        [HttpPost]
        [Route("StoreLoginAttempt/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeLoginAttemptAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreLoginAttempt/Store")]
        public void Store([FromBody] object data)
        {
            var administrationLoginMethods = new Methods.AdministrationSchema.Login();
            var administrationUserMethods = new Methods.AdministrationSchema.User();
            var informationMethods = new Methods.InformationSchema();
            var systemAPIMethods = new Methods.SystemSchema.API();
            var systemMethods = new Methods.SystemSchema();
            var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();                    

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeLoginAttemptAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, new Enums.SystemSchema.API.GUID().StoreLoginAttemptAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = new Methods().GetArray(result.Result.ToString());
                string errorMessage = erroredPrerequisiteAPIs.Any() ? $"Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeLoginAttemptAPIId);

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

                    var mappingMethods = new Methods.MappingSchema();
                    mappingMethods.LoginToUser_Insert(systemUserId, 
                        sourceId, 
                        loginId, 
                        userId);
                }
                
                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeLoginAttemptAPIId, erroredPrerequisiteAPIs.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeLoginAttemptAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}