using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSiteDataController> _logger;
        private readonly Int64 storeSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSiteDataController(ILogger<StoreSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSiteDataAPI, password);
            storeSiteDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSiteDataAPI);
        }

        [HttpPost]
        [Route("StoreSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(storeSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSiteData/Store")]
        public void Store([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeSiteDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreSiteDataAPI, storeSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSiteDataAPIId);

                var tempCustomerDataUploadSiteMethods = new Methods.Temp.CustomerDataUpload.Site();

                //Get Site data from Customer Data Upload
                //TODO: Make into Builk Insert
                var siteDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Sites");

                foreach(var row in siteDictionary.Keys)
                {
                    var values = siteDictionary[row];

                    //Insert site data into [Temp.CustomerDataUpload].[Site]
                    tempCustomerDataUploadSiteMethods.Site_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}