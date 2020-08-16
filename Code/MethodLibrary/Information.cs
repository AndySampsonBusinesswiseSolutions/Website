using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Information
        {
            public long SourceAttribute_GetSourceAttributeIdBySourceAttributeDescription(string sourceAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SourceAttribute_GetBySourceAttributeDescription, 
                    sourceAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceAttributeId"))
                    .FirstOrDefault();
            }

            public long GetSystemUserGeneratedSourceId()
            {
                var systemUserId = new Administration().User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
                var sourceAttributeId = SourceAttribute_GetSourceAttributeIdBySourceAttributeDescription(_informationSourceAttributeEnums.UserGenerated);

                return SourceDetail_GetSourceIdBySourceAttributeIdAndSourceDetailDescription(sourceAttributeId, systemUserId.ToString());
            }

            public long SourceDetail_GetSourceIdBySourceAttributeIdAndSourceDetailDescription(long sourceAttributeId, string sourceDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription, 
                    sourceAttributeId, sourceDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SourceId"))
                    .FirstOrDefault();
            }

            public void File_Insert(long createdByUserId, long sourceId, string fileGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.File_Insert, 
                    createdByUserId, sourceId, fileGUID);
            }

            public long File_GetFileIdByFileGUID(string fileGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.File_GetByFileGUID, 
                    fileGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileId"))
                    .FirstOrDefault();
            }

            public long FileAttribute_GetFileAttributeIdByFileAttributeDescription(string fileAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FileAttribute_GetByFileAttributeDescription, 
                    fileAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileAttributeId"))
                    .FirstOrDefault();
            }

            public void FileDetail_Insert(long createdByUserId, long sourceId, long fileId, long fileAttributeId, string fileDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.FileDetail_Insert, 
                    createdByUserId, sourceId, fileId, fileAttributeId, fileDetailDescription);
            }

            public void FileContent_Insert(long createdByUserId, long sourceId, long fileId, string fileContent)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.FileContent_Insert, 
                    createdByUserId, sourceId, fileId, fileContent);
            }

            public long FileType_GetFileTypeIdByFileTypeDescription(string fileTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FileType_GetByFileTypeDescription, 
                    fileTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("FileTypeId"))
                    .FirstOrDefault();
            }

            public string FileContent_GetFileContentByFileId(long fileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.FileContent_GetByFileId, 
                    fileId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("FileContent"))
                    .FirstOrDefault();
            }

            public string FileContent_GetFileContentByFileGUID(string fileGUID)
            {
                var fileId = File_GetFileIdByFileGUID(fileGUID);
                return FileContent_GetFileContentByFileId(fileId);;
            }

            public JObject FileContent_GetFileContentJSONByFileGUID(JObject jsonObject)
            {
                var fileGUID = jsonObject[_systemAPIRequiredDataKeyEnums.FileGUID].ToString();
                var fileContent = FileContent_GetFileContentByFileGUID(fileGUID);
                return JObject.Parse(fileContent);
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

            public long ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(string profileClassAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassAttribute_GetByProfileClassAttributeDescription, 
                    profileClassAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassAttributeId"))
                    .FirstOrDefault();
            }

            public long ProfileClassDetail_GetProfileClassDetailIdByProfileClassAttributeIdAndProfileClassDetailDescription(long profileClassAttributeId, string profileClassDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription, 
                    profileClassAttributeId, profileClassDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassDetailId"))
                    .FirstOrDefault();
            }

            public long ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(long profileClassAttributeId, string profileClassDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription, 
                    profileClassAttributeId, profileClassDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassId"))
                    .FirstOrDefault();
            }

            public void ProfileClass_Insert(long createdByUserId, long sourceId, string profileClassGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.ProfileClass_Insert, 
                    createdByUserId, sourceId, profileClassGUID);
            }

            public long ProfileClass_GetProfileClassIdByProfileClassGUID(string profileClassGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClass_GetByProfileClassGUID, 
                    profileClassGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassId"))
                    .FirstOrDefault();
            }

            public void ProfileClassDetail_Insert(long createdByUserId, long sourceId, long profileClassId, long profileClassAttributeId, string profileClassDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.ProfileClassDetail_Insert, 
                    createdByUserId, sourceId, profileClassId, profileClassAttributeId, profileClassDetailDescription);
            }

            public long MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(string meterTimeswitchCodeAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeAttribute_GetByMeterTimeswitchCodeAttributeDescription, 
                    meterTimeswitchCodeAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeAttributeId"))
                    .FirstOrDefault();
            }

            public long MeterTimeswitchCodeDetail_GetMeterTimeswitchCodeDetailIdByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription(long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription, 
                    meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeDetailId"))
                    .FirstOrDefault();
            }

            public long MeterTimeswitchCodeDetail_GetMeterTimeswitchCodeIdByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription(long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeIdAndMeterTimeswitchCodeDetailDescription, 
                    meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeId"))
                    .FirstOrDefault();
            }

            public void MeterTimeswitchCode_Insert(long createdByUserId, long sourceId, string meterTimeswitchCodeGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.MeterTimeswitchCode_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeGUID);
            }

            public long MeterTimeswitchCode_GetMeterTimeswitchCodeIdByMeterTimeswitchCodeGUID(string meterTimeswitchCodeGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCode_GetByMeterTimeswitchCodeGUID, 
                    meterTimeswitchCodeGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchCodeId"))
                    .FirstOrDefault();
            }

            public void MeterTimeswitchCodeDetail_Insert(long createdByUserId, long sourceId, long meterTimeswitchCodeId, long meterTimeswitchCodeAttributeId, string meterTimeswitchCodeDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeId, meterTimeswitchCodeAttributeId, meterTimeswitchCodeDetailDescription);
            }

            public DataTable MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(long meterTimeswitchCodeAttributeId)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId, 
                    meterTimeswitchCodeAttributeId);
            }

            public long LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(string localDistributionZoneAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.LocalDistributionZoneAttribute_GetByLocalDistributionZoneAttributeDescription, 
                    localDistributionZoneAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneAttributeId"))
                    .FirstOrDefault();
            }

            public long LocalDistributionZoneDetail_GetLocalDistributionZoneDetailIdByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription(long localDistributionZoneAttributeId, string localDistributionZoneDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription, 
                    localDistributionZoneAttributeId, localDistributionZoneDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneDetailId"))
                    .FirstOrDefault();
            }

            public long LocalDistributionZoneDetail_GetLocalDistributionZoneIdByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription(long localDistributionZoneAttributeId, string localDistributionZoneDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription, 
                    localDistributionZoneAttributeId, localDistributionZoneDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneId"))
                    .FirstOrDefault();
            }

            public void LocalDistributionZone_Insert(long createdByUserId, long sourceId, string localDistributionZoneGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.LocalDistributionZone_Insert, 
                    createdByUserId, sourceId, localDistributionZoneGUID);
            }

            public long LocalDistributionZone_GetLocalDistributionZoneIdByLocalDistributionZoneGUID(string localDistributionZoneGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.LocalDistributionZone_GetByLocalDistributionZoneGUID, 
                    localDistributionZoneGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneId"))
                    .FirstOrDefault();
            }

            public void LocalDistributionZoneDetail_Insert(long createdByUserId, long sourceId, long localDistributionZoneId, long localDistributionZoneAttributeId, string localDistributionZoneDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.LocalDistributionZoneDetail_Insert, 
                    createdByUserId, sourceId, localDistributionZoneId, localDistributionZoneAttributeId, localDistributionZoneDetailDescription);
            }

            public long MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(string meterExemptionAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionAttribute_GetByMeterExemptionAttributeDescription, 
                    meterExemptionAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionAttributeId"))
                    .FirstOrDefault();
            }

            public long MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(long meterExemptionAttributeId, string meterExemptionDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionDetail_GetByMeterExemptionAttributeIdAndMeterExemptionDetailDescription, 
                    meterExemptionAttributeId, meterExemptionDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionId"))
                    .FirstOrDefault();
            }

            public string MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(long meterExemptionId, long meterExemptionAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterExemptionDetail_GetByMeterExemptionIdAndMeterExemptionAttributeId, 
                    meterExemptionId, meterExemptionAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("MeterExemptionDetailDescription"))
                    .FirstOrDefault();
            }

            public List<string> Granularity_GetGranularityCodeList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("GranularityCode"))
                    .ToList();
            }

            public long Area_GetAreaIdByAreaDescription(string areaDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Area_GetByAreaDescription, 
                    areaDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AreaId"))
                    .FirstOrDefault();
            }

            public void Area_Insert(long createdByUserId, long sourceId, string areaDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.Area_Insert, 
                    createdByUserId, sourceId, areaDescription);
            }

            public long SubArea_GetSubAreaIdBySubAreaDescription(string subAreaDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.SubArea_GetBySubAreaDescription, 
                    subAreaDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubAreaId"))
                    .FirstOrDefault();
            }

            public void SubArea_Insert(long createdByUserId, long sourceId, string subAreaDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.SubArea_Insert, 
                    createdByUserId, sourceId, subAreaDescription);
            }

            public long ContractType_GetContractTypeIdByContractTypeDescription(string contractTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ContractType_GetByContractTypeDescription, 
                    contractTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractTypeId"))
                    .FirstOrDefault();
            }

            public long Commodity_GetCommodityIdByCommodityDescription(string commodityDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Commodity_GetByCommodityDescription, 
                    commodityDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityId"))
                    .FirstOrDefault();
            }

            public void Commodity_Insert(long createdByUserId, long sourceId, string commodityDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureInformationEnums.Commodity_Insert, 
                    createdByUserId, sourceId, commodityDescription);
            }

            public long RateType_GetRateTypeIdByRateTypeDescription(string rateTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RateType_GetByRateTypeDescription, 
                    rateTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateTypeId"))
                    .FirstOrDefault();
            }
        }
    }
}
