function pageLoad() {
	createTree(data, "treeDiv", "", "updateChart(chart)", true);

	document.onmousemove=function(e) {
	  var mousecoords = getMousePos(e);
	  if(mousecoords.x <= 25) {
		openNav();
	  }  
	  else if(mousecoords.x >= 400) {
		closeNav();
	  }  
	};
}

function getMousePos(e) {
	return {x:e.clientX,y:e.clientY};
  }
  
  function openNav() {
	document.getElementById("mySidenav").style.width = "400px";
  }
  
  function closeNav() {
	document.getElementById("mySidenav").style.width = "0px";
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

function getShowBy(callingElement) {
	if(!callingElement || document.getElementById('variance0radio').checked) {
		return 'Forecast';
	}

	var display = '';
	if(document.getElementById('usageCostElement0radio').checked) {
		display = 'Usage';
	}
	else if(document.getElementById('costCostElement0radio').checked) {
		display = 'Cost';
	}
	else if(document.getElementById('rateCostElement0radio').checked) {
		display = 'Rate';
	}

	var type = '';
	var typeSelectorList = document.getElementById('typeSelectorList');
	var inputs = typeSelectorList.getElementsByTagName('input');
	var inputLength = inputs.length;

	for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'radio') {
			if(input.checked) {
				type = input.id.replace('CostElement0radio', '');
				break;
			}
		}
	}
  
	type = 'Wholesale';
	return type + display;
}

function getDataWidthDivider(showBy) {
	switch (showBy) {
		case "WholesaleRate":
		case "WholesaleCost":
		case "WholesaleUsage":
		case "Forecast":
			return 6.17;
	}
}

function getDisplayData(showBy, newSeries, newCategories) {
	var displayData = [];
	var categoryLength = newCategories.length;

	switch (showBy) {
		case "WholesaleRate":
			for(var i = 0; i < categoryLength; i++) {
				var row = {
					month: newCategories[i], 
					latestforecastrate:(newSeries[0]["data"][i] ?? 0).toLocaleString(),
					invoicedrate:(newSeries[1]["data"][i] ?? 0).toLocaleString(),
					ratedifference:(newSeries[2]["data"][i] ?? 0).toLocaleString(),
					duosreductionproject:(newSeries[3]["data"][i] ?? 0).toLocaleString(),
					wastereduction:(newSeries[4]["data"][i] ?? 0).toLocaleString(),
					unknown:(newSeries[5]["data"][i] ?? 0).toLocaleString()
				}
				displayData.push(row);
			}
		case "WholesaleCost":
			for(var i = 0; i < categoryLength; i++) {
				var row = {
					month: newCategories[i], 
					latestforecastcost:(newSeries[0]["data"][i] ?? 0).toLocaleString(),
					invoicedcost:(newSeries[1]["data"][i] ?? 0).toLocaleString(),
					costdifference:(newSeries[2]["data"][i] ?? 0).toLocaleString(),
					duosreductionproject:(newSeries[3]["data"][i] ?? 0).toLocaleString(),
					wastereduction:(newSeries[4]["data"][i] ?? 0).toLocaleString(),
					unknown:(newSeries[5]["data"][i] ?? 0).toLocaleString()
				}
				displayData.push(row);
			}
		case "WholesaleUsage":
			for(var i = 0; i < categoryLength; i++) {
				var row = {
					month: newCategories[i], 
					latestforecastusage:(newSeries[0]["data"][i] ?? 0).toLocaleString(),
					invoicedusage:(newSeries[1]["data"][i] ?? 0).toLocaleString(),
					usagedifference:(newSeries[2]["data"][i] ?? 0).toLocaleString(),
					duosreductionproject:(newSeries[3]["data"][i] ?? 0).toLocaleString(),
					wastereduction:(newSeries[4]["data"][i] ?? 0).toLocaleString(),
					unknown:(newSeries[5]["data"][i] ?? 0).toLocaleString()
				}
				displayData.push(row);
			}
		case "Forecast":
			for(var i = 0; i < categoryLength; i++) {
				var row = {
					month: newCategories[i], 
					latestforecastusage:(newSeries[0]["data"][i] ?? 0).toLocaleString(),
					invoicedusage:(newSeries[1]["data"][i] ?? 0).toLocaleString(),
					usagedifference:(newSeries[2]["data"][i] ?? 0).toLocaleString(),
					latestforecastcost:(newSeries[3]["data"][i] ?? 0).toLocaleString(),
					invoicedcost:(newSeries[4]["data"][i] ?? 0).toLocaleString(),
					costdifference:(newSeries[5]["data"][i] ?? 0).toLocaleString()
				}
				displayData.push(row);
			}
	}

	return displayData;
}

