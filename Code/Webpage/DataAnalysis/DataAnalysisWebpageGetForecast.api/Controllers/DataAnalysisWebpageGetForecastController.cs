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
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.Customer.Asset.Attribute _customerAssetAttributeEnums = new Enums.Customer.Asset.Attribute();
        private readonly Int64 dataAnalysisWebpageGetForecastAPIId;
        private FilterData filterData;
        private Dictionary<long, string> dateDictionary;
        private string granularityCode;
        private long granularityId;

        private class Usage
        {
            public string Date;
            public decimal? Value;
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
            public string CustomerGUID {get; set; }
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
                
                filterData = JsonConvert.DeserializeObject<FilterData>(jsonObject["FilterData"].ToString());

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary()
                    .Where(d => Convert.ToDateTime(d.Key) >= Convert.ToDateTime(filterData.StartDate))
                    .Where(d => Convert.ToDateTime(d.Key) <= Convert.ToDateTime(filterData.EndDate))
                    .OrderBy(d => Convert.ToDateTime(d.Key))
                    .ToDictionary(d => d.Value, d => d.Key);

                //get granularity code
                granularityCode = _informationMethods.GetGranularityCodeByGranularityDisplayDescription(filterData.Granularity);

                var informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(informationGranularityAttributeEnums.GranularityCode);
                granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);

                //get commodity Ids
                var commodityDictionary = filterData.Commodities.ToDictionary(
                    c => _informationMethods.Commodity_GetCommodityIdByCommodityDescription(c),
                    c => c
                );

                //get customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(filterData.CustomerGUID);

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
                            GetUsageAndAddToSeriesList(seriesList, splitByCommodity, locationType, siteName, commodity.Value, meterIdListByCommodity, "Meter");
                        }
                    }
                    else if(locationType == "Area")
                    {
                        var locationGUIDArray = locationGUID.Split('_');
                        var siteId = Convert.ToInt64(locationGUIDArray[0]);
                        var areaId = Convert.ToInt64(locationGUIDArray[1]);

                        //get area description
                        var areaDescription = _informationMethods.Area_GetAreaDescriptionByAreaId(areaId);

                        //get meters by area
                        var areaMeterIdList = _mappingMethods.AreaToMeter_GetMeterIdListByAreaId(areaId).ToList();

                        var siteIdList = siteId == 0
                            ? _mappingMethods.CustomerToSite_GetSiteIdListByCustomerId(customerId) //get all sites for customer
                            : new List<long>{siteId};

                        //get meters for sites
                        var meterIdList = siteIdList.SelectMany(s => _mappingMethods.MeterToSite_GetMeterIdListBySiteId(s)).ToList();
                        areaMeterIdList = areaMeterIdList.Intersect(meterIdList).ToList();

                        var baseSeriesName = siteId == 0
                            ? areaDescription
                            : $"{_customerMethods.GetSiteName(siteId)} - {areaDescription}";

                        foreach(var commodity in commodityDictionary)
                        {
                            //get meters that match commodity
                            var meterIdListByCommodity = areaMeterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodity.Key).ToList();
                            GetUsageAndAddToSeriesList(seriesList, splitByCommodity, locationType, baseSeriesName, commodity.Value, meterIdListByCommodity, "Meter");
                        }
                    }
                    else if(locationType == "Commodity")
                    {
                        var locationGUIDArray = locationGUID.Split('_');
                        var siteId = Convert.ToInt64(locationGUIDArray[0]);
                        var areaId = Convert.ToInt64(locationGUIDArray[1]);
                        var commodityId = Convert.ToInt64(locationGUIDArray[2]);

                        if(siteId == 0)
                        {
                            //get all sites for customer
                            var siteIdList = _mappingMethods.CustomerToSite_GetSiteIdListByCustomerId(customerId);

                            //get meters for sites that match commodity
                            var meterIdList = siteIdList.SelectMany(s => _mappingMethods.MeterToSite_GetMeterIdListBySiteId(s))
                                .Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodityId).ToList();

                            if(areaId == 0)
                            {
                                GetUsageAndAddToSeriesList(seriesList, false, locationType, commodityDictionary[commodityId], commodityDictionary[commodityId], meterIdList, "Meter");
                            }
                            else
                            {
                                //get area description
                                var areaDescription = _informationMethods.Area_GetAreaDescriptionByAreaId(areaId);

                                //get meters by area
                                var areaMeterIdList = _mappingMethods.AreaToMeter_GetMeterIdListByAreaId(areaId).Intersect(meterIdList).ToList();

                                GetUsageAndAddToSeriesList(seriesList, true, locationType, $"{areaDescription}", commodityDictionary[commodityId], areaMeterIdList, "Meter");
                            }
                        }
                        else if(areaId == 0)
                        {
                            //get site name
                            var siteName = _customerMethods.GetSiteName(siteId);

                            //get all meters for site
                            var meterIdList = _mappingMethods.MeterToSite_GetMeterIdListBySiteId(siteId);

                            //get meters that match commodity
                            var meterIdListByCommodity = meterIdList.Where(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m) == commodityId).ToList();
                            GetUsageAndAddToSeriesList(seriesList, true, locationType, siteName, commodityDictionary[commodityId], meterIdListByCommodity, "Meter");
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
                            GetUsageAndAddToSeriesList(seriesList, true, locationType, $"{siteName} - {areaDescription}", commodityDictionary[commodityId], meterIdListByCommodity, "Meter");
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
                        var commodityDescription = commodityDictionary[commodityId];
                        GetUsageAndAddToSeriesList(seriesList, true, locationType, meterIdentifier, commodityDescription, new List<long>{meterId}, "Meter");
                    }
                    else if(locationType == "SubArea")
                    {
                        var locationGUIDArray = locationGUID.Split('_');
                        var siteId = Convert.ToInt64(locationGUIDArray[0]);
                        var areaId = Convert.ToInt64(locationGUIDArray[1]);
                        var commodityId = Convert.ToInt64(locationGUIDArray[2]);
                        var meterId = Convert.ToInt64(locationGUIDArray[3]);
                        var subAreaId = Convert.ToInt64(locationGUIDArray[4]);

                        //get subarea description
                        var subAreaDescription = _informationMethods.SubArea_GetSubAreaDescriptionBySubAreaId(subAreaId);

                        if(meterId == 0)
                        {
                            //find all meters that meet the ids provided
                            var seriesBaseName = string.Empty;

                            if(siteId > 0)
                            {
                                seriesBaseName += $"{_customerMethods.GetSiteName(siteId)} - ";
                            }

                            if(areaId > 0)
                            {
                                seriesBaseName += $"{_informationMethods.Area_GetAreaDescriptionByAreaId(areaId)} - ";
                            }

                            //get meters for sites
                            var siteIdList = siteId == 0
                                ? _mappingMethods.CustomerToSite_GetSiteIdListByCustomerId(customerId)
                                : new List<long>{siteId};

                            var meterIdList = siteIdList.SelectMany(s => _mappingMethods.MeterToSite_GetMeterIdListBySiteId(s)).ToList();

                            //get meters that match areaId
                            var areaMeterIdList = areaId == 0
                                ? meterIdList
                                : _mappingMethods.AreaToMeter_GetMeterIdListByAreaId(areaId).Intersect(meterIdList).ToList();

                            //get meters that match commodityId
                            var commodityMeterIdList = commodityId == 0
                                ? areaMeterIdList
                                : _mappingMethods.CommodityToMeter_GetMeterIdByCommodityId(commodityId).Intersect(areaMeterIdList).ToList();

                            //get submeters from meter list
                            var subMeterIdList = commodityMeterIdList.SelectMany(m => _mappingMethods.MeterToSubMeter_GetSubMeterIdListByMeterId(m)).ToList();

                            //get submeters by subarea
                            var subAreaSubMeterIdList = _mappingMethods.SubAreaToSubMeter_GetSubMeterIdListBySubAreaId(subAreaId).Intersect(subMeterIdList).ToList();

                            //get commodities from meters
                            var meterCommodityIdList = commodityMeterIdList.Select(m => _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(m)).Distinct().ToList();

                            foreach(var meterCommodityId in meterCommodityIdList)
                            {
                                var commodityDescription = commodityDictionary[meterCommodityId];
                                GetUsageAndAddToSeriesList(seriesList, true, locationType, $"{seriesBaseName}{subAreaDescription}", commodityDescription, subAreaSubMeterIdList, "SubMeter");
                            }
                        }
                        else
                        {
                            //get all submeters for meter
                            var subMeterIdList = _mappingMethods.MeterToSubMeter_GetSubMeterIdListByMeterId(meterId).ToList();

                            //get MeterIdentifierMeterAttributeId
                            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

                            //Get Meter identifier
                            var meterIdentifier = _customerMethods.MeterDetail_GetMeterDetailDescriptionByMeterIdAndMeterAttributeId(meterId, meterIdentifierMeterAttributeId);

                            //get submeters by subarea
                            var subAreaSubMeterIdList = _mappingMethods.SubAreaToSubMeter_GetSubMeterIdListBySubAreaId(subAreaId).Intersect(subMeterIdList).ToList();

                            //get commodity for meter
                            if(commodityId == 0)
                            {
                                commodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);
                            }
                            
                            var commodityDescription = commodityDictionary[commodityId];
                            GetUsageAndAddToSeriesList(seriesList, splitByCommodity, locationType, $"{meterIdentifier} - {subAreaDescription}", commodityDescription, subAreaSubMeterIdList, "SubMeter");
                        }
                    }
                    else if(locationType == "Asset")
                    {
                        //get AssetNameAssetAttributeId
                        var assetNameAssetAttributeId = _customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(_customerAssetAttributeEnums.AssetName);

                        //Get asset id
                        var assetId = _customerMethods.Asset_GetAssetIdByAssetGUID(locationGUID);

                        //Get asset name
                        var assetName = _customerMethods.AssetDetail_GetAssetDetailDescriptionByAssetIdAndAssetAttributeId(assetId, assetNameAssetAttributeId);

                        //get submeter for asset
                        var subMeterIdList = _mappingMethods.AssetToSubMeter_GetSubMeterIdListByAssetId(assetId);

                        //get subarea for submeters
                        var subAreaId = _mappingMethods.SubAreaToSubMeter_GetSubAreaIdBySubMeterId(subMeterIdList.First());

                        //get meter for submeter
                        var meterId = _mappingMethods.MeterToSubMeter_GetMeterIdBySubMeterId(subMeterIdList.First());

                        //get subarea description
                        var subAreaDescription = _informationMethods.SubArea_GetSubAreaDescriptionBySubAreaId(subAreaId);

                        //get MeterIdentifierMeterAttributeId
                        var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

                        //Get Meter identifier
                        var meterIdentifier = _customerMethods.MeterDetail_GetMeterDetailDescriptionByMeterIdAndMeterAttributeId(meterId, meterIdentifierMeterAttributeId);

                        //get submeters by asset
                        var assetSubMeterIdList = _mappingMethods.AssetToSubMeter_GetSubMeterIdListByAssetId(assetId).Intersect(subMeterIdList).ToList();

                        //get commodity for meter
                        var commodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);
                        var commodityDescription = commodityDictionary[commodityId];
                        GetUsageAndAddToSeriesList(seriesList, splitByCommodity, locationType, $"{meterIdentifier} - {subAreaDescription} - {assetName}", commodityDescription, assetSubMeterIdList, "SubMeter");
                    }
                    else if(locationType == "SubMeter")
                    {
                        //get SubMeterIdentifierSubMeterAttributeId
                        var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

                        //Get SubMeter Id
                        var subMeterId = _customerMethods.SubMeter_GetSubMeterIdBySubMeterGUID(locationGUID);

                        //Get SubMeter identifier
                        var subMeterIdentifier = _customerMethods.SubMeterDetail_GetSubMeterDetailDescriptionBySubMeterIdAndSubMeterAttributeId(subMeterId, subMeterIdentifierSubMeterAttributeId);

                        //get commodity for subMeter
                        var meterId = _mappingMethods.MeterToSubMeter_GetMeterIdBySubMeterId(subMeterId);
                        var commodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);
                        var commodityDescription = commodityDictionary[commodityId];
                        GetUsageAndAddToSeriesList(seriesList, true, locationType, subMeterIdentifier, commodityDescription, new List<long>{subMeterId}, "SubMeter");
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

        private void GetUsageAndAddToSeriesList(List<Series> seriesList, bool splitByCommodity, string type, string baseName, string commodity, List<long> meterIdList, string meterType)
        {
            if(!meterIdList.Any())
            {
                return;
            }

            //get usage for every meter
            var usageList = meterIdList.SelectMany(m => GetMeterForecast(meterType, m)).ToList();

            if (usageList.Any())
            {
                var series = CreateNewSeries(type, commodity, baseName, splitByCommodity, usageList);
                seriesList.Add(series);
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

        private List<Usage> GetMeterForecast(string meterType, long meterId)
        {
            //Get latest daily forecast
            var forecastDataRows = _supplyMethods.ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);
            var idColumns = forecastDataRows.First().Table.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "Usage").Select(c => c.ColumnName).ToList();

            return idColumns.Count == 1
                ? SetupUsageListFromSingleIdForecast(idColumns.First(), forecastDataRows)
                : SetupUsageListFromDoubleIdForecast(idColumns, forecastDataRows);
        }

        private List<Usage> SetupUsageListFromSingleIdForecast(string idColumn, List<DataRow> forecastDataRows)
        {
            //Granularity is Year or Date
            var forecastTuple = new List<Tuple<long, decimal>>();

            foreach (DataRow r in forecastDataRows)
            {
                var tup = Tuple.Create((long)r[idColumn], (decimal)r["Usage"]);
                forecastTuple.Add(tup);
            }

            if (granularityCode == "Date")
            {
                return CreateUsageListFromSingleIdForecast(forecastTuple, dateDictionary);
            }

            //get date to year mappings
            var dateToYearMapping = _mappingMethods.GetDateToYearDictionary()
                .Where(d => dateDictionary.Keys.Contains(d.Key))
                .ToDictionary(d => d.Key, d => d.Value);

            //get years
            var yearIdList = dateToYearMapping.Values.Distinct().ToList();

            var informationYearMethods = new Methods.Information.Year();
            var yearDictionary = yearIdList.ToDictionary(y => y, y => informationYearMethods.Year_GetYearDescriptionByYearId(y))
                .OrderBy(y => Convert.ToInt64(y.Value))
                .ToDictionary(y => y.Key, y => y.Value);

            return CreateUsageListFromSingleIdForecast(forecastTuple, yearDictionary);
        }

        private static List<Usage> CreateUsageListFromSingleIdForecast(List<Tuple<long, decimal>> forecastTuple, Dictionary<long, string> dictionary)
        {
            //create base usage list
            var usageList = dictionary
                .Select(d => new Usage { Date = d.Value, Value = null, EntityCount = 0 })
                .ToList();

            var forecastTupleInDictionary = forecastTuple.Where(f => dictionary.ContainsKey(f.Item1)).ToList();

            //populate with data from forecast
            foreach (var forecast in forecastTupleInDictionary)
            {
                var usage = usageList.FirstOrDefault(u => u.Date == dictionary[forecast.Item1]);
                usage.EntityCount += 1;
                usage.Value = (usage.Value ?? 0) + forecast.Item2;
            }

            return usageList;
        }

        private List<Usage> SetupUsageListFromDoubleIdForecast(List<string> idColumns, List<DataRow> forecastDataRows)
        {
            var forecastTuple = new List<Tuple<long, long, decimal>>();

            foreach (DataRow r in forecastDataRows)
            {
                var tup = Tuple.Create((long)r[idColumns.First()], (long)r[idColumns.Last()], (decimal)r["Usage"]);
                forecastTuple.Add(tup);
            }

            var dictionary = new Dictionary<long, Dictionary<long, string>>();
            var granularMappingDictionary = new Dictionary<long, List<long>>();
            var granularDictionary = new Dictionary<long, string>();

            if(granularityCode == "FiveMinute"
                || granularityCode == "HalfHour")
            {
                //map date to itself for generic code to work
                var dateToDateMapping = dateDictionary.ToDictionary(d => d.Key, d => d.Key);

                //get time periods
                var timePeriodDictionary = _informationMethods.TimePeriod_GetStartTimeDictionary();

                //Get dates that have additional number of time periods for granularity
                var granularityToTimePeriodNonStandardDateList = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetTupleByGranularityId(granularityId);
                var granularityToTimePeriodStandardDateList = _mappingMethods.GranularityToTimePeriod_StandardDate_GetTimePeriodListByGranularityId(granularityId);

                granularMappingDictionary = dateDictionary.ToDictionary(
                    d => d.Key,
                    d => (granularityToTimePeriodNonStandardDateList.Any(g => g.Item1 == d.Key)
                        ? granularityToTimePeriodNonStandardDateList.Where(g => g.Item1 == d.Key).Select(t => t.Item2).ToList()
                        : granularityToTimePeriodStandardDateList)
                            .OrderBy(g => timePeriodDictionary[g]).ToList()
                );

                granularDictionary = timePeriodDictionary.ToDictionary(
                    t => t.Key,
                    t => $"{t.Value.Hours.ToString().PadLeft(2, '0')}:{t.Value.Minutes.ToString().PadLeft(2, '0')}"
                );

                dictionary = GetDictionary(dateToDateMapping, dateDictionary, granularMappingDictionary, granularDictionary);
            }
            else 
            {
                //get date to year mappings
                var dateToYearMapping = _mappingMethods.GetDateToYearDictionary()
                    .Where(d => dateDictionary.Keys.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                //get years
                var yearIdList = dateToYearMapping.Values.Distinct().ToList();

                var informationYearMethods = new Methods.Information.Year();
                var yearDictionary = yearIdList.ToDictionary(
                    y => y,
                    y => informationYearMethods.Year_GetYearDescriptionByYearId(y)
                );

                if(granularityCode == "Week")
                {
                    //get date to week mappings
                    granularMappingDictionary = _mappingMethods.GetDateToWeekDictionary()
                        .Where(d => dateDictionary.Keys.Contains(d.Key))
                        .ToDictionary(d => d.Key, d => new List<long>{d.Value});

                    //get weeks
                    var weekIdList = granularMappingDictionary.Values.SelectMany(v => v).Distinct().ToList();

                    var informationWeekMethods = new Methods.Information.Week();
                    granularDictionary = weekIdList.ToDictionary(
                        w => w,
                        w => informationWeekMethods.Week_GetWeekDescriptionByWeekId(w)
                    );
                }
                else if(granularityCode == "Month")
                {
                    //get date to month mappings
                    granularMappingDictionary = _mappingMethods.GetDateToMonthDictionary()
                        .Where(d => dateDictionary.Keys.Contains(d.Key))
                        .ToDictionary(d => d.Key, d => new List<long>{d.Value});

                    //get months
                    var monthIdList = granularMappingDictionary.Values.SelectMany(v => v).Distinct().ToList();

                    var informationMonthMethods = new Methods.Information.Month();
                    granularDictionary = monthIdList.ToDictionary(
                        m => m,
                        m => informationMonthMethods.Month_GetMonthDescriptionByMonthId(m)
                    );
                }
                else if(granularityCode == "Quarter")
                {
                    //get date to quarter mappings
                    granularMappingDictionary = _mappingMethods.GetDateToQuarterDictionary()
                        .Where(d => dateDictionary.Keys.Contains(d.Key))
                        .ToDictionary(d => d.Key, d => new List<long>{d.Value});

                    //get quarters
                    var quarterIdList = granularMappingDictionary.Values.SelectMany(v => v).Distinct().ToList();

                    var informationQuarterMethods = new Methods.Information.Quarter();
                    granularDictionary = quarterIdList.ToDictionary(
                        q => q,
                        q => informationQuarterMethods.Quarter_GetQuarterDescriptionByQuarterId(q)
                    );
                }

                dictionary = GetDictionary(dateToYearMapping, yearDictionary, granularMappingDictionary, granularDictionary);
            }

            return CreateUsageListFromDoubleIdForecast(forecastTuple, dictionary);
        }

        private Dictionary<long, Dictionary<long, string>> GetDictionary(Dictionary<long, long> baseMapping, Dictionary<long, string> baseDictionary, Dictionary<long, List<long>> subMapping, Dictionary<long, string> subDictionary)
        {
            var dictionary = new Dictionary<long, Dictionary<long, string>>();
            foreach (var date in dateDictionary)
            {
                var baseId = baseMapping[date.Key];
                var baseDescription = baseDictionary[baseId];

                if (!dictionary.ContainsKey(baseId))
                {
                    dictionary.Add(baseId, new Dictionary<long, string>());
                }

                foreach(var subId in subMapping[date.Key])
                {
                    if (!dictionary[baseId].ContainsKey(subId))
                    {
                        var subDescription = subDictionary[subId];
                        dictionary[baseId].Add(subId, $"{baseDescription}-{subDescription}");
                    }
                }
            }

            return dictionary;
        }

        private static List<Usage> CreateUsageListFromDoubleIdForecast(List<Tuple<long, long, decimal>> forecastTuple, Dictionary<long, Dictionary<long, string>> dictionary)
        {
            //create base usage list
            var usageList = (from baseDictionary in dictionary
                             from subDictionary in baseDictionary.Value
                             let usage = new Usage { Date = subDictionary.Value, Value = null, EntityCount = 0 }
                             select usage).ToList();

            var forecastTupleInDictionary = forecastTuple.Where(f => dictionary.ContainsKey(f.Item1) && dictionary[f.Item1].ContainsKey(f.Item2)).ToList();

            //populate with data from forecast
            foreach (var forecast in forecastTupleInDictionary)
            {
                var usage = usageList.FirstOrDefault(u => u.Date == dictionary[forecast.Item1][forecast.Item2]);
                usage.EntityCount += 1;
                usage.Value = (usage.Value ?? 0) + forecast.Item3;
            }

            return usageList;
        }
    }
}