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
            public long InsertNewContractMeter(long createdByUserId, long sourceId)
            {
                //Create new ContractMeterGUID
                var GUID = Guid.NewGuid().ToString();

                while (ContractMeter_GetContractMeterIdByContractMeterGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[ContractMeter]
                ContractMeter_Insert(createdByUserId, sourceId, GUID);
                return ContractMeter_GetContractMeterIdByContractMeterGUID(GUID);
            }

            public long ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(string contractMeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterAttribute_GetByContractMeterAttributeDescription, 
                    contractMeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterAttributeId"))
                    .FirstOrDefault();
            }

            public void ContractMeter_Insert(long createdByUserId, long sourceId, string contractMeterGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeter_Insert, 
                    createdByUserId, sourceId, contractMeterGUID);
            }

            public long ContractMeter_GetContractMeterIdByContractMeterGUID(string contractMeterGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeter_GetByContractMeterGUID, 
                    contractMeterGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .FirstOrDefault();
            }

            public void ContractMeterDetail_Insert(long createdByUserId, long sourceId, long contractMeterId, long contractMeterAttributeId, string contractMeterDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterDetail_Insert, 
                    createdByUserId, sourceId, contractMeterId, contractMeterAttributeId, contractMeterDetailDescription);
            }

            public List<long> ContractMeterDetail_GetContractMeterIdListByContractMeterAttributeIdAndContractMeterDetailDescription(long contractMeterAttributeId, string contractMeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterDetail_GetByContractMeterAttributeIdAndContractMeterDetailDescription, 
                    contractMeterAttributeId, contractMeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }
            
            private List<long> GetContractMeterListByContractReferenceAndMPXN(string contractReference, string mpxn)
            {
                //Get ContractId from ContractReference
                var contractReferenceContractAttributeId = ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var contractId = ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference);

                //If ContractId == 0 then not valid
                if(contractId == 0)
                {
                    return new List<long>();
                }

                //Get MeterId from MPXN
                var meterIdentifierMeterAttributeId = MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterId = MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                //If MeterId == 0 then not valid
                if(meterId == 0)
                {
                    return new List<long>();
                }

                //Get ContractMeterIds from ContractId
                var contractMeterIdFromContractId = new Mapping().ContractToContractMeter_GetContractMeterIdListByContractId(contractId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromContractId.Any())
                {
                    return new List<long>();
                }

                //Get ContractMeterIds from MeterId
                var contractMeterIdFromMeterId = new Mapping().ContractMeterToMeter_GetContractMeterIdListByMeterId(meterId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromMeterId.Any())
                {
                    return new List<long>();
                }

                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = contractMeterIdFromContractId.Intersect(contractMeterIdFromMeterId).ToList();

                return matchingContractMeterIds;
            }

            public bool ContractMeterExists(string contractReference, string mpxn)
            {
                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = GetContractMeterListByContractReferenceAndMPXN(contractReference, mpxn);

                return matchingContractMeterIds.Any();
            }
        }
    }
}
