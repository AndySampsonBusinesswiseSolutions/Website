using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Information
        {
            public long SourceType_GetSourceTypeIdBySourceTypeDescription(string sourceTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SourceType_GetBySourceTypeDescription, 
                    sourceTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceTypeId"))
                    .FirstOrDefault();
            }

            public long GetSystemUserGeneratedSourceId()
            {
                var sourceTypeId = SourceType_GetSourceTypeIdBySourceTypeDescription(_informationSourceTypeEnums.UserGenerated);
                return Source_GetSourceIdBySourceTypeIdAndSourceTypeEntityId(sourceTypeId, 0);
            }

            public long Source_GetSourceIdBySourceTypeIdAndSourceTypeEntityId(long sourceTypeId, long sourceTypeEntityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Source_GetBySourceTypeIdAndSourceTypeEntityId, 
                    sourceTypeId, sourceTypeEntityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceId"))
                    .FirstOrDefault();
            }
        }
    }
}
