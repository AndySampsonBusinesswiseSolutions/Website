using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long GetAreaId(long createdByUserId, long sourceId, string area)
            {
                var areaId = Area_GetAreaIdByAreaDescription(area);

                if(areaId == 0)
                {
                    Area_Insert(createdByUserId, sourceId, area);
                    areaId = Area_GetAreaIdByAreaDescription(area);
                }

                return areaId;
            }

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

            public string Area_GetAreaDescriptionByAreaId(long areaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Area_GetByAreaId, 
                    areaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("AreaDescription"))
                    .FirstOrDefault();
            }

            public Dictionary<long, string> Area_GetAreaDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Area_GetList);

                return dataTable.AsEnumerable()
                    .ToDictionary(d => d.Field<long>("AreaId"), d => d.Field<string>("AreaDescription"));
            }
        }
    }
}