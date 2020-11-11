using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitAssetToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAssetToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitAssetToSubMeterDataController> _logger;
        private readonly Int64 commitAssetToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitAssetToSubMeterDataController(ILogger<CommitAssetToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitAssetToSubMeterDataAPI, password);
            commitAssetToSubMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitAssetToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitAssetToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitAssetToSubMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitAssetToSubMeterDataAPI, commitAssetToSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var tempCustomerDataUploadSubMeterMethods = new Methods.Temp.CustomerDataUpload.SubMeter();
                var subMeterEntities = tempCustomerDataUploadSubMeterMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.Customer();
                var mappingMethods = new Methods.Mapping();
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var assetNameAssetAttributeId = customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(new Enums.CustomerSchema.Asset.Attribute().AssetName);

                var assets = commitableSubMeterEntities.Select(csme => csme.Asset).Distinct()
                    .ToDictionary(a => a, a => customerMethods.GetAssetId(a, createdByUserId, sourceId, assetNameAssetAttributeId));
                
                var subMeters = commitableSubMeterEntities.Select(csme => csme.SubMeterIdentifier).Distinct()
                    .ToDictionary(s => s, s => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, s));

                var newAssetToSubMeterEntities = commitableSubMeterEntities.Where(csme => mappingMethods.AssetToSubMeter_GetAssetToSubMeterIdByAssetIdAndSubMeterId(assets[csme.Asset], subMeters[csme.SubMeterIdentifier]) == 0)
                    .GroupBy(csme => new { csme.Asset, csme.SubMeterIdentifier }).ToList();

                foreach(var subMeterEntity in newAssetToSubMeterEntities)
                {
                    //Insert into [Mapping].[AssetToSubMeter]
                    mappingMethods.AssetToSubMeter_Insert(createdByUserId, sourceId, assets[subMeterEntity.Key.Asset], subMeters[subMeterEntity.Key.SubMeterIdentifier]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}