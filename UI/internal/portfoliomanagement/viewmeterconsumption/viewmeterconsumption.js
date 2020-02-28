function pageLoad() {
    createTree(data, "DeviceType", "electricityTreeDiv", "electricity", "updateChart(electricityChart)", true);
	createTree(data, "DeviceType", "gasTreeDiv", "gas", "updateChart(gasChart)", true);
	addExpanderOnClickEvents();
	addArrowOnClickEvents();
	addCommoditySelectorOnClickEvent();	

	createBlankChart("#electricityChart", "There's no electricity data to display. Select from the tree to the left to display");
	createBlankChart("#gasChart", "There's no gas data to display. Select from the tree to the left to display");
}

function updateChart(callingElement, chart) {
    var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
    var inputs = treeDiv.getElementsByTagName('input');
    var commodity = chart.id.replace('Chart', '').toLowerCase();
    var checkBoxes = getCheckedCheckBoxes(inputs);		
    
    clearElement(chart);

    if(checkBoxes.length == 0) {
        createBlankChart('#' + commodity + 'Chart', 'There is no ' + commodity + ' data to display. Select from the tree to the left to display');
        return;
    }

    var showBySpan = document.getElementById(commodity.concat('ChartHeaderShowBy'));
    var periodSpan = document.getElementById(commodity.concat('ChartHeaderPeriod'));
    var chartDate = new Date(document.getElementById(commodity.concat('Calendar')).value);
    var newCategories = getNewCategories(periodSpan.children[0].value, chartDate);   
    var newSeries = getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity, getPeriodDateFormat(periodSpan.children[0].value));
    var typeSpan = document.getElementById(commodity.concat('ChartHeaderType'));

    var chartOptions = {
    chart: {
        type: getChartType(typeSpan.children[0].value),
        stacked: typeSpan.children[0].value.includes('Stacked')
    },
    tooltip: {
        x: {
        format: getChartTooltipXFormat(periodSpan.children[0].value)
        }
    },
    yaxis: [{
        title: {
            text: getChartYAxisTitle(showBySpan.children[0].value, commodity)
        },
          show: true
    }],
    xaxis: {
        type: 'datetime',
        title: {
        text: formatDate(chartDate, getChartXAxisTitleFormat(periodSpan.children[0].value))
        },
        labels: {
        format: getChartXAxisLabelFormat(periodSpan.children[0].value)
        },
        min: new Date(newCategories[0]).getTime(),
        max: new Date(newCategories[newCategories.length - 1]).getTime(),
          categories: newCategories
    }
    };

    refreshChart(newSeries, newCategories, '#'.concat(commodity).concat('Chart'), chartOptions);
}

function addCommoditySelectorOnClickEvent() {
    var commoditySelector = document.getElementById("electricityGasSelector");
    commoditySelector.addEventListener('click', function(event) {
        updateClassOnClick("electricityDiv", "listitem-hidden", "")
        updateClassOnClick("gasDiv", "listitem-hidden", "")
    })	
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);
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
            branchDiv.removeAttribute('class', 'far fa-plus-square');
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
        branchDiv.removeAttribute('class', 'far fa-plus-square');
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
        branchDiv.setAttribute('class', 'far fa-plus-square');
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

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
	}
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});

	updateAdditionalControls(element);
	expandAdditionalLists(element);
}

function updateAdditionalControls(element) {
	var additionalcontrols = element.getAttribute('additionalcontrols');

	if(!additionalcontrols) {
		return;
	}

	var listToHide = element.id.concat('List');
	var clickEventFunction = function (event) {
		updateClassOnClick(listToHide, 'listitem-hidden', '')
	};

	var controlArray = additionalcontrols.split(',');
	for(var j = 0; j < controlArray.length; j++) {
		var controlId = controlArray[j];	

		element.addEventListener('click', function (event) {
			var controlElement = document.getElementById(controlId);
			if(hasClass(this, 'fa-minus-square')) {				
				controlElement.addEventListener('click', clickEventFunction, false);
			}
			else {
				controlElement.removeEventListener('click', clickEventFunction);
			}
		});
	}	
}

function expandAdditionalLists(element) {
	var additionalLists = element.getAttribute('additionallists');

	if(!additionalLists) {
		return;
	}

	element.addEventListener('click', function (event) {
		var controlArray = additionalLists.split(',');
		for(var j = 0; j < controlArray.length; j++) {
			var controlId = controlArray[j];
			var controlElement = document.getElementById(controlId);
			updateClass(controlElement, 'listitem-hidden', '');
		}
	});		
}

function addArrowOnClickEvents() {
	var arrows = document.getElementsByClassName('fa-angle-double-down');
	var arrowsLength = arrows.length;
	for(var i=0; i< arrowsLength; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, 'fa-angle-double-down', 'fa-angle-double-up')
			updateClassOnClick(this.id.replace('Arrow', 'SubMenu'), 'listitem-hidden', '')
		});
	}

	arrows = document.getElementsByClassName('fa-angle-double-left');
	arrowsLength = arrows.length;
	for(var i=0; i< arrowsLength; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, 'fa-angle-double-left', 'fa-angle-double-right')
		});
	}

	var arrowHeaders = document.getElementsByClassName('arrow-header');
	var arrowHeadersLength = arrowHeaders.length;
	for(var i=0; i< arrowHeadersLength; i++){
		arrowHeaders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id.concat('Arrow'), 'fa-angle-double-down', 'fa-angle-double-up')
			updateClassOnClick(this.id.concat('SubMenu'), 'listitem-hidden', '')
		});
	}
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
  
    return checkBoxes;
}

