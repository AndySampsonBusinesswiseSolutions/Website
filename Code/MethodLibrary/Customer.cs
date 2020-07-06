using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Customer
        {
            public long CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(string customerAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerAttribute_GetByCustomerAttributeDescription, 
                    customerAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerAttributeId"))
                    .FirstOrDefault();
            }

            public long CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(long customerAttributeId, string customerDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription, 
                    customerAttributeId, customerDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerDetailId"))
                    .FirstOrDefault();
            }

            public void Customer_Insert(long createdByUserId, long sourceId, string customerGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Customer_Insert, 
                    createdByUserId, sourceId, customerGUID);
            }

            public void CustomerDetail_Insert(long createdByUserId, long sourceId, long customerId, long customerAttributeId, string customerDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.CustomerDetail_Insert, 
                    createdByUserId, sourceId, customerId, customerAttributeId, customerDetailDescription);
            }
        }
    }
}
