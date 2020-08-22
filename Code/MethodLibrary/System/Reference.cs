using Newtonsoft.Json.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public string GetCallingGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();
            }

            public string GetProcessQueueGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();
            }

            public void SetProcessQueueGUIDInJObject(JObject jsonObject, string processQueueGUID)
            {
                jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID] = processQueueGUID;
            }

            public string GetProcessGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID].ToString();
            }

            public void SetProcessGUIDInJObject(JObject jsonObject, string processGUID)
            {
                jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID] = processGUID;
            }

            public string GetPasswordFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.Password].ToString();
            }

            public string GetPageGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.PageGUID].ToString();
            }

            public string GetEmailAddressFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
            }

            public string GetFileGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
            }

            public string GetFileNameFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileName].ToString();
            }

            public string GetFileContentFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.FileContent].ToString();
            }

            public string GetFileTypeFromJObject(JObject jsonObject)
            {
                try
                {
                    return jsonObject[_systemAPIRequiredDataKeyEnums.FileType].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            public string GetAPIGUIDListFromJObject(JObject jsonObject)
            {
                try
                {
                    return jsonObject[_systemAPIRequiredDataKeyEnums.APIGUIDList].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            public string GetCustomerGUIDFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();
            }

            public string GetCustomerDataFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.CustomerData].ToString();
            }
            
            public string GetChildCustomerDataFromJObject(JObject jsonObject)
            {
                return jsonObject[_systemAPIRequiredDataKeyEnums.ChildCustomerData].ToString();
            }
        }
    }
}
