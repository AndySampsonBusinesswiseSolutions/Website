using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class EmailAddress
        {
            public long EmailAddressId_GetByValue(DatabaseInteraction databaseInteraction, string emailAddress)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailDescription", SqlValue = emailAddress}
                };

                //Get EmailAddress Id
                var processDataTable = databaseInteraction.Get("[Administration.User].[UserDetail_GetByUserDetailDescription]", sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserDetailId"))
                            .FirstOrDefault();
            }
        }
    }
}
