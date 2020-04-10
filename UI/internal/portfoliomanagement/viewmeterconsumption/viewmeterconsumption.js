function pageLoad() {
  createTree(sites, "updateCharts()");  

  document.onmousemove=function(e) {
    var mousecoords = getMousePos(e);
    if(mousecoords.x <= 15) {
      openNav();
    }  
    else if(mousecoords.x >= 400) {
      closeNav();
    }  
  };

  window.onload = function() {
    updateCharts();
    hideSliders();
  }
}

function getMousePos(e) {
	return {x:e.clientX,y:e.clientY};
}

function openNav() {
	document.getElementById("mySidenav").style.width = "400px";
	document.getElementById("openNav").style.color = "#b62a51";
}

function closeNav() {
	document.getElementById("openNav").style.color = "white";
	document.getElementById("mySidenav").style.width = "0px";
}

function hideSliders() {
  var sliders = document.getElementsByClassName('slider-list');
  [...sliders].forEach(slider => {
    slider.classList.add('listitem-hidden');
  });
}

function updateCharts() {
  var treeDiv = document.getElementById('siteDiv');
  var inputs = treeDiv.getElementsByTagName('input');
  var checkboxes = getCheckedCheckBoxes(inputs);	
  var commodityOption = getCommodityOption();
  var meters = getMeters(checkboxes, commodityOption);

  updateUsageChart(meters);
  updateTotalCostChart(meters);
  updateCostBreakdownChart(meters);
  updateCapacityChart(meters);
}

