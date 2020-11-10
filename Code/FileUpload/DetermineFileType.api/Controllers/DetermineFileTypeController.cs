using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DetermineFileType.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DetermineFileTypeController : ControllerBase
    {
        #region Variables
        private readonly ILogger<DetermineFileTypeController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 determineFileTypeAPIId;
        private readonly string hostEnvironment;
        private Int64 fileId;
        #endregion

        public DetermineFileTypeController(ILogger<DetermineFileTypeController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().DetermineFileTypeAPI, password);
            determineFileTypeAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.DetermineFileTypeAPI);
        }

        [HttpPost]
        [Route("DetermineFileType/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(determineFileTypeAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DetermineFileType/Determine")]
        public void Determine([FromBody] object data)
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
                    determineFileTypeAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.DetermineFileTypeAPI, determineFileTypeAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, determineFileTypeAPIId);

                //Get FileId by FileGUID
                var fileGUID = _systemMethods.GetFileGUIDFromJObject(jsonObject);
                fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Check if FileType has been passed through
                var fileType = _systemMethods.GetFileTypeFromJObject(jsonObject);
                var fileTypeId = 0L;

                if(!string.IsNullOrWhiteSpace(fileType))
                {
                    //FileType was passed through so get the FileTypeId
                    fileTypeId = _informationMethods.FileType_GetFileTypeIdByFileTypeDescription(fileType);

                    if(fileTypeId == 0)
                    {
                        //An invalid FileType was passed through so error
                        _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"Invalid FileType {fileType} provided for FileId {fileId}");
                        return;
                    }
                }
                else
                {
                    //TODO: Read file to determine what type it is
                    if(fileTypeId == 0)
                    {
                        //A FileType could not be determined so error
                        _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"Unable to determine FileType for FileId {fileId}");
                        return;
                    }
                }

                //Insert File To FileType Mapping
                _mappingMethods.FileToFileType_Insert(createdByUserId, sourceId, fileId, fileTypeId);

                //Get ProcessGUID related to FileType
                var processId = _mappingMethods.FileTypeToProcess_GetProcessIdByFileTypeId(fileTypeId);
                var processGUID = _systemMethods.Process_GetProcessGUIDByProcessId(processId);

                //Add ProcessGUID into jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ProcessGUID, processGUID);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Link ProcessQueueGUIDs
                _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);

                //Update ProcessQueueGUID in jsonObject
                _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Get Routing.API URL
                var routingAPIId = _systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemAPIMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.DetermineFileTypeAPI, hostEnvironment, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, determineFileTypeAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}