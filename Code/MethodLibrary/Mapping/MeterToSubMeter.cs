using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToSubMeter_Insert(long createdByUserId, long sourceId, long meterId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSubMeter_Insert, 
                    createdByUserId, sourceId, meterId, subMeterId);
            }
        }
    }
}