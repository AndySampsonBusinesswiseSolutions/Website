using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<DataRow> GranularityToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<DataRow> GranularityToTimePeriod_StandardDate_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_StandardDate_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<DataRow> GranularityToTimePeriod_NonStandardDate_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_NonStandardDate_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long, long>> GranularityToTimePeriod_NonStandardDate_GetTuple()
            {
                var dataRows = GranularityToTimePeriod_NonStandardDate_GetList();
                var granularityToTimePeriodTuple = new List<Tuple<long, long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["GranularityId"], (long)r["TimePeriodId"], (long)r["DateId"]);
                    granularityToTimePeriodTuple.Add(tup);
                }

                return granularityToTimePeriodTuple;
            }

            public List<DataRow> GranularityToTimePeriod_StandardDate_GetListByGranularityId(long granularityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_StandardDate_GetByGranularityId,
                    granularityId);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<long> GranularityToTimePeriod_StandardDate_GetTimePeriodListByGranularityId(long granularityId)
            {
                return  GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId)
                    .Select(d => d.Field<long>("TimePeriodId")).ToList();
            }

            public List<DataRow> GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(long granularityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_NonStandardDate_GetByGranularityId,
                    granularityId);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long>> GranularityToTimePeriod_NonStandardDate_GetTupleByGranularityId(long granularityId)
            {
                var dataRows = GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
                var granularityToTimePeriodTuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["TimePeriodId"]);
                    granularityToTimePeriodTuple.Add(tup);
                }

                return granularityToTimePeriodTuple;
            }
        }
    }
}