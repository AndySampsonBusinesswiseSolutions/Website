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
using System.Text;

namespace StoreUsageUploadTempSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 storeUsageUploadTempSubMeterUsageDataAPIId;

        public StoreUsageUploadTempSubMeterUsageDataController(ILogger<StoreUsageUploadTempSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempSubMeterUsageDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempSubMeterUsageDataAPI);
            storeUsageUploadTempSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempSubMeterUsageDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempSubMeterUsageData/Store")]
        public void Store([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    storeUsageUploadTempSubMeterUsageDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadTempSubMeterUsageDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get File Content by FileId
                var fileGUID = jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
                var fileContent = _informationMethods.FileContent_GetFileContentByFileGUID(fileGUID);
                var fileJSON = JObject.Parse(fileContent);

                //Strip out data not related to SubMeter Usage
                var subMeterUsageDataTable = new DataTable();
                subMeterUsageDataTable.Columns.Add("ProcessQueueGUID");
                subMeterUsageDataTable.Columns.Add("MPXN");
                subMeterUsageDataTable.Columns.Add("Date");
                subMeterUsageDataTable.Columns.Add("TimePeriod");
                subMeterUsageDataTable.Columns.Add("Value");

                var sheetJSON = fileJSON.Children().FirstOrDefault(c => c.Path == "Sheets");
                var sitesJSON = sheetJSON.Values().FirstOrDefault(v => v.Path == "Sheets['SubMeter HH Data']");
                var validCells = sitesJSON.Values().Children().Where(c => c.Path.Replace("Sheets['SubMeter HH Data'].", string.Empty) != "!ref" 
                    && c.Path.Replace("Sheets['SubMeter HH Data'].", string.Empty) != "!margins").ToList();
                var cells = validCells.Where(c => !_methods.IsHeaderRow(c.Parent)).ToList();
                var columns = validCells.Where(c => _methods.IsHeaderRow(c.Parent))
                    .Select(c => c.Path.Replace(_methods.GetRow(c.Path).ToString(), string.Empty))
                    .Select(c => c.Replace("Sheets['SubMeter HH Data'].", string.Empty))
                    .OrderBy(c => Convert.ToInt64(string.Join(string.Empty, Encoding.ASCII.GetBytes(c))))
                    .Select(c => $"Sheets['SubMeter HH Data'].{c}")
                    .ToList();

                var cellDictionary = new Dictionary<int, List<string>>(columns.Count());

                foreach(var cell in cells)
                {
                    var row = _methods.GetRow(cell.Path);
                    var columnIndex = columns.IndexOf(cell.Path.Replace(row.ToString(), string.Empty));

                    if(!cellDictionary.ContainsKey(row))
                    {
                        cellDictionary.Add(row, new List<string>());
                        foreach(var column in columns)
                        {
                            cellDictionary[row].Add(string.Empty);
                        }
                    }

                    var valueToken = cell.Children().First(c => ((Newtonsoft.Json.Linq.JProperty)c).Name == "v");
                    var value = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)valueToken).Value).Value.ToString();
                    cellDictionary[row][columnIndex] = value;
                }

                foreach(var row in cellDictionary.Keys)
                {
                    var values = cellDictionary[row];

                    //Insert submeter data into data table
                    var mpxn = values[0];
                    var date = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));

                    for(var timePeriod = 2; timePeriod < columns.Count(); timePeriod++)
                    {
                        var dataRow = subMeterUsageDataTable.NewRow();
                        dataRow["ProcessQueueGUID"] = processQueueGUID;
                        dataRow["SubMeterIdentifier"] = mpxn;
                        dataRow["Date"] = date;
                        dataRow["Value"] = values[timePeriod];

                        subMeterUsageDataTable.Rows.Add(dataRow);
                    }
                }

                //Insert meter usage data into [Temp.Customer].[SubMeterUsage]
                _tempCustomerMethods.SubMeterUsage_Insert(subMeterUsageDataTable);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

