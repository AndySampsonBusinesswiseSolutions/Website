using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(string meterTimeswitchCodeAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeAttribute_GetByMeterTimeswitchCodeAttributeDescription, 
                    meterTimeswitchCodeAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeAttributeId"))
                    .FirstOrDefault();
            }

            public long MeterTimeswitchCodeDetail_GetMeterTimeswitchCodeDetailIdByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription(long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription, 
                    meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeDetailId"))
                    .FirstOrDefault();
            }

            public long MeterTimeswitchCodeDetail_GetMeterTimeswitchCodeIdByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription(long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription, 
                    meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeId"))
                    .FirstOrDefault();
            }

            public void MeterTimeswitchCode_Insert(long createdByUserId, long sourceId, string meterTimeswitchCodeGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.MeterTimeswitchCode_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeGUID);
            }

            public long MeterTimeswitchCode_GetMeterTimeswitchCodeIdByMeterTimeswitchCodeGUID(string meterTimeswitchCodeGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCode_GetByMeterTimeswitchCodeGUID, 
                    meterTimeswitchCodeGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeId"))
                    .FirstOrDefault();
            }

            public void MeterTimeswitchCodeDetail_Insert(long createdByUserId, long sourceId, long meterTimeswitchCodeId, long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeId, meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);
            }

            public DataTable MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(long meterTimeswitchCodeAttributeId)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId, 
                    meterTimeswitchCodeAttributeId);
            }
        }
    }
}