using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MapCustomerToChildCustomer.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class MapCustomerToChildCustomerController : ControllerBase
    {
        private readonly ILogger<MapCustomerToChildCustomerController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private readonly Int64 mapCustomerToChildCustomerAPIId;

        public MapCustomerToChildCustomerController(ILogger<MapCustomerToChildCustomerController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.MapCustomerToChildCustomerAPI, _systemAPIPasswordEnums.MapCustomerToChildCustomerAPI);
            mapCustomerToChildCustomerAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.MapCustomerToChildCustomerAPI);
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(mapCustomerToChildCustomerAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/Map")]
        public void Map([FromBody] object data)
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
                    mapCustomerToChildCustomerAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.MapCustomerToChildCustomerAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, mapCustomerToChildCustomerAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get Customer GUID
                var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();

                //Get Customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Get Current Customer Child Mapping
                var customerChildList = _mappingMethods.CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(customerId);

                //Get New Customer Child Mapping
                var customerChildData = _methods.GetArray(jsonObject[_systemAPIRequiredDataKeyEnums.ChildCustomerData].ToString(), "{", "}");

                //Get Customer Name attribute Id
                var customerNameAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);

                var newChildCustomerIds = new List<Int64>();
                var deleteChildCustomerIds = new List<Int64>();

                //Loop through each child
                foreach(var record in customerChildData)
                {
                    var type = record.Split(':')[0];
                    var value = record.Split(':')[1];
                    var childCustomerId = _customerMethods.CustomerDetail_GetCustomerIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameAttributeId, value);

                    newChildCustomerIds.Add(childCustomerId);
                }

                foreach(var childCustomer in customerChildList)
                {
                    if(!newChildCustomerIds.Contains(childCustomer))
                    {
                        //Existing child customer does not exist in new list so end date mapping
                        _mappingMethods.CustomerToChildCustomer_DeleteByCustomerIdAndChildCustomerId(customerId, childCustomer);
                    }
                }

                foreach(var childCustomer in newChildCustomerIds)
                {
                    if(!customerChildList.Contains(childCustomer))
                    {
                        //New child customer does not exist so insert
                        _mappingMethods.CustomerToChildCustomer_Insert(createdByUserId, sourceId, customerId, childCustomer);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, mapCustomerToChildCustomerAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, mapCustomerToChildCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
