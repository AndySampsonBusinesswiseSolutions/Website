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
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="selectOptionsSpan">Select Options</span>
					<div id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></div>
				</div>
				<div id="selectOptionsList" class="expander-container">
					<div id="treeDisplayList" class="sidebar-tree-div roundborder dashboard">
						<div class="expander-header">
							<span id="treeDisplayOrderSpan">Tree Display Order</span>
							<div id="treeDisplayOrder" class="far fa-plus-square expander show-pointer openExpander"></div>
						</div>
						<div id="treeDisplayOrderList" class="expander-container">
							<ul class="format-listitem toplistitem">
								<li>
									<input type="radio" name="group1" value="Project" id="projectOrderradio" checked onclick='createTree(activeopportunity, "siteTree", "");'><span id="projectOrderspan" style="padding-left: 1px;">Project -> Site -> Meter</span>
								</li>
								<li>
									<input type="radio" name="group1" value="Site" id="siteOrderradio" onclick='createTree(activeopportunity, "siteTree", "");'><span id="siteOrderspan" style="padding-left: 1px;">Site -> Project -> Meter</span>
								</li>
							</ul>
						</div>
					</div>
					<br>
					<div id="siteTree" class="sidebar-tree-div roundborder">
					</div>
				</div>
			</div>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>
	<br>
	<div class="final-column">
		<div class="roundborder dashboard scrolling-wrapper outer-container">
			<div id="spreadsheet"></div>
		</div>
		<div class="roundborder dashboard outer-container expander-container">
			<div class="expander-header">
				<span id="cumulativeSavingspan">Cumulative Saving Chart</span>
				<div id="cumulativeSaving" class="far fa-plus-square show-pointer expander openExpander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="margin-right: 5px; float: right;" title="Add Cumulative Saving Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="float: right;" title="Download Cumulative Saving Chart"></div>
			</div>
			<div id="cumulativeSavingList" class="tree-div roundborder expander-container">
				<div id="cumulativeSavingChart"></div>
			</div>
		</div>
		<div class="roundborder dashboard expander-container outer-container">
			<div class="expander-header">
				<span id="costSavingspan">Cost Saving Chart</span>
				<div id="costSaving" class="far fa-plus-square show-pointer expander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="margin-right: 5px; float: right;" title="Add Cost Saving Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="float: right;" title="Download Cost Saving Chart"></div>
			</div>
			<div id="costSavingList"  class="listitem-hidden tree-div roundborder expander-container">
				<div id="costSavingChart"></div>
			</div>
		</div>
		<div class="roundborder dashboard expander-container outer-container">
			<div class="expander-header">
				<span id="volumeSavingspan">Volume Saving Chart</span>
				<div id="volumeSaving" class="far fa-plus-square show-pointer expander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="margin-right: 5px; float: right;" title="Add Volume Saving Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="float: right;" title="Download Volume Saving Chart"></div>
			</div>
			<div id="volumeSavingList"  class="listitem-hidden tree-div roundborder expander-container">
				<div id="volumeSavingChart"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/includes/base.js"></script>

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