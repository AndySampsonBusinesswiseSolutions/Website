using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace ValidateEmailAddress.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.EmailAddress _emailAddressMethods = new CommonMethods.EmailAddress();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidateEmailAddress.api", @"}h8FfD2r[Rd~PPNR");

        public ValidateEmailAddressController(ILogger<ValidateEmailAddressController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidateEmailAddress/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "99681B37-575F-47E5-95E3-608063EA513E");

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add("CallingGUID", "99681B37-575F-47E5-95E3-608063EA513E");
            
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
                //Get Email Address
                var emailAddress = jsonObject["EmailAddress"].ToString();

                //Validate Email Address
                var emailAddressId = _emailAddressMethods.UserDetailId_GetByEmailAddress(_databaseInteraction, emailAddress);

                //If emailAddressId == 0 then the GUID provided isn't valid so create an error
                if(emailAddressId == 0)
                {
                    
                }

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "99681B37-575F-47E5-95E3-608063EA513E", emailAddressId == 0);
            }
            else
            {
                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "99681B37-575F-47E5-95E3-608063EA513E", true);
            }
        }
    }
}