function getNewCategories(period, chartDate) {
    var dateFormat = getPeriodDateFormat(period);
    switch(period) {
      case 'Daily':
        return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate()), new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate() + 1), dateFormat);
      case "Weekly":
        return getCategoryTexts(getMonday(chartDate), new Date(getMonday(chartDate).getFullYear(), getMonday(chartDate).getMonth(), getMonday(chartDate).getDate() + 7), dateFormat);
      case "Monthly":
        return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), 1), new Date(chartDate.getFullYear(), chartDate.getMonth() + 1, 1), dateFormat);
      case "Yearly":
        return getCategoryTexts(new Date(chartDate.getFullYear(), 0, 1), endDate = new Date(chartDate.getFullYear() + 1, 0, 1), dateFormat);
    }
}

function getPeriodDateFormat(period) {
    switch(period) {
      case 'Daily':
      case "Weekly":
        return 'yyyy-MM-dd hh:mm:ss';
      case "Monthly":
      case "Yearly":
        return 'yyyy-MM-dd';
    }
}

function getCategoryTexts(startDate, endDate, dateFormat) {
    var newCategories = [];
  
    for(var newDate = startDate; newDate < endDate; newDate.setDate(newDate.getDate() + 1)) {
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
		case 'yyyy-MM-dd':
			var aaaa = baseDate.getFullYear();
			var gg = baseDate.getDate();
			var mm = (baseDate.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return aaaa + '-' + mm + '-' + gg;
		case 'yyyy-MM-dd hh:mm:ss':
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
		
			return formatDate(baseDate, 'yyyy-MM-dd') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return baseDate.getFullYear();
		case 'yyyy-MM-dd to yyyy-MM-dd':
			var startDate = getMonday(baseDate);
			var endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + 6);

			return formatDate(startDate, 'yyyy-MM-dd') + ' to ' + formatDate(endDate, 'yyyy-MM-dd')
	}
}

function getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity, dateFormat) {
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
      else if(checkboxBranch.includes('GroupByOption')) {
        meters = getMetersByAttribute(checkboxBranch.replace('GroupByOption|', ''), span.innerHTML, linkedSite);
        seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
      }
      else if(checkboxBranch.includes('GroupBySubOption')) {
        meters = getMetersByAttribute(checkboxBranch.replace('GroupBySubOption|', ''), span.innerHTML, linkedSite);
        seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
      }
      else if(checkboxBranch == 'Meter') {
        meters = getMetersByAttribute('Identifier', span.innerHTML, linkedSite);
      }
      else if(checkboxBranch == 'SubMeter') {
        meters = getSubMetersByAttribute('Identifier', span.innerHTML, linkedSite);
        seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
      }
  
      newSeries.push(summedMeterSeries(meters, seriesName, showBySpan.children[0].value, newCategories, commodity, dateFormat));
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

function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity, dateFormat) {
    var meterLength = meters.length;
    var summedMeterSeries = {
      name: seriesName,
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
          var i = newCategories.findIndex(n => n == formatDate(meterData[j].Date, dateFormat));
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
  
    return summedMeterSeries;
}

function getChartType(chartType) {
    switch(chartType){
      case 'Line':
      case 'Bar':
      case 'Area':
        return chartType.toLowerCase();
      case 'Stacked Line':
      case 'Stacked Bar':
        return chartType.replace('Stacked ', '').toLowerCase();
    }
}

function getChartXAxisLabelFormat(period) {
    switch(period) {
      case 'Daily':
        return 'HH:mm';
      case "Weekly":
        return 'dd/MM/yyyy';
      case "Monthly":
        return 'dd';
      case "Yearly":
        return 'MMM';
    }
}
  
function getChartTooltipXFormat(period) {
    switch(period) {
      case 'Daily':
      case "Weekly":
      case "Monthly":
        return 'dd/MM/yyyy HH:mm';
      case "Yearly":
        return 'dd/MM/yyyy';
    }
}
  
function getChartXAxisTitleFormat(period) {
    switch(period) {
      case 'Daily':
        return 'yyyy-MM-dd';
      case "Weekly":
        return 'yyyy-MM-dd to yyyy-MM-dd';
      case "Monthly":
        return 'MMM yyyy';
      case "Yearly":
        return 'yyyy';
    }
}

function getChartYAxisTitle(showBy, commodity) {
    switch(showBy) {
      case 'Energy':
        if(commodity == 'Gas') {
          return 'Energy (Thm)';
        }
        return 'Energy (MWh)';
      case 'Power':
        return 'Power (MW)';
      case 'Current':
        return 'Current (A)';
      case 'Cost':
        return 'Cost (£)';
    }
}

function refreshChart(newSeries, newCategories, chartId, chartOptions) {
    var options = {
      chart: {
          height: '100%',
          width: '100%',
        type: chartOptions.chart.type,
        stacked: chartOptions.chart.stacked,
        zoom: {
          type: 'x',
          enabled: true,
          autoScaleYaxis: true
        },
        animations: {
          enabled: false
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
      tooltip: {
        x: {
          format: chartOptions.tooltip.x.format
        }
      },
      legend: {
        show: true,
        position: 'right',
        onItemClick: {
          toggleDataSeries: false
        }
      },
      series: newSeries,
      yaxis: chartOptions.yaxis,
      xaxis: chartOptions.xaxis
    };  
  
    renderChart(chartId, options);
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
	return convertMonthIdToFullText(monthId).slice(0, 3).toUpperCase();
}

function getMonday(date) {
	var mondayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
	var day = mondayDate.getDay() || 7;  

	if( day !== 1 ) {
		mondayDate.setHours(-24 * (day - 1)); 
	}
	
	return mondayDate;
}