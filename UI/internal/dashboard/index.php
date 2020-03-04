<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="dashboard.css">
</head>

<body onload="myFunction()">
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div id="loader"></div>

	<div style="display:none;" id="myDiv" class="animate-bottom">
		<br>
		<div style="text-align: center; margin-left: 30px; margin-right: 30px;">
			<div class="roundborder">
				<div style="overflow: hidden;">
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-industry fa-4x" style="margin-top: 2px;"></i><br>
						<span>Number of Sites</span><br><span style="font-size: 15px;">1</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-bolt fa-4x" style="margin-top: 2px;"></i><br>
						<span>Portfolio Annualised Energy</span><br><span style="font-size: 15px;">Usage: 258,075 kWh</span><br><span style="font-size: 15px;">Cost: £31,102</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
						<span>3 Year Extrapolated Spend</span><br><span style="font-size: 15px;">£93,306</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
						<span>Predicted Total Savings (£)</span><br><span style="font-size: 15px;">£9,249</span><br><span style="font-size: 15px;">3 years</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
						<span>Predicted Savings (£)</span><br><span style="font-size: 15px;">£3,083</span><br><span style="font-size: 15px;">per annum</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-percent fa-4x" style="margin-top: 2px;"></i><br>
						<span>Predicted Savings (%)</span><br><span style="font-size: 15px;">10%</span><br><span style="font-size: 15px;">per annum</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="far fa-lightbulb fa-4x" style="margin-top: 2px;"></i><br>
						<span>Good Practice Savings</span><br><span style="font-size: 15px;">3% </span><br><span style="font-size: 15px;">per annum</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
						<span>Predicted Energy Savings Over 5 Years</span><br><span style="font-size: 15px;">£15,415</span>
					</div>
					<div class="roundborder" style="height: 150px; width: 150px; margin: 5px; float: left;">
						<i class="fas fa-charging-station fa-4x" style="margin-top: 2px;"></i><br>
						<span>Number of Sensors / Bridges</span><br><span style="font-size: 15px;">21 / 2</span>
					</div>
				</div>
				<br>
				<div style="overflow: hidden;">
					<div class="roundborder" style="height: 300px; width: 300px; margin: 5px; float: left;">
						<i class="fas fa-map-pin fa-9x" style="margin-top: 2px;"></i><br>
					</div>
					<div class="roundborder" style="height: 300px; width: 300px; margin: 5px; float: left;">
						<i class="fas fa-chart-pie fa-9x" style="margin-top: 2px;"></i><br>
					</div>
				</div>
			</div>
		</div>
		<div style="margin-left: 30px;">Predicted savings are approximate</div>
		<div>
			<div class="tree-column">
				<div>
					<br>
					<div class="tree-column">
						<div id="treeDiv" class="tree-div roundborder">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<br>
				<div class="dashboard roundborder" id="dashboard">
				</div>
				<br>
			</div>	
		</div>
	</div>
</body>

<script src="dashboard.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script type="text/javascript" src="dashboard.json"></script>

<script>
	loadPage();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>