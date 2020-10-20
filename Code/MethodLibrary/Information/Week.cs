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
            public class Week
            {
                private readonly Enums.StoredProcedure.Information.Week _storedProcedureInformationWeekEnums = new Enums.StoredProcedure.Information.Week();

                public string Week_GetWeekDescriptionByWeekId(long weekId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureInformationWeekEnums.Week_GetByWeekId, 
                        weekId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<string>("WeekDescription"))
                        .FirstOrDefault();
                }
            }
        }
    }
}