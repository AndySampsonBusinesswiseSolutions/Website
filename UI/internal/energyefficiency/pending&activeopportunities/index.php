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
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); setGanttChartWidth()" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="treeDisplayList" class="sidebar-tree-div dashboard roundborder">
				<div class="expander-header">
					<span id="treeDisplayOrderSpan">Tree Display Order</span>
					<i id="treeDisplayOrder" class="far fa-plus-square expander show-pointer openExpander"></i>
				</div>
				<div id="treeDisplayOrderList" class="expander-container">
					<ul id="treeDisplayOrderList" class="format-listitem listItemWithoutPadding">
						<li>
							<input type="radio" name="group1" value="Project" id="projectOrderradio" checked onclick='createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()");'><span id="projectOrderspan" style="padding-left: 1px;">Project -> Site -> Meter</span>
						</li>
						<li>
							<input type="radio" name="group1" value="Site" id="siteOrderradio" onclick='createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()");'><span id="siteOrderspan" style="padding-left: 1px;">Site -> Project -> Meter</span>
						</li>
					</ul>
				</div>
			</div>
			<br>
			<div id="siteTree" class="sidebar-tree-div dashboard roundborder">
			</div>
			<br>
			<div id="projectList" class="sidebar-tree-div dashboard roundborder">
				<div class="expander-header">
					<span id="projectStatusSpan">Project Status</span>
					<i id="projectStatus" class="far fa-plus-square expander show-pointer openExpander"></i>
				</div>
				<div id="projectStatusList" class="expander-container">
					<ul class="format-listitem listItemWithoutPadding">
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
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
			</div>
			<br>
			<div class="final-column">
				<div class="dashboard roundborder tree-div scrolling-wrapper">
					<div id="spreadsheet"></div>
				</div>
				<div class="dashboard roundborder tree-div scrolling-wrapper expander-container">
					<div class="expander-header">
						<span>Pending & Active Opportunities Chart</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="pendingAndActiveOpportunities"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending & Active Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending & Active Opportunities"></i>
					</div>
					<div id="pendingAndActiveOpportunitiesList" class="expander-container">
						<div id="ganttChart"></div>
					</div>
				</div>
			</div>
			<br>
		</div>
	</div>
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