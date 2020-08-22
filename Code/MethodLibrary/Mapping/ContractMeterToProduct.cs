using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(long contractMeterId, long productId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterToProduct_GetByContractMeterIdAndProductId, 
                    contractMeterId, productId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterToProductId"))
                    .FirstOrDefault();
            }

            public void ContractMeterToProduct_Insert(long createdByUserId, long sourceId, long contractMeterId, long productId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterToProduct_Insert, 
                    createdByUserId, sourceId, contractMeterId, productId);
            }
        }
    }
}