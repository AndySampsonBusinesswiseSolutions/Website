<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Commissions";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="commissions.css">  
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span style="padding-left: 5px;">Select Commodity <i class="far fa-plus-square show-pointer" id="commoditySelector"></i></span>
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
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<div class="final-column">
		<div>
			<br>
			<div class="dashboard roundborder" style="padding: 10px;">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Commission Chart</span>
					<i class="far fa-plus-square show-pointer" id="commissionsChart"></i>
					<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Commission Chart To Download Basket"></div>
					<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Commission Chart"></div>
				</div>
				<div id="commissionsChartList" class="roundborder chart" style="margin-top: 5px;">
					<div id="commissionChart">
					</div>
				</div>
			</div>
			<div class="dashboard roundborder scrolling-wrapper" style="margin-top: 5px;">
				<div id="commissionDatagrid" style="margin: 5px;">
				</div>
			</div>
		</div>
	</div>	
	<br>
</body>

<script src="commissions.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="commission.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>