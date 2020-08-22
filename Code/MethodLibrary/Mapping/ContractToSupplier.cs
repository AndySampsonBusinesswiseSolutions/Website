using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void ContractToSupplier_Insert(long createdByUserId, long sourceId, long contractId, long supplierId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToSupplier_Insert, 
                    createdByUserId, sourceId, contractId, supplierId);
            }

            public long ContractToSupplier_GetContractToSupplierIdByContractIdAndSupplierId(long contractId, long supplierId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToSupplier_GetByContractIdAndSupplierId, 
                    contractId, supplierId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToSupplierId"))
                    .FirstOrDefault();
            }
        }
    }
}