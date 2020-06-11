using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;

namespace ValidateProcessGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateProcessGUIDController : ControllerBase
    {
        private readonly ILogger<ValidateProcessGUIDController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.ValidateProcessGUIDAPI, _systemAPIPasswordEnums.ValidateProcessGUIDAPI);

        public ValidateProcessGUIDController(ILogger<ValidateProcessGUIDController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidateProcessGUID/Validate")]
        public long Validate([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.ValidateProcessGUIDAPI);

            //Get Process GUID
            var processGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID].ToString();

            //Validate Process GUID
            var processId = _processMethods.ProcessId_GetByGUID(_databaseInteraction, processGUID);

            //If processId == 0 then the GUID provided isn't valid so create an error
            if(processId == 0)
            {
                //TODO: Add error handler
            }

            //Update Process Queue
            _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.ValidateProcessGUIDAPI, processId == 0);

            return processId;
        }
    }
}
