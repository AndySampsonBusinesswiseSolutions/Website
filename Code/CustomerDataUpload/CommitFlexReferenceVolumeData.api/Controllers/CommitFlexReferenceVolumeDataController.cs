using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.ReferenceVolume.Attribute _customerReferenceVolumeAttributeEnums = new Enums.Customer.ReferenceVolume.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CommitFlexReferenceVolumeDataAPI, password);
            commitFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI, commitFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] where CanCommit = 1
                var flexReferenceVolumeDataRows = _tempCustomerDataUploadMethods.FlexReferenceVolume_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(flexReferenceVolumeDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var dateFromReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateFrom);
                var dateToReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateTo);
                var referenceVolumeReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.ReferenceVolume);

                var contracts = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference))
                    .Distinct()
                    .ToDictionary(c => c, c => _customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var fromDates = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom))
                    .Distinct()
                    .ToDictionary(d => d, d => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateFromReferenceVolumeAttributeId, d));

                var toDates = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.DateTo))
                    .Distinct()
                    .ToDictionary(d => d, d => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateToReferenceVolumeAttributeId, d));

                var referenceVolumes = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.Volume))
                    .Distinct()
                    .ToDictionary(rv => rv, rv => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(referenceVolumeReferenceVolumeAttributeId, rv));

                foreach(var dataRow in commitableDataRows)
                {
                    //Get ContractId by ContractReference
                    var contractReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference);
                    var contractId = contracts[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)];

                    if(contractId == 0)
                    {
                        continue;
                    }

                    //Get ReferenceVolumeId from [Customer].[ReferenceVolumeDetail] by DateFrom, DateTo and Reference Volume
                    var dateFrom = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom);
                    var dateTo = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateTo);
                    var referenceVolume = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Volume);

                    var dateFromReferenceVolumeIdList = fromDates[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom)];
                    var dateToCodeReferenceVolumeIdList = toDates[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateTo)];
                    var referenceVolumeReferenceVolumeIdList = referenceVolumes[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Volume)];

                    var matchingReferenceVolumeIdList = dateFromReferenceVolumeIdList.Intersect(dateToCodeReferenceVolumeIdList).Intersect(referenceVolumeReferenceVolumeIdList);
                    var referenceVolumeId = matchingReferenceVolumeIdList.FirstOrDefault();

                    if(referenceVolumeId == 0)
                    {
                        referenceVolumeId = _customerMethods.InsertNewReferenceVolume(createdByUserId, sourceId);

                        //Insert into [Customer].[ReferenceVolumeDetail]
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateFromReferenceVolumeAttributeId, dateFrom);
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateToReferenceVolumeAttributeId, dateTo);
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, referenceVolumeReferenceVolumeAttributeId, referenceVolume);
                    }

                    //Insert into [Mapping].[ContractToReferenceVolume]
                    _mappingMethods.ContractToReferenceVolume_Insert(createdByUserId, sourceId, contractId, referenceVolumeId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}