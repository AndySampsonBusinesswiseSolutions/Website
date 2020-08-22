using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void ForecastUsageGranularityHistory_CreateTable(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateTable, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateDeleteStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityHistory_CreateInsertStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateInsertStoredProcedure, 
                    meterId, granularityCode, meterType);
            }
        }
    }
}