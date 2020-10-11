using System.Data;
using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public void ProcessArchive_Insert(long createdByUserId, long sourceId, string processArchiveGUID, bool hasError)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Insert, 
                    createdByUserId, sourceId, processArchiveGUID, hasError);
            }

            public void ProcessArchive_Update(string processArchiveGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchive_Update, 
                    processArchiveGUID);
            }

            public long ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(string processArchiveGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchive_GetByProcessArchiveGUID, 
                    processArchiveGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveId"))
                    .FirstOrDefault();
            }

            public long ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(string processArchiveAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription, 
                    processArchiveAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveAttributeId"))
                    .First();
            }

            public string ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(long processArchiveDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveDetailId, 
                    processArchiveDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                    .First();
            }

            public List<long> ProcessArchiveDetail_GetProcessArchiveDetailIdListByProcessArchiveIDAndProcessArchiveAttributeId(long processArchiveId, long processArchiveAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId, 
                    processArchiveId, processArchiveAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .ToList();
            }

            public List<string> ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(long processArchiveId, long processArchiveAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId, 
                    processArchiveId, processArchiveAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProcessArchiveDetailDescription"))
                    .ToList();
            }

            public long ProcessArchiveDetail_GetProcessArchiveDetailIdByEffectiveFromDateTimeAndEffectiveToDateTimeAndProcessArchiveDetailDescription(string effectiveFromDateTime, string effectiveToDateTime, string processArchiveDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription,
                    effectiveFromDateTime, effectiveToDateTime, processArchiveDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .FirstOrDefault();
            }

            public void ProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long processArchiveId, long processArchiveAttributeId, string processArchiveDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, processArchiveId, processArchiveAttributeId, processArchiveDetailDescription);
            }

            public void ProcessArchiveDetail_InsertAll(DateTime effectiveFromDateTime, DateTime effectiveToDateTime, long createdByUserId, long sourceId, long processArchiveId, long processArchiveAttributeId, string processArchiveDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessArchiveDetail_InsertAll, 
                    effectiveFromDateTime, effectiveToDateTime, createdByUserId, sourceId, processArchiveId, processArchiveAttributeId, processArchiveDetailDescription);
            }
        }
    }
}