using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Password
        {
            public long PasswordId_GetByPassword(DatabaseInteraction databaseInteraction, string password)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@Password", SqlValue = password}
                };

                //Get Password Id
                var processDataTable = databaseInteraction.Get(_storedProcedureAdministrationEnums.Password_GetByPassword, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PasswordId"))
                            .FirstOrDefault();
            }
        }
    }
}
