using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace databaseInteraction
{
    public partial class CommonMethods
    {
        public class Information
        {
            public long SourceTypeId_GetBySourceTypeDescription(DatabaseInteraction databaseInteraction, string sourceTypeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = sourceTypeDescription}
                };

                //Get Source Type Id
                var processDataTable = databaseInteraction.Get(_storedProcedureInformationEnums.SourceType_GetBySourceTypeDescription, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("SourceTypeId"))
                            .FirstOrDefault();
            }

            public long Source_GetBySourceTypeIdAndSourceTypeEntityId(DatabaseInteraction databaseInteraction, long sourceTypeId, long sourceTypeEntityId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@SourceTypeId", SqlValue = sourceTypeId},
                    new SqlParameter {ParameterName = "@SourceTypeEntityId", SqlValue = sourceTypeEntityId}
                };

                //Get Source Id
                var processDataTable = databaseInteraction.Get(_storedProcedureInformationEnums.Source_GetBySourceTypeIdAndSourceTypeEntityId, sqlParameters);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("SourceId"))
                            .FirstOrDefault();
            }
        }
    }
}
