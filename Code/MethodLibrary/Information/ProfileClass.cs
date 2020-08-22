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
            public long InsertNewProfileClass(long createdByUserId, long sourceId)
            {
                //Create new ProfileClassGUID
                var GUID = Guid.NewGuid().ToString();

                while (ProfileClass_GetProfileClassIdByProfileClassGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[ProfileClass]
                ProfileClass_Insert(createdByUserId, sourceId, GUID);
                return ProfileClass_GetProfileClassIdByProfileClassGUID(GUID);
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
        }
    }
}