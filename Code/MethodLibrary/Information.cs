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

            public long RootFolderType_GetRootFolderIdByRootFolderTypeDescription(string rootFolderTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RootFolderType_GetByRootFolderTypeDescription, 
                    rootFolderTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RootFolderTypeId"))
                    .FirstOrDefault();
            }

            public long FolderExtensionType_GetFolderExtensionTypeIdByFolderExtensionTypeDescription(string folderExtensionTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FolderExtensionType_GetByFolderExtensionTypeDescription, 
                    folderExtensionTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderExtensionTypeId"))
                    .FirstOrDefault();
            }

            public long FolderAttribute_GetFolderAttributeIdByFolderAttributeDescription(string folderAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FolderAttribute_GetByFolderAttributeDescription, 
                    folderAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderAttributeId"))
                    .FirstOrDefault();
            }

            public string FolderDetail_GetFolderDetailDescriptionListByFolderIdAndFolderAttributeId(long folderId, long folderAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FolderDetail_GetByFolderIdAndFolderAttributeId, 
                    folderId, folderAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("FolderDetailDescription"))
                    .First();
            }
        }
    }
}
