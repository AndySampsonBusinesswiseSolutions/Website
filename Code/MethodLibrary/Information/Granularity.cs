using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long GetGranularityIdByGranularityCode(string granularityCode)
            {
                var granularityCodeGranularityAttributeId = GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(new Enums.InformationSchema.Granularity.Attribute().GranularityCode);
                return GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);
            }
            public string GetGranularityCodeByGranularityDisplayDescription(string granularityDisplayDescription)
            {
                var granularityDisplayDescriptionGranularityAttributeId = GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDisplayDescription);
                var granularityId = GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityDisplayDescriptionGranularityAttributeId, granularityDisplayDescription);

                var granularityCodeGranularityAttributeId = GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
                return GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityCodeGranularityAttributeId);
            }

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

            public List<long> Granularity_GetGranularityIdList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetByList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .ToList();
            }

            public string GetDefaultGranularityDescriptionByCommodity(string granularityAttributeDescription)
            {
                var granularityDefaultGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(granularityAttributeDescription);
                var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);
                var granularityDescriptionGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDescription);
                return _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityDescriptionGranularityAttributeId);
            }
        }
    }
}