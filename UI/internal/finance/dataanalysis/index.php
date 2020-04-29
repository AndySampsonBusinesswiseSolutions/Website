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
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); updateChart(); updateDataGrid(chartSeries, categories);" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav(mySidenav)"></i>
		<div class="tree-column">
			<div class="dashboard roundborder" style="padding: 10px;">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span id="selectOptionsSpan">Select Options</span>
					<div id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></div>
				</div>
				<div id="selectOptionsList" style="margin-top: 5px;">
					<div id="displayTree" class="sidebar-tree-div roundborder scrolling-wrapper">
					</div>
					<br>
					<div id="siteTree" class="sidebar-tree-div roundborder scrolling-wrapper">
					</div>
					<br>
					<div id="budgetTree" class="sidebar-tree-div roundborder scrolling-wrapper">
					</div>
					<br>			
					<div id="invoiceTree" class="sidebar-tree-div roundborder scrolling-wrapper">
					</div>
					<br>
					<div id="groupingOptionTree" class="sidebar-tree-div roundborder">
					</div>
				</div>
			</div>
			<br>
			<div class="dashboard roundborder" style="padding: 10px;">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span id="filterOptionsSpan">Filter Options</span>
					<div id="filterOptions" class="far fa-plus-square expander show-pointer"></div>
				</div>
				<div id="filterOptionsList" class="slider-list" style="margin-top: 5px;">
					<div id="commodityTree" class="sidebar-tree-div roundborder">
					</div>
					<br>
					<div id="timePeriodTree" class="sidebar-tree-div roundborder">
					</div>
				</div>
			</div>
			<br>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?><i style="font-size: 10px; vertical-align: text-top;" class="fas fa-trademark"></i></div>
	</div>
	<br>
	<div class="final-column">
		<div class="dashboard roundborder" style="padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="chartHeaderSpan">Usage Chart</span>
				<div id="chartHeader" class="far fa-plus-square expander show-pointer openExpander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Chart"></div>
			</div>
			<div id="chartHeaderList" class="roundborder chart" style="margin-top: 5px;">
				<div id="chart"></div>
			</div>
		</div>
		<div class="dashboard roundborder" style="padding: 10px; margin-top: 5px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Data</span>
				<div id="dataHeader" class="far fa-plus-square expander show-pointer openExpander"></div>
			</div>
			<div id="dataHeaderList" class="roundborder datagrid scrolling-wrapper" style="margin-top: 5px; overflow-x: auto;">
				<div id="datagrid" style="margin: 5px;"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/includes/base.js"></script>

<link rel="stylesheet" href="rzslider.css" />
<link data-require="bootstrap@3.3.7" data-semver="3.3.7" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<script data-require="angular.js@1.6.0" data-semver="1.6.0" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.js"></script>
<script data-require="ui-bootstrap@*" data-semver="2.2.0" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-2.2.0.js"></script>
<script src="rzslider.js"></script>
<script src="script.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
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