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
            public long UserId_GetByUserDetailId(DatabaseInteraction databaseInteraction, long userDetailId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@UserDetailId", SqlValue = userDetailId}
                };

                //Get User Id
                var processDataTable = databaseInteraction.Get("[Administration.User].[UserDetail_GetByUserDetailId]", sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("UserId"))
                            .FirstOrDefault();
            }
        }
    }
}
