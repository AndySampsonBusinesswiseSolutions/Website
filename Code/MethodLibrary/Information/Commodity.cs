using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long Commodity_GetCommodityIdByCommodityDescription(string commodityDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Commodity_GetByCommodityDescription, 
                    commodityDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityId"))
                    .FirstOrDefault();
            }

            public void Commodity_Insert(long createdByUserId, long sourceId, string commodityDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.Commodity_Insert, 
                    createdByUserId, sourceId, commodityDescription);
            }

            public string Commodity_GetCommodityDescriptionByCommodityId(long commodityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Commodity_GetByCommodityId, 
                    commodityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("CommodityDescription"))
                    .FirstOrDefault();
            }
        }
    }
}