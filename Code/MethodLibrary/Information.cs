using System.Data;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

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

            public long GridSupplyPointDetail_GetGridSupplyPointIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(long gridSupplyPointAttributeId, string gridSupplyPointDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.GridSupplyPointDetail_GetByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription, 
                    gridSupplyPointAttributeId, gridSupplyPointDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointDetailId"))
                    .FirstOrDefault();
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

            public long ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(long profileClassAttributeId, string profileClassDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.ProfileClassDetail_GetByProfileClassAttributeIdAndProfileClassDetailDescription, 
                    profileClassAttributeId, profileClassDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileClassDetailId"))
                    .FirstOrDefault();
            }

            public long MeterTimeswitchClassAttribute_GetMeterTimeswitchClassAttributeIdByMeterTimeswitchClassAttributeDescription(string meterTimeswitchClassAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchClassAttribute_GetByMeterTimeswitchClassAttributeDescription, 
                    meterTimeswitchClassAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterTimeswitchClassAttributeId"))
                    .FirstOrDefault();
            }

            public DataTable MeterTimeswitchClassDetail_GetByMeterTimeswitchClassAttributeId(long meterTimeswitchClassAttributeId)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.MeterTimeswitchClassDetail_GetByMeterTimeswitchClassAttributeId, 
                    meterTimeswitchClassAttributeId);
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

            public long LocalDistributionZoneDetail_GetLocalDistributionZoneIdByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription(long localDistributionZoneAttributeId, string localDistributionZoneDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.LocalDistributionZoneDetail_GetByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription, 
                    localDistributionZoneAttributeId, localDistributionZoneDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneDetailId"))
                    .FirstOrDefault();
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
        }
    }
}
