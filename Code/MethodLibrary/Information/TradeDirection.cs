using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
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