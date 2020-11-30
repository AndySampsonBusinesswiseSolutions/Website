using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public long RateUnitToTradeDetail_GetRateUnitToTradeDetailIdByRateUnitIdAndTradeDetailId(long rateUnitId, long tradeDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.RateUnitToTradeDetail_GetByRateUnitIdAndTradeDetailId, 
                    rateUnitId, tradeDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateUnitToTradeDetailId"))
                    .FirstOrDefault();
            }

            public void RateUnitToTradeDetail_Insert(long createdByUserId, long sourceId, long rateUnitId, long tradeDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.RateUnitToTradeDetail_Insert, 
                    createdByUserId, sourceId, rateUnitId, tradeDetailId);
            }
        }
    }
}