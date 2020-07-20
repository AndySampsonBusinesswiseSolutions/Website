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
                public Dictionary<int, List<string>> ConvertCustomerDataUploadToDictionary(JObject jsonObject, string dataType)
                {
                    //Get File Content JSON
                    var fileJSON = new Information().FileContent_GetFileContentJSONByFileGUID(jsonObject);

                    //Strip out data not required
                    var sheetJSON = fileJSON.Children().FirstOrDefault(c => c.Path == "Sheets");
                    var dataTypeJSON = sheetJSON.Values().FirstOrDefault(v => v.Path == $"Sheets.Sheets['{dataType}']");
                    var validCells = dataTypeJSON.Values().Children().Where(c => 
                            c.Path.Replace($"Sheets.Sheets['{dataType}'].", string.Empty) != "!ref" 
                            && c.Path.Replace($"Sheets.Sheets['{dataType}'].", string.Empty) != "!margins")
                        .ToList();
                    var cells = validCells.Where(c => !IsCustomerDataUploadHeaderRow(c.Parent)).ToList();
                    var columns = validCells.Where(c => IsCustomerDataUploadHeaderRow(c.Parent))
                        .Select(c => c.Path.Replace(GetCustomerDataUploadRow(c.Path).ToString(), string.Empty))
                        .Select(c => c.Replace($"Sheets['{dataType}'].", string.Empty))
                        .OrderBy(c => Convert.ToInt64(string.Join(string.Empty, Encoding.ASCII.GetBytes(c))))
                        .Select(c => $"Sheets['{dataType}'].{c}")
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

                private bool IsCustomerDataUploadHeaderRow(JContainer parent)
                {
                    return GetCustomerDataUploadRow(((Newtonsoft.Json.Linq.JProperty)parent).Name) == 1;
                }

                private int GetCustomerDataUploadRow(string cell)
                {
                    return Convert.ToInt16(String.Join("", cell.Where(char.IsDigit)));
                }

                public void Site_Insert(string processQueueGUID, string siteName, string siteAddress, string siteTown, string siteCounty, string sitePostCode)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Site_Insert, 
                        processQueueGUID, siteName, siteAddress, siteTown, siteCounty, sitePostCode);
                }

                public void Meter_Insert(string processQueueGUID, string site, string MPXN, string profileClass, string meterTimeswitchClass, string lineLossFactorClass, string capacity, string localDistributionZone, string standardOfftakeQuantity, string annualUsage, string dayUsage, string nightUsage)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Meter_Insert, 
                        processQueueGUID, site, MPXN, profileClass, meterTimeswitchClass, lineLossFactorClass, capacity, localDistributionZone, standardOfftakeQuantity, annualUsage, dayUsage, nightUsage);
                }

                public void SubMeter_Insert(string processQueueGUID, string MPXN, string subMeterIdentifier)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.SubMeter_Insert, 
                        processQueueGUID, MPXN, subMeterIdentifier);
                }

                public void Customer_Insert(string processQueueGUID, string customerName, string contactName, string contactTelephoneNumber, string contactEmailAddress)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Customer_Insert, 
                        processQueueGUID, contactName, contactTelephoneNumber, contactEmailAddress);
                }

                public void FixedContract_Insert(string processQueueGUID, string contractReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateCount, string standingCharge, string capacityCharge, string rate, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FixedContract_Insert, 
                        processQueueGUID, contractReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateCount, standingCharge, capacityCharge, rate, value);
                }

                public void FlexContract_Insert(string processQueueGUID, string contractReference, string basketReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string standingCharge, string shapeFee, string adminFee, string imbalanceFee, string riskFee, string greenPremium)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexContract_Insert, 
                        processQueueGUID, contractReference, basketReference, MPXN, supplier, contractStartDate, contractEndDate, product, standingCharge, shapeFee, adminFee, imbalanceFee, riskFee, greenPremium);
                }

                public void FlexReferenceVolume_Insert(string processQueueGUID, string contractReference, string dateFrom, string dateTo, string volume)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexReferenceVolume_Insert, 
                        processQueueGUID, contractReference, dateFrom, dateTo, volume);
                }

                public void FlexTrade_Insert(string processQueueGUID, string basketReference, string tradeDate, string tradeProduct, string volume, string price, string direction)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.FlexTrade_Insert, 
                        processQueueGUID, basketReference, tradeDate, tradeProduct, volume, price, direction);
                }

                public void MeterExemption_Insert(string processQueueGUID, string MPXN, string dateFrom, string dateTo, string exemptionProduct, string exemptionProportion)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.MeterExemption_Insert, 
                        processQueueGUID, MPXN, dateFrom, dateTo, exemptionProduct, exemptionProportion);
                }

                public void MeterUsage_Insert(string processQueueGUID, string MPXN, string date, string timePeriod, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.MeterUsage_Insert, 
                        processQueueGUID, MPXN, date, timePeriod, value);
                }

                public void SubMeterUsage_Insert(string processQueueGUID, string subMeterIdentifier, string date, string timePeriod, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.SubMeterUsage_Insert, 
                        processQueueGUID, subMeterIdentifier, date, timePeriod, value);
                }
            }
        }
    }
}