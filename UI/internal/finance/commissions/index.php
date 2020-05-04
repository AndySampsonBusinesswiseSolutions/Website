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
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="selectOptionsSpan">Select Options</span>
					<i id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></i>
				</div>
				<div id="selectOptionsList" class="expander-container">
					<div id="siteTree" class="sidebar-tree-div roundborder">
					</div>
					<br>
					<div id="commodityList" class="sidebar-tree-div roundborder">
						<div class="expander-header">
							<span id="commoditySelectorSpan">Commodity</span>
							<i id="commoditySelector" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="commoditySelectorList" class="expander-container">
							<ul class="format-listitem toplistitem">
								<li>
									<input type="radio" name="commoditySelector" id="allCommodityradio" checked><span id="allCommodityspan">All</span>
								</li>
								<li>
									<input type="radio" name="commoditySelector" id="electricityCommodityradio"><span id="electricityCommodityspan">Electricity</span>
								</li>
								<li>
									<input type="radio" name="commoditySelector" id="gasCommodityradio"><span id="gasCommodityspan">Gas</span>
								</li>
							</ul>
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
				<div>
					<br>
					<div class="dashboard roundborder outer-container">
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
					<div class="dashboard roundborder scrolling-wrapper expander-container">
						<div id="commissionDatagrid" style="margin: 5px;">
						</div>
					</div>
				</div>
			</div>	
			<br>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script src="commissions.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="commission.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>