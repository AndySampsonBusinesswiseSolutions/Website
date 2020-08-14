using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(long MeterAttributeId, string MeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription, 
                    MeterAttributeId, MeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterDetailId"))
                    .FirstOrDefault();
            }

            public long MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(string MeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterAttribute_GetByMeterAttributeDescription, 
                    MeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterAttributeId"))
                    .FirstOrDefault();
            }

            public void Meter_Insert(long createdByUserId, long sourceId, string meterGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Meter_Insert, 
                    createdByUserId, sourceId, meterGUID);
            }

            public long Meter_GetMeterIdByMeterGUID(string meterGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Meter_GetByMeterGUID, 
                    meterGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .FirstOrDefault();
            }

            public void MeterDetail_Insert(long createdByUserId, long sourceId, long meterId, long meterAttributeId, string meterDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.MeterDetail_Insert, 
                    createdByUserId, sourceId, meterId, meterAttributeId, meterDetailDescription);
            }

            public DataRow MeterDetail_GetByMeterIdAndMeterAttributeId(long meterId, long meterAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterDetail_GetByMeterIdAndMeterAttributeId, 
                    meterId, meterAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }

            public void MeterDetail_DeleteByMeterDetailId(long meterDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.MeterDetail_DeleteByMeterDetailId, 
                    meterDetailId);
            }
        }
    }
}
