namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Customer
            {
                public string CustomerAttribute_GetByCustomerAttributeDescription = "[Customer].[CustomerAttribute_GetByCustomerAttributeDescription]";
                public string CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription = "[Customer].[CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription]";
                public string CustomerDetail_DeleteByCustomerDetailId = "[Customer].[CustomerDetail_DeleteByCustomerDetailId]";
                public string CustomerDetail_GetByCustomerIdAndCustomerAttributeId = "[Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId]";
                public string Customer_Insert = "[Customer].[Customer_Insert]";
                public string CustomerDetail_Insert = "[Customer].[CustomerDetail_Insert]";
                public string Customer_GetByCustomerGUID = "[Customer].[Customer_GetByCustomerGUID]";
                public string Customer_GetList = "[Customer].[Customer_GetList]";
            }
        }
    }
}