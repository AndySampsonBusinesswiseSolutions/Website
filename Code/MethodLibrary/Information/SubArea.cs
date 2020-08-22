using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long SubArea_GetSubAreaIdBySubAreaDescription(string subAreaDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SubArea_GetBySubAreaDescription, 
                    subAreaDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubAreaId"))
                    .FirstOrDefault();
            }

            public void SubArea_Insert(long createdByUserId, long sourceId, string subAreaDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.SubArea_Insert, 
                    createdByUserId, sourceId, subAreaDescription);
            }
        }
    }
}