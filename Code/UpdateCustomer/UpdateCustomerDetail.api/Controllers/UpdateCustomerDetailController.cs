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
        private readonly ILogger<UpdateCustomerDetailController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 updateCustomerDetailAPIId;
        private readonly string hostEnvironment;

        public UpdateCustomerDetailController(ILogger<UpdateCustomerDetailController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.UpdateCustomerDetailAPI, password);
            updateCustomerDetailAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.UpdateCustomerDetailAPI);
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(updateCustomerDetailAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/Update")]
        public void Update([FromBody] object data)
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
                    updateCustomerDetailAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.UpdateCustomerDetailAPI, updateCustomerDetailAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, updateCustomerDetailAPIId);

                //Get Customer GUID
                var customerGUID = _systemMethods.GetCustomerGUIDFromJObject(jsonObject);

                //Get Customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Split Customer Data to an array of attribute/value
                var customerData = _methods.GetArray(_systemMethods.GetCustomerDataFromJObject(jsonObject), "{", "}");

                var customerAttributeId = 0L;
                for(var dataCount = 0; dataCount < customerData.Count(); dataCount++)
                {
                    var record = customerData[dataCount];
                    var type = record.Split(':')[0];
                    var value = record.Split(':')[1];

                    if(type == "attribute")
                    {
                        customerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(value);
                    }
                    else
                    {
                        if(customerAttributeId != 0)
                        {
                            var customerDetailDataRow = _customerMethods.CustomerDetail_GetByCustomerIdAndCustomerAttributeId(customerId, customerAttributeId);
                            if(customerDetailDataRow == null)
                            {
                                //Attribute does not exist from this customer so insert
                                _customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, customerAttributeId, value);
                            }
                            else 
                            {
                                var customerDetailDescription = customerDetailDataRow["CustomerDetailDescription"].ToString();
                                if(customerDetailDescription != value)
                                {
                                    //new value is different to current value so end date current value and insert new value
                                    var customerDetailId = Convert.ToInt64(customerDetailDataRow["CustomerDetailId"].ToString());

                                    _customerMethods.CustomerDetail_DeleteByCustomerDetailId(customerDetailId);
                                    _customerMethods.CustomerDetail_Insert(createdByUserId, sourceId, customerId, customerAttributeId, value);
                                }
                            }
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, updateCustomerDetailAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, updateCustomerDetailAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
