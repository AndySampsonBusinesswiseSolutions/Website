﻿using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ValidateFlexReferenceVolumeDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "h9CMbkML68NCyMNj";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexReferenceVolumeDataAPI, password);
                var validateFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexReferenceVolumeDataAPI);

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
                    validateFlexReferenceVolumeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateFlexReferenceVolumeDataAPI, validateFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] table
                var flexReferenceVolumeEntities = new Methods.TempSchema.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);

                if(!flexReferenceVolumeEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.ContractReference,
                        customerDataUploadValidationEntityEnums.DateFrom,
                        customerDataUploadValidationEntityEnums.DateTo,
                        customerDataUploadValidationEntityEnums.Volume,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexReferenceVolumeEntities.Select(frve => frve.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, flexReferenceVolumeEntities, requiredColumns);

                //Validate Contract Dates
                var invalidDateFromDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateFrom)
                    && !methods.IsValidDate(frve.DateFrom));

                foreach(var invalidDateFromDataRecord in invalidDateFromDataRecords)
                {
                    if(!records[invalidDateFromDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Date From '{invalidDateFromDataRecord.DateFrom}'"))
                    {
                        records[invalidDateFromDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Date From '{invalidDateFromDataRecord.DateFrom}'");
                    }
                }

                var invalidDateToDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateTo)
                    && !methods.IsValidDate(frve.DateTo));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    if(!records[invalidDateToDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateTo].Contains($"Invalid Date to '{invalidDateToDataRecord.DateTo}'"))
                    {
                        records[invalidDateToDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateTo].Add($"Invalid Date to '{invalidDateToDataRecord.DateTo}'");
                    }
                }

                var invalidContractDateDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateFrom)
                    && !string.IsNullOrWhiteSpace(frve.DateTo)
                    && methods.IsValidDate(frve.DateFrom)
                    && methods.IsValidDate(frve.DateTo)
                    && Convert.ToDateTime(frve.DateFrom) >= Convert.ToDateTime(frve.DateTo));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    if(!records[invalidDateToDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Contract Dates '{invalidDateToDataRecord.DateFrom}' is equal to or later than '{invalidDateToDataRecord.DateTo}'"))
                    {
                        records[invalidDateToDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Contract Dates '{invalidDateToDataRecord.DateFrom}' is equal to or later than '{invalidDateToDataRecord.DateTo}'");
                    }
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.Volume)
                    && !methods.IsValidFlexReferenceVolume(frve.Volume));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    if(!records[invalidVolumeDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Volume].Contains($"Invalid Reference Volume '{invalidVolumeDataRecord.Volume}'"))
                    {
                        records[invalidVolumeDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Volume].Add($"Invalid Reference Volume '{invalidVolumeDataRecord.Volume}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().FlexReferenceVolume);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
