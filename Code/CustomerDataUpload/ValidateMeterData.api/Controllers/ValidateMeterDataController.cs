using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ValidateMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterDataController : ControllerBase
    {
        private readonly ILogger<ValidateMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Int64 validateMeterDataAPIId;

        public ValidateMeterDataController(ILogger<ValidateMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateMeterDataAPI, _systemAPIPasswordEnums.ValidateMeterDataAPI);
            validateMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterData/Validate")]
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
                    validateMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterDataAPI, validateMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Meter] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"AnnualUsage", "Annual Usage"}
                    };
                
                var records = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns);

                //Get MPANs
                var mpanDataRecords = customerDataRows.Where(r => _methods.IsValidMPAN(r.Field<string>("MPXN")));
                var mpanRows = mpanDataRecords.Select(r => r.Field<int>("RowId"));

                //Get MPRNs
                var mprnDataRecords = customerDataRows.Where(r => _methods.IsValidMPRN(r.Field<string>("MPXN")));
                var mprnRows = mprnDataRecords.Select(r => r.Field<int>("RowId"));

                //Get any MPXNs not in MPANs or MPRNs
                var invalidMPXNRows = customerDataRows.Select(r => r.Field<int>("RowId")).Except(mpanRows).Except(mprnRows);

                foreach(var row in invalidMPXNRows)
                {
                    var dataRow = customerDataRows.First(r => r.Field<int>("RowId") == row);
                    records[row]["MPXN"].Add($"Invalid MPAN/MPRN {dataRow["MPXN"]}");
                }

                //Get MPANs not stored in database
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var newMPANDataRecords = mpanDataRecords.Where(r => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, r.Field<string>("MPXN")) == 0);

                //Site, GSP, PC, MTC, LLFC and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"SiteName", "Site Name"},
                        {"GridSupplyPoint", "GSP"},
                        {"ProfileClass", "Profile Class"},
                        {"MeterTimeswitchClass", "MTC"},
                        {"LineLossFactorClass", "LLFC"},
                        {"Area", "Area"}
                    };
                var newMPANErrors =_tempCustomerMethods.GetMissingRecords(newMPANDataRecords, requiredColumns);
                _tempCustomerMethods.AddErrorsToRecords(records, newMPANErrors);

                //Validate GSP
                var invalidGridSupplyPointDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("GridSupplyPoint"))
                    && !_methods.IsValidGridSupplyPoint(r.Field<string>("GridSupplyPoint")));

                foreach(var invalidGridSupplyPointDataRecord in invalidGridSupplyPointDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidGridSupplyPointDataRecord["RowId"]);
                    records[rowId]["GridSupplyPoint"].Add($"Invalid GSP {invalidGridSupplyPointDataRecord["GridSupplyPoint"]}");
                }

                //Validate Profile Class
                var invalidProfileClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ProfileClass"))
                    && !_methods.IsValidProfileClass(r.Field<string>("ProfileClass")));

                foreach(var invalidProfileClassDataRecord in invalidProfileClassDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidProfileClassDataRecord["RowId"]);
                    records[rowId]["ProfileClass"].Add($"Invalid Profile Class {invalidProfileClassDataRecord["ProfileClass"]}");
                }

                //Validate MTC
                var invalidMeterTimeswitchClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("MeterTimeswitchClass"))
                    && !_methods.IsValidMeterTimeswitchClass(r.Field<string>("MeterTimeswitchClass")));

                foreach(var invalidMeterTimeswitchClassDataRecord in invalidMeterTimeswitchClassDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidMeterTimeswitchClassDataRecord["RowId"]);
                    records[rowId]["MeterTimeswitchClass"].Add($"Invalid MTC {invalidMeterTimeswitchClassDataRecord["MeterTimeswitchClass"]}");
                }

                //Validate LLFC
                var invalidLineLossFactorClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("LineLossFactorClass"))
                    && !_methods.IsValidLineLossFactorClass(r.Field<string>("LineLossFactorClass")));

                foreach(var invalidLineLossFactorClassDataRecord in invalidLineLossFactorClassDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidLineLossFactorClassDataRecord["RowId"]);
                    records[rowId]["LineLossFactorClass"].Add($"Invalid LLFC {invalidLineLossFactorClassDataRecord["LineLossFactorClass"]}");
                }

                //Validate Capacity if it is populated
                var invalidCapacityDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Capacity"))
                    && !_methods.IsValidCapacity(r.Field<string>("Capacity")));

                foreach(var invalidCapacityDataRecord in invalidCapacityDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidCapacityDataRecord["RowId"]);
                    records[rowId]["Capacity"].Add($"Invalid Capacity {invalidCapacityDataRecord["Capacity"]}");
                }

                //Get MPRNs not stored in database
                var newMPRNDataRecords = mprnDataRecords.Where(r => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, r.Field<string>("MPXN")) == 0);

                //Site, LDZ and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"SiteName", "Site Name"},
                        {"LocalDistributionZone", "LDZ"},
                        {"Area", "Area"}
                    };
                var newMPRNErrors =_tempCustomerMethods.GetMissingRecords(newMPRNDataRecords, requiredColumns);
                _tempCustomerMethods.AddErrorsToRecords(records, newMPRNErrors);

                //Validate LDZ
                var invalidLocalDistributionZoneDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("LocalDistributionZone"))
                    && !_methods.IsValidLocalDistributionZone(r.Field<string>("LocalDistributionZone")));

                foreach(var invalidLocalDistributionZoneDataRecord in invalidLocalDistributionZoneDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidLocalDistributionZoneDataRecord["RowId"]);
                    records[rowId]["LocalDistributionZone"].Add($"Invalid LDZ {invalidLocalDistributionZoneDataRecord["LocalDistributionZone"]}");
                }

                //Validate SOQ if it is populated
                var invalidStandardOfftakeQuantityDataRecords = mprnDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("SOQ"))
                    && !_methods.IsValidStandardOfftakeQuantity(r.Field<string>("SOQ")));

                foreach(var invalidStandardOfftakeQuantityDataRecord in invalidStandardOfftakeQuantityDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidStandardOfftakeQuantityDataRecord["RowId"]);
                    records[rowId]["SOQ"].Add($"Invalid Standard Offtake Quantity {invalidStandardOfftakeQuantityDataRecord["SOQ"]}");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Meter);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

