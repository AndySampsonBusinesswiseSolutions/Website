﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterDataController : ControllerBase
    {
        private readonly ILogger<StoreMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeMeterDataAPIId;

        public StoreMeterDataController(ILogger<StoreMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreMeterDataAPI, _systemAPIPasswordEnums.StoreMeterDataAPI);
            storeMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterData/Store")]
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
                    storeMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreMeterDataAPI, storeMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Meter data from Customer Data Upload
                var meterDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Meters");

                foreach(var row in meterDictionary.Keys)
                {
                    var values = meterDictionary[row];

                    //Insert meter data into [Temp.CustomerDataUpload].[Meter]
                    _tempCustomerMethods.Meter_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}