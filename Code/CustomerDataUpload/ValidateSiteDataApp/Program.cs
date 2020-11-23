using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ValidateSiteDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "w2fs7druC2jUCNfQ";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSiteDataAPI, password);
                var validateSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSiteDataAPI);

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
                    validateSiteDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateSiteDataAPI, validateSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] table
                var siteEntities = new Methods.TempSchema.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(processQueueGUID);

                if(!siteEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.CustomerName,
                        customerDataUploadValidationEntityEnums.SiteName,
                        customerDataUploadValidationEntityEnums.SiteAddress,
                        customerDataUploadValidationEntityEnums.SiteTown,
                        customerDataUploadValidationEntityEnums.SiteCounty,
                        customerDataUploadValidationEntityEnums.SitePostCode,
                        customerDataUploadValidationEntityEnums.SiteDescription,
                        customerDataUploadValidationEntityEnums.ContactName,
                        customerDataUploadValidationEntityEnums.ContactRole,
                        customerDataUploadValidationEntityEnums.ContactTelephoneNumber,
                        customerDataUploadValidationEntityEnums.ContactEmailAddress,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(siteEntities.Select(se => se.RowId.Value).Distinct().ToList(), columns);
                
                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SiteName, "Site Name"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, siteEntities, requiredColumns);

                //Validate post code
                var invalidPostCodeEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.SitePostCode) 
                    && !methods.IsValidPostCode(se.SitePostCode));

                foreach(var invalidPostCodeEntity in invalidPostCodeEntities)
                {
                    if(!records[invalidPostCodeEntity.RowId.Value][customerDataUploadValidationEntityEnums.SitePostCode].Contains($"Invalid Site Post Code '{invalidPostCodeEntity.SitePostCode}'"))
                    {
                        records[invalidPostCodeEntity.RowId.Value][customerDataUploadValidationEntityEnums.SitePostCode].Add($"Invalid Site Post Code '{invalidPostCodeEntity.SitePostCode}'");
                    }
                }

                //Validate telephone number
                var invalidTelephoneNumberEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.ContactTelephoneNumber) 
                    && !methods.IsValidPhoneNumber(se.ContactTelephoneNumber));

                foreach(var invalidTelephoneNumberEntity in invalidTelephoneNumberEntities)
                {
                    if(!records[invalidTelephoneNumberEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberEntity.ContactTelephoneNumber}'"))
                    {
                        records[invalidTelephoneNumberEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberEntity.ContactTelephoneNumber}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.ContactEmailAddress) 
                    && !methods.IsValidEmailAddress(se.ContactEmailAddress));

                foreach(var invalidEmailAddressEntity in invalidEmailAddressEntities)
                {
                    if(!records[invalidEmailAddressEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressEntity.ContactEmailAddress}'"))
                    {
                        records[invalidEmailAddressEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressEntity.ContactEmailAddress}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().Site);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
