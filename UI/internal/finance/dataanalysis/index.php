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
	<div id="outerContainer">
		<div id="mainContainer">
			<div id="loader">
				<div class="loader"></div>
			</div>
			<div class="section-header">
				<div id="mySidenav" class="sidenav">
					<div class="header">
						<button class="closebtn" onclick="setElementDisplayStyle(overlay, 'none'); closeNav()">Close</button>
						<i class="fas fa-filter sidenav-icon-close"></i>
					</div>
					<div class="tree-column">
						<div style="float: left;">
							<div id="displayListItemTitlespan" class="tree-div">
							</div>
							<div id="displayListItemAdditionalTitlespan" class="tree-div">
							</div>
							<div id="siteTree" class="tree-div scrolling-wrapper" style="max-height: 185px;">
							</div>
							<div id="commodityTree" class="tree-div">
							</div>
							<div id="dateRangeDisplay" class="tree-div">
							</div>
							<div id="granularityTree" class="tree-div">
							</div>
						</div>
						<div style="float: right; margin-left: 15px;">
							<div class="tree-div scrolling-wrapper">
								<div style="width: 360px;">
									<div class="expander-header">
										<span id="locationSelectorSpan">Location Visibility</span><i class="far fa-question-circle show-pointer" title="Choose which attributes to display in the 'Location' tree on the left-hand side"></i>
										<i id="locationSelector" class="far fa-plus-square expander-container-control openExpander show-pointer"></i>
									</div>
									<div id="locationSelectorList" class="expander-container">
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
							</div>
							<div id="groupingOptionTree" class="tree-div">
							</div>
							<div id="timePeriodTree" class="tree-div">
							</div>
						</div>
					</div>
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="createTrees(false); resetSlider()">Reset To Default</button>
						<button class="applybtn" onclick="doneOnClick(); setElementDisplayStyle(overlay, 'none')">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="setElementDisplayStyle(overlay, ''); openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i style="font-size: 10px; vertical-align: text-top;" class="fas fa-trademark"></i><i class="far fa-question-circle show-pointer" title="Analyse your portfolio using filters on the left-hand side"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none">
				</div>
				<div class="outer-container expander-container pad-container">
					<div class="expander-header">
						<span style="color: lightslategrey;" id="chartHeaderSpan">Usage</span>						
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Chart"></i>
						<i id="chartHeader" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
					</div>
					<div id="chartHeaderList" class="chart expander-container">
						<div id="chart"></div>
					</div>
				</div>
				<div class="outer-container expander-container pad-container">
					<div class="expander-header">
						<span style="color: lightslategrey;">Data</span>
						<i id="dataHeader" class="far fa-plus-square expander-container-control openExpander show-pointer"></i>
					</div>
					<div id="dataHeaderList" class="datagrid scrolling-wrapper expander-container" style="overflow-x: auto;">
						<div id="datagrid" style="margin: 5px;"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<link rel="stylesheet" href="/includes/rzslider/rzslider.css" />
<script data-require="angular.js@1.6.0" data-semver="1.6.0" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.js"></script>
<script data-require="ui-bootstrap@*" data-semver="2.2.0" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-2.2.0.js"></script>
<script src="/includes/rzslider/rzslider.js"></script>
<script src="script.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="dataanalysis.js"></script>

<!-- <script type="text/javascript" src="usage.json"></script>
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
<script type="text/javascript" src="sundry.json"></script> -->

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>