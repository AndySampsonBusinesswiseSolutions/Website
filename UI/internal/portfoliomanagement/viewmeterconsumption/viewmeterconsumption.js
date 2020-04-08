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
  updateUsageChart();
  updateTotalCostChart();
  updateCostBreakdownChart();
  updateCapacityChart();
}

function updateUsageChart() {
  var usageChartOptionsTimeSpan = document.getElementById('usageChartOptionsTimeSpan');
  var usageChartOptionsDateRange = document.getElementById('usageChartOptionsDateRange');

  var showByArray = ['Usage'];
  updateChart(usageChartOptionsDateRange, usageChartOptionsTimeSpan, showByArray, '#usageChart');
}

function updateTotalCostChart() {
  var totalCostChartOptionsTimeSpan = document.getElementById('totalCostChartOptionsTimeSpan');
  var totalCostChartOptionsDateRange = document.getElementById('totalCostChartOptionsDateRange');
  
  var showByArray = ['Cost'];
  updateChart(totalCostChartOptionsDateRange, totalCostChartOptionsTimeSpan, showByArray, '#totalCostChart');
}

function updateCostBreakdownChart() {
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

  updateChart(costBreakdownChartOptionsDateRange, costBreakdownChartOptionsTimeSpan, showByArray, '#costBreakdownChart');
}

function updateCapacityChart() {
  var capacityChartOptionsTimeSpan = document.getElementById('capacityChartOptionsTimeSpan');
  var capacityChartOptionsDateRange = document.getElementById('capacityChartOptionsDateRange');
  
  var showByArray = ['Capacity', 'MaxDemand'];
  updateChart(capacityChartOptionsDateRange, capacityChartOptionsTimeSpan, showByArray, '#capacityChart');
}

function getChartTypeFromCategoryCount(categoryCount) {
  return categoryCount == 1 ? 'bar' : 'line';
}

