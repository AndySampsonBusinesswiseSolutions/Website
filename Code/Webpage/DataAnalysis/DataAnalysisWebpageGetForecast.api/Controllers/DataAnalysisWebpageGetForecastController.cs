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

namespace DataAnalysisWebpageGetForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DataAnalysisWebpageGetForecastController : ControllerBase
    {
        private readonly ILogger<DataAnalysisWebpageGetForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.Page.GUID _systemPageGUIDEnums = new Enums.System.Page.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private readonly Int64 dataAnalysisWebpageGetForecastAPIId;
        private FilterData filterData;
        private Dictionary<string, long> dateDictionary;

        private class Usage
        {
            public string Date;
            public decimal Value;
            public long EntityCount;
        }

        private class Series
        {
            public string SeriesName;
            public string Type;
            public string Commodity;
            public List<Usage> Usage = new List<Usage>();
        }

        private class Forecast
        {
            public List<Series> Meters = new List<Series>();
        }

        public class FilterData    {
            public string EnergyUnit { get; set; } 
            public string EnergyUnitInstance { get; set; } 
            public string StartDate { get; set; } 
            public string EndDate { get; set; } 
            public string Granularity { get; set; } 
            public bool LatestCreated { get; set; } 
            public string CreatedDate { get; set; } 
            public List<string> Locations { get; set; }
            public List<string> Grouping { get; set; }
            public List<string> Commodities { get; set; }
        }

        public DataAnalysisWebpageGetForecastController(ILogger<DataAnalysisWebpageGetForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.DataAnalysisWebpageGetForecastAPI, _systemAPIPasswordEnums.DataAnalysisWebpageGetForecastAPI);
            dataAnalysisWebpageGetForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.DataAnalysisWebpageGetForecastAPI);
        }

        [HttpPost]
        [Route("DataAnalysisWebpageGetForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(dataAnalysisWebpageGetForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DataAnalysisWebpageGetForecast/GetForecast")]
        public void GetForecast([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            filterData = JsonConvert.DeserializeObject<FilterData>(jsonObject["FilterData"].ToString()); 

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    dataAnalysisWebpageGetForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, dataAnalysisWebpageGetForecastAPIId);

                //Get Page Id
                var pageId = _systemMethods.Page_GetPageIdByGUID(_systemPageGUIDEnums.DataAnalysis);

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary()
                    .Where(d => Convert.ToDateTime(d.Key) >= Convert.ToDateTime(filterData.StartDate))
                    .Where(d => Convert.ToDateTime(d.Key) <= Convert.ToDateTime(filterData.EndDate))
                    .ToDictionary(d => d.Key, d => d.Value);

                //Get SiteNameSiteAttributeId
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);

                //Get SitePostcodeSiteAttributeId
                var SitePostcodeSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode);

                //get granularity code
                var granularityCode = _informationMethods.GetGranularityCodeByGranularityDisplayDescription(filterData.Granularity);

                //get commodity Ids
                var commodityDictionary = filterData.Commodities.ToDictionary(
                    c => _informationMethods.Commodity_GetCommodityIdByCommodityDescription(c),
                    c => c
                );

                var forecast = new Forecast();
                var seriesList = new List<Series>();
                var splitByCommodity = filterData.Commodities.Count != 1;

                foreach(var location in filterData.Locations)
                {
                    //split location by | into type and guid
                    var locationType = location.Split('|')[0];
                    var locationGUID = location.Split('|')[1].ToUpper();

                    if(locationType == "Site")
                    {
                        var siteId = _customerMethods.Site_GetSiteIdBySiteGUID(locationGUID);

                        //get site name
                        var siteName = _customerMethods.GetSiteName(siteId);

                        //get all meters for site
                        var meterIdList = _mappingMethods.MeterToSite_GetMeterIdListBySiteId(siteId);

                        foreach(var commodity in commodityDictionary)
                        {
                            //get meters that match commodity
                            var meterIdListByCommodity = meterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodity.Key).ToList();

                            //get usage for every meter
                            var usageList = meterIdListByCommodity.SelectMany(m => GetMeterForecast(m)).ToList();
                            var series = CreateNewSeries(locationType, commodity.Value, siteName, splitByCommodity, usageList);

                            if(series.Usage.Any())
                            {
                                seriesList.Add(series);
                            }
                        }
                    }
                    else if(locationType == "Area")
                    {
                        var siteId = Convert.ToInt64(locationGUID.Split('_')[0]);
                        var areaId = Convert.ToInt64(locationGUID.Split('_')[1]);

                        //get area description
                        var areaDescription = _informationMethods.Area_GetAreaDescriptionByAreaId(areaId);

                        if(siteId == 0)
                        {
                            //TODO: do something.....don't know what yet
                        }
                        else
                        {
                            //get all meters for site
                            var meterIdList = _mappingMethods.MeterToSite_GetMeterIdListBySiteId(siteId);

                            //get site name
                            var siteName = _customerMethods.GetSiteName(siteId);

                            //get meters by area
                            var areaMeterIdList = _mappingMethods.AreaToMeter_GetMeterIdListByAreaId(areaId).Intersect(meterIdList).ToList();

                            foreach(var commodity in commodityDictionary)
                            {
                                //get meters that match commodity
                                var meterIdListByCommodity = areaMeterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodity.Key).ToList();

                                //get usage for every meter
                                var usageList = meterIdListByCommodity.SelectMany(m => GetMeterForecast(m)).ToList();
                                var series = CreateNewSeries(locationType, commodity.Value, $"{siteName} - {areaDescription}", splitByCommodity, usageList);

                                if(series.Usage.Any())
                                {
                                    seriesList.Add(series);
                                }
                            }
                        }
                    }
                    else if(locationType == "Commodity")
                    {
                        var siteId = Convert.ToInt64(locationGUID.Split('_')[0]);
                        var areaId = Convert.ToInt64(locationGUID.Split('_')[1]);
                        var commodityId = Convert.ToInt64(locationGUID.Split('_')[2]);

                        //get commodity description
                        var commodityDescription = _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);

                        if(siteId == 0)
                        {
                            //TODO: do something.....don't know what yet
                        }
                        else if(areaId == 0)
                        {
                            //TODO: do something.....don't know what yet
                        }
                        else
                        {
                            //get all meters for site
                            var meterIdList = _mappingMethods.MeterToSite_GetMeterIdListBySiteId(siteId);

                            //get site name
                            var siteName = _customerMethods.GetSiteName(siteId);

                            //get area description
                            var areaDescription = _informationMethods.Area_GetAreaDescriptionByAreaId(areaId);

                            //get meters by area
                            var areaMeterIdList = _mappingMethods.AreaToMeter_GetMeterIdListByAreaId(areaId).Intersect(meterIdList).ToList();

                            //get meters that match commodity
                            var meterIdListByCommodity = areaMeterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodityId).ToList();

                            //get usage for every meter
                            var usageList = meterIdListByCommodity.SelectMany(m => GetMeterForecast(m)).ToList();
                            var series = CreateNewSeries(locationType, commodityDescription, $"{siteName} - {areaDescription}", true, usageList);

                            if(series.Usage.Any())
                            {
                                seriesList.Add(series);
                            }
                        }
                    }
                    else if(locationType == "Meter")
                    {
                        //get MeterIdentifierMeterAttributeId
                        var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

                        //Get Meter Id
                        var meterId = _customerMethods.Meter_GetMeterIdByMeterGUID(locationGUID);

                        //Get Meter identifier
                        var meterIdentifier = _customerMethods.MeterDetail_GetMeterDetailDescriptionByMeterIdAndMeterAttributeId(meterId, meterIdentifierMeterAttributeId);

                        //get commodity for meter
                        var commodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);
                        var commodityDescription = _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);

                        //get usage for meter
                        var usageList = GetMeterForecast(meterId);
                        var series = CreateNewSeries(locationType, commodityDescription, meterIdentifier, true, usageList);

                        if(series.Usage.Any())
                        {
                            seriesList.Add(series);
                        }
                    }
                }

                if(filterData.Grouping.First() == "No Grouping")
                {
                    forecast.Meters = seriesList;
                }
                else
                {
                    var sumSeriesList = new List<Series>();

                    var locationGroups = seriesList.GroupBy(s => new { s.Type, s.Commodity }).ToList();
                    foreach(var locationGroup in locationGroups)
                    {
                        var series = CreateNewSeries(locationGroup.Key.Type, locationGroup.Key.Commodity, locationGroup.Key.Type, true, locationGroup.SelectMany(s => s.Usage).ToList());                            
                        series.SeriesName += " - Sum";

                        sumSeriesList.Add(series);
                    }

                    if(filterData.Grouping.Contains("Sum"))
                    {
                        forecast.Meters.AddRange(sumSeriesList);
                    }

                    if(filterData.Grouping.Contains("Average"))
                    {
                        var averageSeriesList = new List<Series>();

                        foreach(var locationGroup in locationGroups)
                        {
                            var sumSeries = sumSeriesList.First(s => s.Type == locationGroup.Key.Type && s.Commodity == locationGroup.Key.Commodity);
                            var averageSeries = CreateNewSeries(locationGroup.Key.Type, locationGroup.Key.Commodity, sumSeries.SeriesName.Replace(" - Sum", " - Average"), false, sumSeries.Usage);

                            foreach(var averageUsage in averageSeries.Usage)
                            {
                                averageUsage.Value = averageUsage.Value / averageUsage.EntityCount;
                            }

                            averageSeriesList.Add(averageSeries);
                        }

                        forecast.Meters.AddRange(averageSeriesList);
                    }
                }

                //Write HTML to System.PageRequest
                _systemMethods.PageRequest_Insert(createdByUserId, sourceId, pageId, processQueueGUID, JsonConvert.SerializeObject(forecast));

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, dataAnalysisWebpageGetForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, dataAnalysisWebpageGetForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private Series CreateNewSeries(string type, string commodity, string baseName, bool splitByCommodity, List<Usage> usages)
        {
            var series = new Series();
            series.Type = type;
            series.Commodity = commodity;
            AddUsage(series, usages);

            var seriesCommodity = splitByCommodity
                ? $" - {commodity}"
                : string.Empty;

            series.SeriesName = $"{baseName}{seriesCommodity}";

            return series;
        }

        private void AddUsage(Series series, List<Usage> usages)
        {
            foreach(var usage in usages)
            {
                var seriesUsage = series.Usage.FirstOrDefault(s => s.Date == usage.Date);
                if(seriesUsage == null)
                {
                    series.Usage.Add(usage);
                }
                else
                {
                    seriesUsage.Value += usage.Value;
                    seriesUsage.EntityCount += usage.EntityCount;
                }
            }
        }

        private List<Usage> GetMeterForecast(long meterId)
        {
            //Get latest daily forecast
            var forecastDataRows = _supplyMethods.ForecastUsageGranularityLatest_GetLatest("Meter", meterId, "Date");
            var forecastTuple = new List<Tuple<long, decimal>>();

            foreach (DataRow r in forecastDataRows)
            {
                var tup = Tuple.Create((long)r["DateId"], (decimal)r["Usage"]);
                forecastTuple.Add(tup);
            }

            var forecastTupleInDateDictionary = forecastTuple.Where(f => dateDictionary.Any(d => d.Value == f.Item1)).ToList();

            return forecastTupleInDateDictionary.ToDictionary(
                f => dateDictionary.First(d => d.Value == f.Item1).Key,
                f => f.Item2.ToString()
            )
            .OrderBy(f => Convert.ToDateTime(f.Key))
            .ToDictionary(f => f.Key, f => f.Value)
            .Select(f => new Usage{Date = f.Key, Value = Convert.ToDecimal(f.Value), EntityCount = 1})
            .ToList();
        }
    }
}