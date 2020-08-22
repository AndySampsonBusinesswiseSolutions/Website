using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long BasketToTrade_GetBasketToTradeIdByBasketIdAndTradeId(long basketId, long tradeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToTrade_GetByBasketIdAndTradeId, 
                    basketId, tradeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketToTradeId"))
                    .FirstOrDefault();
            }

            public void BasketToTrade_Insert(long createdByUserId, long sourceId, long basketId, long tradeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.BasketToTrade_Insert, 
                    createdByUserId, sourceId, basketId, tradeId);
            }
        }
    }
}