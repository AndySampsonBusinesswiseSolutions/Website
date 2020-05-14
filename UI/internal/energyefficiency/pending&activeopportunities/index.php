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
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); updateGanttChartAndDataGrid();" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteTree" class="tree-div dashboard roundborder">
			</div>
			<br>
			<div class="tree-div dashboard roundborder scrolling-wrapper">
				<div class="expander-header">
					<span id="configureSelectorSpan">Configure</span>
					<i id="configureSelector" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureSelectorList" class="expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder scrolling-wrapper">
						<div class="expander-header">
							<span id="locationSelectorSpan">Location</span>
							<i id="locationSelector" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="locationSelectorList" class="expander-container">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Projects</span>
								<label class="switch"><input type="checkbox" id="projectsLocationcheckbox" checked onclick='createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()", true);'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Show Sites</span>
								<label class="switch"><input type="checkbox" id="sitesLocationcheckbox" checked onclick='createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()", true);'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Meters</span>
								<label class="switch"><input type="checkbox" id="metersLocationcheckbox" checked onclick='createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()", true);'></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
					<br>
					<div id="treeDisplayList" class="tree-div dashboard roundborder">
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
					<div id="projectList" class="tree-div dashboard roundborder">
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
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><span class="tooltip"><?php echo $PAGE_TITLE ?><span class="tooltiptext">Opportunities that have been approved but have not yet completed</span></span></div>
			</div>
			<div class="final-column">
				<div class="dashboard outer-container tree-div">
					<div class="expander-header">
						<span>Data</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="pendingAndActiveData"></i>
					</div>
					<div id="pendingAndActiveDataList" class="expander-container scrolling-wrapper" style="text-align: center;">
						<div id="spreadsheet"></div>
					</div>
				</div>
				<div class="dashboard outer-container tree-div expander-container">
					<div class="expander-header">
						<span>Chart</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="pendingAndActiveOpportunities"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending & Active Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending & Active Opportunities"></i>
					</div>
					<div id="pendingAndActiveOpportunitiesList" class="expander-container scrolling-wrapper">
						<div id="ganttChart"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="/includes/jquery-1.4.2.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="pending&activeopportunities.js"></script>
<script type="text/javascript" src="pending&activeopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>