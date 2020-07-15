namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public class Administration
            {
                public string Password_GetByPassword = "[Administration.User].[Password_GetByPassword]";
                public string UserDetail_GetByUserDetailDescription = "[Administration.User].[UserDetail_GetByUserDetailDescription]";
                public string UserDetail_GetByUserDetailId = "[Administration.User].[UserDetail_GetByUserDetailId]";
                public string UserDetail_GetByUserIdAndUserAttributeId = "[Administration.User].[UserDetail_GetByUserIdAndUserAttributeId]";
                public string UserDetail_Insert = "[Administration.User].[UserDetail_Insert]";
                public string User_GetByUserGUID = "[Administration.User].[User_GetByUserGUID]";
                public string UserAttribute_GetByUserAttributeDescription = "[Administration.User].[UserAttribute_GetByUserAttributeDescription]";
                public string Login_Insert = "[Administration.User].[Login_Insert]";
                public string Login_GetByProcessArchiveGUID = "[Administration.User].[Login_GetByProcessArchiveGUID]";
                public string Login_GetByLoginId = "[Administration.User].[Login_GetByLoginId]";
            }
        }
    }
}