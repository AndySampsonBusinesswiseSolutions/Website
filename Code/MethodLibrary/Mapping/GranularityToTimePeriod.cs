using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public IEnumerable<DataRow> GranularityToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }

            public IEnumerable<DataRow> GranularityToTimePeriod_StandardDate_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_StandardDate_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }

            public IEnumerable<DataRow> GranularityToTimePeriod_NonStandardDate_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_NonStandardDate_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }

            public IEnumerable<DataRow> GranularityToTimePeriod_StandardDate_GetListByGranularityId(long granularityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_StandardDate_GetByGranularityId,
                    granularityId);

                return dataTable.Rows.Cast<DataRow>();
            }

            public IEnumerable<DataRow> GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(long granularityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_NonStandardDate_GetByGranularityId,
                    granularityId);

                return dataTable.Rows.Cast<DataRow>();
            }
        }
    }
}