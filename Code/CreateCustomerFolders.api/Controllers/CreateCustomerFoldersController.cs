using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.IO;

namespace CreateCustomerFolders.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateCustomerFoldersController : ControllerBase
    {
        private readonly ILogger<CreateCustomerFoldersController> _logger;
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
        private readonly Int64 createCustomerFoldersAPIId;

        public CreateCustomerFoldersController(ILogger<CreateCustomerFoldersController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateCustomerFoldersAPI, _systemAPIPasswordEnums.CreateCustomerFoldersAPI);
            createCustomerFoldersAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateCustomerFoldersAPI);
        }

        [HttpPost]
        [Route("CreateCustomerFolders/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(createCustomerFoldersAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("CreateCustomerFolders/Create")]
        public void Create([FromBody] object data)
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
                    createCustomerFoldersAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.CreateCustomerFoldersAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, createCustomerFoldersAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get Customer GUID
                var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();

                //Get Root Folder Type Id of Customer Files
                var rootFolderTypeId = _informationMethods.RootFolderType_GetRootFolderIdByRootFolderTypeDescription(_informationFolderRootFolderTypeEnums.CustomerFiles);

                //Get Root Folder Folder Ids
                var rootFolderIdList = _mappingMethods.FolderToRootFolderType_GetFolderIdListByRootFolderTypeId(rootFolderTypeId);

                //Get Folder Path Attribute Id
                var folderPathAttributeId = _informationMethods.FolderAttribute_GetFolderAttributeIdByFolderAttributeDescription(_informationFolderAttributeEnums.FolderPath);

                //Get Root Folder Descriptions
                foreach(var folderId in rootFolderIdList)
                {
                    var rootFolderDescription = _informationMethods.FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(folderId, folderPathAttributeId);

                    //Check if Folder Description + Customer GUID exists on fileshare
                    var customerFilesRootFolder = Path.Combine(rootFolderDescription, customerGUID);
                    Directory.CreateDirectory(customerFilesRootFolder);

                    //Get linked folder extensions
                    var folderExtensionIdList = _mappingMethods.FolderToFolderExtension_GetFolderExtensionIdByFolderId(folderId);

                    foreach(var folderExtensionId in folderExtensionIdList)
                    {
                        //Check if folder Description + Customer GUID + Folder Extension exists on fileshare
                        var folderExtensionDescription = _informationMethods.FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(folderExtensionId, folderPathAttributeId);
                        var customerFilesExtensionFolder = Path.Combine(customerFilesRootFolder, folderExtensionDescription);
                        Directory.CreateDirectory(customerFilesExtensionFolder);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, createCustomerFoldersAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, createCustomerFoldersAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

