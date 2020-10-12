namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Customer
            {
                public string SubMeterAttribute_Insert = "[Customer].[SubMeterAttribute_Insert]";
                public string SubMeter_Insert = "[Customer].[SubMeter_Insert]";
                public string SubMeter_GetBySubMeterGUID = "[Customer].[SubMeter_GetBySubMeterGUID]";
                public string SubMeterDetail_Insert = "[Customer].[SubMeterDetail_Insert]";
                public string SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId = "[Customer].[SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId]";
                public string SubMeterDetail_DeleteBySubMeterDetailId = "[Customer].[SubMeterDetail_DeleteBySubMeterDetailId]";
                public string SubMeterAttribute_GetBySubMeterAttributeDescription = "[Customer].[SubMeterAttribute_GetBySubMeterAttributeDescription]";
                public string SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription = "[Customer].[SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription]";
                public string SubMeter_GetBySubMeterId = "[Customer].[SubMeter_GetBySubMeterId]";
            }
        }
    }
}