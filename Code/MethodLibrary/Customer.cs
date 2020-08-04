using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Customer
        {
            public List<long> Customer_GetCustomerIdList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Customer_GetList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .ToList();
            }

            public long Customer_GetCustomerIdByCustomerGUID(string customerGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Customer_GetByCustomerGUID, 
                    customerGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .FirstOrDefault();
            }

            public long CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(string customerAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerAttribute_GetByCustomerAttributeDescription, 
                    customerAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerAttributeId"))
                    .FirstOrDefault();
            }

            public long CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(long customerAttributeId, string customerDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription, 
                    customerAttributeId, customerDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerDetailId"))
                    .FirstOrDefault();
            }

            public long CustomerDetail_GetCustomerIdByCustomerAttributeIdAndCustomerDetailDescription(long customerAttributeId, string customerDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription, 
                    customerAttributeId, customerDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CustomerId"))
                    .FirstOrDefault();
            }

            public DataRow CustomerDetail_GetByCustomerIdAndCustomerAttributeId(long customerId, long customerAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerIdAndCustomerAttributeId, 
                    customerId, customerAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }

            public string CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(long customerId, long customerAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.CustomerDetail_GetByCustomerIdAndCustomerAttributeId, 
                    customerId, customerAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("CustomerDetailDescription"))
                    .FirstOrDefault();
            }

            public void CustomerDetail_DeleteByCustomerDetailId(long customerDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.CustomerDetail_DeleteByCustomerDetailId, 
                    customerDetailId);
            }

            public void Customer_Insert(long createdByUserId, long sourceId, string customerGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Customer_Insert, 
                    createdByUserId, sourceId, customerGUID);
            }

            public void CustomerDetail_Insert(long createdByUserId, long sourceId, long customerId, long customerAttributeId, string customerDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.CustomerDetail_Insert, 
                    createdByUserId, sourceId, customerId, customerAttributeId, customerDetailDescription);
            }

            public long MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription(long meterAttributeId, string meterDetailDescription)
            {
                return 0;
            }

            public long SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription(long subMeterAttributeId, string subMeterDetailDescription)
            {
                return 0;
            }

            public void InsertDataUploadValidationErrors(string processQueueGUID, long createdByUserId, long sourceId, string sheetName, Dictionary<int, Dictionary<string, List<string>>> validationErrors)
            {
                //Insert into DataUploadValidationError
                var dataUploadValidationErrorGUID = Guid.NewGuid().ToString();
                DataUploadValidationError_Insert(createdByUserId, sourceId, dataUploadValidationErrorGUID);

                //Get DataUploadValidationErrorId
                var dataUploadValidationErrorId = DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(dataUploadValidationErrorGUID);

                //Get DataUploadValidationErrorAttributes
                var sheetNameDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_dataUploadValidationAttributeEnums.SheetName);
                var rowNumberDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_dataUploadValidationAttributeEnums.RowNumber);
                var entityDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_dataUploadValidationAttributeEnums.Entity);
                var validationErrorMessageDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_dataUploadValidationAttributeEnums.ValidationErrorMessage);
                var processQueueGUIDDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_systemAPIRequiredDataKeyEnums.ProcessQueueGUID);

                //Insert into DataUploadValidationErrorDetail
                foreach(var validationError in validationErrors)
                {
                    var rowNumber = validationError.Key.ToString();
                    foreach(var validationErrorEntity in validationError.Value)
                    {
                        var entity = validationErrorEntity.Key;
                        foreach(var validationErrorMessage in validationErrorEntity.Value)
                        {
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, sheetNameDataUploadValidationErrorAttributeId, sheetName);
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, rowNumberDataUploadValidationErrorAttributeId, rowNumber);
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, entityDataUploadValidationErrorAttributeId, entity);
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, validationErrorMessageDataUploadValidationErrorAttributeId, processQueueGUID);
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, processQueueGUIDDataUploadValidationErrorAttributeId, validationErrorMessage);
                        }
                    }
                }
            }

            public void DataUploadValidationError_Insert(long createdByUserId, long sourceId, string dataUploadValidationErrorGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationError_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorGUID);
            }

            public void DataUploadValidationErrorDetail_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorId, long dataUploadValidationErrorAttributeId, string dataUploadValidationErrorDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationErrorDetail_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorId, dataUploadValidationErrorAttributeId, dataUploadValidationErrorDetailDescription);
            }

            public long DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(string dataUploadValidationErrorGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationError_GetByDataUploadValidationErrorGUID, 
                    dataUploadValidationErrorGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorId"))
                    .FirstOrDefault();
            }

            public long DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(string dataUploadValidationErrorAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorAttribute_GetByDataUploadValidationErrorAttributeDescription, 
                    dataUploadValidationErrorAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorAttributeId"))
                    .FirstOrDefault();
            }
        }
    }
}
