namespace enums
{
    public partial class Enums
    {
        public class System
        {
            public class API
            {
                public class Attribute
                {
                    public string APIName = "API Name";
                    public string HTTPSApplicationURL = "HTTPS Application URL";
                    public string HTTPApplicationURL = "HTTP Application URL";
                    public string PrerequisiteAPIGUID = "Prerequisite API GUID";
                    public string POSTRoute = "POST Route";
                    public string RequiredDataKey = "Required Data Key";
                    public string MaximumInvalidLoginAttempts = "Maximum Invalid Login Attempts";
                }
                
                public class GUID
                {
                    public string StoreLoginAttemptAPI = "3BBFC2B6-2572-43CD-921A-A237000AC248";//
                    public string RoutingAPI = "A4F25D07-86AA-42BD-ACD7-51A8F772A92B";
                    public string WebsiteAPI = "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE";
                    public string ValidateProcessGUIDAPI = "87AFEDA8-6A0F-4143-BF95-E08E78721CF5";
                    public string ArchiveProcessQueueAPI = "38D3A9E1-A060-4464-B971-8DC523B6A42D";
                    public string ValidatePageGUIDAPI = "F916F19F-9408-4969-84DC-9905D2FEFB0B";
                    public string CheckPrerequisiteAPIAPI = "56371F02-4120-41C9-82F9-4408309684D1";
                    public string ValidateEmailAddressAPI = "99681B37-575F-47E5-95E3-608063EA513E";
                    public string ValidatePasswordAPI = "26FEFFE8-49F7-4458-98ED-FD5F6C65C7C2";
                    public string ValidateEmailAddressPasswordMappingAPI = "CEC56745-C1C5-4E67-805B-159A8A5E991D";
                    public string LockUserAPI = "0C1BAFAA-586D-48BB-8D0B-B0B56BE0CCD2";
                }

                public class Name
                {
                    public string StoreLoginAttemptAPI = "StoreLoginAttempt.api";
                    public string RoutingAPI = "Routing.api";
                    public string WebsiteAPI = "Website.api";
                    public string ValidateProcessGUIDAPI = "ValidateProcessGUID.api";
                    public string ArchiveProcessQueueAPI = "ArchiveProcessQueue.api";
                    public string ValidatePageGUIDAPI = "ValidatePageGUID.api";
                    public string CheckPrerequisiteAPIAPI = "CheckPrerequisiteAPI.api";
                    public string ValidateEmailAddressAPI = "ValidateEmailAddress.api";
                    public string ValidatePasswordAPI = "ValidatePassword.api";
                    public string ValidateEmailAddressPasswordMappingAPI = "ValidateEmailAddressPasswordMapping.api";
                    public string LockUserAPI = "LockUser.api";
                }

                //TODO: Move to config file in each API
                public class Password 
                {
                    public string RoutingAPI = @"E{*Jj5&nLfC}@Q$:";
                    public string WebsiteAPI = @"\wU.D[ArWjPG!F4$";
                    public string ValidateProcessGUIDAPI = @"Y4c?.KT(>HXj@f8D";
                    public string ArchiveProcessQueueAPI = @"nb@89qWEW5!6=2s*";
                    public string ValidatePageGUIDAPI = @"n:Q>V&6P9KtG`(5k";
                    public string CheckPrerequisiteAPIAPI = @"w8chCkRAW]\N[7Hh";
                    public string ValidateEmailAddressAPI = @"}h8FfD2r[Rd~PPNR";
                    public string ValidatePasswordAPI = @"b7.Q!!X3Hp{\mJ}j";
                    public string ValidateEmailAddressPasswordMappingAPI = @"GQzD2!aZNvffr*zC";
                    public string LockUserAPI = "JM7!?q#g#uTyM^!v";
                    public string StoreLoginAttemptAPI = "mLdas-Y*x2rbnJ2e";
                }

                public class RequiredDataKey
                {
                    public string APIGUIDList = "APIGUIDList";
                    public string EmailAddress = "EmailAddress";
                    public string PageGUID = "PageGUID";
                    public string Password = "Password";
                    public string ProcessGUID = "ProcessGUID";
                    public string QueueGUID = "QueueGUID";
                    public string CallingGUID = "CallingGUID";
                }
            }

            public class Page
            {
                public class Attribute
                {
                    public string PageName = "Page Name";
                }

                public class GUID
                {
                    public string Login = "6641A1BF-84C8-48F8-9D79-70D0AB2BB787";
                }

                public class Name
                {
                    public string Login = "Login";
                }
            }

            public class ProcessArchive
            {
                public class Attribute
                {
                    public string Response = "Response";
                }
            }

            public class Process
            {
                public class Attribute
                {
                    public string ProcessName = "Process Name";
                }

                public class GUID
                {
                    public string Login = "AF10359F-FD78-4345-9F26-EF5A921E72FD";
                }

                public class Name
                {
                    public string Login = "Login";
                }
            }
        }
    }
}