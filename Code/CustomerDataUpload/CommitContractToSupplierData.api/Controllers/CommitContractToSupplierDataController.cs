using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace CommitContractToSupplierData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitContractToSupplierDataController : ControllerBase
    {
        private readonly ILogger<CommitContractToSupplierDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supplier _supplierMethods = new Methods.Supplier();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private static readonly Enums.Supplier.Attribute _supplierAttributeEnums = new Enums.Supplier.Attribute();
        private readonly Int64 commitContractToSupplierDataAPIId;

        public CommitContractToSupplierDataController(ILogger<CommitContractToSupplierDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitContractToSupplierDataAPI, _systemAPIPasswordEnums.CommitContractToSupplierDataAPI);
            commitContractToSupplierDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractToSupplierDataAPI);
        }

        [HttpPost]
        [Route("CommitContractToSupplierData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitContractToSupplierDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitContractToSupplierData/Commit")]
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
                    commitContractToSupplierDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitContractToSupplierDataAPI, commitContractToSupplierDataAPIId, jsonObject))
                {
                    return;
                }

                var dataRowList = (IEnumerable<DataRow>) JsonConvert.DeserializeObject(jsonObject[_systemAPIRequiredDataKeyEnums.ContractData].ToString(), typeof(List<DataRow>));

                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);

                foreach(var dataRow in dataRowList)
                {
                    //Get ContractId from [Customer].[ContractDetail] by ContractReference
                    var contractReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference);
                    var contractId = _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference).First();

                    //Get SupplierId from [Supplier].[SupplierDetail] by Supplier
                    var supplier = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Supplier);
                    var supplierNameAttributeId = _supplierMethods.SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(_supplierAttributeEnums.SupplierName);
                    var supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(supplierNameAttributeId, supplier);

                    //Insert into [Mapping].[ContractToSupplier]
                    _mappingMethods.ContractToSupplier_Insert(createdByUserId, sourceId, contractId, supplierId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractToSupplierDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractToSupplierDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

