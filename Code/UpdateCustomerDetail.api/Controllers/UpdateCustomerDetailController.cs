using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace UpdateCustomerDetail.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UpdateCustomerDetailController : ControllerBase
    {
        private readonly ILogger<UpdateCustomerDetailController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 APIId;

        public UpdateCustomerDetailController(ILogger<UpdateCustomerDetailController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.UpdateCustomerDetailAPI, _systemAPIPasswordEnums.UpdateCustomerDetailAPI);
            APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.UpdateCustomerDetailAPI);
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(APIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("UpdateCustomerDetail/Update")]
        public void Update([FromBody] object data)
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
                    APIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.UpdateCustomerDetailAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, APIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get Customer GUID
                var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();

                //Get Customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Split Customer Data to an array of attribute/value
                var customerData = _methods.GetArray(jsonObject[_systemAPIRequiredDataKeyEnums.CustomerData].ToString(), "{", "}");

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
                _systemMethods.ProcessQueue_Update(processQueueGUID, APIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
