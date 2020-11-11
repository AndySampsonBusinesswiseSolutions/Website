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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Asset.Attribute _customerAssetAttributeEnums = new Enums.CustomerSchema.Asset.Attribute();
        private readonly Enums.CustomerSchema.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitAssetToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitAssetToSubMeterDataController(ILogger<CommitAssetToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitAssetToSubMeterDataAPI, password);
            commitAssetToSubMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitAssetToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/Commit")]
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
                    commitAssetToSubMeterDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI, commitAssetToSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var tempCustomerDataUploadSubMeterMethods = new Methods.Temp.CustomerDataUpload.SubMeter();
                var subMeterEntities = tempCustomerDataUploadSubMeterMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
                    return;
                }

                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                var assetNameAssetAttributeId = _customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(_customerAssetAttributeEnums.AssetName);

                var assets = commitableSubMeterEntities.Select(csme => csme.Asset).Distinct()
                    .ToDictionary(a => a, a => GetAssetId(a, createdByUserId, sourceId, assetNameAssetAttributeId));
                
                var subMeters = commitableSubMeterEntities.Select(csme => csme.SubMeterIdentifier).Distinct()
                    .ToDictionary(s => s, s => _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, s));

                var existingAssetToSubMeterMappings = _mappingMethods.AssetToSubMeter_GetLatestTuple();

                foreach(var subMeterEntity in commitableSubMeterEntities)
                {
                    //Get AssetId from [Customer].[AssetDetail]
                    var assetId = assets[subMeterEntity.Asset];

                    //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier
                    var subMeterId = subMeters[subMeterEntity.SubMeterIdentifier];

                    if(!existingAssetToSubMeterMappings.Any(e => e.Item1 == assetId && e.Item2 == subMeterId))
                    {
                        //Insert into [Mapping].[AssetToSubMeter]
                        _mappingMethods.AssetToSubMeter_Insert(createdByUserId, sourceId, assetId, subMeterId);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetAssetId(string asset, long createdByUserId, long sourceId, long assetNameAssetAttributeId)
        {
            var assetId = _customerMethods.Asset_GetAssetIdByAssetAttributeIdAndAssetDetailDescription(assetNameAssetAttributeId, asset);

            if(assetId == 0)
            {
                assetId =_customerMethods.InsertNewAsset(createdByUserId, sourceId);

                //Insert into [Customer].[AssetDetail]
                _customerMethods.AssetDetail_Insert(createdByUserId, sourceId, assetId, assetNameAssetAttributeId, asset);
            }

            return assetId;
        }
    }
}