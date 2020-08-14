using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
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

            public long ContractAttribute_GetContractAttributeIdByContractAttributeDescription(string contractAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractAttribute_GetByContractAttributeDescription, 
                    contractAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractAttributeId"))
                    .FirstOrDefault();
            }

            public long ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(long contractAttributeId, string contractDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractDetail_GetByContractAttributeIdAndContractDetailDescription, 
                    contractAttributeId, contractDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractDetailId"))
                    .FirstOrDefault();
            }

            public long BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(string basketAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketAttribute_GetByBasketAttributeDescription, 
                    basketAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketAttributeId"))
                    .FirstOrDefault();
            }

            public long BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(long basketAttributeId, string basketDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription, 
                    basketAttributeId, basketDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketDetailId"))
                    .FirstOrDefault();
            }
            
            private IEnumerable<long> GetContractMeterListByContractReferenceAndMPXN(string contractReference, string mpxn)
            {
                //Get ContractId from ContractReference
                var contractReferenceContractAttributeId = ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var contractId = ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference);

                //If ContractId == 0 then not valid
                if(contractId == 0)
                {
                    return new List<long>();
                }

                //Get MeterId from MPXN
                var meterIdentifierMeterAttributeId = MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterId = MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                //If MeterId == 0 then not valid
                if(meterId == 0)
                {
                    return new List<long>();
                }

                //Get ContractMeterIds from ContractId
                var contractMeterIdFromContractId = new Mapping().ContractToContractMeter_GetContractMeterIdListByContractId(contractId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromContractId.Any())
                {
                    return new List<long>();
                }

                //Get ContractMeterIds from MeterId
                var contractMeterIdFromMeterId = new Mapping().ContractMeterToMeter_GetContractMeterIdListByMeterId(meterId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromMeterId.Any())
                {
                    return new List<long>();
                }

                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = contractMeterIdFromContractId.Intersect(contractMeterIdFromMeterId);

                return matchingContractMeterIds;
            }

            public bool ContractMeterExists(string contractReference, string mpxn)
            {
                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = GetContractMeterListByContractReferenceAndMPXN(contractReference, mpxn);

                return matchingContractMeterIds.Any();
            }

            public bool ContractBasketMeterExists(string contractReference, string basketReference, string mpxn)
            {
                //Get BasketId from BasketReference
                var basketReferenceBasketAttributeId = BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                var basketId = BasketDetail_GetBasketDetailIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, basketReference);

                //If BasketId == 0 then not valid
                if(basketId == 0)
                {
                    return false;
                }

                //Get ContractMeters from BasketId
                var contractMeterIdFromBasketId = new Mapping().BasketToContractMeter_GetContractMeterIdListByBasketId(basketId);

                //If no ContractMeterIds then not valid
                if(!contractMeterIdFromBasketId.Any())
                {
                    return false;
                }

                //Get ContractMeterIds that exist in both lists
                var matchingContractMeterIds = GetContractMeterListByContractReferenceAndMPXN(contractReference, mpxn);

                if(!matchingContractMeterIds.Any())
                {
                    return false;
                }
                
                //Get ContractMeterIds that exist in both lists
                var matchingContractBasketMeterIds = matchingContractMeterIds.Intersect(contractMeterIdFromBasketId);

                return matchingContractBasketMeterIds.Any();
            }
        }
    }
}
