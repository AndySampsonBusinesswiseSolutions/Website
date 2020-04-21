<?php 
	$PAGE_TITLE = "Eagle Eye";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html ng-app="dateRangeDemo" ng-controller="dateRangeCtrl">
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="dataanalysis.css">
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
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav(mySidenav)"></i>
		<div class="tree-column">
			<div id="displayTreeList" class="sidebar-tree-div roundborder scrolling-wrapper">
				<div id="displayTreeDiv" class="expander-header">
					<span>Select Display</span>
					<i class="far fa-plus-square show-pointer" id="displaySelector"></i>
				</div>
				<div id="displaySelectorList" style="margin-top: 5px;">
					<ul class="format-listitem">
						<li>
							<i class="far fa-plus-square expander show-pointer" id="costDisplaySelector"></i>
							<input type="radio" name="displayGroup" id="costCostElement0radio" checked><span id="costCostElement0span" style="padding-left: 1px;">Cost</span>
							<ul class="format-listitem listitem-hidden" id="costDisplaySelectorList">
								<li>
									<input type="radio" name="costDisplayGroup" id="variance0radio" checked><span id="variance0span" style="padding-left: 1px;">All Cost Items</span>
								</li>
								<li>
									<div id="networkCostElement0" style="padding-right: 4px;" class="far fa-plus-square show-pointer"></div>
									<span id="networkCostElement0span" style="padding-left: 1px;">Network</span>
									<div id="networkCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="wholesaleCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="wholesaleCostElement0radio"><span id="wholesaleCostElement0span" style="padding-left: 1px;">Wholesale</span>
											</li>
											<li>
												<div id="distributionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="distributionCostElement0radio"><span id="distributionCostElement0span" style="padding-left: 1px;">Distribution Use of Systems</span>
											</li>
											<li>
												<div id="transmissionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="transmissionCostElement0radio"><span id="transmissionCostElement0span" style="padding-left: 1px;">Transmission Use of Systems</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="renewablesCostElement0" style="padding-right: 4px;" class="far fa-plus-square show-pointer"></div>
									<span id="renewablesCostElement0span" style="padding-left: 1px;">Renewables</span>
									<div id="renewablesCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="renewablesobligationCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="renewablesobligationCostElement0radio"><span id="renewablesobligationCostElement0span" style="padding-left: 1px;">Renewables Obligation</span>
											</li>
											<li>
												<div id="feedintariffCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="feedintariffCostElement0radio"><span id="feedintariffCostElement0span" style="padding-left: 1px;">Feed In Tariff</span>
											</li>
											<li>
												<div id="contractsfordifferenceCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="contractsfordifferenceCostElement0radio"><span id="contractsfordifferenceCostElement0span" style="padding-left: 1px;">Contracts For Difference</span>
											</li>
											<li>
												<div id="energyintensiveindustriesCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="energyintensiveindustriesCostElement0radio"><span id="energyintensiveindustriesCostElement0span" style="padding-left: 1px;">Energy Intensive Industries</span>
											</li>
											<li>
												<div id="capacitymarketsCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="capacitymarketsCostElement0radio"><span id="capacitymarketsCostElement0span" style="padding-left: 1px;">Capacity Markets</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="balancingCostElement0" style="padding-right: 4px;" class="far fa-plus-square show-pointer"></div>
									<span id="balancingCostElement0span" style="padding-left: 1px;">Balancing</span>
									<div id="balancingCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="bsuosCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="bsuosCostElement0radio"><span id="bsuosCostElement0span" style="padding-left: 1px;">Balancing System Use of Systems</span>
											</li>
											<li>
												<div id="rcrcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="rcrcCostElement0radio"><span id="rcrcCostElement0span" style="padding-left: 1px;">Reallocation Cashflow Residual Cashflow</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="otherCostElement0" style="padding-right: 4px;" class="far fa-plus-square show-pointer"></div>
									<span id="otherCostElement0span" style="padding-left: 1px;">Other</span>
									<div id="otherCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="managementfeeCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="managementfeeCostElement0radio"><span id="managementfeeCostElement0span" style="padding-left: 1px;">Management Fee</span>
											</li>
											<li>
												<div id="aahedcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="costDisplayGroup" id="aahedcCostElement0radio"><span id="aahedcCostElement0span" style="padding-left: 1px;">Hydro Charge</span>
											</li>
										</ul>
									</div>
								</li>
							</ul>
						</li>
						<li>
							<i class="far fa-plus-square expander show-pointer" id="usageDisplaySelector"></i>
							<input type="radio" name="displayGroup" id="usageCostElement0radio"><span id="usageCostElement0span" style="padding-left: 1px;">Usage</span>
							<ul class="format-listitem listitem-hidden" id="usageDisplaySelectorList">
								<li>
									<input type="radio" name="usageDisplayGroup" id="variance0radio" checked><span id="variance0span" style="padding-left: 1px;">Consumption</span>
								</li>
								<li>
									<input type="radio" name="usageDisplayGroup" id="variance0radio"><span id="variance0span" style="padding-left: 1px;">Capacity</span>
								</li>
							</ul>
						</li>
						<li>
							<i class="far fa-times-circle expander"></i>
							<input type="radio" name="displayGroup" id="rateCostElement0radio"><span id="rateCostElement0span" style="padding-left: 1px;">Rate</span>
						</li>
						<li><br></li>
						<li>
							<span>Granularity</span><br>
							<rzslider id="timePeriodOptionsTimeSpan"
								rz-slider-model="timePeriodOptionsTimeSpan.value" 
								rz-slider-options="timePeriodOptionsTimeSpan.options">
							</rzslider>
						</li>
					</ul>
				</div>
			</div>
			<br>
			<div id="budgetList" class="sidebar-tree-div roundborder">
				<div id="budgetHeader" class="expander-header">
					<span>Select Budget</span>
					<i class="far fa-plus-square expander show-pointer" id="budgetSelector"></i>
				</div>
				<div id="budgetSelectorList" style="margin-top: 5px;">
					<ul class="format-listitem">
						<li>
							<i class="far fa-plus-square expander show-pointer" id="budget2Selector"></i>
							<span id="allCommodityspan">Budget 2</span>
							<ul class="format-listitem listitem-hidden" id="budget2SelectorList">
								<li>
									<i class="far fa-times-circle expander" id="budget2Selector"></i>
									<input type="checkbox" name="budgetSelector" id="allCommodityradio">
									<span id="allCommodityspan">Version 2</span>
								</li>
								<li>
									<i class="far fa-times-circle expander" id="budget2Selector"></i>
									<input type="checkbox" name="budgetSelector" id="allCommodityradio">
									<span id="allCommodityspan">Version 1</span>
								</li>
							</ul>
						</li>
						<li>
							<i class="far fa-times-circle expander" id="budget1Selector"></i>
							<input type="checkbox" name="budgetSelector" id="allCommodityradio">
							<span id="allCommodityspan">Budget 1</span>
						</li>
					</ul>
				</div>
			</div>
			<br>
			<div id="siteDiv" class="sidebar-tree-div roundborder scrolling-wrapper">
			</div>
			<br>
			<div id="invoiceList" class="sidebar-tree-div roundborder">
				<div id="invoiceHeader" class="expander-header">
					<span>Select Invoice</span>
					<i class="far fa-plus-square expander show-pointer" id="invoiceSelector"></i>
				</div>
				<div id="invoiceSelectorList" style="margin-top: 5px;" class="listitem-hidden">
					<ul class="format-listitem">
						<li>
							<input type="checkbox" name="invoiceSelector"><span style="padding-left: 1px;">Bill 0004</span>
						</li>
						<li>
							<input type="checkbox" name="invoiceSelector"><span style="padding-left: 1px;">Bill 0003</span>
						</li>
						<li>
							<input type="checkbox" name="invoiceSelector"><span style="padding-left: 1px;">Bill 0002</span>
						</li>
						<li>
							<input type="checkbox" name="invoiceSelector"><span style="padding-left: 1px;">Bill 0001</span>
						</li>
					</ul>
				</div>
			</div>
			<br>
			<div id="groupingList" class="sidebar-tree-div roundborder">
				<div id="groupingHeader" class="expander-header">
					<span>Select Grouping Option</span>
					<i class="far fa-plus-square expander show-pointer" id="groupingSelector"></i>
				</div>
				<div id="groupingSelectorList" style="margin-top: 5px;" class="listitem-hidden">
					<ul class="format-listitem">
						<li>
							<input type="radio" name="groupingSelector" id="noGroupradio" checked><span style="padding-left: 1px;">No Grouping</span>
						</li>
						<li>
							<i class="far fa-plus-square expander show-pointer" id="mathematicalFunctionSelector"></i>
							<input type="radio" name="groupingSelector" id="groupradio"><span id="groupspan" style="padding-left: 1px;">Group</span>
							<ul class="format-listitem listitem-hidden" id="mathematicalFunctionSelectorList">
								<li>
									<input type="checkbox" id="sumcheckbox" checked><span id="sumspan" style="padding-left: 1px;">Sum</span>
								</li>
								<li>
									<input type="checkbox" id="averagecheckbox"><span id="averagespan" style="padding-left: 1px;">Average</span>
								</li>
							</ul>
						</li>
					</ul>
				</div>
			</div>
			<br>
			<div id="commodityList" class="tree-div roundborder">
				<div id="commodityHeader" class="expander-header">
					<span>Filter By Commodity</span>
					<i class="far fa-plus-square expander show-pointer" id="commoditySelector"></i>
				</div>
				<div id="commoditySelectorList" style="margin-top: 5px;" class="listitem-hidden">
					<ul class="format-listitem">
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
			<div id="timePeriodList" class="tree-div roundborder">
				<div id="timePeriodHeader" class="expander-header">
					<span>Filter By Time Period</span>
					<i class="far fa-plus-square expander show-pointer" id="timePeriodOptionsSelector"></i>
				</div>
				<div id="timePeriodOptionsSelectorList" style="margin-top: 5px;" class="slider-list">
					<ul class="format-listitem">
						<li>
							<span>Date Range</span>
							<rzslider id="timePeriodOptionsDateRange"
								rz-slider-model="timePeriodOptionsDateRange.minValue" 
								rz-slider-high="timePeriodOptionsDateRange.maxValue" 
								rz-slider-options="timePeriodOptionsDateRange.options">
							</rzslider>
						</li>
						
						<li>
							<span>Created</span>
							<rzslider id="timePeriodOptionsCreated"
								rz-slider-model="timePeriodOptionsCreated.value" 
								rz-slider-options="timePeriodOptionsCreated.options">
							</rzslider>
						</li>
					</ul>
				</div>
			</div>
			<br>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?> <i class="far fa-copyright"></i></div>
	</div>
	<br>
	<div class="final-column">
		<div class="dashboard roundborder" style="padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Chart</span>
				<div id="chartHeader" class="far fa-plus-square expander show-pointer"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Chart"></div>
			</div>
			<div id="chartHeaderList" class="roundborder chart" style="margin-top: 5px;">
				<div id="chart"></div>
			</div>
		</div>
		<div class="dashboard roundborder" style="padding: 10px; margin-top: 5px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Data</span>
				<div id="dataHeader" class="far fa-plus-square expander show-pointer"></div>
			</div>
			<div id="dataHeaderList" class="roundborder chart listitem-hidden" style="margin-top: 5px;">
				<div id="datagrid" style="margin: 5px;"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<link rel="stylesheet" href="rzslider.css" />
<link data-require="bootstrap@3.3.7" data-semver="3.3.7" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<script data-require="angular.js@1.6.0" data-semver="1.6.0" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.js"></script>
<script data-require="ui-bootstrap@*" data-semver="2.2.0" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-2.2.0.js"></script>
<script src="rzslider.js"></script>
<script src="script.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="dataanalysis.json"></script>
<script type="text/javascript" src="dataanalysis.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>