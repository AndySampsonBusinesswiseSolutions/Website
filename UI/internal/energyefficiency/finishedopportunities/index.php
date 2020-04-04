<?php 
	$PAGE_TITLE = "Finished Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="finishedopportunities.css">
</head>

<body>
	<div id="mySidenav" class="sidenav">	
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Tree Display Order</span>
				<div id="treeDisplayOrder" class="far fa-plus-square show-pointer"></div>
				<ul id="treeDisplayOrderList" class="format-listitem">
					<li>
						<input type="radio" name="group1" value="Project" id="projectOrderradio" checked onclick='createTree(activeopportunity, "treeDiv", "");'><span id="projectOrderspan" style="padding-left: 1px;">Project -> Site -> Meter</span>
					</li>
					<li>
						<input type="radio" name="group1" value="Site" id="siteOrderradio" onclick='createTree(activeopportunity, "treeDiv", "");'><span id="siteOrderspan" style="padding-left: 1px;">Site -> Project -> Meter</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="treeDiv" class="tree-div roundborder">
			</div>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>
	<br>
	<div class="final-column">
		<div id="spreadsheet"></div>
		<br>
		<div class="roundborder dashboard" style="padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="cumulativeSavingspan">Cumulative Saving Chart</span>
				<div id="cumulativeSaving" class="far fa-plus-square show-pointer"></div>
			</div>
			<div id="cumulativeSavingList" class="listitem-hidden tree-div roundborder" style="margin-top: 5px;">
				<div id="cumulativeSavingChart"></div>
			</div>
		</div>
		<div class="roundborder dashboard" style="margin-top: 5px; padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="costSavingspan">Cost Saving Chart</span>
				<div id="costSaving" class="far fa-plus-square show-pointer"></div>
			</div>
			<div id="costSavingList"  class="listitem-hidden tree-div roundborder" style="margin-top: 5px;">
				<div id="costSavingChart"></div>
			</div>
		</div>
		<div class="roundborder dashboard" style="margin-top: 5px; padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="volumeSavingspan">Volume Saving Chart</span>
				<div id="volumeSaving" class="far fa-plus-square show-pointer"></div>
			</div>
			<div id="volumeSavingList"  class="listitem-hidden tree-div roundborder" style="margin-top: 5px;">
				<div id="volumeSavingChart"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="jquery-1.4.2.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="finishedopportunities.js"></script>
<script type="text/javascript" src="finishedopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>