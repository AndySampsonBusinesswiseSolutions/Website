using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public IEnumerable<DataRow> GetCommitableRows(IEnumerable<DataRow> dataRows)
                {
                    return dataRows.Where(r => r.Field<bool>("CanCommit"));
                }

                public Dictionary<int, List<string>> ConvertCustomerDataUploadToDictionary(JObject jsonObject, string dataType)
                {
                    //Get File Content JSON
                    var fileJSON = new Information().FileContent_GetFileContentJSONByFileGUID(jsonObject);

                    //Strip out data not required
                    var sheetJSON = fileJSON.Children().FirstOrDefault(c => c.Path == "Sheets");
                    var dataJSON = sheetJSON.Values().FirstOrDefault(v => v.Path == $"{dataType}");
                    var validCells = dataJSON.Values().Children().Where(c => 
                            c.Path.Replace($"{dataType}.", string.Empty) != "!ref" 
                            && c.Path.Replace($"{dataType}.", string.Empty) != "!margins")
                        .ToList();
                    var cells = validCells.Where(c => !IsCustomerDataUploadHeaderRow(c.Parent)).ToList();
                    var columns = validCells.Where(c => IsCustomerDataUploadHeaderRow(c.Parent))
                        .Select(c => c.Path.Replace(GetCustomerDataUploadRow(c.Path).ToString(), string.Empty))
                        .Select(c => c.Replace($"{dataType}.", string.Empty))
                        .OrderBy(c => ConvertColumnToInteger(c))
                        .Select(c => $"{dataType}.{c}")
                        .ToList();

                    var dictionary = new Dictionary<int, List<string>>();

                    foreach(var cell in cells)
                    {
                        var row = GetCustomerDataUploadRow(cell.Path);
                        var columnIndex = columns.IndexOf(cell.Path.Replace(row.ToString(), string.Empty));

                        if(!dictionary.ContainsKey(row))
                        {
                            dictionary.Add(row, new List<string>());
                            foreach(var column in columns)
                            {
                                dictionary[row].Add(string.Empty);
                            }
                        }

                        var valueToken = cell.Children().First(c => ((Newtonsoft.Json.Linq.JProperty)c).Name == "v");
                        var value = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)valueToken).Value).Value.ToString();
                        dictionary[row][columnIndex] = value;
                    }

                    return dictionary;
                }

                private Int64 ConvertColumnToInteger(string column)
                {
                    var columnBytes = Encoding.ASCII.GetBytes(column);
                    var joinedBytes = string.Join(string.Empty, columnBytes);
                    var integer = Convert.ToInt64(joinedBytes);
                    return integer;
                }

                private bool IsCustomerDataUploadHeaderRow(JContainer parent)
                {
                    return GetCustomerDataUploadRow(((Newtonsoft.Json.Linq.JProperty)parent).Name) == 1;
                }

                private int GetCustomerDataUploadRow(string cell)
                {
                    return Convert.ToInt16(String.Join("", cell.Where(char.IsDigit)));
                }

                public IEnumerable<DataRow> CleanedUpDataTable(DataTable dataTable)
                {
                    var trimmedDataTable = TrimDataTable(dataTable);
                    var columns = trimmedDataTable.Columns
                        .Cast<DataColumn>()
                        .Where(c => c.ColumnName != _systemAPIRequiredDataKeyEnums.ProcessQueueGUID && c.ColumnName != "RowId")
                        .Select(c => c.ColumnName);
                        
                    return GetPopulatedDataRows(trimmedDataTable.Rows.Cast<DataRow>(), columns);
                }

                private IEnumerable<DataRow> GetPopulatedDataRows(IEnumerable<DataRow> dataRows, IEnumerable<string> columns)
                {
                    var emptyDataRows = new List<DataRow>();

                    foreach(var dataRow in dataRows)
                    {
                        var dataRowString = string.Empty;
                        foreach(var column in columns)
                        {
                            dataRowString = $"{dataRowString}{column}";
                        }

                        if(dataRowString == string.Empty)
                        {
                            emptyDataRows.Add(dataRow);
                        }
                    }

                    foreach(var emptyDataRow in emptyDataRows)
                    {
                        dataRows.ToList().Remove(emptyDataRow);
                    }

                    return dataRows;
                }

                public Dictionary<int, Dictionary<string, List<string>>> InitialiseRecordsDictionary(IEnumerable<DataRow> dataRows, Dictionary<string, string> columns)
                {
                    var records = new Dictionary<int, Dictionary<string, List<string>>>();
                    var rowIds = dataRows.Select(d => d.Field<int>("RowId")).Distinct();

                    foreach(var rowId in rowIds)
                    {
                        records.Add(rowId, new Dictionary<string, List<string>>());

                        foreach(var column in columns)
                        {
                            records[rowId].Add(column.Key, new List<string>());
                        }
                    }

                    return records;
                }

                public void GetMissingRecords(Dictionary<int, Dictionary<string, List<string>>> records, IEnumerable<DataRow> dataRows, Dictionary<string, string> columns)
                {
                    foreach(var column in columns)
                    {
                        var emptyRecords = dataRows.Where(c => string.IsNullOrWhiteSpace(c[column.Key].ToString()));

                        foreach(var emptyRecord in emptyRecords)
                        {
                            var rowId = Convert.ToInt32(emptyRecord["RowId"]);
                            if(!records[rowId][column.Key].Contains($"Required column {column.Value} has no value"))
                            {
                                records[rowId][column.Key].Add($"Required column {column.Value} has no value");
                            }
                        }
                    }
                }

                public string FinaliseValidation(Dictionary<int, Dictionary<string, List<string>>> records, string processQueueGUID, long createdByUserId, long sourceId, string sheetName, bool canUpdateValidRecords = true)
                {
                    //Split into two dictionaries
                    //Those rows with errors need to be inserted into [Customer].[DataUploadValidationError]
                    //Those rows without errors update [Temp.CustomerDataUpload].[sheetName].CanCommit to 1

                    //Error records
                    var errorRows = GetReturnRows(records, true);

                    //Update valid records
                    if(canUpdateValidRecords)
                    {
                        //Valid records
                        var validRows = GetReturnRows(records, false).Where(r => !errorRows.ContainsKey(r.Key)).ToDictionary(x => x.Key, x => x.Value);

                        UpdateCanCommit(processQueueGUID, sheetName, validRows, true);
                    }                    

                    //Insert error records
                    var customerMethods = new Methods.Customer();
                    customerMethods.InsertDataUploadValidationErrors(
                        processQueueGUID,
                        createdByUserId,
                        sourceId,
                        sheetName,
                        errorRows);
                    UpdateCanCommit(processQueueGUID, sheetName, errorRows, false);

                    return errorRows.Any() ? "Validation errors found" : null;
                }

                private Dictionary<int, Dictionary<string, List<string>>> GetReturnRows(Dictionary<int, Dictionary<string, List<string>>> records, bool hasError)
                {
                    var returnRows = new Dictionary<int, Dictionary<string, List<string>>>();
                    foreach (var (record, recordValue) in from record in records
                                                          from recordValue in
                                                              from recordValue in record.Value
                                                              where recordValue.Value.Any() == hasError
                                                              select recordValue
                                                          select (record, recordValue))
                    {
                        if (!returnRows.ContainsKey(record.Key))
                        {
                            returnRows.Add(record.Key, new Dictionary<string, List<string>>());
                        }

                        var returnRow = returnRows[record.Key];
                        if (!returnRow.ContainsKey(recordValue.Key))
                        {
                            returnRow.Add(recordValue.Key, recordValue.Value);
                        }
                    }

                    return returnRows;
                }

                public void UpdateCanCommit(string processQueueGUID, string sheetName, Dictionary<int, Dictionary<string, List<string>>> validRecords, bool canCommit)
                {
                    var storedProcedure = DetermineUpdateCanCommitStoredProcedureFromSheetName(sheetName);
                    var requiresSheetNameParameter = DetermineRequiresSheetNameParameter(sheetName);

                    foreach(var rowId in validRecords.Keys)
                    {
                        if(requiresSheetNameParameter)
                        {
                            CanCommit_Update(storedProcedure, processQueueGUID, sheetName, rowId, canCommit);
                        }
                        else
                        {
                            CanCommit_Update(storedProcedure, processQueueGUID, rowId, canCommit);
                        }
                    }
                }

                private string DetermineUpdateCanCommitStoredProcedureFromSheetName(string sheetName)
                {
                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Customer)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.Customer_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Site)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.Site_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Meter)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.Meter_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeter)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.SubMeter_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.MeterUsage
                        || sheetName == _customerDataUploadValidationSheetNameEnums.DailyMeterUsage)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.MeterUsage_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.MeterExemption)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.MeterExemption_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeterUsage)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.SubMeterUsage_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FixedContract)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.FixedContract_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexContract)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.FlexContract_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.FlexReferenceVolume_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexTrade)
                    {
                        return _storedProcedureTempCustomerDataUploadEnums.FlexTrade_UpdateCanCommit;
                    }                    

                    return string.Empty;
                }

                private bool DetermineRequiresSheetNameParameter(string sheetName)
                {
                    return sheetName == _customerDataUploadValidationSheetNameEnums.MeterUsage
                        || sheetName == _customerDataUploadValidationSheetNameEnums.DailyMeterUsage;
                }

                private void CanCommit_Update(string storedProcedure, string processQueueGUID, int rowId, bool canCommit)
                {
                    var parameterArray = MethodBase.GetCurrentMethod().GetParameters()
                        .Where(p => p.Name != "storedProcedure");

                    ExecuteNonQuery(parameterArray.ToArray(),
                        storedProcedure, 
                        processQueueGUID, rowId, canCommit);
                }

                private void CanCommit_Update(string storedProcedure, string processQueueGUID, string sheetName, int rowId, bool canCommit)
                {
                    var parameterArray = MethodBase.GetCurrentMethod().GetParameters()
                        .Where(p => p.Name != "storedProcedure");

                    ExecuteNonQuery(parameterArray.ToArray(),
                        storedProcedure, 
                        processQueueGUID, sheetName, rowId, canCommit);
                }
            }
        }
    }
}