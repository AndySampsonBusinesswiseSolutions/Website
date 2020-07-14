using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Information
        {
            public long SourceAttribute_GetSourceAttributeIdBySourceAttributeDescription(string sourceAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SourceAttribute_GetBySourceAttributeDescription, 
                    sourceAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceAttributeId"))
                    .FirstOrDefault();
            }

            public long GetSystemUserGeneratedSourceId()
            {
                var systemUserId = new Administration().User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
                var sourceAttributeId = SourceAttribute_GetSourceAttributeIdBySourceAttributeDescription(_informationSourceAttributeEnums.UserGenerated);

                return SourceDetail_GetSourceIdBySourceAttributeIdAndSourceDetailDescription(sourceAttributeId, systemUserId.ToString());
            }

            public long SourceDetail_GetSourceIdBySourceAttributeIdAndSourceDetailDescription(long sourceAttributeId, string sourceDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription, 
                    sourceAttributeId, sourceDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceId"))
                    .FirstOrDefault();
            }

            public void File_Insert(long createdByUserId, long sourceId, string fileGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.File_Insert, 
                    createdByUserId, sourceId, fileGUID);
            }

            public long File_GetFileIdByFileGUID(string fileGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.File_GetByFileGUID, 
                    fileGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileId"))
                    .FirstOrDefault();
            }

            public long FileAttribute_GetFileAttributeIdByFileAttributeDescription(string fileAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FileAttribute_GetByFileAttributeDescription, 
                    fileAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileAttributeId"))
                    .FirstOrDefault();
            }

            public void FileDetail_Insert(long createdByUserId, long sourceId, long fileId, long fileAttributeId, string fileDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.FileDetail_Insert, 
                    createdByUserId, sourceId, fileId, fileAttributeId, fileDetailDescription);
            }

            public void FileContent_Insert(long createdByUserId, long sourceId, long fileId, string fileContent)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.FileContent_Insert, 
                    createdByUserId, sourceId, fileId, fileContent);
            }

            public long FileType_GetFileTypeIdByFileTypeDescription(string fileTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FileType_GetByFileTypeDescription, 
                    fileTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileTypeId"))
                    .FirstOrDefault();
            }
        }
    }
}
