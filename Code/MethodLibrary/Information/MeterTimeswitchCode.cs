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

            public DataTable MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(long meterTimeswitchCodeAttributeId)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId, 
                    meterTimeswitchCodeAttributeId);
            }
        }
    }
}