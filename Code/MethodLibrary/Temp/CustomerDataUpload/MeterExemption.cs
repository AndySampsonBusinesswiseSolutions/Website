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
                public void MeterExemption_Insert(string processQueueGUID, int rowId, string MPXN, string dateFrom, string dateTo, string exemptionProduct, string exemptionProportion)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.MeterExemption_Insert, 
                        processQueueGUID, rowId, MPXN, dateFrom, dateTo, exemptionProduct, exemptionProportion);
                }

                public IEnumerable<DataRow> MeterExemption_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.MeterExemption_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void MeterExemption_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.MeterExemption_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}