function getMeters(checkboxes, commodityOption) {
  var noGroupradio = document.getElementById('noGroupradio');
  var checkboxLength = checkboxes.length;
  var tempMeters = [];
  var commodities = [];
  var branches = [];

  if(commodityOption == '') {
    commodities.push('Electricity');
    commodities.push('Gas');
  }
  else {
    commodities.push(commodityOption);
  }

  for(var i = 0; i < checkboxLength; i++) {
    var span = document.getElementById(checkboxes[i].id.replace('checkbox', 'span'));
    var hierarchy = checkboxes[i].id.replace('checkbox', '').split('_');
    var lastRecord = hierarchy[hierarchy.length - 1];

    for(var j = 0; j < commodities.length; j++) {
      var meters = [];
      var branch = '';

      if(lastRecord.includes('Site')) {
        var site = sites[parseInt(lastRecord.replace('Site', ''))];
        meters = getMetersBySite(site, commodities[j]);
        branch = 'Site';
      }
      else if(lastRecord.includes('SubArea')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 5].replace('Site', ''))];
        var area = site.Areas[parseInt(hierarchy[hierarchy.length - 4].replace('Area', ''))];
        var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 3].replace('Commodity', ''))];
        var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 2].replace('Meter', ''))];
        var subArea = meter.SubAreas[parseInt(lastRecord.replace('SubArea', ''))];
        meters = getSubMetersBySubArea(subArea, commodities[j]);
        branch = 'SubArea';
      }
      else if(lastRecord.includes('Area')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 2].replace('Site', ''))];
        var area = site.Areas[parseInt(lastRecord.replace('Area', ''))];
        meters = getMetersByArea(area, commodities[j]);
        branch = 'Area';
      }
      else if(lastRecord.includes('Commodity')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 3].replace('Site', ''))];
        var area = site.Areas[parseInt(hierarchy[hierarchy.length - 2].replace('Area', ''))];
        var commodity = area.Commodities[parseInt(lastRecord.replace('Commodity', ''))];
        meters = getMetersByCommodity(commodity, commodities[j]);
        branch = 'Commodity';
      }
      else if(lastRecord.includes('SubMeter')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 7].replace('Site', ''))];
        var area = site.Areas[parseInt(hierarchy[hierarchy.length - 6].replace('Area', ''))];
        var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 5].replace('Commodity', ''))];
        var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 4].replace('Meter', ''))];
        var subArea = meter.SubAreas[parseInt(hierarchy[hierarchy.length - 3].replace('SubArea', ''))];
        var asset = subArea.Assets[parseInt(hierarchy[hierarchy.length - 2].replace('Asset', ''))];
        var subMeter = asset.SubMeters[parseInt(lastRecord.replace('SubMeter', ''))];
        branch = 'SubMeter';

        if(getAttribute(subMeter.Attributes, 'Commodities').includes(commodities[j])) {
          meters.push(subMeter);
        }
      }
      else if(lastRecord.includes('Meter')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 4].replace('Site', ''))];
        var area = site.Areas[parseInt(hierarchy[hierarchy.length - 3].replace('Area', ''))];
        var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 2].replace('Commodity', ''))];
        var meter = commodity.Meters[parseInt(lastRecord.replace('Meter', ''))];
        branch = 'Meter';

        if(getAttribute(meter.Attributes, 'Commodities').includes(commodities[j])) {
          meters.push(meter);
        }
      }
      else if(lastRecord.includes('Asset')) {
        var site = sites[parseInt(hierarchy[hierarchy.length - 6].replace('Site', ''))];
        var area = site.Areas[parseInt(hierarchy[hierarchy.length - 5].replace('Area', ''))];
        var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 4].replace('Commodity', ''))];
        var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 3].replace('Meter', ''))];
        var subArea = meter.SubAreas[parseInt(hierarchy[hierarchy.length - 2].replace('SubArea', ''))];
        var asset = subArea.Assets[parseInt(lastRecord.replace('Asset', ''))];
        meters = getSubMetersByAsset(asset, commodities[j]);
        branch = 'Asset';
      }
  
      if(meters.length > 0) {
        var tempMeter = {
          SeriesName: span.innerText + ' - ' + commodities[j],
          Commodity: commodities[j],
          Branch: branch,
          Meters: meters
        }
    
        tempMeters.push(tempMeter);

        if(!branches.includes(branch)) {
          branches.push(branch);
        }
      }
    }    
  }

  if(noGroupradio.checked) {
    return tempMeters;
  }

  var series = [];
  for(var i = 0; i < branches.length; i++) {
    for(var j = 0; j < commodities.length; j++) {
      var tempMeter = {
        SeriesName: branches[i] + ' - ' + commodities[j],
        Meters: []
      }
  
      for(var k = 0; k < tempMeters.length; k++) {
        if(tempMeters[k].Commodity == commodities[j] && tempMeters[k].Branch == branches[i]) {
          tempMeter.Meters.push(...tempMeters[k].Meters);
        }      
      }
  
      if(tempMeter.Meters.length > 0) {
        series.push(tempMeter);
      }
    }
  }  

  return series;
}

function getMetersBySite(site, commodityOption) {
  var meters = [];

  var areaLength = site.Areas.length;
  for(var areaCount = 0; areaCount < areaLength; areaCount++) {
    var area = site.Areas[areaCount];

    if(getAttribute(area.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getMetersByArea(area, commodityOption));
    }
  }

  return [...meters];
}

function getMetersByArea(area, commodityOption) {
  var meters = [];

  var commodityLength = area.Commodities.length;
  for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
    var commodity = area.Commodities[commodityCount];

    if(getAttribute(commodity.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getMetersByCommodity(commodity, commodityOption));
    }
  }

  return [...meters];
}

function getMetersByCommodity(commodity, commodityOption) {
  var meters = [];

  var meterLength = commodity.Meters.length;
  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = commodity.Meters[meterCount];

    if(getAttribute(meter.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(meter);
    }
  }

  return [...meters];
}

function getSubMetersBySubArea(subArea, commodityOption) {
  var meters = [];

  var assetLength = subArea.Assets.length;
  for(var assetCount = 0; assetCount < assetLength; assetCount++) {
    var asset = subArea.Assets[assetCount];

    if(getAttribute(asset.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getSubMetersByAsset(asset, commodityOption));
    }
  }

  return [...meters];
}

