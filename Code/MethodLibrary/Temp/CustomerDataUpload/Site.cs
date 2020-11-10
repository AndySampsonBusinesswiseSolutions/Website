using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public class Site
                {
                    public void Site_Insert(string processQueueGUID, int rowId, string customerName, string siteName, string siteAddress, string siteTown, string siteCounty, string sitePostCode, string siteDescription, string contactName, string contactRole, string contactTelephoneNumber, string contactEmailAddress)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.Site_Insert, 
                            processQueueGUID, rowId, customerName, siteName, siteAddress, siteTown, siteCounty, sitePostCode, siteDescription, contactName, contactRole, contactTelephoneNumber, contactEmailAddress);
                    }

                    public List<DataRow> Site_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.Site_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.Temp.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.Site> Site_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.Site_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.Temp.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.Site(d)).ToList();
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
}