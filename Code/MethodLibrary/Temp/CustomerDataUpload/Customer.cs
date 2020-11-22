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
                public class Customer
                {
                    public void Customer_Insert(string processQueueGUID, int rowId, string customerName, string contactName, string contactTelephoneNumber, string contactEmailAddress)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.Customer_Insert, 
                            processQueueGUID, rowId, customerName, contactName, contactTelephoneNumber, contactEmailAddress);
                    }

                    //TODO: Remove
                    public List<DataRow> Customer_GetDataRowsByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.Customer_GetByProcessQueueGUID, 
                            processQueueGUID);

                        return new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                    }

                    public List<Entity.Temp.CustomerDataUpload.Customer> Customer_GetByProcessQueueGUID(string processQueueGUID)
                    {
                        var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                            _storedProcedureTempCustomerDataUploadEnums.Customer_GetByProcessQueueGUID, 
                            processQueueGUID);

                        var dataRows = new Methods.TempSchema.CustomerDataUpload().CleanedUpDataTable(dataTable);
                        return dataRows.Select(d => new Entity.Temp.CustomerDataUpload.Customer(d)).ToList();
                    }

                    public void Customer_DeleteByProcessQueueGUID(string processQueueGUID)
                    {
                        ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                            _storedProcedureTempCustomerDataUploadEnums.Customer_DeleteByProcessQueueGUID, 
                            processQueueGUID);
                    }
                }
            }
        }
    }
}