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
							<div id="siteTree" class="tree-div ">
							</div>
							<div class="tree-div  scrolling-wrapper">
								<div class="expander-header">
									<span id="locationSelectorSpan">Location</span>
									<i id="locationSelector" class="far fa-plus-square expander show-pointer openExpander" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
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
							
						</div>
						<div style="float: right; margin-left: 15px;">
							<div id="treeDisplayList" class="tree-div">
								<div class="expander-header">
									<span id="treeDisplayOrderSpan">Tree Display Order</span>
									<i id="treeDisplayOrder" class="far fa-plus-square expander show-pointer openExpander" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
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
							<div id="projectList" class="tree-div ">
								<div class="expander-header">
									<span id="projectStatusSpan">Project Status</span>
									<i id="projectStatus" class="far fa-plus-square expander show-pointer openExpander" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
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
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="pageLoad(true)">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="Items can be added to the Dashboard using the 'Filter' icon on the left-hand side"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
				<div class="pad-container outer-container tree-div">
					<div class="expander-header">
						<span>Data</span>
						<i id="pendingAndActiveData" class="far fa-plus-square expander show-pointer openExpander" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="pendingAndActiveDataList" class="expander-container scrolling-wrapper" style="text-align: center;">
						<div id="spreadsheet"></div>
					</div>
				</div>
				<div class="pad-container outer-container tree-div expander-container">
					<div class="expander-header">
						<span>Chart</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending & Active Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending & Active Opportunities"></i>
						<i id="pendingAndActiveOpportunities" class="far fa-plus-square expander show-pointer openExpander" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="pendingAndActiveOpportunitiesList" class="expander-container srolling-wrapper">
						<div id="ganttChart"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>
<script type="text/javascript" src="/includes/jquery/jquery.js"></script>
<script type="text/javascript" src="/includes/jquery/date.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="pending&activeopportunities.js"></script>
<script type="text/javascript" src="pending&activeopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>