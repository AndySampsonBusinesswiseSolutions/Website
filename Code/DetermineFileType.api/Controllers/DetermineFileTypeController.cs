using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace DetermineFileType.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DetermineFileTypeController : ControllerBase
    {
        private readonly ILogger<DetermineFileTypeController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 determineFileTypeAPIId;

        public DetermineFileTypeController(ILogger<DetermineFileTypeController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.DetermineFileTypeAPI, _systemAPIPasswordEnums.DetermineFileTypeAPI);
            determineFileTypeAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.DetermineFileTypeAPI);
        }

        [HttpPost]
        [Route("DetermineFileType/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(determineFileTypeAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DetermineFileType/Determine")]
        public void Determine([FromBody] object data)
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
                    determineFileTypeAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.DetermineFileTypeAPI, determineFileTypeAPIId, jsonObject))
                {
                    return;
                }

                //Get FileId by FileGUID
                var fileGUID = _systemMethods.GetFileGUIDFromJObject(jsonObject);
                var fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Check if FileType has been passed through
                var fileType = _systemMethods.GetFileTypeFromJObject(jsonObject);

                if(!string.IsNullOrWhiteSpace(fileType))
                {
                    //FileType was passed through so get the FileTypeId
                    var fileTypeId = _informationMethods.FileType_GetFileTypeIdByFileTypeDescription(fileType);

                    if(fileTypeId == 0)
                    {
                        //An invalid FileType was passed through so error
                        _systemMethods.ProcessQueue_Update(processQueueGUID, determineFileTypeAPIId, true, $"Invalid FileType {fileType} provided for FileId {fileId}");
                        return;
                    }

                    //Insert File To FileType Mapping
                    _mappingMethods.FileToFileType_Insert(createdByUserId, sourceId, fileId, fileTypeId);
                }
                else
                {
                    //TODO: Read file to determine what type it is
                    if(0 == 0)
                    {
                        //A FileType could not be determined so error
                        _systemMethods.ProcessQueue_Update(processQueueGUID, determineFileTypeAPIId, true, $"Unable to determine FileType for FileId {fileId}");
                        return;
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, determineFileTypeAPIId, false, null);

                //TODO: Get ProcessGUID related to FileType

                //TODO: Call RoutingAPI for ProcessGUID
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, determineFileTypeAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

