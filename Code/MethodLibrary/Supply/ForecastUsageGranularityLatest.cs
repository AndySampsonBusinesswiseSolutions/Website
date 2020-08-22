using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void ForecastUsageGranularityLatest_CreateTable(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateTable, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateDeleteStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityLatest_CreateInsertStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateInsertStoredProcedure, 
                    meterId, granularityCode, meterType);
            }
        }
    }
}