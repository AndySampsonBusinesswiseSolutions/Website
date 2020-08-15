using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(long BasketAttributeId, string BasketDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription, 
                    BasketAttributeId, BasketDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketDetailId"))
                    .FirstOrDefault();
            }

            public long BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(long BasketAttributeId, string BasketDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription, 
                    BasketAttributeId, BasketDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketId"))
                    .FirstOrDefault();
            }

            public long BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(string BasketAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketAttribute_GetByBasketAttributeDescription, 
                    BasketAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketAttributeId"))
                    .FirstOrDefault();
            }

            public void Basket_Insert(long createdByUserId, long sourceId, string basketGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Basket_Insert, 
                    createdByUserId, sourceId, basketGUID);
            }

            public long Basket_GetBasketIdByBasketGUID(string basketGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Basket_GetByBasketGUID, 
                    basketGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketId"))
                    .FirstOrDefault();
            }

            public void BasketDetail_Insert(long createdByUserId, long sourceId, long basketId, long basketAttributeId, string basketDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.BasketDetail_Insert, 
                    createdByUserId, sourceId, basketId, basketAttributeId, basketDetailDescription);
            }
        }
    }
}
