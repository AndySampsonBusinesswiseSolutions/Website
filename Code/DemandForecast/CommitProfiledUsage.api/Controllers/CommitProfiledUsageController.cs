using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CommitProfiledUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitProfiledUsageController : ControllerBase
    {
        private readonly ILogger<CommitProfiledUsageController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.UsageType _informationUsageTypeEnums = new Enums.Information.UsageType();
        private readonly Int64 commitProfiledUsageAPIId;

        public CommitProfiledUsageController(ILogger<CommitProfiledUsageController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitProfiledUsageAPI, _systemAPIPasswordEnums.CommitProfiledUsageAPI);
            commitProfiledUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitProfiledUsageAPI);
        }

        [HttpPost]
        [Route("CommitProfiledUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitProfiledUsageAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitProfiledUsage/Commit")]
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
                    commitProfiledUsageAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitProfiledUsageAPIId);

                //Launch GetProfile process and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitProfiledUsageAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var profileString = JsonConvert.DeserializeObject(result.Result.ToString()).ToString();

                //No profile found so empty dictionary returned
                if(profileString == "{}")
                {
                    return;
                }

                var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(profileString.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
                var periodicUsageDictionary = periodicUsageTempDictionary.ToDictionary(x => Convert.ToInt64(x.Key), x => JsonConvert.DeserializeObject<Dictionary<long, decimal>>(x.Value));
                var profileDictionary = new Dictionary<long, Dictionary<long, decimal>>();

                if(!periodicUsageDictionary.Any())
                {
                    return;
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
                dataTable.Columns["UsageTypeId"].DefaultValue = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(_informationUsageTypeEnums.Profile);

                foreach(var periodicUsage in periodicUsageDictionary)
                {
                    var timePeriodUsage = periodicUsage.Value;

                    foreach (var timePeriod in timePeriodUsage)
                    {
                        var dataRow = dataTable.NewRow();
                        dataRow["DateId"] = periodicUsage.Key;
                        dataRow["TimePeriodId"] = timePeriod.Key;
                        dataRow["Usage"] = timePeriod.Value;
                        dataTable.Rows.Add(dataRow);
                    }
                }

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId
                var meterId = GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString());

                //Bulk Insert new Periodic Usage into LoadedUsage_Temp table
                var supplyMethods = new Methods.Supply();
                supplyMethods.LoadedUsageTemp_Insert(meterType, meterId, dataTable);

                //End date existing Periodic Usage
                supplyMethods.LoadedUsage_Delete(meterType, meterId);

                //Insert new Periodic Usage into LoadedUsage table
                supplyMethods.LoadedUsage_Insert(meterType, meterId, processQueueGUID);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitProfiledUsageAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitProfiledUsageAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var customerMethods = new Methods.Customer();
            var customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}