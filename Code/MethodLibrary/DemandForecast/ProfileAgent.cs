using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class DemandForecast
        {
            // public long InsertNewProfileAgent(long createdByUserId, long sourceId)
            // {
            //     //Create new ProfileAgentGUID
            //     var GUID = Guid.NewGuid().ToString();

            //     while (ProfileAgent_GetProfileAgentIdByProfileAgentGUID(GUID) > 0)
            //     {
            //         GUID = Guid.NewGuid().ToString();
            //     }

            //     //Insert into [DemandForecast].[ProfileAgent]
            //     ProfileAgent_Insert(createdByUserId, sourceId, GUID);
            //     return ProfileAgent_GetProfileAgentIdByProfileAgentGUID(GUID);
            // }

            // public void ProfileAgent_Insert(long createdByUserId, long sourceId, string profileAgentGUID)
            // {
            //     ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
            //         _storedProcedureDemandForecastEnums.ProfileAgent_Insert, 
            //         createdByUserId, sourceId, profileAgentGUID);
            // }

            // public long ProfileAgent_GetProfileAgentIdByProfileAgentGUID(string profileAgentGUID)
            // {
            //     var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
            //         _storedProcedureDemandForecastEnums.ProfileAgent_GetByProfileAgentGUID, 
            //         profileAgentGUID);

            //     return dataTable.AsEnumerable()
            //         .Select(r => r.Field<long>("ProfileAgentId"))
            //         .FirstOrDefault();
            // }

            // public string ProfileAgent_GetProfileAgentGUIDByProfileAgentId(long profileAgentId)
            // {
            //     var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
            //         _storedProcedureDemandForecastEnums.ProfileAgent_GetByProfileAgentId, 
            //         profileAgentId);

            //     return dataTable.AsEnumerable()
            //         .Select(r => r.Field<Guid>("ProfileAgentGUID"))
            //         .FirstOrDefault().ToString();
            // }

            // public void ProfileAgentDetail_Insert(long createdByUserId, long sourceId, long profileAgentId, long profileAgentAttributeId, string profileAgentDetailDescription)
            // {
            //     ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
            //         _storedProcedureDemandForecastEnums.ProfileAgentDetail_Insert, 
            //         createdByUserId, sourceId, profileAgentId, profileAgentAttributeId, profileAgentDetailDescription);
            // }

            // public DataRow ProfileAgentDetail_GetByProfileAgentIdAndProfileAgentAttributeId(long profileAgentId, long profileAgentAttributeId)
            // {
            //     var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
            //         _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentIdAndProfileAgentAttributeId, 
            //         profileAgentId, profileAgentAttributeId);

            //     return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            // }

            // public void ProfileAgentDetail_DeleteByProfileAgentDetailId(long customerDetailId)
            // {
            //     ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
            //         _storedProcedureDemandForecastEnums.ProfileAgentDetail_DeleteByProfileAgentDetailId, 
            //         customerDetailId);
            // }

            public long ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(string profileAgentAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentAttribute_GetByProfileAgentAttributeDescription, 
                    profileAgentAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileAgentAttributeId"))
                    .FirstOrDefault();
            }

            public IEnumerable<string> ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentAttributeId(long profileAgentAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentAttributeId, 
                    profileAgentAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProfileAgentDetailDescription"))
                    .ToList();
            }

            public string ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentIdAndProfileAgentAttributeId(long profileAgentId, long profileAgentAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentIdAndProfileAgentAttributeId, 
                    profileAgentId, profileAgentAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProfileAgentDetailDescription"))
                    .FirstOrDefault();
            }

            public long ProfileAgentDetail_GetProfileAgentIdByProfileAgentAttributeIdAndProfileAgentDetailDescription(long profileAgentAttributeId, string profileAgentDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentAttributeIdAndProfileAgentDetailDescription, 
                    profileAgentAttributeId, profileAgentDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileAgentId"))
                    .First();
            }

            // public IEnumerable<DataRow> ProfileAgentDetail_GetListByProfileAgentId(long profileAgentId)
            // {
            //     var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
            //         _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentId, 
            //         profileAgentId);

            //     return dataTable.Rows.Cast<DataRow>();
            // }
        }
    }
}
