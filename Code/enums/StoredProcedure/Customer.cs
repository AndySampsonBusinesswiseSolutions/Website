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
                public string DataUploadValidationErrorAttribute_GetByDataUploadValidationErrorAttributeDescription = "[Customer].[DataUploadValidationErrorAttribute_GetByDataUploadValidationErrorAttributeDescription]";
                public string DataUploadValidationError_GetByDataUploadValidationErrorGUID = "[Customer].[DataUploadValidationError_GetByDataUploadValidationErrorGUID]";
                public string SubMeterAttribute_Insert = "[Customer].[SubMeterAttribute_Insert]";
                public string SubMeter_Insert = "[Customer].[SubMeter_Insert]";
                public string SubMeterDetail_Insert = "[Customer].[SubMeterDetail_Insert]";
                public string SubMeterAttribute_GetBySubMeterAttributeDescription = "[Customer].[SubMeterAttribute_GetBySubMeterAttributeDescription]";
                public string SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription = "[Customer].[SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription]";
                public string MeterAttribute_Insert = "[Customer].[MeterAttribute_Insert]";
                public string Meter_Insert = "[Customer].[Meter_Insert]";
                public string MeterDetail_Insert = "[Customer].[MeterDetail_Insert]";
                public string MeterAttribute_GetByMeterAttributeDescription = "[Customer].[MeterAttribute_GetByMeterAttributeDescription]";
                public string MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription = "[Customer].[MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription]";
            }
        }
    }
}