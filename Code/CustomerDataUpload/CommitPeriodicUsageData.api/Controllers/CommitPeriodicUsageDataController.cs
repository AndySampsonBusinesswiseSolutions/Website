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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, commitPeriodicUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get mpxn
                var mpxn = jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString();

                //Get MeterIdentifierMeterAttributeId
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

                //Get MeterId
                var meterId = _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();

                //Get CommodityId by MeterId
                var commodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);

                //Get Commodity
                var commodity = _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get UsageTypeId
                var usageType = "Customer Estimated";
                var usageTypeId = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);

                //Get GranularityId
                var granularity = "Half Hour";
                var granularityId = _informationMethods.Granularity_GetGranularityIdByGranularityDescription(granularity);
                var granularityTimePeriods = _mappingMethods.GranularityToTimePeriod_GetTimePeriodIdListByGranularityId(granularityId);

                //Get Periodic Usage
                var periodicUsage = (IEnumerable<DataRow>) JsonConvert.DeserializeObject(jsonObject[_systemAPIRequiredDataKeyEnums.PeriodicUsage].ToString(), typeof(List<DataRow>));
                var dates = periodicUsage.Select(r => r.Field<string>("Date"))
                    .Distinct()
                    .ToDictionary(d => d, d => _informationMethods.Date_GetDateIdByDateDescription(d));
                var timePeriods = periodicUsage.Select(r => r.Field<string>("TimePeriod"))
                    .Distinct()
                    .ToDictionary(t => t, t => _informationMethods.TimePeriod_GetTimePeriodIdListByEndTime(t).Intersect(granularityTimePeriods).FirstOrDefault());

                foreach(var dataRow in periodicUsage)
                {
                    var date = dataRow["Date"].ToString();
                    var timePeriod = dataRow["TimePeriod"].ToString();
                    var dateId = dates[date];
                    var timePeriodId = timePeriods[timePeriod];
                    var usage = Convert.ToDecimal(dataRow["Value"]);

                    //End date existing Periodic Usage
                    _supplyMethods.LoadedUsage_Delete(meterType, meterId, dateId, timePeriodId);

                    //Insert new Periodic Usage
                    _supplyMethods.LoadedUsage_Insert(createdByUserId, sourceId, meterType, meterId, dateId, timePeriodId, usageTypeId, usage);
                }

                //Get last 365days of periodic usage
                var latestPeriodicUsage = new List<DataRow>();

                //Create Estimated Annual Usage
                var estimatedAnnualUsage = latestPeriodicUsage.Where(r => r.Field<DateTime>("Date") >= DateTime.Today.AddDays(-365))
                    .Sum(r => r.Field<decimal>("Usage"));

                //End date existing Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                //Insert new Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);            

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitPeriodicUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

