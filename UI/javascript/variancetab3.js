function openTab(tabName, cardDivName) {
	var cardDiv = document.getElementById(cardDivName);
	clearElement(cardDiv);

	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName.concat('div');
	cardDiv.appendChild(newDiv);
	document.getElementById(newDiv.id).style.display = "block";

	if(tabName == 'Wholesale') {
		if(document.getElementById('usageCostElement0radio').checked) {
			tabName = 'WholesaleUsage';
		}
		else if(document.getElementById('costCostElement0radio').checked) {
			tabName = 'WholesaleCost';
		}
		else if(document.getElementById('rateCostElement0radio').checked) {
			tabName = 'WholesaleRate';
		}
		else {
			tabName = 'Forecast';
		}
	}
	else if(tabName == 'Usage') {
		tabName = 'WholesaleUsage';
	}
	else if(tabName == 'Cost') {
		tabName = 'WholesaleCost';
	}
	else if(tabName == 'Rate') {
		tabName = 'WholesaleRate';
	}

	createCard(newDiv, tabName);
  }

function createCardButtons(){
	openTab("Forecast", "cardDiv");	
	updateChart(document.getElementById('variance0radio'), electricityChart);
}

function createCard(divToAppendTo, tabName) {
	switch(tabName) {
		case "WholesaleUsage":
			buildWholesaleUsageForm(divToAppendTo);
			break;
		case "WholesaleCost":
			buildWholesaleCostForm(divToAppendTo);
			break;
		case "WholesaleRate":
			buildWholesaleRateForm(divToAppendTo);
			break;
		case "RenewablesObligationUsage":
			buildRenewablesUsageForm(divToAppendTo);
			break;
		case "DistributionUsage":
		case "DistributionCost":
		case "DistributionRate":
		case "RenewablesCost":
		case "RenewablesRate":
		case "BalancingUsage":
		case "BalancingCost":
		case "BalancingRate":
		case "OtherUsage":
		case "OtherCost":
		case "OtherRate":
		case "Forecast":
			buildForecastForm(divToAppendTo);
			break;
		case "CostElements":
			buildCostElementHeaderForm(divToAppendTo);
			createCostElementCardButtons();
			break;
		case "Wholesale":
		case "RenewablesObligation":
		case "Balancing":
		case "Other":
			buildCostElementHeaderSubForm(divToAppendTo);
			createCostElementDetailCardButtons(tabName);
			break;
		case "Network":
			buildCostElementDetailHeaderSubForm(divToAppendTo);
			createNetworkCostElementCardButtons(tabName);
			break;
		case "Renewables":
			buildCostElementDetailHeaderSubForm(divToAppendTo);
			createRenewablesCostElementCardButtons(tabName);
			break;
	}
}

function createDisplayAttributesDiv(divToAppendTo, id) {
	var div = document.createElement('div');
	div.id = id;
	divToAppendTo.appendChild(div);

	var treeDiv = document.getElementById(id);
	clearElement(treeDiv);

	return treeDiv;
}

function buildForecastForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayAttributes');	

	var treeDivWidth = treeDiv.clientWidth;
	var monthWidth = Math.floor(treeDivWidth/10);
	var dataWidth = Math.floor((treeDivWidth - monthWidth)/6)-1;

	var html = 
		'<div>'+
			'<div class="chart">'+
				'<div id="electricityChart">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div class="datagrid scrolling-wrapper">'+
				'<table>'+
					'<tr>'+
						'<th style="width: '+monthWidth+'px; border-right: solid black 1px;">Month</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Cost</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Cost</th>'+
						'<th style="width: '+dataWidth+'px;">Cost Difference</th>'+
					'</tr>'+
					'<tr><td style="border-right: solid black 1px;">2019-01</td></tr>'+
				'</table>'+
			'</div>'+
		'</div>'

	treeDiv.innerHTML = html;
}

function buildForecastForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayAttributes');	

	var treeDivWidth = treeDiv.clientWidth;
	var monthWidth = Math.floor(treeDivWidth/10);
	var dataWidth = Math.floor((treeDivWidth - monthWidth)/6)-1;

	var html = 
		'<div>'+
			'<div class="chart">'+
				'<div id="electricityChart">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div class="datagrid scrolling-wrapper">'+
				'<table>'+
					'<tr>'+
						'<th style="width: '+monthWidth+'px; border-right: solid black 1px;">Month</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Cost</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Cost</th>'+
						'<th style="width: '+dataWidth+'px;">Cost Difference</th>'+
					'</tr>'+
					'<tr><td style="border-right: solid black 1px;">2019-01</td></tr>'+
				'</table>'+
			'</div>'+
		'</div>'

	treeDiv.innerHTML = html;
}

function buildWholesaleUsageForm(divToAppendTo) {
	var datagridDiv = createDisplayAttributesDiv(divToAppendTo, 'displayAttributes');	

	var html = 
		'<div>'+
			'<div class="chart">'+
				'<div id="electricityChart">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div id="datagrid" class="datagrid scrolling-wrapper">'+
			'</div></div>';

	datagridDiv.innerHTML = html;

	updateWholesaleUsageDatagrid();
}

