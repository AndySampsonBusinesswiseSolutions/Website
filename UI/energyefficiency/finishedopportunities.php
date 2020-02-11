<?php 
	$PAGE_TITLE = "Finished Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="tree-column">
			<div>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<div class="tree-div" style="height: 375px;">
				<div id="electricityPriceChart"></div>
			</div>
			<br>
			<div class="tree-div" style="height: 375px;">
				<div id="electricityVolumeChart"></div>
			</div>
			<br>
			<div id="tableContainer" class="tableContainer3">
				<table style="width: 100%;">
					<thead>
						<tr>
							<th id="th1" style="border: solid black 1px;">Project Name</th>
							<th id="th2" style="border: solid black 1px;">Site</th>
							<th id="th3" style="border: solid black 1px;">Meter</th>
							<th id="th4" style="border: solid black 1px;">Engineer</th>
							<th id="th5" style="border: solid black 1px;">Start Date</th>
							<th id="th6" style="border: solid black 1px;">Finish Date</th>
							<th id="th7" style="border: solid black 1px;">Cost</th>
							<th id="th8" style="border: solid black 1px;">kWh Savings (pa)</th>
							<th id="th9" style="border: solid black 1px;">£ Savings (pa)</th>
						</tr>
					</thead>
					<tbody class="scrollContent3">
					<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site X</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
							<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
							<td headers="th7" style="border: solid black 1px;">£55,000</td>
							<td headers="th8" style="border: solid black 1px;">10,000</td>
							<td headers="th9" style="border: solid black 1px;">£65,000</td>
						</tr>
						<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site Y</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
							<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
							<td headers="th7" style="border: solid black 1px;">£10,000</td>
							<td headers="th8" style="border: solid black 1px;">5,000</td>
							<td headers="th9" style="border: solid black 1px;">£60,000</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

<script type="text/javascript"> 
	window.onload = function(){
		resizeFinalColumns(375);
	}

	window.onresize = function(){
		resizeFinalColumns(375);
	}

	var electricityVolumeSeries = [{
	name: 'CAPEX/OPEX',
	type: 'bar',
    data: [
            -55000, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0,
            -10000, 0, 0, 0, 0, 0
          ]
  }, {
	name: 'Cumulative £ Saving',
	type: 'line',
    data: [
			-50000, -45000, -40000, -35000, -30000, -25000, 
            -20000, -15000, -10000, -5000, 0, 5000, 
			10000, 15000, 20000, 25000, 30000, 35000,
			40000, 45000, 50000, 55000, 60000, 65000,
			70000, 75000, 80000, 85000, 90000, 95000,
			90000, 100000, 110000, 120000, 130000, 140000
          ]
  }];
  
var electricityPriceSeries = [{
    name: 'Cost Saving',
    type: 'line',
    data: [
              5718,5718,5718,5718,5718,5718,
              4888,4888,4888,4888,4888,4888,
              5536,5536,5536,5536,5536,5536,
              4905,4905,4905,4905,4905,4905,
              5558,5558,5558,5558,5558,5558,
              4907,4907,4907,4907,4907,4907
            ]
  }, {
    name: 'kWh Saving',
    type: 'line',
    data: [
              4990,4990,4990,4990,4990,4990,
              4310,4310,4310,4310,4310,4310,
              5030,5030,5030,5030,5030,5030,
              4340,4340,4340,4340,4340,4340,
              5010,5010,5010,5010,5010,5010,
              4350,4350,4350,4350,4350,4350,
          ]
  }];

var electricityCategories = [
  '10 2020', '11 2020', '12 2020',
  '01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
  '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
  '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
  ];
  
var costVolumeSavingOptions = {
  chart: {
    type: 'line'
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
      text: '£'
    }
  }]
};

var cumulativeSavingOptions = {
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
      text: 'kWh'
    },
      min: 4000,
      max: 6000,
      decimalsInFloat: 0
  },
	{
	seriesName: 'Cost Saving',
	opposite: true,
	axisTicks: {
		show: true,
	},
	labels: {
		style: {
		color: '#00E396',
		}
	},
	title: {
		text: "£"
	},
      min: 4000,
      max: 6000,
      decimalsInFloat: 0
	}]
};
	
	refreshChart(electricityVolumeSeries, electricityCategories, "#electricityVolumeChart", costVolumeSavingOptions);
	refreshChart(electricityPriceSeries, electricityCategories, "#electricityPriceChart", cumulativeSavingOptions);
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>