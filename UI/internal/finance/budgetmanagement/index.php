<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Budget Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html ng-app="timePeriodControlDemo" ng-controller="timePeriodControl">
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="budgetmanagement.css">
</head>

<body>
	<div id="popup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="title"></span>
			</div>
			<br>
			<span id="text" style="font-size: 15px;"></span><br><br>
			<button style="float: right;" class="reject" id="button">Delete Attribute</button>
			<br>
		</div>
	</div>
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
					<div id="budgetList" class="sidebar-tree-div roundborder dashboard">
						<div class="expander-header">
							<span id="budgetSelectorSpan">Budget</span>
							<div id="budgetSelector" class="far fa-plus-square expander show-pointer openExpander"></div>
						</div>
						<div id="budgetSelectorList" class="expander-container">
							<ul class="format-listitem toplistitem">
								<li>
									<i class="far fa-plus-square show-pointer" id="budget2Selector"></i>
									<span id="allCommodityspan">Budget 2</span>
									<ul class="format-listitem listitem-hidden" id="budget2SelectorList">
										<li>
											<i class="far fa-times-circle show-pointer" id="budget2Selector"></i>
											<input type="radio" name="budgetSelector" id="allCommodityradio">
											<span id="allCommodityspan">Version 2</span>
											<i class="far fa-question-circle show-pointer" onclick="displayBudget2V2Popup();"></i>
										</li>
										<li>
											<i class="far fa-times-circle show-pointer" id="budget2Selector"></i>
											<input type="radio" name="budgetSelector" id="allCommodityradio">
											<span id="allCommodityspan">Version 1</span>
											<i class="far fa-question-circle show-pointer" onclick="displayBudget2V1Popup();"></i>
										</li>
									</ul>
								</li>
								<li>
									<i class="far fa-times-circle show-pointer" id="budget1Selector"></i>
									<input type="radio" name="budgetSelector" id="allCommodityradio">
									<span id="allCommodityspan">Budget 1</span>
									<i class="far fa-question-circle show-pointer" onclick="displayBudget1Popup();"></i>
								</li>
							</ul>
							<br>
							<button style="width: 100%;">Review Selected Budget</button>
						</div>
					</div>
					<br>
					<div id="siteTree" class="sidebar-tree-div roundborder">
					</div>
					<br>
					<div id="commodityList" class="sidebar-tree-div roundborder dashboard">
						<div class="expander-header">
							<span id="commoditySelectorSpan">Commodity</span>
							<div id="commoditySelector" class="far fa-plus-square expander show-pointer openExpander"></div>
						</div>
						<div id="commoditySelectorList" class="expander-container">
							<ul class="format-listitem toplistitem">
								<li>
									<input type="radio" name="commoditySelector" id="allCommodityradio" checked><span id="allCommodityspan">All</span>
								</li>
								<li>
									<input type="radio" name="commoditySelector" id="electricityCommodityradio"><span id="electricityCommodityspan">Electricity</span>
								</li>
								<li>
									<input type="radio" name="commoditySelector" id="gasCommodityradio"><span id="gasCommodityspan">Gas</span>
								</li>
							</ul>
						</div>
					</div>
					<br>
					<div id="timePeriodList" class="sidebar-tree-div roundborder dashboard">
						<div class="expander-header">
							<span id="timePeriodSelectorSpan">Time Period</span>
							<div id="timePeriodSelector" class="far fa-plus-square expander show-pointer openExpander"></div>
						</div>
						<div id="timePeriodSelectorList" class="expander-container">
							<ul class="format-listitem slider-list toplistitem">
								<li>
									<span>Date Range</span>
									<rzslider id="timePeriodDateRange"
										rz-slider-model="timePeriodDateRange.minValue" 
										rz-slider-high="timePeriodDateRange.maxValue" 
										rz-slider-options="timePeriodDateRange.options">
									</rzslider>
								</li>
								<li>
								<input type="radio" name="timePeriodSelector" checked><span style="padding-left: 1px; margin-right: 10px;">Budget Period</span><input type="radio" name="timePeriodSelector"><span id="allCommodityspan">Creation Date</span>
								</li>
							</ul>
						</div>
					</div>
				</div>
			</div>
			<br>
			<button style="width: 100%" onclick="deleteBudgets();">Delete Selected Budgets</button>
			<br><br>
			<button style="width: 100%" onclick="reinstateBudgets();">Reinstate Deleted Budgets</button>
			<br>
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
			</div>
			<div class="final-column">
				<br>
				<div class="dashboard roundborder outer-container">
					<div class="expander-header">
						<span>Create\Review Budget</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="createReviewBudget"></i>
						<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Budget To Download Basket"></div>
						<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Budget"></div>
					</div>
					<div id="createReviewBudgetList" class="expander-container">
						<div id="createReviewBudgetDetailDiv" class="tree-div roundborder" style="float: left; margin-left: 45px; margin-right: 15px;">
							<label for="createReviewBudgetName" style="width: 75px; text-align: right;">Name: </label>
							<input id="createReviewBudgetName" style="width: calc(100% - 80px);"></input><br>
							<label for="createReviewBudgetBaseType" style="width: 75px; text-align: right;">Base Type: </label>
							<select id="createReviewBudgetBaseType" style="width: calc(100% - 80px);">
								<option value="">Select Option</option>
								<option value="LatestBWSForecast">Latest BWS Forecast</option>
								<option value="LatestActuals">Latest Actuals</option>
								<option value="UploadedFile">Uploaded File</option>
							</select><br>
							<label for="createReviewBudgetDateFrom" style="width: 75px; text-align: right;">Period: </label>
							<input type="date" id="createReviewBudgetDateFrom" style="width: calc(50% - 52px);"></input>
							<label for="createReviewBudgetDateTo">To</label>
							<input type="date" id="createReviewBudgetDateTo" style="width: calc(50% - 52px);"></input><br>
							<label for="createReviewBudgetBaseType" style="width: 75px; text-align: right;">Status: </label>
							<select id="createReviewBudgetBaseType" style="width: calc(100% - 80px);">
								<option value="">Select Option</option>
								<option value="Approved">Approved</option>
								<option value="Rejected">Rejected</option>
								<option value="Pending">Pending</option>
								<option value="Overriden">Overriden</option>
							</select><br><br>
							<button class="reject show-pointer" style="width: calc(45%); margin-right: calc(10%);">Reset Budget</button><button class="approve show-pointer" style="width: calc(45%);">Save Budget</button>
						</div>
						<div id="createReviewBudgetAdjustAreaDiv" class="tree-div roundborder scrolling-wrapper" style="width: 950px; float: left; margin-right: 15px;">
							<label for="createReviewBudgetAdjustArea" style="margin-left: 50px;">Adjust </label>
							<select id="createReviewBudgetAdjustArea">
								<option value="">Select Option</option>
								<option value="Cost">Cost</option>
								<option value="Usage">Usage</option>
							</select>
							<label for="createReviewBudgetAdjustType">by</label>
							<select id="createReviewBudgetAdjustType">
								<option value="Uplift">Uplifting</option>
								<option value="Set">Setting</option>
							</select>
							<label for="createReviewBudgetAdjustAmount">by</label>
							<input type="number" id="createReviewBudgetAdjustAmount" style="width: 7.5%;"></input>
							<select id="createReviewBudgetAdjustAmountType">
								<option value="Percent">%</option>
								<option value="Usage">kWh</option>
								<option value="Cost">£</option>
							</select>
							<label for="createReviewBudgetAdjustDateFrom">between</label>
							<input type="date" id="createReviewBudgetAdjustDateFrom"></input>
							<label for="createReviewBudgetAdjustDateTo">and</label>
							<input type="date" id="createReviewBudgetAdjustDateTo"></input>
							<button class="approve show-pointer roundborder" title="Add Adjustment">+</button>
							<br>
							<div class="expander-header">
								<span>Budget Adjustments</span>
								<div id="createReviewBudgetAdjustments" class="far fa-plus-square show-pointer expander"></div>
							</div>
							<div id="createReviewBudgetAdjustmentsList" class="roundborder datagrid listitem-hidden expander-container" style="padding: 5px;">
								<div id="adjustmentsSpreadsheet"></div>
							</div>				
						</div>
						<div id="createReviewBudgetTreeDiv" class="tree-div roundborder scrolling-wrapper" style="float: left; width: 385px;">
						</div>
						<div style="clear: both;"></div>
						<div class="dashboard roundborder expander-container" style="padding: 10px;">
							<div class="expander-header">
								<span>Charts</span>
								<i class="far fa-plus-square show-pointer expander openExpander" id="charts"></i>
							</div>
							<div id="chartsList" class="expander-container">
								<div id="leftHandChartDiv" class="roundborder chart" style="margin-right: 5px;">
									<div id="leftHandChart">
									</div>
								</div>
								<div id="rightHandChartDiv" class="roundborder chart">
									<div id="rightHandChart">
									</div>
								</div>
							</div>			
							<div style="clear: left;"></div>
						</div>
					</div>
				</div>
			</div>
			<br>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<link rel="stylesheet" href="rzslider.css" />
<link data-require="bootstrap@3.3.7" data-semver="3.3.7" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<script data-require="angular.js@1.6.0" data-semver="1.6.0" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.js"></script>
<script data-require="ui-bootstrap@*" data-semver="2.2.0" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-2.2.0.js"></script>
<script src="rzslider.js"></script>
<script src="script.js"></script>
<script src="budgetmanagement.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="budgetmanagement.json"></script>
<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>