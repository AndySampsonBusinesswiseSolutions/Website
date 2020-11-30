using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ValidateSubMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "nqCLyMb92urhKraj";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterDataAPI, password);
                var validateSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI);

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
                    validateSubMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI, validateSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] table
                var subMeterEntities = new Methods.TempSchema.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, false, null);
                    return;
                }

                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();
                var customerMethods = new Methods.CustomerSchema();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        customerDataUploadValidationEntityEnums.SerialNumber,
                        customerDataUploadValidationEntityEnums.SubArea,
                        customerDataUploadValidationEntityEnums.Asset
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterEntities.Select(sme => sme.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Name"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterEntities, requiredColumns);

                //Get submeters not stored in database
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var newSubMeterDataRecords = subMeterEntities.Where(sme => 
                    customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, sme.SubMeterIdentifier) == 0)
                    .ToList();

                //MPXN, SerialNumber, SubArea and Asset must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.SerialNumber, "SubMeter Serial Number"},
                        {customerDataUploadValidationEntityEnums.SubArea, customerDataUploadValidationEntityEnums.SubArea},
                        {customerDataUploadValidationEntityEnums.Asset, customerDataUploadValidationEntityEnums.Asset}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newSubMeterDataRecords, requiredColumns);

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().SubMeter);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
