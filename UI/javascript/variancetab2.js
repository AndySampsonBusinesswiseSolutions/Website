function openTab(evt, tabName, cardDivName) {
	var cardDiv = document.getElementById(cardDivName);

	var tabContent = cardDiv.getElementsByClassName("tabcontent");
	var tabContentLength = tabContent.length;
	for (var i = 0; i < tabContentLength; i++) {
		if(tabContent[i]) {
			cardDiv.removeChild(tabContent[i]);
		}
	}

	var tabLinks = cardDiv.getElementsByClassName("tablinks");
	var tabLinksLength = tabLinks.length;
	for (var i = 0; i < tabLinksLength; i++) {
		var tabLink = tabLinks[i];
		tabLink.classList.remove('active');
	}
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName.concat('div');
	cardDiv.appendChild(newDiv);
	document.getElementById(newDiv.id).style.display = "block";

	createCard(newDiv, tabName);
	evt.currentTarget.className += " active";
  }

function createCardButtons(){
	var cardDiv = document.getElementById('cardDiv');
	var tabDiv = document.getElementById('tabDiv');

	cardDiv.setAttribute('style', '');

	var button = document.createElement('button');
	button.setAttribute('class', 'tablinks');
	button.setAttribute('onclick', 'openTab(event, "Forecast", "cardDiv"); updateChart(event, electricityChart)');
	button.innerHTML = 'Forecast v Invoice';
	button.id = 'Forecast';
	tabDiv.appendChild(button);

	var button2 = document.createElement('button');
	button2.setAttribute('class', 'tablinks');
	button2.setAttribute('onclick', 'openTab(event, "CostElements", "cardDiv")');
	button2.innerHTML = 'Cost Elements';
	button2.id = 'CostElements';
	tabDiv.appendChild(button2);

	updateTabDiv(tabDiv);
	openTab(event, "Forecast", "cardDiv");	
	button.className += " active";
	updateChart(button, electricityChart);
}

function createCostElementCardButtons(){
	var cardDiv = document.getElementById('costElementCardDiv');
	var tabDiv = document.getElementById('costElementTabDiv');

	cardDiv.setAttribute('style', '');

	var networkButton = document.createElement('button');
	networkButton.setAttribute('class', 'tablinks');
	networkButton.setAttribute('onclick', 'openTab(event, "Network", "costElementCardDiv")');
	networkButton.innerHTML = 'Network';
	networkButton.id = 'Network';
	tabDiv.appendChild(networkButton);

	var renewablesButton = document.createElement('button');
	renewablesButton.setAttribute('class', 'tablinks');
	renewablesButton.setAttribute('onclick', 'openTab(event, "Renewables", "costElementCardDiv"); updateChart(event, electricityChart)');
	renewablesButton.innerHTML = 'Renewables';
	renewablesButton.id = 'Renewables';
	tabDiv.appendChild(renewablesButton);

	var balancingButton = document.createElement('button');
	balancingButton.setAttribute('class', 'tablinks');
	balancingButton.setAttribute('onclick', 'openTab(event, "Balancing", "costElementCardDiv"); updateChart(event, electricityChart)');
	balancingButton.innerHTML = 'Balancing';
	balancingButton.id = 'Balancing';
	tabDiv.appendChild(balancingButton);

	var otherButton = document.createElement('button');
	otherButton.setAttribute('class', 'tablinks');
	otherButton.setAttribute('onclick', 'openTab(event, "Other", "costElementCardDiv"); updateChart(event, electricityChart)');
	otherButton.innerHTML = 'Other';
	otherButton.id = 'Other';
	tabDiv.appendChild(otherButton);

	updateTabDiv(tabDiv);
	openTab(event, "Network", "costElementCardDiv");
	networkButton.className += " active";
}

