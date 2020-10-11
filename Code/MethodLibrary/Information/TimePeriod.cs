using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public List<DataRow> TimePeriod_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureInformationEnums.TimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }
        }
    }
}