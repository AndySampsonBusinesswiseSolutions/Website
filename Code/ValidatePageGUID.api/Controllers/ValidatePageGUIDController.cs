using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace ValidatePageGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePageGUIDController : ControllerBase
    {
        private readonly ILogger<ValidatePageGUIDController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Page _pageMethods = new CommonMethods.Page();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _apiGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.ValidatePageGUIDAPI, _systemAPIPasswordEnums.ValidatePageGUIDAPI);

        public ValidatePageGUIDController(ILogger<ValidatePageGUIDController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidatePageGUID/Validate")]
        public void Validate([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _apiGUIDEnums.ValidatePageGUIDAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _apiGUIDEnums.ValidatePageGUIDAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
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
                //Get Page GUID
                var pageGUID = jsonObject[_systemAPIRequiredDataKeyEnums.PageGUID].ToString();

                //Validate Page GUID
                var pageId = _pageMethods.PageId_GetByGUID(_databaseInteraction, pageGUID);

                //If pageId == 0 then the GUID provided isn't valid so create an error
                if(pageId == 0)
                {
                    //TODO: Add error handler
                }

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _apiGUIDEnums.ValidatePageGUIDAPI, pageId == 0);
            }
            else
            {
                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _apiGUIDEnums.ValidatePageGUIDAPI, true);
            }
        }
    }
}
