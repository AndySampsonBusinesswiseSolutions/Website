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
            private void CreateEstimatedAnnualUsageEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"EstimatedAnnualUsage";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    EstimatedAnnualUsage_CreateTable(meterId, meterType);
                }

                EstimatedAnnualUsage_CreateDeleteStoredProcedure(meterId, meterType);
                EstimatedAnnualUsage_CreateInsertStoredProcedure(meterId, meterType);
                EstimatedAnnualUsage_GrantExecuteToStoredProcedures(meterId, meterType);
            }

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

            public void EstimatedAnnualUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, decimal usage)
            {
                var estimatedAnnualUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.EstimatedAnnualUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    estimatedAnnualUsageInsertStoredProcedure,
                    createdByUserId, sourceId, usage);
            }

            private void EstimatedAnnualUsage_GrantExecuteToStoredProcedures(long meterId, string meterType)
            {
                foreach(var estimatedAnnualUsageStoredProcedure in _storedProcedureSupplyEnums.EstimatedAnnualUsageStoredProcedureList)
                {
                    var storedProcedure = string.Format(estimatedAnnualUsageStoredProcedure, meterType, meterId);

                    foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                    {
                        var SQL = $"GRANT EXECUTE ON OBJECT::{storedProcedure} TO [{api}];";
                        ExecuteSQL(SQL);
                    }
                }
            }
        }
    }
}