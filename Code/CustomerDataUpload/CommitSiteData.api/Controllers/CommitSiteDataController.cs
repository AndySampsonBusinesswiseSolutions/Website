using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace CommitSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSiteDataController : ControllerBase
    {
        private readonly ILogger<CommitSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitSiteDataAPIId;

        public CommitSiteDataController(ILogger<CommitSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitSiteDataAPI, _systemAPIPasswordEnums.CommitSiteDataAPI);
            commitSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitSiteDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSiteDataAPI, commitSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteDataRows = _tempCustomerDataUploadMethods.Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(siteDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitSiteDataAPIId, false, null);
                    return;
                }

                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode);

                //For each column, get CustomerAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {siteNameSiteAttributeId, _customerDataUploadValidationEntityEnums.SiteName},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteAddress), _customerDataUploadValidationEntityEnums.SiteAddress},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteTown), _customerDataUploadValidationEntityEnums.SiteTown},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteCounty), _customerDataUploadValidationEntityEnums.SiteCounty},
                    {sitePostCodeSiteAttributeId, _customerDataUploadValidationEntityEnums.SitePostCode},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteDescription), _customerDataUploadValidationEntityEnums.SiteDescription},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.ContactName), _customerDataUploadValidationEntityEnums.ContactName},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.ContactRole), _customerDataUploadValidationEntityEnums.ContactRole},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.ContactTelephoneNumber), _customerDataUploadValidationEntityEnums.ContactTelephoneNumber},
                    {_customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.ContactEmailAddress), _customerDataUploadValidationEntityEnums.ContactEmailAddress},
                };

                var detailDictionary = new Dictionary<long, string>();

                foreach(var attribute in attributes)
                {
                    detailDictionary.Add(attribute.Key, string.Empty);
                }

                foreach(var dataRow in commitableDataRows)
                {
                    foreach(var attribute in attributes)
                    {
                        detailDictionary[attribute.Key] = dataRow.Field<string>(attribute.Value);
                    }

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, detailDictionary[siteNameSiteAttributeId]);
                    var sitePostCodeSiteIdList = _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, detailDictionary[sitePostCodeSiteAttributeId]);

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);
                    var siteId = matchingSiteIdList.First();

                    //TODO: What to do if the update would make another matching entry?

                    if(siteId == 0)
                    {
                        siteId = _customerMethods.InsertNewSite(createdByUserId, sourceId);

                        //Insert into [Site].[SiteDetail]
                        foreach(var detail in detailDictionary)
                        {
                            _customerMethods.SiteDetail_Insert(createdByUserId, sourceId, siteId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Site].[SiteDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailDataRow = _customerMethods.SiteDetail_GetBySiteIdAndSiteAttributeId(siteId, detail.Key);
                            var currentDetail = currentDetailDataRow.Field<string>("SiteDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var siteDetailId = currentDetailDataRow.Field<int>("SiteDetailId");
                                _customerMethods.SiteDetail_DeleteBySiteDetailId(siteDetailId);
                                _customerMethods.SiteDetail_Insert(createdByUserId, sourceId, siteId, detail.Key, detail.Value);
                            }
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

