function pageLoad(){  
	createBudgetTree(sites, "alertMessage()");
	createSiteTree(sites, "alertMessage()");
	setupDataGrid();
	displayCharts();
	addExpanderOnClickEvents();
	setOpenExpanders();

	document.onmousemove = function(e) {
		setupSidebarHeight();
		setupSidebar(e);
	  };
	
	  window.onscroll = function() {
		setupSidebarHeight();
	  };
}

function createBudgetTree(sites, functions) {
	var div = document.getElementById('createReviewBudgetTreeDiv');
	clearElement(div);

	var tree = document.createElement('div');
	var ul = createBranchUl('createReviewBudgetTreeDivSelector', false, true);
	tree.appendChild(ul);

	buildSiteBranch(sites, '', ul, functions, true);

	var headerDiv = document.createElement('div');
	headerDiv.setAttribute('class', 'expander-header');

	var headerSpan = document.createElement('span');
	headerSpan.style = "padding-left: 5px;";
	headerSpan.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square show-pointer expander openExpander" id="createReviewBudgetTreeDivSelector"></i>';

	headerDiv.appendChild(headerSpan);
	div.appendChild(headerDiv);
	div.appendChild(tree);

	var exclamationIcon1 = document.createElement('i');
	var exclamationIcon2 = document.createElement('i');
	exclamationIcon1.setAttribute('class', "fas fa-exclamation-circle");
	exclamationIcon2.setAttribute('class', "fas fa-exclamation-circle");
	exclamationIcon1.setAttribute('title', "Budget 2 for Leeds for period 01/04/2021 to 31/03/2022 will be overriden");
	exclamationIcon2.setAttribute('title', "Budget 2 for Manchester for period 01/04/2021 to 31/03/2022 will be overriden");

	var site = document.getElementById("Budget_Site0checkbox");
	var span = document.getElementById("Budget_Site0span");
	span.appendChild(exclamationIcon1);
	site.checked = true;

	site = document.getElementById("Budget_Site1checkbox");
	span = document.getElementById("Budget_Site1span");
	span.appendChild(exclamationIcon2);
	site.checked = true;
}

