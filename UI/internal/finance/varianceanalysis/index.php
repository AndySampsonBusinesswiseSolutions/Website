<?php 
	$PAGE_TITLE = "Variance Analysis";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="varianceanalysis.css">
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<br>
			<div class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Commodity <i class="far fa-plus-square show-pointer" id="commoditySelector"></i></span>
				<ul class="format-listitem" id="commoditySelectorList">
					<li>
						<input type="radio" name="group2" id="allCommodityradio" checked guid="0" onclick='createTree(data, "treeDiv", "", "updateChart()", true);'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
					</li>
					<li>
						<input type="radio" name="group2" id="electricityCommodityradio" guid="0" onclick='createTree(data, "treeDiv", "Electricity", "updateChart()", true);'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
					</li>
					<li>
						<input type="radio" name="group2" id="gasCommodityradio" guid="0" onclick='createTree(data, "treeDiv", "Gas", "updateChart()", true);'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="displayTreeDiv" class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Display <i class="far fa-plus-square show-pointer" id="displaySelector"></i></span>
				<ul class="format-listitem" id="displaySelectorList">
					<li>
						<input type="radio" name="group1" id="costCostElement0radio" checked guid="0" onclick='updateChart(this);'><span id="costCostElement0span" style="padding-left: 1px;">Cost</span>
					</li>
					<li>
						<input type="radio" name="group1" id="usageCostElement0radio" guid="0" onclick='updateChart(this);'><span id="usageCostElement0span" style="padding-left: 1px;">Usage</span>
					</li>
					<li>
						<input type="radio" name="group1" id="rateCostElement0radio" guid="0" onclick='updateChart(this);'><span id="rateCostElement0span" style="padding-left: 1px;">Rate</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="costTreeDiv" class="tree-div roundborder scrolling-wrapper">
				<span style="padding-left: 5px;">Select Type <i class="far fa-plus-square show-pointer" id="typeSelector"></i></span>
				<ul class="format-listitem" id="typeSelectorList">
					<li>
						<input type="radio" name="group0" id="variance0radio" guid="0" checked onclick='updateChart(this);'><span id="variance0span" style="padding-left: 1px;">Forecast v Invoice Summary</span>
					</li>
					<li>
						<div id="variance1" class="far fa-plus-square show-pointer" style="padding-right: 4px;"></div>
						<span id="variance1span" style="padding-left: 1px;">Cost Elements</span>
						<div id="variance1List" class="listitem-hidden">
							<ul class="format-listitem">
								<li>
									<div id="networkCostElement0" style="padding-right: 4px;" class="far fa-plus-square show-pointer"></div>
									<span id="networkCostElement0span" style="padding-left: 1px;">Network</span>
									<div id="networkCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="wholesaleCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="wholesaleCostElement0radio" guid="0" onclick='updateChart(this);'><span id="wholesaleCostElement0span" style="padding-left: 1px;">Wholesale</span>
											</li>
											<li>
												<div id="distributionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="distributionCostElement0radio" guid="0" onclick='updateChart(this);'><span id="distributionCostElement0span" style="padding-left: 1px;">Distribution Use of Systems</span>
											</li>
											<li>
												<div id="transmissionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="transmissionCostElement0radio" guid="0" onclick='updateChart(this);'><span id="transmissionCostElement0span" style="padding-left: 1px;">Transmission Use of Systems</span>
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
												<input type="radio" name="group0" id="renewablesobligationCostElement0radio" guid="0" onclick='updateChart(this);'><span id="renewablesobligationCostElement0span" style="padding-left: 1px;">Renewables Obligation</span>
											</li>
											<li>
												<div id="feedintariffCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="feedintariffCostElement0radio" guid="0" onclick='updateChart(this);'><span id="feedintariffCostElement0span" style="padding-left: 1px;">Feed In Tariff</span>
											</li>
											<li>
												<div id="contractsfordifferenceCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="contractsfordifferenceCostElement0radio" guid="0" onclick='updateChart(this);'><span id="contractsfordifferenceCostElement0span" style="padding-left: 1px;">Contracts For Difference</span>
											</li>
											<li>
												<div id="energyintensiveindustriesCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="energyintensiveindustriesCostElement0radio" guid="0" onclick='updateChart(this);'><span id="energyintensiveindustriesCostElement0span" style="padding-left: 1px;">Energy Intensive Industries</span>
											</li>
											<li>
												<div id="capacitymarketsCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="capacitymarketsCostElement0radio" guid="0" onclick='updateChart(this);'><span id="capacitymarketsCostElement0span" style="padding-left: 1px;">Capacity Markets</span>
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
												<input type="radio" name="group0" id="bsuosCostElement0radio" guid="0" onclick='updateChart(this);'><span id="bsuosCostElement0span" style="padding-left: 1px;">Balancing System Use of Systems</span>
											</li>
											<li>
												<div id="rcrcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="rcrcCostElement0radio" guid="0" onclick='updateChart(this);'><span id="rcrcCostElement0span" style="padding-left: 1px;">Reallocation Cashflow Residual Cashflow</span>
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
												<input type="radio" name="group0" id="managementfeeCostElement0radio" guid="0" onclick='updateChart(this);'><span id="managementfeeCostElement0span" style="padding-left: 1px;">Management Fee</span>
											</li>
											<li>
												<div id="aahedcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="aahedcCostElement0radio" guid="0" onclick='updateChart(this);'><span id="aahedcCostElement0span" style="padding-left: 1px;">Hydro Charge</span>
											</li>
										</ul>
									</div>
								</li>
							</ul>
						</div>
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
		<div class="dashboard roundborder" style="padding: 10px;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Variance Analysis Charts</span>
				<i class="far fa-plus-square show-pointer" id="varianceAnalysisChart"></i>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Variance Analysis Chart To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Variance Analysis Chart"></div>
			</div>
			<div id="varianceAnalysisChartList" style="margin-top: 5px;">
				<div id="rightHandChartDiv" class="roundborder half-chart" style="margin-right: 5px;">
					<div id="rightHandChart">
					</div>
				</div>
				<div id="leftHandChartDiv" class="roundborder half-chart">
					<div id="leftHandChart">
					</div>
				</div>
				<div id="chartDiv" class="roundborder chart">
					<div id="chart">
					</div>
				</div>
			</div>			
			<div style="clear: left;"></div>
		</div>
		<div id="datagridDiv" class="roundborder tree-div scrolling-wrapper" style="margin-top: 5px;">
			<div id="datagrid" style="margin: 5px;">
			</div>
		</div>
	</div>
	<br>
</body>

<script src="varianceanalysis.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="varianceanalysis.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>