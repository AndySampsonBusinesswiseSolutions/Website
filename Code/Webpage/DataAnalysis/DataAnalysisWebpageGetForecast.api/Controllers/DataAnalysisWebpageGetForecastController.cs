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
        }

        private class Series
        {
            public string SeriesName;
            public string Type;
            public string Commodity;
            public List<Usage> Usage;
        }

        private class Forecast
        {
            public List<Series> Meters;
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
                            var series = new Series();
                            series.Type = locationType;
                            series.Commodity = commodity.Value;
                            series.Usage = new List<Usage>();

                            var seriesCommodity = filterData.Commodities.Count == 1
                                ? string.Empty
                                : $" - {commodity.Value}";

                            series.SeriesName = $"{siteName}{seriesCommodity}";

                            //get meters that match commodity
                            var meterIdListByCommodity = meterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodity.Key).ToList();

                            //get usage for every meter
                            var usageList = new List<Usage>();
                            foreach(var meterId in meterIdListByCommodity)
                            {
                                usageList.AddRange(GetMeterForecast(meterId));
                            }

                            foreach(var usage in usageList)
                            {
                                var seriesUsage = series.Usage.FirstOrDefault(s => s.Date == usage.Date);
                                if(seriesUsage == null)
                                {
                                    series.Usage.Add(usage);
                                }
                                else
                                {
                                    seriesUsage.Value += usage.Value;
                                }
                            }

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
                    var groupedSeriesList = new List<Series>();

                    var locationGroups = seriesList.GroupBy(s => new { s.Type, s.Commodity }).ToList();
                    foreach(var locationGroup in locationGroups)
                    {
                        var series = new Series();
                        series.Type = locationGroup.Key.Type;
                        series.Commodity = locationGroup.Key.Commodity;
                        series.Usage = new List<Usage>();

                        var seriesCommodity = filterData.Commodities.Count == 1
                                ? string.Empty
                                : $" - {series.Commodity}";
                            
                        series.SeriesName = $"{series.Type}{seriesCommodity} - Sum";

                        foreach(var usage in locationGroup.SelectMany(s => s.Usage).ToList())
                        {
                            var seriesUsage = series.Usage.FirstOrDefault(s => s.Date == usage.Date);
                            if(seriesUsage == null)
                            {
                                series.Usage.Add(usage);
                            }
                            else
                            {
                                seriesUsage.Value += usage.Value;
                            }
                        }

                        groupedSeriesList.Add(series);
                    }



                    forecast.Meters = groupedSeriesList;
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
            .Select(f => new Usage{Date = f.Key, Value = Convert.ToDecimal(f.Value)})
            .ToList();
        }
    }
}

