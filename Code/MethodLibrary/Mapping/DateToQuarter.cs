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
            public List<DataRow> DateToQuarter_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToQuarter_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public Dictionary<long, long> GetDateToQuarterDictionary()
            {
                var dataRows = DateToQuarter_GetList();
                var dateToQuarterTuple = new List<Tuple<long, long>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["QuarterId"]);
                    dateToQuarterTuple.Add(tup);
                }

                return dateToQuarterTuple.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
                );
            }
        }
    }
}