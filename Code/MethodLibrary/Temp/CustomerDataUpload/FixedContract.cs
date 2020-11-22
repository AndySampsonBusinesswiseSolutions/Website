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
                public class FixedContract
                {
                    public void FixedContract_Insert(string processQueueGUID, int rowId, string contractReference, string MPXN, string supplier, string contractStartDate, string contractEndDate, string product, string rateCount, string rateType, string value)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.FixedContract_Insert, 
                            processQueueGUID, rowId, contractReference, MPXN, supplier, contractStartDate, contractEndDate, product, rateCount, rateType, value);
                    }

                    public List<DataRow> FixedContract_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FixedContract_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.FixedContract> FixedContract_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.FixedContract_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.FixedContract(d)).ToList();
                    }

                    public void FixedContract_DeleteByProcessQueueGUID(string processQueueGUID)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.FixedContract_DeleteByProcessQueueGUID, 
                            processQueueGUID);
                    }
                }
            }
        }
    }
}