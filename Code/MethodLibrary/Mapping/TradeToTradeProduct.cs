using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long TradeToTradeProduct_GetTradeToTradeProductIdByTradeIdAndTradeProductId(long tradeId, long tradeProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeToTradeProduct_GetByTradeIdAndTradeProductId, 
                    tradeId, tradeProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeToTradeProductId"))
                    .FirstOrDefault();
            }

            public void TradeToTradeProduct_Insert(long createdByUserId, long sourceId, long tradeId, long tradeProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeToTradeProduct_Insert, 
                    createdByUserId, sourceId, tradeId, tradeProductId);
            }
        }
    }
}