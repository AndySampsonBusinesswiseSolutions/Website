using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void LoadedUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            public void LoadedUsage_Delete(string meterType, long meterId, long dateId, long timePeriodId)
            {
                var loadedUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Delete, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageDeleteStoredProcedure,
                    dateId, timePeriodId);
            }

            public void LoadedUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, long dateId, long timePeriodId, long usageTypeId, decimal usage)
            {
                var loadedUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageInsertStoredProcedure,
                    createdByUserId, sourceId, dateId, timePeriodId, usageTypeId, usage);
            }
        }
    }
}
