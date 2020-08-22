using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void GridSupplyPointToMeter_Insert(long createdByUserId, long sourceId, long gridSupplyPointId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.GridSupplyPointToMeter_Insert, 
                    createdByUserId, sourceId, gridSupplyPointId, meterId);
            }
        }
    }
}