<?php 
	$PAGE_TITLE = "Eagle Eye";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html ng-app="dateRangeDemo" ng-controller="dateRangeCtrl">
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="dataanalysis.css">
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); updateChart(); updateDataGrid(chartSeries, categories);" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav(mySidenav)"></i>
		</div>
		<div class="tree-column">
			<div id="displayTree" class="tree-div dashboard roundborder outer-container scrolling-wrapper">
			</div>
			<br>
			<div id="configureContainer" class="tree-div dashboard roundborder outer-container scrolling-wrapper">
				<div class="expander-header">
					<span id="configureOptionsSpan">Configure</span>
					<i id="configureOptions" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureOptionsList" class="slider-list expander-container">
					<div class="dashboard roundborder outer-container scrolling-wrapper">
						<div class="expander-header">
							<span id="configureLocationSelectorSpan">Location</span>
							<i id="configureLocationSelector" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="configureLocationSelectorList" class="expander-container">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Sites</span>
								<label class="switch"><input type="checkbox" id="siteLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Areas</span>
								<label class="switch"><input type="checkbox" id="areaLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>Commodities</span>
								<label class="switch"><input type="checkbox" id="commodityLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Meters</span>
								<label class="switch"><input type="checkbox" id="meterLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>SubAreas</span>
								<label class="switch"><input type="checkbox" id="subareaLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Assets</span>
								<label class="switch"><input type="checkbox" id="assetLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>SubMeters</span>
								<label class="switch"><input type="checkbox" id="submeterLocationcheckbox" checked onclick="updatePage(this)" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
					<br>
					<div id="siteTree" class="dashboard roundborder outer-container scrolling-wrapper">
					</div>
					<br>
					<div id="groupingOptionTree" class="dashboard roundborder outer-container">
					</div>
					<br>
					<div id="timePeriodTree" class="dashboard roundborder outer-container">
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i style="font-size: 10px; vertical-align: text-top;" class="fas fa-trademark"></i></div>
			</div>
			<div class="final-column">
				<div class="dashboard outer-container">
					<div class="expander-header">
						<span id="chartHeaderSpan">Usage Chart</span>
						<i id="chartHeader" class="far fa-plus-square expander show-pointer openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Chart"></i>
					</div>
					<div id="chartHeaderList" class="roundborder chart expander-container">
						<div id="chart"></div>
					</div>
				</div>
				<div class="dashboard outer-container expander-container">
					<div class="expander-header">
						<span>Data</span>
						<i id="dataHeader" class="far fa-plus-square expander show-pointer openExpander"></i>
					</div>
					<div id="dataHeaderList" class="datagrid roundborder scrolling-wrapper expander-container" style="overflow-x: auto;">
						<div id="datagrid" style="margin: 5px;"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<link rel="stylesheet" href="/includes/rzslider/rzslider.css" />
<link data-require="bootstrap@3.3.7" data-semver="3.3.7" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<script data-require="angular.js@1.6.0" data-semver="1.6.0" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.js"></script>
<script data-require="ui-bootstrap@*" data-semver="2.2.0" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-2.2.0.js"></script>
<script src="/includes/rzslider/rzslider.js"></script>
<script src="script.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="dataanalysis.js"></script>

<script type="text/javascript" src="usage.json"></script>
<script type="text/javascript" src="cost.json"></script>
<script type="text/javascript" src="capacity.json"></script>

<script type="text/javascript" src="budget 1usage.json"></script>
<script type="text/javascript" src="budget 2 v1usage.json"></script>
<script type="text/javascript" src="budget 2 v2usage.json"></script>
<script type="text/javascript" src="budget 1cost.json"></script>
<script type="text/javascript" src="budget 2 v1cost.json"></script>
<script type="text/javascript" src="budget 2 v2cost.json"></script>

<script type="text/javascript" src="invoice 0001usage.json"></script>
<script type="text/javascript" src="invoice 0002usage.json"></script>
<script type="text/javascript" src="invoice 0003usage.json"></script>
<script type="text/javascript" src="invoice 0001cost.json"></script>
<script type="text/javascript" src="invoice 0002cost.json"></script>
<script type="text/javascript" src="invoice 0003cost.json"></script>

<script type="text/javascript" src="invoice 0001 - budget 1 varianceusage.json"></script>
<script type="text/javascript" src="invoice 0002 - budget 1 varianceusage.json"></script>
<script type="text/javascript" src="invoice 0003 - budget 1 varianceusage.json"></script>
<script type="text/javascript" src="invoice 0001 - budget 1 variancecost.json"></script>
<script type="text/javascript" src="invoice 0002 - budget 1 variancecost.json"></script>
<script type="text/javascript" src="invoice 0003 - budget 1 variancecost.json"></script>
<script type="text/javascript" src="invoice 0001 - forecast varianceusage.json"></script>
<script type="text/javascript" src="invoice 0002 - forecast varianceusage.json"></script>
<script type="text/javascript" src="invoice 0003 - forecast varianceusage.json"></script>
<script type="text/javascript" src="invoice 0001 - forecast variancecost.json"></script>
<script type="text/javascript" src="invoice 0002 - forecast variancecost.json"></script>
<script type="text/javascript" src="invoice 0003 - forecast variancecost.json"></script>

<script type="text/javascript" src="wholesale.json"></script>
<script type="text/javascript" src="distribution.json"></script>
<script type="text/javascript" src="transmission.json"></script>
<script type="text/javascript" src="renewables obligation.json"></script>
<script type="text/javascript" src="feed in tariff.json"></script>
<script type="text/javascript" src="contracts for difference.json"></script>
<script type="text/javascript" src="energy intensive industry.json"></script>
<script type="text/javascript" src="capacity market.json"></script>
<script type="text/javascript" src="balancing system use of system.json"></script>
<script type="text/javascript" src="residual cashflow reallocation cashflow.json"></script>
<script type="text/javascript" src="sundry.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>