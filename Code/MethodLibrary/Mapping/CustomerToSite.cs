using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void CustomerToSite_Insert(long createdByUserId, long sourceId, long customerId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToSite_Insert, 
                    createdByUserId, sourceId, customerId, siteId);
            }

            public long CustomerToSite_GetCustomerToSiteIdByCustomerIdAndSiteId(long customerId, long siteId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToSite_GetByCustomerIdAndSiteId, 
                    customerId, siteId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerToSiteId"))
                    .FirstOrDefault();
            }

            public List<long> CustomerToSite_GetSiteIdListByCustomerId(long customerId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToSite_GetByCustomerId, 
                    customerId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteId"))
                    .ToList();
            }
        }
    }
}