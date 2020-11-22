using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UploadFile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        #region Variables
        private readonly ILogger<UploadFileController> _logger;
        private readonly Int64 uploadFileAPIId;
        private readonly string hostEnvironment;
        #endregion

        public UploadFileController(ILogger<UploadFileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().UploadFileAPI, password);
            uploadFileAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().UploadFileAPI);
        }

        [HttpPost]
        [Route("UploadFile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(uploadFileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("UploadFile/Upload")]
        public void Upload([FromBody] object data)
        {
            var informationMethods = new Methods.InformationSchema();
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    uploadFileAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().UploadFileAPI, uploadFileAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, uploadFileAPIId);

                var informationFileAttributeEnums = new Enums.InformationSchema.File.Attribute();

                //Insert FileGUID into Information.File
                var fileGUID = systemMethods.GetFileGUIDFromJObject(jsonObject);
                informationMethods.File_Insert(createdByUserId, sourceId, fileGUID);

                //Get FileId by FileGUID
                var fileId = informationMethods.File_GetFileIdByFileGUID(fileGUID);

                Parallel.ForEach(new List<int>{1, 2, 3}, process => {
                    if(process == 1)
                    {
                        //Get FileName File Attribute Id
                        var fileNameFileAttributeId = informationMethods.FileAttribute_GetFileAttributeIdByFileAttributeDescription(informationFileAttributeEnums.FileName);

                        //Insert File Name into Information.FileDetail
                        var fileName = systemMethods.GetFileNameFromJObject(jsonObject);
                        informationMethods.FileDetail_Insert(createdByUserId, sourceId, fileId, fileNameFileAttributeId, fileName);
                    }
                    else if(process == 2)
                    {
                        //Get ProcessQueueGUID File Attribute Id
                        var processQueueGUIDFileAttributeId = informationMethods.FileAttribute_GetFileAttributeIdByFileAttributeDescription(informationFileAttributeEnums.ProcessQueueGUID);

                        //Insert Process Queue GUID into Information.FileDetail
                        informationMethods.FileDetail_Insert(createdByUserId, sourceId, fileId, processQueueGUIDFileAttributeId, processQueueGUID);
                    }
                    else {
                        //Insert File Content into Information.FileContent
                        var fileContent = systemMethods.GetFileContentFromJObject(jsonObject);
                        informationMethods.FileContent_Insert(createdByUserId,sourceId, fileId, fileContent);
                    }
                });
                
                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, uploadFileAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, uploadFileAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}