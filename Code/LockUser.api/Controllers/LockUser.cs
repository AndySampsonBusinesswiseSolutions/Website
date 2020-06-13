﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace LockUser.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class LockUserController : ControllerBase
    {
        private readonly ILogger<LockUserController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.UserDetail _userDetailMethods = new CommonMethods.UserDetail();
        private readonly CommonMethods.Password _passwordMethods = new CommonMethods.Password();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private readonly CommonEnums.System.API.Attribute _systemAPIAttributeEnums = new CommonEnums.System.API.Attribute();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.LockUserAPI, _systemAPIPasswordEnums.LockUserAPI);

        public LockUserController(ILogger<LockUserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("LockUser/Lock")]
        public void Lock([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.LockUserAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _systemAPIGUIDEnums.LockUserAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync(); //TODO: Make into common method
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries); //TODO: Make into extension

            if(erroredPrerequisiteAPIs.Any()) //TODO: Add try/catch for system error
            {
                //Get User Id
                var emailAddress = jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
                var userDetailId = _userDetailMethods.UserDetailId_GetByEmailAddress(_databaseInteraction, emailAddress);
                var userId = _userDetailMethods.UserId_GetByUserDetailId(_databaseInteraction, userDetailId);

                //Get logins by user id and order by descending
                var loginList = _mappingMethods.Login_GetByUserId(_databaseInteraction, userId).OrderByDescending(l => l);

                //Initialise invalidAttempts variable
                var invalidAttempts = 0;

                //Loop through each login
                foreach(var login in loginList)
                {
                    //Get LoginSuccessful attribute
                    var loginSucessful = _administrationMethods.LoginSuccessful_GetByLoginId(_databaseInteraction, login);

                    //If login is successful, then exit loop
                    //Else increment invalidAttempts
                    if(loginSucessful)
                    {
                        break;
                    }
                    else
                    {
                        invalidAttempts++;
                    }
                }

                //Get maximum attempts allowed before locking
                var maximumInvalidAttempts = _apiMethods.GetAPIDetailByAPIGUID(_databaseInteraction, 
                    _systemAPIGUIDEnums.LockUserAPI, 
                    _systemAPIAttributeEnums.MaximumInvalidLoginAttempts)
                    .Select(a => Convert.ToInt64(a))
                    .First();

                var isAttemptValid = invalidAttempts < maximumInvalidAttempts;

                //If invalid attempt count >= maximum allowed invalid attempts, lock the user
                if(!isAttemptValid)
                {
                    //Lock account by adding 'Account Locked' to user detail
                    _userDetailMethods.UserDetail_Insert(_databaseInteraction, 
                        _administrationUserGUIDEnums.System, 
                        _informationSourceTypeEnums.UserGenerated, 
                        "Account Locked", 
                        "true");
                }

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.LockUserAPI, !isAttemptValid);
            }
            else
            {
                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.LockUserAPI, false);
            }
        }
    }
}
