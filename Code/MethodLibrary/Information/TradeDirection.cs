using System.Data;
using System.Linq;
using System.Reflection;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public string GetTradeDirection(string direction)
            {
                var informationTradeDirectionEnums = new Enums.InformationSchema.TradeDirection();
                return direction.StartsWith("B")
                    ? informationTradeDirectionEnums.Buy
                    : informationTradeDirectionEnums.Sell;
            }
            public long TradeDirection_GetTradeDirectionIdByTradeDirectionDescription(string tradeDirectionDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.TradeDirection_GetByTradeDirectionDescription, 
                    tradeDirectionDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeDirectionId"))
                    .FirstOrDefault();
            }
        }
    }
}