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
                public class MeterExemption
                {
                    public void MeterExemption_Insert(string processQueueGUID, int rowId, string MPXN, string dateFrom, string dateTo, string exemptionProduct, string exemptionProportion)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.MeterExemption_Insert, 
                            processQueueGUID, rowId, MPXN, dateFrom, dateTo, exemptionProduct, exemptionProportion);
                    }

                    public List<DataRow> MeterExemption_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.MeterExemption_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.MeterExemption> MeterExemption_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.MeterExemption_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.MeterExemption(d)).ToList();
                    }

                    public void MeterExemption_DeleteByProcessQueueGUID(string processQueueGUID)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.MeterExemption_DeleteByProcessQueueGUID, 
                            processQueueGUID);
                    }
                }
            }
        }
    }
}