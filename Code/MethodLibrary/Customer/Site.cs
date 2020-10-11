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

            public void SiteDetail_Insert(long createdByUserId, long sourceId, long siteId, long siteAttributeId, string siteDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.SiteDetail_Insert, 
                    createdByUserId, sourceId, siteId, siteAttributeId, siteDetailDescription);
            }

            public DataRow SiteDetail_GetBySiteIdAndSiteAttributeId(long siteId, long siteAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteIdAndSiteAttributeId, 
                    siteId, siteAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
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
        }
    }
}