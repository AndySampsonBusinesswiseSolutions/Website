using System.Data;
using System.Linq;
using System.Reflection;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public class Quarter
            {
                private readonly Enums.StoredProcedure.Information.Quarter _storedProcedureInformationQuarterEnums = new Enums.StoredProcedure.Information.Quarter();

                public string Quarter_GetQuarterDescriptionByQuarterId(long quarterId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureInformationQuarterEnums.Quarter_GetByQuarterId, 
                        quarterId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<string>("QuarterDescription"))
                        .FirstOrDefault();
                }
            }
        }
    }
}