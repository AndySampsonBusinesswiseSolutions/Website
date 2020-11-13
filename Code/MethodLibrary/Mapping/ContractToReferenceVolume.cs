using System.Reflection;
using System.Linq;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void ContractToReferenceVolume_Insert(long createdByUserId, long sourceId, long contractId, long referenceVolumeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToReferenceVolume_Insert, 
                    createdByUserId, sourceId, contractId, referenceVolumeId);
            }

            public long ContractToReferenceVolume_GetContractToReferenceVolumeIdByContractIdAndReferenceVolumeId(long contractId, long referenceVolumeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToReferenceVolume_GetByContractIdAndReferenceVolumeId, 
                    contractId, referenceVolumeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToReferenceVolumeId"))
                    .FirstOrDefault();
            }
        }
    }
}