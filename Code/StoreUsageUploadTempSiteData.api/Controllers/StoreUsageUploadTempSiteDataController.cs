﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StoreUsageUploadTempSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempSiteDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 storeUsageUploadTempSiteDataAPIId;

        public StoreUsageUploadTempSiteDataController(ILogger<StoreUsageUploadTempSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempSiteDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempSiteDataAPI);
            storeUsageUploadTempSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempSiteDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempSiteDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSiteData/Store")]
        public void Store([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeUsageUploadTempSiteDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadTempSiteDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSiteDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get File Content by FileId
                var fileGUID = jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
                var fileContent = _informationMethods.FileContent_GetFileContentByFileGUID(fileGUID);
                var fileJSON = JObject.Parse(fileContent);

                //Strip out data not related to Site
                var sheetJSON = fileJSON.Children().FirstOrDefault(c => c.Path == "Sheets");
                var sitesJSON = sheetJSON.Values().FirstOrDefault(v => v.Path == "Sheets.Sites");
                var cells = sitesJSON.Values().Children().Where(c => c.Path.Replace("Sheets.Sites.", "") != "!ref" 
                    && c.Path.Replace("Sheets.Sites.", "") != "!margins"
                    && !_methods.IsHeaderRow(c.Parent)).ToList();
                var cellDictionary = new Dictionary<int, List<string>>();

                foreach(var cell in cells)
                {
                    var row = _methods.GetRow(cell.Path);

                    if(!cellDictionary.ContainsKey(row))
                    {
                        cellDictionary.Add(row, new List<string>());
                    }

                    var valueToken = cell.Children().First(c => ((Newtonsoft.Json.Linq.JProperty)c).Name == "v");
                    var value = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)valueToken).Value).Value.ToString();
                    cellDictionary[row].Add(value);
                }

                foreach(var row in cellDictionary.Keys)
                {
                    var values = cellDictionary[row];

                    //Insert site data into [Temp.Customer].[Site]
                    _tempCustomerMethods.Site_Insert(processQueueGUID, values[0], values[1], values[2], values[3], values[4]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}