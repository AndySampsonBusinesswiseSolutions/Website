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
            public class Year
            {
                private readonly Enums.StoredProcedure.Information.Year _storedProcedureInformationYearEnums = new Enums.StoredProcedure.Information.Year();

                public string Year_GetYearDescriptionByYearId(long yearId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureInformationYearEnums.Year_GetByYearId, 
                        yearId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<string>("YearDescription"))
                        .FirstOrDefault();
                }
            }
        }
    }
}