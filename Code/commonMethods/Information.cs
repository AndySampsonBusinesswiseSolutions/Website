using System.Data;
using System.Linq;
using System.Reflection;

namespace commonMethods
{
    public partial class CommonMethods
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

            public long SourceId_GetSourceIdBySourceTypeIdAndSourceTypeEntityId(long sourceTypeId, long sourceTypeEntityId)
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
