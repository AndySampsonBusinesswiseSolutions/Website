using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long VolumeUnit_GetVolumeUnitIdByVolumeUnitDescription(string volumeUnitDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.VolumeUnit_GetByVolumeUnitDescription, 
                    volumeUnitDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("VolumeUnitId"))
                    .FirstOrDefault();
            }
        }
    }
}