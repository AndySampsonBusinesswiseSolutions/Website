using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ValidateMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterExemptionDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 ValidateMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterExemptionDataController(ILogger<ValidateMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidateMeterExemptionDataAPI, password);
            ValidateMeterExemptionDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(ValidateMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    ValidateMeterExemptionDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterExemptionDataAPI, ValidateMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] table
                var meterExemptionEntities = new Methods.Temp.CustomerDataUpload.MeterExemption().MeterExemption_GetByProcessQueueGUID(processQueueGUID);

                if(!meterExemptionEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.MPXN,
                        _customerDataUploadValidationEntityEnums.DateFrom,
                        _customerDataUploadValidationEntityEnums.DateTo,
                        _customerDataUploadValidationEntityEnums.ExemptionProduct,
                        _customerDataUploadValidationEntityEnums.ExemptionProportion,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(meterExemptionEntities.Select(mee => mee.RowId).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.DateFrom, "Date From"},
                        {_customerDataUploadValidationEntityEnums.DateTo, "Date To"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, meterExemptionEntities, requiredColumns);

                //Validate Exemption Product
                var invalidExemptionProductDataRecords = meterExemptionEntities.Where(mee => !string.IsNullOrWhiteSpace(mee.ExemptionProduct)
                    && !methods.IsValidExemptionProduct(mee.ExemptionProduct));

                foreach(var invalidExemptionProductDataRecord in invalidExemptionProductDataRecords)
                {
                    if(!records[invalidExemptionProductDataRecord.RowId][_customerDataUploadValidationEntityEnums.ExemptionProduct].Contains($"Invalid Exemption Product {invalidExemptionProductDataRecord.ExemptionProduct}"))
                    {
                        records[invalidExemptionProductDataRecord.RowId][_customerDataUploadValidationEntityEnums.ExemptionProduct].Add($"Invalid Exemption Product {invalidExemptionProductDataRecord.ExemptionProduct}");
                    }
                }

                //Validate Exemption Proportion
                var invalidExemptionProportionDataRecords = meterExemptionEntities.Where(mee => !string.IsNullOrWhiteSpace(mee.ExemptionProportion)
                    && !methods.IsValidExemptionProportion(mee.ExemptionProduct, mee.ExemptionProportion));

                foreach(var invalidExemptionProportionDataRecord in invalidExemptionProportionDataRecords)
                {
                    if(!records[invalidExemptionProportionDataRecord.RowId][_customerDataUploadValidationEntityEnums.ExemptionProportion].Contains($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord.ExemptionProportion}"))
                    {
                        records[invalidExemptionProportionDataRecord.RowId][_customerDataUploadValidationEntityEnums.ExemptionProportion].Add($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord.ExemptionProportion}");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.MeterExemption);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, ValidateMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}