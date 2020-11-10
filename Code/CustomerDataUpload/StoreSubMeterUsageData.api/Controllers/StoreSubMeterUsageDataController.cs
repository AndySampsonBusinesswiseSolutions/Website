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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSubMeterUsageDataController(ILogger<StoreSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().StoreSubMeterUsageDataAPI, password);
            storeSubMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/Store")]
        public void Store([FromBody] object data)
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
                    storeSubMeterUsageDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreSubMeterUsageDataAPI, storeSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSubMeterUsageDataAPIId);

                var methods = new Methods();

                //Get SubMeter Usage data from Customer Data Upload
                var subMeterUsageDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['SubMeter HH Data']");

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
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}