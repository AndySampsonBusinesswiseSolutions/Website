using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public List<Entity.Information.TimePeriod> TimePeriod_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureInformationEnums.TimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Information.TimePeriod(d)).ToList();
            }

            public Dictionary<long, TimeSpan> TimePeriod_GetStartTimeDictionary()
            {
                return TimePeriod_GetList().ToDictionary(r => r.TimePeriodId, r => r.StartTime);
            }
        }
    }
}