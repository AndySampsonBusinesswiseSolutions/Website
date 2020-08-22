using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public void Trade_Insert(long createdByUserId, long sourceId, string tradeGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Trade_Insert, 
                    createdByUserId, sourceId, tradeGUID);
            }

            public long Trade_GetTradeIdByTradeGUID(string tradeGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Trade_GetByTradeGUID, 
                    tradeGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeId"))
                    .FirstOrDefault();
            }

            public void TradeDetail_Insert(long createdByUserId, long sourceId, long tradeId, long tradeAttributeId, string tradeDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.TradeDetail_Insert, 
                    createdByUserId, sourceId, tradeId, tradeAttributeId, tradeDetailDescription);
            }

            public DataRow TradeDetail_GetByTradeIdAndTradeAttributeId(long tradeId, long tradeAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.TradeDetail_GetByTradeIdAndTradeAttributeId, 
                    tradeId, tradeAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }

            public void TradeDetail_DeleteByTradeDetailId(long customerDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.TradeDetail_DeleteByTradeDetailId, 
                    customerDetailId);
            }

            public long TradeAttribute_GetTradeAttributeIdByTradeAttributeDescription(string tradeAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.TradeAttribute_GetByTradeAttributeDescription, 
                    tradeAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeAttributeId"))
                    .FirstOrDefault();
            }

            public long TradeDetail_GetTradeDetailIdByTradeIdAndTradeAttributeId(long tradeId, long tradeAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.TradeDetail_GetByTradeIdAndTradeAttributeId, 
                    tradeId, tradeAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeDetailId"))
                    .FirstOrDefault();
            }

            public long TradeDetail_GetTradeIdByTradeAttributeIdAndTradeDetailDescription(long tradeAttributeId, string tradeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.TradeDetail_GetByTradeAttributeIdAndTradeDetailDescription, 
                    tradeAttributeId, tradeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeId"))
                    .First();
            }

            public IEnumerable<DataRow> TradeDetail_GetListByTradeId(long tradeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.TradeDetail_GetByTradeId, 
                    tradeId);

                return dataTable.Rows.Cast<DataRow>();
            }
        }
    }
}
