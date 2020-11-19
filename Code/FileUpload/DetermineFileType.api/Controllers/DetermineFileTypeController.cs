using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace DetermineFileType.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DetermineFileTypeController : ControllerBase
    {
        #region Variables
        private readonly ILogger<DetermineFileTypeController> _logger;
        private readonly Int64 determineFileTypeAPIId;
        private readonly string hostEnvironment;
        private Int64 fileId;
        #endregion

        public DetermineFileTypeController(ILogger<DetermineFileTypeController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().DetermineFileTypeAPI, password);
            determineFileTypeAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().DetermineFileTypeAPI);
        }

        [HttpPost]
        [Route("DetermineFileType/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(determineFileTypeAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DetermineFileType/Determine")]
        public void Determine([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var informationMethods = new Methods.Information();
            var systemAPIMethods = new Methods.System.API();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
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
                    determineFileTypeAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.DetermineFileTypeAPI, determineFileTypeAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, determineFileTypeAPIId);

                //Get FileId by FileGUID
                var fileGUID = systemMethods.GetFileGUIDFromJObject(jsonObject);
                fileId = informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Check if FileType has been passed through
                var fileType = systemMethods.GetFileTypeFromJObject(jsonObject);
                var fileTypeId = 0L;

                if(!string.IsNullOrWhiteSpace(fileType))
                {
                    //FileType was passed through so get the FileTypeId
                    fileTypeId = informationMethods.FileType_GetFileTypeIdByFileTypeDescription(fileType);

                    if(fileTypeId == 0)
                    {
                        //An invalid FileType was passed through so error
                        systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"Invalid FileType {fileType} provided for FileId {fileId}");
                        return;
                    }
                }
                else
                {
                    //TODO: Read file to determine what type it is
                    if(fileTypeId == 0)
                    {
                        //A FileType could not be determined so error
                        systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"Unable to determine FileType for FileId {fileId}");
                        return;
                    }
                }

                var mappingMethods = new Methods.Mapping();

                //Insert File To FileType Mapping
                mappingMethods.FileToFileType_Insert(createdByUserId, sourceId, fileId, fileTypeId);

                //Get ProcessGUID related to FileType
                var processId = mappingMethods.FileTypeToProcess_GetProcessIdByFileTypeId(fileTypeId);
                var processGUID = systemMethods.Process_GetProcessGUIDByProcessId(processId);

                //Add ProcessGUID into jsonObject
                jsonObject.Add(new Enums.SystemSchema.API.RequiredDataKey().ProcessGUID, processGUID);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Link ProcessQueueGUIDs
                systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);

                //Update ProcessQueueGUID in jsonObject
                systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.DetermineFileTypeAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}