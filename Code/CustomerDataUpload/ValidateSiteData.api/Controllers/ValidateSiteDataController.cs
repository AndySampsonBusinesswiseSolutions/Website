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

namespace ValidateSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSiteDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSiteDataController(ILogger<ValidateSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSiteDataAPI, password);
            validateSiteDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSiteDataAPI);
        }

        [HttpPost]
        [Route("ValidateSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSiteDataAPI, validateSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] table
                var siteEntities = new Methods.Temp.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(processQueueGUID);

                if(!siteEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSiteDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

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

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(siteEntities.Select(se => se.RowId.Value).Distinct().ToList(), columns);
                
                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SiteName, "Site Name"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, siteEntities, requiredColumns);

                //Validate post code
                var invalidPostCodeEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.SitePostCode) 
                    && !methods.IsValidPostCode(se.SitePostCode));

                foreach(var invalidPostCodeEntity in invalidPostCodeEntities)
                {
                    if(!records[invalidPostCodeEntity.RowId.Value][_customerDataUploadValidationEntityEnums.SitePostCode].Contains($"Invalid Site Post Code '{invalidPostCodeEntity.SitePostCode}'"))
                    {
                        records[invalidPostCodeEntity.RowId.Value][_customerDataUploadValidationEntityEnums.SitePostCode].Add($"Invalid Site Post Code '{invalidPostCodeEntity.SitePostCode}'");
                    }
                }

                //Validate telephone number
                var invalidTelephoneNumberEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.ContactTelephoneNumber) 
                    && !methods.IsValidPhoneNumber(se.ContactTelephoneNumber));

                foreach(var invalidTelephoneNumberEntity in invalidTelephoneNumberEntities)
                {
                    if(!records[invalidTelephoneNumberEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberEntity.ContactTelephoneNumber}'"))
                    {
                        records[invalidTelephoneNumberEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberEntity.ContactTelephoneNumber}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressEntities = siteEntities.Where(se => !string.IsNullOrWhiteSpace(se.ContactEmailAddress) 
                    && !methods.IsValidEmailAddress(se.ContactEmailAddress));

                foreach(var invalidEmailAddressEntity in invalidEmailAddressEntities)
                {
                    if(!records[invalidEmailAddressEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressEntity.ContactEmailAddress}'"))
                    {
                        records[invalidEmailAddressEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressEntity.ContactEmailAddress}'");
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