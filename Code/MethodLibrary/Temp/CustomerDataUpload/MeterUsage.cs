using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public class MeterUsage
                {
                    public List<DataRow> MeterUsage_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.MeterUsage_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.Temp.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.MeterUsage> MeterUsage_GetMeterUsageEntityListByProcessQueueGUID(string processQueueGUID)
                    {
                        return MeterUsage_GetByProcessQueueGUID(processQueueGUID).Select(dataRow => new Entity.Temp.CustomerDataUpload.MeterUsage(dataRow)).ToList();
                    }

                    public void MeterUsage_DeleteByProcessQueueGUID(string processQueueGUID)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.MeterUsage_DeleteByProcessQueueGUID, 
                            processQueueGUID);
                    }
                }
            }
        }
    }
}