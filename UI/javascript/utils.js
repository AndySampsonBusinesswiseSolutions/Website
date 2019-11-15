'use strict';

var config;
var dateArr;

function initialiseTree(datasets){
	var checks = document.querySelectorAll("input[type=checkbox]");
	var labels = document.querySelectorAll("label");

	for(var i = 0; i < labels.length; i++){
		labels[i].addEventListener( 'click', function() {
			updateChildrenDisplay(this);
		});

		checks[i].addEventListener( 'click', function() {
			updateChildrenChecked(this, datasets);
		});
	}
}

function updateChildrenChecked(elm, datasets) {
	var pN = elm.parentNode;
	var childCheks = pN.children;

	for(var i = 0; i < childCheks.length; i++){
		if(childCheks[i].tagName.toUpperCase() == 'DIV'){
			var div = childCheks[i];
			var divInputs = div.getElementsByTagName('INPUT');

			for(var j = 0; j < divInputs.length; j++){
				divInputs[j].checked = elm.checked;
				var dataset = getDataset(datasets, divInputs[j].id);

				if(dataset != null){
					updateGraph(divInputs[j], dataset);
				}
			}
		}
	}
}

function getDataset(datasets, elementId){
	var dataset;

	if(elementId == 0)	{
		dataset = getClone(datasets[1][1]);

		for(var i = 2; i < datasets.length; i++){
			for(var j = 0; j < datasets[i][1].length; j++){
				var readDateText = datasets[i][1][j][0];
				var readDateIndex = getReadDateIndex(readDateText, dataset);

				var read = getClone(datasets[i][1][j]);
				if(readDateIndex == -1){
					dataset.push(read);
				}
				else {
					dataset[readDateIndex][1] = dataset[readDateIndex][1] + read[1];
				}
			}
		}
	}
	else {
		for(var i = 1; i < datasets.length; i++){
			if(datasets[i][0] == elementId){
				dataset = getClone(datasets[i][1]);
				break;
			}	
		}

		if(dataset == null){
			console.log('Dataset could not be found for input with id ' + elementId);
		}
	}	

	return dataset;
}

function getClone(item){
	return JSON.parse(JSON.stringify(item));
}

function getReadDateIndex(readDateText, dataset){
	var index = -1;

	for(var k = 0; k < dataset.length; k++){
		if(dataset[k][0] == readDateText){
			index = k;
			break;
		}
	}

	return index;
}

function updateChildrenDisplay(elm) {
	var pN = elm.parentNode;
	var childCheks = pN.children;
  
	for(var i = 0; i < childCheks.length; i++){
	  if(hasClass(childCheks[i], 'child-check')){
		if(hasClass(childCheks[i], 'active')){
			childCheks[i].classList.remove("active");
		}
		else {
			childCheks[i].classList.add("active");
		}
	  }
	}
}
  
function hasClass(elem, className) {
	return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}

function initialiseGraph(window, document, datasets) {
	dateArr = getDateArray(getMinimumDataSetDate(datasets), getMaximumDataSetDate(datasets));
	config = setupChartConfiguration(dateArr, 'line', false, 'Date', 'Volume (kWh)');

	var inputs = document.getElementsByClassName('child-check-input');

	for(var i = 0; i < inputs.length; i++) {
		inputs[i].addEventListener('click', function() {
			getDataSetAndUpdateGraph(datasets, this);			
		});
	}

	createChart(window, document, config);

	inputs[0].checked = true;
	getDataSetAndUpdateGraph(datasets, inputs[0]);
}

function getMinimumDataSetDate(datasets){
	var minDate = new Date(9999, 12, 31);

	for(var i = 1; i < datasets.length; i++){
		var dataset = datasets[i];

		for(var j = 0; j < dataset[1].length; j++){
			var readDateText = dataset[1][j][0];
			var readDate = new Date(readDateText);

			if(readDate < minDate){minDate = readDate};
		}
	}

	minDate.setDate(minDate.getDate() - 1);
	minDate.setHours(0, 0, 0);

	return minDate;
}

function getMaximumDataSetDate(datasets){
	var maxDate = new Date(1900, 1, 1);

	for(var i = 1; i < datasets.length; i++){
		var dataset = datasets[i];

		for(var j = 0; j < dataset[1].length; j++){
			var readDateText = dataset[1][j][0];
			var readDate = new Date(readDateText);

			if(readDate > maxDate){maxDate = readDate};
		}
	}

	maxDate.setDate(maxDate.getDate() + 1);
	maxDate.setHours(0, 0, 0);

	return maxDate;
}

function getDataSetAndUpdateGraph(datasets, input){
	var dataset = getDataset(datasets, input.id);

	if(dataset != null){
		updateGraph(input, dataset);
	}
}

function updateChartType(){
	var chartType = document.getElementById('chartType').value;

	if(chartType == "stackedBar"){
		config = setupChartConfiguration(dateArr, 'bar', true, 'Date', 'Volume (kWh)');
	}
	else if(chartType == "horizontalBar"){
		config = setupChartConfiguration(dateArr, chartType, false, 'Volume (kWh)', 'Date');
	}
	else if(chartType == "horizontalStackedBar"){
		config = setupChartConfiguration(dateArr, "horizontalBar", true, 'Volume (kWh)', 'Date');
	}
	else {
		config = setupChartConfiguration(dateArr, chartType, false, 'Date', 'Volume (kWh)');
	}

	window.myLine.destroy();
	createChart(window, document, config);
	
	var inputs = document.getElementsByClassName('child-check-input');
	for(var i = 0; i < inputs.length; i++) {
		getDataSetAndUpdateGraph(datasets, inputs[i]);			
	}
}