function updateChart(dateRangeElement, timeSpanElement, showByArray, chartId) {
  var startDateMilliseconds = parseInt(dateRangeElement.getElementsByClassName('rz-pointer-min')[0].getAttribute('aria-valuenow'));
  var endDateMilliseconds = parseInt(dateRangeElement.getElementsByClassName('rz-pointer-max')[0].getAttribute('aria-valuenow'));
  var startDate = new Date(startDateMilliseconds);
  var endDate = new Date(endDateMilliseconds + (24*60*60*1000));

  var treeDiv = document.getElementById('siteDiv');
  var inputs = treeDiv.getElementsByTagName('input');
  var checkBoxes = getCheckedCheckBoxes(inputs);	
  var dateFormat = getPeriodDateFormat(timeSpanElement.children[6].innerHTML)
  var newCategories = getCategoryTexts(startDate, endDate, dateFormat);

  var newSeries = [];
  var showByLength = showByArray.length;

  for(var i = 0; i < showByLength; i++) {
    newSeries.push(...getNewChartSeries(checkBoxes, showByArray[i], newCategories, getCommodityOption(), dateFormat, startDate, endDate));
  }  

  var chartOptions = {
    chart: {
        type: getChartTypeFromCategoryCount(newCategories.length),
    },
    yaxis: {
      title: {
        text: getChartYAxisTitle(chartId)
      },
      forceNiceScale: true,
      labels: {
        formatter: function(val) {
          return getYAxisLabelFormat(chartId, val);
        }
      }
    },
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
      width: getLegendWidth(chartId),
      offsetY: getLegendOffsetY(chartId),
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

function getLegendWidth(chartId) {
  switch(chartId) {
    case '#usageChart':
    case '#totalCostChart':
    case '#capacityChart':
      return 100;
    default:
      return 200;
  }
}

function getLegendOffsetY(chartId) {
  switch(chartId) {
    case '#usageChart':
    case '#totalCostChart':
    case '#capacityChart':
      return 250;
    default:
      return 0;
  }
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
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createBranchUl('siteDivSelector', false);
    tree.appendChild(ul);

    buildSiteBranch(sites, getCommodityOption(), ul, functions);

    var div = document.getElementById('siteDiv');
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square show-pointer"" id="siteDivSelector"></i>';

    div.appendChild(header);
    div.appendChild(tree);

    addExpanderOnClickEvents();
}

//build site
function buildSiteBranch(sites, commodityOption, elementToAppendTo, functions) {
  var siteLength = sites.length;

  for(var siteCount = 0; siteCount < siteLength; siteCount++) {
    var site = sites[siteCount];

    if(!commodityMatch(site, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren('Site' + siteCount, site.hasOwnProperty('Areas'), functions, site.Attributes);
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

    var listItem = appendListItemChildren(previousId + 'Area' + areaCount, area.hasOwnProperty('Commodities'), functions, area.Attributes);
    elementToAppendTo.appendChild(listItem);

    if(area.hasOwnProperty('Commodities')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildCommodityBranch(area.Commodities, commodityOption, ul, functions, previousId + 'Area' + areaCount);
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

    var listItem = appendListItemChildren(previousId + 'Commodity' + commodityCount, commodity.hasOwnProperty('Meters'), functions, commodity.Attributes);
    elementToAppendTo.appendChild(listItem);

    if(commodity.hasOwnProperty('Meters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildMeterBranch(commodity.Meters, commodityOption, ul, functions, previousId + 'Commodity' + commodityCount);
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

    var listItem = appendListItemChildren(previousId + 'Meter' + meterCount, meter.hasOwnProperty('SubAreas'), functions, meter.Attributes);
    elementToAppendTo.appendChild(listItem);

    if(meter.hasOwnProperty('SubAreas')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubAreaBranch(meter.SubAreas, commodityOption, ul, functions, previousId + 'Meter' + meterCount);
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

    var listItem = appendListItemChildren(previousId + 'SubArea' + subAreaCount, subArea.hasOwnProperty('Assets'), functions, subArea.Attributes);
    elementToAppendTo.appendChild(listItem);

    if(subArea.hasOwnProperty('Assets')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildAssetBranch(subArea.Assets, commodityOption, ul, functions, previousId + 'SubArea' + subAreaCount);
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

    var listItem = appendListItemChildren(previousId + 'Asset' + assetCount, asset.hasOwnProperty('SubMeters'), functions, asset.Attributes);
    elementToAppendTo.appendChild(listItem);

    if(asset.hasOwnProperty('SubMeters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubMeterBranch(asset.SubMeters, commodityOption, ul, functions, previousId + 'Asset' + assetCount);
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

    var listItem = appendListItemChildren(previousId + 'SubMeter' + subMeterCount, false, functions, subMeter.Attributes);
    elementToAppendTo.appendChild(listItem);
  }
}

function commodityMatch(entity, commodity) {
  if(commodity == '') {
      return true;
  }

  var entityCommodities = getAttribute(entity.Attributes, 'Commodities');
  return entityCommodities && entityCommodities.contains(commodity);
}

function appendListItemChildren(id, hasChildren, functions, attributes) {
  var li = document.createElement('li');
  li.appendChild(createBranchDiv(id, hasChildren));
  li.appendChild(createBranchCheckbox(id, functions));
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
    branchDiv.setAttribute('class', (hasChildren ? 'far fa-plus-square show-pointer' : 'far fa-times-circle'));
    branchDiv.setAttribute('style', 'padding-right: 4px;');
    return branchDiv;
}

function createBranchCheckbox(id, functions) {
  var checkbox = document.createElement('input');
  checkbox.type = 'checkbox';  
  checkbox.id = id.concat('checkbox');

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
      case "Weekly":
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
        var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), dateFormat);
  
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

function getNewChartSeries(checkBoxes, showBy, newCategories, commodity, dateFormat, startDate, endDate) {
    var meters = [];
    var newSeries = [];
    var checkBoxesLength = checkBoxes.length;
  
    for(var checkboxCount = 0; checkboxCount < checkBoxesLength; checkboxCount++) {
      var checkboxBranch = checkBoxes[checkboxCount].attributes['Branch'].nodeValue;
      var linkedSite = checkBoxes[checkboxCount].attributes['LinkedSite'].nodeValue;
      var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
      var seriesName = span.innerHTML;
  
      if(checkboxBranch == 'Site') {
        meters = getSitesByAttribute('BaseName', linkedSite)[0].Meters;
      }
      else if(checkboxBranch == 'Meter') {
        meters = getMetersByAttribute('Identifier', span.innerHTML, linkedSite);
      }
      else if(checkboxBranch == 'SubMeter') {
        meters = getSubMetersByAttribute('Identifier', span.innerHTML, linkedSite);
        seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
      }
  
      if(showBy == "MaxDemand") {
        var meterLength = meters.length;

        for(var meterCount = 0; meterCount < meterLength; meterCount++) {
          var meter = meters[meterCount];
      
          if(commodityMeterMatch(meter, commodity)) {
            var meterData = meter['Capacity'];
            
            if(!meterData) {
              continue;
            }

            var site = getSitesByAttribute('BaseName', linkedSite)[0];
            var maxDemand = getAttribute(site.Attributes, 'MaxDemand');      
            meter['MaxDemand'] = JSON.parse(JSON.stringify(meterData));

            var meterDataLength = meterData.length;
            for(var j = 0; j < meterDataLength; j++) {
              meter['MaxDemand'][j].Value = maxDemand;
            }
          }
        }
      }
      
      newSeries.push(summedMeterSeries(meters, seriesName, showBy, newCategories, commodity, dateFormat, startDate, endDate));
    }
  
    return newSeries;
}

function getSitesByAttribute(attribute, value) {
    var sites = []
    var dataLength = data.length;
  
    for(var siteCount = 0; siteCount < dataLength; siteCount++) {
      var site = data[siteCount];
  
      if(getAttribute(site.Attributes, attribute) == value) {
        sites.push(site);
      }
    }
  
    return sites;
}
  
function getMetersByAttribute(attribute, value, linkedSite) {
    var meters = [];
    var dataLength = data.length;
  
    for(var siteCount = 0; siteCount < dataLength; siteCount++) {
      var site = data[siteCount];
      var meterLength = site.Meters.length;
  
      for(var meterCount = 0; meterCount < meterLength; meterCount++) {
        var meter = site.Meters[meterCount];
  
        if(getAttribute(meter.Attributes, attribute) == value) {
          if(linkedSiteMatch(meter.GUID, 'Meter', linkedSite)) {
            meters.push(meter);
          }
        }
      }
    }
  
    return meters;
}

function getSubMetersByAttribute(attribute, value, linkedSite) {
  var subMeters = [];
  var dataLength = data.length;

  for(var siteCount = 0; siteCount < dataLength; siteCount++) {
    var site = data[siteCount];
    var meterLength = site.Meters.length;

    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = site.Meters[meterCount];

      if(meter['SubMeters']){
        var subMeterLength = meter.SubMeters.length;

        for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++){
          var subMeter = meter.SubMeters[subMeterCount];

          if(getAttribute(subMeter.Attributes, attribute) == value) {
            if(linkedSiteMatch(subMeter.GUID, 'SubMeter', linkedSite)) {
              subMeters.push(subMeter);
            }
          }
        }
      }              
    }			
  }

  return subMeters;
}

function linkedSiteMatch(identifier, meterType, linkedSite) {
  var identifierCheckbox = document.getElementById(meterType.concat(identifier).concat('checkbox'));
  var identifierLinkedSite = identifierCheckbox.attributes['linkedSite'].nodeValue;

  return identifierLinkedSite == linkedSite;
}

function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity, dateFormat, startDate, endDate) {
    var meterLength = meters.length;
    var summedMeterSeries = {
      name: seriesName.concat(' - ').concat(showBy),
      data: [0]
    };
    
    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = meters[meterCount];
  
      if(commodityMeterMatch(meter, commodity)) {
        var meterData = meter[showBy];
        
        if(!meterData) {
          continue;
        }
  
        var meterDataLength = meterData.length;
        for(var j = 0; j < meterDataLength; j++) {
          var meterDate = new Date(meterData[j].Date);

          if(meterDate >= startDate && meterDate <= endDate) {
            var formattedDate = formatDate(meterDate, dateFormat);
            var i = newCategories.findIndex(n => n == formattedDate);
            var value = meterData[j].Value;
    
            if(!value && !summedMeterSeries.data[i]){
              summedMeterSeries.data[i] = null;
            }
            else if(value && !summedMeterSeries.data[i]) {
              summedMeterSeries.data[i] = value;
            }
            else if(value && summedMeterSeries.data[i]) {
              summedMeterSeries.data[i] += value;
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