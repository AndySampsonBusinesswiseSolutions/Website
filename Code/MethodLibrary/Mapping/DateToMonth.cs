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
            public List<DataRow> DateToMonth_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToMonth_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public Dictionary<long, long> GetDateToMonthDictionary()
            {
                var dataRows = DateToMonth_GetList();
                var dateToMonthTuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["MonthId"]);
                    dateToMonthTuple.Add(tup);
                }

                return dateToMonthTuple.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
                );
            }
        }
    }
}