function createChart(window, document, config){
	var ctx = document.getElementById('canvas').getContext('2d');
	window.myLine = new Chart(ctx, config);
}

function setupChartConfiguration(dateArr, chartType, stack, xAxisLabel, yAxisLabel){
	return {
		type: chartType,
		data: {
			labels: dateArr,
			datasets: []
		},
		options: {
			responsive: true,
			title: {
				display: true,
				text: 'Electricity Consumption Summary'
			},
			tooltips: {
				mode: 'index',
				intersect: false,
			},
			hover: {
				mode: 'nearest',
				intersect: false
			},
			scales: {
				xAxes: [{
					stacked: stack,
					scaleLabel: {
						display: true,
						labelString: xAxisLabel
					},
					ticks: {
						fontSize: 10,
						autoSkip: false,
                    	maxRotation: 90,
						minRotation: 90,
						beginAtZero: true
					}
				}],
				yAxes: [{
					stacked: stack,
					scaleLabel: {
						display: true,
						labelString: yAxisLabel
					},
					ticks: {
						beginAtZero: true
					}
				}]
			}
		}
	};
}

function getDateArray(start, end) {
	var arr = new Array();
	var dt = new Date(start);
	while (dt <= end) {
		arr.push(convertDateToString(dt));
		dt.setDate(dt.getDate() + 1);
	}
	return arr;
}

function convertDateToString(date){
	var dd = date.getDate();
	var mm = date.getMonth()+1; 
	var yyyy = date.getFullYear();

	if(dd<10) 
	{
		dd='0'+dd;
	} 

	if(mm<10) 
	{
		mm='0'+mm;
	} 

	return yyyy + '-' + mm + '-' + dd;
}

function updateGraph(input, dataset){
	if(input.checked) {
		addGraphDataSet(input.id, input.name, dataset);
	}
	else {
		removeGraphDataSet(input.name);
	}; 

	window.myLine.update();
}

function addGraphDataSet(inputId, inputName, dataset){
	var colorNames = Object.keys(window.chartColors);
	var colorName = colorNames[inputId % colorNames.length];
	var newColor = window.chartColors[colorName];
	var newDataset = {
		label: inputName,
		backgroundColor: newColor,
		borderColor: newColor,
		data: [],
		fill: false
	} 

	for (var index = 0; index < config.data.labels.length; ++index) {
		var readDateIndex = getReadDateIndex(config.data.labels[index], dataset);

		if(readDateIndex == -1){
			newDataset.data.push(null);
		}
		else {
			newDataset.data.push(dataset[readDateIndex][1]);
		}
	}	

	config.data.datasets.push(newDataset);
}

function removeGraphDataSet(inputName){
	for(var j = 0; j < config.data.datasets.length; j++) {
		if(config.data.datasets[j].label == inputName) {
			config.data.datasets.splice(j, 1);
			break;
		}
	}
}

function getDummyDataSets(datasets){
	var submeter1data = [3, [
		['2019-10-01', 50],
		['2019-10-02', 51],
		['2019-10-03', 52],
		['2019-10-04', 51],
		['2019-10-05', 50],
		['2019-10-06', 49],
		['2019-10-07', 48],
		['2019-10-08', 47],
		['2019-10-09', 46],
		['2019-10-10', 45],
		['2019-10-11', 44],
		['2019-10-12', 43],
		['2019-10-13', 42],
		['2019-10-14', 43],
		['2019-10-15', 44],
		['2019-10-16', 45],
		['2019-10-17', 46],
		['2019-10-18', 47],
		['2019-10-19', 48],
		['2019-10-20', 49],
		['2019-10-21', 50],
		['2019-10-22', 51],
		['2019-10-23', 52],
		['2019-10-24', 53],
		['2019-10-25', 54],
		['2019-10-26', 55],
		['2019-10-27', 56],
		['2019-10-28', 57],
		['2019-10-29', 58],
		['2019-10-30', 59],
		['2019-10-31', 61]
	]];

	var submeter2data = [4, [
		['2019-10-30', 59],
		['2019-10-31', 60],
		['2019-11-01', 50],
		['2019-11-02', 51],
		['2019-11-03', 52],
		['2019-11-04', 51],
		['2019-11-05', 50],
		['2019-11-06', 49],
		['2019-11-07', 48],
		['2019-11-08', 47],
		['2019-11-09', 46],
		['2019-11-10', 45],
		['2019-11-11', 44],
		['2019-11-12', 43],
		['2019-11-13', 42],
		['2019-11-14', 43],
		['2019-11-15', 44],
		['2019-11-16', 45],
		['2019-11-17', 46],
		['2019-11-18', 47],
		['2019-11-19', 48],
		['2019-11-20', 49],
		['2019-11-21', 50],
		['2019-11-22', 51],
		['2019-11-23', 52],
		['2019-11-24', 53],
		['2019-11-25', 54],
		['2019-11-26', 55],
		['2019-11-27', 56],
		['2019-11-28', 57],
		['2019-11-29', 58],
		['2019-11-30', 59]
	]];

	datasets.push(submeter1data);
	datasets.push(submeter2data);
}

window.chartColors = {
	red: 'rgb(255, 99, 132)',
	orange: 'rgb(255, 159, 64)',
	yellow: 'rgb(255, 205, 86)',
	green: 'rgb(75, 192, 192)',
	blue: 'rgb(54, 162, 235)',
	purple: 'rgb(153, 102, 255)',
	grey: 'rgb(201, 203, 207)'
};