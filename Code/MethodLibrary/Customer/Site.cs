using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public string GetSiteName(long siteId)
            {
                //Get SiteNameSiteAttributeId
                var siteNameSiteAttributeId = SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);

                //Get Site name
                var siteName = SiteDetail_GetSiteDetailDescriptionBySiteIdAndSiteAttributeId(siteId, siteNameSiteAttributeId);

                //Get SitePostcodeSiteAttributeId
                var sitePostcodeSiteAttributeId = SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode);

                //Get Site postcode
                var sitePostcode = SiteDetail_GetSiteDetailDescriptionBySiteIdAndSiteAttributeId(siteId, sitePostcodeSiteAttributeId);

                return $"{siteName}, {sitePostcode}";
            }

            public long InsertNewSite(long createdByUserId, long sourceId)
            {
                //Create new SiteGUID
                var GUID = Guid.NewGuid().ToString();

                while (Site_GetSiteIdBySiteGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[Site]
                Site_Insert(createdByUserId, sourceId, GUID);
                return Site_GetSiteIdBySiteGUID(GUID);
            }

            public void Site_Insert(long createdByUserId, long sourceId, string siteGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Site_Insert, 
                    createdByUserId, sourceId, siteGUID);
            }

            public long Site_GetSiteIdBySiteGUID(string siteGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Site_GetBySiteGUID, 
                    siteGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteId"))
                    .FirstOrDefault();
            }

            public Guid Site_GetSiteGUIDBySiteId(long siteId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Site_GetBySiteId, 
                    siteId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<Guid>("SiteGUID"))
                    .FirstOrDefault();
            }

            public void SiteDetail_Insert(long createdByUserId, long sourceId, long siteId, long siteAttributeId, string siteDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SiteDetail_Insert, 
                    createdByUserId, sourceId, siteId, siteAttributeId, siteDetailDescription);
            }

            public Entity.Customer.SiteDetail SiteDetail_GetBySiteIdAndSiteAttributeId(long siteId, long siteAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteIdAndSiteAttributeId, 
                    siteId, siteAttributeId);

                return new Entity.Customer.SiteDetail(dataTable.Rows.Cast<DataRow>().FirstOrDefault());
            }

            public string SiteDetail_GetSiteDetailDescriptionBySiteIdAndSiteAttributeId(long siteId, long siteAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteIdAndSiteAttributeId, 
                    siteId, siteAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("SiteDetailDescription"))
                    .FirstOrDefault();
            }

            public void SiteDetail_DeleteBySiteDetailId(long customerDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SiteDetail_DeleteBySiteDetailId, 
                    customerDetailId);
            }

            public long SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(string siteAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteAttribute_GetBySiteAttributeDescription, 
                    siteAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteAttributeId"))
                    .FirstOrDefault();
            }

            public long SiteDetail_GetSiteDetailIdBySiteAttributeIdAndSiteDetailDescription(long siteAttributeId, string siteDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription, 
                    siteAttributeId, siteDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteDetailId"))
                    .FirstOrDefault();
            }

            public List<long> SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(long siteAttributeId, string siteDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription, 
                    siteAttributeId, siteDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteId"))
                    .ToList();
            }

            public Dictionary<long, string> SiteDetail_GetSiteDetailDescriptionDictionaryBySiteAttributeId(long siteAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteAttributeId, 
                    siteAttributeId);

                return dataTable.AsEnumerable()
                    .ToDictionary(d => d.Field<long>("SiteId"), d => d.Field<string>("SiteDetailDescription"));
            }
        }
    }
}