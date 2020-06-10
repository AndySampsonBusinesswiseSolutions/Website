using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace ValidateEmailAddressPasswordMapping.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressPasswordMappingController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressPasswordMappingController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.UserDetail _userDetailMethods = new CommonMethods.UserDetail();
        private readonly CommonMethods.EmailAddress _emailAddressMethods = new CommonMethods.EmailAddress();
        private readonly CommonMethods.Password _passwordMethods = new CommonMethods.Password();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidateEmailAddressPasswordMapping.api", @"GQzD2!aZNvffr*zC");

        public ValidateEmailAddressPasswordMappingController(ILogger<ValidateEmailAddressPasswordMappingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "CEC56745-C1C5-4E67-805B-159A8A5E991D");

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add("CallingGUID", "CEC56745-C1C5-4E67-805B-159A8A5E991D");
            
            //Call CheckPrerequisiteAPI API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            if(!erroredPrerequisiteAPIs.Any())
            {
                //Get Password Id
                var password = jsonObject["Password"].ToString();
                var passwordId = _passwordMethods.PasswordId_GetByPassword(_databaseInteraction, password);

                //Get User Id
                var emailAddress = jsonObject["EmailAddress"].ToString();
                var userDetailId = _emailAddressMethods.UserDetailId_GetByEmailAddress(_databaseInteraction, emailAddress);
                var userId = _userDetailMethods.UserId_GetByUserDetailId(_databaseInteraction, userDetailId);

                //Validate Password and User combination
                var mappingId = _mappingMethods.PasswordToUser_GetByPasswordIdAndUserId(_databaseInteraction, passwordId, userId);

                //If passwordId == 0 then the GUID provided isn't valid so create an error
                if(mappingId == 0)
                {
                    
                }

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "CEC56745-C1C5-4E67-805B-159A8A5E991D", mappingId == 0);
            }
            else
            {
                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "CEC56745-C1C5-4E67-805B-159A8A5E991D", true);
            }
        }
    }
}
