using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CreateDataAnalysisWebpage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDataAnalysisWebpageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateDataAnalysisWebpageController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.CustomerSchema.Asset.Attribute _customerAssetAttributeEnums = new Enums.CustomerSchema.Asset.Attribute();
        private readonly Enums.CustomerSchema.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
        private readonly Enums.SystemSchema.Page.GUID _systemPageGUIDEnums = new Enums.SystemSchema.Page.GUID();
        private readonly Int64 createDataAnalysisWebpageAPIId;
        private string HTML = string.Empty;
        private JObject jsonObject;
        private Dictionary<string, long> attributeDictionary = new Dictionary<string, long>();
        private Dictionary<long, long> meterCommodityDictionary = new Dictionary<long, long>();
        private Dictionary<long, string> commodityDictionary = new Dictionary<long, string>();
        private Dictionary<long, string> areaDictionary = new Dictionary<long, string>();
        private Dictionary<long, string> subAreaDictionary = new Dictionary<long, string>();
        private Dictionary<long, string> assetGUIDDictionary = new Dictionary<long, string>();
        private Dictionary<long, string> assetNameDictionary = new Dictionary<long, string>();
        private FilterData filterData;
        private string hostEnvironment;

        private class FilterData    {
            public bool SiteChecked { get; set; } 
            public bool AreaChecked { get; set; } 
            public bool CommodityChecked { get; set; } 
            public bool MeterChecked { get; set; } 
            public bool SubAreaChecked { get; set; } 
            public bool AssetChecked { get; set; } 
            public bool SubMeterChecked { get; set; } 
            public List<string> Commodities { get; set; } 
        }
        #endregion

        public CreateDataAnalysisWebpageController(ILogger<CreateDataAnalysisWebpageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateDataAnalysisWebpageAPI, password);
            createDataAnalysisWebpageAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateDataAnalysisWebpageAPI);
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(createDataAnalysisWebpageAPIId, hostEnvironment, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/BuildLocationTree")]
        public void BuildLocationTree([FromBody] object data)
        {

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            filterData = JsonConvert.DeserializeObject<FilterData>(jsonObject["FilterData"].ToString()); 

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    createDataAnalysisWebpageAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createDataAnalysisWebpageAPIId);

                //Setup required Attribute Ids
                GetRequiredAttributes();

                //Setup commodityDictionary
                commodityDictionary = _informationMethods.Commodity_GetCommodityDictionary();

                //TODO: Get Customer GUIDs

                //If no customers selected, get all customers linked to user
                var customerIds = _customerMethods.Customer_GetCustomerIdList();

                foreach (var customerId in customerIds)
                {
                    //Get sites linked to customer
                    var siteIds = _mappingMethods.CustomerToSite_GetSiteIdListByCustomerId(customerId).Distinct().ToList();;

                    //Build branches for sites
                    BuildSiteBranch(siteIds);
                }

                var baseUl = $"<ul id='siteSelectorList' class='format-listitem listItemWithoutPadding'>{HTML}<ul>";

                //Get Page Id
                var pageId = _systemMethods.Page_GetPageIdByGUID(_systemPageGUIDEnums.DataAnalysis);

                //Write HTML to System.PageRequest
                _systemMethods.PageRequest_Insert(createdByUserId, sourceId, pageId, processQueueGUID, baseUl);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDataAnalysisWebpageAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDataAnalysisWebpageAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetRequiredAttributes()
        {
            //Get MeterIdentifierMeterAttributeId
            attributeDictionary.Add("MeterIdentifier", _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier));

            //Get AssetNameAssetAttributeId
            attributeDictionary.Add("AssetName", _customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(_customerAssetAttributeEnums.AssetName));

            //Get SubMeterIdentifierSubMeterAttributeId
            attributeDictionary.Add("SubMeterIdentifier", _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier));
        }

        private void BuildSiteBranch(List<long> siteIds)
        {
            var siteMeterDictionary = siteIds.ToDictionary(s => s, s => _mappingMethods.MeterToSite_GetMeterIdListBySiteId(s).Distinct().ToList());
            meterCommodityDictionary = siteMeterDictionary.Values.SelectMany(m => m).ToDictionary(m => m, m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m));
            var commodityMatchingSiteDictionary = siteMeterDictionary.Where(s => CommodityMeterMatch(s.Value)).ToDictionary(s => s.Key, s => s.Value.Where(m => CommodityMeterMatch(m)).ToList());

            if(filterData.SiteChecked)
            {
                foreach(var site in commodityMatchingSiteDictionary)
                {
                    //Get Site GUID
                    var siteGUID = _customerMethods.Site_GetSiteGUIDBySiteId(site.Key).ToString();

                    //Get Site name
                    var siteName = _customerMethods.GetSiteName(site.Key);

                    //Get areas linked to site
                    var areaMeterDictionary = BuildAreaMeterDictionary(site.Value);

                    //Finish html
                    var areaHTML = BuildAreaBranch(areaMeterDictionary, site.Key);
                    var siteHTML = GetLiHtml("Site", siteGUID, siteName, areaHTML);
                    HTML += siteHTML;
                }
            }
            else
            {
                var meterIds = commodityMatchingSiteDictionary.SelectMany(s => s.Value).ToList();
                var areaMeterDictionary = BuildAreaMeterDictionary(meterIds);
                HTML = BuildAreaBranch(areaMeterDictionary, 0);
            }
        }

        private string BuildAreaBranch(Dictionary<long, List<long>> areaMeterDictionary, long siteId)
        {
            if(filterData.AreaChecked)
            {
                var areaHTML = string.Empty;

                foreach(var areaMeter in areaMeterDictionary)
                {
                    //Get Area description
                    if(!areaDictionary.ContainsKey(areaMeter.Key))
                    {
                        areaDictionary.Add(areaMeter.Key, _informationMethods.Area_GetAreaDescriptionByAreaId(areaMeter.Key));
                    }

                    var commodityHTML = BuildCommodityBranch(areaMeter.Value, siteId, areaMeter.Key);
                    areaHTML += GetLiHtml("Area", $"{siteId}_{areaMeter.Key}", areaDictionary[areaMeter.Key], commodityHTML);
                }

                return areaHTML;
            }
            else
            {
                var meterIds = areaMeterDictionary.SelectMany(s => s.Value).ToList();
                return BuildCommodityBranch(meterIds, siteId, 0);
            }
        }

        private string BuildCommodityBranch(List<long> meterIds, long siteId, long areaId)
        {
            if(filterData.CommodityChecked)
            {
                var commodityHTML = string.Empty;

                //Get commodities linked to meters
                var commodityIds = meterIds.Select(m => meterCommodityDictionary[m]).Distinct().ToList();

                foreach(var commodityId in commodityIds)
                {
                    //Get Commodity description
                    var commodity = commodityDictionary[commodityId];
                    var commodityMeterIds = meterIds.Where(m => meterCommodityDictionary[m] == commodityId).ToList();

                    var meterHTML = BuildMeterBranch(siteId, areaId, commodityId, commodityMeterIds);
                    commodityHTML += GetLiHtml("Commodity", $"{siteId}_{areaId}_{commodityId}", commodity, meterHTML);
                }

                return commodityHTML;
            }
            else
            {
                return BuildMeterBranch(siteId, areaId, 0, meterIds);
            }
        }

        private string BuildMeterBranch(long siteId, long areaId, long commodityId, List<long> meterIds)
        {
            var meterSubMeterDictionary = meterIds.ToDictionary(m => m, m => _mappingMethods.MeterToSubMeter_GetSubMeterIdListByMeterId(m));

            if(filterData.MeterChecked)
            {
                var meterHTML = string.Empty;

                foreach(var meter in meterSubMeterDictionary)
                {
                    //Get Meter GUID
                    var meterGUID = _customerMethods.Meter_GetMeterGUIDByMeterId(meter.Key).ToString();

                    //Get Meter identifier
                    var meterIdentifier = _customerMethods.MeterDetail_GetMeterDetailDescriptionByMeterIdAndMeterAttributeId(meter.Key, attributeDictionary["MeterIdentifier"]);

                    //Get subareas linked to meter
                    var subAreaSubMeterDictionary = BuildSubAreaSubMeterDictionary(meter.Value);

                    //Finish html
                    var subAreaHTML = BuildSubAreaBranch(subAreaSubMeterDictionary, siteId, areaId, commodityId, meter.Key);
                    meterHTML += GetLiHtml("Meter", meterGUID, meterIdentifier, subAreaHTML);
                }

                return meterHTML;
            }
            else
            {
                var subMeterIds = meterSubMeterDictionary.SelectMany(m => m.Value).ToList();
                var subAreaSubMeterDictionary = BuildSubAreaSubMeterDictionary(subMeterIds);
                return BuildSubAreaBranch(subAreaSubMeterDictionary, siteId, areaId, commodityId, 0);
            }
        }

        private string BuildSubAreaBranch(Dictionary<long, List<long>> subAreaSubMeterDictionary, long siteId, long areaId, long commodityId, long meterId)
        {
            if(filterData.SubAreaChecked)
            {
                var subAreaHTML = string.Empty;

                foreach(var subAreaSubMeter in subAreaSubMeterDictionary)
                {
                    //Get SubArea description
                    if(!subAreaDictionary.ContainsKey(subAreaSubMeter.Key))
                    {
                        subAreaDictionary.Add(subAreaSubMeter.Key, _informationMethods.SubArea_GetSubAreaDescriptionBySubAreaId(subAreaSubMeter.Key));
                    }

                    var assetHTML = BuildAssetBranch(subAreaSubMeter.Value);
                    subAreaHTML += GetLiHtml("SubArea", $"{siteId}_{areaId}_{commodityId}_{meterId}_{subAreaSubMeter.Key}", subAreaDictionary[subAreaSubMeter.Key], assetHTML);
                }

                return subAreaHTML;
            }
            else
            {
                var subMeterIds = subAreaSubMeterDictionary.SelectMany(s => s.Value).ToList();
                return BuildAssetBranch(subMeterIds);
            }
        }

        private string BuildAssetBranch(List<long> subMeterIds)
        {
            if(filterData.AssetChecked)
            {
                var assetHTML = string.Empty;

                //Get assets linked to submeters
                var subMeterAssetDictionary = subMeterIds.ToDictionary(s => s, s => _mappingMethods.AssetToSubMeter_GetAssetIdBySubMeterId(s));
                var assetIds = subMeterAssetDictionary.Values.Select(a => a).Distinct().ToList();

                foreach(var assetId in assetIds)
                {
                    //Get Asset GUID
                    if(!assetGUIDDictionary.ContainsKey(assetId))
                    {
                        assetGUIDDictionary.Add(assetId, _customerMethods.Asset_GetAssetGUIDByAssetId(assetId).ToString());
                    }

                    //Get Asset name
                    if(!assetNameDictionary.ContainsKey(assetId))
                    {
                        assetNameDictionary.Add(assetId, _customerMethods.AssetDetail_GetAssetDetailDescriptionByAssetIdAndAssetAttributeId(assetId, attributeDictionary["AssetName"]));
                    }

                    var assetSubMeterIds = subMeterIds.Where(s => subMeterAssetDictionary[s] == assetId).ToList();

                    var subMeterHTML = BuildSubMeterBranch(assetSubMeterIds);
                    assetHTML += GetLiHtml("Asset", assetGUIDDictionary[assetId], assetNameDictionary[assetId], subMeterHTML);
                }

                return assetHTML;
            }
            else
            {
                return BuildSubMeterBranch(subMeterIds);
            }
        }

        private string BuildSubMeterBranch(List<long> subMeterIds)
        {
            if(filterData.SubMeterChecked)
            {
                var subMeterHTML = string.Empty;

                foreach(var subMeterId in subMeterIds)
                {
                    //Get SubMeter GUID
                    var subMeterGUID = _customerMethods.SubMeter_GetSubMeterGUIDBySubMeterId(subMeterId).ToString();

                    //Get SubMeter identifier
                    var subMeterIdentifier = _customerMethods.SubMeterDetail_GetSubMeterDetailDescriptionBySubMeterIdAndSubMeterAttributeId(subMeterId, attributeDictionary["SubMeterIdentifier"]);

                    //Finish html
                    subMeterHTML += GetLiHtml("SubMeter", subMeterGUID, subMeterIdentifier, string.Empty);
                }

                return subMeterHTML;
            }
            else
            {
                return string.Empty;
            }
        }

        private Dictionary<long, List<long>> BuildAreaMeterDictionary(List<long> meterIds)
        {
            var areaMeterDictionary = new Dictionary<long, List<long>>();

            foreach(var meterId in meterIds)
            {
                var areaId = _mappingMethods.AreaToMeter_GetAreaIdByMeterId(meterId);
                if(!areaMeterDictionary.ContainsKey(areaId))
                {
                    areaMeterDictionary.Add(areaId, new List<long>());
                }
                areaMeterDictionary[areaId].Add(meterId);
            }

            return areaMeterDictionary;
        }

        private Dictionary<long, List<long>> BuildSubAreaSubMeterDictionary(List<long> subMeterIds)
        {
            var subAreaSubMeterDictionary = new Dictionary<long, List<long>>();

            foreach(var subMeterId in subMeterIds)
            {
                var subAreaId = _mappingMethods.SubAreaToSubMeter_GetSubAreaIdBySubMeterId(subMeterId);
                if(!subAreaSubMeterDictionary.ContainsKey(subAreaId))
                {
                    subAreaSubMeterDictionary.Add(subAreaId, new List<long>());
                }
                subAreaSubMeterDictionary[subAreaId].Add(subMeterId);
            }

            return subAreaSubMeterDictionary;
        }

        private bool CommodityMeterMatch(long meterId)
        {
            return CommodityMeterMatch(new List<long>{meterId});
        }

        private bool CommodityMeterMatch(List<long> meterIds)
        {
            //Get commodities linked to meters
            var commodityIds = meterIds.Select(m => meterCommodityDictionary[m]).Distinct().ToList();
            var meterCommodities = commodityIds.Select(c => commodityDictionary[c]).ToList();

            return filterData.Commodities.Any(c => meterCommodities.Contains(c));
        }

        private string GetLiHtml(string type, string guid, string value, string ulHTML)
        {
            var branchDivClass = string.IsNullOrWhiteSpace(ulHTML)
                ? "fa-times-circle"
                : "fa-plus-square show-pointer";

            ulHTML = $"<ul class='format-listitem'>{ulHTML}</ul>";

            var branchListDiv = $"<div id='{type}|{guid}List' class='listitem-hidden'>{ulHTML}</div>";
            var span = $"<span id='{type}|{guid}span'>{value}</span>";
            var icon = $"<i class='fas fa-site' style='padding-left: 3px; padding-right: 3px;'></i>";
            var checkbox = $"<input type='checkbox' id='{type}|{guid}checkbox' onclick='updatePage(this)' GUID='{type}|{guid}' Branch='{type}'></input>";
            var branchDiv = $"<i id='{type}|{guid}' class='far {branchDivClass} expander'></i>";

            return $"<li>{branchDiv}{checkbox}{icon}{span}{branchListDiv}</li>";
        }
    }
}