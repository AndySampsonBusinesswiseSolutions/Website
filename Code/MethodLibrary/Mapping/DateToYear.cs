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
            public List<DataRow> DateToYear_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToYear_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public Dictionary<long, long> GetDateToYearDictionary()
            {
                var dataRows = DateToYear_GetList();
                var dateToYearTuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["YearId"]);
                    dateToYearTuple.Add(tup);
                }

                return dateToYearTuple.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
                );
            }
        }
    }
}