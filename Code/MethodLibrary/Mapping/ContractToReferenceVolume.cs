using System.Reflection;

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
        }
    }
}