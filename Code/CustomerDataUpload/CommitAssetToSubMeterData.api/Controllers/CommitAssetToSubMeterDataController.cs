using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitAssetToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAssetToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitAssetToSubMeterDataController> _logger;
        private readonly Entity.System.API.CommitAssetToSubMeterData.Configuration _configuration;
        #endregion

        public CommitAssetToSubMeterDataController(ILogger<CommitAssetToSubMeterDataController> logger, Entity.System.API.CommitAssetToSubMeterData.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(_configuration.APIId, _configuration.HostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
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
                    _configuration.APIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(_configuration.APIGUID, _configuration.APIId, _configuration.HostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, _configuration.APIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
                    return;
                }

                var customerMethods = new Methods.Customer();
                var mappingMethods = new Methods.Mapping();

                var assets = commitableSubMeterEntities.Select(csme => csme.Asset).Distinct()
                    .ToDictionary(a => a, a => customerMethods.GetAssetId(a, createdByUserId, sourceId, _configuration.AssetNameAssetAttributeId));
                
                var subMeters = commitableSubMeterEntities.Select(csme => csme.SubMeterIdentifier).Distinct()
                    .ToDictionary(s => s, s => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(_configuration.SubMeterIdentifierSubMeterAttributeId, s));

                var newAssetToSubMeterEntities = commitableSubMeterEntities.Where(csme => mappingMethods.AssetToSubMeter_GetAssetToSubMeterIdByAssetIdAndSubMeterId(assets[csme.Asset], subMeters[csme.SubMeterIdentifier]) == 0)
                    .GroupBy(csme => new { csme.Asset, csme.SubMeterIdentifier }).ToList();

                foreach(var subMeterEntity in newAssetToSubMeterEntities)
                {
                    //TODO:Delete existing AssetToSubMeter mappings

                    //Insert into [Mapping].[AssetToSubMeter]
                    mappingMethods.AssetToSubMeter_Insert(createdByUserId, sourceId, assets[subMeterEntity.Key.Asset], subMeters[subMeterEntity.Key.SubMeterIdentifier]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}