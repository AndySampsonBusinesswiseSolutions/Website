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
using Newtonsoft.Json;

namespace CommitPeriodicUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitPeriodicUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitPeriodicUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 commitPeriodicUsageDataAPIId;

        public CommitPeriodicUsageDataController(ILogger<CommitPeriodicUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitPeriodicUsageDataAPI, _systemAPIPasswordEnums.CommitPeriodicUsageDataAPI);
            commitPeriodicUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitPeriodicUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/Commit")]
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
                    commitPeriodicUsageDataAPIId);

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, commitPeriodicUsageDataAPIId, jsonObject))
                {
                    return;
                }

                /*
                    TODO:
                        Upgrade to extract last 365 days to calculate EAU
                */

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get mpxn
                var mpxn = jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString();

                //Get meterId/subMeterId
                long meterId = meterType == "Meter"
                    ? GetMeterId(mpxn)
                    : GetSubMeterId(mpxn);

                //Get UsageTypeId
                var usageType = "Customer Estimated";
                var usageTypeId = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);

                //Get GranularityId
                var granularity = jsonObject[_systemAPIRequiredDataKeyEnums.Granularity].ToString();
                var granularityId = _informationMethods.Granularity_GetGranularityIdByGranularityDescription(granularity);
                var granularityTimePeriods = _mappingMethods.GranularityToTimePeriod_GetTimePeriodIdListByGranularityId(granularityId);

                //Get Periodic Usage
                var periodicUsageJson = jsonObject[_systemAPIRequiredDataKeyEnums.PeriodicUsage].ToString();
                var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(periodicUsageJson.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
                var periodicUsageDictionary = periodicUsageTempDictionary.ToDictionary(x => x.Key.Substring(0, 10), x => JsonConvert.DeserializeObject<Dictionary<string, string>>(x.Value));

                var dates = periodicUsageDictionary.Select(u => u.Key)
                    .Distinct()
                    .Select(d => _methods.GetDateTimeSqlParameterFromDateTimeString(d).Substring(0, 10))
                    .ToDictionary(d => d, d => _informationMethods.Date_GetDateIdByDateDescription(d));

                var timePeriods = periodicUsageDictionary.SelectMany(u => u.Value)
                    .Select(d => d.Key)
                    .Distinct()
                    .ToDictionary(t => t, t => _informationMethods.TimePeriod_GetTimePeriodIdListByEndTime(t).Intersect(granularityTimePeriods).FirstOrDefault());

                foreach (var periodicUsage in periodicUsageDictionary)
                {
                    var date = periodicUsage.Key;
                    var dateId = dates[date];

                    foreach (var timePeriod in periodicUsage.Value)
                    {
                        if (string.IsNullOrWhiteSpace(timePeriod.Value))
                        {
                            continue;
                        }

                        var timePeriodId = timePeriods[timePeriod.Key];
                        var usage = Convert.ToDecimal(timePeriod.Value);

                        //End date existing Periodic Usage
                        _supplyMethods.LoadedUsage_Delete(meterType, meterId, dateId, timePeriodId);

                        //Insert new Periodic Usage
                        _supplyMethods.LoadedUsage_Insert(createdByUserId, sourceId, meterType, meterId, dateId, timePeriodId, usageTypeId, usage);
                    }
                }

                //Get last 365days of periodic usage
                var latestPeriodicUsage = new List<DataRow>();

                //Create Estimated Annual Usage
                var estimatedAnnualUsage = 0; //latestPeriodicUsage.Where(r => r.Field<DateTime>("Date") >= DateTime.Today.AddDays(-365))
                                              //.Sum(r => r.Field<decimal>("Usage"));

                //End date existing Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                //Insert new Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitPeriodicUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string mpxn)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return _customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}

