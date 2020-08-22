using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<long> BasketToContractMeter_GetContractMeterIdListByBasketId(long basketId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToContractMeter_GetByBasketId, 
                    basketId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }

            public void BasketToContractMeter_Insert(long createdByUserId, long sourceId, long basketId, long contractMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.BasketToContractMeter_Insert, 
                    createdByUserId, sourceId, basketId, contractMeterId);
            }

            public long BasketToContractMeter_GetBasketToContractMeterIdByBasketIdAndContractMeterId(long basketId, long contractMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToContractMeter_GetByBasketIdAndContractMeterId, 
                    basketId, contractMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketToContractMeterId"))
                    .FirstOrDefault();
            }
        }
    }
}