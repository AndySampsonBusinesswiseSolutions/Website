using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long TradeToTradeDirection_GetTradeToTradeDirectionIdByTradeIdAndTradeDirectionId(long tradeId, long tradeDirectionId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeToTradeDirection_GetByTradeIdAndTradeDirectionId, 
                    tradeId, tradeDirectionId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeToTradeDirectionId"))
                    .FirstOrDefault();
            }

            public void TradeToTradeDirection_Insert(long createdByUserId, long sourceId, long tradeId, long tradeDirectionId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeToTradeDirection_Insert, 
                    createdByUserId, sourceId, tradeId, tradeDirectionId);
            }
        }
    }
}