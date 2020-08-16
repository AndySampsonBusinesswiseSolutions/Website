using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CommitFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFixedContractDataController : ControllerBase
    {
        private readonly ILogger<CommitFixedContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 commitFixedContractDataAPIId;

        public CommitFixedContractDataController(ILogger<CommitFixedContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFixedContractDataAPI, _systemAPIPasswordEnums.CommitFixedContractDataAPI);
            commitFixedContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFixedContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFixedContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFixedContractData/Commit")]
        public void Commit([FromBody] object data)
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
                    commitFixedContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFixedContractDataAPI, commitFixedContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FixedContract] where CanCommit = 1
                var customerDataRows = _tempCustomerMethods.FixedContract_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(customerDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitFixedContractDataAPIId, false, null);
                    return;
                }

                //Add ContractType to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractType, _informationContractTypeEnums.Fixed);

                //Add ContractData to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractData, JsonConvert.SerializeObject(commitableDataRows));

                //Call CommitContractData API and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitFixedContractDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Call CommitContractToSupplierData API
                APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractToSupplierDataAPI);
                API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitFixedContractDataAPI, jsonObject);

                //Call CommitContractMeterToProductData API
                APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractMeterToProductDataAPI);
                API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitFixedContractDataAPI, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

