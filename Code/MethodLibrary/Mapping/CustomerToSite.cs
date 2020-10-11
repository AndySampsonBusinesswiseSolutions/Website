using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void CustomerToSite_Insert(long createdByUserId, long sourceId, long customerId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToSite_Insert, 
                    createdByUserId, sourceId, customerId, siteId);
            }
        }
    }
}