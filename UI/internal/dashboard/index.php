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

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div>
		<br>
		<div>
			<div class="tree-column">
				<div>
					<div class="tree-column">
						<div id="siteDiv" class="tree-div roundborder">
						</div>
						<br>
						<div class="roundborder" style="background-color: #e9eaee;">
							<span style="padding-left: 5px;">Select Commodity</span>
							<ul class="format-listitem">
								<li>
									<input type="radio" name="group1" id="allCommodityradio" checked guid="0" onclick='createTree(data, "siteDiv", "", "Site");'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
								</li>
								<li>
									<input type="radio" name="group1" id="electricityCommodityradio" guid="0" onclick='createTree(data, "siteDiv", "", "Site");'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
								</li>
								<li>
									<input type="radio" name="group1" id="gasCommodityradio" guid="0" onclick='createTree(data, "siteDiv", "", "Site");'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
								</li>
							</ul>
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div id="dashboardHeader" class="dashboard roundborder" style="text-align: center; overflow: auto">
					<div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-industry fa-4x" style="margin-top: 2px;"></i><br>
							<span>Number of Sites</span><br><br><span id="dashboardHeaderNumberOfSites" style="font-size: 15px;">7</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-bolt fa-4x" style="margin-top: 2px;"></i><br>
							<span>Portfolio Annualised Energy</span><br><br>
							<span id="dashboardHeaderPortfolioAnnualisedEnergyUsage" style="font-size: 15px;">Usage: 5,653,691kWh</span><br>
							<span id="dashboardHeaderPortfolioAnnualisedEnergyCost" style="font-size: 15px;">Cost: £704,567</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
							<span>Extrapolated Spend</span><br><br><span style="font-size: 15px;">£93,306</span><br><span style="font-size: 15px;">over 3 years</span>
						</div>
						<div id="map-canvas" class="roundborder" style="height: 360px; width: 360px; margin: 5px; float: right;"></div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
							<span>Predicted Savings (£)</span><br><br><span style="font-size: 15px;">£15,415</span><br><span style="font-size: 15px;">over 5 years</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
							<span>Predicted Savings (£)</span><br><br><span style="font-size: 15px;">£9,249</span><br><span style="font-size: 15px;">over 3 years</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
							<span>Predicted Savings (£)</span><br><br><span style="font-size: 15px;">£3,083</span><br><span style="font-size: 15px;">per annum</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-percent fa-4x" style="margin-top: 2px;"></i><br>
							<span>Predicted Savings (%)</span><br><br><span style="font-size: 15px;">10%</span><br><span style="font-size: 15px;">per annum</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="far fa-lightbulb fa-4x" style="margin-top: 2px;"></i><br>
							<span>Good Practice Savings</span><br><br><span style="font-size: 15px;">3% </span><br><span style="font-size: 15px;">per annum</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-cloud fa-4x" style="margin-top: 2px;"></i><br>
							<span>Carbon</span><br><br><span id="dashboardHeaderCarbon" style="font-size: 15px;">1,550 tonnes</span><br><span style="font-size: 15px;">per annum</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span>Pending Opportunities</span><br><br>
							<span id="dashboardHeaderPendingOpportunitiesCount" style="font-size: 12px;">1</span><br>
							<span id="dashboardHeaderPendingOpportunitiesUsage" style="font-size: 12px;">100,000kWh</span><br>
							<span id="dashboardHeaderPendingOpportunitiesCost" style="font-size: 12px;">£10,000</span>
						</div>
						<div class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span>Active Opportunities</span><br><br>
							<span id="dashboardHeaderActiveOpportunitiesCount" style="font-size: 12px;">1</span><br>
							<span id="dashboardHeaderActiveOpportunitiesUsage" style="font-size: 12px;">100,000kWh</span><br>
							<span id="dashboardHeaderActiveOpportunitiesCost" style="font-size: 12px;">£10,000</span>
						</div>
						<a href="/Internal/EnergyEfficiency/FinishedOpportunities/" class="roundborder" style="height: 175px; width: 175px; margin: 5px; float: left;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span>Finished Opportunities</span><br><br>
							<span id="dashboardHeaderFinishedOpportunitiesCount" style="font-size: 12px;">1</span><br>
							<span id="dashboardHeaderFinishedOpportunitiesUsage" style="font-size: 12px;">100,000kWh</span><br>
							<span id="dashboardHeaderFinishedOpportunitiesCost" style="font-size: 12px;">£10,000</span>
						</a>
						<div class="roundborder" style="height: 270px; width: 490px; margin: 5px; float: left;">
							<span>Predictive Energy Spend & Savings Over 3 Years</span>
							<div id="piechart"></div>
						</div>
						<div class="roundborder" style="height: 270px; width: 490px; margin: 5px; float: left; overflow: auto;">
							<div id="spreadsheet"></div>
						</div>
						<div class="roundborder" style="height: 270px; width: 490px; margin: 5px; float: left;">
							<div id="electricityVolumeChart"></div>
						</div>
					</div>
				</div>
				<div>Predicted savings are approximate</div>
			</div>	
		</div>
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
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>
<script type="text/javascript" src="dashboard.json"></script>

<script>
	loadPage();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>