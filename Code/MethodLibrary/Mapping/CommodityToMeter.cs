using System.Data;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void CommodityToMeter_Insert(long createdByUserId, long sourceId, long commodityId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CommodityToMeter_Insert, 
                    createdByUserId, sourceId, commodityId, meterId);
            }

            public long CommodityToMeter_GetCommodityToMeterIdByCommodityIdAndMeterId(long commodityId, long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToMeter_GetByCommodityIdAndMeterId, 
                    commodityId, meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityToMeterId"))
                    .FirstOrDefault();
            }

            public long CommodityToMeter_GetCommodityIdByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToMeter_GetByMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityId"))
                    .FirstOrDefault();
            }

            public List<long> CommodityToMeter_GetMeterIdByCommodityId(long commodityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToMeter_GetByCommodityId, 
                    commodityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .ToList();
            }

            public List<DataRow> CommodityToMeter_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToMeter_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long>> CommodityToMeter_GetLatestTuple()
            {
                var dataRows = CommodityToMeter_GetList();

                var tuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["CommodityId"], (long)r["MeterId"]);
                    tuple.Add(tup);
                }

                return tuple;
            }
            
            public Dictionary<long, List<long>> CommodityToMeter_GetMeterToCommodityDictionaryByMeterIdList(Dictionary<long, string> meterIdentifierDictionary)
            {
                var tuple = CommodityToMeter_GetLatestTuple();

                return tuple.Where(t => meterIdentifierDictionary.ContainsKey(t.Item2)).Select(t => t.Item2).Distinct().ToDictionary(
                    t => t,
                    t => tuple.Where(t1 => t1.Item2 == t).Select(t1 => t1.Item1).Distinct().ToList()
                );
            }
        }
    }
}