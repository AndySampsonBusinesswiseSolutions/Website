using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class Administration
        {
            public long Password_GetPasswordIdByPassword(string password)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.Password_GetByPassword, 
                    password);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PasswordId"))
                    .FirstOrDefault();
            }
            
            // public void UserDetail_Insert(long createdByUserId, long sourceId, long userId, long userattributeId, string userDetailDescription)
            public void UserDetail_Insert(string userGUID, string sourceTypeDescription, string UserAttributeDescription, string userDetailDescription)
            {
                //Set up stored procedure parameters
                // var sqlParameters = new List<SqlParameter>
                // {
                //     new SqlParameter {ParameterName = "@CreatedByUserId", SqlValue = createdByUserId},
                //     new SqlParameter {ParameterName = "@SourceId", SqlValue = sourceId},
                //     new SqlParameter {ParameterName = "@UserId", SqlValue = userId},
                //     new SqlParameter {ParameterName = "@UserattributeId", SqlValue = userattributeId},
                //     new SqlParameter {ParameterName = "@UserDetailDescription", SqlValue = userDetailDescription}
                // };

                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = sourceTypeDescription},
                    new SqlParameter {ParameterName = "@UserAttributeDescription", SqlValue = UserAttributeDescription},
                    new SqlParameter {ParameterName = "@UserDetailDescription", SqlValue = userDetailDescription}
                };

                //TODO: Refactor sproc to not run against multiple tables
                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureAdministrationEnums.UserDetail_Insert, sqlParameters);
            }
            
            public long User_GetUserIdByUserDetailId(long userDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.UserDetail_GetByUserDetailId, 
                    userDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserId"))
                    .FirstOrDefault();
            }
            
            public long UserDetail_GetUserDetailIdByEmailAddress(string emailAddress)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.UserDetail_GetByUserDetailDescription, 
                    emailAddress);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserDetailId"))
                    .FirstOrDefault();
            }

            public void Login_Insert(long userId, long sourceId, bool loginSuccessful, string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureAdministrationEnums.Login_Insert, 
                    userId, sourceId, loginSuccessful, processArchiveGUID);
            }

            public long Login_GetLoginIdByProcessArchiveGUID(string processArchiveGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.Login_GetByProcessArchiveGUID, 
                    processArchiveGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LoginId"))
                    .FirstOrDefault();
            }

            public bool Login_GetLoginSuccessfulByLoginId(long loginId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.Login_GetByLoginId, 
                    loginId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("LoginSuccessful"))
                    .FirstOrDefault();
            }
        }
    }
}
