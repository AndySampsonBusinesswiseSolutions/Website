using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long InsertNewBasket(long createdByUserId, long sourceId)
            {
                //Create new BasketGUID
                var GUID = Guid.NewGuid().ToString();

                while (Basket_GetBasketIdByBasketGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[Basket]
                Basket_Insert(createdByUserId, sourceId, GUID);
                return Basket_GetBasketIdByBasketGUID(GUID);
            }

            public long BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(long basketAttributeId, string basketDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription, 
                    basketAttributeId, basketDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketDetailId"))
                    .FirstOrDefault();
            }

            public long BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(long basketAttributeId, string basketDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription, 
                    basketAttributeId, basketDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketId"))
                    .FirstOrDefault();
            }

            public long BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(string basketAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketAttribute_GetByBasketAttributeDescription, 
                    basketAttributeDescription);

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
