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

            public long RootFolderType_GetRootFolderIdByRootFolderTypeDescription(string rootFolderDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RootFolderType_GetByRootFolderTypeDescription, 
                    rootFolderDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RootFolderId"))
                    .FirstOrDefault();
            }

            public long FolderAttribute_GetFolderAttributeIdByFolderAttributeDescription(string folderAttribute)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FolderAttribute_GetByFolderAttributeDescription, 
                    folderAttribute);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderAttributeId"))
                    .FirstOrDefault();
            }

            public string FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(long folderId, long folderAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveDetailId, 
                    folderId, folderAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("FolderDetailDescription"))
                    .First();
            }
        }
    }
}
