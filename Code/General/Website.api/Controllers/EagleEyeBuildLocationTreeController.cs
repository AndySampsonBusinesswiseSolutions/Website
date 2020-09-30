using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using enums;
using System.Linq;
using System.Collections.Generic;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class EagleEyeBuildLocationTreeController : WebsiteController
    {
        private readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();

        public EagleEyeBuildLocationTreeController(ILogger<WebsiteController> logger) : base(logger)
        {
        }

        [HttpPost]
        [Route("EagleEyeBuildLocationTree/BuildLocationTree")]
        public IActionResult BuildLocationTree([FromBody] object data) //TODO: Build into new API
        {
            //Get SiteNameSiteAttributeId
            var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);

            //Get Site names
            var siteDictionary = _customerMethods.SiteDetail_GetSiteDetailDescriptionDictionaryBySiteAttributeId(siteNameSiteAttributeId);

            //Get MeterToSite mappings
            var siteToMeterDictionary = _mappingMethods.MeterToSite_GetSiteToMeterDictionaryBySiteIdList(siteDictionary);

            //Get MeterIdentifierMeterAttributeId
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get Meter identifiers
            var meterIdentifierDictionary = _customerMethods.MeterDetail_GetMeterDetailDescriptionDictionaryByMeterAttributeId(meterIdentifierMeterAttributeId);

            //Get AreaToMeter mappings
            var meterToAreaDictionary = _mappingMethods.AreaToMeter_GetMeterToAreaDictionaryByMeterIdList(meterIdentifierDictionary);
            
            //Get Areas
            var areaDictionary = _informationMethods.Area_GetAreaDictionary();

            //Get CommodityToMeter mappings
            var meterToCommodityDictionary = _mappingMethods.CommodityToMeter_GetMeterToCommodityDictionaryByMeterIdList(meterIdentifierDictionary);

            //Get Commodities
            var commodityDictionary = _informationMethods.Commodity_GetCommodityDictionary();

            //Build HTML
            var html = string.Empty;
            foreach(var site in siteDictionary)
            {
                var siteUlHTML = $"<ul class='format-listitem'>";

                var meterIdList = siteToMeterDictionary[site.Key];
                var areaIdList = meterToAreaDictionary.Where(m => meterIdList.Contains(m.Key)).SelectMany(m => m.Value).Distinct().ToList();

                foreach(var area in areaDictionary.Where(a => areaIdList.Contains(a.Key)))
                {
                    var areaUlHTML = $"<ul class='format-listitem'>";

                    var commodityIdList = meterToCommodityDictionary.Where(m => meterIdList.Contains(m.Key)).SelectMany(m => m.Value).Distinct().ToList();

                    foreach(var commodity in commodityDictionary.Where(c => commodityIdList.Contains(c.Key)))
                    {
                        var commodityUlHTML = $"<ul class='format-listitem'>";

                        foreach(var meter in meterIdentifierDictionary.Where(m => meterIdList.Contains(m.Key)))
                        {
                            var meterUlHTML = $"<ul class='format-listitem'>";
                            commodityUlHTML += $"{GetLiHtml("Meter", meter, meterUlHTML)}";
                        }

                        areaUlHTML += $"{GetLiHtml("Commodity", commodity, commodityUlHTML)}";
                    }

                    siteUlHTML += $"{GetLiHtml("Area", area, areaUlHTML)}";
                }

                html += $"{GetLiHtml("Site", site, siteUlHTML)}";
            }

            var baseUl = $"<ul id='siteSelectorList' class='format-listitem listItemWithoutPadding'>{html}<ul>";
            return new OkObjectResult(new { message = baseUl });
        }

        private string GetLiHtml(string type, KeyValuePair<long, string> dictionary, string ulHTML)
        {
            ulHTML += $"</ul>";

            var branchListDiv = $"<div id='{type}{dictionary.Key}List' class='listitem-hidden'>{ulHTML}</div>";
            var span = $"<span id='{type}{dictionary.Key}span'>{dictionary.Value}</span>";
            var icon = $"<i class='fas fa-site' style='padding-left: 3px; padding-right: 3px;'></i>";
            var checkbox = $"<input type='checkbox' id='{type}{dictionary.Key}checkbox' GUID='{dictionary.Key}' Branch='{type}' onclick='updatePage(this);'></input>";
            var branchDiv = $"<i id='{type}{dictionary.Key}' class='far fa-plus-square show-pointer expander'></i>";

            return $"<li>{branchDiv}{checkbox}{icon}{span}{branchListDiv}</li>";
        }
    }
}