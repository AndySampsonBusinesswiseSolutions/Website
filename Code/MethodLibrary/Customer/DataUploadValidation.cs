using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class CustomerSchema
        {
            public void InsertDataUploadValidationErrors(string processQueueGUID, long createdByUserId, long sourceId, string sheetName, Dictionary<int, Dictionary<string, List<string>>> validationErrors)
            {
                 //Insert into DataUploadValidationError
                DataUploadValidationError_Insert(createdByUserId, sourceId, processQueueGUID);

                //Get DataUploadValidationErrorId
                var dataUploadValidationErrorId = DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(processQueueGUID);

                //Get DataUploadValidationErrorSheetAttributeId
                var dataUploadValidationErrorSheetAttributeId = DataUploadValidationErrorSheetAttribute_GetDataUploadValidationErrorSheetAttributeIdByDataUploadValidationErrorSheetAttributeDescription(sheetName);

                //Insert into DataUploadValidationErrorSheet - this links sheet to file
                DataUploadValidationErrorSheet_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, dataUploadValidationErrorSheetAttributeId);

                //Get DataUploadValidationErrorSheetId
                var dataUploadValidationErrorSheetId = DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetIdByDataUploadValidationErrorIdAndDataUploadValidationErrorSheetAttributeId(dataUploadValidationErrorId, dataUploadValidationErrorSheetAttributeId);

                foreach(var validationError in validationErrors)
                {
                    //Insert into DataUploadValidationErrorRow - this links row to sheet
                    var rowNumber = validationError.Key;
                    DataUploadValidationErrorRow_Insert(createdByUserId, sourceId, dataUploadValidationErrorSheetId, rowNumber);

                    //Get DataUploadValidationErrorRowId
                    var dataUploadValidationErrorRowId = DataUploadValidationErrorRow_GetDataUploadValidationErrorRowIdByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow(dataUploadValidationErrorSheetId, rowNumber);

                    foreach(var validationErrorEntity in validationError.Value)
                    {
                        //Get DataUploadValidationErrorEntityAttributeId
                        var dataUploadValidationErrorEntityAttributeId = DataUploadValidationErrorEntityAttribute_GetDataUploadValidationErrorEntityAttributeIdByDataUploadValidationErrorEntityAttributeDescription(validationErrorEntity.Key);

                        //Insert into DataUploadValidationErrorEntity - this links entity to row
                        DataUploadValidationErrorEntity_Insert(createdByUserId, sourceId, dataUploadValidationErrorRowId, dataUploadValidationErrorEntityAttributeId);

                        //Get DataUploadValidationErrorEntityId
                        var dataUploadValidationErrorEntityId = DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityIdByDataUploadValidationErrorRowIdAndDataUploadValidationErrorEntityAttributeId(dataUploadValidationErrorRowId, dataUploadValidationErrorEntityAttributeId);

                        foreach(var validationErrorMessage in validationErrorEntity.Value)
                        {
                            //Insert into DataUploadValidationErrorMessage - this links message to entity
                            DataUploadValidationErrorMessage_Insert(createdByUserId, sourceId, dataUploadValidationErrorEntityId, validationErrorMessage);
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

            public long DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(string dataUploadValidationErrorGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationError_GetByDataUploadValidationErrorGUID, 
                    dataUploadValidationErrorGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorId"))
                    .FirstOrDefault();
            }

            public long DataUploadValidationErrorSheetAttribute_GetDataUploadValidationErrorSheetAttributeIdByDataUploadValidationErrorSheetAttributeDescription(string dataUploadValidationErrorSheetAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheetAttribute_GetByDataUploadValidationErrorSheetAttributeDescription, 
                    dataUploadValidationErrorSheetAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorSheetAttributeId"))
                    .FirstOrDefault();
            }

            public string DataUploadValidationErrorSheetAttribute_GetDataUploadValidationErrorSheetAttributeDescriptionByDataUploadValidationErrorSheetAttributeId(long dataUploadValidationErrorSheetAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheetAttribute_GetByDataUploadValidationErrorSheetAttributeId, 
                    dataUploadValidationErrorSheetAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("DataUploadValidationErrorSheetAttributeDescription"))
                    .FirstOrDefault();
            }

            public void DataUploadValidationErrorSheet_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorId, long dataUploadValidationErrorSheetAttributeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheet_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorId, dataUploadValidationErrorSheetAttributeId);
            }

            public long DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetIdByDataUploadValidationErrorIdAndDataUploadValidationErrorSheetAttributeId(long dataUploadValidationErrorId, long dataUploadValidationErrorSheetAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheet_GetByDataUploadValidationErrorIdAndDataUploadValidationErrorSheetAttributeId, 
                    dataUploadValidationErrorId, dataUploadValidationErrorSheetAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorSheetId"))
                    .FirstOrDefault();
            }

            public List<long> DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetIdListByDataUploadValidationErrorId(long dataUploadValidationErrorId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheet_GetByDataUploadValidationErrorId, 
                    dataUploadValidationErrorId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorSheetId"))
                    .ToList();
            }

            public long DataUploadValidationErrorSheet_GetDataUploadValidationErrorSheetAttributeIdByDataUploadValidationErrorSheetId(long dataUploadValidationErrorSheetId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorSheet_GetByDataUploadValidationErrorSheetId, 
                    dataUploadValidationErrorSheetId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorSheetAttributeId"))
                    .FirstOrDefault();
            }

            public void DataUploadValidationErrorRow_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorSheetId, long dataUploadValidationErrorRow)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationErrorRow_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorSheetId, dataUploadValidationErrorRow);
            }

            public long DataUploadValidationErrorRow_GetDataUploadValidationErrorRowIdByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow(long dataUploadValidationErrorSheetId, long dataUploadValidationErrorRow)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorRow_GetByDataUploadValidationErrorSheetIdAndDataUploadValidationErrorRow, 
                    dataUploadValidationErrorSheetId, dataUploadValidationErrorRow);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorRowId"))
                    .FirstOrDefault();
            }

            public List<long> DataUploadValidationErrorRow_GetDataUploadValidationErrorRowIdListByDataUploadValidationErrorSheetId(long dataUploadValidationErrorSheetId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorRow_GetByDataUploadValidationErrorSheetId, 
                    dataUploadValidationErrorSheetId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorRowId"))
                    .ToList();
            }

            public long DataUploadValidationErrorEntityAttribute_GetDataUploadValidationErrorEntityAttributeIdByDataUploadValidationErrorEntityAttributeDescription(string dataUploadValidationErrorEntityAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntityAttribute_GetByDataUploadValidationErrorEntityAttributeDescription, 
                    dataUploadValidationErrorEntityAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorEntityAttributeId"))
                    .FirstOrDefault();
            }

            public string DataUploadValidationErrorEntityAttribute_GetDataUploadValidationErrorEntityAttributeDescriptionByDataUploadValidationErrorEntityAttributeId(long dataUploadValidationErrorEntityAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntityAttribute_GetByDataUploadValidationErrorEntityAttributeId, 
                    dataUploadValidationErrorEntityAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("DataUploadValidationErrorEntityAttributeDescription"))
                    .FirstOrDefault();
            }

            public void DataUploadValidationErrorEntity_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorRowId, long dataUploadValidationErrorEntityAttributeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntity_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorRowId, dataUploadValidationErrorEntityAttributeId);
            }

            public long DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityIdByDataUploadValidationErrorRowIdAndDataUploadValidationErrorEntityAttributeId(long dataUploadValidationErrorRowId, long dataUploadValidationErrorEntityAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntity_GetByDataUploadValidationErrorRowIdAndDataUploadValidationErrorEntityAttributeId, 
                    dataUploadValidationErrorRowId, dataUploadValidationErrorEntityAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorEntityId"))
                    .FirstOrDefault();
            }

            public long DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityAttributeIdByDataUploadValidationErrorEntityId(long dataUploadValidationErrorEntityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntity_GetByDataUploadValidationErrorEntityId, 
                    dataUploadValidationErrorEntityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorEntityAttributeId"))
                    .FirstOrDefault();
            }

            public List<long> DataUploadValidationErrorEntity_GetDataUploadValidationErrorEntityIdListByDataUploadValidationErrorRowId(long dataUploadValidationErrorRowId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorEntity_GetByDataUploadValidationErrorRowId, 
                    dataUploadValidationErrorRowId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DataUploadValidationErrorEntityId"))
                    .ToList();
            }

            public void DataUploadValidationErrorMessage_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorEntityId, string dataUploadValidationErrorMessageDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.DataUploadValidationErrorMessage_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorEntityId, dataUploadValidationErrorMessageDescription);
            }

            public List<string> DataUploadValidationErrorMessage_GetDataUploadValidationErrorMessageDescriptionListByDataUploadValidationErrorEntityId(long dataUploadValidationErrorEntityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.DataUploadValidationErrorMessage_GetByDataUploadValidationErrorEntityId, 
                    dataUploadValidationErrorEntityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("DataUploadValidationErrorMessageDescription"))
                    .ToList();
            }
        }
    }
}