using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using MethodLibrary;
using enums;
using System;
using System.Net.Http;

namespace ArchiveProcessQueue.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ArchiveProcessQueueController : ControllerBase
    {
        private readonly ILogger<ArchiveProcessQueueController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();
        private readonly Enums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new Enums.System.ProcessArchive.Attribute();

        public ArchiveProcessQueueController(ILogger<ArchiveProcessQueueController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ArchiveProcessQueueAPI, _systemAPIPasswordEnums.ArchiveProcessQueueAPI);
        }

        [HttpPost]
        [Route("ArchiveProcessQueue/Archive")]
        public void Archive([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Process Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Get Process GUID
            var processGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID].ToString();

            try
            {
                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ArchiveProcessQueueAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();           

                //Get whether there is an error in the API records
                var hasError = _systemMethods.ProcessQueue_GetHasErrorByProcessQueueGUID(processQueueGUID);

                //If there is an error, check to see if it's a system error
                var hasSystemError = hasError && _systemMethods.ProcessQueue_GetHasSystemErrorByProcessQueueGUID(processQueueGUID);

                //Create record in ProcessArchive
                _systemMethods.ProcessArchive_Insert(createdByUserId,
                    sourceId,
                    processQueueGUID,
                    hasError);

                var processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                var processArchiveAttributeId = _systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(_systemProcessArchiveAttributeEnums.Response);

                //Write record into ProcessToProcessArchive mapping table
                var processId = _systemMethods.Process_GetProcessIdByProcessGUID(processGUID);

                if(processId != 0)
                {
                    _mappingMethods.ProcessToProcessArchive_Insert(createdByUserId, sourceId, processId, processArchiveId);
                }

                //TODO Write records for each API into ProcessArchiveDetail

                //Write response into ProcessArchiveDetail
                _systemMethods.ProcessArchiveDetail_Insert(createdByUserId,
                    sourceId,
                    processArchiveId,
                    processArchiveAttributeId,
                    hasSystemError ? "SYSTEM ERROR" : hasError ? "ERROR" : "OK");

                //Update ProcessArchive
                _systemMethods.ProcessArchive_Update(processQueueGUID);

                //Delete GUID from ProcessQueue
                _systemMethods.ProcessQueue_Delete(processQueueGUID);
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }
        }
    }
}
