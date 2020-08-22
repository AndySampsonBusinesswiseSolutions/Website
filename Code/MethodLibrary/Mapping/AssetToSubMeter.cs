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
            public void AssetToSubMeter_Insert(long createdByUserId, long sourceId, long assetId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AssetToSubMeter_Insert, 
                    createdByUserId, sourceId, assetId, subMeterId);
            }
        }
    }
}