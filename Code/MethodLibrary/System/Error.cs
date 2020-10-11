using System.Data;
using System.Linq;
using System;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public long InsertSystemError(long createdByUserId, long sourceId, Exception error)
            {
                return InsertSystemError(createdByUserId,
                    sourceId,
                    error.Message.ToString(),
                    error.GetType().Name.ToString(),
                    error.StackTrace.ToString());
            }

            public long InsertSystemError(long createdByUserId, long sourceId, string errorMessage, string errorType, string errorSource)
            {
                var errorGUID = Guid.NewGuid().ToString();
                
                while (Error_GetErrorIdByErrorGUID(errorGUID) > 0)
                {
                    errorGUID = Guid.NewGuid().ToString();
                }

                Error_Insert(createdByUserId,
                    sourceId,
                    errorGUID,
                    errorMessage,
                    errorType,
                    errorSource);

                return Error_GetErrorIdByErrorGUID(errorGUID);
            }

            public void Error_Insert(long createdByUserId, long sourceId, string errorGUID, string errorMessage, string errorType, string errorSource)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.Error_Insert, 
                    createdByUserId, sourceId, errorGUID, errorMessage, errorType, errorSource);
            }

            public long Error_GetErrorIdByErrorGUID(string errorGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Error_GetByErrorGUID, 
                    errorGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ErrorId"))
                    .FirstOrDefault();
            }
        }
    }
}