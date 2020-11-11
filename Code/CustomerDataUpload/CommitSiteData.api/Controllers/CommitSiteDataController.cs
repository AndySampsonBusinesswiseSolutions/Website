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
using Microsoft.Extensions.Configuration;

namespace CommitSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSiteDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Site.Attribute _customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSiteDataController(ILogger<CommitSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSiteDataAPI, password);
            commitSiteDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSiteDataAPI, commitSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteEntities = new Methods.Temp.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSiteEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(siteEntities);

                if(!commitableSiteEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, false, null);
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

                foreach(var siteEntity in commitableSiteEntities)
                {
                    foreach(var attribute in attributes)
                    {
                        detailDictionary[attribute.Key] = siteEntity.GetType().GetProperty(attribute.Value).GetValue(siteEntity).ToString();
                    }

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, detailDictionary[siteNameSiteAttributeId]);
                    var sitePostCodeSiteIdList = _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, detailDictionary[sitePostCodeSiteAttributeId]);

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);
                    var siteId = matchingSiteIdList.FirstOrDefault();

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
                            var currentDetailEntity = _customerMethods.SiteDetail_GetBySiteIdAndSiteAttributeId(siteId, detail.Key);
                            var currentDetail = currentDetailEntity.Field<string>("SiteDetailDescription");

                            if(detail.Value != currentDetail)
                            {
                                var siteDetailId = currentDetailEntity.Field<int>("SiteDetailId");
                                _customerMethods.SiteDetail_DeleteBySiteDetailId(siteDetailId);
                                _customerMethods.SiteDetail_Insert(createdByUserId, sourceId, siteId, detail.Key, detail.Value);
                            }
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}