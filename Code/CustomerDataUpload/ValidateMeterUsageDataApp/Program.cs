using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Entity;

namespace ValidateMeterUsageDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "qashfvSa2xB58PXR";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterUsageDataAPI, password);
                var validateMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterUsageDataAPI);

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
                    validateMeterUsageDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateMeterUsageDataAPI, validateMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] table
                var meterUsageEntities = new Methods.TempSchema.CustomerDataUpload.MeterUsage().MeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!meterUsageEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                string errorMessage = null;
                var sourceSheetList = new List<string>
                {
                    customerDataUploadValidationSheetNameEnums.MeterUsage,
                    customerDataUploadValidationSheetNameEnums.DailyMeterUsage
                };

                var columns = new List<string>
                {
                    customerDataUploadValidationEntityEnums.MPXN,
                    customerDataUploadValidationEntityEnums.Date,
                    customerDataUploadValidationEntityEnums.TimePeriod,
                    customerDataUploadValidationEntityEnums.Value,
                };
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.Date, "Read Date"}
                    };

                foreach(var sourceSheet in sourceSheetList)
                {
                    var usageEntities = meterUsageEntities.Where(mu => mu.SheetName == sourceSheet).ToList();

                    if(!usageEntities.Any())
                    {
                        continue;
                    }

                    var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(usageEntities.Select(u => u.RowId.Value).Distinct().ToList(), columns);

                    //If any are empty records, store error
                    tempCustomerDataUploadMethods.GetMissingRecords<Temp.CustomerDataUpload.MeterUsage>(records, usageEntities, requiredColumns);

                    //Check dates are valid
                    var invalidDates = usageEntities.Where(u => !methods.IsValidDate(u.Date)).Select(u => new {u.RowId, u.Date}).Distinct().ToList();

                    foreach(var invalidDate in invalidDates)
                    {
                        if(!records[invalidDate.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date '{invalidDate.Date}' found"))
                        {
                            records[invalidDate.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Invalid date '{invalidDate.Date}' found");
                        }
                    }

                    //Check all dates are in the past
                    var validDates = usageEntities.Where(u => methods.IsValidDate(u.Date)).ToList();
                    var futureDates = validDates.Where(u => Convert.ToDateTime(u.Date) >= DateTime.Today).ToList();

                    foreach(var futureDate in futureDates)
                    {
                        if(!records[futureDate.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Future date '{futureDate.Date}' found"))
                        {
                            records[futureDate.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Future date '{futureDate.Date}' found");
                        }
                    }

                    //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                    var invalidUsages = usageEntities.Where(u => !methods.IsValidUsage(u.Value)).ToList();

                    foreach(var invalidUsage in invalidUsages)
                    {
                        if(!records[invalidUsage.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage '{invalidUsage.Value}' for {invalidUsage.Date} {invalidUsage.TimePeriod}"))
                        {
                            records[invalidUsage.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage '{invalidUsage.Value}' for {invalidUsage.Date} {invalidUsage.TimePeriod}");
                        }
                    }

                    var additionalHalfHours = usageEntities.Where(u => methods.IsAdditionalTimePeriod(u.TimePeriod) && !string.IsNullOrWhiteSpace(u.Value)).ToList();
                    var invalidAdditionalHalfHours = additionalHalfHours.Where(u => !methods.IsOctoberClockChange(u.Date)).ToList();

                    foreach(var invalidUsage in invalidAdditionalHalfHours)
                    {
                        if(!records[invalidUsage.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour '{invalidUsage.TimePeriod}' but {invalidUsage.Date} is not an October clock change date"))
                        {
                            records[invalidUsage.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour '{invalidUsage.TimePeriod}' but {invalidUsage.Date} is not an October clock change date");
                        }
                    }

                    //Update Process Queue
                    var sourceErrorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, sourceSheet);
                    if(!string.IsNullOrWhiteSpace(sourceErrorMessage))
                    {
                        errorMessage = sourceErrorMessage;
                    }
                }
                
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
