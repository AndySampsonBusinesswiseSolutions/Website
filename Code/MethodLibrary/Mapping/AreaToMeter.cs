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

            public long AreaToMeter_GetAreaIdByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetByMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AreaId"))
                    .FirstOrDefault();
            }

            public List<long> AreaToMeter_GetMeterIdListByAreaId(long areaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetByAreaId, 
                    areaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .ToList();
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

            public Dictionary<long, List<long>> AreaToMeter_GetMeterToAreaDictionaryByMeterIdList(Dictionary<long, string> meterIdentifierDictionary)
            {
                var tuple = AreaToMeter_GetLatestTuple();

                return tuple.Where(t => meterIdentifierDictionary.ContainsKey(t.Item2)).Select(t => t.Item2).Distinct().ToDictionary(
                    t => t,
                    t => tuple.Where(t1 => t1.Item2 == t).Select(t1 => t1.Item1).Distinct().ToList()
                );
            }
        }
    }
}