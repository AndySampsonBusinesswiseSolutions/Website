using System.Data;
using System.Linq;
using System;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SystemSchema
        {
            public void InsertProcessQueueError(string processQueueGUID, long createdByUserId, long sourceId, long APIId, string errorMessage = null)
            {
                var errorId = InsertSystemError(createdByUserId, 
                                sourceId, 
                                $"API {APIId} Not Started - {errorMessage}",
                                "API Not Started",
                                Environment.StackTrace);
                    
                ProcessQueue_Insert(
                        processQueueGUID, 
                        createdByUserId,
                        sourceId,
                        APIId);

                ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, APIId, true, $"System Error Id {errorId}");
            }

            public void ProcessQueue_Insert(string processQueueGUID, long createdByUserId, long sourceId, long APIId, bool hasError = false, string errorMessage = null)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Insert, 
                    processQueueGUID, createdByUserId, sourceId, APIId, hasError, errorMessage);
            }

            public void ProcessQueue_UpdateEffectiveFromDateTime(string processQueueGUID, long APIId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_UpdateEffectiveFromDateTime, 
                    processQueueGUID, APIId);
            }

            public void ProcessQueue_UpdateEffectiveToDateTime(string processQueueGUID, long APIId, bool hasError = false, string errorMessage = null)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_UpdateEffectiveToDateTime, 
                    processQueueGUID, APIId, hasError, errorMessage);
            }

            public DataRow ProcessQueue_GetByProcessQueueGUIDAndAPIId(string processQueueGUID, long apiId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetByProcessQueueGUIDAndAPIId, 
                    processQueueGUID, apiId);

                return dataTable.AsEnumerable()
                    .Select(r => r)
                    .FirstOrDefault();
            }

            public DataTable ProcessQueue_GetByProcessQueueGUID(string processQueueGUID)
            {
                return GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetByProcessQueueGUID, 
                    processQueueGUID);
            }

            public bool ProcessQueue_GetHasErrorByProcessQueueGUID(string processQueueGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetHasErrorByProcessQueueGUID, 
                    processQueueGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("HasError"))
                    .FirstOrDefault();
            }

            public bool ProcessQueue_GetHasSystemErrorByProcessQueueGUID(string processQueueGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.ProcessQueue_GetHasSystemErrorByProcessQueueGUID, 
                    processQueueGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<bool>("HasError"))
                    .FirstOrDefault();
            }

            public void ProcessQueue_Delete(string processQueueGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueue_Delete, 
                    processQueueGUID);
            }
        }
    }
}