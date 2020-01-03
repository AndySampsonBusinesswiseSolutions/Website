<?php 
	$PAGE_TITLE = "View Flex Position";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">  

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<div id="electricityDiv" style="margin-left: 30px;">
		<div class="row">
			<div class="final-column">
				<div class="tree-div" style="height: 315px;">
					<div id="electricityVolumeChart">
					</div>
				</div>
			</div>	
		</div>
		<br>
		<div class="row">
			<div class="final-column">
				<div class="tree-div" style="height: 315px;">
					<div id="electricityPriceChart">
					</div>
				</div>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden" style="margin-left: 30px;">
		<div class="row">
			<div class="final-column">
				<div class="tree-div" style="height: 315px;">
					<div id="gasVolumeChart">
					</div>
				</div>
			</div>
		</div>
		<br>
		<div class="row">
			<div class="final-column">
				<div class="tree-div" style="height: 315px;">
					<div id="gasPriceChart">
					</div>
				</div>
			</div>	
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

<script>
	function addCommoditySelectorOnClickEvent() {
		var commoditySelector = document.getElementById("electricityGasSelector");
		commoditySelector.addEventListener('click', function(event) {
			updateClassOnClick("electricityDiv", "listitem-hidden", "")
			updateClassOnClick("gasDiv", "listitem-hidden", "")
		})	
	}
</script>

