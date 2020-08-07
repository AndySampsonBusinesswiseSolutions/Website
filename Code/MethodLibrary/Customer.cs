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

            public long SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(string siteAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteAttribute_GetBySiteAttributeDescription, 
                    siteAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteAttributeId"))
                    .FirstOrDefault();
            }

            public long SiteDetail_GetSiteDetailIdBySiteAttributeIdAndSiteDetailDescription(long siteAttributeId, string siteDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription, 
                    siteAttributeId, siteDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SiteDetailId"))
                    .FirstOrDefault();
            }

            public long MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(long MeterAttributeId, string MeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription, 
                    MeterAttributeId, MeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterDetailId"))
                    .FirstOrDefault();
            }

            public long MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(string MeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.MeterAttribute_GetByMeterAttributeDescription, 
                    MeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterAttributeId"))
                    .FirstOrDefault();
            }

            public long SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(long subMeterAttributeId, string subMeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription, 
                    subMeterAttributeId, subMeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterDetailId"))
                    .FirstOrDefault();
            }

            public long SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(string subMeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.SubMeterAttribute_GetBySubMeterAttributeDescription, 
                    subMeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterAttributeId"))
                    .FirstOrDefault();
            }

            public void InsertDataUploadValidationErrors(string processQueueGUID, long createdByUserId, long sourceId, string sheetName, Dictionary<int, Dictionary<string, List<string>>> validationErrors)
            {
                //Insert into DataUploadValidationError
                var dataUploadValidationErrorGUID = Guid.NewGuid().ToString();
                DataUploadValidationError_Insert(createdByUserId, sourceId, dataUploadValidationErrorGUID);

                //Get DataUploadValidationErrorId
                var dataUploadValidationErrorId = DataUploadValidationError_GetDataUploadValidationErrorIdByDataUploadValidationErrorGUID(dataUploadValidationErrorGUID);

                //Get DataUploadValidationErrorAttributes
                var sheetNameDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_customerDataUploadValidationAttributeEnums.SheetName);
                var rowNumberDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_customerDataUploadValidationAttributeEnums.RowNumber);
                var entityDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_customerDataUploadValidationAttributeEnums.Entity);
                var validationErrorMessageDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_customerDataUploadValidationAttributeEnums.ValidationErrorMessage);
                var processQueueGUIDDataUploadValidationErrorAttributeId = DataUploadValidationErrorAttribute_GetDataUploadValidationErrorAttributeIdByDataUploadValidationErrorAttributeDescription(_customerDataUploadValidationAttributeEnums.ProcessQueueGUID);

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
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, validationErrorMessageDataUploadValidationErrorAttributeId, validationErrorMessage);
                            DataUploadValidationErrorDetail_Insert(createdByUserId, sourceId, dataUploadValidationErrorId, processQueueGUIDDataUploadValidationErrorAttributeId, processQueueGUID);
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
