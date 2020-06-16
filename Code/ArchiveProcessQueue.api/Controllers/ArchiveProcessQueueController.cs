using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using commonMethods;
using enums;

namespace ArchiveProcessQueue.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ArchiveProcessQueueController : ControllerBase
    {
        private readonly ILogger<ArchiveProcessQueueController> _logger;
        private readonly CommonMethods _methods = new CommonMethods();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
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
            //Get API List
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(checkPrerequisiteAPIAPIId), 
                        _systemMethods.GetAPIData(checkPrerequisiteAPIAPIId, jsonObject));
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync(); //TODO: Make into common method

            //All APIs have finished so create record in ProcessArchive
            _systemMethods.ProcessArchive_Insert(queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated);
            var processArchiveId = _systemMethods.ProcessArchiveId_GetByGUID(queueGUID);

            //TODO Write records for each API into ProcessArchiveDetail

            //Write response into ProcessArchiveDetail
            _systemMethods.ProcessArchiveDetail_Insert(queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemProcessArchiveAttributeEnums.Response, 
                "OK");

            //Update ProcessArchive
            _systemMethods.ProcessArchive_Update(queueGUID);

            //TODO Delete GUID from ProcessQueue
        }
    }
}
