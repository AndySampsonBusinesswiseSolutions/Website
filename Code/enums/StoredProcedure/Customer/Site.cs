namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Customer
            {
                public string SiteAttribute_Insert = "[Customer].[SiteAttribute_Insert]";
                public string Site_Insert = "[Customer].[Site_Insert]";
                public string Site_GetBySiteGUID = "[Customer].[Site_GetBySiteGUID]";
                public string SiteDetail_Insert = "[Customer].[SiteDetail_Insert]";
                public string SiteDetail_GetBySiteIdAndSiteAttributeId = "[Customer].[SiteDetail_GetBySiteIdAndSiteAttributeId]";
                public string SiteDetail_DeleteBySiteDetailId = "[Customer].[SiteDetail_DeleteBySiteDetailId]";
                public string SiteAttribute_GetBySiteAttributeDescription = "[Customer].[SiteAttribute_GetBySiteAttributeDescription]";
                public string SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription = "[Customer].[SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription]";
            }
        }
    }
}