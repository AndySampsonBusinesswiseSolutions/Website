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
            public void SubAreaToSubMeter_Insert(long createdByUserId, long sourceId, long subSubAreaId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.SubAreaToSubMeter_Insert, 
                    createdByUserId, sourceId, subSubAreaId, subMeterId);
            }
        }
    }
}