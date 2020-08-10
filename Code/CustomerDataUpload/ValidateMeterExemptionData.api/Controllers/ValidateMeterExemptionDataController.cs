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

namespace ValidateMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterExemptionDataController : ControllerBase
    {
        private readonly ILogger<ValidateMeterExemptionDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 ValidateMeterExemptionDataAPIId;

        public ValidateMeterExemptionDataController(ILogger<ValidateMeterExemptionDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateMeterExemptionDataAPI, _systemAPIPasswordEnums.ValidateMeterExemptionDataAPI);
            ValidateMeterExemptionDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(ValidateMeterExemptionDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterExemptionDataAPI, ValidateMeterExemptionDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] table
                var meterExemptionDataRows = _tempCustomerMethods.MeterExemption_GetByProcessQueueGUID(processQueueGUID);

                if(!meterExemptionDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, ValidateMeterExemptionDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.DateFrom, "Date From"},
                        {_customerDataUploadValidationEntityEnums.DateTo, "Date To"},
                        {_customerDataUploadValidationEntityEnums.ExemptionProduct, "Exemption Product"},
                        {_customerDataUploadValidationEntityEnums.ExemptionProportion, "Exemption Proportion"}
                    };

                var records = _tempCustomerMethods.InitialiseRecordsDictionary(meterExemptionDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.DateFrom, "Date From"},
                        {_customerDataUploadValidationEntityEnums.DateTo, "Date To"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, meterExemptionDataRows, requiredColumns);

                //Validate Exemption Product
                var invalidExemptionProductDataRecords = meterExemptionDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProduct))
                    && !_methods.IsValidExemptionProduct(r.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProduct)));

                foreach(var invalidExemptionProductDataRecord in invalidExemptionProductDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidExemptionProductDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ExemptionProduct].Contains($"Invalid Exemption Product {invalidExemptionProductDataRecord[_customerDataUploadValidationEntityEnums.ExemptionProduct]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ExemptionProduct].Add($"Invalid Exemption Product {invalidExemptionProductDataRecord[_customerDataUploadValidationEntityEnums.ExemptionProduct]}");
                    }
                }

                //Validate Exemption Proportion
                var invalidExemptionProportionDataRecords = meterExemptionDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProportion))
                    && !_methods.IsValidExemptionProportion(r.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProduct), 
                                                            r.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProportion))
                    );

                foreach(var invalidExemptionProportionDataRecord in invalidExemptionProportionDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidExemptionProportionDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ExemptionProportion].Contains($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord[_customerDataUploadValidationEntityEnums.ExemptionProportion]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ExemptionProportion].Add($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord[_customerDataUploadValidationEntityEnums.ExemptionProportion]}");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.MeterExemption);
                _systemMethods.ProcessQueue_Update(processQueueGUID, ValidateMeterExemptionDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, ValidateMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

