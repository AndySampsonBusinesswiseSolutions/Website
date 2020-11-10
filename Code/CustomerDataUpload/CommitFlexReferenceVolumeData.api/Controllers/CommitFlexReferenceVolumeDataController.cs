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
        #region Variables
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
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
        #endregion

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().CommitFlexReferenceVolumeDataAPI, password);
            commitFlexReferenceVolumeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI, commitFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] where CanCommit = 1
                var flexReferenceVolumeEntities = new Methods.Temp.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexReferenceVolumeEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(flexReferenceVolumeEntities);

                if(!commitableFlexReferenceVolumeEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var dateFromReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateFrom);
                var dateToReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateTo);
                var referenceVolumeReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.ReferenceVolume);

                var contracts = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.ContractReference).Distinct()
                    .ToDictionary(c => c, c => _customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var fromDates = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.DateFrom).Distinct()
                    .ToDictionary(d => d, d => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateFromReferenceVolumeAttributeId, d));

                var toDates = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.DateTo).Distinct()
                    .ToDictionary(d => d, d => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateToReferenceVolumeAttributeId, d));

                var referenceVolumes = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.Volume).Distinct()
                    .ToDictionary(rv => rv, rv => _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(referenceVolumeReferenceVolumeAttributeId, rv));

                foreach(var flexReferenceVolumeEntity in commitableFlexReferenceVolumeEntities.Where(cfrve => contracts[cfrve.ContractReference] > 0))
                {
                    //Get ReferenceVolumeId from [Customer].[ReferenceVolumeDetail] by DateFrom, DateTo and Reference Volume
                    var dateFromReferenceVolumeIdList = fromDates[flexReferenceVolumeEntity.DateFrom];
                    var dateToCodeReferenceVolumeIdList = toDates[flexReferenceVolumeEntity.DateTo];
                    var referenceVolumeReferenceVolumeIdList = referenceVolumes[flexReferenceVolumeEntity.Volume];

                    var matchingReferenceVolumeIdList = dateFromReferenceVolumeIdList.Intersect(dateToCodeReferenceVolumeIdList).Intersect(referenceVolumeReferenceVolumeIdList);
                    var referenceVolumeId = matchingReferenceVolumeIdList.FirstOrDefault();

                    if(referenceVolumeId == 0)
                    {
                        referenceVolumeId = _customerMethods.InsertNewReferenceVolume(createdByUserId, sourceId);

                        //Insert into [Customer].[ReferenceVolumeDetail]
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateFromReferenceVolumeAttributeId, flexReferenceVolumeEntity.DateFrom);
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateToReferenceVolumeAttributeId, flexReferenceVolumeEntity.DateTo);
                        _customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, referenceVolumeReferenceVolumeAttributeId, flexReferenceVolumeEntity.Volume);
                    }

                    //Insert into [Mapping].[ContractToReferenceVolume]
                    _mappingMethods.ContractToReferenceVolume_Insert(createdByUserId, sourceId, contracts[flexReferenceVolumeEntity.ContractReference], referenceVolumeId);
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