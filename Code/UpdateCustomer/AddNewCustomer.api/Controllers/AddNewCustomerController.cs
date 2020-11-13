﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Customer.Attribute _customerAttributeEnums = new Enums.CustomerSchema.Customer.Attribute();
        private readonly Int64 addNewCustomerAPIId;
        private readonly string hostEnvironment;
        #endregion

        public AddNewCustomerController(ILogger<AddNewCustomerController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().AddNewCustomerAPI, password);
            addNewCustomerAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.AddNewCustomerAPI);
        }

        [HttpPost]
        [Route("AddNewCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(addNewCustomerAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("AddNewCustomer/Add")]
        public void Add([FromBody] object data)
        {

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
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
                    addNewCustomerAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.AddNewCustomerAPI, addNewCustomerAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, addNewCustomerAPIId);

                //Get Customer Name attribute Id
                var customerNameAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);

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
                var customerDetailId = _customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameAttributeId, customerName);

                if(customerDetailId == 0)
                {
                    //Customer name does not exist as an active customer so insert
                    var customerGUID = _systemMethods.GetCustomerGUIDFromJObject(jsonObject);
                    _customerMethods.Customer_Insert(createdByUserId, sourceId, customerGUID);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, false, null);
                }
                else 
                {
                    //Customer name exists as an active customer so fail
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, true, $"Customer Name {customerName} already exists as an active record");
                }
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, addNewCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
