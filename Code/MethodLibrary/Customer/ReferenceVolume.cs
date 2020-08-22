using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public void ReferenceVolume_Insert(long createdByUserId, long sourceId, string referenceVolumeGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ReferenceVolume_Insert, 
                    createdByUserId, sourceId, referenceVolumeGUID);
            }

            public long ReferenceVolume_GetReferenceVolumeIdByReferenceVolumeGUID(string referenceVolumeGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ReferenceVolume_GetByReferenceVolumeGUID, 
                    referenceVolumeGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ReferenceVolumeId"))
                    .FirstOrDefault();
            }

            public void ReferenceVolumeDetail_Insert(long createdByUserId, long sourceId, long referenceVolumeId, long referenceVolumeAttributeId, string referenceVolumeDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ReferenceVolumeDetail_Insert, 
                    createdByUserId, sourceId, referenceVolumeId, referenceVolumeAttributeId, referenceVolumeDetailDescription);
            }

            public long ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(string referenceVolumeAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ReferenceVolumeAttribute_GetByReferenceVolumeAttributeDescription, 
                    referenceVolumeAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ReferenceVolumeAttributeId"))
                    .FirstOrDefault();
            }

            public List<long> ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(long referenceVolumeAttributeId, string referenceVolumeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ReferenceVolumeDetail_GetByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription, 
                    referenceVolumeAttributeId, referenceVolumeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ReferenceVolumeId"))
                    .ToList();
            }
        }
    }
}
