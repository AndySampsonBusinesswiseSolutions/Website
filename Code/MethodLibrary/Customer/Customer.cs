using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class CustomerSchema
        {
            public long InsertNewCustomer(long createdByUserId, long sourceId)
            {
                //Create new CustomerGUID
                var GUID = Guid.NewGuid().ToString();

                while (Customer_GetCustomerIdByCustomerGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[Customer]
                Customer_Insert(createdByUserId, sourceId, GUID);
                return Customer_GetCustomerIdByCustomerGUID(GUID);
            }

            public List<long> Customer_GetCustomerIdList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Customer_GetList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .ToList();
            }

            public long Customer_GetCustomerIdByCustomerGUID(string customerGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Customer_GetByCustomerGUID, 
                    customerGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .FirstOrDefault();
            }

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

            public long CustomerDetail_GetCustomerIdByCustomerAttributeIdAndCustomerDetailDescription(long customerAttributeId, string customerDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription, 
                    customerAttributeId, customerDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .FirstOrDefault();
            }

            public Entity.Customer.CustomerDetail CustomerDetail_GetByCustomerIdAndCustomerAttributeId(long customerId, long customerAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerIdAndCustomerAttributeId, 
                    customerId, customerAttributeId);

                return new Entity.Customer.CustomerDetail(dataTable.Rows.Cast<DataRow>().FirstOrDefault());
            }

            public string CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(long customerId, long customerAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerIdAndCustomerAttributeId, 
                    customerId, customerAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("CustomerDetailDescription"))
                    .FirstOrDefault();
            }

            public void CustomerDetail_DeleteByCustomerDetailId(long customerDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.CustomerDetail_DeleteByCustomerDetailId, 
                    customerDetailId);
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