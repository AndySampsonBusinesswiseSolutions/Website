using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void LocalDistributionZoneToMeter_Insert(long createdByUserId, long sourceId, long localDistributionZoneId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.LocalDistributionZoneToMeter_Insert, 
                    createdByUserId, sourceId, localDistributionZoneId, meterId);
            }
        }
    }
}