function getColumns(showBy, monthWidth, dataWidth) {
	switch (showBy) {
		case "WholesaleRate":
			return [
				{type:'text', width:monthWidth, name:'month', title:'Month'},
				{type:'text', width:dataWidth, name:'latestforecastrate', title:'Latest Forecast Rate'},
				{type:'text', width:dataWidth, name:'invoicedrate', title:'Invoiced Rate'},
				{type:'text', width:dataWidth, name:'ratedifference', title:'Rate Difference'},
				{type:'text', width:dataWidth, name:'duosreductionproject', title:'DUoS Reduction Project'},
				{type:'text', width:dataWidth, name:'wastereduction', title:'Waste Reduction'},
				{type:'text', width:dataWidth, name:'unknown', title:'Unknown'},
			 ];
		case "WholesaleCost":
			return [
				{type:'text', width:monthWidth, name:'month', title:'Month'},
				{type:'text', width:dataWidth, name:'latestforecastcost', title:'Latest Forecast Cost'},
				{type:'text', width:dataWidth, name:'invoicedcost', title:'Invoiced Cost'},
				{type:'text', width:dataWidth, name:'costdifference', title:'Cost Difference'},
				{type:'text', width:dataWidth, name:'duosreductionproject', title:'DUoS Reduction Project'},
				{type:'text', width:dataWidth, name:'wastereduction', title:'Waste Reduction'},
				{type:'text', width:dataWidth, name:'unknown', title:'Unknown'},
			 ];
		case "WholesaleUsage":
			return [
				{type:'text', width:monthWidth, name:'month', title:'Month'},
				{type:'text', width:dataWidth, name:'latestforecastusage', title:'Latest Forecast Usage'},
				{type:'text', width:dataWidth, name:'invoicedusage', title:'Invoiced Usage'},
				{type:'text', width:dataWidth, name:'usagedifference', title:'Usage Difference'},
				{type:'text', width:dataWidth, name:'duosreductionproject', title:'DUoS Reduction Project'},
				{type:'text', width:dataWidth, name:'wastereduction', title:'Waste Reduction'},
				{type:'text', width:dataWidth, name:'unknown', title:'Unknown'},
			 ];
		case "Forecast":
			return [
				{type:'text', width:monthWidth, name:'month', title:'Month'},
				{type:'text', width:dataWidth, name:'latestforecastusage', title:'Latest Forecast'},
				{type:'text', width:dataWidth, name:'invoicedusage', title:'Invoiced'},
				{type:'text', width:dataWidth, name:'usagedifference', title:'Difference'},
				{type:'text', width:dataWidth, name:'latestforecastcost', title:'Latest Forecast'},
				{type:'text', width:dataWidth, name:'invoicedcost', title:'Invoiced'},
				{type:'text', width:dataWidth, name:'costdifference', title:'Difference'},
			 ];
	}
}

function getNestedHeaders(showBy) {
	switch (showBy) {
		case "WholesaleRate":
		case "WholesaleCost":
		case "WholesaleUsage":
			return [
				[
					{
						title: '',
						colspan: '1',
					},
					{
						title: 'Summary',
						colspan: '3',
					},
					{
						title: 'Reason For Difference',
						colspan: '3',
					},
				],
			];
		case "Forecast":
			return [
				[
					{
						title: '',
						colspan: '1',
					},
					{
						title: 'Usage',
						colspan: '3',
					},
					{
						title: 'Cost',
						colspan: '3',
					},
				],
			];
	}
}

function updateChart(callingElement, chart) {
	var treeDiv = document.getElementById('treeDiv');
	var inputs = treeDiv.getElementsByTagName('input');
	var commodity = getCommodity();
	var checkBoxes = getCheckedCheckBoxes(inputs);
	var showBy = getShowBy(callingElement);
  
	clearElement(chart);
	
	var newCategories = getNewCategories();   
	var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity);
  
	var chartOptions = {
	chart: {
	  type: 'bar',
	  stacked: false
	},
	tooltip: {
	  x: {
	  format: getChartTooltipXFormat()
	  }
	},
	yaxis: [{
	  axisTicks: {
		show: true
	  },
	  axisBorder: {
		show: true,
	  },
	  title: {
		text: ''
	  },
		  show: true,
		  decimalsInFloat: 2
	}],
	xaxis: {
	  type: 'category',
	  title: {
		text: ''
	  },
	  min: new Date(newCategories[0]).getTime(),
	  max: new Date(newCategories[newCategories.length - 1]).getTime(),
		  categories: newCategories
	}
	};
  
	refreshChart(newSeries, '#chart', chartOptions);
	updateDataGrid(showBy, newSeries, newCategories);
  }

