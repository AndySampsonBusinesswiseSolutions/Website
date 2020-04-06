function pageLoad() {
  createTree(data, "DeviceType", "siteDiv", "", "updateCharts()", true);
  addExpanderOnClickEvents(document);
  updateClassOnClick('usage', 'fa-plus-square', 'fa-minus-square');

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
    newSeries.push(...getNewChartSeries(checkBoxes, showByArray[i], newCategories, getCommodity(), dateFormat, startDate, endDate));
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
    // case 'Daily':
    // case 'Weekly':
      return 'datetime';
    default:
      return 'category';
  }
}

function getCommodity() {
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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square show-pointer"" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
    div.appendChild(tree);
}

function buildTree(baseData, groupByOption, baseElement, commodity, checkboxFunction, showSubMeters) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];

        if(!commoditySiteMatch(base, commodity)) {
            continue;
        }
        
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();
        var childrenCreated = false;
        
        if(groupByOption == 'Hierarchy') {
            if(base.hasOwnProperty('Meters')) {
                buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName, showSubMeters);
                childrenCreated = true;
            }

        }
        else {
            buildBranch(base.Meters, groupByOption, ul, commodity, checkboxFunction, baseName, showSubMeters);
            childrenCreated = true;
        }
        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, childrenCreated);

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var branchOptions = getBranchOptions(meters, groupByOption, commodity);
    var branchId;
    var groupBySubOption;

    switch (groupByOption) {
        case 'DeviceType':
            branchId = commodity.concat('DeviceType');
            groupBySubOption = 'DeviceSubType';
            break;
        case 'Zone':
            branchId = commodity.concat('Zone');
            groupBySubOption = 'Panel';
            break;
    }

    var branchOptionsLength = branchOptions.length;
    for(var i = 0; i < branchOptionsLength; i++) {
        var branchOption = branchOptions[i];
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupByOption, branchOption, commodity);
        var ul = createUL();
        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity, checkboxFunction, linkedSite, showSubMeters);
        appendListItemChildren(li, branchId.concat(branchCount), checkboxFunction, 'GroupByOption|'.concat(groupByOption), branchOption, commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var branchOptions = getBranchOptions(meters, groupBySubOption, commodity);
    var branchId; 

    switch (groupBySubOption) {
        case 'DeviceSubType':
            branchId = commodity.concat('DeviceSubType');
            break;
        case 'Panel':
            branchId = commodity.concat('Panel');
            break;
    }

    var branchOptionsLength = branchOptions.length;
    for(var i = 0; i < branchOptionsLength; i++) {
        var branchOption = branchOptions[i];
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupBySubOption, branchOption, commodity);
        var ul = createUL();
        buildIdentifierHierarchy(matchedMeters, ul, commodity, checkboxFunction, linkedSite, showSubMeters);
        appendListItemChildren(li, branchId.concat(subBranchCount), checkboxFunction, 'GroupBySubOption|'.concat(groupBySubOption), branchOption, commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, childrenCreated) {
    li.appendChild(createBranchDiv(id, childrenCreated));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(!commodityMeterMatch(meter, commodity)) {
            continue;
        }

        var meterAttributes = meter.Attributes;
        var identifier = getAttribute(meterAttributes, 'Identifier');
        var meterCommodity = getAttribute(meterAttributes, 'Commodity');
        var deviceType = getAttribute(meterAttributes, 'DeviceType');
        var hasSubMeters = meter.hasOwnProperty('SubMeters');
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meter.GUID);
        var branchDiv = createBranchDiv(branchId);
        
        if(!showSubMeters || !hasSubMeters) {
            branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer');
            branchDiv.setAttribute('class', 'far fa-times-circle');
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID));
        li.appendChild(createTreeIcon(deviceType, meterCommodity));
        li.appendChild(createSpan(branchId, identifier));

        if(showSubMeters && hasSubMeters) {
            var ul = createUL();
            buildSubMeterHierarchy(meter['SubMeters'], ul, deviceType, meterCommodity, checkboxFunction, linkedSite);

            li.appendChild(createBranchListDiv(branchId.concat('List'), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity, checkboxFunction, linkedSite) {
    var subMetersLength = subMeters.length;
    for(var i = 0; i < subMetersLength; i++){
        var subMeter = subMeters[i];
        var li = document.createElement('li');

        var identifier = getAttribute(subMeter.Attributes, 'Identifier');
        var branchDiv = createBranchDiv(subMeter.GUID);
        branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox('SubMeter'.concat(subMeter.GUID), checkboxFunction, 'SubMeter', linkedSite, subMeter.GUID));
        li.appendChild(createTreeIcon(deviceType, commodity));
        li.appendChild(createSpan('SubMeter'.concat(subMeter.GUID), identifier));   

        baseElement.appendChild(li); 
    }
}

function getBranchOptions(meters, property, commodity) {
    var branchOptions = [];
    var metersLength = meters.length;

    for(var i = 0; i < metersLength; i++) {
        var meter = meters[i];
        var attribute = getAttribute(meter.Attributes, property);
        if(!branchOptions.includes(attribute)
            && commodityMeterMatch(meter, commodity)) {
            branchOptions.push(attribute);
        }        
    }

    return branchOptions;
}

function createBranchDiv(branchDivId, childrenCreated = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(childrenCreated) {
        branchDiv.setAttribute('class', 'far fa-plus-square show-pointer');
    }

    branchDiv.setAttribute('style', 'padding-right: 4px;');
    return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
    var branchListDiv = document.createElement('div');
    branchListDiv.id = branchListDivId;
    branchListDiv.setAttribute('class', 'listitem-hidden');
    branchListDiv.appendChild(ul);
    return branchListDiv;
}

function createUL() {
    var ul = document.createElement('ul');
    ul.setAttribute('class', 'format-listitem');
    return ul;
}

function createTreeIcon(branch, commodity) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch, commodity));
    icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
    return icon;
}

