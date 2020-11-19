using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace StoreSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSubMeterUsageDataController> _logger;
        private readonly Int64 storeSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSubMeterUsageDataController(ILogger<StoreSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSubMeterUsageDataAPI, password);
            storeSubMeterUsageDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(storeSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/Store")]
        public void Store([FromBody] object data)
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
                    storeSubMeterUsageDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreSubMeterUsageDataAPI, storeSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSubMeterUsageDataAPIId);

                var methods = new Methods();

                //Get SubMeter Usage data from Customer Data Upload
                var subMeterUsageDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['SubMeter HH Data']");

                //Create data table
                var subMeterUsageDataTable = new DataTable();
                subMeterUsageDataTable.Columns.Add("ProcessQueueGUID", typeof(Guid));
                subMeterUsageDataTable.Columns.Add("RowId", typeof(int));
                subMeterUsageDataTable.Columns.Add("SubMeterIdentifier", typeof(string));
                subMeterUsageDataTable.Columns.Add("Date", typeof(string));
                subMeterUsageDataTable.Columns.Add("TimePeriod", typeof(string));
                subMeterUsageDataTable.Columns.Add("Value", typeof(string));
                subMeterUsageDataTable.Columns.Add("CanCommit", typeof(bool));

                //Set default values
                subMeterUsageDataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                subMeterUsageDataTable.Columns["CanCommit"].DefaultValue = false;

                //TODO: Make parallel

                foreach(var row in subMeterUsageDictionary.Keys)
                {
                    var values = subMeterUsageDictionary[row];
                    var subMeterIdentifier = values[0];
                    var date = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);

                    for(var timePeriod = 2; timePeriod < values.Count(); timePeriod++)
                    {
                        var timePeriodString = methods.ConvertIntegerToHalfHourTimePeriod(timePeriod);

                        //Insert data into subMeterUsageDataTable
                        var subMeterUsageDataRow = subMeterUsageDataTable.NewRow();
                        subMeterUsageDataRow["RowId"] = row;
                        subMeterUsageDataRow["SubMeterIdentifier"] = subMeterIdentifier;
                        subMeterUsageDataRow["Date"] = date;
                        subMeterUsageDataRow["TimePeriod"] = timePeriodString;
                        subMeterUsageDataRow["Value"] = values[timePeriod];

                        subMeterUsageDataTable.Rows.Add(subMeterUsageDataRow);
                    }
                }

                //Bulk Insert subMeterUsageDataTable
                methods.BulkInsert(subMeterUsageDataTable, "[Temp.CustomerDataUpload].[SubMeterUsage]");

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}