using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ProcessCustomerDataUploadValidation.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ProcessCustomerDataUploadValidationController : ControllerBase
    {
        private readonly ILogger<ProcessCustomerDataUploadValidationController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 processCustomerDataUploadValidationAPIId;

        public ProcessCustomerDataUploadValidationController(ILogger<ProcessCustomerDataUploadValidationController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ProcessCustomerDataUploadValidationAPI, _systemAPIPasswordEnums.ProcessCustomerDataUploadValidationAPI);
            processCustomerDataUploadValidationAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI);
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(processCustomerDataUploadValidationAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/Process")]
        public void Process([FromBody] object data)
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
                    processCustomerDataUploadValidationAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ProcessCustomerDataUploadValidationAPI, processCustomerDataUploadValidationAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, processCustomerDataUploadValidationAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, processCustomerDataUploadValidationAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

