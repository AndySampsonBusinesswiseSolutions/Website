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

namespace ValidateSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSiteDataController : ControllerBase
    {
        private readonly ILogger<ValidateSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateSiteDataAPIId;

        public ValidateSiteDataController(ILogger<ValidateSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateSiteDataAPI, _systemAPIPasswordEnums.ValidateSiteDataAPI);
            validateSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSiteDataAPI);
        }

        [HttpPost]
        [Route("ValidateSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSiteData/Validate")]
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
                    validateSiteDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSiteDataAPI, validateSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] table
                var siteDataRows = _tempCustomerDataUploadMethods.Site_GetByProcessQueueGUID(processQueueGUID);

                if(!siteDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, false, null);
                    return;
                }

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.CustomerName,
                        _customerDataUploadValidationEntityEnums.SiteName,
                        _customerDataUploadValidationEntityEnums.SiteAddress,
                        _customerDataUploadValidationEntityEnums.SiteTown,
                        _customerDataUploadValidationEntityEnums.SiteCounty,
                        _customerDataUploadValidationEntityEnums.SitePostCode,
                        _customerDataUploadValidationEntityEnums.SiteDescription,
                        _customerDataUploadValidationEntityEnums.ContactName,
                        _customerDataUploadValidationEntityEnums.ContactRole,
                        _customerDataUploadValidationEntityEnums.ContactTelephoneNumber,
                        _customerDataUploadValidationEntityEnums.ContactEmailAddress,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(siteDataRows.Select(d => Convert.ToInt32(d["RowId"].ToString())).Distinct().ToList(), columns);
                
                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, siteDataRows, requiredColumns);

                //Validate post code
                var invalidPostCodeDataRows = siteDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.SitePostCode)) 
                    && !_methods.IsValidPostCode(r.Field<string>(_customerDataUploadValidationEntityEnums.SitePostCode)));

                foreach(var invalidPostCodeDataRow in invalidPostCodeDataRows)
                {
                    var rowId = Convert.ToInt32(invalidPostCodeDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.SitePostCode].Contains($"Invalid Site Post Code '{invalidPostCodeDataRow[_customerDataUploadValidationEntityEnums.SitePostCode]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.SitePostCode].Add($"Invalid Site Post Code '{invalidPostCodeDataRow[_customerDataUploadValidationEntityEnums.SitePostCode]}'");
                    }
                }

                //Validate telephone number
                var invalidTelephoneNumberDataRows = siteDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactTelephoneNumber)) 
                    && !_methods.IsValidPhoneNumber(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactTelephoneNumber)));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberDataRows)
                {
                    var rowId = Convert.ToInt32(invalidTelephoneNumberDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow[_customerDataUploadValidationEntityEnums.ContactTelephoneNumber]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow[_customerDataUploadValidationEntityEnums.ContactTelephoneNumber]}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressDataRows = siteDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactEmailAddress)) 
                    && !_methods.IsValidEmailAddress(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactEmailAddress)));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    var rowId = Convert.ToInt32(invalidEmailAddressDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressDataRow[_customerDataUploadValidationEntityEnums.ContactEmailAddress]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow[_customerDataUploadValidationEntityEnums.ContactEmailAddress]}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Site);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}