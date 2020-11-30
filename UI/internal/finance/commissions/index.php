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
							<div id="siteTree" class="tree-div">
							</div>
							<div class="tree-div scrolling-wrapper">
								<div class="expander-header">
									<span id="configureLocationSelectorSpan">Location Visibility</span>
									<i id="configureLocationSelector" class="far fa-plus-square expander-container-control show-pointer openExpander" style="float: right;"></i>
								</div>
								<div id="configureLocationSelectorList" class="expander-container">
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
						</div>
						<div style="float: right; margin-left: 15px;">
							<div class="tree-div scrolling-wrapper">
								<div class="expander-header">
									<span id="commoditySelectorSpan">Commodity</span>
									<i id="commoditySelector" class="far fa-plus-square expander-container-control show-pointer openExpander" style="float: right;"></i>
								</div>
								<div id="commoditySelectorList" class="expander-container">
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
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="resetPage()">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="Where's our money coming from?"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
				<div class="pad-container outer-container">
					<div class="expander-header">
						<span>Commission Chart</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Commission Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Commission Chart"></i>
						<i id="commissionsChart" class="far fa-plus-square expander-container-control show-pointer openExpander" style="float: right;"></i>
					</div>
					<div id="commissionsChartList" class="chart expander-container">
						<div id="commissionChart">
						</div>
					</div>
				</div>
				<div class="pad-container outer-container expander-container">
					<div class="expander-header">
						<span>Commission Data</span>
						<i id="commissionsData" class="far fa-plus-square expander-container-control show-pointer openExpander" style="float: right;"></i>
					</div>
					<div id="commissionsDataList" class="chart expander-container scrolling-wrapper">
						<div id="commissionDatagrid" style="margin: 5px;">
						</div>
					</div>
				</div>
			</div>	
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