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
            public long SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(long subMeterAttributeId, string subMeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription, 
                    subMeterAttributeId, subMeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterDetailId"))
                    .FirstOrDefault();
            }

            public long SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(string subMeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterAttribute_GetBySubMeterAttributeDescription, 
                    subMeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterAttributeId"))
                    .FirstOrDefault();
            }

            public void SubMeter_Insert(long createdByUserId, long sourceId, string subMeterGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SubMeter_Insert, 
                    createdByUserId, sourceId, subMeterGUID);
            }

            public long SubMeter_GetSubMeterIdBySubMeterGUID(string subMeterGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeter_GetBySubMeterGUID, 
                    subMeterGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterId"))
                    .FirstOrDefault();
            }

            public void SubMeterDetail_Insert(long createdByUserId, long sourceId, long subMeterId, long subMeterAttributeId, string subMeterDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SubMeterDetail_Insert, 
                    createdByUserId, sourceId, subMeterId, subMeterAttributeId, subMeterDetailDescription);
            }

            public DataRow SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId(long subMeterId, long subMeterAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId, 
                    subMeterId, subMeterAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }

            public void SubMeterDetail_DeleteBySubMeterDetailId(long subMeterDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SubMeterDetail_DeleteBySubMeterDetailId, 
                    subMeterDetailId);
            }

            public List<long> SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(long subMeterAttributeId, string subMeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription, 
                    subMeterAttributeId, subMeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterId"))
                    .ToList();
            }
        }
    }
}
