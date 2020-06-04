using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Process
        {
            public long ProcessId_GetByGUID(DatabaseInteraction databaseInteraction, string guid)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@ProcessGUID", SqlValue = guid}
                };

                //Get Process Id
                var APIDataTable = databaseInteraction.Get("[System].[Process_GetByGUID]", sqlParameters);
                return APIDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("ProcessId"))
                            .FirstOrDefault();
            }

            public void ProcessQueue_Insert(DatabaseInteraction databaseInteraction, string queueGUID, string userGUID, string source, string apiGUID, bool hasError = false)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@GUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@UserGUID", SqlValue = userGUID},
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = source},
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = apiGUID},
                    new SqlParameter {ParameterName = "@HasError", SqlValue = Convert.ToByte(hasError)}
                };

                //Execute stored procedure
                databaseInteraction.ExecuteNonQuery("[System].[ProcessQueue_Insert]", sqlParameters);
            }

            public void ProcessQueue_Update(DatabaseInteraction databaseInteraction, string queueGUID, string apiGUID, bool hasError = false)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@GUID", SqlValue = queueGUID},
                    new SqlParameter {ParameterName = "@APIGUID", SqlValue = apiGUID},
                    new SqlParameter {ParameterName = "@HasError", SqlValue = Convert.ToByte(hasError)}
                };

                //Execute stored procedure
                databaseInteraction.ExecuteNonQuery("[System].[ProcessQueue_Update]", sqlParameters);
            }
        }
    }
}
