using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(string granularityAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GranularityAttribute_GetByGranularityAttributeDescription, 
                    granularityAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityAttributeId"))
                    .FirstOrDefault();
            }

            public long GranularityDetail_GetGranularityIdByGranularityAttributeId(long granularityAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GranularityDetail_GetByGranularityAttributeId, 
                    granularityAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .FirstOrDefault();
            }

            public string GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(long granularityId, long granularityAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GranularityDetail_GetByGranularityIdAndGranularityAttributeId, 
                    granularityId, granularityAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("GranularityDetailDescription"))
                    .FirstOrDefault();
            }

            public List<string> GranularityDetail_GetGranularityDetailDescriptionListByGranularityAttributeId(long granularityAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GranularityDetail_GetByGranularityAttributeId, 
                    granularityAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("GranularityDetailDescription"))
                    .ToList();
            }

            public long GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(long granularityAttributeId, string granularityDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GranularityDetail_GetByGranularityAttributeIdAndGranularityDetailDescription, 
                    granularityAttributeId, granularityDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .FirstOrDefault();
            }

            public void Granularity_Insert(long createdByUserId, long sourceId, string granularityGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.Granularity_Insert, 
                    createdByUserId, sourceId, granularityGUID);
            }

            public long Granularity_GetGranularityIdByGranularityGUID(string granularityGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetByGranularityGUID, 
                    granularityGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .FirstOrDefault();
            }

            public void GranularityDetail_Insert(long createdByUserId, long sourceId, long granularityId, long granularityAttributeId, string granularityDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.GranularityDetail_Insert, 
                    createdByUserId, sourceId, granularityId, granularityAttributeId, granularityDetailDescription);
            }

            public List<long> Granularity_GetGranularityIdList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetByList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .ToList();
            }
        }
    }
}