using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace CreateManageCustomersWebpageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostEnvironment = "Development";
            var password = "J33e23q9DXJw5aTL";

            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateManageCustomersWebpageAPI, password);
            var createManageCustomersWebpageAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateManageCustomersWebpageAPI);

            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

            //Get Process Queue GUID
            var jsonObject = JObject.Parse(args[0]);
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            //Insert into ProcessQueue
            systemMethods.ProcessQueue_Insert(
                processQueueGUID, 
                createdByUserId,
                sourceId,
                createManageCustomersWebpageAPIId);

            //Update Process Queue
            systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createManageCustomersWebpageAPIId);

            var customerMethods = new Methods.CustomerSchema();

            //Get Page Id
            var pageId = systemMethods.Page_GetPageIdByGUID(new Enums.SystemSchema.Page.GUID().ManageCustomers);

            //TODO: Get Role

            //Get Customer GUID
            var customerGUID = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().CustomerGUID].ToString();

            //Get Customer Id
            var customerId = customerMethods.Customer_GetCustomerIdByCustomerGUID(customerGUID);
            var customerIds = new List<long>{customerId};

            //TODO: If Customer Id is 0, then check Role to load customers
            if(customerId == 0)
            {
                //Get all customer Ids
                customerIds = customerMethods.Customer_GetCustomerIdList();
            }

            //Get Customer Name attribute Id
            var customerNameAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(new Enums.CustomerSchema.Customer.Attribute().CustomerName);

            //Get all CustomerToChildCustomer mappings
            var customerToChildCustomerMappings = new Methods.MappingSchema().CustomerToChildCustomer_GetCustomerIdToChildCustomerIdDictionary();

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
                    var customerName = customerMethods.CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(customer, customerNameAttributeId);
                    customerNames.Add(customerName, new List<string>());

                    if(customerToChildCustomerMappings.Select(cc => cc.Key).Any(c => c == customer))
                    {
                        var children = customerToChildCustomerMappings[customer];

                        foreach(var child in children)
                        {
                            var childCustomerName = customerMethods.CustomerDetail_GetCustomerDetailDescriptionByCustomerIdAndCustomerAttributeId(child, customerNameAttributeId);
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
            systemMethods.PageRequest_Insert(createdByUserId, sourceId, pageId, processQueueGUID, tree);

            //Update Process Queue
            systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createManageCustomersWebpageAPIId);
        }
    }
}