function createCostElementDetailCardButtons(tabName){
	var cardDiv = document.getElementById('costElementDetailDataCardDiv');
	var tabDiv = document.getElementById('costElementDetailDataTabDiv');

	cardDiv.setAttribute('style', '');

	var usageButton = document.createElement('button');
	usageButton.setAttribute('class', 'tablinks');
	usageButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Usage", "costElementDetailDataCardDiv"); updateChart(event, electricityChart)');
	usageButton.innerHTML = 'Usage';
	usageButton.id = tabName+'Usage';
	tabDiv.appendChild(usageButton);

	var costButton = document.createElement('button');
	costButton.setAttribute('class', 'tablinks');
	costButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Cost", "costElementDetailDataCardDiv"); updateChart(event, electricityChart)');
	costButton.innerHTML = 'Cost';
	costButton.id = tabName+'Cost';
	tabDiv.appendChild(costButton);

	var rateButton = document.createElement('button');
	rateButton.setAttribute('class', 'tablinks');
	rateButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Rate", "costElementDetailDataCardDiv"); updateChart(event, electricityChart)');
	rateButton.innerHTML = 'Rate';
	rateButton.id = tabName+'Rate';
	tabDiv.appendChild(rateButton);

	updateTabDiv(tabDiv);
	openTab(event, tabName+'Usage', "costElementDetailDataCardDiv");
	usageButton.className += " active";
	updateChart(usageButton, electricityChart);
}

function createNetworkCostElementCardButtons(){
	var cardDiv = document.getElementById('costElementDetailCardDiv');
	var tabDiv = document.getElementById('costElementDetailTabDiv');

	cardDiv.setAttribute('style', '');

	var wholesaleButton = document.createElement('button');
	wholesaleButton.setAttribute('class', 'tablinks');
	wholesaleButton.setAttribute('onclick', 'openTab(event, "Wholesale", "costElementDetailCardDiv")');
	wholesaleButton.innerHTML = 'Wholesale';
	wholesaleButton.id = 'Wholesale';
	tabDiv.appendChild(wholesaleButton);

	var distributionButton = document.createElement('button');
	distributionButton.setAttribute('class', 'tablinks');
	distributionButton.setAttribute('onclick', 'openTab(event, "Distribution", "costElementDetailCardDiv")');
	distributionButton.innerHTML = 'Distribution Use of Systems';
	distributionButton.id = 'Distribution';
	tabDiv.appendChild(distributionButton);

	var transmissionButton = document.createElement('button');
	transmissionButton.setAttribute('class', 'tablinks');
	transmissionButton.setAttribute('onclick', 'openTab(event, "Transmission", "costElementDetailCardDiv")');
	transmissionButton.innerHTML = 'Transmission Use of Systems';
	transmissionButton.id = 'Transmission';
	tabDiv.appendChild(transmissionButton);

	updateTabDiv(tabDiv);
	openTab(event, "Wholesale", "costElementDetailCardDiv");
	wholesaleButton.className += " active";
}

function updateTabDiv(tabDiv) {
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

    tabDivChildren[0].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px;'));
    for(var i = 1; i < tabDivChildrenLength; i++) {
        tabDivChildren[i].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px; border-left: solid black 1px;'));
    }
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
		case "RenewablesUsage":
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
		case "Renewables":
		case "Balancing":
		case "Other":
			buildCostElementHeaderSubForm(divToAppendTo);
			createCostElementDetailCardButtons(tabName);
			break;
		case "Network":
			buildCostElementDetailHeaderSubForm(divToAppendTo);
			createNetworkCostElementCardButtons(tabName);
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

function buildCostElementHeaderForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayCostElements');

	var html = 
		'<div>'+
			'<div class="group-by-div" id="costElementCardDiv" style="display: none;">'+
				'<div class="tabDiv" id="costElementTabDiv" style="overflow-y: auto; overflow: auto;"></div>'+
			'</div>'+
		'</div>'
		treeDiv.innerHTML = html;
}

function buildCostElementDetailHeaderSubForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayCostElementDetail');

	var html = 
		'<div>'+
			'<div class="group-by-div" id="costElementDetailCardDiv" style="display: none;">'+
				'<div class="tabDiv" id="costElementDetailTabDiv" style="overflow-y: auto; overflow: auto;"></div>'+
			'</div>'+
		'</div>'
	treeDiv.innerHTML = html;
}

