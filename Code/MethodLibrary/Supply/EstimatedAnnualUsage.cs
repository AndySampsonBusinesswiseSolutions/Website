using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            public void EstimatedAnnualUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateTable, 
                    meterId, meterType);
            }

            private void EstimatedAnnualUsage_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void EstimatedAnnualUsage_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            public void EstimatedAnnualUsage_Delete(string meterType, long meterId)
            {
                var estimatedAnnualUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.EstimatedAnnualUsage_Delete, meterType, meterId);

                ExecuteNonQuery(new List<ParameterInfo>().ToArray(), estimatedAnnualUsageDeleteStoredProcedure);
            }

            public void EstimatedAnnualUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, decimal estimatedAnnualUsage)
            {
                var estimatedAnnualUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.EstimatedAnnualUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    estimatedAnnualUsageInsertStoredProcedure,
                    createdByUserId, sourceId, estimatedAnnualUsage);
            }
        }
    }
}