function getSubMetersByAsset(asset, commodityOption) {
  var meters = [];

  var subMeterLength = asset.SubMeters.length;
  for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++) {
    var subMeter = asset.SubMeters[subMeterCount];

    if(getAttribute(subMeter.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(subMeter);
    }
  }

  return [...meters];
}

function updateUsageChart(meters) {
  var usageChartOptionsTimeSpan = document.getElementById('usageChartOptionsTimeSpan');
  var usageChartOptionsDateRange = document.getElementById('usageChartOptionsDateRange');

  var showByArray = ['Usage'];
  updateChart(usageChartOptionsDateRange, usageChartOptionsTimeSpan, showByArray, '#usageChart', meters);
}

function updateTotalCostChart(meters) {
  var totalCostChartOptionsTimeSpan = document.getElementById('totalCostChartOptionsTimeSpan');
  var totalCostChartOptionsDateRange = document.getElementById('totalCostChartOptionsDateRange');
  
  var showByArray = ['Cost'];
  updateChart(totalCostChartOptionsDateRange, totalCostChartOptionsTimeSpan, showByArray, '#totalCostChart', meters);
}

function updateCostBreakdownChart(meters) {
  var costBreakdownChartOptionsTimeSpan = document.getElementById('costBreakdownChartOptionsTimeSpan');
  var costBreakdownChartOptionsDateRange = document.getElementById('costBreakdownChartOptionsDateRange');
  
  var showByArray = [];

  var costBreakdownChartElementAllOptionscheckbox = document.getElementById('costBreakdownChartElementAllOptionscheckbox');
  if(costBreakdownChartElementAllOptionscheckbox.checked) {
    showByArray = [
      'CCL',
      'CM',
      'BSUoS',
      'CFD',
      'FiT',
      'RO',
      'DLoss',
      'DUoSCap',
      'DUoSCapFix',
      'DUoSSC',
      'DUoS',
      'TLoss',
      'Wholesale'
    ];
  }
  else {
    var costBreakdownChartElementOptionsList = document.getElementById('costBreakdownChartElementOptionsList');
    var inputs = costBreakdownChartElementOptionsList.getElementsByTagName('input');
    var inputLength = inputs.length;

    for(var i = 0; i < inputLength; i++) {
      if(inputs[i].checked) {
        showByArray.push(inputs[i].id.replace('costBreakdownChartElement', '').replace('Optionscheckbox', ''));
      }
    }
  }

  updateChart(costBreakdownChartOptionsDateRange, costBreakdownChartOptionsTimeSpan, showByArray, '#costBreakdownChart', meters);
}

function updateCapacityChart(meters) {
  var capacityChartOptionsTimeSpan = document.getElementById('capacityChartOptionsTimeSpan');
  var capacityChartOptionsDateRange = document.getElementById('capacityChartOptionsDateRange');
  
  var showByArray = ['Capacity', 'MaxDemand'];
  updateChart(capacityChartOptionsDateRange, capacityChartOptionsTimeSpan, showByArray, '#capacityChart', meters);
}

function getChartTypeFromCategoryCount(categoryCount) {
  return categoryCount == 1 ? 'bar' : 'line';
}

