using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(long contractMeterRateId, long rateTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterRateToRateType_GetByContractMeterRateIdAndRateTypeId, 
                    contractMeterRateId, rateTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateToRateTypeId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRateToRateType_Insert(long createdByUserId, long sourceId, long contractMeterRateId, long rateTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterRateToRateType_Insert, 
                    createdByUserId, sourceId, contractMeterRateId, rateTypeId);
            }
        }
    }
}