﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private static readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private static readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private static readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private static readonly Enums.Customer.FlexContract.Attribute _customerFlexContractAttributeEnums = new Enums.Customer.FlexContract.Attribute();
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

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI, validateCrossSheetEntityDataAPIId, jsonObject))
                {
                    return;
                }

                var errorsFound = false;

                //Get data from required tables
                var customerDataRows = _tempCustomerMethods.Customer_GetByProcessQueueGUID(processQueueGUID);
                var siteDataRows = _tempCustomerMethods.Site_GetByProcessQueueGUID(processQueueGUID);
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var meterExemptionDataRows = _tempCustomerMethods.MeterExemption_GetByProcessQueueGUID(processQueueGUID);
                var meterUsageDataRows = _tempCustomerMethods.MeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var subMeterDataRows = _tempCustomerMethods.SubMeter_GetByProcessQueueGUID(processQueueGUID);
                var subMeterUsageDataRows = _tempCustomerMethods.SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var fixedContractDataRows = _tempCustomerMethods.FixedContract_GetByProcessQueueGUID(processQueueGUID);
                var flexContractDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);
                var flexReferenceVolumeDataRows = _tempCustomerMethods.FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);
                var flexTradeDataRows = _tempCustomerMethods.FlexTrade_GetByProcessQueueGUID(processQueueGUID);

                //Sites - If Customer Name is populated, check it exists and if not, check it is in the Customers table
                var customerNameCustomerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);
                var errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, siteDataRows, customerDataRows, customerNameCustomerAttributeId, "CustomerName", "Customer Name", _customerDataUploadValidationSheetNameEnums.Site);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meters - If Site Name is populated, check it exists and if not, check it is in the Sites table
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterDataRows, siteDataRows, siteNameSiteAttributeId, "SiteName", "Site Name", _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //SubMeters - If MPXN is populated, check it exists and if not, check it is in the Meters table
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, subMeterDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MeterIdentifier", "Meter Identifier", _customerDataUploadValidationSheetNameEnums.SubMeter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meter HH Data - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterUsageDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MeterIdentifier", "Meter Identifier", _customerDataUploadValidationSheetNameEnums.MeterUsage);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Meter Exemptions - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, meterExemptionDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MeterIdentifier", "Meter Identifier", _customerDataUploadValidationSheetNameEnums.MeterExemption);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //SubMeter HH Data - If SubMeter Identifier is populated, check it exists and if not, check it is in the SubMeters table
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, subMeterUsageDataRows, subMeterDataRows, subMeterIdentifierSubMeterAttributeId, "SubMeterIdentifier", "SubMeter Identifier", _customerDataUploadValidationSheetNameEnums.SubMeterUsage);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Fixed Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, fixedContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MeterIdentifier", "Meter Identifier", _customerDataUploadValidationSheetNameEnums.FixedContract);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MeterIdentifier", "Meter Identifier", _customerDataUploadValidationSheetNameEnums.FlexContract);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Reference Volumes - If Contract Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var contractReferenceFlexContractAttributeId = _customerMethods.FlexContractAttribute_GetFlexContractAttributeIdByFlexContractAttributeDescription(_customerFlexContractAttributeEnums.ContractReference);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexReferenceVolumeDataRows, flexContractDataRows, contractReferenceFlexContractAttributeId, "ContractReference", "Contract Reference", _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Trades - If Basket Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var basketReferenceFlexContractAttributeId = _customerMethods.FlexContractAttribute_GetFlexContractAttributeIdByFlexContractAttributeDescription(_customerFlexContractAttributeEnums.BasketReference);
                errorMessage = ValidateCrossEntities(createdByUserId, sourceId, processQueueGUID, flexTradeDataRows, flexContractDataRows, basketReferenceFlexContractAttributeId, "BasketReference", "Basket Reference", _customerDataUploadValidationSheetNameEnums.FlexTrade);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);                

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCrossSheetEntityDataAPIId, errorsFound, "Validation errors found");
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCrossSheetEntityDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private string ValidateCrossEntities(long createdByUserId, long sourceId, string processQueueGUID, IEnumerable<DataRow> mainDataRows, IEnumerable<DataRow> crossEntityDataRows, long attributeId, string columnName, string columnDisplayName, string sheetName)
        {
            var entities = new Dictionary<string, string>
                {
                    {columnName, columnDisplayName}
                };
            var mainCrossEntityDataRowsDataRows = mainDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(columnName)));
            var records = _tempCustomerMethods.InitialiseRecordsDictionary(mainCrossEntityDataRowsDataRows, entities);
            var newMainCrossEntityDataRows = mainCrossEntityDataRowsDataRows.Where(r => GetDetailId(attributeId, r.Field<string>(columnName), sheetName) == 0);
            var invalidMainCrossEntityDataRows = newMainCrossEntityDataRows.Where(r => !crossEntityDataRows.Any(cr => cr.Field<string>(columnName) == r.Field<string>(columnName)));

            foreach (var invalidMainCrossEntityDataRow in invalidMainCrossEntityDataRows)
            {
                var rowId = Convert.ToInt32(invalidMainCrossEntityDataRow["RowId"]);
                records[rowId][columnName].Add($"New {columnDisplayName} entered'{invalidMainCrossEntityDataRow[columnName]}' but not entered in Customers sheet");
            }

            return _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, sheetName);
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
            
            if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume
                || sheetName == _customerDataUploadValidationSheetNameEnums.FlexTrade)
            {
                return _customerMethods.FlexContractDetail_GetFlexContractDetailIdByFlexContractAttributeIdAndFlexContractDetailDescription(attributeId, detailDescription);
            }
            
            return 0;
        }
    }
}