function updateChart(dateRangeElement, timeSpanElement, showByArray, chartId, meters) {
  var startDateMilliseconds = parseInt(dateRangeElement.getElementsByClassName('rz-pointer-min')[0].getAttribute('aria-valuenow'));
  var endDateMilliseconds = parseInt(dateRangeElement.getElementsByClassName('rz-pointer-max')[0].getAttribute('aria-valuenow'));
  var startDate = new Date(startDateMilliseconds);
  var endDate = new Date(endDateMilliseconds + (24*60*60*1000));
  var dateFormat = getPeriodDateFormat(timeSpanElement.children[6].innerHTML)
  var newCategories = getCategoryTexts(startDate, endDate, dateFormat);

  var newSeries = [];
  var showByLength = showByArray.length;

  for(var i = 0; i < showByLength; i++) {
    for(var j = 0; j < meters.length; j++) {
      var series = getNewChartSeries(meters[j].Meters, showByArray[i], newCategories, dateFormat, startDate, endDate, meters[j].SeriesName);

      if(series.length) {
        newSeries.push(...series);
      }
      else {
        newSeries.push(series);
      }
    }    
  }  

  var chartOptions = {
    chart: {
        type: getChartTypeFromCategoryCount(newCategories.length),
    },
    yaxis: [{
      title: {
        text: getChartYAxisTitle(chartId)
      },
      forceNiceScale: true,
      labels: {
        formatter: function(val) {
          return getYAxisLabelFormat(chartId, val);
        }
      }
    }],
    xaxis: {
        type: getXAxisTypeFromTimeSpan(timeSpanElement.children[6].innerHTML),
        min: newCategories[0],
        max: newCategories[newCategories.length - 1],
        categories: newCategories
    }
  };

  clearElement(document.getElementById(chartId.replace('#', '')));
  refreshChart(newSeries, chartId, chartOptions);
}

function getYAxisLabelFormat(chartId, val) {
  switch(chartId) {
    case '#usageChart':
    case '#capacityChart':
      return val.toLocaleString();
    default:
      return '£' + val.toLocaleString();
  }
}

function refreshChart(newSeries, chartId, chartOptions) {
  var options = {
    chart: {
        height: '100%',
        width: '100%',
      type: chartOptions.chart.type,
      zoom: {
        type: 'x',
        enabled: true,
        autoScaleYaxis: true
      },
      animations: {
        enabled: true,
        easing: 'easeout',
        speed: 800,
        animateGradually: {
            enabled: true,
            delay: 150
        },
        dynamicAnimation: {
            enabled: true,
            speed: 350
        }
      },
      toolbar: {
        autoSelected: 'zoom',
        tools: {
          download: false
        }
      }
    },
    dataLabels: {
      enabled: false
    },
    legend: {
      show: true,
      showForSingleSeries: true,
      showForNullSeries: true,
      showForZeroSeries: true,
      position: 'right',
      onItemClick: {
        toggleDataSeries: true
      },
      width: 200,
      offsetY: 25,
      formatter: function(seriesName) {
        return getLegendFormat(chartId, seriesName);
      }
    },
    series: newSeries,
    yaxis: chartOptions.yaxis,
    xaxis: chartOptions.xaxis
  };  

  renderChart(chartId, options);
}

function getLegendFormat(chartId, seriesName) {
  switch(chartId) {
    case '#usageChart':
    case '#totalCostChart':
    case '#capacityChart':
      return seriesName + '<br><br>';
    default:
      return seriesName;
  }
}

function getXAxisTypeFromTimeSpan(timeSpan) {
  switch(timeSpan) {
    case 'Half Hourly':
      return 'datetime';
    default:
      return 'category';
  }
}

function getCommodityOption() {
  var commodity = '';
  var electricityCommodityradio = document.getElementById('electricityCommodityradio');
  if(electricityCommodityradio.checked) {
    commodity = 'Electricity';
  }
  else {
    var gasCommodityradio = document.getElementById('gasCommodityradio');
    if(gasCommodityradio.checked) {
      commodity = 'Gas';
    }
  }
  return commodity;
}

function createTree(sites, functions) {
  var div = document.getElementById('siteDiv');
  var inputs = div.getElementsByTagName('input');
  var checkboxes = getCheckedCheckBoxes(inputs);
  var elements = div.getElementsByTagName("*");

  var checkboxIds = [];
  for(var i = 0; i < checkboxes.length; i++) {
    checkboxIds.push(checkboxes[i].id);
  }

  var elementClasses = [];
  for(var i = 0; i < elements.length; i++) {
    if(elements[i].id != '') {
      var element = {
        id: elements[i].id,
        classList: elements[i].classList
      }
  
      elementClasses.push(element);
    }    
  }

  clearElement(div);
  
  var tree = document.createElement('div');
  tree.setAttribute('class', 'scrolling-wrapper');
  
  var ul = createBranchUl('siteDivSelector', false);
  tree.appendChild(ul);

  buildSiteBranch(sites, getCommodityOption(), ul, functions);

  var header = document.createElement('span');
  header.style = "padding-left: 5px;";
  header.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square show-pointer"" id="siteDivSelector"></i>';

  div.appendChild(header);
  div.appendChild(tree);

  addExpanderOnClickEvents();

  for(var i = 0; i < checkboxIds.length; i++) {
    var checkbox = document.getElementById(checkboxIds[i]);
    if(checkbox) {
      checkbox.checked = true;
    }
  }

  for(var i = 0; i < elementClasses.length; i++) {
    var element = document.getElementById(elementClasses[i].id);
    if(element) {
      element.classList = elementClasses[i].classList;
    }
  }  
}

