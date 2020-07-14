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
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
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
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(determineFileTypeAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("DetermineFileType/Determine")]
        public void Determine([FromBody] object data)
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
                    determineFileTypeAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.DetermineFileTypeAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, determineFileTypeAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get FileId by FileGUID
                var fileGUID = jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
                var fileId = _informationMethods.File_GetFileIdByFileGUID(fileGUID);

                //Check if FileType has been passed through
                var fileType = jsonObject[_systemAPIRequiredDataKeyEnums.FileType].ToString();

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

