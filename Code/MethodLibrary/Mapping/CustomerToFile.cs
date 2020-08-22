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
            public void CustomerToFile_Insert(long createdByUserId, long sourceId, long customerId, long fileId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToFile_Insert, 
                    createdByUserId, sourceId, customerId, fileId);
            }
        }
    }
}