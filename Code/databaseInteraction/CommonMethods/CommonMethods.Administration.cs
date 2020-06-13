using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Administration
        {
            public void Login_Insert(DatabaseInteraction databaseInteraction, long userId, long sourceId, bool loginSuccessful, string processArchiveGUID)
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
                databaseInteraction.ExecuteNonQuery(_storedProcedureAdministrationEnums.Login_Insert, sqlParameters);
            }

            public long LoginId_GetByProcessArchiveGUID(DatabaseInteraction databaseInteraction, string processArchiveGUID)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessArchiveGUID", SqlValue = processArchiveGUID}
                };

                //Get Login Id
                var processDataTable = databaseInteraction.Get(_storedProcedureAdministrationEnums.Login_GetByProcessArchiveGUID, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("LoginId"))
                            .FirstOrDefault();
            }
        }
    }
}
