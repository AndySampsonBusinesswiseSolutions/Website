using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public long Page_GetPageIdByGUID(string pageGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Page_GetByPageGUID, 
                    pageGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PageId"))
                    .FirstOrDefault();
            }

            public string PageRequest_GetPageRequestResultByProcessQueueGUIDAndUserId(string processQueueGUID, long createdByUserId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.PageRequest_GetByProcessQueueGUIDAndUserId, 
                    processQueueGUID, createdByUserId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("PageRequestResult"))
                    .FirstOrDefault();
            }

            public void PageRequest_Insert(long createdByUserId, long sourceId, long pageId, string processQueueGUID, string pageRequestResult)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.PageRequest_Insert, 
                    createdByUserId, sourceId, pageId, processQueueGUID, pageRequestResult);
            }
        }
    }
}