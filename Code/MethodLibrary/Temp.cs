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
        public class Temp
        {
            public class Customer
            {
                public IEnumerable<DataRow> GetCommitableRows(IEnumerable<DataRow> dataRows)
                {
                    return dataRows.Where(r => r.Field<string>("CanCommit") == "1");
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

                public void Site_Insert(string processQueueGUID, int rowId, string customerName, string siteName, string siteAddress, string siteTown, string siteCounty, string sitePostCode, string siteDescription, string contactName, string contactRole, string contactTelephoneNumber, string contactEmailAddress)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Site_Insert, 
                        processQueueGUID, rowId, customerName, siteName, siteAddress, siteTown, siteCounty, sitePostCode, siteDescription, contactName, contactRole, contactTelephoneNumber, contactEmailAddress);
                }

                public void Meter_Insert(string processQueueGUID, int rowId, string siteName, string MPXN, string gridSupplyPoint, string profileClass, string meterTimeswitchCode, string lineLossFactorClass, string capacity, string localDistributionZone, string standardOfftakeQuantity, string annualUsage, string meterSerialNumber, string area, string importExport)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Meter_Insert, 
                        processQueueGUID, rowId, siteName, MPXN, gridSupplyPoint, profileClass, meterTimeswitchCode, lineLossFactorClass, capacity, localDistributionZone, standardOfftakeQuantity, annualUsage, meterSerialNumber, area, importExport);
                }

                public void SubMeter_Insert(string processQueueGUID, int rowId, string MPXN, string subMeterIdentifier, string serialNumber, string subArea, string asset)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.SubMeter_Insert, 
                        processQueueGUID, rowId, MPXN, subMeterIdentifier, serialNumber, subArea, asset);
                }

                public void Customer_Insert(string processQueueGUID, int rowId, string customerName, string contactName, string contactTelephoneNumber, string contactEmailAddress)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Customer_Insert, 
                        processQueueGUID, rowId, customerName, contactName, contactTelephoneNumber, contactEmailAddress);
                }

                public void FixedContract_Insert(string processQueueGUID, int rowId, string contractReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateCount, string rateType, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FixedContract_Insert, 
                        processQueueGUID, rowId, contractReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateCount, rateType, value);
                }

                public void FlexContract_Insert(string processQueueGUID, int rowId, string contractReference, string basketReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateType, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexContract_Insert, 
                        processQueueGUID, rowId, contractReference, basketReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateType, value);
                }

                public void FlexReferenceVolume_Insert(string processQueueGUID, int rowId, string contractReference, string dateFrom, string dateTo, string volume)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexReferenceVolume_Insert, 
                        processQueueGUID, rowId, contractReference, dateFrom, dateTo, volume);
                }

                public void FlexTrade_Insert(string processQueueGUID, int rowId, string basketReference, string tradeReference, string tradeDate, string tradeProduct, string volume, string price, string direction)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexTrade_Insert, 
                        processQueueGUID, rowId, basketReference, tradeReference, tradeDate, tradeProduct, volume, price, direction);
                }

                public void MeterExemption_Insert(string processQueueGUID, int rowId, string MPXN, string dateFrom, string dateTo, string exemptionProduct, string exemptionProportion)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.MeterExemption_Insert, 
                        processQueueGUID, rowId, MPXN, dateFrom, dateTo, exemptionProduct, exemptionProportion);
                }

                public void MeterUsage_Insert(string processQueueGUID, int rowId, string MPXN, string date, string timePeriod, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.MeterUsage_Insert, 
                        processQueueGUID, rowId, MPXN, date, timePeriod, value);
                }

                public void SubMeterUsage_Insert(string processQueueGUID, int rowId, string subMeterIdentifier, string date, string timePeriod, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.SubMeterUsage_Insert, 
                        processQueueGUID, rowId, subMeterIdentifier, date, timePeriod, value);
                }

                public IEnumerable<DataRow> Site_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.Site_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> Meter_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.Meter_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> SubMeter_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.SubMeter_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> Customer_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.Customer_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> FixedContract_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.FixedContract_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> FlexContract_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.FlexContract_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> FlexReferenceVolume_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.FlexReferenceVolume_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> FlexTrade_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.FlexTrade_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> MeterExemption_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.MeterExemption_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> MeterUsage_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.MeterUsage_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public IEnumerable<DataRow> SubMeterUsage_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerEnums.SubMeterUsage_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
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
                    var newProcessQueueGUID = $"{DetermineUpdateCanCommitStoredProcedureFromSheetName(sheetName)}|{processQueueGUID}";

                    foreach(var rowId in validRecords.Keys)
                    {
                        CanCommit_Update(newProcessQueueGUID, rowId, canCommit);
                    }
                }

                private string DetermineUpdateCanCommitStoredProcedureFromSheetName(string sheetName)
                {
                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Customer)
                    {
                        return _storedProcedureTempCustomerEnums.Customer_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Site)
                    {
                        return _storedProcedureTempCustomerEnums.Site_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.Meter)
                    {
                        return _storedProcedureTempCustomerEnums.Meter_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeter)
                    {
                        return _storedProcedureTempCustomerEnums.SubMeter_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.MeterUsage)
                    {
                        return _storedProcedureTempCustomerEnums.MeterUsage_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.MeterExemption)
                    {
                        return _storedProcedureTempCustomerEnums.MeterExemption_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.SubMeterUsage)
                    {
                        return _storedProcedureTempCustomerEnums.SubMeterUsage_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FixedContract)
                    {
                        return _storedProcedureTempCustomerEnums.FixedContract_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexContract)
                    {
                        return _storedProcedureTempCustomerEnums.FlexContract_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume)
                    {
                        return _storedProcedureTempCustomerEnums.FlexReferenceVolume_UpdateCanCommit;
                    }

                    if(sheetName == _customerDataUploadValidationSheetNameEnums.FlexTrade)
                    {
                        return _storedProcedureTempCustomerEnums.FlexTrade_UpdateCanCommit;
                    }                    

                    return string.Empty;
                }

                private void CanCommit_Update(string processQueueGUID, int rowId, bool canCommit)
                {
                    var processQueueGUIDArray = processQueueGUID.Split('|');
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        processQueueGUIDArray[0], 
                        processQueueGUIDArray[1], rowId, canCommit);
                }
            }
        }
    }
}