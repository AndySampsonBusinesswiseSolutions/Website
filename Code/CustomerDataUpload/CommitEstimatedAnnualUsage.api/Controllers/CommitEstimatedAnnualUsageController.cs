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

namespace CommitEstimatedAnnualUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitEstimatedAnnualUsageController : ControllerBase
    {
        private readonly ILogger<CommitEstimatedAnnualUsageController> _logger;
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
        private readonly Int64 commitEstimatedAnnualUsageAPIId;

        public CommitEstimatedAnnualUsageController(ILogger<CommitEstimatedAnnualUsageController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitEstimatedAnnualUsageAPI, _systemAPIPasswordEnums.CommitEstimatedAnnualUsageAPI);
            commitEstimatedAnnualUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitEstimatedAnnualUsageAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/Commit")]
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
                    commitEstimatedAnnualUsageAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI, commitEstimatedAnnualUsageAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId);

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

                //Get Estimated Annual Usage
                var estimatedAnnualUsage = Convert.ToDecimal(jsonObject[_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage]);

                //End date existing Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                //Insert new Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);

                //Get UsageTypeId
                var usageType = "Customer Estimated";
                var usageTypeId = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);

                //TODO: Get profile -> DateId, TimePeriodId, Usage
                var profile = new Dictionary<long, Dictionary<long, decimal>>();
                var profileUsage = new Dictionary<long, Dictionary<long, decimal>>();

                //Create Periodic Usage
                foreach(var date in profile)
                {
                    profileUsage.Add(date.Key, new Dictionary<long, decimal>());

                    foreach(var timePeriod in date.Value)
                    {
                        profileUsage[date.Key].Add(timePeriod.Key, timePeriod.Value * estimatedAnnualUsage);
                    }
                }

                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("ProcessQueueGUID", typeof(string));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("DateId", typeof(long));
                dataTable.Columns.Add("TimePeriodId", typeof(long));
                dataTable.Columns.Add("UsageTypeId", typeof(long));
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;
                dataTable.Columns["UsageTypeId"].DefaultValue = usageTypeId;

                foreach(var date in profileUsage)
                {
                    foreach(var timePeriod in date.Value)
                    {
                        var dataRow = dataTable.NewRow();
                        dataRow["DateId"] = date.Key;
                        dataRow["TimePeriodId"] = timePeriod.Key;
                        dataRow["Usage"] = timePeriod.Value;
                        dataTable.Rows.Add(dataRow);
                    }
                }   

                //Bulk Insert new Periodic Usage into LoadedUsage_Temp table
                _supplyMethods.LoadedUsageTemp_Insert(meterType, meterId, dataTable);

                //End date existing Periodic Usage
                _supplyMethods.LoadedUsage_Delete(meterType, meterId);

                //Insert new Periodic Usage into LoadedUsage table
                _supplyMethods.LoadedUsage_Insert(meterType, meterId, processQueueGUID);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}