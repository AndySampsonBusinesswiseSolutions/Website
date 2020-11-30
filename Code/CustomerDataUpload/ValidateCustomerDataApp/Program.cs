using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ValidateCustomerDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "Mkf2GTm2crKuk6jh";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateCustomerDataAPI, password);
                var validateCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateCustomerDataAPI);

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
                    validateCustomerDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateCustomerDataAPI, validateCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateCustomerDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Customer] table
                var tempCustomerDataUploadCustomerMethods = new Methods.TempSchema.CustomerDataUpload.Customer();
                var customerEntities = tempCustomerDataUploadCustomerMethods.Customer_GetByProcessQueueGUID(processQueueGUID);

                if(!customerEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.CustomerName,
                        customerDataUploadValidationEntityEnums.ContactName,
                        customerDataUploadValidationEntityEnums.ContactTelephoneNumber,
                        customerDataUploadValidationEntityEnums.ContactEmailAddress,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(customerEntities.Select(ce => ce.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.CustomerName, "Customer Name"},
                        {customerDataUploadValidationEntityEnums.ContactName, "Contact Name"},
                        {customerDataUploadValidationEntityEnums.ContactTelephoneNumber, "Contact Telephone Number"},
                        {customerDataUploadValidationEntityEnums.ContactEmailAddress, "Contact Email Address"}
                    };
                
                tempCustomerDataUploadMethods.GetMissingRecords(records, customerEntities, requiredColumns);

                //Validate telephone number
                var invalidTelephoneNumberEntities = customerEntities.Where(ce => !string.IsNullOrWhiteSpace(ce.ContactTelephoneNumber) 
                    && !methods.IsValidPhoneNumber(ce.ContactTelephoneNumber));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberEntities)
                {
                    if(!records[invalidTelephoneNumberDataRow.RowId.Value][customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow.ContactTelephoneNumber}'"))
                    {
                        records[invalidTelephoneNumberDataRow.RowId.Value][customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow.ContactTelephoneNumber}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressDataRows = customerEntities.Where(ce => !string.IsNullOrWhiteSpace(ce.ContactEmailAddress) 
                    && !methods.IsValidEmailAddress(ce.ContactEmailAddress));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    if(!records[invalidEmailAddressDataRow.RowId.Value][customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressDataRow.ContactEmailAddress}'"))
                    {
                        records[invalidEmailAddressDataRow.RowId.Value][customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow.ContactEmailAddress}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().Customer);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
