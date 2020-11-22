using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace ValidateMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterDataController> _logger;
        private readonly Int64 validateMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterDataController(ILogger<ValidateMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterDataAPI, password);
            validateMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateMeterDataAPI, validateMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] table
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(processQueueGUID);

                if(!meterEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
                var customerMethods = new Methods.Customer();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.SiteName,
                        customerDataUploadValidationEntityEnums.SitePostCode,
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.GridSupplyPoint,
                        customerDataUploadValidationEntityEnums.ProfileClass,
                        customerDataUploadValidationEntityEnums.MeterTimeswitchCode,
                        customerDataUploadValidationEntityEnums.LineLossFactorClass,
                        customerDataUploadValidationEntityEnums.Capacity,
                        customerDataUploadValidationEntityEnums.LocalDistributionZone,
                        customerDataUploadValidationEntityEnums.StandardOfftakeQuantity,
                        customerDataUploadValidationEntityEnums.AnnualUsage,
                        customerDataUploadValidationEntityEnums.MeterSerialNumber,
                        customerDataUploadValidationEntityEnums.Area,
                        customerDataUploadValidationEntityEnums.ImportExport,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(meterEntities.Select(me => me.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.AnnualUsage, "Annual Usage"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, meterEntities, requiredColumns);

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
                    if(!records[row.Value][customerDataUploadValidationEntityEnums.MPXN].Contains($"Invalid MPAN/MPRN '{meterEntity.MPXN}'"))
                    {
                        records[row.Value][customerDataUploadValidationEntityEnums.MPXN].Add($"Invalid MPAN/MPRN '{meterEntity.MPXN}'");
                    }
                }

                //Get MPANs not stored in database
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var newMPANEntities = mpanEntities.Where(me => 
                    customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, me.MPXN) == 0)
                    .ToList();

                //Site, GSP, PC, MTC, LLFC and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {customerDataUploadValidationEntityEnums.GridSupplyPoint, "GSP"},
                        {customerDataUploadValidationEntityEnums.ProfileClass, "Profile Class"},
                        {customerDataUploadValidationEntityEnums.MeterTimeswitchCode, "MTC"},
                        {customerDataUploadValidationEntityEnums.LineLossFactorClass, "LLFC"},
                        {customerDataUploadValidationEntityEnums.Area, customerDataUploadValidationEntityEnums.Area}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newMPANEntities, requiredColumns);

                //Validate GSP
                var invalidGridSupplyPointEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.GridSupplyPoint)
                    && !methods.IsValidGridSupplyPoint(me.GridSupplyPoint));

                foreach(var invalidGridSupplyPointEntity in invalidGridSupplyPointEntities)
                {
                    if(!records[invalidGridSupplyPointEntity.RowId.Value][customerDataUploadValidationEntityEnums.GridSupplyPoint].Contains($"Invalid GSP '{invalidGridSupplyPointEntity.GridSupplyPoint}'"))
                    {
                        records[invalidGridSupplyPointEntity.RowId.Value][customerDataUploadValidationEntityEnums.GridSupplyPoint].Add($"Invalid GSP '{invalidGridSupplyPointEntity.GridSupplyPoint}'");
                    }
                }

                //Validate Profile Class
                var invalidProfileClassEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.ProfileClass)
                    && !methods.IsValidProfileClass(me.ProfileClass));

                foreach(var invalidProfileClassEntity in invalidProfileClassEntities)
                {
                    if(!records[invalidProfileClassEntity.RowId.Value][customerDataUploadValidationEntityEnums.ProfileClass].Contains($"Invalid Profile Class '{invalidProfileClassEntity.ProfileClass}'"))
                    {
                        records[invalidProfileClassEntity.RowId.Value][customerDataUploadValidationEntityEnums.ProfileClass].Add($"Invalid Profile Class '{invalidProfileClassEntity.ProfileClass}'");
                    }
                }

                //Validate MTC
                var invalidMeterTimeswitchCodeEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.MeterTimeswitchCode)
                    && !methods.IsValidMeterTimeswitchCode(me.MeterTimeswitchCode));

                foreach(var invalidMeterTimeswitchCodeEntity in invalidMeterTimeswitchCodeEntities)
                {
                    if(!records[invalidMeterTimeswitchCodeEntity.RowId.Value][customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Contains($"Invalid MTC '{invalidMeterTimeswitchCodeEntity.MeterTimeswitchCode}'"))
                    {
                        records[invalidMeterTimeswitchCodeEntity.RowId.Value][customerDataUploadValidationEntityEnums.MeterTimeswitchCode].Add($"Invalid MTC '{invalidMeterTimeswitchCodeEntity.MeterTimeswitchCode}'");
                    }
                }

                //Validate LLFC
                var invalidLineLossFactorClassEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.LineLossFactorClass)
                    && !methods.IsValidLineLossFactorClass(me.LineLossFactorClass));

                foreach(var invalidLineLossFactorClassEntity in invalidLineLossFactorClassEntities)
                {
                    if(!records[invalidLineLossFactorClassEntity.RowId.Value][customerDataUploadValidationEntityEnums.LineLossFactorClass].Contains($"Invalid LLFC '{invalidLineLossFactorClassEntity.LineLossFactorClass}'"))
                    {
                        records[invalidLineLossFactorClassEntity.RowId.Value][customerDataUploadValidationEntityEnums.LineLossFactorClass].Add($"Invalid LLFC '{invalidLineLossFactorClassEntity.LineLossFactorClass}'");
                    }
                }

                //Validate Capacity if it is populated
                var invalidCapacityEntities = mpanEntities.Where(me => !string.IsNullOrWhiteSpace(me.Capacity)
                    && !methods.IsValidCapacity(me.Capacity));

                foreach(var invalidCapacityEntity in invalidCapacityEntities)
                {
                    if(!records[invalidCapacityEntity.RowId.Value][customerDataUploadValidationEntityEnums.Capacity].Contains($"Invalid Capacity '{invalidCapacityEntity.Capacity}'"))
                    {
                        records[invalidCapacityEntity.RowId.Value][customerDataUploadValidationEntityEnums.Capacity].Add($"Invalid Capacity '{invalidCapacityEntity.Capacity}'");
                    }
                }

                //Get MPRNs not stored in database
                var newMPRNEntities = mprnEntities.Where(me => 
                    customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, me.MPXN) == 0)
                    .ToList();

                //Site, LDZ and Area must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SiteName, "Site Name"},
                        {customerDataUploadValidationEntityEnums.LocalDistributionZone, "LDZ"},
                        {customerDataUploadValidationEntityEnums.Area, customerDataUploadValidationEntityEnums.Area}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newMPRNEntities, requiredColumns);

                //Validate LDZ
                var invalidLocalDistributionZoneEntities = mprnEntities.Where(me => !string.IsNullOrWhiteSpace(me.LocalDistributionZone)
                    && !methods.IsValidLocalDistributionZone(me.LocalDistributionZone));

                foreach(var invalidLocalDistributionZoneEntity in invalidLocalDistributionZoneEntities)
                {
                    if(!records[invalidLocalDistributionZoneEntity.RowId.Value][customerDataUploadValidationEntityEnums.LocalDistributionZone].Contains($"Invalid LDZ '{invalidLocalDistributionZoneEntity.LocalDistributionZone}'"))
                    {
                        records[invalidLocalDistributionZoneEntity.RowId.Value][customerDataUploadValidationEntityEnums.LocalDistributionZone].Add($"Invalid LDZ '{invalidLocalDistributionZoneEntity.LocalDistributionZone}'");
                    }
                }

                //Validate SOQ if it is populated
                var invalidStandardOfftakeQuantityEntities = mprnEntities.Where(me => !string.IsNullOrWhiteSpace(me.StandardOfftakeQuantity)
                    && !methods.IsValidStandardOfftakeQuantity(me.StandardOfftakeQuantity));

                foreach(var invalidStandardOfftakeQuantityEntity in invalidStandardOfftakeQuantityEntities)
                {
                    if(!records[invalidStandardOfftakeQuantityEntity.RowId.Value][customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Contains($"Invalid Standard Offtake Quantity '{invalidStandardOfftakeQuantityEntity.StandardOfftakeQuantity}'"))
                    {
                        records[invalidStandardOfftakeQuantityEntity.RowId.Value][customerDataUploadValidationEntityEnums.StandardOfftakeQuantity].Add($"Invalid Standard Offtake Quantity '{invalidStandardOfftakeQuantityEntity.StandardOfftakeQuantity}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().Meter);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}