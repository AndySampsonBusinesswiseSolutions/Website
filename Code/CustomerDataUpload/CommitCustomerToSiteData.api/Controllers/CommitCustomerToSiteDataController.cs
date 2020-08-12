using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitCustomerToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerToSiteDataController : ControllerBase
    {
        private readonly ILogger<CommitCustomerToSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitCustomerToSiteDataAPIId;

        public CommitCustomerToSiteDataController(ILogger<CommitCustomerToSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitCustomerToSiteDataAPI, _systemAPIPasswordEnums.CommitCustomerToSiteDataAPI);
            commitCustomerToSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitCustomerToSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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
                    commitCustomerToSiteDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI, commitCustomerToSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1

                //Get CustomerId from [Customer].[CustomerDetail]
                //If CustomerId == 0
                //Throw error as customer should have been invalidated or inserted

                //Get SiteId from [Customer].[SiteDetail]
                //If SiteId == 0
                //Throw error as site should have been invalidated or inserted

                //If SiteId != 0 and CustomerId != 0
                //Insert into [Mapping].[CustomerToSite]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitCustomerToSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

