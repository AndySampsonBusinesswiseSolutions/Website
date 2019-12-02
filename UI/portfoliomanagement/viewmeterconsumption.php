<?php 
	$PAGE_TITLE = "View Meter Consumption";
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
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/electricitytree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/electricitychart.php") ?>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/gastree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/gaschart.php") ?>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/chart.js"></script>
<script src="/javascript/tree.js"></script>
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
		//loop through tree div
		//get all checkboxes
		//find all checked
		//work out what to show
		//design options
		//push values

		testChart(chart);
	}
</script>

<script type="text/javascript"> 
	createTree(data, "DeviceType", "electricityTreeDiv", "electricity", "updateChart(electricityChart)");
	createTree(data, "DeviceType", "gasTreeDiv", "gas", "updateChart(gasChart)");
	addExpanderOnClickEvents();
	addArrowOnClickEvents();
	addCommoditySelectorOnClickEvent();	

	window.onload = function(){
		resizeCharts(365);
	}

	window.onresize = function(){
		resizeCharts(365);
	}

	createBlankChart("#electricityChart", "There's no electricity data to display. Select from the tree to the left to display");
	createBlankChart("#gasChart", "There's no gas data to display. Select from the tree to the left to display");
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>