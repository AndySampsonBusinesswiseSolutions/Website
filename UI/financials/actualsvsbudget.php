<?php 
	$PAGE_TITLE = "Variance Analysis";
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
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/electricitytree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/electricitychart.php") ?>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/gastree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/gaschart.php") ?>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/chart.js"></script>
<script src="/javascript/tree.js"></script>
<script src="/javascript/actualsvbudgettab.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="/basedata/data.json"></script>

<script>
	function addCommoditySelectorOnClickEvent() {
		var commoditySelector = document.getElementById("electricityGasSelector");
		commoditySelector.addEventListener('click', function(event) {
			updateClassOnClick("electricityDiv", "listitem-hidden", "")
			updateClassOnClick("gasDiv", "listitem-hidden", "")
		})	
	}
</script>

<script>
	function updateChart(callingElement, chart) {
		var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
		var inputs = treeDiv.getElementsByTagName('input');
		var commodity = chart.id.replace('Chart', '').toLowerCase();
		var checkBoxes = getCheckedCheckBoxes(inputs);		
		
		clearElement(chart);

		if(checkBoxes.length == 0) {
			createBlankChart('#' + commodity + 'Chart', 'There is no ' + commodity + ' data to display. Select from the tree to the left to display');
			return;
		}
    
		var showBySpan = document.getElementById(commodity.concat('ChartHeaderShowBy'));
		var periodSpan = document.getElementById(commodity.concat('ChartHeaderPeriod'));
		var chartDate = new Date(document.getElementById(commodity.concat('Calendar')).value);
		var newCategories = getNewCategories(periodSpan.children[0].value, chartDate);   
		var newSeries = getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity, getPeriodDateFormat(periodSpan.children[0].value));
		var typeSpan = document.getElementById(commodity.concat('ChartHeaderType'));

		var chartOptions = {
		chart: {
			type: getChartType(typeSpan.children[0].value),
			stacked: typeSpan.children[0].value.includes('Stacked')
		},
		tooltip: {
			x: {
			format: getChartTooltipXFormat(periodSpan.children[0].value)
			}
		},
		yaxis: [{
			title: {
				text: getChartYAxisTitle(showBySpan.children[0].value, commodity)
			},
      		show: true
		}],
		xaxis: {
			type: 'datetime',
			title: {
			text: formatDate(chartDate, getChartXAxisTitleFormat(periodSpan.children[0].value))
			},
			labels: {
			format: getChartXAxisLabelFormat(periodSpan.children[0].value)
			},
			min: new Date(newCategories[0]).getTime(),
			max: new Date(newCategories[newCategories.length - 1]).getTime(),
      		categories: newCategories
		}
		};

		refreshChart(newSeries, newCategories, '#'.concat(commodity).concat('Chart'), chartOptions);
	}
</script>

<script type="text/javascript"> 
	window.onload = function(){
		resizeFinalColumns(365);

		createTree(data, "DeviceType", "electricityTreeDiv", "electricity", "updateChart(electricityChart)", true);
		createTree(data, "DeviceType", "gasTreeDiv", "gas", "updateChart(gasChart)", true);

		addExpanderOnClickEvents();
		addArrowOnClickEvents();
		addCommoditySelectorOnClickEvent();
		createCardButtons();

		createBlankChart("#electricityChart", "There's no electricity data to display. Select from the tree to the left to display");
	}

	window.onresize = function(){
		resizeFinalColumns(365);
	}	
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>