﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace AddNewCustomer.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class AddNewCustomerController : ControllerBase
    {
        private readonly ILogger<AddNewCustomerController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private readonly Int64 addNewCustomerAPIId;

        public AddNewCustomerController(ILogger<AddNewCustomerController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.AddNewCustomerAPI, _systemAPIPasswordEnums.AddNewCustomerAPI);
            addNewCustomerAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.AddNewCustomerAPI);
        }

        [HttpPost]
        [Route("AddNewCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(addNewCustomerAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("AddNewCustomer/Add")]
        public void Add([FromBody] object data)
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
                    addNewCustomerAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.AddNewCustomerAPI, addNewCustomerAPIId, jsonObject))
                {
                    return;
                }

                //Get Customer Name attribute Id
                var customerNameAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);

                //Split Customer Data to an array of attribute/value
                var customerData = _methods.GetArray(jsonObject["CustomerData"].ToString(), "{", "}");

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
                    _systemMethods.ProcessQueue_Update(processQueueGUID, addNewCustomerAPIId, false, null);
                }
                else 
                {
                    //Customer name exists as an active customer so fail
                    _systemMethods.ProcessQueue_Update(processQueueGUID, addNewCustomerAPIId, true, $"Customer Name {customerName} already exists as an active record");
                }                
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, addNewCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
