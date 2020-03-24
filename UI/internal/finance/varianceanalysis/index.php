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
				<span style="padding-left: 5px;">Select Commodity <i class="far fa-plus-square" id="commoditySelector"></i></span>
				<ul class="format-listitem" id="commoditySelectorList">
					<li>
						<input type="radio" name="group2" id="allCommodityradio" checked guid="0" onclick='createTree(data, "treeDiv", "", "updateChart(chart)", true);'><span id="allCommodityspan" style="padding-left: 1px;">All</span>
					</li>
					<li>
						<input type="radio" name="group2" id="electricityCommodityradio" guid="0" onclick='createTree(data, "treeDiv", "Electricity", "updateChart(chart)", true);'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity</span>
					</li>
					<li>
						<input type="radio" name="group2" id="gasCommodityradio" guid="0" onclick='createTree(data, "treeDiv", "Gas", "updateChart(chart)", true);'><span id="gasCommodityspan" style="padding-left: 1px;">Gas</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="displayTreeDiv" class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Display <i class="far fa-plus-square" id="displaySelector"></i></span>
				<ul class="format-listitem" id="displaySelectorList">
					<li>
						<input type="radio" name="group1" id="costCostElement0radio" checked guid="0" onclick='updateChart(this, chart);'><span id="costCostElement0span" style="padding-left: 1px;">Cost</span>
					</li>
					<li>
						<input type="radio" name="group1" id="usageCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="usageCostElement0span" style="padding-left: 1px;">Usage</span>
					</li>
					<li>
						<input type="radio" name="group1" id="rateCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="rateCostElement0span" style="padding-left: 1px;">Rate</span>
					</li>
				</ul>
			</div>
			<br>
			<div id="costTreeDiv" class="tree-div roundborder scrolling-wrapper">
				<span style="padding-left: 5px;">Select Type <i class="far fa-plus-square" id="typeSelector"></i></span>
				<ul class="format-listitem" id="typeSelectorList">
					<li>
						<input type="radio" name="group0" id="variance0radio" guid="0" checked onclick='updateChart(this, chart);'><span id="variance0span" style="padding-left: 1px;">Forecast v Invoice Summary</span>
					</li>
					<li>
						<div id="variance1" class="far fa-plus-square" style="padding-right: 4px;"></div>
						<span id="variance1span" style="padding-left: 1px;">Cost Elements</span>
						<div id="variance1List" class="listitem-hidden">
							<ul class="format-listitem">
								<li>
									<div id="networkCostElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
									<span id="networkCostElement0span" style="padding-left: 1px;">Network</span>
									<div id="networkCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="wholesaleCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="wholesaleCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="wholesaleCostElement0span" style="padding-left: 1px;">Wholesale</span>
											</li>
											<li>
												<div id="distributionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="distributionCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="distributionCostElement0span" style="padding-left: 1px;">Distribution Use of Systems</span>
											</li>
											<li>
												<div id="transmissionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="transmissionCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="transmissionCostElement0span" style="padding-left: 1px;">Transmission Use of Systems</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="renewablesCostElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
									<span id="renewablesCostElement0span" style="padding-left: 1px;">Renewables</span>
									<div id="renewablesCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="renewablesobligationCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="renewablesobligationCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="renewablesobligationCostElement0span" style="padding-left: 1px;">Renewables Obligation</span>
											</li>
											<li>
												<div id="feedintariffCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="feedintariffCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="feedintariffCostElement0span" style="padding-left: 1px;">Feed In Tariff</span>
											</li>
											<li>
												<div id="contractsfordifferenceCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="contractsfordifferenceCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="contractsfordifferenceCostElement0span" style="padding-left: 1px;">Contracts For Difference</span>
											</li>
											<li>
												<div id="energyintensiveindustriesCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="energyintensiveindustriesCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="energyintensiveindustriesCostElement0span" style="padding-left: 1px;">Energy Intensive Industries</span>
											</li>
											<li>
												<div id="capacitymarketsCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="capacitymarketsCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="capacitymarketsCostElement0span" style="padding-left: 1px;">Capacity Markets</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="balancingCostElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
									<span id="balancingCostElement0span" style="padding-left: 1px;">Balancing</span>
									<div id="balancingCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="bsuosCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="bsuosCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="bsuosCostElement0span" style="padding-left: 1px;">Balancing System Use of Systems</span>
											</li>
											<li>
												<div id="rcrcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="rcrcCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="rcrcCostElement0span" style="padding-left: 1px;">Reallocation Cashflow Residual Cashflow</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div id="otherCostElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
									<span id="otherCostElement0span" style="padding-left: 1px;">Other</span>
									<div id="otherCostElement0List" class="listitem-hidden">
										<ul class="format-listitem">
											<li>
												<div id="managementfeeCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="managementfeeCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="managementfeeCostElement0span" style="padding-left: 1px;">Management Fee</span>
											</li>
											<li>
												<div id="aahedcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
												<input type="radio" name="group0" id="aahedcCostElement0radio" guid="0" onclick='updateChart(this, chart);'><span id="aahedcCostElement0span" style="padding-left: 1px;">Hydro Charge</span>
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
		<i class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<div class="final-column">
		<div>
			<br>
			<div class="roundborder chart">
				<div id="chart">
				</div>
			</div>
			<br>
			<div id="datagridDiv" class="roundborder scrolling-wrapper">
				<div id="datagrid">
				</div>
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