function updateDataGrid(showBy, newSeries, newCategories) {
	var datagridDiv = document.getElementById('datagridDiv');
	var datagridDivWidth = datagridDiv.clientWidth;
	var monthWidth = Math.floor(datagridDivWidth/10);
	var dataWidthDivider = getDataWidthDivider(showBy);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/dataWidthDivider)-1;

	var datagrid = document.getElementById('datagrid');
	clearElement(datagrid);

	jexcel(datagrid, {
		pagination:12,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: getDisplayData(showBy, newSeries, newCategories),
		columns: getColumns(showBy, monthWidth, dataWidth),
		nestedHeaders: getNestedHeaders(showBy)
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
  
  function refreshChart(newSeries, chartId, chartOptions) {
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
  
function getCheckedCheckBoxes(inputs) {
var checkBoxes = [];
var inputLength = inputs.length;

for(var i = 0; i < inputLength; i++) {
	var input = inputs[i];
	if(input.type.toLowerCase() == 'checkbox') {
	if(input.checked) {
		if(input.attributes['Branch'].nodeValue == 'Meter') {
		if(!checkBoxes.includes(input)) {
			checkBoxes.push(input);
		}          
		}

		if(input.attributes['Branch'].nodeValue == 'Site') {
		var linkedSite = input.attributes['LinkedSite'].nodeValue;

		for(var j = 0; j< inputLength; j++) {
			var meterInput = inputs[j];

			if(meterInput.type.toLowerCase() == 'checkbox'
			&& meterInput.attributes['Branch'].nodeValue == 'Meter'
			&& meterInput.attributes['LinkedSite'].nodeValue == linkedSite) {
			if(!checkBoxes.includes(meterInput)) {
				checkBoxes.push(meterInput);
			} 
			}
		}
		}
	}
	}
}

if(checkBoxes.length == 0) {
	for(var i = 0; i < inputLength; i++) {
	var input = inputs[i];
	if(input.type.toLowerCase() == 'checkbox') {
		if(input.attributes['Branch'].nodeValue == 'Meter') {
		checkBoxes.push(input);
		}
	}
	}
}

return checkBoxes;
}
  
function getNewChartSeries(checkBoxes, showBy, newCategories, commodity) {
var meters = [];
var newSeries = [];
var checkBoxesLength = checkBoxes.length;

for(var checkboxCount = 0; checkboxCount < checkBoxesLength; checkboxCount++) {
	var linkedSite = checkBoxes[checkboxCount].attributes['LinkedSite'].nodeValue;
	var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));

	meters.push(getMetersByAttribute('Identifier', span.innerHTML, linkedSite));
}

var series = getSeries(showBy);

for(var i = 0; i < series.length; i++) {
	newSeries.push(summedMeterSeries(meters, series[i], showBy, newCategories, commodity));
}

return newSeries;
}
  
function getSeries(showBy) {
	switch(showBy) {
		case "WholesaleUsage":
			return ["Latest Forecast Usage","Invoiced Usage","Usage Difference","DUoS Reduction Project","Waste Reduction","Unknown"];
		case "WholesaleCost":
			return ["Latest Forecast Cost","Invoiced Cost","Cost Difference","DUoS Reduction Project","Waste Reduction","Unknown"];
		case "WholesaleRate":
			return ["Latest Forecast Rate","Invoiced Rate","Rate Difference","Usage Change","DUoS Reduction Project","Waste Reduction","Unknown"];
		case "Forecast":
			return ["Latest Forecast Usage","Invoiced Usage","Usage Difference","Latest Forecast Cost","Invoiced Cost","Cost Difference"];
	}
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

return meters[0];
}
  
function linkedSiteMatch(identifier, meterType, linkedSite) {
var identifierCheckbox = document.getElementById(meterType.concat(identifier).concat('checkbox'));
var identifierLinkedSite = identifierCheckbox.attributes['LinkedSite'].nodeValue;

return identifierLinkedSite == linkedSite;
}
  
function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity) {
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
		var i = newCategories.findIndex(n => n == meterData[j][0].Month);
		var value = getAttribute(meterData[j], seriesName);

		if(!value && !summedMeterSeries.data[i]){
		summedMeterSeries.data[i] = null;
		}
		else if(value && !summedMeterSeries.data[i]) {
		summedMeterSeries.data[i] = preciseRound(value,2);
		}
		else if(value && summedMeterSeries.data[i]) {
		summedMeterSeries.data[i] += preciseRound(value,2);
		}							     
	}
	}
}

return summedMeterSeries;
}
  
function getNewCategories() {
return getCategoryTexts(new Date(2018, 12, 1), new Date(2019, 12, 1), 'MMM yyyy');
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
  
function getChartTooltipXFormat() {
return 'MMM yyyy';
}
  
function getChartXAxisLabelFormat() {
return 'MMM yyyy';
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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
	var ul = createUL();
	ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
	div.appendChild(tree);
	
	updateChart(null, chart);
	addExpanderOnClickEvents();
}

function buildTree(baseData, baseElement, commodity, checkboxFunction, showSubMeters) {
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
        
        if(base.hasOwnProperty('Meters')) {
            buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName, showSubMeters);
            childrenCreated = true;
        }

        appendListItemChildren(li, 'site'.concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, childrenCreated);

        baseElement.appendChild(li);        
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

	updateClassOnClick('treeDivSelector', 'fa-plus-square', 'fa-minus-square');
	updateClassOnClick('commoditySelector', 'fa-plus-square', 'fa-minus-square');
	updateClassOnClick('displaySelector', 'fa-plus-square', 'fa-minus-square');
	updateClassOnClick('typeSelector', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
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

function preciseRound(num, dec){
	if ((typeof num !== 'number') || (typeof dec !== 'number')) {
		return false; 
	}	

	var num_sign = num >= 0 ? 1 : -1;
		
	return Number((Math.round((num*Math.pow(10,dec))+(num_sign*0.0001))/Math.pow(10,dec)).toFixed(dec));
}