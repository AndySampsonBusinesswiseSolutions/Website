﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace StoreMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<StoreMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeMeterUsageDataAPIId;

        public StoreMeterUsageDataController(ILogger<StoreMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreMeterUsageDataAPI, _systemAPIPasswordEnums.StoreMeterUsageDataAPI);
            storeMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterUsageData/Store")]
        public void Store([FromBody] object data)
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
                    storeMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreMeterUsageDataAPI, storeMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Meter Usage data from Customer Data Upload
                var meterUsageDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Meter HH Data']");

                //Create data table
                var meterUsageDataTable = new DataTable();
                meterUsageDataTable.Columns.Add("ProcessQueueGUID", typeof(Guid));
                meterUsageDataTable.Columns.Add("RowId", typeof(int));
                meterUsageDataTable.Columns.Add("MPXN", typeof(string));
                meterUsageDataTable.Columns.Add("Date", typeof(string));
                meterUsageDataTable.Columns.Add("TimePeriod", typeof(string));
                meterUsageDataTable.Columns.Add("Value", typeof(string));
                meterUsageDataTable.Columns.Add("CanCommit", typeof(bool));

                foreach(var row in meterUsageDictionary.Keys)
                {
                    var values = meterUsageDictionary[row];
                    var mpxn = values[0];
                    var date = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));

                    for(var timePeriod = 2; timePeriod < values.Count(); timePeriod++)
                    {                       
                        var timePeriodString = _methods.ConvertIntegerToHalfHourTimePeriod(timePeriod);

                        //Insert data into meterUsageDataTable
                        var meterUsageDataRow = meterUsageDataTable.NewRow();
                        meterUsageDataRow["ProcessQueueGUID"] = processQueueGUID;
                        meterUsageDataRow["RowId"] = row;
                        meterUsageDataRow["MPXN"] = mpxn;
                        meterUsageDataRow["Date"] = date;
                        meterUsageDataRow["TimePeriod"] = timePeriodString;
                        meterUsageDataRow["Value"] = values[timePeriod];
                        meterUsageDataRow["CanCommit"] = false;

                        meterUsageDataTable.Rows.Add(meterUsageDataRow);
                    }
                }

                //Bulk Insert meterUsageDataTable
                _methods.BulkInsert(meterUsageDataTable, "[Temp.CustomerDataUpload].[MeterUsage]");

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

