using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.IO;

namespace StoreUsageUpload.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.Folder.RootFolderType _informationFolderRootFolderTypeEnums = new Enums.Information.Folder.RootFolderType();
        private readonly Enums.Information.Folder.Attribute _informationFolderAttributeEnums = new Enums.Information.Folder.Attribute();
        private readonly Enums.Information.Folder.ExtensionType _informationFolderExtensionTypeEnums = new Enums.Information.Folder.ExtensionType();
        private readonly Int64 storeUsageUploadAPIId;

        public StoreUsageUploadController(ILogger<StoreUsageUploadController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadAPI, _systemAPIPasswordEnums.StoreUsageUploadAPI);
            storeUsageUploadAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadAPI);
        }

        [HttpPost]
        [Route("StoreUsageUpload/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUpload/Store")]
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
                    storeUsageUploadAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get xlsx JSON
                var xlsxFile = jsonObject[_systemAPIRequiredDataKeyEnums.XLSXFile].ToString();

                //Get Customer GUID
                var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();

                //Get Root Folder Type Id of Customer Files
                var rootFolderTypeId = _informationMethods.RootFolderType_GetRootFolderIdByRootFolderTypeDescription(_informationFolderRootFolderTypeEnums.CustomerFiles);

                //Get Root Folder Folder Ids
                var rootFolderIdList = _mappingMethods.FolderToRootFolderType_GetFolderIdListByRootFolderTypeId(rootFolderTypeId);

                //Get Folder Path Attribute Id
                var folderPathAttributeId = _informationMethods.FolderAttribute_GetFolderAttributeIdByFolderAttributeDescription(_informationFolderAttributeEnums.FolderPath);

                //Get Usage Upload Folder Extension Type Id
                var folderExtensionTypeId = _informationMethods.FolderExtensionType_GetFolderExtensionTypeIdByFolderExtensionTypeDescription(_informationFolderExtensionTypeEnums.UsageUpload);

                //Get Folder Extension Id List
                var folderIdList = _mappingMethods.FolderToFolderExtensionType_GetFolderIdListByFolderExtensionTypeId(folderExtensionTypeId);

                //Get Root Folder Descriptions
                foreach(var folderId in rootFolderIdList)
                {
                    //Get Customer Files folder
                    var rootFolderDescription = _informationMethods.FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(folderId, folderPathAttributeId);
                    var customerFilesRootFolder = Path.Combine(rootFolderDescription, customerGUID);

                    //Get linked folder extensions
                    var folderExtensionIdList = _mappingMethods.FolderToFolderExtension_GetFolderExtensionIdByFolderId(folderId);

                    //Get linked Usage Upload folder extension
                    var usageUploadFolderExtensionId = folderExtensionIdList.Intersect(folderIdList).First();

                    //Get Customer Files UsageUpload folder
                    var usageUploadFolderDescription = _informationMethods.FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(usageUploadFolderExtensionId, folderPathAttributeId);
                    var customerFilesUsageUploadFolder = Path.Combine(customerFilesRootFolder, usageUploadFolderDescription);

                    //Save to folder
                    System.IO.File.WriteAllText($@"{customerFilesUsageUploadFolder}\test.json", xlsxFile);
                }            

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
