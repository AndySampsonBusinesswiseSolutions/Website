using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CommitFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private readonly Int64 commitFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexReferenceVolumeDataAPI, password);
            commitFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/Commit")]
        public void Commit([FromBody] object data)
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
                    commitFlexReferenceVolumeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitFlexReferenceVolumeDataAPI, commitFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId);

                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] where CanCommit = 1
                var flexReferenceVolumeEntities = new Methods.TempSchema.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexReferenceVolumeEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(flexReferenceVolumeEntities);

                if(!commitableFlexReferenceVolumeEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var customerReferenceVolumeAttributeEnums = new Enums.CustomerSchema.ReferenceVolume.Attribute();
                var customerMethods = new Methods.CustomerSchema();
                var mappingMethods = new Methods.MappingSchema();

                var contractReferenceContractAttributeId = customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(new Enums.CustomerSchema.Contract.Attribute().ContractReference);
                var dateFromReferenceVolumeAttributeId = customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(customerReferenceVolumeAttributeEnums.DateFrom);
                var dateToReferenceVolumeAttributeId = customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(customerReferenceVolumeAttributeEnums.DateTo);
                var referenceVolumeReferenceVolumeAttributeId = customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(customerReferenceVolumeAttributeEnums.ReferenceVolume);

                var contracts = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.ContractReference).Distinct()
                    .ToDictionary(c => c, c => customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var fromDates = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.DateFrom).Distinct()
                    .ToDictionary(d => d, d => customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateFromReferenceVolumeAttributeId, d));

                var toDates = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.DateTo).Distinct()
                    .ToDictionary(d => d, d => customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateToReferenceVolumeAttributeId, d));

                var referenceVolumes = commitableFlexReferenceVolumeEntities.Select(cfrve => cfrve.Volume).Distinct()
                    .ToDictionary(rv => rv, rv => customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(referenceVolumeReferenceVolumeAttributeId, rv));

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
                        referenceVolumeId = customerMethods.InsertNewReferenceVolume(createdByUserId, sourceId);

                        //Insert into [Customer].[ReferenceVolumeDetail]
                        customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateFromReferenceVolumeAttributeId, flexReferenceVolumeEntity.DateFrom);
                        customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, dateToReferenceVolumeAttributeId, flexReferenceVolumeEntity.DateTo);
                        customerMethods.ReferenceVolumeDetail_Insert(createdByUserId, sourceId, referenceVolumeId, referenceVolumeReferenceVolumeAttributeId, flexReferenceVolumeEntity.Volume);
                    }

                    //Get existing ContractToReferenceVolume mapping
                    var existingContractToReferenceId = mappingMethods.ContractToReferenceVolume_GetContractToReferenceVolumeIdByContractIdAndReferenceVolumeId(contracts[flexReferenceVolumeEntity.ContractReference], referenceVolumeId);

                    if(existingContractToReferenceId == 0)
                    {
                        //Insert into [Mapping].[ContractToReferenceVolume]
                        mappingMethods.ContractToReferenceVolume_Insert(createdByUserId, sourceId, contracts[flexReferenceVolumeEntity.ContractReference], referenceVolumeId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}