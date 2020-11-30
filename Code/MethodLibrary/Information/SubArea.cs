using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
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

            public long GetSubAreaId(string subArea, long createdByUserId, long sourceId)
            {
                var subAreaId = SubArea_GetSubAreaIdBySubAreaDescription(subArea);

                if(subAreaId == 0)
                {
                    SubArea_Insert(createdByUserId, sourceId, subArea);
                    subAreaId = SubArea_GetSubAreaIdBySubAreaDescription(subArea);
                }

                return subAreaId;
            }

            public void SubArea_Insert(long createdByUserId, long sourceId, string subAreaDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.SubArea_Insert, 
                    createdByUserId, sourceId, subAreaDescription);
            }

            public string SubArea_GetSubAreaDescriptionBySubAreaId(long subAreaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SubArea_GetBySubAreaId, 
                    subAreaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("SubAreaDescription"))
                    .FirstOrDefault();
            }

            public Dictionary<long, string> SubArea_GetSubAreaDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SubArea_GetList);

                return dataTable.AsEnumerable()
                    .ToDictionary(d => d.Field<long>("SubAreaId"), d => d.Field<string>("SubAreaDescription"));
            }
        }
    }
}