function openTab(tabName, cardDivName) {
	var cardDiv = document.getElementById(cardDivName);
	clearElement(cardDiv);

	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName.concat('div');
	cardDiv.appendChild(newDiv);

	newDiv = document.getElementById(newDiv.id);
	newDiv.style.display = "block";
	newDiv.style.border = "none";

	createCard(newDiv, tabName);
  }

function createCardButtons(){
	openTab("Usage", "cardDiv");	
	updateChart(null, electricityChart);
}

function createCard(divToAppendTo, tabName) {
	buildWholesaleUsageForm(divToAppendTo);
}

function createDisplayAttributesDiv(divToAppendTo, id) {
	var div = document.createElement('div');
	div.id = id;
	divToAppendTo.appendChild(div);

	var treeDiv = document.getElementById(id);
	clearElement(treeDiv);

	return treeDiv;
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
	var monthWidth = Math.floor(datagridDivWidth/5);
	var dataWidth = Math.floor((datagridDivWidth - monthWidth)/2);

	var treeDiv = document.getElementById('electricityTreeDiv');
	var inputs = treeDiv.getElementsByTagName('input');
	var commodity = 'electricity';
	var checkBoxes = getCheckedCheckBoxes(inputs);

	var html = 
				'<table>'+
					'<tr>'+
						'<th style="width: '+monthWidth+'px; border-right: solid black 1px;">Date</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Meter</th>'+
						'<th style="width: '+dataWidth+'px; border-right: solid black 1px;">Capacity</th>'+
						'<th style="width: '+dataWidth+'px;">Maximum Demand</th>'+
					'</tr>';

	var showBy = 'WholesaleUsage';
	var newCategories = getNewCategories('Yearly', new Date(2019, 1, 1));   
	var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity, 'yyyy-MM-dd');

	var categoryLength = newCategories.length;
	var newSeriesLength = newSeries.length;

	for(var i = 0; i < categoryLength; i++) {
		for(var j = 0; j < newSeriesLength; j++) {
			var htmlRow = '<tr>'+
				'<td style="border-right: solid black 1px;">'+newCategories[i]+'</td>'+
				'<td style="border-right: solid black 1px;">'+newSeries[j].name+'</td>'+
				'<td style="border-right: solid black 1px;">'+newSeries[j]["data"][i]+'</td>'+
				'<td>250</td>'+
			'</tr>';

			html += htmlRow;
		}		
	}
	
	html += '</table>';

	datagridDiv.innerHTML = html;
}