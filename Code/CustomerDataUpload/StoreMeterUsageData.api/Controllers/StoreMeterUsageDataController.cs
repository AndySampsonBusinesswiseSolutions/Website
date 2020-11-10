using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace StoreMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterUsageDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 storeMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterUsageDataController(ILogger<StoreMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().StoreMeterUsageDataAPI, password);
            storeMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterUsageData/Store")]
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
                    storeMeterUsageDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreMeterUsageDataAPI, storeMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterUsageDataAPIId);

                var methods = new Methods();

                //Create data table
                var meterUsageDataTable = new DataTable();
                meterUsageDataTable.Columns.Add("ProcessQueueGUID", typeof(Guid));
                meterUsageDataTable.Columns.Add("SheetName", typeof(string));
                meterUsageDataTable.Columns.Add("RowId", typeof(int));
                meterUsageDataTable.Columns.Add("MPXN", typeof(string));
                meterUsageDataTable.Columns.Add("Date", typeof(string));
                meterUsageDataTable.Columns.Add("TimePeriod", typeof(string));
                meterUsageDataTable.Columns.Add("Value", typeof(string));
                meterUsageDataTable.Columns.Add("CanCommit", typeof(bool));

                //Set default values
                meterUsageDataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                meterUsageDataTable.Columns["CanCommit"].DefaultValue = false;

                var sourceSheetList = new List<string>
                {
                    _customerDataUploadValidationSheetNameEnums.MeterUsage,
                    _customerDataUploadValidationSheetNameEnums.DailyMeterUsage
                };

                foreach(var sourceSheet in sourceSheetList)
                {
                    var jsonSheet = $"Sheets['{sourceSheet}']";
                    var dailyTimePeriod = sourceSheet == _customerDataUploadValidationSheetNameEnums.DailyMeterUsage ? 1 : 0;

                    //Get Meter Usage data from Customer Data Upload
                    var meterUsageDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, jsonSheet);

                    foreach(var row in meterUsageDictionary.Keys)
                    {
                        var values = meterUsageDictionary[row];
                        var mpxn = values[0];
                        var date = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);

                        for(var timePeriod = 2; timePeriod < values.Count(); timePeriod++)
                        {                       
                            var timePeriodString = methods.ConvertIntegerToHalfHourTimePeriod(timePeriod - dailyTimePeriod);

                            //Insert data into meterUsageDataTable
                            var meterUsageDataRow = meterUsageDataTable.NewRow();
                            meterUsageDataRow["SheetName"] = sourceSheet;
                            meterUsageDataRow["RowId"] = row;
                            meterUsageDataRow["MPXN"] = mpxn;
                            meterUsageDataRow["Date"] = date;
                            meterUsageDataRow["TimePeriod"] = timePeriodString;
                            meterUsageDataRow["Value"] = values[timePeriod];

                            meterUsageDataTable.Rows.Add(meterUsageDataRow);
                        }
                    }
                }

                //Bulk Insert meterUsageDataTable
                methods.BulkInsert(meterUsageDataTable, "[Temp.CustomerDataUpload].[MeterUsage]");

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}