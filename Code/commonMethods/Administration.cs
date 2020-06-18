using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace commonMethods
{
    public partial class CommonMethods
    {
        public class Administration
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
            
            public void UserDetail_Insert(long createdByUserId, long sourceId, long userId, long userattributeId, string userDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureAdministrationEnums.UserDetail_Insert, 
                    createdByUserId, sourceId, userId, userattributeId, userDetailDescription);
            }
            
            public long User_GetUserIdByUserDetailId(long userDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.UserDetail_GetByUserDetailId, 
                    userDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserId"))
                    .FirstOrDefault();
            }

            public long User_GetUserIdByUserGUID(string userGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.User_GetByUserGUID, 
                    userGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserId"))
                    .FirstOrDefault();
            }

            public long UserAttribute_GetUserAttributeIdByUserAttributeDescription(string userAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.UserAttribute_GetByUserAttributeDescription, 
                    userAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserAttributeId"))
                    .FirstOrDefault();
            }
            
            public long UserDetail_GetUserDetailIdByEmailAddress(string userDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureAdministrationEnums.UserDetail_GetByUserDetailDescription, 
                    userDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UserDetailId"))
                    .FirstOrDefault();
            }

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

            public long GetUserIdByEmailAddress(JObject jsonObject)
            {
                var emailAddress = jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
                var userDetailId = UserDetail_GetUserDetailIdByEmailAddress(emailAddress);
                return User_GetUserIdByUserDetailId(userDetailId);
            }
        }
    }
}
