using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CommitCustomerDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "yuPPW9N2d346ATSM";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerDataAPI, password);
                var commitCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCustomerDataAPI);

                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitCustomerDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitCustomerDataAPI, commitCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCustomerDataAPIId);

                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerUploadData].[Customer] where CanCommit = 1
                var customerEntities = new Methods.TempSchema.CustomerDataUpload.Customer().Customer_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableCustomerEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(customerEntities);

                if(!commitableCustomerEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.CustomerSchema();
                var customerAttributeEnums = new Enums.CustomerSchema.Customer.Attribute();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();

                //For each column, get CustomerAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(customerAttributeEnums.CustomerName), customerDataUploadValidationEntityEnums.CustomerName},
                    {customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(customerAttributeEnums.ContactName), customerDataUploadValidationEntityEnums.ContactName},
                    {customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(customerAttributeEnums.ContactTelephoneNumber), customerDataUploadValidationEntityEnums.ContactTelephoneNumber},
                    {customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(customerAttributeEnums.ContactEmailAddress), customerDataUploadValidationEntityEnums.ContactEmailAddress},
                };

                foreach(var customerEntity in commitableCustomerEntities)
                {
                    var detailDictionary = attributes.ToDictionary(
                        a => a.Key,
                        a => customerEntity.GetType().GetProperty(a.Value).GetValue(customerEntity).ToString()
                    );

                    //Get CustomerId by CustomerName
                    var customerId = customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value);

                    if(customerId == 0)
                    {
                        customerId = customerMethods.InsertNewCustomer(createdByUserId, sourceId);

                        //Insert into [Customer].[CustomerDetail]
                        foreach(var detail in detailDictionary)
                        {
                            customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[CustomerDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailEntity = customerMethods.CustomerDetail_GetByCustomerIdAndCustomerAttributeId(customerId, detail.Key);

                            if(detail.Value != currentDetailEntity.CustomerDetailDescription)
                            {
                                customerMethods.CustomerDetail_DeleteByCustomerDetailId(currentDetailEntity.CustomerDetailId);
                                customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, detail.Key, detail.Value);
                            }
                        }
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
