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
                public List<DataRow> MeterUsage_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.MeterUsage_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void MeterUsage_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.MeterUsage_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}