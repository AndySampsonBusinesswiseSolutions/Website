﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSiteDataController : ControllerBase
    {
        private readonly ILogger<StoreSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeSiteDataAPIId;

        public StoreSiteDataController(ILogger<StoreSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreSiteDataAPI, _systemAPIPasswordEnums.StoreSiteDataAPI);
            storeSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreSiteDataAPI);
        }

        [HttpPost]
        [Route("StoreSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSiteData/Store")]
        public void Store([FromBody] object data)
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
                    storeSiteDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreSiteDataAPI, storeSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Site data from Customer Data Upload
                var siteDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Sites");

                foreach(var row in siteDictionary.Keys)
                {
                    var values = siteDictionary[row];

                    //Insert site data into [Temp.CustomerDataUpload].[Site]
                    _tempCustomerMethods.Site_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}