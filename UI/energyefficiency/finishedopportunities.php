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
							<th id="th10" style="border: solid black 1px;">Notes</th>
							<th id="th5" style="border: solid black 1px;">Start Date</th>
							<th id="th6" style="border: solid black 1px;">Finish Date</th>
							<th id="th7" style="border: solid black 1px;">Cost</th>
							<th id="th8" style="border: solid black 1px;">Actual<br>kWh Savings (pa)</th>
							<th id="th9" style="border: solid black 1px;">Actual<br>£ Savings (pa)</th>
							<th id="th11" style="border: solid black 1px;">Estimated<br>kWh Savings (pa)</th>
							<th id="th12" style="border: solid black 1px;">Estimated<br>£ Savings (pa)</th>
							<th id="th13" style="border: solid black 1px;">Net<br>kWh Savings (pa)</th>
							<th id="th14" style="border: solid black 1px;">Net<br>£ Savings (pa)</th>
							<th id="th15" style="border: solid black 1px;">Total<br>ROI Months</th>
							<th id="th16" style="border: solid black 1px;">Remaining<br>ROI Months</th>
						</tr>
					</thead>
					<tbody class="scrollContent3">
					<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site X</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th10" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
							<td headers="th5" style="border: solid black 1px;">01/01/2017</td>
							<td headers="th6" style="border: solid black 1px;">28/02/2017</td>
							<td headers="th7" style="border: solid black 1px;">£55,000</td>
							<td headers="th8" style="border: solid black 1px;">10,000</td>
							<td headers="th9" style="border: solid black 1px;">£65,000</td>
							<td headers="th11" style="border: solid black 1px;">9,000</td>
							<td headers="th12" style="border: solid black 1px;">£60,000</td>
							<td headers="th13" style="border: solid black 1px;">45,000</td>
							<td headers="th14" style="border: solid black 1px;">£140,000</td>
							<td headers="th15" style="border: solid black 1px;">9</td>
							<td headers="th16" style="border: solid black 1px;">0</td>
						</tr>
						<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site Y</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th10" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
							<td headers="th5" style="border: solid black 1px;">01/08/2019</td>
							<td headers="th6" style="border: solid black 1px;">15/08/2019</td>
							<td headers="th7" style="border: solid black 1px;">£10,000</td>
							<td headers="th8" style="border: solid black 1px;">5,000</td>
							<td headers="th9" style="border: solid black 1px;">£60,000</td>
							<td headers="th11" style="border: solid black 1px;">9,000</td>
							<td headers="th12" style="border: solid black 1px;">£160,000</td>
							<td headers="th13" style="border: solid black 1px;">45,000</td>
							<td headers="th14" style="border: solid black 1px;">£20,000</td>
							<td headers="th15" style="border: solid black 1px;">2</td>
							<td headers="th16" style="border: solid black 1px;">0</td>
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
		-49583.3333333333,-44166.6666666667,-38750,-33333.3333333333,-27916.6666666667,-22500,
-17083.3333333333,-11666.6666666667,-6250,-833.333333333338,4583.33333333333,10000,
15416.6666666667,20833.3333333333,26250,31666.6666666667,37083.3333333333,42500,
47916.6666666667,53333.3333333333,58750,64166.6666666667,69583.3333333333,75000,
80416.6666666667,85833.3333333333,91250,96666.6666666667,102083.333333333,107500,
107916.666666667,118333.333333333,128750,139166.666666667,149583.333333333,160000

          ]
  }, {
	name: 'LED Lighting - Site X',
	type: 'line',
    data: [
		-49583.33,-44166.66,-38749.99,-33333.32,-27916.65,-22499.98,
-17083.31,-11666.64,-6249.97,-833.300000000003,4583.37,10000.04,
15416.71,20833.38,26250.05,31666.72,37083.39,42500.06,
47916.73,53333.4,58750.07,64166.74,69583.41,75000.08,
80416.75,85833.42,91250.09,96666.76,102083.43,107500.1,
112916.77,118333.44,123750.11,129166.78,134583.45,140000.12
          ]
  }, {
	name: 'LED Lighting - Site Y',
	type: 'line',
    data: [
		null,null,null,null,null,null,
		null,null,null,null,null,null,
		null,null,null,null,null,null,
		null,null,null,null,null,null,
		null,null,null,null,null,null,
		-5000,0,5000,10000,15000,20000
          ]
  }];
  
