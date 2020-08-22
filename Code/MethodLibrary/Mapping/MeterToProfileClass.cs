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
            public void MeterToProfileClass_Insert(long createdByUserId, long sourceId, long profileClassId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToProfileClass_Insert, 
                    createdByUserId, sourceId, profileClassId, meterId);
            }
        }
    }
}