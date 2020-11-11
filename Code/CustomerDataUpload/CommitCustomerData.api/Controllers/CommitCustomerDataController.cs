using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommitCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCustomerDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Customer.Attribute _customerAttributeEnums = new Enums.CustomerSchema.Customer.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCustomerDataController(ILogger<CommitCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerDataAPI, password);
            commitCustomerDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitCustomerDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerData/Commit")]
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
                    commitCustomerDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitCustomerDataAPI, commitCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCustomerDataAPIId);

                //Get data from [Temp.CustomerUploadData].[Customer] where CanCommit = 1
                var tempCustomerDataUploadCustomerMethods = new Methods.Temp.CustomerDataUpload.Customer();
                var customerEntities = tempCustomerDataUploadCustomerMethods.Customer_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableCustomerEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(customerEntities);

                if(!commitableCustomerEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, false, null);
                    return;
                }

                //For each column, get CustomerAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {_customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName), _customerDataUploadValidationEntityEnums.CustomerName},
                    {_customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.ContactName), _customerDataUploadValidationEntityEnums.ContactName},
                    {_customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.ContactTelephoneNumber), _customerDataUploadValidationEntityEnums.ContactTelephoneNumber},
                    {_customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.ContactEmailAddress), _customerDataUploadValidationEntityEnums.ContactEmailAddress},
                };

                var detailDictionary = new Dictionary<long, string>();

                foreach(var attribute in attributes)
                {
                    detailDictionary.Add(attribute.Key, string.Empty);
                }

                foreach(var customerEntity in commitableCustomerEntities)
                {
                    foreach(var attribute in attributes)
                    {
                        detailDictionary[attribute.Key] = customerEntity.GetType().GetProperty(attribute.Value).GetValue(customerEntity).ToString();
                    }

                    //Get CustomerId by CustomerName
                    var customerId = _customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value);

                    if(customerId == 0)
                    {
                        customerId = _customerMethods.InsertNewCustomer(createdByUserId, sourceId);

                        //Insert into [Customer].[CustomerDetail]
                        foreach(var detail in detailDictionary)
                        {
                            _customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[CustomerDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailEntity = _customerMethods.CustomerDetail_GetByCustomerIdAndCustomerAttributeId(customerId, detail.Key);
                            var currentDetail = currentDetailEntity.Field<string>("CustomerDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var customerDetailId = currentDetailEntity.Field<int>("CustomerDetailId");
                                _customerMethods.CustomerDetail_DeleteByCustomerDetailId(customerDetailId);
                                _customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, detail.Key, detail.Value);
                            }
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}