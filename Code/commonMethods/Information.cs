using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class Information
        {
            public long SourceTypeId_GetBySourceTypeDescription(string sourceTypeDescription)
            {
                //Get Source Type Id
                var processDataTable = SourceType_GetBySourceTypeDescription(sourceTypeDescription);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("SourceTypeId"))
                            .FirstOrDefault();
            }

            public long SourceId_GetBySourceTypeIdAndSourceTypeEntityId(long sourceTypeId, long sourceTypeEntityId)
            {
                //Get Source Id
                var processDataTable = Source_GetBySourceTypeIdAndSourceTypeEntityId(sourceTypeId, sourceTypeEntityId);
                return processDataTable.AsEnumerable()
                            .Select(r => r.Field<long>("SourceId"))
                            .FirstOrDefault();
            }

            private DataTable SourceType_GetBySourceTypeDescription(string sourceTypeDescription)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@SourceTypeDescription", SqlValue = sourceTypeDescription}
                };

                //Execute stored procedure
                return _databaseInteraction.Get(_storedProcedureInformationEnums.SourceType_GetBySourceTypeDescription, sqlParameters);
            }

            private DataTable Source_GetBySourceTypeIdAndSourceTypeEntityId(long sourceTypeId, long sourceTypeEntityId)
            {
                //Set up stored procedure parameters
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@SourceTypeId", SqlValue = sourceTypeId},
                    new SqlParameter {ParameterName = "@SourceTypeEntityId", SqlValue = sourceTypeEntityId}
                };

                //Execute stored procedure
                return _databaseInteraction.Get(_storedProcedureInformationEnums.Source_GetBySourceTypeIdAndSourceTypeEntityId, sqlParameters);
            }
        }
    }
}
