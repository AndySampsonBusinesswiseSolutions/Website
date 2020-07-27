﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ValidateUsageUploadTempMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempMeterDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateUsageUploadTempMeterDataAPIId;

        public ValidateUsageUploadTempMeterDataController(ILogger<ValidateUsageUploadTempMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempMeterDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempMeterDataAPI);
            validateUsageUploadTempMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterData/Validate")]
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
                    validateUsageUploadTempMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateUsageUploadTempMeterDataAPI, validateUsageUploadTempMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.Customer].[Meter] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"AnnualUsage", "Annual Usage"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

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
                    errors.Add($"Invalid MPAN/MPRN {dataRow["MPXN"]} in row {row}");
                }

                //Get MPANs not stored in database
                var newMPANDataRecords = mpanDataRecords.Where(r => _customerMethods.MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription(0, r.Field<string>("MPXN")) > 0);

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
                errors.AddRange(_tempCustomerMethods.GetMissingRecords(newMPANDataRecords, requiredColumns));

                //Validate GSP
                var invalidGridSupplyPointDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("GridSupplyPoint"))
                    && !_methods.IsValidGridSupplyPoint(r.Field<string>("GridSupplyPoint")));

                foreach(var invalidGridSupplyPointDataRecord in invalidGridSupplyPointDataRecords)
                {
                    errors.Add($"Invalid GSP {invalidGridSupplyPointDataRecord["GridSupplyPoint"]} in row {invalidGridSupplyPointDataRecord["RowId"]}");
                }

                //Validate Profile Class
                var invalidProfileClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ProfileClass"))
                    && !_methods.IsValidProfileClass(r.Field<string>("ProfileClass")));

                foreach(var invalidProfileClassDataRecord in invalidProfileClassDataRecords)
                {
                    errors.Add($"Invalid Profile Class {invalidProfileClassDataRecord["ProfileClass"]} in row {invalidProfileClassDataRecord["RowId"]}");
                }

                //Validate MTC
                var invalidMeterTimeswitchClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("MeterTimeswitchClass"))
                    && !_methods.IsValidMeterTimeswitchClass(r.Field<string>("MeterTimeswitchClass")));

                foreach(var invalidMeterTimeswitchClassDataRecord in invalidMeterTimeswitchClassDataRecords)
                {
                    errors.Add($"Invalid MTC {invalidMeterTimeswitchClassDataRecord["MeterTimeswitchClass"]} in row {invalidMeterTimeswitchClassDataRecord["RowId"]}");
                }

                //Validate LLFC
                var invalidLineLossFactorClassDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("LineLossFactorClass"))
                    && !_methods.IsValidLineLossFactorClass(r.Field<string>("LineLossFactorClass")));

                foreach(var invalidLineLossFactorClassDataRecord in invalidLineLossFactorClassDataRecords)
                {
                    errors.Add($"Invalid LLFC {invalidLineLossFactorClassDataRecord["LineLossFactorClass"]} in row {invalidLineLossFactorClassDataRecord["RowId"]}");
                }

                //Validate Capacity if it is populated
                var invalidCapacityDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Capacity"))
                    && !_methods.IsValidCapacity(r.Field<string>("Capacity")));

                //Get MPRNs not stored in database
                var newMPRNDataRecords = mprnDataRecords.Where(r => _customerMethods.MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription(0, r.Field<string>("MPXN")) > 0);

                //Site, LDZ and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"SiteName", "Site Name"},
                        {"LocalDistributionZone", "LDZ"},
                        {"Area", "Area"}
                    };
                errors.AddRange(_tempCustomerMethods.GetMissingRecords(newMPANDataRecords, requiredColumns));

                //Validate LDZ
                var invalidLocalDistributionZoneDataRecords = mpanDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("LocalDistributionZone"))
                    && !_methods.IsValidLocalDistributionZone(r.Field<string>("LocalDistributionZone")));

                foreach(var invalidLocalDistributionZoneDataRecord in invalidLocalDistributionZoneDataRecords)
                {
                    errors.Add($"Invalid LDZ {invalidLocalDistributionZoneDataRecord["LocalDistributionZone"]} in row {invalidLocalDistributionZoneDataRecord["RowId"]}");
                }

                //Validate SOQ if it is populated
                var invalidStandardOfftakeQuantityDataRecords = mprnDataRecords.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("SOQ"))
                    && !_methods.IsValidStandardOfftakeQuantity(r.Field<string>("SOQ")));

                //Update Process Queue
                var errorMessage = errors.Any() ? string.Join(';', errors) : null;
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, errors.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