var electricityPriceSeries = [{
    name: 'Estimated Cost Saving',
    data: [
              4718,4718,4718,4718,4718,4718,4718,4718,
              3888,3888,3888,3888,3888,3888,3888,3888,
              4536,4536,4536,4536,4536,4536,4536,4536,
              3905,3905,3905,3905,3905,3905,3905,3905,
              4558,4558,4558,4558,4558,4558,4558,4558,
              3907,3907,3907,3907,3907,3907,3907,3907,
            ]
  },{
    name: 'Actual Cost Saving',
    data: [
              5718,5718,5718,5718,5718,5718,
              4888,4888,4888,4888,4888,4888,
              5536,5536,5536,5536,5536,5536,
              4905,4905,4905,4905,4905,4905,
              5558,5558,5558,5558,5558,5558,
              4907,4907,4907,4907,4907,4907
            ]
  },
   {
    name: 'Estimated kWh Saving',
    data: [
              3990,3990,3990,3990,3990,3990,3990,3990,
              3310,3310,3310,3310,3310,3310,3310,3310,
              4030,4030,4030,4030,4030,4030,4030,4030,
              3340,3340,3340,3340,3340,3340,3340,3340,
              4010,4010,4010,4010,4010,4010,4010,4010,
              3350,3350,3350,3350,3350,3350,3350,3350,
          ]
  }, {
    name: 'Actual kWh Saving',
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
  '02 2017', '03 2017', '04 2017',
  '05 2017', '06 2017', '07 2017', '08 2017', '09 2017', '10 2017', '11 2017', '12 2017', '01 2018', '02 2018', '03 2018', '04 2018',
  '05 2018', '06 2018', '07 2018', '08 2018', '09 2018', '10 2018', '11 2018', '12 2018', '01 2019', '02 2019', '03 2019', '04 2019',
  '05 2019', '06 2019', '07 2019', '08 2019', '09 2019', '10 2019', '11 2019', '12 2019', '01 2020', '02 2020', '03 2020', '04 2020',
  '05 2020', '06 2020', '07 2020', '08 2020', '09 2020', '10 2020', '11 2020', '12 2020', '01 2021'
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
    },
	decimalsInFloat: 0
  }]
};

var cumulativeSavingOptions = {
  chart: {
    type: 'bar',
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
      min: 3000,
      max: 6000,
      decimalsInFloat: 0
  },
	{
	seriesName: 'Estimated Cost Saving',
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
      min: 3000,
      max: 6000,
      decimalsInFloat: 0
	},
	{
	seriesName: 'Actual kWh Saving',
	show: false,
	opposite: false,
	axisTicks: {
		show: true,
	},
	labels: {
		style: {
		color: '#00E396',
		}
	},
	title: {
		text: "kWh"
	},
      min: 3000,
      max: 6000,
      decimalsInFloat: 0
	},
	{
	seriesName: 'Estimated kWh  Saving',
	show: false,
	opposite: true,
	axisTicks: {
		show: false,
	},
	labels: {
		style: {
		color: '#00E396',
		}
	},
	title: {
		text: "£"
	},
      min: 3000,
      max: 6000,
      decimalsInFloat: 0
	}]
};
	
	refreshChart(electricityVolumeSeries, electricityCategories, "#electricityVolumeChart", costVolumeSavingOptions);
	refreshChart(electricityPriceSeries, electricityCategories, "#electricityPriceChart", cumulativeSavingOptions);
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>