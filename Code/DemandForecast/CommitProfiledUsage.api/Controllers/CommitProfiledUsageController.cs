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
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
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

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId/subMeterId
                var meterId = meterType == "Meter"
                    ? GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString())
                    : GetSubMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString());

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

                var profileDictionary = new Dictionary<long, Dictionary<long, decimal>>();

                if(!profileDictionary.Any())
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

                foreach(var periodicUsage in profileDictionary)
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

                //Bulk Insert new Periodic Usage into LoadedUsage_Temp table
                _supplyMethods.LoadedUsageTemp_Insert(meterType, meterId, dataTable);

                //End date existing Periodic Usage
                _supplyMethods.LoadedUsage_Delete(meterType, meterId);

                //Insert new Periodic Usage into LoadedUsage table
                _supplyMethods.LoadedUsage_Insert(meterType, meterId, processQueueGUID);

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
            var _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string mpxn)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
            var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return _customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}