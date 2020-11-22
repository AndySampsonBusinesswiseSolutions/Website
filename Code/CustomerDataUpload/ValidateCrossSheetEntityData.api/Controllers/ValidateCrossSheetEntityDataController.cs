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
        private readonly Methods.TempSchema.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();
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
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateCrossSheetEntityDataAPI, password);
            validateCrossSheetEntityDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateCrossSheetEntityDataAPI);
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateCrossSheetEntityDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();
            var systemAPIMethods = new Methods.SystemSchema.API();

            //Get base variables
            createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    validateCrossSheetEntityDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API to wait until prerequisite APIs have finished
                var API = systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, new Enums.SystemSchema.API.GUID().ValidateCrossSheetEntityDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId);

                var customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();

                //TODO: Change to entities
                var getMethodList = new Dictionary<string, Func<List<DataRow>>>
                {
                    {"Customer", () => new Methods.TempSchema.CustomerDataUpload.Customer().Customer_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"Site", () => new Methods.TempSchema.CustomerDataUpload.Site().Site_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"Meter", () => new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"MeterExemption", () => new Methods.TempSchema.CustomerDataUpload.MeterExemption().MeterExemption_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"MeterUsage", () => new Methods.TempSchema.CustomerDataUpload.MeterUsage().MeterUsage_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"SubMeter", () => new Methods.TempSchema.CustomerDataUpload.SubMeter().SubMeter_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"SubMeterUsage", () => new Methods.TempSchema.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FixedContract", () => new Methods.TempSchema.CustomerDataUpload.FixedContract().FixedContract_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexContract", () => new Methods.TempSchema.CustomerDataUpload.FlexContract().FlexContract_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexReferenceVolume", () => new Methods.TempSchema.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetDataRowsByProcessQueueGUID(processQueueGUID)},
                    {"FlexTrade", () => new Methods.TempSchema.CustomerDataUpload.FlexTrade().FlexTrade_GetDataRowsByProcessQueueGUID(processQueueGUID)}
                };

                var dataRowDictionary = new ConcurrentDictionary<string, List<DataRow>>();
                Parallel.ForEach(getMethodList, new ParallelOptions{MaxDegreeOfParallelism = 5}, getMethod => {
                    dataRowDictionary.TryAdd(getMethod.Key, getMethod.Value());
                });

                var customerMethods = new Methods.CustomerSchema();
                var customerNameCustomerAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(new Enums.CustomerSchema.Customer.Attribute().CustomerName);
                var siteNameSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(new Enums.CustomerSchema.Site.Attribute().SiteName);
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var contractReferenceContractAttributeId = customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(new Enums.CustomerSchema.Contract.Attribute().ContractReference);
                var basketReferenceBasketAttributeId = customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(new Enums.CustomerSchema.Basket.Attribute().BasketReference);

                var validationTupleList = new List<Tuple<List<DataRow>, List<DataRow>, long, string, string, string, string>>();
                validationTupleList.Add(Tuple.Create(dataRowDictionary["Site"], dataRowDictionary["Customer"], customerNameCustomerAttributeId, "CustomerName", "Customer Name", customerDataUploadValidationSheetNameEnums.Site, customerDataUploadValidationSheetNameEnums.Customer)); //Sites - If Customer Name is populated, check it exists and if not, check it is in the Customers table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["Meter"], dataRowDictionary["Site"], siteNameSiteAttributeId, "SiteName", "Site Name", customerDataUploadValidationSheetNameEnums.Meter, customerDataUploadValidationSheetNameEnums.Site)); //Meters - If Site Name is populated, check it exists and if not, check it is in the Sites table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["SubMeter"], dataRowDictionary["Meter"], meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", customerDataUploadValidationSheetNameEnums.SubMeter, customerDataUploadValidationSheetNameEnums.Meter)); //SubMeters - If MPXN is populated, check it exists and if not, check it is in the Meters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["MeterUsage"], dataRowDictionary["Meter"], meterIdentifierMeterAttributeId, "MPXN", "MPAN", customerDataUploadValidationSheetNameEnums.MeterUsage, customerDataUploadValidationSheetNameEnums.Meter)); //Meter HH Data - If MPXN is populated, check it exists and if not, check it is in the Meters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["MeterExemption"], dataRowDictionary["Meter"], meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", customerDataUploadValidationSheetNameEnums.MeterExemption, customerDataUploadValidationSheetNameEnums.Meter)); //Meter Exemptions - If MPXN is populated, check it exists and if not, check it is in the Meters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["SubMeterUsage"], dataRowDictionary["SubMeter"], subMeterIdentifierSubMeterAttributeId, "SubMeterIdentifier", "SubMeter Identifier", customerDataUploadValidationSheetNameEnums.SubMeterUsage, customerDataUploadValidationSheetNameEnums.SubMeterUsage)); //SubMeter HH Data - If SubMeter Identifier is populated, check it exists and if not, check it is in the SubMeters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["FixedContract"], dataRowDictionary["Meter"], meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", customerDataUploadValidationSheetNameEnums.FixedContract, customerDataUploadValidationSheetNameEnums.Meter)); //Fixed Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["FlexContract"], dataRowDictionary["Meter"], meterIdentifierMeterAttributeId, "MPXN", "MPAN/MPRN", customerDataUploadValidationSheetNameEnums.FlexContract, customerDataUploadValidationSheetNameEnums.Meter)); //Flex Contracts - If MPXN is populated, check it exists and if not, check it is in the Meters table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["FlexReferenceVolume"], dataRowDictionary["FlexContract"], contractReferenceContractAttributeId, "ContractReference", "Contract Reference", customerDataUploadValidationSheetNameEnums.FlexReferenceVolume, customerDataUploadValidationSheetNameEnums.FlexContract)); //Flex Reference Volumes - If Contract Reference is populated, check it exists and if not, check it is in the Flex Contracts table
                validationTupleList.Add(Tuple.Create(dataRowDictionary["FlexTrade"], dataRowDictionary["FlexContract"], basketReferenceBasketAttributeId, "BasketReference", "Basket Reference", customerDataUploadValidationSheetNameEnums.FlexTrade, customerDataUploadValidationSheetNameEnums.FlexContract)); //Flex Trades - If Basket Reference is populated, check it exists and if not, check it is in the Flex Contracts table

                var errorList = new ConcurrentBag<string>();
                Parallel.ForEach(validationTupleList, new ParallelOptions{MaxDegreeOfParallelism = 5}, vt => {
                    errorList.Add(ValidateCrossEntities(vt.Item1, vt.Item2, vt.Item3, vt.Item4, vt.Item5, vt.Item6, vt.Item7));
                });              

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId, errorList.Any(e => !string.IsNullOrWhiteSpace(e)), errorList.Any(e => !string.IsNullOrWhiteSpace(e)) ? string.Join("; ", errorList.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct()) : null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCrossSheetEntityDataAPIId, true, $"System Error Id {errorId}");
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
                records[rowId][columnName].Add($"New {columnDisplayName} entered '{invalidMainCrossEntityDataRow[columnName]}' but not entered in {otherSheetName} sheet");
            }

            return _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, sheetName, false);
        }

        private long GetDetailId(long attributeId, string detailDescription, string sheetName)
        {
            //TODO: This needs fixing as is badly written
            //TODO: Maybe move into Dictionary<string, Action>
            
            var customerMethods = new Methods.CustomerSchema();
            var customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
            if(sheetName == customerDataUploadValidationSheetNameEnums.Site)
            {
                return customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == customerDataUploadValidationSheetNameEnums.Meter)
            {
                return customerMethods.SiteDetail_GetSiteDetailIdBySiteAttributeIdAndSiteDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == customerDataUploadValidationSheetNameEnums.SubMeter
                || sheetName == customerDataUploadValidationSheetNameEnums.MeterUsage
                || sheetName == customerDataUploadValidationSheetNameEnums.MeterExemption
                || sheetName == customerDataUploadValidationSheetNameEnums.FixedContract
                || sheetName == customerDataUploadValidationSheetNameEnums.FlexContract)
            {
                return customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == customerDataUploadValidationSheetNameEnums.SubMeterUsage)
            {
                return customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(attributeId, detailDescription);
            }

            if(sheetName == customerDataUploadValidationSheetNameEnums.FlexReferenceVolume)
            {
                return customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(attributeId, detailDescription);
            }
            
            if(sheetName == customerDataUploadValidationSheetNameEnums.FlexTrade)
            {
                return customerMethods.BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(attributeId, detailDescription);
            }
            
            return 0;
        }
    }
}