using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(string meterExemptionAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionAttribute_GetByMeterExemptionAttributeDescription, 
                    meterExemptionAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionAttributeId"))
                    .FirstOrDefault();
            }

            public long MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(long meterExemptionAttributeId, string meterExemptionDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription, 
                    meterExemptionAttributeId, meterExemptionDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionId"))
                    .FirstOrDefault();
            }

            public string MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(long meterExemptionId, long meterExemptionAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId, 
                    meterExemptionId, meterExemptionAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("MeterExemptionDetailDescription"))
                    .FirstOrDefault();
            }
        }
    }
}