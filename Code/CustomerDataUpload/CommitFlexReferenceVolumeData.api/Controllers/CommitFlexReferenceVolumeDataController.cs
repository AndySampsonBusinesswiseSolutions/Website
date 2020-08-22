﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.ReferenceVolume.Attribute _customerReferenceVolumeAttributeEnums = new Enums.Customer.ReferenceVolume.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitFlexReferenceVolumeDataAPIId;

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexReferenceVolumeDataAPI, _systemAPIPasswordEnums.CommitFlexReferenceVolumeDataAPI);
            commitFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/Commit")]
        public void Commit([FromBody] object data)
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
                    commitFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI, commitFlexReferenceVolumeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] where CanCommit = 1
                var customerDataRows = _tempCustomerDataUploadMethods.FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(customerDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var dateFromReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateFrom);
                var dateToReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.DateTo);
                var referenceVolumeReferenceVolumeAttributeId = _customerMethods.ReferenceVolumeAttribute_GetReferenceVolumeAttributeIdByReferenceVolumeAttributeDescription(_customerReferenceVolumeAttributeEnums.ReferenceVolume);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get ContractId by ContractReference
                    var contractReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference);
                    var contractId = _customerMethods.ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference);

                    //Get ReferenceVolumeId from [Customer].[ReferenceVolumeDetail] by DateFrom, DateTo and Reference Volume
                    var dateFrom = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom);
                    var dateTo = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateTo);
                    var referenceVolume = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Volume);

                    var dateFromReferenceVolumeIdList = _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateFromReferenceVolumeAttributeId, dateFrom);
                    var dateToCodeReferenceVolumeIdList = _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(dateToReferenceVolumeAttributeId, dateTo);
                    var referenceVolumeReferenceVolumeIdList = _customerMethods.ReferenceVolumeDetail_GetReferenceVolumeIdListByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription(referenceVolumeReferenceVolumeAttributeId, referenceVolume);

                    var matchingReferenceVolumeIdList = dateFromReferenceVolumeIdList.Intersect(dateToCodeReferenceVolumeIdList).Intersect(referenceVolumeReferenceVolumeIdList);
                    var referenceVolumeId = matchingReferenceVolumeIdList.First();

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
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

