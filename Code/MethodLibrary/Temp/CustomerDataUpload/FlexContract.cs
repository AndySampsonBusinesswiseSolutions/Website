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
                public void FlexContract_Insert(string processQueueGUID, int rowId, string contractReference, string basketReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateType, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.FlexContract_Insert, 
                        processQueueGUID, rowId, contractReference, basketReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateType, value);
                }

                public IEnumerable<DataRow> FlexContract_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.FlexContract_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void FlexContract_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.FlexContract_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}