using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long InsertNewLocalDistributionZone(long createdByUserId, long sourceId)
            {
                //Create new LocalDistributionZoneGUID
                var GUID = Guid.NewGuid().ToString();

                while (LocalDistributionZone_GetLocalDistributionZoneIdByLocalDistributionZoneGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[LocalDistributionZone]
                LocalDistributionZone_Insert(createdByUserId, sourceId, GUID);
                return LocalDistributionZone_GetLocalDistributionZoneIdByLocalDistributionZoneGUID(GUID);
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
        }
    }
}