using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public void ProcessQueueProgression_Insert(long createdByUserId, long sourceId, string fromProcessQueueGUID, string toProcessQueueGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSystemEnums.ProcessQueueProgression_Insert, 
                    createdByUserId, sourceId, fromProcessQueueGUID, toProcessQueueGUID);
            }
        }
    }
}