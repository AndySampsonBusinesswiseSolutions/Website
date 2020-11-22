using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class CustomerSchema
        {
            public bool ContractBasketMeterExists(string contractReference, string basketReference, string mpxn)
            {
                //Get BasketId from BasketReference
                var basketReferenceBasketAttributeId = BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                var basketId = BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, basketReference);

                //If BasketId == 0 then not valid
                if(basketId == 0)
                {
                    return false;
                }

                //Get ContractMeters from BasketId
                var contractMeterIdFromBasketId = new MappingSchema().BasketToContractMeter_GetContractMeterIdListByBasketId(basketId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromBasketId.Any())
                {
                    return false;
                }

                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = GetContractMeterListByContractReferenceAndMPXN(contractReference, mpxn);

                if(!matchingContractMeterIds.Any())
                {
                    return false;
                }
                
                //Get ContractMeterIds that exist in both lists
                var matchingContractBasketMeterIds = matchingContractMeterIds.Intersect(contractMeterIdFromBasketId);

                return matchingContractBasketMeterIds.Any();
            }
        }
    }
}