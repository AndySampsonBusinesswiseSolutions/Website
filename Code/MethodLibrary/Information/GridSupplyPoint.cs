using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long GridSupplyPointAttribute_GetGridSupplyPointAttributeIdByGridSupplyPointAttributeDescription(string gridSupplyPointAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GridSupplyPointAttribute_GetByGridSupplyPointAttributeDescription, 
                    gridSupplyPointAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointAttributeId"))
                    .FirstOrDefault();
            }

            public long GridSupplyPointDetail_GetGridSupplyPointDetailIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(long gridSupplyPointAttributeId, string gridSupplyPointDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription, 
                    gridSupplyPointAttributeId, gridSupplyPointDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointDetailId"))
                    .FirstOrDefault();
            }

            public long GridSupplyPointDetail_GetGridSupplyPointIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(long gridSupplyPointAttributeId, string gridSupplyPointDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription, 
                    gridSupplyPointAttributeId, gridSupplyPointDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointId"))
                    .FirstOrDefault();
            }

            public void GridSupplyPoint_Insert(long createdByUserId, long sourceId, string gridSupplyPointGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.GridSupplyPoint_Insert, 
                    createdByUserId, sourceId, gridSupplyPointGUID);
            }

            public long GridSupplyPoint_GetGridSupplyPointIdByGridSupplyPointGUID(string gridSupplyPointGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GridSupplyPoint_GetByGridSupplyPointGUID, 
                    gridSupplyPointGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointId"))
                    .FirstOrDefault();
            }

            public void GridSupplyPointDetail_Insert(long createdByUserId, long sourceId, long gridSupplyPointId, long gridSupplyPointAttributeId, string gridSupplyPointDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.GridSupplyPointDetail_Insert, 
                    createdByUserId, sourceId, gridSupplyPointId, gridSupplyPointAttributeId, gridSupplyPointDetailDescription);
            }
        }
    }
}