namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public class Customer
            {
                public string CustomerAttribute_GetByCustomerAttributeDescription = "[Customer].[CustomerAttribute_GetByCustomerAttributeDescription]";
                public string CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription = "[Customer].[CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription]";
                public string CustomerDetail_DeleteByCustomerDetailId = "[Customer].[CustomerDetail_DeleteByCustomerDetailId]";
                public string CustomerDetail_GetByCustomerIdAndCustomerAttributeId = "[Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId]";
                public string Customer_Insert = "[Customer].[Customer_Insert]";
                public string CustomerDetail_Insert = "[Customer].[CustomerDetail_Insert]";
                public string Customer_GetByCustomerGUID = "[Customer].[Customer_GetByCustomerGUID]";
                public string Customer_GetList = "[Customer].[Customer_GetList]";
                public string DataUploadValidationErrorAttribute_Insert = "[Customer].[DataUploadValidationErrorAttribute_Insert]";
                public string DataUploadValidationError_Insert = "[Customer].[DataUploadValidationError_Insert]";
                public string DataUploadValidationErrorDetail_Insert = "[Customer].[DataUploadValidationErrorDetail_Insert]";
                public string DataUploadValidationErrorDescription_GetByDataUploadValidationErrorDescriptionDescription = "[Customer].[DataUploadValidationErrorDescription_GetByDataUploadValidationErrorDescriptionDescription]";
                public string DataUploadValidationError_GetByDataUploadValidationErrorGUID = "[Customer].[DataUploadValidationError_GetByDataUploadValidationErrorGUID]";
            }
        }
    }
}