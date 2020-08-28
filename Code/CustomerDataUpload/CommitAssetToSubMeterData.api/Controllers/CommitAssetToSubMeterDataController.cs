using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitAssetToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAssetToSubMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitAssetToSubMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Asset.Attribute _customerAssetAttributeEnums = new Enums.Customer.Asset.Attribute();
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitAssetToSubMeterDataAPIId;

        public CommitAssetToSubMeterDataController(ILogger<CommitAssetToSubMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitAssetToSubMeterDataAPI, _systemAPIPasswordEnums.CommitAssetToSubMeterDataAPI);
            commitAssetToSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitAssetToSubMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/Commit")]
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
                    commitAssetToSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI, commitAssetToSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
                    return;
                }

                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                var assetNameAssetAttributeId = _customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(_customerAssetAttributeEnums.AssetName);

                var assets = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.Asset))
                    .Distinct()
                    .ToDictionary(a => a, a => GetAssetId(a, createdByUserId, sourceId, assetNameAssetAttributeId));
                
                var subMeters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier))
                    .Distinct()
                    .ToDictionary(s => s, s => _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, s));

                foreach(var dataRow in commitableDataRows)
                {
                    //Get AssetId from [Customer].[AssetDetail]
                    var assetId = assets[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Asset)];

                    //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier
                    var subMeterId = subMeters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier)];

                    //Insert into [Mapping].[AssetToSubMeter]
                    _mappingMethods.AssetToSubMeter_Insert(createdByUserId, sourceId, assetId, subMeterId);
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