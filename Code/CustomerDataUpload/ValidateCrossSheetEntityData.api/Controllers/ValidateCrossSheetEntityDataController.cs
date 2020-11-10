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
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ValidateCrossSheetEntityData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCrossSheetEntityDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateCrossSheetEntityDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private static readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private static readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private static readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private static readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private static readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 validateCrossSheetEntityDataAPIId;
        private readonly string hostEnvironment;
        private Int64 createdByUserId;
        private Int64 sourceId;
        private string processQueueGUID;
        #endregion

        public ValidateCrossSheetEntityDataController(ILogger<ValidateCrossSheetEntityDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidateCrossSheetEntityDataAPI, password);
            validateCrossSheetEntityDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI);
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateCrossSheetEntityDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            createdByUserId = administrationUserMethods.GetSystemUserId();
            sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    validateCrossSheetEntityDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API to wait until prerequisite APIs have finished
                var API = _systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId);

                var errorsFound = false;

                //TODO: Change to entities
                var getMethodList = new Dictionary<string, Func<List<DataRow>>>
                {
                    {"Customer", () => new Methods.Temp.CustomerDataUpload.Customer().Customer_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"Site", () => new Methods.Temp.CustomerDataUpload.Site().Site_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"Meter", () => new Methods.Temp.CustomerDataUpload.Meter().Meter_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"MeterExemption", () => new Methods.Temp.CustomerDataUpload.MeterExemption().MeterExemption_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"MeterUsage", () => new Methods.Temp.CustomerDataUpload.MeterUsage().MeterUsage_GetByProcessQueueGUID(processQueueGUID)},
                    {"SubMeter", () => new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"SubMeterUsage", () => new Methods.Temp.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FixedContract", () => new Methods.Temp.CustomerDataUpload.FixedContract().FixedContract_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexContract", () => new Methods.Temp.CustomerDataUpload.FlexContract().FlexContract_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexReferenceVolume", () => new Methods.Temp.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexTrade", () => new Methods.Temp.CustomerDataUpload.FlexTrade().FlexTrade_GetDataRowsByProcessQueueGUID(processQueueGUID)}
                };

                var dataRowdictionary = new ConcurrentDictionary<string, List<DataRow>>();
                Parallel.ForEach(getMethodList, new ParallelOptions{MaxDegreeOfParallelism = 5}, getMethod => {
                    dataRowdictionary.TryAdd(getMethod.Key, getMethod.Value());
                });

                //Get data from required tables
                var customerDataRows = dataRowdictionary["Customer"];
                var siteDataRows = dataRowdictionary["Site"];
                var meterDataRows = dataRowdictionary["Meter"];
                var meterExemptionDataRows = dataRowdictionary["MeterExemption"];
                var meterUsageDataRows = dataRowdictionary["MeterUsage"];
                var subMeterDataRows = dataRowdictionary["SubMeter"];
                var subMeterUsageDataRows = dataRowdictionary["SubMeterUsage"];
                var fixedContractDataRows = dataRowdictionary["FixedContract"];
                var flexContractDataRows = dataRowdictionary["FlexContract"];
                var flexReferenceVolumeDataRows = dataRowdictionary["FlexReferenceVolume"];
                var flexTradeDataRows = dataRowdictionary["FlexTrade"];

                //Sites - If Customer Name is populated, check it exists and if not, check it is in the Customers table
                var customerNameCustomerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);
                var errorMessage = ValidateCrossEntities(siteDataRows, customerDataRows, customerNameCustomerAttributeId, "CustomerName", "Customer Name", _customerDataUploadValidationSheetNameEnums.Site, _customerDataUploadValidationSheetNameEnums.Customer);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meters - If Site Name is populated, check it exists and if not, check it is in the Sites table
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                errorMessage = ValidateCrossEntities(meterDataRows, siteDataRows, siteNameSiteAttributeId, "SiteName", "Site Name", _customerDataUploadValidationSheetNameEnums.Meter, _customerDataUploadValidationSheetNameEnums.Site);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //SubMeters - If MPXN is populated, check it exists and if not, check it is in the Meters table
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                errorMessage = ValidateCrossEntities(subMeterDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.SubMeter, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);

                //Meter HH Data - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(meterUsageDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN", _customerDataUploadValidationSheetNameEnums.MeterUsage, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Meter Exemptions - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(meterExemptionDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.MeterExemption, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //SubMeter HH Data - If SubMeter Identifier is populated, check it exists and if not, check it is in the SubMeters table
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                errorMessage = ValidateCrossEntities(subMeterUsageDataRows, subMeterDataRows, subMeterIdentifierSubMeterAttributeId, "SubMeterIdentifier", "SubMeter Identifier", _customerDataUploadValidationSheetNameEnums.SubMeterUsage, _customerDataUploadValidationSheetNameEnums.SubMeterUsage);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Fixed Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(fixedContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.FixedContract, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                errorMessage = ValidateCrossEntities(flexContractDataRows, meterDataRows, meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", _customerDataUploadValidationSheetNameEnums.FlexContract, _customerDataUploadValidationSheetNameEnums.Meter);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Reference Volumes - If Contract Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                errorMessage = ValidateCrossEntities(flexReferenceVolumeDataRows, flexContractDataRows, contractReferenceContractAttributeId, "ContractReference", "Contract Reference", _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume, _customerDataUploadValidationSheetNameEnums.FlexContract);
                errorsFound = errorsFound || !string.IsNullOrWhiteSpace(errorMessage);
                
                //Flex Trades - If Basket Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                var basketReferenceBasketAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                errorMessage = ValidateCrossEntities(flexTradeDataRows, flexContractDataRows, basketReferenceBasketAttributeId, "BasketReference", "Basket Reference", _customerDataUploadValidationSheetNameEnums.FlexTrade, _customerDataUploadValidationSheetNameEnums.FlexContract);
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

        private string ValidateCrossEntities(List<DataRow> mainDataRows, List<DataRow> crossEntityDataRows, long attributeId, string columnName, string columnDisplayName, string sheetName, string otherSheetName)
        {
            var mainCrossEntityDataRowsDataRows = mainDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(columnName))).ToList();
            var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(mainCrossEntityDataRowsDataRows.Select(d => Convert.ToInt32(d["RowId"].ToString())).Distinct().ToList(), new List<string>{columnName});
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