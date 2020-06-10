using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class UserDetail
        {
            public long UserDetailId_GetByEmailAddress(DatabaseInteraction databaseInteraction, string emailAddress)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailDescription", SqlValue = emailAddress}
                };

                //Get EmailAddress Id
                var processDataTable = databaseInteraction.Get(_storedProcedureAdministrationEnums.UserDetail_GetByUserDetailDescription, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserDetailId"))
                            .FirstOrDefault();
            }
            
            public long UserId_GetByUserDetailId(DatabaseInteraction databaseInteraction, long userDetailId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailId", SqlValue = userDetailId}
                };

                //Get User Id
                var processDataTable = databaseInteraction.Get(_storedProcedureAdministrationEnums.UserDetail_GetByUserDetailId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserId"))
                            .FirstOrDefault();
            }
        }
    }
}
