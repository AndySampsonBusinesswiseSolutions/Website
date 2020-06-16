using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using databaseInteraction;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class Mapping
        {
            public long PasswordToUser_GetByPasswordIdAndUserId(DatabaseInteraction _databaseInteraction, long passwordId, long userId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@PasswordId", SqlValue = passwordId},
                    new SqlParameter {ParameterName = "@UserId", SqlValue = userId}
                };

                //Get Mapping Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureMappingEnums.PasswordToUser_GetByPasswordIdAndUserId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PasswordToUserId"))
                            .FirstOrDefault();
            }

            public void LoginToUser_Insert(DatabaseInteraction _databaseInteraction, long createdByUserId, long sourceId, long loginId, long userId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@CreatedByUserId", SqlValue = createdByUserId},
                    new SqlParameter {ParameterName = "@SourceId", SqlValue = sourceId},
                    new SqlParameter {ParameterName = "@LoginId", SqlValue = loginId},
                    new SqlParameter {ParameterName = "@UserId", SqlValue = userId}
                };

                //Execute stored procedure
                _databaseInteraction.ExecuteNonQuery(_storedProcedureMappingEnums.LoginToUser_Insert, sqlParameters);
            }

            public List<long> Login_GetByUserId(DatabaseInteraction _databaseInteraction, long userId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserId", SqlValue = userId}
                };

                //Get Login Id
                var processDataTable = _databaseInteraction.Get(_storedProcedureMappingEnums.LoginToUser_GetByUserId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("LoginId"))
                            .ToList();
            }
        }
    }
}
