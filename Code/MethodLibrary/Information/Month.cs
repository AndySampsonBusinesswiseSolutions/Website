using System.Data;
using System.Linq;
using System.Reflection;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public class Month
            {
                private readonly Enums.StoredProcedure.Information.Month _storedProcedureInformationMonthEnums = new Enums.StoredProcedure.Information.Month();

                public string Month_GetMonthDescriptionByMonthId(long monthId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureInformationMonthEnums.Month_GetByMonthId, 
                        monthId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<string>("MonthDescription"))
                        .FirstOrDefault();
                }
            }
        }
    }
}