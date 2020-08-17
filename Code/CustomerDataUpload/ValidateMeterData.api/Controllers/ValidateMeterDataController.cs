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
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
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
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);

                if(!meterDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {_customerDataUploadValidationEntityEnums.SitePostCode, "Site PostCode"},
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.GridSupplyPoint, "GSP"},
                        {_customerDataUploadValidationEntityEnums.ProfileClass, "Profile Class"},
                        {_customerDataUploadValidationEntityEnums.MeterTimeswitchCode, "MTC"},
                        {_customerDataUploadValidationEntityEnums.LineLossFactorClass, "LLFC"},
                        {_customerDataUploadValidationEntityEnums.Capacity, _customerDataUploadValidationEntityEnums.Capacity},
                        {_customerDataUploadValidationEntityEnums.LocalDistributionZone, "LDZ"},
                        {_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity, "Standard Offtake Quantity"},
                        {_customerDataUploadValidationEntityEnums.AnnualUsage, "Annual Usage"},
                        {_customerDataUploadValidationEntityEnums.MeterSerialNumber, "Meter Serial Number"},
                        {_customerDataUploadValidationEntityEnums.Area, _customerDataUploadValidationEntityEnums.Area},
                        {_customerDataUploadValidationEntityEnums.ImportExport, "Import/Export"}
                    };

                var records = _tempCustomerMethods.InitialiseRecordsDictionary(meterDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.AnnualUsage, "Annual Usage"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, meterDataRows, requiredColumns);

                //Get MPANs
                var mpanDataRecords = meterDataRows.Where(r => _methods.IsValidMPAN(r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)));
                var mpanRows = mpanDataRecords.Select(r => r.Field<int>("RowId"));

                //Get MPRNs
                var mprnDataRecords = meterDataRows.Where(r => _methods.IsValidMPRN(r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)));
                var mprnRows = mprnDataRecords.Select(r => r.Field<int>("RowId"));

                //Get any MPXNs not in MPANs or MPRNs
                var invalidMPXNRows = meterDataRows.Select(r => r.Field<int>("RowId")).Except(mpanRows).Except(mprnRows);

                foreach(var row in invalidMPXNRows)
                {
                    var dataRow = meterDataRows.First(r => r.Field<int>("RowId") == row);
                    if(!records[row][_customerDataUploadValidationEntityEnums.MPXN].Contains($"Invalid MPAN/MPRN {dataRow[_customerDataUploadValidationEntityEnums.MPXN]}"))
                    {
                        records[row][_customerDataUploadValidationEntityEnums.MPXN].Add($"Invalid MPAN/MPRN {dataRow[_customerDataUploadValidationEntityEnums.MPXN]}");
                    }
                }

                //Get MPANs not stored in database
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var newMPANDataRecords = mpanDataRecords.Where(r => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)) == 0);

                //Site, GSP, PC, MTC, LLFC and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {_customerDataUploadValidationEntityEnums.GridSupplyPoint, "GSP"},
                        {_customerDataUploadValidationEntityEnums.ProfileClass, "Profile Class"},
                        {_customerDataUploadValidationEntityEnums.MeterTimeswitchCode, "MTC"},
                        {_customerDataUploadValidationEntityEnums.LineLossFactorClass, "LLFC"},
                        {_customerDataUploadValidationEntityEnums.Area, _customerDataUploadValidationEntityEnums.Area}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newMPANDataRecords, requiredColumns);

                //Validate GSP
                var invalidGridSupplyPointDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.GridSupplyPoint))
                    && !_methods.IsValidGridSupplyPoint(r.Field<string>(_customerDataUploadValidationEntityEnums.GridSupplyPoint)));

                foreach(var invalidGridSupplyPointDataRecord in invalidGridSupplyPointDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidGridSupplyPointDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.GridSupplyPoint].Contains($"Invalid GSP {invalidGridSupplyPointDataRecord[_customerDataUploadValidationEntityEnums.GridSupplyPoint]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.GridSupplyPoint].Add($"Invalid GSP {invalidGridSupplyPointDataRecord[_customerDataUploadValidationEntityEnums.GridSupplyPoint]}");
                    }
                }

                //Validate Profile Class
                var invalidProfileClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ProfileClass))
                    && !_methods.IsValidProfileClass(r.Field<string>(_customerDataUploadValidationEntityEnums.ProfileClass)));

                foreach(var invalidProfileClassDataRecord in invalidProfileClassDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidProfileClassDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ProfileClass].Contains($"Invalid Profile Class {invalidProfileClassDataRecord[_customerDataUploadValidationEntityEnums.ProfileClass]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ProfileClass].Add($"Invalid Profile Class {invalidProfileClassDataRecord[_customerDataUploadValidationEntityEnums.ProfileClass]}");
                    }
                }

                //Validate MTC
                var invalidMeterTimeswitchCodeDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.MeterTimeswitchCode))
                    && !_methods.IsValidMeterTimeswitchCode(r.Field<string>(_customerDataUploadValidationEntityEnums.MeterTimeswitchCode)));

                foreach(var invalidMeterTimeswitchCodeDataRecord in invalidMeterTimeswitchCodeDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidMeterTimeswitchCodeDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Contains($"Invalid MTC {invalidMeterTimeswitchCodeDataRecord[_customerDataUploadValidationEntityEnums.MeterTimeswitchCode]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Add($"Invalid MTC {invalidMeterTimeswitchCodeDataRecord[_customerDataUploadValidationEntityEnums.MeterTimeswitchCode]}");
                    }
                }

                //Validate LLFC
                var invalidLineLossFactorClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.LineLossFactorClass))
                    && !_methods.IsValidLineLossFactorClass(r.Field<string>(_customerDataUploadValidationEntityEnums.LineLossFactorClass)));

                foreach(var invalidLineLossFactorClassDataRecord in invalidLineLossFactorClassDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidLineLossFactorClassDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.LineLossFactorClass].Contains($"Invalid LLFC {invalidLineLossFactorClassDataRecord[_customerDataUploadValidationEntityEnums.LineLossFactorClass]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.LineLossFactorClass].Add($"Invalid LLFC {invalidLineLossFactorClassDataRecord[_customerDataUploadValidationEntityEnums.LineLossFactorClass]}");
                    }
                }

                //Validate Capacity if it is populated
                var invalidCapacityDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Capacity))
                    && !_methods.IsValidCapacity(r.Field<string>(_customerDataUploadValidationEntityEnums.Capacity)));

                foreach(var invalidCapacityDataRecord in invalidCapacityDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidCapacityDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Capacity].Contains($"Invalid Capacity {invalidCapacityDataRecord[_customerDataUploadValidationEntityEnums.Capacity]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Capacity].Add($"Invalid Capacity {invalidCapacityDataRecord[_customerDataUploadValidationEntityEnums.Capacity]}");
                    }
                }

                //Get MPRNs not stored in database
                var newMPRNDataRecords = mprnDataRecords.Where(r => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)) == 0);

                //Site, LDZ and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {_customerDataUploadValidationEntityEnums.LocalDistributionZone, "LDZ"},
                        {_customerDataUploadValidationEntityEnums.Area, _customerDataUploadValidationEntityEnums.Area}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newMPRNDataRecords, requiredColumns);

                //Validate LDZ
                var invalidLocalDistributionZoneDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.LocalDistributionZone))
                    && !_methods.IsValidLocalDistributionZone(r.Field<string>(_customerDataUploadValidationEntityEnums.LocalDistributionZone)));

                foreach(var invalidLocalDistributionZoneDataRecord in invalidLocalDistributionZoneDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidLocalDistributionZoneDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.LocalDistributionZone].Contains($"Invalid LDZ {invalidLocalDistributionZoneDataRecord[_customerDataUploadValidationEntityEnums.LocalDistributionZone]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.LocalDistributionZone].Add($"Invalid LDZ {invalidLocalDistributionZoneDataRecord[_customerDataUploadValidationEntityEnums.LocalDistributionZone]}");
                    }
                }

                //Validate SOQ if it is populated
                var invalidStandardOfftakeQuantityDataRecords = mprnDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity))
                    && !_methods.IsValidStandardOfftakeQuantity(r.Field<string>(_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity)));

                foreach(var invalidStandardOfftakeQuantityDataRecord in invalidStandardOfftakeQuantityDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidStandardOfftakeQuantityDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Contains($"Invalid Standard Offtake Quantity {invalidStandardOfftakeQuantityDataRecord[_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Add($"Invalid Standard Offtake Quantity {invalidStandardOfftakeQuantityDataRecord[_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity]}");
                    }
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

