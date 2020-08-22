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
                public void FixedContract_Insert(string processQueueGUID, int rowId, string contractReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateCount, string rateType, string value)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.FixedContract_Insert, 
                        processQueueGUID, rowId, contractReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateCount, rateType, value);
                }

                public IEnumerable<DataRow> FixedContract_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.FixedContract_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }
            }
        }
    }
}