<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Revenue Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteTree" class="tree-div dashboard roundborder outer-container">
			</div>
			<br>
			<div id="configureContainer" class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="configureOptionsSpan">Configure</span>
					<i id="configureOptions" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureOptionsList" class="slider-list expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder outer-container scrolling-wrapper">
						<div class="expander-header">
							<span id="configureLocationSelectorSpan">Location</span>
							<i id="configureLocationSelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="configureLocationSelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Suppliers</span>
								<label class="switch"><input type="checkbox" id="supplierLocationcheckbox" checked onclick='createTree(data, "siteTree", "updateChart(commissionChart)");'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Customers</span>
								<label class="switch"><input type="checkbox" id="customerLocationcheckbox" checked onclick='createTree(data, "siteTree", "updateChart(commissionChart)");'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>Meters</span>
								<label class="switch"><input type="checkbox" id="meterLocationcheckbox" checked onclick='createTree(data, "siteTree", "updateChart(commissionChart)");'></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
					<br>
					<div class="tree-div dashboard roundborder outer-container scrolling-wrapper">
						<div class="expander-header">
							<span id="commoditySelectorSpan">Commodity</span>
							<i id="commoditySelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="commoditySelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Electricity</span>
								<label class="switch"><input type="checkbox" id="electricityCommoditycheckbox" checked onclick='createTree(data, "siteTree", "updateChart(commissionChart)");'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Gas</span>
								<label class="switch"><input type="checkbox" id="gasCommoditycheckbox" checked onclick='createTree(data, "siteTree", "updateChart(commissionChart)");'></input><div class="switch-btn"></div></label>
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
				<div class="dashboard outer-container">
					<div class="expander-header">
						<span>Commission Chart</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="commissionsChart"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Commission Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Commission Chart"></i>
					</div>
					<div id="commissionsChartList" class="roundborder chart expander-container">
						<div id="commissionChart">
						</div>
					</div>
				</div>
				<div class="dashboard outer-container expander-container">
					<div class="expander-header">
						<span>Commission Data</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="commissionsData"></i>
					</div>
					<div id="commissionsDataList" class="roundborder chart expander-container scrolling-wrapper">
						<div id="commissionDatagrid" style="margin: 5px;">
						</div>
					</div>
				</div>
			</div>	
			<br>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script src="commissions.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="commission.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>