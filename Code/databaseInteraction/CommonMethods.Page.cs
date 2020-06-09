using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Page
        {
            public long PageId_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@PageGUID", SqlValue = guid}
                };

                //Get Page Id
                var processDataTable = databaseInteraction.Get("[System].[Page_GetByGUID]", sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("PageId"))
                            .FirstOrDefault();
            }
        }
    }
}
