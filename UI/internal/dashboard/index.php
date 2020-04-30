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
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="selectOptionsSpan">Select Options</span>
					<div id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></div>
				</div>
				<div id="selectOptionsList" class="expander-container">
					<div id="siteDiv" class="tree-div roundborder">
					</div>
					<br>
					<div class="sidebar-tree-div roundborder scrolling-wrapper">
						<div class="expander-header">
							<span id="commoditySelectorSpan">Commodity</span>
							<div id="commoditySelector" class="far fa-plus-square expander show-pointer openExpander"></div>
						</div>
						<div id="commoditySelectorList" class="expander-container">
							<ul class="format-listitem toplistitem">
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
					</div>
					<br>
					<div id="dashboardDiv" class="tree-div roundborder">
					</div>
				</div>
			</div>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<br>
	<div class="final-column">
		<div id="dashboardHeader" class="dashboard roundborder outer-container" style="text-align: center; overflow: auto;">
			<div class="expander-header">
				<span id="mainDashboardSelectorSpan">Main Dashboard</span>
				<div id="mainDashboardSelector" class="far fa-plus-square expander show-pointer openExpander"></div>
			</div>
			<div id="mainDashboardSelectorList">
				<a href="/Internal/PortfolioManagement/SiteManagement/" class="roundborder dashboard-item-small">
					<i class="fas fa-industry fa-4x" style="margin-top: 2px;"></i><br>
					<span>Number of Sites</span><br><br><br><span id="dashboardHeaderNumberOfSites" style="font-size: 50px;"></span>
				</a>
				<a href="/Internal/PortfolioManagement/ViewMeterConsumption/"  class="roundborder dashboard-item-small">
					<i class="fas fa-bolt fa-4x" style="margin-top: 2px;"></i><br>
					<span>Portfolio Annualised Energy</span><br><br><br>
					<span id="dashboardHeaderPortfolioAnnualisedEnergyUsage" style="font-size: 15px;"></span><br>
					<span id="dashboardHeaderPortfolioAnnualisedEnergyCost" style="font-size: 15px;"></span>
				</a>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-pound-sign fa-4x" style="margin-top: 2px;"></i><br>
					<span>Extrapolated Spend</span><br><br><br><span style="font-size: 15px;">£93,306</span><br><span style="font-size: 15px;">over 3 years</span>
				</div>
				<div class="roundborder dashboard-item-large" style="overflow: auto;">
					<div id="spreadsheet" style="margin: 2px; float: left;"></div>
					<div class="fas fa-download show-pointer" style="margin-top: 5px; float: left;" title="Download Site List"></div>
					<div class="fas fa-cart-arrow-down show-pointer" style="margin-top: 5px; float: left;" title="Add Site List To Download Basket"></div>
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
				<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Pending Opportunities</span><br><br><br>
					<span id="dashboardHeaderPendingOpportunitiesCount" style="font-size: 15px;">1</span><br>
					<span id="dashboardHeaderPendingOpportunitiesUsage" style="font-size: 15px;">100,000kWh</span><br>
					<span id="dashboardHeaderPendingOpportunitiesCost" style="font-size: 15px;">£10,000</span>
				</a>
				<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Active Opportunities</span><br><br><br>
					<span id="dashboardHeaderActiveOpportunitiesCount" style="font-size: 15px;">1</span><br>
					<span id="dashboardHeaderActiveOpportunitiesUsage" style="font-size: 15px;">100,000kWh</span><br>
					<span id="dashboardHeaderActiveOpportunitiesCost" style="font-size: 15px;">£10,000</span>
				</a>
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
		<div id="customDashboard" class="dashboard roundborder outer-container" style="text-align: center; overflow: auto; margin-top: 15px;">
			<div class="expander-header">
				<span id="customDashboardSelectorSpan">Custom Dashboard</span>
				<div id="customDashboardSelector" class="far fa-plus-square expander show-pointer openExpander"></div>
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

<script src="/includes/base.js"></script>

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