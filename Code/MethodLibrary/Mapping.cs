using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Mapping
        {
            public List<long> APIToProcess_GetAPIIdListByProcessId(long processId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.APIToProcess_GetByProcessId, 
                    processId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("APIId"))
                    .ToList();
            }

            public long PasswordToUser_GetPasswordFromJObjectToUserIdByPasswordIdAndUserId(long passwordId, long userId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.PasswordToUser_GetByPasswordIdAndUserId, 
                    passwordId, userId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PasswordToUserId"))
                    .FirstOrDefault();
            }

            public void LoginToUser_Insert(long createdByUserId, long sourceId, long loginId, long userId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.LoginToUser_Insert, 
                    createdByUserId, sourceId, loginId, userId);
            }

            public List<long> LoginToUser_GetLoginIdListByUserId(long userId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.LoginToUser_GetByUserId, 
                    userId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LoginId"))
                    .ToList();
            }

            public void ProcessToProcessArchive_Insert(long createdByUserId, long sourceId, long processId, long processArchiveId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ProcessToProcessArchive_Insert, 
                    createdByUserId, sourceId, processId, processArchiveId);
            }

            public List<long> APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(long APIId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.APIToProcessArchiveDetail_GetByAPIId, 
                    APIId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .ToList();
            }

            public void APIToProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long APIId, long processArchiveDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.APIToProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, APIId, processArchiveDetailId);
            }

            public Dictionary<long, List<long>> CustomerToChildCustomer_GetCustomerIdToChildCustomerIdDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetList);
                
                var dictionary = new Dictionary<long, List<long>>();

                foreach(DataRow r in dataTable.Rows)
                {
                    if(!dictionary.ContainsKey(r.Field<long>("CustomerId")))
                    {
                        dictionary.Add(r.Field<long>("CustomerId"), new List<long>());
                    }

                    dictionary[r.Field<long>("CustomerId")].Add(r.Field<long>("ChildCustomerId"));
                }

                return dictionary;
            }

            public List<long> CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(long customerId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetByCustomerId, 
                    customerId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ChildCustomerId"))
                    .ToList();
            }

            public void CustomerToChildCustomer_DeleteByCustomerIdAndChildCustomerId(long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId, 
                    customerId, childCustomerId);
            }

            public void CustomerToChildCustomer_Insert(long createdByUserId, long sourceId, long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToChildCustomer_Insert, 
                    createdByUserId, sourceId, customerId, childCustomerId);
            }

            public List<long> FolderToRootFolderType_GetFolderIdListByRootFolderTypeId(long rootFolderTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.FolderToRootFolderType_GetByRootFolderTypeId, 
                    rootFolderTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderId"))
                    .ToList();
            }

            public List<long> FolderToFolderExtension_GetFolderExtensionIdByFolderId(long folderId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.FolderToFolderExtension_GetByFolderId, 
                    folderId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderExtensionId"))
                    .ToList();
            }

            public List<long> FolderToFolderExtensionType_GetFolderIdListByFolderExtensionTypeId(long folderExtensionTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.FolderToFolderExtensionType_GetByFolderExtensionTypeId, 
                    folderExtensionTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FolderId"))
                    .ToList();
            }

            public void CustomerToFile_Insert(long createdByUserId, long sourceId, long customerId, long fileId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToFile_Insert, 
                    createdByUserId, sourceId, customerId, fileId);
            }

            public void FileToFileType_Insert(long createdByUserId, long sourceId, long fileId, long fileTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.FileToFileType_Insert, 
                    createdByUserId, sourceId, fileId, fileTypeId);
            }
        }
    }
}
