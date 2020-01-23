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
	button.setAttribute('onclick', 'openTab(event, "Forecast", "cardDiv")');
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
}

function createCostElementCardButtons(){
	var cardDiv = document.getElementById('costElementCardDiv');
	var tabDiv = document.getElementById('costElementTabDiv');

	cardDiv.setAttribute('style', '');

	var wholesaleButton = document.createElement('button');
	wholesaleButton.setAttribute('class', 'tablinks');
	wholesaleButton.setAttribute('onclick', 'openTab(event, "Wholesale", "costElementCardDiv")');
	wholesaleButton.innerHTML = 'Wholesale';
	wholesaleButton.id = 'Wholesale';
	tabDiv.appendChild(wholesaleButton);

	var distributionButton = document.createElement('button');
	distributionButton.setAttribute('class', 'tablinks');
	distributionButton.setAttribute('onclick', 'openTab(event, "Distribution", "costElementCardDiv")');
	distributionButton.innerHTML = 'Distribution';
	distributionButton.id = 'Distribution';
	tabDiv.appendChild(distributionButton);

	var renewablesButton = document.createElement('button');
	renewablesButton.setAttribute('class', 'tablinks');
	renewablesButton.setAttribute('onclick', 'openTab(event, "Renewables", "costElementCardDiv")');
	renewablesButton.innerHTML = 'Renewables';
	renewablesButton.id = 'Renewables';
	tabDiv.appendChild(renewablesButton);

	var balancingButton = document.createElement('button');
	balancingButton.setAttribute('class', 'tablinks');
	balancingButton.setAttribute('onclick', 'openTab(event, "Balancing", "costElementCardDiv")');
	balancingButton.innerHTML = 'Balancing';
	balancingButton.id = 'Balancing';
	tabDiv.appendChild(balancingButton);

	var otherButton = document.createElement('button');
	otherButton.setAttribute('class', 'tablinks');
	otherButton.setAttribute('onclick', 'openTab(event, "Other", "costElementCardDiv")');
	otherButton.innerHTML = 'Other';
	otherButton.id = 'Other';
	tabDiv.appendChild(otherButton);

	updateTabDiv(tabDiv);
	openTab(event, "Wholesale", "costElementCardDiv");
	wholesaleButton.className += " active";
}

function createCostElementDetailCardButtons(tabName){
	var cardDiv = document.getElementById('costElementDetailCardDiv');
	var tabDiv = document.getElementById('costElementDetailTabDiv');

	cardDiv.setAttribute('style', '');

	var usageButton = document.createElement('button');
	usageButton.setAttribute('class', 'tablinks');
	usageButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Usage", "costElementDetailCardDiv")');
	usageButton.innerHTML = 'Usage';
	usageButton.id = tabName+'Usage';
	tabDiv.appendChild(usageButton);

	var costButton = document.createElement('button');
	costButton.setAttribute('class', 'tablinks');
	costButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Cost", "costElementDetailCardDiv")');
	costButton.innerHTML = 'Cost';
	costButton.id = tabName+'Cost';
	tabDiv.appendChild(costButton);

	var rateButton = document.createElement('button');
	rateButton.setAttribute('class', 'tablinks');
	rateButton.setAttribute('onclick', 'openTab(event, "'+tabName+'Rate", "costElementDetailCardDiv")');
	rateButton.innerHTML = 'Rate';
	rateButton.id = tabName+'Rate';
	tabDiv.appendChild(rateButton);

	updateTabDiv(tabDiv);
	openTab(event, tabName+'Usage', "costElementDetailCardDiv");
	usageButton.className += " active";
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
		case "Forecast":
		case "WholesaleUsage":
		case "WholesaleCost":
		case "WholesaleRate":
		case "DistributionUsage":
		case "DistributionCost":
		case "DistributionRate":
		case "RenewablesUsage":
		case "RenewablesCost":
		case "RenewablesRate":
		case "BalancingUsage":
		case "BalancingCost":
		case "BalancingRate":
		case "OtherUsage":
		case "OtherCost":
		case "OtherRate":
			buildForecastForm(divToAppendTo);
			break;
		case "CostElements":
			buildCostElementHeaderForm(divToAppendTo);
			createCostElementCardButtons();
			break;
		case "Wholesale":
		case "Distribution":
		case "Renewables":
		case "Balancing":
		case "Other":
			buildCostElementHeaderSubForm(divToAppendTo);
			createCostElementDetailCardButtons(tabName);
			break;
	}
}

function buildCostElementHeaderForm(divToAppendTo) {
	var div = document.createElement('div');
	div.id = 'displayCostElements';
	divToAppendTo.appendChild(div);

	var html = 
		'<div>'+
			'<div class="group-by-div" id="costElementCardDiv" style="display: none;">'+
				'<div class="tabDiv" id="costElementTabDiv" style="overflow-y: auto; overflow: auto;"></div>'+
			'</div>'+
		'</div>'
	div.innerHTML = html;
}

function buildCostElementHeaderSubForm(divToAppendTo) {
	var div = document.createElement('div');
	div.id = 'displayCostElementDetails';
	divToAppendTo.appendChild(div);

	var html = 
		'<div>'+
			'<div class="group-by-div" id="costElementDetailCardDiv" style="display: none;">'+
				'<div class="tabDiv" id="costElementDetailTabDiv" style="overflow-y: auto; overflow: auto;"></div>'+
			'</div>'+
		'</div>'
	div.innerHTML = html;
}

function buildForecastForm(divToAppendTo) {
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

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