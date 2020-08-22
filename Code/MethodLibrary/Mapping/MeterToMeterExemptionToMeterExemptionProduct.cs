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
            public void MeterToMeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterToMeterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterToMeterExemptionId, meterExemptionProductId);
            }
        }
    }
}