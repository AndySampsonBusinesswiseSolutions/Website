using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CreateManageCustomersWebpage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateManageCustomersWebpageController : ControllerBase
    {
        private readonly ILogger<CreateManageCustomersWebpageController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        public readonly Methods.Customer _customerMethods = new Methods.Customer();
        public readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.Page.GUID _systemPageGUIDEnums = new Enums.System.Page.GUID();
        private readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private readonly Int64 createManageCustomersWebpageAPIId;
        private readonly string hostEnvironment;

        public CreateManageCustomersWebpageController(ILogger<CreateManageCustomersWebpageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CreateManageCustomersWebpageAPI, password);
            createManageCustomersWebpageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateManageCustomersWebpageAPI);
        }

        [HttpPost]
        [Route("CreateManageCustomersWebpage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createManageCustomersWebpageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateManageCustomersWebpage/Create")]
        public void Create([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Process Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            //Insert into ProcessQueue
            _systemMethods.ProcessQueue_Insert(
                processQueueGUID, 
                createdByUserId,
                sourceId,
                createManageCustomersWebpageAPIId);

            //Update Process Queue
            _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createManageCustomersWebpageAPIId);

            //Get Page Id
            var pageId = _systemMethods.Page_GetPageIdByGUID(_systemPageGUIDEnums.ManageCustomers);

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
            
            //Write HTML to System.PageRequest
            _systemMethods.PageRequest_Insert(createdByUserId, sourceId, pageId, processQueueGUID, tree);

            //Update Process Queue
            _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createManageCustomersWebpageAPIId);
        }
    }
}