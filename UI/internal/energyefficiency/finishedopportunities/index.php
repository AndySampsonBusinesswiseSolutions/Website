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
							<div class="tree-div dashboard scrolling-wrapper">
								<div class="expander-header">
									<span id="locationSelectorSpan">Location Visibilty</span>
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
						</div>
						<div style="float: right; margin-left: 15px;">
							<div id="groupingOptionTree" class="tree-div">
							</div>
							<div id="treeDisplayList" class="tree-div">
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
					<div id="cumulativeSavingList" class="chart expander-container">
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
					<div id="costSavingList"  class="listitem-hidden chart expander-container">
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
					<div id="volumeSavingList"  class="listitem-hidden chart expander-container">
						<div id="volumeSavingChart"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>
<script type="text/javascript" src="/includes/jquery/jquery.js"></script>
<script type="text/javascript" src="/includes/jquery/date.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="finishedopportunities.js"></script>
<script type="text/javascript" src="finishedopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>