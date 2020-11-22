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
        private readonly Int64 mapCustomerToChildCustomerAPIId;
        private readonly string hostEnvironment;
        #endregion

        public MapCustomerToChildCustomerController(ILogger<MapCustomerToChildCustomerController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().MapCustomerToChildCustomerAPI, password);
            mapCustomerToChildCustomerAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().MapCustomerToChildCustomerAPI);
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(mapCustomerToChildCustomerAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("MapCustomerToChildCustomer/Map")]
        public void Map([FromBody] object data)
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
                    mapCustomerToChildCustomerAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().MapCustomerToChildCustomerAPI, mapCustomerToChildCustomerAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId);

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                //Get Customer GUID
                var customerGUID = systemMethods.GetCustomerGUIDFromJObject(jsonObject);

                //Get Customer Id
                var customerId = customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);

                //Get Current Customer Child Mapping
                var customerChildList = mappingMethods.CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(customerId);

                //Get New Customer Child Mapping
                var customerChildData = new Methods().GetArray(systemMethods.GetChildCustomerDataFromJObject(jsonObject), "{", "}");

                //Get Customer Name attribute Id
                var customerNameAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(new Enums.CustomerSchema.Customer.Attribute().CustomerName);

                var newChildCustomerIds = new List<Int64>();
                var deleteChildCustomerIds = new List<Int64>();

                //Loop through each child
                foreach(var record in customerChildData)
                {
                    var type = record.Split(':')[0];
                    var value = record.Split(':')[1];
                    var childCustomerId = customerMethods.CustomerDetail_GetCustomerIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameAttributeId, value);

                    newChildCustomerIds.Add(childCustomerId);
                }

                foreach(var childCustomer in customerChildList)
                {
                    if(!newChildCustomerIds.Contains(childCustomer))
                    {
                        //Existing child customer does not exist in new list so end date mapping
                        mappingMethods.CustomerToChildCustomer_DeleteByCustomerIdAndChildCustomerId(customerId, childCustomer);
                    }
                }

                foreach(var childCustomer in newChildCustomerIds)
                {
                    if(!customerChildList.Contains(childCustomer))
                    {
                        //New child customer does not exist so insert
                        mappingMethods.CustomerToChildCustomer_Insert(createdByUserId, sourceId, customerId, childCustomer);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, mapCustomerToChildCustomerAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
