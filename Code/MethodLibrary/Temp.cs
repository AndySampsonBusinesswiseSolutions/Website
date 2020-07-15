using System.Reflection;

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
            }
        }
    }
}