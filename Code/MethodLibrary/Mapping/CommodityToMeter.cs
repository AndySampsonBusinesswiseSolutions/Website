using System.Data;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void CommodityToMeter_Insert(long createdByUserId, long sourceId, long commodityId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CommodityToMeter_Insert, 
                    createdByUserId, sourceId, commodityId, meterId);
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
        }
    }
}