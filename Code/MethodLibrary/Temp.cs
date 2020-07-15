using System.Reflection;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Temp
        {
            public class Customer
            {
                public void Site_Insert(string processQueueGUID, string customerGUID, string siteName, string siteAddress, string siteTown, string siteCounty, string sitePostCode)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Site_Insert, 
                        processQueueGUID, customerGUID, siteName, siteAddress, siteTown, siteCounty, sitePostCode);
                }

                public void Meter_Insert(string processQueueGUID, string customerGUID, string site, string MPXN, string profileClass, string meterTimeswitchClass, string lineLossFactorClass, string capacity, string localDistributionZone, string standardOfftakeQuantity, string annualUsage, string dayUsage, string nightUsage)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Meter_Insert, 
                        processQueueGUID, customerGUID, site, MPXN, profileClass, meterTimeswitchClass, lineLossFactorClass, capacity, localDistributionZone, standardOfftakeQuantity, annualUsage, dayUsage, nightUsage);
                }

                public void SubMeter_Insert(string processQueueGUID, string customerGUID, string MPXN, string subMeterIdentifier)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerEnums.Meter_Insert, 
                        processQueueGUID, customerGUID, MPXN, subMeterIdentifier);
                }

                public void MeterUsage_Insert(DataTable dataTable)
                {
                    _databaseInteraction.BulkInsert(dataTable, "[Temp.Customer].[MeterUsage]");
                }

                public void SubMeterUsage_Insert(DataTable dataTable)
                {
                    _databaseInteraction.BulkInsert(dataTable, "[Temp.Customer].[SubMeterUsage]");
                }
            }
        }
    }
}