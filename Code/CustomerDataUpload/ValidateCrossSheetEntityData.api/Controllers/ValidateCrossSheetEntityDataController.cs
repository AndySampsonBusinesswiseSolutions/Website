using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace ValidateCrossSheetEntityData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCrossSheetEntityDataController : ControllerBase
    {
        private readonly ILogger<ValidateCrossSheetEntityDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private static readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private static readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private static readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private static readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private static readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 validateCrossSheetEntityDataAPIId;

        public ValidateCrossSheetEntityDataController(ILogger<ValidateCrossSheetEntityDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateCrossSheetEntityDataAPI, _systemAPIPasswordEnums.ValidateCrossSheetEntityDataAPI);
            validateCrossSheetEntityDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI);
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateCrossSheetEntityDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/Validate")]
        public void Validate([FromBody] object data)
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
                    validateCrossSheetEntityDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API to wait until prerequisite APIs have finished
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId);

                var errorsFound = false;

                //Get data from required tables
                var customerDataRows = _tempCustomerDataUploadMethods.Customer_GetByProcessQueueGUID(processQueueGUID);
                var siteDataRows = _tempCustomerDataUploadMethods.Site_GetByProcessQueueGUID(processQueueGUID);
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var meterExemptionDataRows = _tempCustomerDataUploadMethods.MeterExemption_GetByProcessQueueGUID(processQueueGUID);
                var meterUsageDataRows = _tempCustomerDataUploadMethods.MeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(processQueueGUID);
                var subMeterUsageDataRows = _tempCustomerDataUploadMethods.SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var fixedContractDataRows = _tempCustomerDataUploadMethods.FixedContract_GetByProcessQueueGUID(processQueueGUID);
                var flexContractDataRows = _tempCustomerDataUploadMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);
                var flexReferenceVolumeDataRows = _tempCustomerDataUploadMethods.FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);
                var flexTradeDataRows = _tempCustomerDataUploadMethods.FlexTrade_GetByProcessQueueGUID(processQueueGUID);

                //Sites - If Customer Name is populated, check it exists and if not, check it is in the Customers table
                var customerNameCustomerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);
                var errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, siteDataRows, customerDataRows, customerNameCustomerAttributeId, "CustomerName", "Customer Name", _customerDataUploadValidationSheetNameEnums.Site, _customerDataUploadValidationSheetNameEnums.Customer);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meters - If Site Name is populated, check it exists and if not, check it is in the Sites table
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterDataRows, siteDataRows, siteNameSiteAttributeId, "SiteName", "Site Name", _customerDataUploadValidationSheetNameEnums.Meter, _customerDataUploadValidationSheetNameEnums.Site);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //SubMeters - If MPXN is populated, check it exists and if not, check it is in the Meters table
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, subMeterDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.SubMeter, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meter HH Data - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterUsageDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN", _customerDataUploadValidationSheetNameEnums.MeterUsage, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Meter Exemptions - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterExemptionDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.MeterExemption, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //SubMeter HH Data - If SubMeter Identifier is populated, check it exists and if not, check it is in the SubMeters table
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, subMeterUsageDataRows, subMeterDataRows, subMeterIdentifierSubMeterAttributeId, "SubMeterIdentifier", "SubMeter Identifier", _customerDataUploadValidationSheetNameEnums.SubMeterUsage, _customerDataUploadValidationSheetNameEnums.SubMeterUsage);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Fixed Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, fixedContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.FixedContract, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.FlexContract, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Reference Volumes - If Contract Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexReferenceVolumeDataRows, flexContractDataRows, contractReferenceContractAttributeId, "ContractReference", "Contract Reference", _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume, _customerDataUploadValidationSheetNameEnums.FlexContract);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Trades - If Basket Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var basketReferenceBasketAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexTradeDataRows, flexContractDataRows, basketReferenceBasketAttributeId, "BasketReference", "Basket Reference", _customerDataUploadValidationSheetNameEnums.FlexTrade, _customerDataUploadValidationSheetNameEnums.FlexContract);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);                

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId, errorsFound, errorsFound ? "Validation errors found" : null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private string ValidateCrossEntities(long createdByUserId, long sourceId, string processQueueGUID, IEnumerable<DataRow> mainDataRows, IEnumerable<DataRow> crossEntityDataRows, long attributeId, string columnName, string columnDisplayName, string sheetName, string otherSheetName)
        {
            var entities = new Dictionary<string, string>
                {
                    {columnName, columnDisplayName}
                };
            var mainCrossEntityDataRowsDataRows = mainDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(columnName)));
            var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(mainCrossEntityDataRowsDataRows, entities);
            var newMainCrossEntityDataRows = mainCrossEntityDataRowsDataRows.Where(r => GetDetailId(attributeId, r.Field<string>(columnName), sheetName) == 0);
            var invalidMainCrossEntityDataRows = newMainCrossEntityDataRows.Where(r => !crossEntityDataRows.Any(cr => cr.Field<string>(columnName) == r.Field<string>(columnName)));

            foreach (var invalidMainCrossEntityDataRow in invalidMainCrossEntityDataRows)
            {
                var rowId = Convert.ToInt32(invalidMainCrossEntityDataRow["RowId"]);
                records[rowId][columnName].Add($"New {columnDisplayName} entered'{invalidMainCrossEntityDataRow[columnName]}' but not entered in {otherSheetName} sheet");
            }

            return _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, sheetName, false);
        }

        private long GetDetailId(long attributeId, string detailDescription, string sheetName)
        {
            if(sheetName == _customerDataUploadValidationSheetNameEnums.Site)
            {
                return _customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == _customerDataUploadValidationSheetNameEnums.Meter)
            {
                return _customerMethods.SiteDetail_GetSiteDetailIdBySiteAttributeIdAndSiteDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeter
                || sheetName == _customerDataUploadValidationSheetNameEnums.MeterUsage
                || sheetName == _customerDataUploadValidationSheetNameEnums.MeterExemption
                || sheetName == _customerDataUploadValidationSheetNameEnums.FixedContract
                || sheetName == _customerDataUploadValidationSheetNameEnums.FlexContract)
            {
                return _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(attributeId, detailDescription);
            }
            if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeterUsage)
            {
                return _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume)
            {
                return _customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(attributeId, detailDescription);
            }
            
            if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexTrade)
            {
                return _customerMethods.BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(attributeId, detailDescription);
            }
            
            return 0;
        }
    }
}