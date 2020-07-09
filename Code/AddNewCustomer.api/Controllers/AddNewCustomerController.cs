using Microsoft.AspNetCore.Mvc;
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
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
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
            var jsonObject = JObject.Parse(data.ToString());
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(addNewCustomerAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("AddNewCustomer/Add")]
        public void Add([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    addNewCustomerAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.AddNewCustomerAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, addNewCustomerAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
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
                    var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();
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
