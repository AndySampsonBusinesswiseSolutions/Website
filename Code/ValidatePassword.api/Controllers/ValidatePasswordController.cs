using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using commonMethods;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace ValidatePassword.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePasswordController : ControllerBase
    {
        private readonly ILogger<ValidatePasswordController> _logger;
        private readonly CommonMethods _methods = new CommonMethods();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private readonly CommonMethods.Information _informationMethods = new CommonMethods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();

        public ValidatePasswordController(ILogger<ValidatePasswordController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidatePasswordAPI, _systemAPIPasswordEnums.ValidatePasswordAPI);
        }

        [HttpPost]
        [Route("ValidatePassword/Validate")]
        public void Validate([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();
            var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidatePasswordAPI);

            _systemMethods.ProcessQueue_Insert(
                queueGUID, 
                createdByUserId,
                sourceId,
                APIId);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

            //Build JObject
            var apiData = _systemMethods.GetAPIData(checkPrerequisiteAPIAPIId, jsonObject, _systemAPIGUIDEnums.ValidatePasswordAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(checkPrerequisiteAPIAPIId), 
                        apiData);
            
            var result = processTask.GetAwaiter().GetResult().Content.ReadAsStringAsync();
            var erroredPrerequisiteAPIs = _methods.GetAPIArray(result.Result.ToString());

            if(!erroredPrerequisiteAPIs.Any())
            {
                //Get Password
                var password = jsonObject[_systemAPIRequiredDataKeyEnums.Password].ToString();

                //Validate Password
                var passwordId = _administrationMethods.Password_GetPasswordIdByPassword(password);

                //If passwordId == 0 then the GUID provided isn't valid so create an error
                if(passwordId == 0)
                {
                    //TODO: Add error handler
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, passwordId == 0);
            }
            else
            {
                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, true);
            }
        }
    }
}
