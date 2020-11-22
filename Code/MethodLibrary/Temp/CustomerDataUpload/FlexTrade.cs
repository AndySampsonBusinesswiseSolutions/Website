using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class TempSchema
        {
            public partial class CustomerDataUpload
            {
                public class FlexTrade
                {
                    public void FlexTrade_Insert(string processQueueGUID, int rowId, string basketReference, string tradeReference, string tradeDate, string tradeProduct, string volume, string price, string direction)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.FlexTrade_Insert, 
                            processQueueGUID, rowId, basketReference, tradeReference, tradeDate, tradeProduct, volume, price, direction);
                    }

                    public List<DataRow> FlexTrade_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FlexTrade_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.FlexTrade> FlexTrade_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FlexTrade_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.FlexTrade(d)).ToList();
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
}