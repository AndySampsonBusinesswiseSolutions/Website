using System.Reflection;
using System.Data;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public void Site_Insert(string processQueueGUID, int rowId, string customerName, string siteName, string siteAddress, string siteTown, string siteCounty, string sitePostCode, string siteDescription, string contactName, string contactRole, string contactTelephoneNumber, string contactEmailAddress)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.Site_Insert, 
                        processQueueGUID, rowId, customerName, siteName, siteAddress, siteTown, siteCounty, sitePostCode, siteDescription, contactName, contactRole, contactTelephoneNumber, contactEmailAddress);
                }

                public List<DataRow> Site_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.Site_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void Site_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.Site_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}