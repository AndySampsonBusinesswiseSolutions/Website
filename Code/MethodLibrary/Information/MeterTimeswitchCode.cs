using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

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
            
            public List<Entity.Information.MeterTimeswitchCodeDetail> MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(long meterTimeswitchCodeAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId, 
                    meterTimeswitchCodeAttributeId);
                
                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Information.MeterTimeswitchCodeDetail(d)).ToList();
            }
        }
    }
}