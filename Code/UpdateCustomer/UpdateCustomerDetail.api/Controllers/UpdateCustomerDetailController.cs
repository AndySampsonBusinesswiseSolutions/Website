using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace UpdateCustomerDetail.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UpdateCustomerDetailController : ControllerBase
    {
        #region Variables
        private readonly ILogger<UpdateCustomerDetailController> _logger;
        private readonly Int64 updateCustomerDetailAPIId;
        private readonly string hostEnvironment;
        #endregion

        public UpdateCustomerDetailController(ILogger<UpdateCustomerDetailController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().UpdateCustomerDetailAPI, password);
            updateCustomerDetailAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().UpdateCustomerDetailAPI);
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(updateCustomerDetailAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/Update")]
        public void Update([FromBody] object data)
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
                    updateCustomerDetailAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().UpdateCustomerDetailAPI, updateCustomerDetailAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, updateCustomerDetailAPIId);

                //Get Customer GUID
                var customerGUID = systemMethods.GetCustomerGUIDFromJObject(jsonObject);

                //Get Customer Id
                var customerMethods = new Methods.CustomerSchema();
                var customerId = customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Split Customer Data to an array of attribute/value
                var customerData = new Methods().GetArray(systemMethods.GetCustomerDataFromJObject(jsonObject), "{", "}");

                var customerAttributeId = 0L;
                for(var dataCount = 0; dataCount < customerData.Count(); dataCount++)
                {
                    var record = customerData[dataCount];
                    var type = record.Split(':')[0];
                    var value = record.Split(':')[1];

                    if(type == "attribute")
                    {
                        customerAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(value);
                    }
                    else
                    {
                        if(customerAttributeId != 0)
                        {
                            var customerDetailEntity = customerMethods.CustomerDetail_GetByCustomerIdAndCustomerAttributeId(customerId, customerAttributeId);
                            if(customerDetailEntity == null)
                            {
                                //Attribute does not exist from this customer so insert
                                customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, customerAttributeId, value);
                            }
                            else 
                            {
                                if(customerDetailEntity.CustomerDetailDescription != value)
                                {
                                    //new value is different to current value so end date current value and insert new value
                                    customerMethods.CustomerDetail_DeleteByCustomerDetailId(customerDetailEntity.CustomerDetailId);
                                    customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, customerAttributeId, value);
                                }
                            }
                        }
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, updateCustomerDetailAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, updateCustomerDetailAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
