'use strict';

var config;

function initialiseTree(){
	var checks = document.querySelectorAll("input[type=checkbox]");
	var labels = document.querySelectorAll("label");

	for(var i = 0; i < labels.length; i++){
		labels[i].addEventListener( 'click', function() {
			updateChildrenDisplay(this);
		});

		checks[i].addEventListener( 'click', function() {
			updateChildrenChecked(this);
		});
	}
}

function updateChildrenChecked(elm) {
	var pN = elm.parentNode;
	var childCheks = pN.children;

	for(var i = 0; i < childCheks.length; i++){
		if(childCheks[i].tagName.toUpperCase() == 'DIV'){
			var div = childCheks[i];
			var divInputs = div.getElementsByTagName('INPUT');

			for(var j = 0; j < divInputs.length; j++){
				divInputs[j].checked = elm.checked;
				updateGraph(divInputs[j]);
			}
		}
	}
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
	var minDate = new Date(9999, 12, 31);
	var maxDate = new Date(1900, 1, 1);

	for(var i = 1; i < datasets.length; i++){
		var dataset = datasets[i];

		for(var j = 0; j < dataset[1].length; j++){
			var readDateText = dataset[1][j][0];
			var readDate = new Date(readDateText);

			if(readDate < minDate){minDate = readDate};
			if(readDate > maxDate){maxDate = readDate};
		}
	}

	var getDateArray = function(start, end) {
		var arr = new Array();
		var dt = new Date(start);
		while (dt <= end) {
			arr.push(convertDateToString(dt));
			dt.setDate(dt.getDate() + 1);
		}
		return arr;
	}
	
	var dateArr = getDateArray(minDate, maxDate);

	config = {
		type: 'line',
		data: {
			labels: dateArr
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
				intersect: true
			},
			scales: {
				xAxes: [{
					display: true,
					scaleLabel: {
						display: true,
						labelString: 'Date'
					}
				}],
				yAxes: [{
					display: true,
					scaleLabel: {
						display: true,
						labelString: 'Volume (kWh)'
					}
				}]
			}
		}
	};

	window.onload = function() {
		var ctx = document.getElementById('canvas').getContext('2d');
		window.myLine = new Chart(ctx, config);
	};

	var inputs = document.getElementsByTagName('input');

	for(var i = 0; i < inputs.length; i++) {
		if(inputs[i].type.toLowerCase() == 'checkbox') {
			inputs[i].addEventListener('click', function() {
				updateGraph(this);
			});
		}
	}
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

	return dd+'/'+mm+'/'+yyyy;
}

function updateGraph(input){
	if(input.checked) {
		addGraphDataSet(input.id, input.name);
	}
	else {
		for(var j = 0; j < config.data.datasets.length; j++) {
			if(config.data.datasets[j].label == input.name) {
				config.data.datasets.splice(j, 1);
				break;
			}
		}
	}; 

	window.myLine.update();
}

function addGraphDataSet(inputId, inputName){
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
		newDataset.data.push(randomScalingFactor());
	}

	config.data.datasets.push(newDataset);
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
		['2019-10-31', 60],
	]];

	var submeter2data = [4, [
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
		['2019-11-30', 59],
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

(function(global) {
	var MONTHS = [
		'January',
		'February',
		'March',
		'April',
		'May',
		'June',
		'July',
		'August',
		'September',
		'October',
		'November',
		'December'
	];

	var COLORS = [
		'#4dc9f6',
		'#f67019',
		'#f53794',
		'#537bc4',
		'#acc236',
		'#166a8f',
		'#00a950',
		'#58595b',
		'#8549ba'
	];

	var Samples = global.Samples || (global.Samples = {});
	var Color = global.Color;

	Samples.utils = {
		// Adapted from http://indiegamr.com/generate-repeatable-random-numbers-in-js/
		srand: function(seed) {
			this._seed = seed;
		},

		rand: function(min, max) {
			var seed = this._seed;
			min = min === undefined ? 0 : min;
			max = max === undefined ? 1 : max;
			this._seed = (seed * 9301 + 49297) % 233280;
			return min + (this._seed / 233280) * (max - min);
		},

		numbers: function(config) {
			var cfg = config || {};
			var min = cfg.min || 0;
			var max = cfg.max || 1;
			var from = cfg.from || [];
			var count = cfg.count || 8;
			var decimals = cfg.decimals || 8;
			var continuity = cfg.continuity || 1;
			var dfactor = Math.pow(10, decimals) || 0;
			var data = [];
			var i, value;

			for (i = 0; i < count; ++i) {
				value = (from[i] || 0) + this.rand(min, max);
				if (this.rand() <= continuity) {
					data.push(Math.round(dfactor * value) / dfactor);
				} else {
					data.push(null);
				}
			}

			return data;
		},

		labels: function(config) {
			var cfg = config || {};
			var min = cfg.min || 0;
			var max = cfg.max || 100;
			var count = cfg.count || 8;
			var step = (max - min) / count;
			var decimals = cfg.decimals || 8;
			var dfactor = Math.pow(10, decimals) || 0;
			var prefix = cfg.prefix || '';
			var values = [];
			var i;

			for (i = min; i < max; i += step) {
				values.push(prefix + Math.round(dfactor * i) / dfactor);
			}

			return values;
		},

		months: function(config) {
			var cfg = config || {};
			var count = cfg.count || 12;
			var section = cfg.section;
			var values = [];
			var i, value;

			for (i = 0; i < count; ++i) {
				value = MONTHS[Math.ceil(i) % 12];
				values.push(value.substring(0, section));
			}

			return values;
		},

		color: function(index) {
			return COLORS[index % COLORS.length];
		},

		transparentize: function(color, opacity) {
			var alpha = opacity === undefined ? 0.5 : 1 - opacity;
			return Color(color).alpha(alpha).rgbString();
		}
	};

	// DEPRECATED
	window.randomScalingFactor = function() {
		return Math.round(Samples.utils.rand(-100, 100));
	};

	// INITIALIZATION

	Samples.utils.srand(Date.now());

	// Google Analytics
	/* eslint-disable */
	if (document.location.hostname.match(/^(www\.)?chartjs\.org$/)) {
		(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
		(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
		m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
		})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
		ga('create', 'UA-28909194-3', 'auto');
		ga('send', 'pageview');
	}
	/* eslint-enable */

}(this));