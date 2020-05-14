<?php 
	$PAGE_TITLE = "Finished Opportunities";
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
								<label class="switch"><input type="checkbox" id="projectLocationcheckbox" checked onclick='createTree("updateGraphs()", true);'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Show Sites</span>
								<label class="switch"><input type="checkbox" id="siteLocationcheckbox" checked onclick='createTree("updateGraphs()", true);'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Meters</span>
								<label class="switch"><input type="checkbox" id="meterLocationcheckbox" checked onclick='createTree("updateGraphs()", true);'></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
					<br>
					<div id="groupingOptionTree" class="dashboard roundborder outer-container">
					</div>
					<br>
					<div id="treeDisplayList" class="tree-div dashboard roundborder">
						<div class="expander-header">
							<span id="treeDisplayOrderSpan">Tree Display Order</span>
							<i id="treeDisplayOrder" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="treeDisplayOrderList" class="expander-container">
							<ul class="format-listitem listItemWithoutPadding">
								<li>
									<input type="radio" name="group1" value="Project" id="projectOrderradio" checked onclick='createTree("updateGraphs()", true); updateGraphs();'><span id="projectOrderspan" style="padding-left: 1px;">Project -> Site -> Meter</span>
								</li>
								<li>
									<input type="radio" name="group1" value="Site" id="siteOrderradio" onclick='createTree("updateGraphs()", true); updateGraphs();'><span id="siteOrderspan" style="padding-left: 1px;">Site -> Project -> Meter</span>
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
				<div class="section-header-text"><span class="tooltip"><?php echo $PAGE_TITLE ?><span class="tooltiptext">Opportunities that have been completed</span></span></div>
			</div>
			<br>
			<div class="final-column">
				<div class="dashboard outer-container tree-div">
					<div class="expander-header">
						<span>Project Financials Data</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="finishedData"></i>
					</div>
					<div id="finishedDataList" class="expander-container scrolling-wrapper" style="text-align: center;">
						<div id="spreadsheet"></div>
					</div>
				</div>
				<div class="dashboard outer-container expander-container">
					<div class="expander-header">
						<span id="cumulativeSavingspan">Project Financials Chart</span>
						<i id="cumulativeSaving" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Cumulative Saving Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Cumulative Saving Chart"></i>
					</div>
					<div id="cumulativeSavingList" class="chart roundborder expander-container">
						<div id="cumulativeSavingChart"></div>
					</div>
				</div>
				<div class="dashboard expander-container outer-container">
					<div class="expander-header">
						<span id="costSavingspan">Cost Saving Chart</span>
						<i id="costSaving" class="far fa-plus-square show-pointer expander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Cost Saving Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Cost Saving Chart"></i>
					</div>
					<div id="costSavingList"  class="listitem-hidden chart roundborder expander-container">
						<div id="costSavingChart"></div>
					</div>
				</div>
				<div class="dashboard expander-container outer-container">
					<div class="expander-header">
						<span id="volumeSavingspan">Volume Saving Chart</span>
						<i id="volumeSaving" class="far fa-plus-square show-pointer expander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Volume Saving Chart To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Volume Saving Chart"></i>
					</div>
					<div id="volumeSavingList"  class="listitem-hidden chart roundborder expander-container">
						<div id="volumeSavingChart"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="/includes/jquery-1.4.2.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="finishedopportunities.js"></script>
<script type="text/javascript" src="finishedopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>