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

namespace CreateDataAnalysisWebpage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDataAnalysisWebpageController : ControllerBase
    {
        private readonly ILogger<CreateDataAnalysisWebpageController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Int64 createDataAnalysisWebpageAPIId;
        private string HTML = string.Empty;
        private JObject jsonObject;
        private Dictionary<string, long> attributeDictionary = new Dictionary<string, long>();
        private FilterData filterData;

        private class Usage
        {
            public string Date;
            public string Value;
        }

        private class Meter
        {
            public string SeriesName;
            public List<Usage> Usage;
        }

        private class Forecast
        {
            public List<Meter> Meters;
        }

        public class FilterData    {
            public bool SiteChecked { get; set; } 
            public bool AreaChecked { get; set; } 
            public bool CommodityChecked { get; set; } 
            public bool MeterChecked { get; set; } 
            public bool SubAreaChecked { get; set; } 
            public bool AssetChecked { get; set; } 
            public bool SubMeterChecked { get; set; } 
            public List<string> Commodities { get; set; } 
        }

        public CreateDataAnalysisWebpageController(ILogger<CreateDataAnalysisWebpageController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateDataAnalysisWebpageAPI, _systemAPIPasswordEnums.CreateDataAnalysisWebpageAPI);
            createDataAnalysisWebpageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateDataAnalysisWebpageAPI);
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createDataAnalysisWebpageAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/BuildLocationTree")]
        public IActionResult BuildLocationTree([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDataAnalysisWebpageAPIId, false, null);

                return new OkObjectResult(new { message = baseUl });
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDataAnalysisWebpageAPIId, true, $"System Error Id {errorId}");

                return new OkObjectResult(new { message = string.Empty });
            }
        }

        private void GetRequiredAttributes()
        {
            //Get SiteNameSiteAttributeId
            attributeDictionary.Add("SiteName", _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName));

            //Get SitePostcodeSiteAttributeId
            attributeDictionary.Add("SitePostcode", _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode));

            //Get MeterIdentifierMeterAttributeId
            attributeDictionary.Add("MeterIdentifier", _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier));
        }

        private void BuildSiteBranch(List<long> siteIds)
        {
            var siteMeterDictionary = siteIds.ToDictionary(s => s, s => _mappingMethods.MeterToSite_GetMeterIdListBySiteId(s));
            var commodityMatchingSiteDictionary = siteMeterDictionary.Where(s => CommodityMeterMatch(s.Value)).ToDictionary(s => s.Key, s => s.Value.Where(m => CommodityMeterMatch(m)).ToList());

            if(filterData.SiteChecked)
            {
                foreach(var site in commodityMatchingSiteDictionary)
                {
                    //Get Site GUID
                    var siteGUID = _customerMethods.Site_GetSiteGUIDBySiteId(site.Key).ToString();

                    //Get Site name
                    var siteName = GetSiteName(site.Key);

                    //Get areas linked to site
                    var areaMeterDictionary = BuildAreaMeterDictionary(site.Value);

                    //Finish html
                    var areaHTML = BuildAreaBranch(areaMeterDictionary);
                    var siteHTML = GetLiHtml("Site", siteGUID, siteName, areaHTML);
                    HTML += siteHTML;
                }
            }
            else
            {
                var meterIds = commodityMatchingSiteDictionary.SelectMany(s => s.Value).ToList();
                var areaMeterDictionary = BuildAreaMeterDictionary(meterIds);
                HTML = BuildAreaBranch(areaMeterDictionary);
            }
        }

        private string BuildAreaBranch(Dictionary<long, List<long>> areaMeterDictionary)
        {
            if(filterData.AreaChecked)
            {
                var areaHTML = string.Empty;

                foreach(var areaMeter in areaMeterDictionary)
                {
                    //Get Area description
                    var areaDescription = _informationMethods.Area_GetAreaDescriptionByAreaId(areaMeter.Key);

                    var commodityHTML = BuildCommodityBranch(areaMeter.Value);
                    areaHTML += GetLiHtml("Area", areaMeter.Key.ToString(), areaDescription, commodityHTML);
                }

                return areaHTML;
            }
            else
            {
                var meterIds = areaMeterDictionary.SelectMany(s => s.Value).ToList();
                return BuildCommodityBranch(meterIds);
            }
        }

        private string BuildCommodityBranch(List<long> meterIds)
        {
            if(filterData.CommodityChecked)
            {
                var commodityHTML = string.Empty;

                //Get commodities linked to meters
                var commodityIds = meterIds.Select(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m)).Distinct().ToList();

                foreach(var commodityId in commodityIds)
                {
                    //Get Commodity description
                    var commodity = _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);
                    var commodityMeterIds = meterIds.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodityId).ToList();

                    var meterHTML = BuildMeterBranch(commodityMeterIds);
                    commodityHTML += GetLiHtml("Commodity", commodityId.ToString(), commodity, meterHTML);
                }

                return commodityHTML;
            }
            else
            {
                return BuildMeterBranch(meterIds);
            }
        }

        private string BuildMeterBranch(List<long> meterIds)
        {
            if(filterData.MeterChecked)
            {
                var meterHTML = string.Empty;

                foreach(var meterId in meterIds)
                {
                    //Get Meter GUID
                    var meterGUID = _customerMethods.Meter_GetMeterGUIDByMeterId(meterId).ToString();

                    //Get Meter identifier
                    var meterIdentifier = _customerMethods.MeterDetail_GetMeterDetailDescriptionByMeterIdAndMeterAttributeId(meterId, attributeDictionary["MeterIdentifier"]);

                    meterHTML += GetLiHtml("Meter", meterGUID, meterIdentifier, string.Empty);
                }

                return meterHTML;
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

        private bool CommodityMeterMatch(long meterId)
        {
            return CommodityMeterMatch(new List<long>{meterId});
        }

        private bool CommodityMeterMatch(List<long> meterIds)
        {
            //Get commodities linked to meters
            var commodityIds = meterIds.Select(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m)).Distinct().ToList();
            var meterCommodities = commodityIds.Select(c => _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(c)).ToList();

            return filterData.Commodities.Any(c => meterCommodities.Contains(c));
        }

        private string GetSiteName(long siteId)
        {
            //Get Site name
            var siteName = _customerMethods.SiteDetail_GetSiteDetailDescriptionBySiteIdAndSiteAttributeId(siteId, attributeDictionary["SiteName"]);

            //Get Site postcode
            var sitePostcode = _customerMethods.SiteDetail_GetSiteDetailDescriptionBySiteIdAndSiteAttributeId(siteId, attributeDictionary["SitePostcode"]);

            return $"{siteName}, {sitePostcode}";
        }

        private string GetLiHtml(string type, string guid, string value, string ulHTML)
        {
            ulHTML = $"<ul class='format-listitem'>{ulHTML}</ul>";

            var branchListDiv = $"<div id='{type}|{guid}List' class='listitem-hidden'>{ulHTML}</div>";
            var span = $"<span id='{type}|{guid}span'>{value}</span>";
            var icon = $"<i class='fas fa-site' style='padding-left: 3px; padding-right: 3px;'></i>";
            var checkbox = $"<input type='checkbox' id='{type}|{guid}checkbox' GUID='{type}|{guid}' Branch='{type}' onclick='updatePage(this);'></input>";
            var branchDiv = $"<i id='{type}|{guid}' class='far fa-plus-square show-pointer expander'></i>";

            return $"<li>{branchDiv}{checkbox}{icon}{span}{branchListDiv}</li>";
        }
        
        [HttpPost]
        [Route("CreateDataAnalysisWebpage/GetDailyForecast")]
        public IActionResult GetDailyForecast([FromBody] object data) //TODO: Build into new API
        {
            //Get Date dictionary
            var dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

            //Get MeterIds
            var meterIdList = _customerMethods.Meter_GetMeterIdList();

            //Get MeterIdentifierMeterAttributeId
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get Meter identifiers
            var meterIdentifierDictionary = _customerMethods.MeterDetail_GetMeterDetailDescriptionDictionaryByMeterAttributeId(meterIdentifierMeterAttributeId);

            var forecast = new Forecast();
            var meterList = new List<Meter>();

            foreach(var meterId in meterIdList)
            {
                var meter = new Meter();
                meter.SeriesName = meterIdentifierDictionary[meterId];
                meter.Usage = new List<Usage>();

                //Get latest daily forecast
                var forecastDataRows = _supplyMethods.ForecastUsageGranularityLatest_GetLatest("Meter", meterId, "Date");
                var forecastTuple = new List<Tuple<long, decimal>>();

                foreach (DataRow r in forecastDataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (decimal)r["Usage"]);
                    forecastTuple.Add(tup);
                }

                var meterForecastList = forecastTuple.ToDictionary(
                    f => dateDictionary.First(d => d.Value == f.Item1).Key,
                    f => f.Item2.ToString()
                )
                .OrderBy(f => Convert.ToDateTime(f.Key))
                .ToDictionary(f => f.Key, f => f.Value)
                .Select(f => new Usage{Date = f.Key, Value = f.Value})
                .ToList();

                meter.Usage.AddRange(meterForecastList);
                meterList.Add(meter);
            }

            forecast.Meters = meterList;

            return new OkObjectResult(new { message = JsonConvert.SerializeObject(forecast) });
        }
    }
}