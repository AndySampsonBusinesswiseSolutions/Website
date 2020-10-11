using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(string profileClassAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassAttribute_GetByProfileClassAttributeDescription, 
                    profileClassAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassAttributeId"))
                    .FirstOrDefault();
            }

            public long ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(long profileClassAttributeId, string profileClassDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription, 
                    profileClassAttributeId, profileClassDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassId"))
                    .First();
            }
        }
    }
}