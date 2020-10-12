namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Customer
            {
                public string MeterAttribute_Insert = "[Customer].[MeterAttribute_Insert]";
                public string Meter_Insert = "[Customer].[Meter_Insert]";
                public string Meter_GetByMeterGUID = "[Customer].[Meter_GetByMeterGUID]";
                public string MeterDetail_Insert = "[Customer].[MeterDetail_Insert]";
                public string MeterDetail_GetByMeterIdAndMeterAttributeId = "[Customer].[MeterDetail_GetByMeterIdAndMeterAttributeId]";
                public string MeterDetail_DeleteByMeterDetailId = "[Customer].[MeterDetail_DeleteByMeterDetailId]";
                public string MeterAttribute_GetByMeterAttributeDescription = "[Customer].[MeterAttribute_GetByMeterAttributeDescription]";
                public string MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription = "[Customer].[MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription]";
                public string MeterDetail_GetByMeterAttributeId = "[Customer].[MeterDetail_GetByMeterAttributeId]";
                public string Meter_GetList = "[Customer].[Meter_GetList]";
                public string Meter_GetByMeterId = "[Customer].[Meter_GetByMeterId]";
            }
        }
    }
}