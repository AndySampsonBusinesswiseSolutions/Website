using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public long ProfileToProfileClass_GetProfileClassIdByProfileId(long profileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ProfileToProfileClass_GetByProfileId, 
                    profileId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassId"))
                    .FirstOrDefault();
            }
        }
    }
}