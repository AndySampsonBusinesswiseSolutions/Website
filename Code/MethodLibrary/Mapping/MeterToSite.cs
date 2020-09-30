using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToSite_Insert(long createdByUserId, long sourceId, long meterId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSite_Insert, 
                    createdByUserId, sourceId, meterId, siteId);
            }

            public List<DataRow> MeterToSite_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSite_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long>> MeterToSite_GetLatestTuple()
            {
                var dataRows = MeterToSite_GetList();

                var tuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["MeterId"], (long)r["SiteId"]);
                    tuple.Add(tup);
                }

                return tuple;
            }

            public Dictionary<long, List<long>> MeterToSite_GetSiteToMeterDictionaryBySiteIdList(Dictionary<long, string> siteNameDictionary)
            {
                var tuple = MeterToSite_GetLatestTuple();

                return tuple.Where(t => siteNameDictionary.ContainsKey(t.Item2)).Select(t => t.Item2).Distinct().ToDictionary(
                    t => t,
                    t => tuple.Where(t1 => t1.Item2 == t).Select(t1 => t1.Item1).Distinct().ToList()
                );
            }
        }
    }
}