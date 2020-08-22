using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long Area_GetAreaIdByAreaDescription(string areaDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Area_GetByAreaDescription, 
                    areaDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AreaId"))
                    .FirstOrDefault();
            }

            public void Area_Insert(long createdByUserId, long sourceId, string areaDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.Area_Insert, 
                    createdByUserId, sourceId, areaDescription);
            }
        }
    }
}