function updateWholesaleUsageDatagrid() {
	var datagridDiv = document.getElementById('datagrid');
	clearElement(datagridDiv);

	var datagridDivWidth = datagridDiv.clientWidth;
	var monthWidth = Math.floor(datagridDivWidth/10);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/6)-1;

	var treeDiv = document.getElementById('electricityTreeDiv');
	var inputs = treeDiv.getElementsByTagName('input');
	var commodity = 'electricity';
	var checkBoxes = getCheckedCheckBoxes(inputs);

	var html = 
				'<table>'+
					'<tr>'+
						'<th style="width: '+monthWidth+'px; border-right: solid black 1px; border-bottom: solid black 1px;"></th>'+
						'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Summary</th>'+
						'<th style="border-bottom: solid black 1px;" colspan="3">Reason For Difference</th>'+
					'</tr>'+
					'<tr>'+
						'<th style="border-right: solid black 1px;">Month</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">DUoS Reduction Project</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Waste Reduction</th>'+
						'<th style="width: '+dataWidth+'px;">Unknown</th>'+
					'</tr>';

	var showBy = 'WholesaleUsage';
	var newCategories = getNewCategories();   
	var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity);

	var categoryLength = newCategories.length;
	for(var i = 0; i < categoryLength; i++) {
		var htmlRow = '<tr>'+
						'<td style="border-right: solid black 1px;">'+newCategories[i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[0]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[1]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[2]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[3]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[4]["data"][i]+'</td>'+
						'<td>'+newSeries[5]["data"][i]+'</td>'+
					  '</tr>';

		html += htmlRow;
	}
	
	html += '</table>';

	datagridDiv.innerHTML = html;
}

function buildWholesaleCostForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayAttributes');	

	var html = 
		'<div>'+
			'<div class="chart">'+
				'<div id="electricityChart">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div id="datagrid" class="datagrid scrolling-wrapper">'+
			'</div></div>';

	treeDiv.innerHTML = html;

	updateWholesaleCostDatagrid()
}

function updateWholesaleCostDatagrid() {
	var datagridDiv = document.getElementById('datagrid');
	clearElement(datagridDiv);

	var datagridDivWidth = datagridDiv.clientWidth;
	var monthWidth = Math.floor(datagridDivWidth/10);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/6)-1;

	var treeDiv = document.getElementById('electricityTreeDiv');
	var inputs = treeDiv.getElementsByTagName('input');
	var commodity = 'electricity';
	var checkBoxes = getCheckedCheckBoxes(inputs);

	var html = 
			'<table>'+
				'<tr>'+
					'<th style="width: '+monthWidth+'px; border-right: solid black 1px; border-bottom: solid black 1px;"></th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Summary</th>'+
					'<th style="border-bottom: solid black 1px;" colspan="3">Reason For Difference</th>'+
				'</tr>'+
				'<tr>'+
					'<th style="border-right: solid black 1px;">Month</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Cost</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Cost</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Cost Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">DUoS Reduction Project</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Waste Reduction</th>'+
					'<th style="width: '+dataWidth+'px;">Unknown</th>'+
				'</tr>';

	var showBy = 'WholesaleCost';
	var newCategories = getNewCategories();   
	var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity);

	var categoryLength = newCategories.length;
	for(var i = 0; i < categoryLength; i++) {
		var htmlRow = '<tr>'+
						'<td style="border-right: solid black 1px;">'+newCategories[i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[0]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[1]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[2]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[3]["data"][i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+newSeries[4]["data"][i]+'</td>'+
						'<td>'+newSeries[5]["data"][i]+'</td>'+
					  '</tr>';

		html += htmlRow;
	}
	
	html += '</table>';

	datagridDiv.innerHTML = html;
}

function buildWholesaleRateForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayAttributes');	

	var html = 
		'<div>'+
			'<div class="chart">'+
				'<div id="electricityChart">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div id="datagrid" class="datagrid scrolling-wrapper">'+
			'</div></div>'

	treeDiv.innerHTML = html;

	updateWholesaleRateDatagrid();
}

function updateWholesaleRateDatagrid() {
	var datagridDiv = document.getElementById('datagrid');
	clearElement(datagridDiv);

	var datagridDivWidth = datagridDiv.clientWidth;
	var monthWidth = Math.floor(datagridDivWidth/10);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/7)-1;

	var treeDiv = document.getElementById('electricityTreeDiv');
	var inputs = treeDiv.getElementsByTagName('input');
	var commodity = 'electricity';
	var checkBoxes = getCheckedCheckBoxes(inputs);

	var html = 
			'<table>'+
				'<tr>'+
					'<th style="width: '+monthWidth+'px; border-right: solid black 1px; border-bottom: solid black 1px;"></th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Summary</th>'+
					'<th style="border-bottom: solid black 1px;" colspan="4">Reason For Difference</th>'+
				'</tr>'+
				'<tr>'+
					'<th style="border-right: solid black 1px;">Month</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Rate</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Rate</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Rate Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Change</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">DUoS Reduction Project</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Waste Reduction</th>'+
					'<th style="width: '+dataWidth+'px;">Unknown</th>'+
				'</tr>';

	var showBy = 'WholesaleRate';
	var newCategories = getNewCategories();   
	var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity);

	var categoryLength = newCategories.length;
	for(var i = 0; i < categoryLength; i++) {
		var htmlRow = '<tr>'+
						'<td style="border-right: solid black 1px;">'+newCategories[i]+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[0]["data"][i],2)+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[1]["data"][i],2)+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[2]["data"][i],2)+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[3]["data"][i],2)+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[4]["data"][i],2)+'</td>'+
						'<td style="border-right: solid black 1px;">'+preciseRound(newSeries[5]["data"][i],2)+'</td>'+
						'<td>'+preciseRound(newSeries[6]["data"][i],2)+'</td>'+
					  '</tr>';

		html += htmlRow;
	}
	
	html += '</table>';

	datagridDiv.innerHTML = html;
}