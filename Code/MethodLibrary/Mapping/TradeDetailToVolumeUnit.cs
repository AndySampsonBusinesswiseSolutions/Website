using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long TradeDetailToVolumeUnit_GetTradeDetailToVolumeUnitIdByTradeDetailIdAndVolumeUnitId(long tradeDetailId, long volumeUnitId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeDetailToVolumeUnit_GetByTradeDetailIdAndVolumeUnitId, 
                    tradeDetailId, volumeUnitId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeDetailToVolumeUnitId"))
                    .FirstOrDefault();
            }

            public void TradeDetailToVolumeUnit_Insert(long createdByUserId, long sourceId, long tradeDetailId, long volumeUnitId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeDetailToVolumeUnit_Insert, 
                    createdByUserId, sourceId, tradeDetailId, volumeUnitId);
            }
        }
    }
}