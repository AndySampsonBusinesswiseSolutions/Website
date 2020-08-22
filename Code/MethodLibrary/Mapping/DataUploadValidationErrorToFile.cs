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
            public void DataUploadValidationErrorToFile_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorId, long fileId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.DataUploadValidationErrorToFile_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorId, fileId);
            }
        }
    }
}