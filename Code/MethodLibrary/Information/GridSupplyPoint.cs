using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long InsertNewGridSupplyPoint(long createdByUserId, long sourceId)
            {
                //Create new GridSupplyPointGUID
                var GUID = Guid.NewGuid().ToString();

                while (GridSupplyPoint_GetGridSupplyPointIdByGridSupplyPointGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[GridSupplyPoint]
                GridSupplyPoint_Insert(createdByUserId, sourceId, GUID);
                return GridSupplyPoint_GetGridSupplyPointIdByGridSupplyPointGUID(GUID);
            }

            public long GetGridSupplyPointId(string gridSupplyPoint, long createdByUserId, long sourceId, long gridSupplyPointGroupIdGridSupplyPointAttributeId)
            {
                var gridSupplyPointId = GridSupplyPointDetail_GetGridSupplyPointIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(gridSupplyPointGroupIdGridSupplyPointAttributeId, gridSupplyPoint);

                if(gridSupplyPointId == 0)
                {
                    gridSupplyPointId = InsertNewGridSupplyPoint(createdByUserId, sourceId);

                    //Insert into [Customer].[GridSupplyPointDetail]
                    GridSupplyPointDetail_Insert(createdByUserId, sourceId, gridSupplyPointId, gridSupplyPointGroupIdGridSupplyPointAttributeId, gridSupplyPoint);
                }

                return gridSupplyPointId;
            }

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