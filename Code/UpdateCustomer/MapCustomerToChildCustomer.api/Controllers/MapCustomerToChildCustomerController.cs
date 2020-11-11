using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MapCustomerToChildCustomer.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class MapCustomerToChildCustomerController : ControllerBase
    {
        #region Variables
        private readonly ILogger<MapCustomerToChildCustomerController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Customer.Attribute _customerAttributeEnums = new Enums.CustomerSchema.Customer.Attribute();
        private readonly Int64 mapCustomerToChildCustomerAPIId;
        private readonly string hostEnvironment;
        #endregion

        public MapCustomerToChildCustomerController(ILogger<MapCustomerToChildCustomerController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().MapCustomerToChildCustomerAPI, password);
            mapCustomerToChildCustomerAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.MapCustomerToChildCustomerAPI);
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(mapCustomerToChildCustomerAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/Map")]
        public void Map([FromBody] object data)
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
                    mapCustomerToChildCustomerAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.MapCustomerToChildCustomerAPI, mapCustomerToChildCustomerAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId);

                //Get Customer GUID
                var customerGUID = _systemMethods.GetCustomerGUIDFromJObject(jsonObject);

                //Get Customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Get Current Customer Child Mapping
                var customerChildList = _mappingMethods.CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(customerId);

                //Get New Customer Child Mapping
                var customerChildData = new Methods().GetArray(_systemMethods.GetChildCustomerDataFromJObject(jsonObject), "{", "}");

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
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
