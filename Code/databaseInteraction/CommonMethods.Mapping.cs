using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Mapping
        {
            public long PasswordToUser_GetByPasswordIdAndUserId(DatabaseInteraction databaseInteraction, long passwordId, long userId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@PasswordId", SqlValue = passwordId},
                    new SqlParameter {ParameterName = "@UserId", SqlValue = userId}
                };

                //Get Mapping Id
                var processDataTable = databaseInteraction.Get("[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]", sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PasswordToUserId"))
                            .FirstOrDefault();
            }
        }
    }
}
