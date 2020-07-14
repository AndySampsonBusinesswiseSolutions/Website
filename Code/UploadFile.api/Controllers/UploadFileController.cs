using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace UploadFile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly ILogger<UploadFileController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.File.Attribute _informationFileAttributeEnums = new Enums.Information.File.Attribute();
        private readonly Int64 uploadFileAPIId;

        public UploadFileController(ILogger<UploadFileController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.UploadFileAPI, _systemAPIPasswordEnums.UploadFileAPI);
            uploadFileAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.UploadFileAPI);
        }

        [HttpPost]
        [Route("UploadFile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(uploadFileAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("UploadFile/Upload")]
        public void Upload([FromBody] object data)
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
                    uploadFileAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.UploadFileAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, uploadFileAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Insert FileGUID into Information.File
                var fileGUID = jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
                _informationMethods.File_Insert(createdByUserId, sourceId, fileGUID);

                //Get FileId by FileGUID
                var fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Get FileName File Attribute Id
                var fileNameFileAttributeId = _informationMethods.FileAttribute_GetFileAttributeIdByFileAttributeDescription(_informationFileAttributeEnums.FileName);

                //Insert File Name into Information.FileDetail
                var fileName = jsonObject[_systemAPIRequiredDataKeyEnums.FileName].ToString();
                _informationMethods.FileDetail_Insert(createdByUserId, sourceId, fileId, fileNameFileAttributeId, fileName);

                //Insert File Content into Information.FileContent
                var fileContent = jsonObject[_systemAPIRequiredDataKeyEnums.FileContent].ToString();
                _informationMethods.FileContent_Insert(createdByUserId,sourceId, fileId, fileContent);

                //Get CustomerId by CustomerGUID
                var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Insert Customer To File Mapping
                _mappingMethods.CustomerToFile_Insert(createdByUserId, sourceId, customerId, fileId);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, uploadFileAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, uploadFileAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}