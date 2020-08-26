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
            public long InsertNewContractMeterRate(long createdByUserId, long sourceId)
            {
                //Create new ContractMeterRateGUID
                var GUID = Guid.NewGuid().ToString();

                while (ContractMeterRate_GetContractMeterRateIdByContractMeterRateGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[ContractMeterRate]
                ContractMeterRate_Insert(createdByUserId, sourceId, GUID);
                return ContractMeterRate_GetContractMeterRateIdByContractMeterRateGUID(GUID);
            }

            public long ContractMeterRateAttribute_GetContractMeterRateAttributeIdByContractMeterRateAttributeDescription(string contractMeterRateAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRateAttribute_GetByContractMeterRateAttributeDescription, 
                    contractMeterRateAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateAttributeId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRate_Insert(long createdByUserId, long sourceId, string contractMeterRateGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterRate_Insert, 
                    createdByUserId, sourceId, contractMeterRateGUID);
            }

            public long ContractMeterRate_GetContractMeterRateIdByContractMeterRateGUID(string contractMeterRateGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRate_GetByContractMeterRateGUID, 
                    contractMeterRateGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRateDetail_Insert(long createdByUserId, long sourceId, long contractMeterRateId, long contractMeterRateAttributeId, string contractMeterRateDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterRateDetail_Insert, 
                    createdByUserId, sourceId, contractMeterRateId, contractMeterRateAttributeId, contractMeterRateDetailDescription);
            }

            public long ContractMeterRateDetail_GetContractMeterRateIdByContractMeterRateAttributeIdAndContractMeterRateDetailDescription(long contractMeterRateAttributeId, string contractMeterRateDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRateDetail_GetByContractMeterRateAttributeIdAndContractMeterRateDetailDescription, 
                    contractMeterRateAttributeId, contractMeterRateDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateId"))
                    .FirstOrDefault();
            }
        }
    }
}
