using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class AdministrationSchema
        {
            public class Login
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

                public int CountInvalidAttempts(IOrderedEnumerable<long> loginList)
                {
                    var invalidAttempts = 0;

                    //Loop through each login
                    foreach (var login in loginList)
                    {
                        //Get LoginSuccessful attribute
                        var loginSucessful = Login_GetLoginSuccessfulByLoginId(login);

                        //If login is successful, then exit loop
                        //Else increment invalidAttempts
                        if (loginSucessful)
                        {
                            break;
                        }
                        else
                        {
                            invalidAttempts++;
                        }
                    }

                    return invalidAttempts;
                }
            }
        }
    }
}