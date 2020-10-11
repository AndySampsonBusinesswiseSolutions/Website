using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void AreaToMeter_Insert(long createdByUserId, long sourceId, long areaId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AreaToMeter_Insert, 
                    createdByUserId, sourceId, areaId, meterId);
            }

            public List<DataRow> AreaToMeter_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long>> AreaToMeter_GetLatestTuple()
            {
                var dataRows = AreaToMeter_GetList();

                var tuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["AreaId"], (long)r["MeterId"]);
                    tuple.Add(tup);
                }

                return tuple;
            }
        }
    }
}