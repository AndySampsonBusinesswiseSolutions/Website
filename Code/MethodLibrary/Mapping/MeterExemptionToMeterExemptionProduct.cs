using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterExemptionId, meterExemptionProductId);
            }
        }
    }
}