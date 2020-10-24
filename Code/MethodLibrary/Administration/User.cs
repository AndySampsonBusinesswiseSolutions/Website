using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Administration
        {
            public class User
            {
                public long GetSystemUserId()
                {
                    return User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
                }

                public void UserDetail_Insert(long createdByUserId, long sourceId, long userId, long userAttributeId, string userDetailDescription)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureAdministrationEnums.UserDetail_Insert, 
                        createdByUserId, sourceId, userId, userAttributeId, userDetailDescription);
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

                public long UserDetail_GetUserDetailIdByUserIdAndUserAttributeId(long userId, long userAttributeId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureAdministrationEnums.UserDetail_GetByUserIdAndUserAttributeId, 
                        userId, userAttributeId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<long>("UserDetailId"))
                        .FirstOrDefault();
                }

                public long GetUserIdByEmailAddress(JObject jsonObject)
                {
                    var emailAddress = new System().GetEmailAddressFromJObject(jsonObject);
                    var userDetailId = UserDetail_GetUserDetailIdByEmailAddress(emailAddress);
                    return User_GetUserIdByUserDetailId(userDetailId);
                }
            }
        }
    }
}