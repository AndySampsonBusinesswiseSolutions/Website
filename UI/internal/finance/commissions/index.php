<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Commissions";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="commissions.css">  
</head>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<div>
					<br>
					<div class="tree-column">
						<div id="electricityTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<br>
					<div class="chart">
						<div id="electricityChart">
						</div>
					</div>
					<br>
					<div id="electricityDatagrid" class="datagrid scrolling-wrapper">
					</div>
				</div>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<div>
					<br>
					<div class="tree-column">
						<div id="gasTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<br>
					<div class="chart">
						<div id="gasChart">
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="commissions.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="commission.json"></script>

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
	window.onload = function(){
		createTree(data, "electricityTreeDiv", "electricity", "updateChart(electricityChart)");
		//createTree(data, "gasTreeDiv", "gas", "updateChart(gasChart)");
		addExpanderOnClickEvents();
		addArrowOnClickEvents();
		addCommoditySelectorOnClickEvent();

		updateChart(null, electricityChart);
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>