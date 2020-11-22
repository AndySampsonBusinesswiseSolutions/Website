using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace ValidateMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterExemptionDataController> _logger;
        private readonly Int64 ValidateMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterExemptionDataController(ILogger<ValidateMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterExemptionDataAPI, password);
            ValidateMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(ValidateMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    ValidateMeterExemptionDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateMeterExemptionDataAPI, ValidateMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] table
                var meterExemptionEntities = new Methods.TempSchema.CustomerDataUpload.MeterExemption().MeterExemption_GetByProcessQueueGUID(processQueueGUID);

                if(!meterExemptionEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.DateFrom,
                        customerDataUploadValidationEntityEnums.DateTo,
                        customerDataUploadValidationEntityEnums.ExemptionProduct,
                        customerDataUploadValidationEntityEnums.ExemptionProportion,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(meterExemptionEntities.Select(mee => mee.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.DateFrom, "Date From"},
                        {customerDataUploadValidationEntityEnums.DateTo, "Date To"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, meterExemptionEntities, requiredColumns);

                //Validate Exemption Product
                var invalidExemptionProductDataRecords = meterExemptionEntities.Where(mee => !string.IsNullOrWhiteSpace(mee.ExemptionProduct)
                    && !methods.IsValidExemptionProduct(mee.ExemptionProduct));

                foreach(var invalidExemptionProductDataRecord in invalidExemptionProductDataRecords)
                {
                    if(!records[invalidExemptionProductDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ExemptionProduct].Contains($"Invalid Exemption Product {invalidExemptionProductDataRecord.ExemptionProduct}"))
                    {
                        records[invalidExemptionProductDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ExemptionProduct].Add($"Invalid Exemption Product {invalidExemptionProductDataRecord.ExemptionProduct}");
                    }
                }

                //Validate Exemption Proportion
                var invalidExemptionProportionDataRecords = meterExemptionEntities.Where(mee => !string.IsNullOrWhiteSpace(mee.ExemptionProportion)
                    && !methods.IsValidExemptionProportion(mee.ExemptionProduct, mee.ExemptionProportion));

                foreach(var invalidExemptionProportionDataRecord in invalidExemptionProportionDataRecords)
                {
                    if(!records[invalidExemptionProportionDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ExemptionProportion].Contains($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord.ExemptionProportion}"))
                    {
                        records[invalidExemptionProportionDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ExemptionProportion].Add($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord.ExemptionProportion}");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().MeterExemption);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}