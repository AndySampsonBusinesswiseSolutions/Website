using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceAttribute _informationSourceAttributeEnums = new Enums.Information.SourceAttribute();
        private readonly Enums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new Enums.System.ProcessArchive.Attribute();
        private readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.WebsiteAPI, _systemAPIPasswordEnums.WebsiteAPI);
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Process Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

                //Insert into ProcessQueue
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.WebsiteAPI);

                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    APIId);

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.WebsiteAPI, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, APIId);
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }        
        }

        [HttpPost]
        [Route("Website/GetProcessResponse")]
        public IActionResult GetProcessResponse([FromBody] string processQueueGUID)
        {
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Process Archive Id
                var processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                while(processArchiveId == 0)
                {
                    processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                }

                //Loop until a response record is written into ProcessArchiveDetail
                var responseAttributeId = _systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(_systemProcessArchiveAttributeEnums.Response);
                var response = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, responseAttributeId).FirstOrDefault();

                while(response == null)
                {
                    response = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, responseAttributeId).FirstOrDefault();
                }

                //Create return object with response record
                switch(response)
                {
                    case "OK":
                        return new OkObjectResult(new { message = "OK" });
                    case "ERROR":
                        return new UnauthorizedResult(); //status = 401
                    case "SYSTEM ERROR":
                    default:
                        return new BadRequestResult(); //status = 400
                }
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return new BadRequestResult();
            }
        }

        [HttpPost]
        [Route("Website/GetProcessResponseDetail")]
        public IActionResult GetProcessResponseDetail([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());

            //Get Process Queue GUID
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            //Get Process Archive Id
            var processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);

            //Get API Response Process Archive Attribute Id
            var APIResponseProcesArchiveAttributeId = _systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(_systemProcessArchiveAttributeEnums.APIResponse);

            //Get Process Archive Detail Id List By Process Archive Id and Process Archive Attribute Id
            var processArchiveDetailIdList = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailIdListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, APIResponseProcesArchiveAttributeId);

            //Get API GUID
            var APIGUID = jsonObject[_systemAPIRequiredDataKeyEnums.APIGUID].ToString();

            //Get API Id
            var APIId = _systemMethods.API_GetAPIIdByAPIGUID(APIGUID);

            //Get Process Archive Detail Id List by API Id
            var APIProcessArchiveDetailIdList = _mappingMethods.APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(APIId);

            //Get Process Archive Detail Id that is in both lists
            var processArchiveDetailId = processArchiveDetailIdList.Intersect(APIProcessArchiveDetailIdList).First();

            //Get Process Archive Detail Description by Process Archive Detail Id
            var processArchiveDetailDescription = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(processArchiveDetailId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = processArchiveDetailDescription });
        }

        [HttpPost]
        [Route("Website/BuildCustomerTree")]
        public IActionResult BuildCustomerTree([FromBody] object data) //TODO: Build into new API
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());

            //TODO: Get Role

            //Get Customer GUID
            var customerGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CustomerGUID].ToString();

            //Get Customer Id
            var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);
            var customerIds = new List<long>{customerId};

            //TODO: If Customer Id is 0, then check Role to load customers
            if(customerId == 0)
            {
                //Get all customer Ids
                customerIds = _customerMethods.Customer_GetCustomerIdList();
            }

            //Get Customer Name attribute Id
            var customerNameAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);

            //Get all CustomerToChildCustomer mappings
            var customerToChildCustomerMappings = _mappingMethods.CustomerToChildCustomer_GetCustomerIdToChildCustomerIdDictionary();

            //Loop though Customer Ids, get Customer Name and add to dictionary
            var customerNames = new Dictionary<string, List<string>>();

            //Add New Customer option
            customerNames.Add("Add New Customer", new List<string>());

            foreach(var customer in customerIds)
            {
                //Is customer alone, a child or a parent and child?
                var isChild = customerToChildCustomerMappings.SelectMany(cc => cc.Value).Any(c => c == customer);
                var isParent = true;

                //Customer cannot be a parent if it is a child and not in the parent mapping
                if(isChild && !customerToChildCustomerMappings.Select(cc => cc.Key).Any(c => c == customer))
                {
                    isParent = false;
                }

                if(isParent)
                {
                    var customerName = _customerMethods.CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(customer, customerNameAttributeId);
                    customerNames.Add(customerName, new List<string>());

                    if(customerToChildCustomerMappings.Select(cc => cc.Key).Any(c => c == customer))
                    {
                        var children = customerToChildCustomerMappings[customer];

                        foreach(var child in children)
                        {
                            var childCustomerName = _customerMethods.CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(child, customerNameAttributeId);
                            customerNames[customerName].Add(childCustomerName);
                        }
                    }
                }
            }

            //Build HTML
            var childCustomerId = 0;
            customerId = 0;

            var customerLi = "";
            foreach(var customer in customerNames)
            {
                var childLi = "";
                var ul = $"<ul class='format-listitem'>";

                foreach(var child in customer.Value)
                {
                    var childSpan = $"<span id='ChildCustomer{childCustomerId}span'>{child}</span>";
                    var childIcon = $"<i class='fas fa-customer' style='padding-left: 3px; padding-right: 3px;'></i>";
                    var childCheckbox = $"<input type='checkbox' id='ChildCustomer{childCustomerId}checkbox' GUID='{childCustomerId}' Branch='ChildCustomer' LinkedSite='{customer.Key}' onclick='createCardButton(ChildCustomer{childCustomerId}checkbox)'></input>";
                    var childBranchDiv = $"<i id='ChildCustomer{childCustomerId}' class='far fa-times-circle expander'></i>";

                    var childCustomerLi = $"<li>{childBranchDiv}{childCheckbox}{childIcon}{childSpan}</li>";
                    childLi += $"{childCustomerLi}";
                    childCustomerId++;
                }

                ul += $"{childLi}</ul>";
                var branchListDiv = $"<div id='Customer{customerId}List' class='listitem-hidden'>{ul}</div>";
                var span = $"<span id='Customer{customerId}span'>{customer.Key}</span>";
                var icon = $"<i class='fas fa-customer' style='padding-left: 3px; padding-right: 3px;'></i>";
                var checkbox = $"<input type='checkbox' id='Customer{customerId}checkbox' GUID='{customerId}' Branch='Customer' LinkedSite='{customer.Key}' onclick='createCardButton(Customer{customerId}checkbox)'></input>";
                var branchDiv = $"<i id='Customer{customerId}' class='far fa-times-circle expander'></i>";

                var li = $"<li>{branchDiv}{checkbox}{icon}{span}{branchListDiv}</li>";
                customerLi += $"{li}";
                customerId++;
            }

            var baseUl = $"<ul id='siteSelectorList' class='format-listitem listItemWithoutPadding'>{customerLi}<ul>";
            var tree = $"<div class='scrolling-wrapper'>{baseUl}</div>";
            return new OkObjectResult(new { message = tree });
        }
    }
}
