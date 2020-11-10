using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitEstimatedAnnualUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitEstimatedAnnualUsageController : ControllerBase
    {
        private readonly ILogger<CommitEstimatedAnnualUsageController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.Process.GUID _systemProcessGUIDEnums = new Enums.System.Process.GUID();
        private readonly Int64 commitEstimatedAnnualUsageAPIId;
        private readonly string hostEnvironment;

        public CommitEstimatedAnnualUsageController(ILogger<CommitEstimatedAnnualUsageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CommitEstimatedAnnualUsageAPI, password);
            commitEstimatedAnnualUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitEstimatedAnnualUsageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/Commit")]
        public void Commit([FromBody] object data)
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
                    commitEstimatedAnnualUsageAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI, commitEstimatedAnnualUsageAPIId, hostEnvironment, jsonObject))
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

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get Estimated Annual Usage
                var estimatedAnnualUsage = Convert.ToDecimal(jsonObject[_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage]);

                //End date existing Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                //Insert new Estimated Annual Usage
                _supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);

                //Get HasPeriodicUsage
                var hasPeriodicUsage = Convert.ToBoolean(jsonObject[_systemAPIRequiredDataKeyEnums.HasPeriodicUsage]);

                //Since the entity has periodic usage, don't bother creating a profiled version of the estimated annual usage
                if(hasPeriodicUsage)
                {
                    //Update Process Queue
                   _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitEstimatedAnnualUsageAPIId, false, null);

                    return;
                }

                //Launch GetProfile process and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitProfiledUsageAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Get profile from Profiling API
                var profileString = JsonConvert.DeserializeObject(result.Result.ToString()).ToString();
                var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(profileString.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
                var periodicUsageDictionary = periodicUsageTempDictionary.ToDictionary(x => Convert.ToInt64(x.Key), x => JsonConvert.DeserializeObject<Dictionary<long, decimal>>(x.Value));

                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("LoadedUsageId", typeof(long));
                dataTable.Columns.Add("CreatedDateTime", typeof(DateTime));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("DateId", typeof(long));
                dataTable.Columns.Add("TimePeriodId", typeof(long));
                dataTable.Columns.Add("UsageTypeId", typeof(long));
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["CreatedDateTime"].DefaultValue = DateTime.UtcNow;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;
                dataTable.Columns["UsageTypeId"].DefaultValue = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription("Customer Estimated");

                foreach(var date in periodicUsageDictionary)
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
                _supplyMethods.LoadedUsage_Insert(meterType, meterId, dataTable);

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Update Process GUID to Create Forecast Usage Process GUID
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CreateForecastUsage);

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);

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