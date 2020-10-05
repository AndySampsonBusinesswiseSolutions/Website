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
            public void AssetToSubMeter_Insert(long createdByUserId, long sourceId, long assetId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AssetToSubMeter_Insert, 
                    createdByUserId, sourceId, assetId, subMeterId);
            }

            public List<DataRow> AssetToSubMeter_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AssetToSubMeter_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long>> AssetToSubMeter_GetLatestTuple()
            {
                var dataRows = AssetToSubMeter_GetList();

                var tuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["AssetId"], (long)r["SubMeterId"]);
                    tuple.Add(tup);
                }

                return tuple;
            }
        }
    }
}