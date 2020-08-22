using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(long contractToMeterId, long contractMeterToProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProduct_GetByContractToMeterIdAndContractMeterToProductId, 
                    contractToMeterId, contractMeterToProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToMeterToContractMeterToProductId"))
                    .FirstOrDefault();
            }

            public void ContractToMeterToContractMeterToProduct_Insert(long createdByUserId, long sourceId, long contractToMeterId, long contractMeterToProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProduct_Insert, 
                    createdByUserId, sourceId, contractToMeterId, contractMeterToProductId);
            }
        }
    }
}