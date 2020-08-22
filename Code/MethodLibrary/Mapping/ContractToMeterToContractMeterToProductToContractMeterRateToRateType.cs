using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetContractToMeterToContractMeterToProductToContractMeterRateToRateTypeIdByBasketIdAndContractMeterId(long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetByContractToMeterToContractMeterToProductIdAndContractMeterRateToRateTypeId, 
                    contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToMeterToContractMeterToProductToContractMeterRateToRateTypeId"))
                    .FirstOrDefault();
            }

            public void ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert(long createdByUserId, long sourceId, long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert, 
                    createdByUserId, sourceId, contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);
            }
        }
    }
}