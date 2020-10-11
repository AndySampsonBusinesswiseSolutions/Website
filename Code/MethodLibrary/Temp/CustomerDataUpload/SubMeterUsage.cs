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
                public List<DataRow> SubMeterUsage_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.SubMeterUsage_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void SubMeterUsage_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.SubMeterUsage_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}