function createSiteTree(sites, functions) {
	var div = document.getElementById('siteTree');
	var inputs = div.getElementsByTagName('input');
	var checkboxes = getCheckedElements(inputs);
  
	if(checkboxes.length == 0) {
	  checkboxes = getBranchCheckboxes(inputs, 'Site');
	}
  
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
  
	var headerDiv = createHeaderDiv("siteHeader", 'Sites/Meters', true);
	var ul = createBranchUl("siteSelector", false, true);
  
	var breakDisplayListItem = document.createElement('li');
	breakDisplayListItem.innerHTML = '<br>';
	breakDisplayListItem.classList.add('format-listitem');
  
	var recurseSelectionListItem = document.createElement('li');
	recurseSelectionListItem.classList.add('format-listitem');
	recurseSelectionListItem.classList.add('topListItem');
  
	var recurseSelectionCheckbox = createBranchCheckbox('recuseSelectionCheckbox', '', 'recuseSelection', 'checkbox', 'recurseSelection', false);
	var recurseSelectionSpan = createBranchSpan('recurseSelectionSpan', 'Recurse Selection?');
	recurseSelectionListItem.appendChild(recurseSelectionCheckbox);
	recurseSelectionListItem.appendChild(recurseSelectionSpan);
  
	buildSiteBranch(sites, getCommodityOption(), ul, functions);  
  
	div.appendChild(headerDiv);
	ul.appendChild(breakDisplayListItem);
	ul.appendChild(recurseSelectionListItem);
	div.appendChild(ul);
  
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
function buildSiteBranch(sites, commodityOption, elementToAppendTo, functions, isNewBudget) {
	var siteLength = sites.length;
  
	for(var siteCount = 0; siteCount < siteLength; siteCount++) {
	  var site = sites[siteCount];
  
	  if(!commodityMatch(site, commodityOption)) {
		continue;
	  }
  
	  var listItem = appendListItemChildren((isNewBudget ? 'Budget_' : '') + 'Site' + siteCount, site.hasOwnProperty('Areas'), functions, site.Attributes, 'Site');
	  elementToAppendTo.appendChild(listItem);
  
	  if(site.hasOwnProperty('Areas')) {
		var ul = listItem.getElementsByTagName('ul')[0];
		buildAreaBranch(site.Areas, commodityOption, ul, functions, (isNewBudget ? 'Budget_' : '') + 'Site' + siteCount);
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

function deleteBudgets() {
	var budgets = getBudgets().join("<br>");

	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');
	var button = document.getElementById('button');

	button.innerText = "Delete Budget(s)";
	text.innerHTML = "Are you sure you want to delete the following budgets?<br>" + budgets;

    finalisePopup(title, 'Delete Budget?<br><br>', modal, span);
}

function reinstateBudgets() {
	var budgets = getBudgets();

	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');
	var button = document.getElementById('button');

	button.innerText = "Reinstate Budget(s)";
	button.classList.replace('reject', 'approve');

	text.innerHTML = "Please select budgets to reinstate:<br>";
	budgets.forEach(function(budget, i) {
		var checkbox = document.createElement('input');
		checkbox.type = 'checkbox';
		checkbox.id = 'reinstateBudget' + i + 'checkbox';
		checkbox.setAttribute('budgetValue', budget);

		text.innerHTML += checkbox.outerHTML + budget + '<br>';
	});

    finalisePopup(title, 'Reinstate Budget?<br><br>', modal, span);
}

function getBudgets() {
	var div = document.getElementById('budgetList');
	var inputs = div.getElementsByTagName('input');
	var inputLength = inputs.length;
	var budgets = [];

	for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];

		if(input.type.toLowerCase() == 'checkbox' && input.checked) {
			var span = document.getElementById(input.id.replace('checkbox', 'span'));

			if(!span.innerText.startsWith('Add New')) {
				var button = document.getElementById(input.id.replace('checkbox', 'button'));
				budgets.push(button.innerText);
			}
		}
	}

	return budgets;
}

function setupDataGrid() {
	var datagrid = document.getElementById('adjustmentsSpreadsheet');
	clearElement(datagrid);

	var displayData = [];
	var row = {
		area:'Usage', 
		type:'Uplifting',
		amount:'-5',
		amounttype:'%',
		datefrom:'01/04/2021',
		dateto:'31/03/2022',
		actions:'<i class="fas fa-trash-alt show-pointer" title="Delete Adjustment" style="margin-right: 5px;"></i>'
				+ '<i class="fas fa-save show-pointer" title="Save Changes" style="margin-right: 5px;"></i>'
				+ '<i class="fas fa-undo show-pointer" title="Undo Changes" style="margin-right: 5px;"></i>'
	}
	displayData.push(row);

	row = {
		area:'Cost', 
		type:'Set',
		amount:'50,000',
		amounttype:'£',
		datefrom:'01/04/2021',
		dateto:'31/03/2022',
		actions:'<i class="fas fa-trash-alt show-pointer" title="Delete Adjustment" style="margin-right: 5px;"></i>'
				+ '<i class="fas fa-save show-pointer" title="Save Changes" style="margin-right: 5px;"></i>'
				+ '<i class="fas fa-undo show-pointer" title="Undo Changes" style="margin-right: 5px;"></i>'
	}
	displayData.push(row);

	jexcel(datagrid, {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: displayData,
		columns: [
			{type:'text', width:'125px', name:'type', title:'Type'},
			{type:'text', width:'125px', name:'area', title:'Area'},
			{type:'text', width:'125px', name:'amounttype', title:'Amount Type'},
			{type:'text', width:'125px', name:'amount', title:'Amount'},
			{type:'text', width:'125px', name:'datefrom', title:'Date From'},
			{type:'text', width:'125px', name:'dateto', title:'Date To'},
			{type:'text', width:'125px', name:'actions', title:'<i class="fas fa-trash-alt show-pointer" title="Delete All Adjustments"></i>'},
		 ],
		}); 
}

function displayBudget2V2Popup() {
	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');
	var button = document.getElementById('button');

	button.style.display = "none";

	text.innerHTML = "Budget Period: 01/04/2021 to 31/03/2023<br>"
					+ "Created Date: 06/04/2020 12:53:27<br>"
					+ "Sites/Meters:<br>"
					+ "<span style='margin-left: 10px;'>Leeds</span><br>"
					+ "<span style='margin-left: 10px;'>Manchester</span>"

    finalisePopup(title, 'Budget Details<br><br>', modal, span);
}

function displayBudget2V1Popup() {
	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var button = document.getElementById('button');

	button.style.display = "none";

	text.innerHTML = "Budget Period: 01/04/2021 to 31/03/2023<br>"
					+ "Created Date: 01/04/2020 12:53:27<br>"
					+ "Sites/Meters:<br>"
					+ "<span style='margin-left: 10px;'>Leeds</span><br>"
					+ "<span style='margin-left: 10px;'>Manchester</span>"

    finalisePopup(title, 'Budget Details<br><br>', modal, span);
}

function displayBudget1Popup() {
	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var button = document.getElementById('button');

	button.style.display = "none";

	text.innerHTML = "Budget Period: 01/04/2021 to 31/03/2023<br>"
					+ "Created Date: 25/03/2020 12:53:27<br>"
					+ "Sites/Meters:<br>"
					+ "<span style='margin-left: 10px;'>987650</span>"

    finalisePopup(title, 'Budget Details<br><br>', modal, span);
}

function displayCharts()
{
	loadCostChart();
	loadUsageChart();
}

function loadCostChart() {
	var electricityCostSeries = [{
		name: 'Latest BWS Forecast',
		data: [
				45000, 43000, 41000, 39000, 37000, 38000,
				39000, 40000, 41000, 42000, 43000, 44000,
				45000, 46000, 47000, 48000, 49000, 50000,
				51000, 52000, 53000, 54000, 55000, 56000
			  ]
	  }, {
		name: 'Adjusted',
		data: [
				50000, 50000, 50000, 50000, 50000, 50000,
				50000, 50000, 50000, 50000, 50000, 50000,
				45000, 46000, 47000, 48000, 49000, 50000,
				51000, 52000, 53000, 54000, 55000, 56000
			  ]
	  }];
	  var electricityCategories = [
		'04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
		'01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
		'01 2023', '02 2023', '03 2023'
		];
	  var electricityCostOptions = {
		  chart: {
			type: 'line',
			stacked: false
		  },
		  title: {
			text: 'Cost',
			align: 'center'
		  },
		  tooltip: {
			  x: {
			  format: 'dd/MM/yyyy'
			  }
		  },
		  xaxis: {
			  title: {
			  text: ''
			  },
			  labels: {
			  format: 'dd/MM/yyyy'
			  },
			  categories: electricityCategories
		  },
		  yaxis: [{
			title: {
			  text: '£'
			},
			forceNiceScale: true,
			labels: {
			  formatter: function(val) {
				return val.toLocaleString();
			  }
			}
		  }]
		};
	  refreshChart(electricityCostSeries, "#rightHandChart", electricityCostOptions);
}
  
function loadUsageChart() {
	var electricityUsageSeries = [{
	  name: 'Latest BWS Forecast',
	  data: [
				3700, 3800, 3900, 4000, 4100, 4100,
				4000, 3900, 3800, 3700, 3800, 3900,
				4000, 4100, 4200, 4300, 4200, 4100,
				4000, 3900, 3800, 3700, 3800, 3900,
			]
	}, {
	  name: 'Adjusted',
	  data: [
				3515, 3610, 3705, 3800, 3895, 3895,
				3800, 3705, 3610, 3515, 3610, 3705,
				4000, 4100, 4200, 4300, 4200, 4100,
				4000, 3900, 3800, 3700, 3800, 3900,
			]
	}];
	var electricityCategories = [
	  '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
	  '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
	  '01 2023', '02 2023', '03 2023'
	  ];
	var electricityUsageOptions = {
		chart: {
		  type: 'line',
		  stacked: false
		},
		title: {
		  text: 'Usage',
		  align: 'center'
		},
		tooltip: {
			x: {
			format: 'dd/MM/yyyy'
			}
		},
		xaxis: {
			title: {
			text: ''
			},
			labels: {
			format: 'dd/MM/yyyy'
			},
			categories: electricityCategories
		},
		yaxis: [{
		  title: {
			text: 'kWh'
		  },
		  forceNiceScale: true,
		  labels: {
			formatter: function(val) {
			  return val.toLocaleString();
			}
		  }
		}]
	  };
	refreshChart(electricityUsageSeries, "#leftHandChart", electricityUsageOptions);
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
	  title: chartOptions.title,
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
		showForSingleSeries: true,
		showForNullSeries: true,
		showForZeroSeries: true,
		position: 'right',
		onItemClick: {
		  toggleDataSeries: true
		},
		formatter: function(seriesName) {
		  return seriesName + '<br><br>';
		}
	  },
	  series: newSeries,
	  yaxis: chartOptions.yaxis,
	  xaxis: chartOptions.xaxis
	};  
  
	renderChart(chartId, options);
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