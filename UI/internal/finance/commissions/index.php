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
				<span style="padding-left: 5px;">Select Commodity <i class="far fa-plus-square" id="commoditySelector"></i></span>
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
			<div class="roundborder chart">
				<div id="commissionChart">
				</div>
			</div>
			<br>
			<div id="commissionDatagrid" class="roundborder scrolling-wrapper">
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