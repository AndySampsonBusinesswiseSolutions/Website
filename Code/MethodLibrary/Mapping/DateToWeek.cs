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
            public List<DataRow> DateToWeek_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToWeek_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public Dictionary<long, long> GetDateToWeekDictionary()
            {
                var dataRows = DateToWeek_GetList();
                var dateToWeekTuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["WeekId"]);
                    dateToWeekTuple.Add(tup);
                }

                return dateToWeekTuple.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
                );
            }
        }
    }
}