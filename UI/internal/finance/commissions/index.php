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

	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<div>
					<br>
					<div class="tree-column">
						<div id="electricityTreeDiv" class="tree-div">
						</div>
						<br>
						<div class="datagrid" style="background-color: #e9eaee;">
							<span style="padding-left: 5px;">Select Commodity</span>
							<ul class="format-listitem">
								<li>
									<input type="radio" name="group2" id="allCommodityradio" checked guid="0" onclick='createTree(data, "electricityTreeDiv", "", "updateChart(electricityChart)");addExpanderOnClickEvents();'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
								</li>
								<li>
									<input type="radio" name="group2" id="electricityCommodityradio" guid="0" onclick='createTree(data, "electricityTreeDiv", "electricity", "updateChart(electricityChart)");addExpanderOnClickEvents();'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
								</li>
								<li>
									<input type="radio" name="group2" id="gasCommodityradio" guid="0" onclick='createTree(data, "electricityTreeDiv", "gas", "updateChart(electricityChart)");addExpanderOnClickEvents();'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
								</li>
							</ul>
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
	<br>
</body>

<script src="commissions.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="commission.json"></script>

<script type="text/javascript"> 
	window.onload = function(){
		createTree(data, "electricityTreeDiv", "", "updateChart(electricityChart)");
		addExpanderOnClickEvents();
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>