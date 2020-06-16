using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
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
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.ValidatePasswordAPI, _systemAPIPasswordEnums.ValidatePasswordAPI);

        public ValidatePasswordController(ILogger<ValidatePasswordController> logger)
        {
            _logger = logger;
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
            _systemMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.ValidatePasswordAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _systemMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _systemAPIGUIDEnums.ValidatePasswordAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();//TODO: Make into common method
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);//TODO: Make into extension

            if(!erroredPrerequisiteAPIs.Any())
            {
                //Get Password
                var password = jsonObject[_systemAPIRequiredDataKeyEnums.Password].ToString();

                //Validate Password
                var passwordId = _administrationMethods.PasswordId_GetByPassword(_databaseInteraction, password);

                //If passwordId == 0 then the GUID provided isn't valid so create an error
                if(passwordId == 0)
                {
                    //TODO: Add error handler
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.ValidatePasswordAPI, passwordId == 0);
            }
            else
            {
                //Update Process Queue
                _systemMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.ValidatePasswordAPI, true);
            }
        }
    }
}
