using System;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;

namespace DataAnalysisWebpageGetForecastApp
{
    class Program
    {
        private static Methods.SystemSchema _systemMethods = new Methods.SystemSchema();
        private static Methods.InformationSchema _informationMethods = new Methods.InformationSchema();
        private static Methods.CustomerSchema _customerMethods = new Methods.CustomerSchema();
        private static Methods.SupplySchema _supplyMethods = new Methods.SupplySchema();
        private static Methods.MappingSchema _mappingMethods = new Methods.MappingSchema();
        private static FilterData filterData;
        private static Dictionary<long, string> dateDictionary;
        private static string granularityCode;
        private static long granularityId;
        private static ConcurrentDictionary<Tuple<string, long>, List<Usage>> forecastDictionary = new ConcurrentDictionary<Tuple<string, long>, List<Usage>>();
        private static ParallelOptions parallelOptions = new ParallelOptions {MaxDegreeOfParallelism = 5};

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

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "nJbgPkmV6JFh26GS";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().DataAnalysisWebpageGetForecastAPI, password);
                var dataAnalysisWebpageGetForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().DataAnalysisWebpageGetForecastAPI);

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    dataAnalysisWebpageGetForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, dataAnalysisWebpageGetForecastAPIId);

                //Get Page Id
                var pageId = _systemMethods.Page_GetPageIdByGUID(new Enums.SystemSchema.Page.GUID().DataAnalysis);
                
                filterData = JsonConvert.DeserializeObject<FilterData>(jsonObject["FilterData"].ToString());

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary()
                    .Where(d => Convert.ToDateTime(d.Key) >= Convert.ToDateTime(filterData.StartDate))
                    .Where(d => Convert.ToDateTime(d.Key) <= Convert.ToDateTime(filterData.EndDate))
                    .OrderBy(d => Convert.ToDateTime(d.Key))
                    .ToDictionary(d => d.Value, d => d.Key);

                //get granularity code
                granularityCode = _informationMethods.GetGranularityCodeByGranularityDisplayDescription(filterData.Granularity);

                var informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(informationGranularityAttributeEnums.GranularityCode);
                granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);

                //get commodity Ids
                var commodityDictionary = filterData.Commodities.ToDictionary(
                    c => _informationMethods.Commodity_GetCommodityIdByCommodityDescription(c),
                    c => c
                );

                //get customer Id
                var customerId = _customerMethods.Customer_GetCustomerIdByCustomerGUID(filterData.CustomerGUID);

                //get MeterIdentifierMeterAttributeId
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);

                //get AssetNameAssetAttributeId
                var assetNameAssetAttributeId = _customerMethods.AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(new Enums.CustomerSchema.Asset.Attribute().AssetName);

                //get SubMeterIdentifierSubMeterAttributeId
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);

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
                        var meterIdList = _mappingMethods.MeterToSite_GetMeterIdListBySiteId(siteId).Distinct().ToList();

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
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //_systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, dataAnalysisWebpageGetForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetUsageAndAddToSeriesList(List<Series> seriesList, bool splitByCommodity, string type, string baseName, string commodity, List<long> meterIdList, string meterType)
        {
            if(!meterIdList.Any())
            {
                return;
            }

            //get usage for every meter
            //Because there's a possibility that we want the same meters multiple times
                //the first time we see a meterType/meterId combination, it is stored
                //in the forecastDictionary. Then, if we need that combination again,
                //it can be retrieved from the dictionary instead of querying the database
            var usageList = new ConcurrentBag<Usage>();

            Parallel.ForEach(meterIdList, parallelOptions, meterId => {
                var meterTypeMeterIdTuple = Tuple.Create(meterType, meterId);
                if(!forecastDictionary.ContainsKey(meterTypeMeterIdTuple))
                {
                    var usages = GetMeterForecast(meterType, meterId).ToList();
                    forecastDictionary.TryAdd(meterTypeMeterIdTuple, usages);
                }

                foreach(var usage in forecastDictionary[meterTypeMeterIdTuple])
                {
                    usageList.Add(usage);
                }
            });

            if (usageList.Any())
            {
                var series = CreateNewSeries(type, commodity, baseName, splitByCommodity, usageList.ToList());
                seriesList.Add(series);
            }
        }

        private static Series CreateNewSeries(string type, string commodity, string baseName, bool splitByCommodity, List<Usage> usages)
        {
            var series = new Series();
            series.Type = type;
            series.Commodity = commodity;
            series.Usage = AddUsage(usages);

            var seriesCommodity = splitByCommodity
                ? $" - {commodity}"
                : string.Empty;

            series.SeriesName = $"{baseName}{seriesCommodity}";

            return series;
        }

        private static List<Usage> AddUsage(List<Usage> usages)
        {
            var dateList = usages.Select(u => u.Date).Distinct().ToList();

            if(dateList.Count() == usages.Count())
            {
                return usages;
            }

            var usageGroups = usages.OrderBy(u => granularityCode == "Month" ? ConvertMonth(u.Date) : u.Date).GroupBy(u => u.Date).ToList();

            return (from date in usageGroups
                             let usage = new Usage { 
                                 Date = date.Key, 
                                 Value = date.Select(s => s.Value).Sum(), 
                                 EntityCount = date.Select(u => u.EntityCount).Sum() }
                             select usage).ToList();
        }

        private static List<Usage> GetMeterForecast(string meterType, long meterId)
        {
            //Get latest daily forecast
            var forecastDataRows = _supplyMethods.ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);
            var idColumns = forecastDataRows.First().Table.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "Usage").Select(c => c.ColumnName).ToList();

            return idColumns.Count == 1
                ? SetupUsageListFromSingleIdForecast(idColumns.First(), forecastDataRows)
                : SetupUsageListFromDoubleIdForecast(idColumns, forecastDataRows);
        }

        private static List<Usage> SetupUsageListFromSingleIdForecast(string idColumn, List<DataRow> forecastDataRows)
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
            var dateToYearMapping = GetGranularMappingDictionary(granularityCode).ToDictionary(d => d.Key, d => d.Value.First());

            //get years
            var yearDictionary = GetGranularDictionary(dateToYearMapping.Values.Distinct().ToList(), granularityCode);

            return CreateUsageListFromSingleIdForecast(forecastTuple, yearDictionary);
        }

        private static List<Usage> CreateUsageListFromSingleIdForecast(List<Tuple<long, decimal>> forecastTuple, Dictionary<long, string> dictionary)
        {
            var forecastTupleInDictionary = forecastTuple.Where(f => dictionary.ContainsKey(f.Item1)).ToList();

            var dictionaryInForecastTuple = dictionary.Where(d => forecastTuple.Any(f => f.Item1 == d.Key))
                .ToDictionary(d => d.Key, d => d.Value);
            var dictionaryNotInForecastTuple = dictionary.Where(d => !forecastTuple.Any(f => f.Item1 == d.Key))
                .ToDictionary(d => d.Key, d => d.Value);

            //create base usage list
            var dictionaryNotInForecastTupleUsageList = dictionaryNotInForecastTuple
                .Select(d => new Usage { Date = d.Value, Value = null, EntityCount = 0 })
                .ToList();

            var dictionaryInForecastTupleUsageList = dictionaryInForecastTuple
                .Select(d => new Usage { Date = d.Value, Value = forecastTupleInDictionary.Where(f => f.Item1 == d.Key).Sum(f => f.Item2), EntityCount = forecastTupleInDictionary.Count(f => f.Item1 == d.Key) })
                .ToList();

            var usageList = dictionaryNotInForecastTupleUsageList.Union(dictionaryInForecastTupleUsageList).OrderBy(u => u.Date).ToList();

            return usageList;
        }

        private static List<Usage> SetupUsageListFromDoubleIdForecast(List<string> idColumns, List<DataRow> forecastDataRows)
        {
            var forecastTuple = new List<Tuple<long, long, decimal>>();

            foreach (DataRow r in forecastDataRows)
            {
                var tup = Tuple.Create((long)r[idColumns.First()], (long)r[idColumns.Last()], (decimal)r["Usage"]);
                forecastTuple.Add(tup);
            }

            var granularMappingDictionary = new Dictionary<long, List<long>>();
            var granularDictionary = new Dictionary<long, string>();
            var baseMapping = new Dictionary<long, long>();
            var baseDictionary = dateDictionary.ToDictionary(d => d.Key, d => d.Value);

            if(granularityCode == "FiveMinute"
                || granularityCode == "HalfHour")
            {
                //map date to itself for generic code to work
                baseMapping = baseDictionary.ToDictionary(d => d.Key, d => d.Key);

                //get time periods
                var timePeriodDictionary = _informationMethods.TimePeriod_GetStartTimeDictionary();

                //Get dates that have additional number of time periods for granularity
                var granularityToTimePeriodNonStandardDateList = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetTupleByGranularityId(granularityId);
                var granularityToTimePeriodStandardDateList = _mappingMethods.GranularityToTimePeriod_StandardDate_GetTimePeriodListByGranularityId(granularityId);

                granularMappingDictionary = baseDictionary.ToDictionary(
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
            }
            else 
            {
                //get date to year mappings
                baseMapping = GetGranularMappingDictionary("Year").ToDictionary(d => d.Key, d => d.Value.First());
                baseDictionary = GetGranularDictionary(baseMapping.Values.Distinct().ToList(), "Year");

                granularMappingDictionary = GetGranularMappingDictionary(granularityCode);
                granularDictionary = GetGranularDictionary(granularMappingDictionary.Values.SelectMany(v => v).Distinct().ToList(), granularityCode);                
            }

            var dictionary = GetDictionary(baseMapping, baseDictionary, granularMappingDictionary, granularDictionary);
            return CreateUsageListFromDoubleIdForecast(forecastTuple, dictionary);
        }

        private static Dictionary<long, string> GetGranularDictionary(List<long> idList, string granularityCode)
        {
            if(granularityCode == "Year")
            {
                //get years
                var informationYearMethods = new Methods.InformationSchema.Year();
                return idList.ToDictionary(
                    id => id,
                    id => informationYearMethods.Year_GetYearDescriptionByYearId(id)
                );
            }

            if(granularityCode == "Week")
            {
                //get weeks
                var informationWeekMethods = new Methods.InformationSchema.Week();
                return idList.ToDictionary(
                    id => id,
                    id => informationWeekMethods.Week_GetWeekDescriptionByWeekId(id)
                );
            }

            if(granularityCode == "Month")
            {
                //get months
                var informationMonthMethods = new Methods.InformationSchema.Month();
                return idList.ToDictionary(
                    id => id,
                    id => informationMonthMethods.Month_GetMonthDescriptionByMonthId(id)
                );
            }

            if(granularityCode == "Quarter")
            {
                //get quarters
                var informationQuarterMethods = new Methods.InformationSchema.Quarter();
                return idList.ToDictionary(
                    id => id,
                    id => informationQuarterMethods.Quarter_GetQuarterDescriptionByQuarterId(id)
                );
            }

            return new Dictionary<long, string>();
        }

        private static Dictionary<long, List<long>> GetGranularMappingDictionary(string granularityCode)
        {
            var dictionary = new Dictionary<long, long>();

            if(granularityCode == "Year")
            {
                //get date to quarter mappings
                dictionary = _mappingMethods.GetDateToYearDictionary();
            }
            else if(granularityCode == "Week")
            {
                //get date to week mappings
                dictionary = _mappingMethods.GetDateToWeekDictionary();
            }
            else if(granularityCode == "Month")
            {
                //get date to month mappings
                dictionary = _mappingMethods.GetDateToMonthDictionary();
            }
            else if(granularityCode == "Quarter")
            {
                //get date to quarter mappings
                dictionary = _mappingMethods.GetDateToQuarterDictionary();
            }
            
            return dictionary.Where(d => dateDictionary.Keys.Contains(d.Key)).ToDictionary(d => d.Key, d => new List<long> { d.Value });
        }

        private static Dictionary<long, Dictionary<long, string>> GetDictionary(Dictionary<long, long> baseMapping, Dictionary<long, string> baseDictionary, Dictionary<long, List<long>> subMapping, Dictionary<long, string> subDictionary)
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
                        var subDescription = (subDictionary[subId].Contains(':') ? " " : "-") + subDictionary[subId];
                        dictionary[baseId].Add(subId, $"{baseDescription}{subDescription}");
                    }
                }
            }

            return dictionary;
        }

        private static List<Usage> CreateUsageListFromDoubleIdForecast(List<Tuple<long, long, decimal>> forecastTuple, Dictionary<long, Dictionary<long, string>> dictionary)
        {
            var forecastTupleInDictionary = forecastTuple.Where(f => dictionary.ContainsKey(f.Item1) && dictionary[f.Item1].ContainsKey(f.Item2)).ToList();
            var forecastStringTupleList = (from forecast in forecastTupleInDictionary
                                            let forecastStringTuple = Tuple.Create((string)dictionary[forecast.Item1][forecast.Item2], (decimal)forecast.Item3)
                                            select forecastStringTuple).ToList();

            var forecastStringPeriodList = forecastStringTupleList.Select(f => f.Item1).Distinct().ToList();
            var forecastTupleDictionary = new Dictionary<string, List<decimal>>();

            if(forecastStringPeriodList.Count() == forecastStringTupleList.Count())
            {
                forecastTupleDictionary = forecastStringTupleList.ToDictionary(
                    f => f.Item1,
                    f => new List<decimal> { f.Item2 }
                );
            }
            else
            {
                forecastTupleDictionary = forecastStringPeriodList.ToDictionary(
                    d => d,
                    d => forecastStringTupleList.Where(f => f.Item1 == d).Select(f => f.Item2).ToList()
                );
            }

            var periodList = dictionary.SelectMany(d => d.Value).Select(d => d.Value).ToList();
            var periodsNotInForecastList = periodList.Except(forecastStringPeriodList).ToList();
            var periodsNotInForecastTupleUsageList = (from period in periodsNotInForecastList
                             let usage = new Usage { Date = period, Value = null, EntityCount = 0 }
                             select usage).ToList();

            var periodsInForecastTupleUsageList = (from forecast in forecastTupleDictionary
                             let usage = new Usage { Date = forecast.Key, Value = forecast.Value.Sum(), EntityCount = forecast.Value.Count() }
                             select usage).ToList();

            var usageList = periodsNotInForecastTupleUsageList.Union(periodsInForecastTupleUsageList).ToList();

            return usageList;
        }

        private static string ConvertMonth(string fullMonth)
        {
            var year = fullMonth.Substring(0, 5);
            var month = fullMonth.Replace(year, string.Empty);

            if(month == "January")
            {
                return $"{year}01";
            }
            else if(month == "February")
            {
                return $"{year}02";
            }
            else if(month == "March")
            {
                return $"{year}03";
            }
            else if(month == "April")
            {
                return $"{year}04";
            }
            else if(month == "May")
            {
                return $"{year}05";
            }
            else if(month == "June")
            {
                return $"{year}06";
            }
            else if(month == "July")
            {
                return $"{year}07";
            }
            else if(month == "August")
            {
                return $"{year}08";
            }
            else if(month == "September")
            {
                return $"{year}09";
            }
            else if(month == "October")
            {
                return $"{year}10";
            }
            else if(month == "November")
            {
                return $"{year}11";
            }
            else
            {
                return $"{year}12";
            }
        }
    }
}