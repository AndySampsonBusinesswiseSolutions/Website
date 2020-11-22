using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long TradeProduct_GetTradeProductIdByTradeProductDescription(string tradeProductDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.TradeProduct_GetByTradeProductDescription, 
                    tradeProductDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeProductId"))
                    .FirstOrDefault();
            }
        }
    }
}