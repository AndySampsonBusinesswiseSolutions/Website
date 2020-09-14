using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long CommodityToProfile_GetCommodityIdByProfileId(long profileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToProfile_GetByProfileId, 
                    profileId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityId"))
                    .FirstOrDefault();
            }
        }
    }
}