using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void FileToFileType_Insert(long createdByUserId, long sourceId, long fileId, long fileTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.FileToFileType_Insert, 
                    createdByUserId, sourceId, fileId, fileTypeId);
            }
        }
    }
}