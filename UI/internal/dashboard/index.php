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
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); updateDashboard(this);" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteDiv" class="tree-div dashboard roundborder">
			</div>
			<br>
			<div class="tree-div dashboard roundborder scrolling-wrapper">
				<div class="expander-header">
					<span id="commoditySelectorSpan">Commodity</span>
					<i id="commoditySelector" class="far fa-plus-square expander show-pointer openExpander"></i>
				</div>
				<div id="commoditySelectorList" class="expander-container">
					<div style="width: 45%; text-align: center; float: left;">
						<span>Electricity</span>
						<label class="switch"><input type="checkbox" id="electricityCommoditycheckbox" checked onclick='pageLoad();'></input><div class="switch-btn"></div></label>
					</div>
					<div style="width: 45%; text-align: center; float: right;">
						<span>Gas</span>
						<label class="switch"><input type="checkbox" id="gasCommoditycheckbox" checked onclick='pageLoad();'></input><div class="switch-btn"></div></label>
					</div>
				</div>
			</div>
			<br>
			<div id="dashboardDiv" class="tree-div dashboard roundborder">
			</div>
			<br>
			<div class="tree-div dashboard roundborder scrolling-wrapper">
				<div class="expander-header">
					<span id="configureSelectorSpan">Configure</span>
					<i id="configureSelector" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureSelectorList" class="expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder scrolling-wrapper">
						<div class="expander-header">
							<span id="locationSelectorSpan">Location</span>
							<i id="locationSelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="locationSelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Sites</span>
								<label class="switch"><input type="checkbox" id="sitesLocationcheckbox" checked onclick='pageLoad();'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Show Meters</span>
								<label class="switch"><input type="checkbox" id="metersLocationcheckbox" onclick='pageLoad();'></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
			</div>
			<div class="final-column">
				<div id="dashboardHeader" class="dashboard outer-container" style="text-align: center; overflow: auto;">
					<div class="expander-header">
						<span id="mainDashboardSelectorSpan">Main</span>
						<i id="mainDashboardSelector" class="far fa-plus-square expander show-pointer openExpander"></i>
					</div>
					<div id="mainDashboardSelectorList">
						<div style="width: 40%; float: left;">
							<a href="/Internal/PortfolioManagement/SiteManagement/" class="roundborder dashboard-item-small" style="background-color: #02601e;">
								<i class="fas fa-map-marker-alt fa-6x icon"></i><br>
								<span style="font-size: 19px;">Number of Locations Selected</span><br>
								<span id="dashboardHeaderNumberOfSites" style="font-size: 25px;"></span>
							</a>
							<a href="/Internal/Finance/DataAnalysis/"  class="roundborder dashboard-item-small" style="background-color: #663399;">
								<i class="fas fa-bolt fa-6x icon"></i><br>
								<span style="font-size: 19px;">Portfolio Annualised Usage</span><br>
								<span id="dashboardHeaderPortfolioAnnualisedEnergyUsage" style="font-size: 25px;"></span>
							</a>
							<a href="/Internal/Finance/DataAnalysis/"  class="roundborder dashboard-item-small" style="background-color: #CC3333;">
								<i class="fas fa-bolt fa-6x icon"></i><br>
								<span style="font-size: 19px;">Portfolio Annualised Cost</span><br>
								<span id="dashboardHeaderPortfolioAnnualisedEnergyCost" style="font-size: 25px;"></span>
							</a>
							<div class="roundborder dashboard-item-small" style="background-color: #CC66CC;">
								<i class="fas fa-pound-sign fa-6x icon"></i><br>
								<span style="font-size: 19px;">Savings (£)</span><br>
								<span style="font-size: 20px;">£10,571 to date</span><br>
								<span style="font-size: 20px;">£3,083 per annum</span><br>
								<span style="font-size: 20px;">£9,249 over next 3 years</span>
							</div>
							<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" class="roundborder dashboard-item-small" style="background-color: #6699CC;">
								<i class="fas fa-tools fa-6x icon"></i><br>
								<span style="font-size: 19px;">Active Opportunities</span><br>
								<span id="dashboardHeaderActiveOpportunitiesCount" style="font-size: 20px;">1</span><br>
								<span id="dashboardHeaderActiveOpportunitiesUsage" style="font-size: 19px;">100,000kWh</span><br>
								<span id="dashboardHeaderActiveOpportunitiesCost" style="font-size: 20px;">£10,000</span>
							</a>
							<div class="roundborder dashboard-item-small" style="background-color: #676762;">
								<i class="fas fa-cloud fa-6x icon"></i><br>
								<span style="font-size: 19px;">Carbon</span><br>
								<span id="dashboardHeaderCarbon" style="font-size: 25px;">1,550 tonnes per annum</span>
							</div>
						</div>
						<div style="width: 17.5%; float: right;">
							<div id="map-canvas" class="roundborder dashboard-item-large"></div>
						</div>
						<div style="width: 42.5%; float: right;">
							<div id="spreadsheetContainer" class="roundborder dashboard-item-large" style="overflow: auto;">
								<div id="spreadsheet" style="margin: 2px; float: left;"></div>
							</div>
						</div>
					</div>
				</div>
				<div style="clear: both;"></div>
				<div id="customDashboard" class="dashboard outer-container" style="text-align: center;">
					<div class="expander-header">
						<span id="customDashboardSelectorSpan">Custom</span>
						<i id="customDashboardSelector" class="far fa-plus-square expander show-pointer openExpander"></i>
					</div>
					<div id="customDashboardSelectorList">
						<div id="customDashboardItem11" class="roundborder custom-dashboard-item-large">
							<div id="totalUsageChart"></div>
						</div>
						<div id="customDashboardItem00" class="roundborder custom-dashboard-item-large">
							<div id="electricityPriceChart"></div>
						</div>	
						<div id="customDashboardItem01" class="roundborder custom-dashboard-item-large listitem-hidden">
							<div id="electricityUsageChart"></div>
						</div>	
						<div id="customDashboardItem02" class="roundborder custom-dashboard-item-large listitem-hidden">
							<div id="gasPriceChart"></div>
						</div>	
						<div id="customDashboardItem03" class="roundborder custom-dashboard-item-large listitem-hidden">
							<div id="gasUsageChart"></div>
						</div>	
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script src="dashboard.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>
<script type="text/javascript" src="dashboard.json"></script>

<script>
	pageLoad(true);
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>