using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long ContractType_GetContractTypeIdByContractTypeDescription(string contractTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ContractType_GetByContractTypeDescription, 
                    contractTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractTypeId"))
                    .FirstOrDefault();
            }
        }
    }
}