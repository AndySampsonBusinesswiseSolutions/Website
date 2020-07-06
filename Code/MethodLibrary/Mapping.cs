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

            public long PasswordToUser_GetPasswordToUserIdByPasswordIdAndUserId(long passwordId, long userId)
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

            public void APIToProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long APIId, long processArchiveDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.APIToProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, APIId, processArchiveDetailId);
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
        }
    }
}
