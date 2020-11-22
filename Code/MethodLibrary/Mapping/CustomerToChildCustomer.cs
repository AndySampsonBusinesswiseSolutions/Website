using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public Dictionary<long, List<long>> CustomerToChildCustomer_GetCustomerIdToChildCustomerIdDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetList);
                
                var dictionary = new Dictionary<long, List<long>>();

                foreach(DataRow r in dataTable.Rows)
                {
                    if(!dictionary.ContainsKey(r.Field<long>("CustomerId")))
                    {
                        dictionary.Add(r.Field<long>("CustomerId"), new List<long>());
                    }

                    dictionary[r.Field<long>("CustomerId")].Add(r.Field<long>("ChildCustomerId"));
                }

                return dictionary;
            }

            public List<long> CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(long customerId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetByCustomerId, 
                    customerId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ChildCustomerId"))
                    .ToList();
            }

            public void CustomerToChildCustomer_DeleteByCustomerIdAndChildCustomerId(long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId, 
                    customerId, childCustomerId);
            }

            public void CustomerToChildCustomer_Insert(long createdByUserId, long sourceId, long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToChildCustomer_Insert, 
                    createdByUserId, sourceId, customerId, childCustomerId);
            }
        }
    }
}