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
            }
        }
    }
}