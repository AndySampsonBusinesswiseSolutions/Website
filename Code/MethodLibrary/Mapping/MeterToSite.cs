using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToSite_Insert(long createdByUserId, long sourceId, long meterId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSite_Insert, 
                    createdByUserId, sourceId, meterId, siteId);
            }
        }
    }
}