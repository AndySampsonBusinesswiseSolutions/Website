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
using Microsoft.Extensions.Configuration;

namespace ValidateMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private static readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Int64 validateMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterDataController(ILogger<ValidateMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterDataAPI, password);
            validateMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterData/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterDataAPI, validateMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] table
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(processQueueGUID);

                if(!meterEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.SiteName,
                        _customerDataUploadValidationEntityEnums.SitePostCode,
                        _customerDataUploadValidationEntityEnums.MPXN,
                        _customerDataUploadValidationEntityEnums.GridSupplyPoint,
                        _customerDataUploadValidationEntityEnums.ProfileClass,
                        _customerDataUploadValidationEntityEnums.MeterTimeswitchCode,
                        _customerDataUploadValidationEntityEnums.LineLossFactorClass,
                        _customerDataUploadValidationEntityEnums.Capacity,
                        _customerDataUploadValidationEntityEnums.LocalDistributionZone,
                        _customerDataUploadValidationEntityEnums.StandardOfftakeQuantity,
                        _customerDataUploadValidationEntityEnums.AnnualUsage,
                        _customerDataUploadValidationEntityEnums.MeterSerialNumber,
                        _customerDataUploadValidationEntityEnums.Area,
                        _customerDataUploadValidationEntityEnums.ImportExport,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(meterEntities.Select(me => me.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.AnnualUsage, "Annual Usage"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, meterEntities, requiredColumns);

                //Get MPANs
                var mpanEntities = meterEntities.Where(me => methods.IsValidMPAN(me.MPXN));
                var mpanRows = mpanEntities.Select(me => me.RowId);

                //Get MPRNs
                var mprnEntities = meterEntities.Where(me => methods.IsValidMPRN(me.MPXN));
                var mprnRows = mprnEntities.Select(me => me.RowId);

                //Get any MPXNs not in MPANs or MPRNs
                var invalidMPXNRows = meterEntities.Select(me => me.RowId).Except(mpanRows).Except(mprnRows);

                foreach(var row in invalidMPXNRows)
                {
                    var meterEntity = meterEntities.First(me => me.RowId == row);
                    if(!records[row.Value][_customerDataUploadValidationEntityEnums.MPXN].Contains($"Invalid MPAN/MPRN '{meterEntity.MPXN}'"))
                    {
                        records[row.Value][_customerDataUploadValidationEntityEnums.MPXN].Add($"Invalid MPAN/MPRN '{meterEntity.MPXN}'");
                    }
                }

                //Get MPANs not stored in database
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var newMPANEntities = mpanEntities.Where(me => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, me.MPXN) == 0)
                    .ToList();

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
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newMPANEntities, requiredColumns);

                //Validate GSP
                var invalidGridSupplyPointEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.GridSupplyPoint)
                    && !methods.IsValidGridSupplyPoint(me.GridSupplyPoint));

                foreach(var invalidGridSupplyPointEntity in invalidGridSupplyPointEntities)
                {
                    if(!records[invalidGridSupplyPointEntity.RowId.Value][_customerDataUploadValidationEntityEnums.GridSupplyPoint].Contains($"Invalid GSP '{invalidGridSupplyPointEntity.GridSupplyPoint}'"))
                    {
                        records[invalidGridSupplyPointEntity.RowId.Value][_customerDataUploadValidationEntityEnums.GridSupplyPoint].Add($"Invalid GSP '{invalidGridSupplyPointEntity.GridSupplyPoint}'");
                    }
                }

                //Validate Profile Class
                var invalidProfileClassEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.ProfileClass)
                    && !methods.IsValidProfileClass(me.ProfileClass));

                foreach(var invalidProfileClassEntity in invalidProfileClassEntities)
                {
                    if(!records[invalidProfileClassEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ProfileClass].Contains($"Invalid Profile Class '{invalidProfileClassEntity.ProfileClass}'"))
                    {
                        records[invalidProfileClassEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ProfileClass].Add($"Invalid Profile Class '{invalidProfileClassEntity.ProfileClass}'");
                    }
                }

                //Validate MTC
                var invalidMeterTimeswitchCodeEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.MeterTimeswitchCode)
                    && !methods.IsValidMeterTimeswitchCode(me.MeterTimeswitchCode));

                foreach(var invalidMeterTimeswitchCodeEntity in invalidMeterTimeswitchCodeEntities)
                {
                    if(!records[invalidMeterTimeswitchCodeEntity.RowId.Value][_customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Contains($"Invalid MTC '{invalidMeterTimeswitchCodeEntity.MeterTimeswitchCode}'"))
                    {
                        records[invalidMeterTimeswitchCodeEntity.RowId.Value][_customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Add($"Invalid MTC '{invalidMeterTimeswitchCodeEntity.MeterTimeswitchCode}'");
                    }
                }

                //Validate LLFC
                var invalidLineLossFactorClassEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.LineLossFactorClass)
                    && !methods.IsValidLineLossFactorClass(me.LineLossFactorClass));

                foreach(var invalidLineLossFactorClassEntity in invalidLineLossFactorClassEntities)
                {
                    if(!records[invalidLineLossFactorClassEntity.RowId.Value][_customerDataUploadValidationEntityEnums.LineLossFactorClass].Contains($"Invalid LLFC '{invalidLineLossFactorClassEntity.LineLossFactorClass}'"))
                    {
                        records[invalidLineLossFactorClassEntity.RowId.Value][_customerDataUploadValidationEntityEnums.LineLossFactorClass].Add($"Invalid LLFC '{invalidLineLossFactorClassEntity.LineLossFactorClass}'");
                    }
                }

                //Validate Capacity if it is populated
                var invalidCapacityEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.Capacity)
                    && !methods.IsValidCapacity(me.Capacity));

                foreach(var invalidCapacityEntity in invalidCapacityEntities)
                {
                    if(!records[invalidCapacityEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Capacity].Contains($"Invalid Capacity '{invalidCapacityEntity.Capacity}'"))
                    {
                        records[invalidCapacityEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Capacity].Add($"Invalid Capacity '{invalidCapacityEntity.Capacity}'");
                    }
                }

                //Get MPRNs not stored in database
                var newMPRNEntities = mprnEntities.Where(me => 
                    _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, me.MPXN) == 0)
                    .ToList();

                //Site, LDZ and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {_customerDataUploadValidationEntityEnums.LocalDistributionZone, "LDZ"},
                        {_customerDataUploadValidationEntityEnums.Area, _customerDataUploadValidationEntityEnums.Area}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newMPRNEntities, requiredColumns);

                //Validate LDZ
                var invalidLocalDistributionZoneEntities = mprnEntities.Where(me => !string.IsNullOrWhiteSpace(me.LocalDistributionZone)
                    && !methods.IsValidLocalDistributionZone(me.LocalDistributionZone));

                foreach(var invalidLocalDistributionZoneEntity in invalidLocalDistributionZoneEntities)
                {
                    if(!records[invalidLocalDistributionZoneEntity.RowId.Value][_customerDataUploadValidationEntityEnums.LocalDistributionZone].Contains($"Invalid LDZ '{invalidLocalDistributionZoneEntity.LocalDistributionZone}'"))
                    {
                        records[invalidLocalDistributionZoneEntity.RowId.Value][_customerDataUploadValidationEntityEnums.LocalDistributionZone].Add($"Invalid LDZ '{invalidLocalDistributionZoneEntity.LocalDistributionZone}'");
                    }
                }

                //Validate SOQ if it is populated
                var invalidStandardOfftakeQuantityEntities = mprnEntities.Where(me => !string.IsNullOrWhiteSpace(me.StandardOfftakeQuantity)
                    && !methods.IsValidStandardOfftakeQuantity(me.StandardOfftakeQuantity));

                foreach(var invalidStandardOfftakeQuantityEntity in invalidStandardOfftakeQuantityEntities)
                {
                    if(!records[invalidStandardOfftakeQuantityEntity.RowId.Value][_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Contains($"Invalid Standard Offtake Quantity '{invalidStandardOfftakeQuantityEntity.StandardOfftakeQuantity}'"))
                    {
                        records[invalidStandardOfftakeQuantityEntity.RowId.Value][_customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Add($"Invalid Standard Offtake Quantity '{invalidStandardOfftakeQuantityEntity.StandardOfftakeQuantity}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Meter);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}