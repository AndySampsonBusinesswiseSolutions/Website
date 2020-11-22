using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class TempSchema
        {
            public partial class CustomerDataUpload
            {
                public class FlexReferenceVolume
                {
                    public void FlexReferenceVolume_Insert(string processQueueGUID, int rowId, string contractReference, string dateFrom, string dateTo, string volume)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.FlexReferenceVolume_Insert, 
                            processQueueGUID, rowId, contractReference, dateFrom, dateTo, volume);
                    }

                    public List<DataRow> FlexReferenceVolume_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FlexReferenceVolume_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.FlexReferenceVolume> FlexReferenceVolume_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FlexReferenceVolume_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.FlexReferenceVolume(d)).ToList();
                    }

                    public void FlexReferenceVolume_DeleteByProcessQueueGUID(string processQueueGUID)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.FlexReferenceVolume_DeleteByProcessQueueGUID, 
                            processQueueGUID);
                    }
                }
            }
        }
    }
}