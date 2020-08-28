using System.Reflection;
using System.Data;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public void SubMeter_Insert(string processQueueGUID, int rowId, string MPXN, string subMeterIdentifier, string serialNumber, string subArea, string asset)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.SubMeter_Insert, 
                        processQueueGUID, rowId, MPXN, subMeterIdentifier, serialNumber, subArea, asset);
                }

                public IEnumerable<DataRow> SubMeter_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.SubMeter_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void SubMeter_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.SubMeter_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}