using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AddNewCustomer.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class AddNewCustomerController : ControllerBase
    {
        #region Variables
        private readonly ILogger<AddNewCustomerController> _logger;
        private readonly Int64 addNewCustomerAPIId;
        private readonly string hostEnvironment;
        #endregion

        public AddNewCustomerController(ILogger<AddNewCustomerController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().AddNewCustomerAPI, password);
            addNewCustomerAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().AddNewCustomerAPI);
        }

        [HttpPost]
        [Route("AddNewCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(addNewCustomerAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("AddNewCustomer/Add")]
        public void Add([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    addNewCustomerAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().AddNewCustomerAPI, addNewCustomerAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, addNewCustomerAPIId);

                var customerMethods = new Methods.Customer();

                //Get Customer Name attribute Id
                var customerNameAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(new Enums.CustomerSchema.Customer.Attribute().CustomerName);

                //Split Customer Data to an array of attribute/value
                var customerData = new Methods().GetArray(jsonObject["CustomerData"].ToString(), "{", "}");

                //Loop through array and find Customer Name attribute
                var customerName = "";
                for(var dataCount = 0; dataCount < customerData.Count(); dataCount++)
                {
                    var record = customerData[dataCount];
                    var type = record.Split(':')[0];
                    var value = record.Split(':')[1];

                    if(type == "attribute" && value == "Customer Name")
                    {
                        customerName = customerData[dataCount + 1].Split(':')[1];
                        break;
                    }
                }

                //Check if customer name exists
                var customerDetailId = customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameAttributeId, customerName);

                if(customerDetailId == 0)
                {
                    //Customer name does not exist as an active customer so insert
                    var customerGUID = systemMethods.GetCustomerGUIDFromJObject(jsonObject);
                    customerMethods.Customer_Insert(createdByUserId, sourceId, customerGUID);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, false, null);
                }
                else 
                {
                    //Customer name exists as an active customer so fail
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, true, $"Customer Name {customerName} already exists as an active record");
                }
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
