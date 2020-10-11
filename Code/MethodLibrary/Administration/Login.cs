using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Administration
        {
            public void Login_Insert(long createdByUserId, long sourceId, bool loginSuccessful, string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureAdministrationEnums.Login_Insert, 
                    createdByUserId, sourceId, loginSuccessful, processArchiveGUID);
            }

            public long Login_GetLoginIdByProcessArchiveGUID(string processArchiveGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.Login_GetByProcessArchiveGUID, 
                    processArchiveGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LoginId"))
                    .FirstOrDefault();
            }

            public bool Login_GetLoginSuccessfulByLoginId(long loginId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.Login_GetByLoginId, 
                    loginId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("LoginSuccessful"))
                    .FirstOrDefault();
            }
        }
    }
}