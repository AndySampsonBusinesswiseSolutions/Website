using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using databaseInteraction;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class Administration
        {
            public long PasswordId_GetByPassword(DatabaseInteraction _databaseInteraction, string password)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@Password", SqlValue = password}
                };

                //Get Password Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureAdministrationEnums.Password_GetByPassword, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PasswordId"))
                            .FirstOrDefault();
            }
            
            // public void UserDetail_Insert(DatabaseInteraction _databaseInteraction, long createdByUserId, long sourceId, long userId, long userattributeId, string userDetailDescription)
            public void UserDetail_Insert(DatabaseInteraction _databaseInteraction, string userGUID, string sourceTypeDescription, string UserAttributeDescription, string userDetailDescription)
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
            
            public long UserId_GetByUserDetailId(DatabaseInteraction _databaseInteraction, long userDetailId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailId", SqlValue = userDetailId}
                };

                //Get User Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureAdministrationEnums.UserDetail_GetByUserDetailId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserId"))
                            .FirstOrDefault();
            }
            
            public long UserDetailId_GetByEmailAddress(DatabaseInteraction _databaseInteraction, string emailAddress)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailDescription", SqlValue = emailAddress}
                };

                //Get EmailAddress Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureAdministrationEnums.UserDetail_GetByUserDetailDescription, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserDetailId"))
                            .FirstOrDefault();
            }

            public void Login_Insert(DatabaseInteraction _databaseInteraction, long userId, long sourceId, bool loginSuccessful, string processArchiveGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserId", SqlValue = userId},
                    new SqlParameter {ParameterName = "@SourceId", SqlValue = sourceId},
                    new SqlParameter {ParameterName = "@LoginSuccessful", SqlValue = loginSuccessful},
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureAdministrationEnums.Login_Insert, sqlParameters);
            }

            public long LoginId_GetByProcessArchiveGUID(DatabaseInteraction _databaseInteraction, string processArchiveGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID}
                };

                //Get Login Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureAdministrationEnums.Login_GetByProcessArchiveGUID, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("LoginId"))
                            .FirstOrDefault();
            }

            public bool LoginSuccessful_GetByLoginId(DatabaseInteraction _databaseInteraction, long loginId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@LoginId", SqlValue = loginId}
                };

                //Get Login Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureAdministrationEnums.Login_GetByLoginId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<bool>("LoginSuccessful"))
                            .FirstOrDefault();
            }
        }
    }
}
