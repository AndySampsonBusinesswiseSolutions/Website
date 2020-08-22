﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSubMeterDataController : ControllerBase
    {
        private readonly ILogger<StoreSubMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeSubMeterDataAPIId;

        public StoreSubMeterDataController(ILogger<StoreSubMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreSubMeterDataAPI, _systemAPIPasswordEnums.StoreSubMeterDataAPI);
            storeSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreSubMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeSubMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterData/Store")]
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
                    storeSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreSubMeterDataAPI, storeSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get SubMeter data from Customer Data Upload
                var subMeterDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.SubMeters");

                foreach(var row in subMeterDictionary.Keys)
                {
                    var values = subMeterDictionary[row];

                    //Insert submeter data into [Temp.CustomerDataUpload].[SubMeter]
                    _tempCustomerDataUploadMethods.SubMeter_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

