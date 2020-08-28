using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Data;
using System;
using System.Linq;

namespace CommitFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexContractDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitFlexContractDataAPIId;

        public CommitFlexContractDataController(ILogger<CommitFlexContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexContractDataAPI, _systemAPIPasswordEnums.CommitFlexContractDataAPI);
            commitFlexContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexContractData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFlexContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexContractDataAPI, commitFlexContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1
                var flexContractDataRows = _tempCustomerDataUploadMethods.FlexContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(flexContractDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, false, null);
                    return;
                }

                //Add ContractType to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractType, _informationContractTypeEnums.Flex);

                //Convert dataRows to a string
                var commitableDataRowJSON = string.Empty;
                foreach(var dataRow in commitableDataRows.Where(d => !string.IsNullOrWhiteSpace(d.Field<string>(_customerDataUploadValidationEntityEnums.Value))))
                {
                    commitableDataRowJSON += $"{string.Join('|' , dataRow.ItemArray)};;";
                }

                //Add ContractData to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractData, commitableDataRowJSON);

                //Call CommitContractData API and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitFlexContractDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

