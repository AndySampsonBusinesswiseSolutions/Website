<?php 
	$PAGE_TITLE = "View Meter Consumption";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="viewmeterconsumption.css">
</head>

<body ng-app="dateRangeDemo" ng-controller="dateRangeCtrl">
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="siteDiv" class="tree-div">
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span style="padding-left: 5px;">Select Commodity</span>
				<ul class="format-listitem">
					<li>
						<input type="radio" name="group1" id="allCommodityradio" checked guid="0" onclick='createTree(data, "DeviceType", "siteDiv", "", "updateCharts()", true); addExpanderOnClickEvents(siteDiv); updateCharts();'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
					</li>
					<li>
						<input type="radio" name="group1" id="electricityCommodityradio" guid="0" onclick='createTree(data, "DeviceType", "siteDiv", "Electricity", "updateCharts()", true); addExpanderOnClickEvents(siteDiv); updateCharts();'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
					</li>
					<li>
						<input type="radio" name="group1" id="gasCommodityradio" guid="0" onclick='createTree(data, "DeviceType", "siteDiv", "Gas", "updateCharts()", true); addExpanderOnClickEvents(siteDiv); updateCharts();'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
					</li>
				</ul>
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span id="usageChartOptionsspan" style="padding-left: 5px;">Usage Chart Options</span>
				<div id="usageChartOptions" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<ul id="usageChartOptionsList" class="format-listitem slider-list">
					<li>
						<span>Date Range</span>
						<rzslider id="usageChartOptionsDateRange"
							rz-slider-model="usageChartOptionsDateRange.minValue" 
							rz-slider-high="usageChartOptionsDateRange.maxValue" 
							rz-slider-options="usageChartOptionsDateRange.options">
						</rzslider>
					</li>
					<li>
						<span>Time Span</span>
						<rzslider id="usageChartOptionsTimeSpan"
							rz-slider-model="usageChartOptionsTimeSpan.value" 
							rz-slider-options="usageChartOptionsTimeSpan.options">
						</rzslider>
					</li>
				</ul>
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span id="totalCostChartOptionsspan" style="padding-left: 5px;">Total Cost Chart Options</span>
				<div id="totalCostChartOptions" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<ul id="totalCostChartOptionsList" class="format-listitem slider-list">
					<li>
						<span>Date Range</span>
						<rzslider id="totalCostChartOptionsDateRange"
							rz-slider-model="totalCostChartOptionsDateRange.minValue" 
							rz-slider-high="totalCostChartOptionsDateRange.maxValue" 
							rz-slider-options="totalCostChartOptionsDateRange.options">
						</rzslider>
					</li>
					<li>
						<span>Time Span</span>
						<rzslider id="totalCostChartOptionsTimeSpan"
							rz-slider-model="totalCostChartOptionsTimeSpan.value" 
							rz-slider-options="totalCostChartOptionsTimeSpan.options">
						</rzslider>
					</li>
				</ul>
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span id="costBreakdownChartOptionsspan" style="padding-left: 5px;">Cost Breakdown Chart Options</span>
				<div id="costBreakdownChartOptions" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<ul id="costBreakdownChartOptionsList" class="format-listitem slider-list">
					<li>
						<span>Date Range</span>
						<rzslider id="costBreakdownChartOptionsDateRange"
							rz-slider-model="costBreakdownChartOptionsDateRange.minValue" 
							rz-slider-high="costBreakdownChartOptionsDateRange.maxValue" 
							rz-slider-options="costBreakdownChartOptionsDateRange.options">
						</rzslider>
					</li>
					<li>
						<span>Time Span</span>
						<rzslider id="costBreakdownChartOptionsTimeSpan"
							rz-slider-model="costBreakdownChartOptionsTimeSpan.value" 
							rz-slider-options="costBreakdownChartOptionsTimeSpan.options">
						</rzslider>
					</li>
					<li>
						<span id="costBreakdownChartElementOptionsspan" style="padding-left: 5px;">Cost Elements</span>
						<div id="costBreakdownChartElementOptions" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
						<ul id="costBreakdownChartElementOptionsList" class="format-listitem listitem-hidden">
							<li>
								<input type="checkbox" id="costBreakdownChartElementAllOptionscheckbox" checked onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementAllOptionsspan" style="padding-left: 5px;">All Cost Elements</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementCCLOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementCCLOptionsspan" style="padding-left: 5px;">Climate Change Levy</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementCMOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementCMOptionsspan" style="padding-left: 5px;">Capacity Market</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementBSUoSOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementBSUoSOptionsspan" style="padding-left: 5px;">BSUoS</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementCfDOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementCfDOptionsspan" style="padding-left: 5px;">Contract For Difference</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementFiTOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementFiTOptionsspan" style="padding-left: 5px;">Feed In Tariff</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementROOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementROOptionsspan" style="padding-left: 5px;">Renewable Obligation</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementDLossOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementDLossOptionsspan" style="padding-left: 5px;">Distribution Loss</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementDUoSCapOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementDUoSCapOptionsspan" style="padding-left: 5px;">DUoS Capacity</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementDUoSCapFixOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementDUoSCapFixOptionsspan" style="padding-left: 5px;">DUoS Capacity Fixed</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementDUoSSCOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementDUoSSCOptionsspan" style="padding-left: 5px;">DUoS Standing Charge</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementDUoSOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementDUoSOptionsspan" style="padding-left: 5px;">DUoS</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementTLossOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementTLossOptionsspan" style="padding-left: 5px;">Transmission Loss</span>
							</li>
							<li>
								<input type="checkbox" id="costBreakdownChartElementWholesaleOptionscheckbox" onclick="updateCostBreakdownChart();"></input>
								<span id="costBreakdownChartElementWholesaleOptionsspan" style="padding-left: 5px;">Wholesale</span>
							</li>
						</ul>
					</li>
				</ul>
			</div>
			<br>
			<div class="roundborder" style="background-color: #e9eaee;">
				<span id="capacityChartOptionsspan" style="padding-left: 5px;">Capacity Chart Options</span>
				<div id="capacityChartOptions" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<ul id="capacityChartOptionsList" class="format-listitem slider-list">
					<li>
						<span>Date Range</span>
						<rzslider id="capacityChartOptionsDateRange"
							rz-slider-model="capacityChartOptionsDateRange.minValue" 
							rz-slider-high="capacityChartOptionsDateRange.maxValue" 
							rz-slider-options="capacityChartOptionsDateRange.options">
						</rzslider>
					</li>
					<li>
						<span>Time Span</span>
						<rzslider id="capacityChartOptionsTimeSpan"
							rz-slider-model="capacityChartOptionsTimeSpan.value" 
							rz-slider-options="capacityChartOptionsTimeSpan.options">
						</rzslider>
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
		<div class="roundborder">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="usagespan" style="padding-left: 5px;">Usage Chart</span>
				<div id="usage" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
			</div>
			<div id="usageList" class="chart" style="margin: 5px;">
				<div id="usageChart"></div>
			</div>
		</div>
		<div class="roundborder" style="margin-top: 5px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="totalCostspan" style="padding-left: 5px;">Total Cost Chart</span>
				<div id="totalCost" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
			</div>
			<div id="totalCostList"  class="listitem-hidden chart" style="margin: 5px;">
				<div id="totalCostChart"></div>
			</div>
		</div>
		<div class="roundborder" style="margin-top: 5px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="costBreakdownspan" style="padding-left: 5px;">Cost Breakdown Chart</span>
				<div id="costBreakdown" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
			</div>
			<div id="costBreakdownList"  class="listitem-hidden chart" style="margin: 5px;">
				<div id="costBreakdownChart"></div>
			</div>
		</div>
		<div class="roundborder" style="margin-top: 5px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span id="capacityspan" style="padding-left: 5px;">Capacity Chart</span>
				<div id="capacity" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
			</div>
			<div id="capacityList"  class="listitem-hidden chart" style="margin: 5px;">
				<div id="capacityChart"></div>
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
<script type="text/javascript" src="viewmeterconsumption.json"></script>
<script type="text/javascript" src="viewmeterconsumption.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>