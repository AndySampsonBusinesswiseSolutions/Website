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
            public long InsertNewMeterExemption(long createdByUserId, long sourceId)
            {
                //Create new MeterExemptionGUID
                var GUID = Guid.NewGuid().ToString();

                while (MeterExemption_GetMeterExemptionIdByMeterExemptionGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[MeterExemption]
                MeterExemption_Insert(createdByUserId, sourceId, GUID);
                return MeterExemption_GetMeterExemptionIdByMeterExemptionGUID(GUID);
            }

            public long MeterExemptionDetail_GetMeterExemptionDetailIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(long MeterExemptionAttributeId, string MeterExemptionDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription, 
                    MeterExemptionAttributeId, MeterExemptionDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionDetailId"))
                    .FirstOrDefault();
            }

            public long MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(string MeterExemptionAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterExemptionAttribute_GetByMeterExemptionAttributeDescription, 
                    MeterExemptionAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionAttributeId"))
                    .FirstOrDefault();
            }

            public void MeterExemption_Insert(long createdByUserId, long sourceId, string meterExemptionGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.MeterExemption_Insert, 
                    createdByUserId, sourceId, meterExemptionGUID);
            }

            public long MeterExemption_GetMeterExemptionIdByMeterExemptionGUID(string meterExemptionGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterExemption_GetByMeterExemptionGUID, 
                    meterExemptionGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionId"))
                    .FirstOrDefault();
            }

            public void MeterExemptionDetail_Insert(long createdByUserId, long sourceId, long meterExemptionId, long meterExemptionAttributeId, string meterExemptionDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.MeterExemptionDetail_Insert, 
                    createdByUserId, sourceId, meterExemptionId, meterExemptionAttributeId, meterExemptionDetailDescription);
            }

            public DataRow MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId(long meterExemptionId, long meterExemptionAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId, 
                    meterExemptionId, meterExemptionAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }

            public List<long> MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(long meterExemptionAttributeId, string meterExemptionDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription, 
                    meterExemptionAttributeId, meterExemptionDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionId"))
                    .ToList();
            }
        }
    }
}
