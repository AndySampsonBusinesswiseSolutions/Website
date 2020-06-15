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
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<div id="mySidenav" class="sidenav" style="display: none;">
					<div class="header">
						<button class="closebtn" onclick="closeNav()">Close</button>
						<i class="fas fa-filter sidenav-icon-close"></i>
					</div>
					<div class="tree-column">
						<div style="float: left;">
							<div id="siteDiv" class="tree-div ">
							</div>
							<div class="tree-div  scrolling-wrapper">
								<div class="expander-header">
									<span id="commoditySelectorSpan">Commodity</span><i class="far fa-question-circle show-pointer" title="Choose whether to display Electricity and/or Gas Sites/Meters in the 'Location' tree above"></i>
									<i id="commoditySelector" class="far fa-plus-square expander-container-control show-pointer openExpander"></i>
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
						</div>
						<div style="float: right; margin-left: 15px;">
							<div class="tree-div scrolling-wrapper">
								<div class="expander-header">
									<span id="locationSelectorSpan">Location Visibility</span><i class="far fa-question-circle show-pointer" title="Choose whether to display Sites and/or Meters in the 'Location' tree on the left-hand side"></i>
									<i id="locationSelector" class="far fa-plus-square expander-container-control openExpander show-pointer"></i>
								</div>
								<div id="locationSelectorList" class="expander-container">
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
							<div id="dashboardDiv" class="tree-div">
							</div>
						</div>
					</div>
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="pageLoad(true)">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="Items can be added to the Dashboard using the 'Filter' icon on the left-hand side"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
				<div id="dashboardHeader" class="pad-container" style="text-align: center;">
					<div style="width: 40%; float: left;">
						<a href="/Internal/PortfolioManagement/SiteManagement/" class="dashboard-item-small" style="background-color: #3b7e84; margin-right: 9px;">
							<img src="/images/location.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Locations Selected</span><br>
							<span id="dashboardHeaderNumberOfSites" class="dashboard-item-text"></span>
						</a>
						<a href="/Internal/Finance/DataAnalysis/"  class="dashboard-item-small" style="background-color: #69566c; margin-right: 9px;">
							<img src="/images/lightening bolt.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Portfolio Usage p.a</span><br>
							<span id="dashboardHeaderPortfolioAnnualisedEnergyUsage" class="dashboard-item-text"></span>
						</a>
						<a href="/Internal/Finance/DataAnalysis/"  class="dashboard-item-small" style="background-color: #97a3af">
							<img src="/images/money6.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Portfolio Cost p.a</span><br>
							<span id="dashboardHeaderPortfolioAnnualisedEnergyCost" class="dashboard-item-text"></span>
						</a>
						<div class="dashboard-item-small" style="background-color: #97a3af; margin-right: 9px; margin-top: 9px;">
							<img src="/images/money.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Savings</span><br>
							<div class="dashboard-item-text">
								<span>£10,571 to date</span><br>
								<span>£9,249 over next 3 years</span>
							</div>
						</div>
						<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" class="dashboard-item-small" style="background-color: #333333; margin-right: 9px; margin-top: 9px;">
							<img src="/images/wrench.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Opportunities</span><br>
							<div class="dashboard-item-text">
								<span id="dashboardHeaderActiveOpportunitiesCount"></span><br>
								<span id="dashboardHeaderActiveOpportunitiesCost">£10,000</span>
							</div>
						</a>
						<div class="dashboard-item-small" style="background-color: #3b7e84; margin-top: 9px;">
							<img src="/images/cloud.png" class="dashboard-icon"></img><br>
							<span class="dashboard-item-header">Carbon</span><br>
							<span id="dashboardHeaderCarbon" class="dashboard-item-text">1,550 tonnes p.a</span>
						</div>
					</div>
					<div style="width: calc(42.5% - 9px); float: left;">
						<div id="spreadsheetContainer" class="dashboard-item-large" style="overflow: auto; background-color: white;">
							<div id="spreadsheet" style="float: left; margin-left: 9px; margin-top: 9px;"></div>
						</div>
					</div>
					<div style="width: 17.5%; float: left; margin-left: 9px;">
						<div id="map-canvas" class=" dashboard-item-large"></div>
					</div>
					<div style="clear: both;"></div>
				</div>
				<div id="customDashboard" class="pad-container">
					<div id="customDashboardItem11" class=" custom-dashboard-item-large" style="margin-right: 9px;">
						<div id="totalUsageChart"></div>
					</div>
					<div id="customDashboardItem00" class=" custom-dashboard-item-large">
						<div id="electricityPriceChart"></div>
					</div>	
					<div id="customDashboardItem01" class=" custom-dashboard-item-large listitem-hidden">
						<div id="electricityUsageChart"></div>
					</div>	
					<div id="customDashboardItem02" class=" custom-dashboard-item-large listitem-hidden">
						<div id="gasPriceChart"></div>
					</div>	
					<div id="customDashboardItem03" class=" custom-dashboard-item-large listitem-hidden">
						<div id="gasUsageChart"></div>
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