//build site
function buildSiteBranch(sites, commodityOption, elementToAppendTo, functions) {
  var siteLength = sites.length;

  for(var siteCount = 0; siteCount < siteLength; siteCount++) {
    var site = sites[siteCount];

    if(!commodityMatch(site, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren('Site' + siteCount, site.hasOwnProperty('Areas'), functions, site.Attributes, 'Site');
    elementToAppendTo.appendChild(listItem);

    if(site.hasOwnProperty('Areas')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildAreaBranch(site.Areas, commodityOption, ul, functions, 'Site' + siteCount);
    }
  }
}

//build area
function buildAreaBranch(areas, commodityOption, elementToAppendTo, functions, previousId) {
  var areaLength = areas.length;

  for(var areaCount = 0; areaCount < areaLength; areaCount++) {
    var area = areas[areaCount];

    if(!commodityMatch(area, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Area' + areaCount, area.hasOwnProperty('Commodities'), functions, area.Attributes, 'Area');
    elementToAppendTo.appendChild(listItem);

    if(area.hasOwnProperty('Commodities')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildCommodityBranch(area.Commodities, commodityOption, ul, functions, previousId + '_Area' + areaCount);
    }
  }
}

//build commodity
function buildCommodityBranch(commodities, commodityOption, elementToAppendTo, functions, previousId) {
  var commodityLength = commodities.length;

  for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
    var commodity = commodities[commodityCount];

    if(!commodityMatch(commodity, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Commodity' + commodityCount, commodity.hasOwnProperty('Meters'), functions, commodity.Attributes, 'Commodity');
    elementToAppendTo.appendChild(listItem);

    if(commodity.hasOwnProperty('Meters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildMeterBranch(commodity.Meters, commodityOption, ul, functions, previousId + '_Commodity' + commodityCount);
    }
  }
}

//build meter
function buildMeterBranch(meters, commodityOption, elementToAppendTo, functions, previousId) {
  var meterLength = meters.length;

  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = meters[meterCount];

    if(!commodityMatch(meter, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Meter' + meterCount, meter.hasOwnProperty('SubAreas'), functions, meter.Attributes, 'Meter');
    elementToAppendTo.appendChild(listItem);

    if(meter.hasOwnProperty('SubAreas')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubAreaBranch(meter.SubAreas, commodityOption, ul, functions, previousId + '_Meter' + meterCount);
    }
  }
}

//build sub area
function buildSubAreaBranch(subAreas, commodityOption, elementToAppendTo, functions, previousId) {
  var subAreaLength = subAreas.length;

  for(var subAreaCount = 0; subAreaCount < subAreaLength; subAreaCount++) {
    var subArea = subAreas[subAreaCount];

    if(!commodityMatch(subArea, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_SubArea' + subAreaCount, subArea.hasOwnProperty('Assets'), functions, subArea.Attributes, 'SubArea');
    elementToAppendTo.appendChild(listItem);

    if(subArea.hasOwnProperty('Assets')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildAssetBranch(subArea.Assets, commodityOption, ul, functions, previousId + '_SubArea' + subAreaCount);
    }
  }
}

//build asset
function buildAssetBranch(assets, commodityOption, elementToAppendTo, functions, previousId) {
  var assetLength = assets.length;

  for(var assetCount = 0; assetCount < assetLength; assetCount++) {
    var asset = assets[assetCount];

    if(!commodityMatch(asset, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Asset' + assetCount, asset.hasOwnProperty('SubMeters'), functions, asset.Attributes, 'Asset');
    elementToAppendTo.appendChild(listItem);

    if(asset.hasOwnProperty('SubMeters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubMeterBranch(asset.SubMeters, commodityOption, ul, functions, previousId + '_Asset' + assetCount);
    }
  }
}

//build sub meter
function buildSubMeterBranch(subMeters, commodityOption, elementToAppendTo, functions, previousId) {
  var subMeterLength = subMeters.length;

  for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++) {
    var subMeter = subMeters[subMeterCount];

    if(!commodityMatch(subMeter, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_SubMeter' + subMeterCount, false, functions, subMeter.Attributes, 'SubMeter');
    elementToAppendTo.appendChild(listItem);
  }
}

function commodityMatch(entity, commodity) {
  if(commodity == '') {
      return true;
  }

  var entityCommodities = getAttribute(entity.Attributes, 'Commodities');
  return entityCommodities && entityCommodities.includes(commodity);
}

function appendListItemChildren(id, hasChildren, functions, attributes, branch) {
  var li = document.createElement('li');
  li.appendChild(createBranchDiv(id, hasChildren));
  li.appendChild(createBranchCheckbox(id, functions, branch));
  li.appendChild(createBranchIcon(getAttribute(attributes, 'Icon')));
  li.appendChild(createBranchSpan(id, getAttribute(attributes, 'Name')));

  if(hasChildren) {
    li.appendChild(createBranchUl(id));
  }

  return li;
}

function createBranchUl(id, hideUl = true) {
  var ul = document.createElement('ul');
  ul.id = id.concat('List');
  ul.setAttribute('class', 'format-listitem' + (hideUl ? ' listitem-hidden' : ''));
  return ul;
}

function createBranchDiv(branchDivId, hasChildren = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    branchDiv.setAttribute('class', (hasChildren ? 'far fa-plus-square show-pointer' : 'far fa-times-circle') + ' expander');
    branchDiv.setAttribute('style', 'padding-right: 4px;');
    return branchDiv;
}

function createBranchCheckbox(id, functions, branch) {
  var checkbox = document.createElement('input');
  checkbox.type = 'checkbox';  
  checkbox.id = id.concat('checkbox');
  checkbox.setAttribute('branch', branch);

  var functionArray = functions.replace(')', '').split('(');
  var functionArrayLength = functionArray.length;
  var functionName = functionArray[0];
  var functionArguments = [];

  functionArguments.push(checkbox.id);
  if(functionArrayLength > 1) {
      var functionArgumentLength = functionArray[1].split(',').length;
      for(var i = 0; i < functionArgumentLength; i++) {
          functionArguments.push(functionArray[1].split(',')[i]);
      }
  }
  functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
  
  checkbox.setAttribute('onclick', functionName);
  return checkbox;
}

function getAttribute(attributes, attributeRequired) {
	for (var attribute in attributes) {
		var array = attributes[attribute];

		for(var key in array) {
			if(key == attributeRequired) {
				return array[key];
			}
		}
	}

	return null;
}

function createBranchIcon(iconClass) {
  var icon = document.createElement('i');
  icon.setAttribute('class', iconClass);
  icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
  return icon;
}

function createBranchSpan(id, innerHTML) {
  var span = document.createElement('span');
  span.id = id.concat('span');
  span.innerHTML = innerHTML;
  return span;
}

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

function updateClassOnClick(elementId, firstClass, secondClass){
	var elements = document.getElementsByClassName(elementId);

	if(elements.length == 0) {
		var element = document.getElementById(elementId);
		updateClass(element, firstClass, secondClass);
	}
	else {
		for(var i = 0; i< elements.length; i++) {
			updateClass(elements[i], firstClass, secondClass)
		}
	}
}

function updateClass(element, firstClass, secondClass)
{
	if(hasClass(element, firstClass)){
		element.classList.remove(firstClass);

		if(secondClass != ''){
			element.classList.add(secondClass);
		}
	}
	else {
		if(secondClass != ''){
			element.classList.remove(secondClass);
		}
		
		element.classList.add(firstClass);
	}
}
  
function hasClass(elem, className) {
	return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
  }
  
  updateClassOnClick('siteDivSelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('commoditySelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('usage', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('groupingSelector', 'fa-plus-square', 'fa-minus-square');  
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
}

function createBlankChart(chartId, noDataText) {
    var options = {
        chart: {
            height: '100%',
            width: '100%',
          type: 'line'
        },
        series: [],
        yaxis: {
            show: false
        },
      noData: {
            text: noDataText,
            align: 'center',
            verticalAlign: 'middle',
            offsetX: 0,
            offsetY: 0
        }
      }
      
      renderChart(chartId, options);
}

function renderChart(chartId, options) {
  var chart = new ApexCharts(document.querySelector(chartId), options);
  chart.render();
}

function getCheckedCheckBoxes(inputs) {
    var checkBoxes = [];
    var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
      if(inputs[i].type.toLowerCase() == 'checkbox') {
        if(inputs[i].checked) {
          checkBoxes.push(inputs[i]);
        }
      }
    }

    if(checkBoxes.length == 0) {
      for(var i = 0; i < inputLength; i++) {
        if(inputs[i].type.toLowerCase() == 'checkbox') {
          if(inputs[i].getAttribute('branch') == 'Site') {
            inputs[i].checked = true;
            checkBoxes.push(inputs[i]);
          }
        }
      }
    }
  
    return checkBoxes;
}

function getPeriodDateFormat(period) {
    switch(period) {
      case 'Half Hourly':
        return 'dd MMM yy hh:mm:ss';
      case 'Daily':
        return 'dd MMM yy';
      case "Monthly":
        return 'MMM yyyy';
      case "Quarterly":
        return 'yyyy QQ';
      case "Yearly":
        return 'yyyy';
    }
}

function getCategoryTexts(startDate, endDate, dateFormat) {
    var newCategories = [];
  
    for(var newDate = new Date(startDate); newDate < new Date(endDate); newDate.setDate(newDate.getDate() + 1)) {
      for(var hh = 0; hh < 48; hh++) {
        var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), dateFormat).toString();
  
        if(!newCategories.includes(newCategoryText)) {
          newCategories.push(newCategoryText);
        }      
      }
    }
  
    return newCategories;
}

function formatDate(dateToBeFormatted, format) {
	var baseDate = new Date(dateToBeFormatted);

	switch(format) {
		case 'dd MMM yy':
			var aaaa = baseDate.getFullYear();
			var gg = baseDate.getDate();
			var mm = (baseDate.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return gg + ' ' + convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'dd MMM yy hh:mm:ss':
			var hours = baseDate.getHours()
			var minutes = baseDate.getMinutes()
			var seconds = baseDate.getSeconds();
		
			if (hours < 10) {
				hours = '0' + hours;
			}				
		
			if (minutes < 10) {
				minutes = '0' + minutes;
			}				
		
			if (seconds < 10) {
				seconds = '0' + seconds;
			}			
		
			return formatDate(baseDate, 'dd MMM yy') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return baseDate.getFullYear();
		case 'yyyy QQ':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

      return aaaa + ' ' + convertMonthIdToQuarter(mm);
	}
}

function getNewChartSeries(meters, showBy, newCategories, dateFormat, startDate, endDate, seriesName) {
  if(showBy == "MaxDemand") {
    var meterLength = meters.length;

    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = meters[meterCount];
      var meterData = meter['Capacity'];
      
      if(!meterData) {
        continue;
      }

      var maxDemand = getAttribute(meter.Attributes, 'MaxDemand');      
      meter['MaxDemand'] = JSON.parse(JSON.stringify(meterData));

      var meterDataLength = meterData.length;
      for(var j = 0; j < meterDataLength; j++) {
        meter['MaxDemand'][j].Value = maxDemand;
      }
    }
  }

  var summedMeterSeries = getSummedMeterSeries(meters, showBy, newCategories, dateFormat, startDate, endDate);
  return finaliseData(summedMeterSeries, seriesName.concat(' - ').concat(showBy));
}

function finaliseData(summedMeterSeries, seriesName) {
  var finalSeries = [];
  var noGroupradio = document.getElementById('noGroupradio');

  if(noGroupradio.checked) {
    var series = {
      name: seriesName,
      data: summedMeterSeries.value
    };

    return series;
  }
  else {
    var sumcheckbox = document.getElementById('sumcheckbox');
    if(sumcheckbox.checked) {
      var series = {
        name: seriesName + ' - Sum',
        data: summedMeterSeries.value
      };
  
      finalSeries.push(series);
    }

    var averagecheckbox = document.getElementById('averagecheckbox');
    if(averagecheckbox.checked) {
      var series = {
        name: seriesName + ' - Average',
        data: []
      };
  
      for(var i = 0; i < summedMeterSeries.value.length; i++) {
        series.data.push(summedMeterSeries.value[i]/summedMeterSeries.count[i]);
      }
  
      finalSeries.push(series);
    }
  }

  var temp = [...finalSeries];
  return temp;
}

function getSummedMeterSeries(meters, showBy, newCategories, dateFormat, startDate, endDate) {
  var meterLength = meters.length;
  var summedMeterSeries = {
    value: [0],
    count: [0]
  }
  
  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = meters[meterCount];
    var meterData = meter[showBy];
    
    if(!meterData) {
      continue;
    }

    var meterDatesApplied = [];
    var meterDataLength = meterData.length;
    for(var j = 0; j < meterDataLength; j++) {
      var meterDate = new Date(meterData[j].Date);

      if(meterDate >= startDate && meterDate <= endDate) {
        var formattedDate = formatDate(meterDate, dateFormat);
        var i = newCategories.findIndex(n => n == formattedDate);
        var value = meterData[j].Value;

        if(!value && !summedMeterSeries.value[i]){
          summedMeterSeries.value[i] = null;
        }
        else if(value && !summedMeterSeries.value[i]) {
          summedMeterSeries.value[i] = value;
        }
        else if(value && summedMeterSeries.value[i]) {
          summedMeterSeries.value[i] += value;
        }  
        
        if(!meterDatesApplied.includes(formattedDate)) {
          meterDatesApplied.push(formattedDate);

          if(summedMeterSeries.count[i]) {
            summedMeterSeries.count[i] += 1;
          }
          else {
            summedMeterSeries.count[i] = 1;
          }
        }
      }          					     
    }
  } 

  return summedMeterSeries;
}

function getChartYAxisTitle(showBy) {
  switch(showBy) {
    case '#usageChart':
      return 'kWh';
    case '#capacityChart':
      return 'kVa';
    default:
      return '£';
  }
}

function convertMonthIdToFullText(monthId) {
	switch(monthId) {
		case 1:
			return 'January';
		case 2:
			return 'February';
		case 3:
			return 'March';
		case 4:
			return 'April';
		case 5:
			return 'May';
		case 6:
			return 'June';
		case 7:
			return 'July';
		case 8:
			return 'August';
		case 9:
			return 'September';
		case 10:
			return 'October';
		case 11:
			return 'November';
		case 12:
			return 'December';
	}
}

function convertMonthIdToShortCode(monthId) {
	return convertMonthIdToFullText(parseInt(monthId)).slice(0, 3).toUpperCase();
}

function convertMonthIdToQuarter(monthId) {
  switch(monthId) {
		case 1:
		case 2:
		case 3:
			return 'Q1';
		case 4:
		case 5:
		case 6:
      return 'Q2';
		case 7:
		case 8:
		case 9:
      return 'Q3';
		case 10:
		case 11:
		case 12:
			return 'Q4';
	}
}