<script type="text/javascript"> 
	addArrowOnClickEvents();
	addCommoditySelectorOnClickEvent();	

	window.onload = function(){
		resizeFinalColumns(50);
	}

	window.onresize = function(){
		resizeFinalColumns(50);
	}

	var electricityVolumeSeries = [{
          name: 'Open Vol',
		  data: [
			  		1.324, 1.324, 1.324, 1.324, 1.324, 1.324, 
					0.713, 0.713, 0.713, 0.713, 0.713, 0.713, 
					1.323, 1.323, 1.323, 1.323, 1.323, 1.323,
					0.711, 0.711, 0.711, 0.711, 0.711, 0.711,
					0.934, 0.934, 0.934, 0.934, 0.934, 0.934,
					0.711, 0.711, 0.711, 0.711, 0.711, 0.711
				]
		}, {
          name: 'Hedge Vol',
          data: [
			  		1.426, 1.426, 1.426, 1.426, 1.426, 1.426, 
		  			1.657, 1.657, 1.657, 1.657, 1.657, 1.657, 
		  			1.417, 1.417, 1.417, 1.417, 1.417, 1.417,
		  			1.659, 1.659, 1.659, 1.659, 1.659, 1.659,
		  			1.806, 1.806, 1.806, 1.806, 1.806, 1.806,
				  	1.659, 1.659, 1.659, 1.659, 1.659, 1.659
				]
		}];
		
	var electricityPriceSeries = [{
          name: 'Cap Price',
          type: 'line',
          data: [
					5.718,5.718,5.718,5.718,5.718,5.718,
					4.888,4.888,4.888,4.888,4.888,4.888,
					5.536,5.536,5.536,5.536,5.536,5.536,
					4.905,4.905,4.905,4.905,4.905,4.905,
					5.558,5.558,5.558,5.558,5.558,5.558,
					4.907,4.907,4.907,4.907,4.907,4.907
		  		]
        }, {
          name: 'ECP',
          type: 'line',
          data: [
					4.990,4.990,4.990,4.990,4.990,4.990,
					4.310,4.310,4.310,4.310,4.310,4.310,
					5.030,5.030,5.030,5.030,5.030,5.030,
					4.340,4.340,4.340,4.340,4.340,4.340,
					5.010,5.010,5.010,5.010,5.010,5.010,
					4.350,4.350,4.350,4.350,4.350,4.350,
				]
        }, {
          name: 'Market',
          type: 'line',
          data: [
					4.886,4.886,4.886,4.886,4.886,4.886,
					4.270,4.270,4.270,4.270,4.270,4.270,
					4.990,4.990,4.990,4.990,4.990,4.990,
					4.296,4.296,4.296,4.296,4.296,4.296,
					4.973,4.973,4.973,4.973,4.973,4.973,
					4.301,4.301,4.301,4.301,4.301,4.301,
				]
        }, {
          name: 'Day1Price',
          type: 'line',
          data: [
					5.918,5.918,5.918,5.918,5.918,5.918,
					4.444,4.444,4.444,4.444,4.444,4.444,
					5.033,5.033,5.033,5.033,5.033,5.033,
					4.459,4.459,4.459,4.459,4.459,4.459,
					5.053,5.053,5.053,5.053,5.053,5.053,
					4.461,4.461,4.461,4.461,4.461,4.461,
				]
        }];
	
	var electricityCategories = [
		'10 2020', '11 2020', '12 2020',
		'01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
		'01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
		'01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
		];
		
	var electricityVolumeOptions = {
        chart: {
		  type: 'bar',
		  stacked: true
		},
		tooltip: {
			x: {
			format: getChartTooltipXFormat("Yearly")
			}
		},
        xaxis: {
			title: {
			text: ''
			},
			labels: {
			format: getChartXAxisLabelFormat('Weekly')
			},
			categories: electricityCategories
		},
        yaxis: [{
          title: {
            text: 'MW'
          },
			min: 0,
			max: 3,
			decimalsInFloat: 3
        }]
	  };

	var electricityPriceOptions = {
        chart: {
		  type: 'line',
		  stacked: false
		},
		tooltip: {
			x: {
			format: getChartTooltipXFormat("Yearly")
			}
		},
        xaxis: {
			title: {
			text: ''
			},
			labels: {
			format: getChartXAxisLabelFormat('Weekly')
			},
			categories: electricityCategories
		},
        yaxis: [{
          title: {
            text: 'p/kWh'
          },
			min: 4,
			max: 6,
			decimalsInFloat: 3
        }]
	  };

	var gasVolumeSeries = [{
          name: 'Open Vol',
		  data: [
			  		300,300,300,1000,1650,1900,2150,1900,1800,1200,300,300,300,300,300,1000,1650,1900,2150,1900,1800,1200,300,300,300,300,300
				]
		}, {
          name: 'Hedge Vol',
          data: [
			  		456,434,664,1036,1631,2032,2180,1949,1790,1232,1192,579,448,442,663,1047,1638,2007,2198,1953,1786,1218,1202,579,448,445,653
				]
        }];
	var gasPriceSeries = [{
          name: 'Cap Price',
          data: [
					1.505846706,1.517857448,1.568527763,1.692013198,1.800109871,1.88943976,2.022683923,1.986276363,1.824882025,1.601181966,1.532870874,1.512227412,1.528742182,1.565900413,1.630082813,1.786973123,1.880431704,1.930726684,1.952496153,1.901825837,1.790351144,1.589171225,1.532495539,1.51072607,1.517857448,1.558769035,1.673997086,
		  		]
        }, {
          name: 'ECP',
          data: [
					1.3,1.31,1.33,1.54,1.59,1.66,1.74,1.73,1.64,1.43,1.35,1.35,1.37,1.39,1.42,1.63,1.66,1.71,1.74,1.71,1.64,1.44,1.35,1.35,1.36,1.38,1.45,
				]
        }, {
          name: 'Market',
          data: [
					1.233148281,1.24747928,1.301732345,1.439924114,1.547065387,1.631004091,1.753158791,1.728250151,1.595176595,1.390106834,1.328347056,1.314016058,1.334147698,1.368610337,1.428664044,1.578457097,1.66307823,1.708800939,1.727226508,1.679797728,1.574703741,1.388741977,1.336877412,1.318793057,1.326982199,1.363492123,1.464832754,
				]
        }, {
          name: 'Day1Price',
          data: [
					1.368951551,1.379870407,1.42593433,1.538193817,1.636463519,1.717672509,1.838803566,1.805705785,1.658983659,1.455619969,1.393518977,1.374752193,1.38976562,1.42354583,1.481893466,1.624521021,1.709483368,1.755206076,1.774996503,1.728932579,1.627591949,1.444701113,1.393177762,1.373387336,1.379870407,1.41706276,1.521815533,
				]
        }];
	
	var gasCategories = [
		'07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
		'01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
		'01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
		];
		
	var gasVolumeOptions = {
        chart: {
			type: 'bar',
		  stacked: true
		},
		tooltip: {
			x: {
			format: getChartTooltipXFormat("Yearly")
			}
		},
        xaxis: {
			title: {
			text: ''
			},
			labels: {
			format: getChartXAxisLabelFormat('Weekly')
			},
			categories: gasCategories
		},
        yaxis: [{
          title: {
            text: 'th/day'
          },
			min: 0,
			max: 5000,
			decimalsInFloat: 0
        }]
	  };
	var gasPriceOptions = {
        chart: {
		  type: 'line',
		  stacked: false
		},
		tooltip: {
			x: {
			format: getChartTooltipXFormat("Yearly")
			}
		},
        xaxis: {
			title: {
			text: ''
			},
			labels: {
			format: getChartXAxisLabelFormat('Weekly')
			},
			categories: gasCategories
		},
        yaxis: [{
          title: {
            text: 'p/kWh'
          },
			min: 1.1,
			max: 2.1,
			decimalsInFloat: 3
        }]
	  };
	
	refreshChart(electricityVolumeSeries, electricityCategories, "#electricityVolumeChart", electricityVolumeOptions);
	refreshChart(electricityPriceSeries, electricityCategories, "#electricityPriceChart", electricityPriceOptions);
	refreshChart(gasVolumeSeries, gasCategories, "#gasVolumeChart", gasVolumeOptions);
	refreshChart(gasPriceSeries, gasCategories, "#gasPriceChart", gasPriceOptions);
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>