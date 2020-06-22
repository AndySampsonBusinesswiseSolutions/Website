using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ValidateEmailAddressPasswordMapping.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressPasswordMappingController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressPasswordMappingController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();

        public ValidateEmailAddressPasswordMappingController(ILogger<ValidateEmailAddressPasswordMappingController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateEmailAddressPasswordMappingAPI, _systemAPIPasswordEnums.ValidateEmailAddressPasswordMappingAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/Validate")]
        public void Validate([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();
            var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI);

            _systemMethods.ProcessQueue_Insert(
                queueGUID, 
                createdByUserId,
                sourceId,
                APIId);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

            //Call CheckPrerequisiteAPI API
            var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI, jsonObject);
            var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
            var erroredPrerequisiteAPIs = _methods.GetAPIArray(result.Result.ToString());

            if(!erroredPrerequisiteAPIs.Any())
            {
                //Get Password Id
                var password = jsonObject[_systemAPIRequiredDataKeyEnums.Password].ToString();
                var passwordId = _administrationMethods.Password_GetPasswordIdByPassword(password);

                //Get User Id
                var userId = _administrationMethods.GetUserIdByEmailAddress(jsonObject);

                //Validate Password and User combination
                var mappingId = _mappingMethods.PasswordToUser_GetPasswordToUserIdByPasswordIdAndUserId(passwordId, userId);

                //If passwordId == 0 then the GUID provided isn't valid so create an error
                if(mappingId == 0)
                {
                    //TODO: Add error handler
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, mappingId == 0);
            }
            else
            {
                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, true);
            }
        }
    }
}
