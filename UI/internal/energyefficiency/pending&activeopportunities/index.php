<?php 
	$PAGE_TITLE = "Pending & Active Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
	
	<link rel="stylesheet" href="pending&activeopportunities.css">
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Tree Display Order</span>
				<div id="treeDisplayOrder" class="far fa-plus-square show-pointer expander openExpander"></div>
				<ul id="treeDisplayOrderList" class="format-listitem">
					<li>
						<input type="radio" name="group1" value="Project" id="projectOrderradio" checked onclick='createTree(activeopportunity, "treeDiv", "updateGanttChartAndDataGrid()");'><span id="projectOrderspan" style="padding-left: 1px;">Project -> Site -> Meter</span>
					</li>
					<li>
						<input type="radio" name="group1" value="Site" id="siteOrderradio" onclick='createTree(activeopportunity, "treeDiv", "updateGanttChartAndDataGrid()");'><span id="siteOrderspan" style="padding-left: 1px;">Site -> Project -> Meter</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<br>
			<div class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Project Status</span>
				<div id="projectStatus" class="far fa-plus-square show-pointer expander openExpander"></div>
					<ul id="projectStatusList" class="format-listitem">
						<li>
							<input type="radio" name="group2" value="All" id="allStatusradio" checked onclick='updateGanttChartAndDataGrid()'><span id="allStatusspan" style="padding-left: 1px;">All</span>
						</li>
						<li>
							<input type="radio" name="group2" value="Pending" id="pendingStatusradio" onclick='updateGanttChartAndDataGrid()'><span id="pendingStatusspan" style="padding-left: 1px;">Pending</span>
						</li>
						<li>
							<input type="radio" name="group2" value="Active" id="activeStatusradio" onclick='updateGanttChartAndDataGrid()'><span id="activeStatusspan" style="padding-left: 1px;">Active</span>
						</li>
					</ul>
			</div>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<br>
	<div class="final-column">
		<div class="roundborder tree-div scrolling-wrapper">
			<div id="spreadsheet"></div>
		</div>
		<div style="margin-top: 5px;"></div>
		<div class="roundborder tree-div scrolling-wrapper">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Pending & Active Opportunities Chart</span>
				<i class="far fa-plus-square show-pointer expander openExpander" id="pendingAndActiveOpportunities"></i>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Pending & Active Opportunities To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Pending & Active Opportunities"></div>
			</div>
			<div id="pendingAndActiveOpportunitiesList" style="margin-top: 5px;">
				<div id="ganttChart"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="jquery-1.4.2.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="pending&activeopportunities.js"></script>
<script type="text/javascript" src="pending&activeopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>