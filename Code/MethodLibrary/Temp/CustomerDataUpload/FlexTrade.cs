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
                public void FlexTrade_Insert(string processQueueGUID, int rowId, string basketReference, string tradeReference, string tradeDate, string tradeProduct, string volume, string price, string direction)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.FlexTrade_Insert, 
                        processQueueGUID, rowId, basketReference, tradeReference, tradeDate, tradeProduct, volume, price, direction);
                }

                public List<DataRow> FlexTrade_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.FlexTrade_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void FlexTrade_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.FlexTrade_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}