function buildCostElementHeaderSubForm(divToAppendTo) {
	var treeDiv = createDisplayAttributesDiv(divToAppendTo, 'displayCostElementDetailData');

	var html = 
		'<div>'+
			'<div class="group-by-div" id="costElementDetailDataCardDiv" style="display: none;">'+
				'<div class="tabDiv" id="costElementDetailDataTabDiv" style="overflow-y: auto; overflow: auto;"></div>'+
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

function buildRenewablesUsageForm(divToAppendTo) {
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

	updateRenewablesUsageDatagrid();
}

function updateRenewablesUsageDatagrid() {
	var datagridDiv = document.getElementById('datagrid');
	clearElement(datagridDiv);

	var datagridDivWidth = datagridDiv.clientWidth;
	var monthWidth = Math.floor(datagridDivWidth/10);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/6)-1;

	var html = 
			'<table>'+
				'<tr>'+
					'<th style="width: '+monthWidth+'px; border-right: solid black 1px; border-bottom: solid black 1px;"></th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Renewables Obligation</th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Feed In Tariff</th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Contracts For Difference</th>'+
					'<th style="border-right: solid black 1px; border-bottom: solid black 1px;" colspan="3">Energy Intensive Industries</th>'+
					'<th style="border-bottom: solid black 1px;" colspan="4">Reason For Difference</th>'+
				'</tr>'+
				'<tr>'+
					'<th style="border-right: solid black 1px;">Month</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Latest Forecast Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Invoiced Usage</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Difference</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Usage Change</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">DUoS Reduction Project</th>'+
					'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Waste Reduction</th>'+
					'<th style="width: '+dataWidth+'px;">Unknown</th>'+
				'</tr>'

	for(var i = 0; i < 1; i++) {
		var htmlRow = '<tr>'+
						'<td style="border-right: solid black 1px;">JAN 2019</td>'+
						'<td style="border-right: solid black 1px;">250000</td>'+
						'<td style="border-right: solid black 1px;">200000</td>'+
						'<td style="border-right: solid black 1px;">50000</td>'+
						'<td style="border-right: solid black 1px;">250000</td>'+
						'<td style="border-right: solid black 1px;">200000</td>'+
						'<td style="border-right: solid black 1px;">50000</td>'+
						'<td style="border-right: solid black 1px;">250000</td>'+
						'<td style="border-right: solid black 1px;">200000</td>'+
						'<td style="border-right: solid black 1px;">50000</td>'+
						'<td style="border-right: solid black 1px;">250000</td>'+
						'<td style="border-right: solid black 1px;">200000</td>'+
						'<td style="border-right: solid black 1px;">50000</td>'+
						'<td style="border-right: solid black 1px;">10000</td>'+
						'<td style="border-right: solid black 1px;">25000</td>'+
						'<td style="border-right: solid black 1px;">5000</td>'+
						'<td>10000</td>'+
						'</tr>';

		html += htmlRow;
	}
	
	html += '</table>';

	datagridDiv.innerHTML = html;
}

function updateDatagrid() {
	switch(showBy) {
		case "WholesaleUsage":
			updateWholesaleUsageDatagrid();
			break;
		case "WholesaleCost":
			buildWholesaleCostDatagrid();
			break;
		case "WholesaleRate":
			updateWholesaleRateDatagrid();
			break;
		// case "DistributionUsage":
		// case "DistributionCost":
		// case "DistributionRate":
		case "RenewablesUsage":
			updateRenewablesUsageDatagrid();
			break;
		// case "RenewablesCost":
		// case "RenewablesRate":
		// case "BalancingUsage":
		// case "BalancingCost":
		// case "BalancingRate":
		// case "OtherUsage":
		// case "OtherCost":
		// case "OtherRate":
		// case "Forecast":
		// 	buildForecastForm(divToAppendTo);
		// 	break;
		// case "CostElements":
		// 	buildCostElementHeaderForm(divToAppendTo);
		// 	createCostElementCardButtons();
		// 	break;
		// case "Wholesale":
		// case "Distribution":
		// case "Renewables":
		// case "Balancing":
		// case "Other":
		// 	buildCostElementHeaderSubForm(divToAppendTo);
		// 	createCostElementDetailCardButtons(tabName);
		// 	break;
	}
}