function createSpan(spanId, innerHTML) {
    var span = document.createElement('span');
    span.id = spanId.concat('span');
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid) {
    var functionArray = checkboxFunction.replace(')', '').split('(');
    var functionArrayLength = functionArray.length;
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement('input');
    checkBox.type = 'checkbox';  
    checkBox.id = checkboxId.concat('checkbox');
    checkBox.setAttribute('Branch', branch);
    checkBox.setAttribute('LinkedSite', linkedSite);
    checkBox.setAttribute('GUID', guid);

    functionArguments.push(checkBox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function commoditySiteMatch(site, commodity) {
    if(commodity == '') {
        return true;
    }

    if(!site.hasOwnProperty('Meters')) {
        return false;
    }

    var metersLength = site.Meters.length;
    for(var i = 0; i < metersLength; i++) {
        if(commodityMeterMatch(site.Meters[i], commodity)) {
            return true;
        }
    }

    return false;
}

function commodityMeterMatch(meter, commodity) {
    if(commodity == '') {
        return true;
    }

    var meterCommodity = getAttribute(meter.Attributes, 'Commodity');
    return meterCommodity.toLowerCase() == commodity.toLowerCase();
}

function getIconByBranch(branch, commodity) {
    switch (branch) {
        case 'Mains':
            if(commodity == 'Gas') {
                return 'fas fa-burn';
            }
            else {
                return 'fas fa-plug';
            }
        case 'Lighting':
            return 'fas fa-lightbulb';
        case 'Unknown':
            return 'fas fa-question-circle';
        default:
            return 'fas fa-map-marker-alt';
    }
}

function getMatchedMeters(meters, attribute, branchOption, commodity) {
    var matchedMeters = [];
    var metersLength = meters.length;

    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(getAttribute(meter.Attributes, attribute) == branchOption
            && commodityMeterMatch(meter, commodity)) {
            matchedMeters.push(meter);
        }
    }

    return matchedMeters;
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

function addExpanderOnClickEvents(element) {
	var expanders = element.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
  }
  
  updateClassOnClick('siteDivSelector', 'fa-plus-square', 'fa-minus-square');
	updateClassOnClick('commoditySelector', 'fa-plus-square', 'fa-minus-square');
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