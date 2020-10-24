using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Administration
        {
            public class Password
            {
                public long Password_GetPasswordIdByPassword(string password)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureAdministrationEnums.Password_GetByPassword, 
                        password);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<long>("PasswordId"))
                        .FirstOrDefault();
                }
            }
        }
    }
}