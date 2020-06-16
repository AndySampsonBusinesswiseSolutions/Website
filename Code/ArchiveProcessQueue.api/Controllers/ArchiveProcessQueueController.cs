using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;

namespace ArchiveProcessQueue.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ArchiveProcessQueueController : ControllerBase
    {
        private readonly ILogger<ArchiveProcessQueueController> _logger;
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly CommonEnums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new CommonEnums.System.ProcessArchive.Attribute();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.ArchiveProcessQueueAPI, _systemAPIPasswordEnums.ArchiveProcessQueueAPI);

        public ArchiveProcessQueueController(ILogger<ArchiveProcessQueueController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ArchiveProcessQueue/Archive")]
        public void Archive([FromBody] object data)
        {
            //Get API List
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        _systemMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject));
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync(); //TODO: Make into common method

            //All APIs have finished so create record in ProcessArchive
            _systemMethods.ProcessArchive_Insert(_databaseInteraction, queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated);
            var processArchiveId = _systemMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, queueGUID);

            //TODO Write records for each API into ProcessArchiveDetail

            //Write response into ProcessArchiveDetail
            _systemMethods.ProcessArchiveDetail_Insert(_databaseInteraction, queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemProcessArchiveAttributeEnums.Response, 
                "OK");

            //Update ProcessArchive
            _systemMethods.ProcessArchive_Update(_databaseInteraction, queueGUID);

            //TODO Delete GUID from ProcessQueue
        }
    }
}
