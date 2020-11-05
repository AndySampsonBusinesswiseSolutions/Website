using System.Reflection;
using System.Linq;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public class HostEnvironment
            {
                public long GetHostEnvironmentIdByHostEnvironmentName(string hostEnvironmentName)
                {
                    //Get hostEnvironmentNameHostEnvironmentAttributeId
                    var hostEnvironmentNameHostEnvironmentAttributeId = HostEnvironmentAttribute_GetHostEnvironmentAttributeIdByHostEnvironmentAttributeDescription(_systemHostEnvironmentAttributeEnums.HostEnvironmentName);

                    return HostEnvironment_GetHostEnvironmentIdByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription(hostEnvironmentNameHostEnvironmentAttributeId, hostEnvironmentName);
                }

                public string GetHostEnvironmentURLByHostEnvironmentId(long hostEnvironmentId)
                {
                    //Get hostEnvironmentURLHostEnvironmentAttributeId
                    var hostEnvironmentURLHostEnvironmentAttributeId = HostEnvironmentAttribute_GetHostEnvironmentAttributeIdByHostEnvironmentAttributeDescription(_systemHostEnvironmentAttributeEnums.HostEnvironmentURL);

                    return HostEnvironment_GetHostEnvironmentDetailDescriptionByHostEnvironmentIdAndHostEnvironmentAttributeId(hostEnvironmentId, hostEnvironmentURLHostEnvironmentAttributeId);
                }

                public long HostEnvironmentAttribute_GetHostEnvironmentAttributeIdByHostEnvironmentAttributeDescription(string hostEnvironmentAttributeDescription)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureSystemHostEnvironmentEnums.HostEnvironmentAttribute_GetByHostEnvironmentAttributeDescription, 
                        hostEnvironmentAttributeDescription);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<long>("HostEnvironmentAttributeId"))
                        .FirstOrDefault();
                }

                public long HostEnvironment_GetHostEnvironmentIdByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription(long hostEnvironmentAttributeId, string hostEnvironmentDetailDescription)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureSystemHostEnvironmentEnums.HostEnvironmentDetail_GetByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription, 
                        hostEnvironmentAttributeId, hostEnvironmentDetailDescription);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<long>("HostEnvironmentId"))
                        .FirstOrDefault();
                }

                public string HostEnvironment_GetHostEnvironmentDetailDescriptionByHostEnvironmentIdAndHostEnvironmentAttributeId(long hostEnvironmentId, long hostEnvironmentAttributeId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureSystemHostEnvironmentEnums.HostEnvironmentDetail_GetByHostEnvironmentIdAndHostEnvironmentAttributeId, 
                        hostEnvironmentId, hostEnvironmentAttributeId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<string>("HostEnvironmentDetailDescription"))
                        .FirstOrDefault();
                }
            }
        }
    }
}