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
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="siteDiv" class="tree-div roundborder">
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span style="padding-left: 5px;">Select Commodity <i class="far fa-plus-square" id="commoditySelector"></i></span>
				<ul class="format-listitem" id="commoditySelectorList">
					<li>
						<input type="radio" name="group1" id="allCommodityradio" checked guid="0" onclick='pageLoad();'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
					</li>
					<li>
						<input type="radio" name="group1" id="electricityCommodityradio" guid="0" onclick='pageLoad();'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
					</li>
					<li>
						<input type="radio" name="group1" id="gasCommodityradio" guid="0" onclick='pageLoad();'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="dashboardDiv" class="tree-div roundborder">
			</div>
		</div>
	</div>

	<div class="section-header">
		<i class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<br>
	<div class="final-column">
		<div id="dashboardHeader" class="dashboard roundborder" style="text-align: center; overflow: auto">
			<div style="text-align: left; margin-left: 4.25px;">
				<span>Main Dashboard <i class="far fa-plus-square" id="mainDashboardSelector"></i></span>
			</div>
			<div id="mainDashboardSelectorList">
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-industry fa-4x" style="margin-top: 2px;"></i><br>
					<span>Number of Sites</span><br><br><br><span id="dashboardHeaderNumberOfSites" style="font-size: 50px;"></span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-bolt fa-4x" style="margin-top: 2px;"></i><br>
					<span>Portfolio Annualised Energy</span><br><br><br>
					<span id="dashboardHeaderPortfolioAnnualisedEnergyUsage" style="font-size: 15px;"></span><br>
					<span id="dashboardHeaderPortfolioAnnualisedEnergyCost" style="font-size: 15px;"></span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
					<span>Extrapolated Spend</span><br><br><br><span style="font-size: 15px;">£93,306</span><br><span style="font-size: 15px;">over 3 years</span>
				</div>
				<div class="roundborder dashboard-item-large" style="overflow: auto;">
					<div id="spreadsheet" style="margin-top: 2px;"></div>
				</div>
				<div id="map-canvas" class="roundborder dashboard-item-small"></div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
					<span>Predicted Savings (£)</span><br><br><br><span style="font-size: 15px;">£9,249</span><br><span style="font-size: 15px;">over 3 years</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
					<span>Predicted Savings (£)</span><br><br><br><span style="font-size: 15px;">£3,083</span><br><span style="font-size: 15px;">per annum</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-percent fa-4x" style="margin-top: 2px;"></i><br>
					<span>Predicted Savings (%)</span><br><br><br><span style="font-size: 15px;">10%</span><br><span style="font-size: 15px;">per annum</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="far fa-lightbulb fa-4x" style="margin-top: 2px;"></i><br>
					<span>Good Practice Savings</span><br><br><br><span style="font-size: 15px;">3% </span><br><span style="font-size: 15px;">per annum</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-cloud fa-4x" style="margin-top: 2px;"></i><br>
					<span>Carbon</span><br><br><br><span id="dashboardHeaderCarbon" style="font-size: 15px;">1,550 tonnes</span><br><span style="font-size: 15px;">per annum</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Pending Opportunities</span><br><br><br>
					<span id="dashboardHeaderPendingOpportunitiesCount" style="font-size: 15px;">1</span><br>
					<span id="dashboardHeaderPendingOpportunitiesUsage" style="font-size: 15px;">100,000kWh</span><br>
					<span id="dashboardHeaderPendingOpportunitiesCost" style="font-size: 15px;">£10,000</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Active Opportunities</span><br><br><br>
					<span id="dashboardHeaderActiveOpportunitiesCount" style="font-size: 15px;">1</span><br>
					<span id="dashboardHeaderActiveOpportunitiesUsage" style="font-size: 15px;">100,000kWh</span><br>
					<span id="dashboardHeaderActiveOpportunitiesCost" style="font-size: 15px;">£10,000</span>
				</div>
				<a href="/Internal/EnergyEfficiency/FinishedOpportunities/" class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Finished Opportunities</span><br><br><br>
					<span id="dashboardHeaderFinishedOpportunitiesCount" style="font-size: 15px;">1</span><br>
					<span id="dashboardHeaderFinishedOpportunitiesUsage" style="font-size: 15px;">100,000kWh</span><br>
					<span id="dashboardHeaderFinishedOpportunitiesCost" style="font-size: 15px;">£10,000</span>
				</a>				
			</div>
		</div>
		<div style="font-size: 10px;">*Predicted savings are approximate</div>
		<br>
		<div id="customDashboard" class="dashboard roundborder" style="text-align: center; overflow: auto">
			<div style="text-align: left; margin-left: 4.25px;">
				<span>Custom Dashboard <i class="far fa-plus-square" id="customDashboardSelector"></i></span>
			</div>
			<div id="customDashboardSelectorList">
				<div id="customDashboardItem4" class="roundborder custom-dashboard-item-large">
					<div id="totalUsageChart"></div>
				</div>
				<div id="customDashboardItem0" class="roundborder custom-dashboard-item-large listitem-hidden">
					<div id="electricityPriceChart"></div>
				</div>	
				<div id="customDashboardItem1" class="roundborder custom-dashboard-item-large listitem-hidden">
					<div id="electricityUsageChart"></div>
				</div>	
				<div id="customDashboardItem2" class="roundborder custom-dashboard-item-large listitem-hidden">
					<div id="gasPriceChart"></div>
				</div>	
				<div id="customDashboardItem3" class="roundborder custom-dashboard-item-large listitem-hidden">
					<div id="gasUsageChart"></div>
				</div>	
			</div>
		</div>
		<br>
	</div>	
</body>

<script src="dashboard.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>
<script type="text/javascript" src="dashboard.json"></script>

<script>
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>