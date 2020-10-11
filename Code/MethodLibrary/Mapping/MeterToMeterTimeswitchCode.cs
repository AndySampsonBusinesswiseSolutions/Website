using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToMeterTimeswitchCode_Insert(long createdByUserId, long sourceId, long meterTimeswitchCodeId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterTimeswitchCode_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
            }
        }
    }
}