using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace UploadFile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly ILogger<UploadFileController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Information.File.Attribute _informationFileAttributeEnums = new Enums.Information.File.Attribute();
        private readonly Int64 uploadFileAPIId;
        private readonly string hostEnvironment;

        public UploadFileController(ILogger<UploadFileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.UploadFileAPI, password);
            uploadFileAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.UploadFileAPI);
        }

        [HttpPost]
        [Route("UploadFile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(uploadFileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("UploadFile/Upload")]
        public void Upload([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
                    uploadFileAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.UploadFileAPI, uploadFileAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, uploadFileAPIId);

                //Insert FileGUID into Information.File
                var fileGUID = _systemMethods.GetFileGUIDFromJObject(jsonObject);
                _informationMethods.File_Insert(createdByUserId, sourceId, fileGUID);

                //Get FileId by FileGUID
                var fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Get FileName File Attribute Id
                var fileNameFileAttributeId = _informationMethods.FileAttribute_GetFileAttributeIdByFileAttributeDescription(_informationFileAttributeEnums.FileName);

                //Get ProcessQueueGUID File Attribute Id
                var processQueueGUIDFileAttributeId = _informationMethods.FileAttribute_GetFileAttributeIdByFileAttributeDescription(_informationFileAttributeEnums.ProcessQueueGUID);

                //Insert File Name into Information.FileDetail
                var fileName = _systemMethods.GetFileNameFromJObject(jsonObject);
                _informationMethods.FileDetail_Insert(createdByUserId, sourceId, fileId, fileNameFileAttributeId, fileName);

                //Insert Process Queue GUID into Information.FileDetail
                _informationMethods.FileDetail_Insert(createdByUserId, sourceId, fileId, processQueueGUIDFileAttributeId, processQueueGUID);

                //Insert File Content into Information.FileContent
                var fileContent = _systemMethods.GetFileContentFromJObject(jsonObject);
                _informationMethods.FileContent_Insert(createdByUserId,sourceId, fileId, fileContent);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, uploadFileAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, uploadFileAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}