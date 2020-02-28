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
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<br>
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<div>
					<div>
						<div class="tree-column" style="padding-top: 5px;">
							<div id="electricityTreeDiv" class="tree-div">
							</div>
							<br>
							<div id="electricityDisplayTreeDiv" class="datagrid">
								<ul class="format-listitem">
									<li>
										<div id="displayCostElement0"></div>
										<span id="displayCostElement0span">Display</span>
										<div id="displayCostElement0List">
											<ul class="format-listitem">
												<li>
													<div id="usageCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="group1" id="usageCostElement0radio" checked guid="0" onclick='openTab("Usage", "cardDiv");  updateChart(this, electricityChart);'><span id="usageCostElement0span" style="padding-left: 1px;">Usage</span>
												</li>
												<li>
													<div id="costCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="group1" id="costCostElement0radio" guid="0" onclick='openTab("Cost", "cardDiv");  updateChart(this, electricityChart);'><span id="costCostElement0span" style="padding-left: 1px;">Cost</span>
												</li>
												<li>
													<div id="rateCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="group1" id="rateCostElement0radio" guid="0" onclick='openTab("Rate", "cardDiv");  updateChart(this, electricityChart);'><span id="rateCostElement0span" style="padding-left: 1px;">Rate</span>
												</li>
											</ul>
										</div>
									</li>
								</ul>
							</div>
							<br>
							<div id="electricityCostTreeDiv" class="datagrid">
								<div class="scrolling-wrapper">
									<ul class="format-listitem">
										<span>Type</span>
										<li>
											<div id="variance0" class="far fa-times-circle" style="padding-right: 3px;"></div>
											<input type="radio" name="group0" id="variance0radio" guid="0" checked onclick='openTab("Forecast", "cardDiv");  updateChart(this, electricityChart);'><span id="variance0span" style="padding-left: 1px;">Forecast v Invoice Summary</span>
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
																	<input type="radio" name="group0" id="wholesaleCostElement0radio" guid="0" onclick='openTab("Wholesale", "cardDiv");  updateChart(this, electricityChart);'><span id="wholesaleCostElement0span" style="padding-left: 1px;">Wholesale</span>
																</li>
																<li>
																	<div id="distributionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="distributionCostElement0radio" guid="0"><span id="distributionCostElement0span" style="padding-left: 1px;">Distribution Use of Systems</span>
																</li>
																<li>
																	<div id="transmissionCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="transmissionCostElement0radio" guid="0"><span id="transmissionCostElement0span" style="padding-left: 1px;">Transmission Use of Systems</span>
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
																	<input type="radio" name="group0" id="renewablesobligationCostElement0radio" guid="0"><span id="renewablesobligationCostElement0span" style="padding-left: 1px;">Renewables Obligation</span>
																</li>
																<li>
																	<div id="feedintariffCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="feedintariffCostElement0radio" guid="0"><span id="feedintariffCostElement0span" style="padding-left: 1px;">Feed In Tariff</span>
																</li>
																<li>
																	<div id="contractsfordifferenceCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="contractsfordifferenceCostElement0radio" guid="0"><span id="contractsfordifferenceCostElement0span" style="padding-left: 1px;">Contracts For Difference</span>
																</li>
																<li>
																	<div id="energyintensiveindustriesCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="energyintensiveindustriesCostElement0radio" guid="0"><span id="energyintensiveindustriesCostElement0span" style="padding-left: 1px;">Energy Intensive Industries</span>
																</li>
																<li>
																	<div id="capacitymarketsCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="capacitymarketsCostElement0radio" guid="0"><span id="capacitymarketsCostElement0span" style="padding-left: 1px;">Capacity Markets</span>
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
																	<input type="radio" name="group0" id="bsuosCostElement0radio" guid="0"><span id="bsuosCostElement0span" style="padding-left: 1px;">Balancing System Use of Systems</span>
																</li>
																<li>
																	<div id="rcrcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="rcrcCostElement0radio" guid="0"><span id="rcrcCostElement0span" style="padding-left: 1px;">Reallocation Cashflow Residual Cashflow</span>
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
																	<input type="radio" name="group0" id="managementfeeCostElement0radio" guid="0"><span id="managementfeeCostElement0span" style="padding-left: 1px;">Management Fee</span>
																</li>
																<li>
																	<div id="aahedcCostElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="group0" id="aahedcCostElement0radio" guid="0"><span id="aahedcCostElement0span" style="padding-left: 1px;">Hydro Charge</span>
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
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<div id="cardDiv">
					</div>
				</div>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<div>
					<div>
						<div class="tree-column" style="padding-top: 5px;">
							<div id="gasTreeDiv" class="tree-div">
							</div>
							<br>
							<div id="gasDisplayTreeDiv" class="datagrid">
								<ul class="format-listitem">
									<li>
										<div id="displayCostGasElement0"></div>
										<span id="displayCostGasElement0span">Display</span>
										<div id="displayCostGasElement0List">
											<ul class="format-listitem">
												<li>
													<div id="usageCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="gasGroup1" id="usageCostGasElement0radio" checked guid="0" onclick='openTab("Usage", "cardDiv");  updateChart(this, gasChart);'><span id="usageCostGasElement0span" style="padding-left: 1px;">Usage</span>
												</li>
												<li>
													<div id="costCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="gasGroup1" id="costCostGasElement0radio" guid="0" onclick='openTab("Cost", "cardDiv");  updateChart(this, gasChart);'><span id="costCostGasElement0span" style="padding-left: 1px;">Cost</span>
												</li>
												<li>
													<div id="rateCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="radio" name="gasGroup1" id="rateCostGasElement0radio" guid="0" onclick='openTab("Rate", "cardDiv");  updateChart(this, gasChart);'><span id="rateCostGasElement0span" style="padding-left: 1px;">Rate</span>
												</li>
											</ul>
										</div>
									</li>
								</ul>
							</div>
							<br>
							<div id="gasCostTreeDiv" class="datagrid">
								<div class="scrolling-wrapper">
									<ul class="format-listitem">
										<span>Type</span>
										<li>
											<div id="variance0" class="far fa-times-circle" style="padding-right: 3px;"></div>
											<input type="radio" name="gasGroup0" id="variance0radio" guid="0" checked onclick='openTab("Forecast", "cardDiv");  updateChart(this, gasChart);'><span id="variance0span" style="padding-left: 1px;">Forecast v Invoice Summary</span>
										</li>
										<li>
											<div id="variance1" class="far fa-plus-square" style="padding-right: 4px;"></div>
											<span id="variance1span" style="padding-left: 1px;">Cost GasElements</span>
											<div id="variance1List" class="listitem-hidden">
												<ul class="format-listitem">
													<li>
														<div id="networkCostGasElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
														<span id="networkCostGasElement0span" style="padding-left: 1px;">Network</span>
														<div id="networkCostGasElement0List" class="listitem-hidden">
															<ul class="format-listitem">
																<li>
																	<div id="wholesaleCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="wholesaleCostGasElement0radio" guid="0" onclick='openTab("Wholesale", "cardDiv");  updateChart(this, gasChart);'><span id="wholesaleCostGasElement0span" style="padding-left: 1px;">Wholesale</span>
																</li>
																<li>
																	<div id="distributionCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="distributionCostGasElement0radio" guid="0"><span id="distributionCostGasElement0span" style="padding-left: 1px;">Distribution Use of Systems</span>
																</li>
																<li>
																	<div id="transmissionCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="transmissionCostGasElement0radio" guid="0"><span id="transmissionCostGasElement0span" style="padding-left: 1px;">Transmission Use of Systems</span>
																</li>
															</ul>
														</div>
													</li>
													<li>
														<div id="renewablesCostGasElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
														<span id="renewablesCostGasElement0span" style="padding-left: 1px;">Renewables</span>
														<div id="renewablesCostGasElement0List" class="listitem-hidden">
															<ul class="format-listitem">
																<li>
																	<div id="renewablesobligationCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="renewablesobligationCostGasElement0radio" guid="0"><span id="renewablesobligationCostGasElement0span" style="padding-left: 1px;">Renewables Obligation</span>
																</li>
																<li>
																	<div id="feedintariffCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="feedintariffCostGasElement0radio" guid="0"><span id="feedintariffCostGasElement0span" style="padding-left: 1px;">Feed In Tariff</span>
																</li>
																<li>
																	<div id="contractsfordifferenceCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="contractsfordifferenceCostGasElement0radio" guid="0"><span id="contractsfordifferenceCostGasElement0span" style="padding-left: 1px;">Contracts For Difference</span>
																</li>
																<li>
																	<div id="energyintensiveindustriesCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="energyintensiveindustriesCostGasElement0radio" guid="0"><span id="energyintensiveindustriesCostGasElement0span" style="padding-left: 1px;">Energy Intensive Industries</span>
																</li>
																<li>
																	<div id="capacitymarketsCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="capacitymarketsCostGasElement0radio" guid="0"><span id="capacitymarketsCostGasElement0span" style="padding-left: 1px;">Capacity Markets</span>
																</li>
															</ul>
														</div>
													</li>
													<li>
														<div id="balancingCostGasElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
														<span id="balancingCostGasElement0span" style="padding-left: 1px;">Balancing</span>
														<div id="balancingCostGasElement0List" class="listitem-hidden">
															<ul class="format-listitem">
																<li>
																	<div id="bsuosCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="bsuosCostGasElement0radio" guid="0"><span id="bsuosCostGasElement0span" style="padding-left: 1px;">Balancing System Use of Systems</span>
																</li>
																<li>
																	<div id="rcrcCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="rcrcCostGasElement0radio" guid="0"><span id="rcrcCostGasElement0span" style="padding-left: 1px;">Reallocation Cashflow Residual Cashflow</span>
																</li>
															</ul>
														</div>
													</li>
													<li>
														<div id="otherCostGasElement0" style="padding-right: 4px;" class="far fa-plus-square"></div>
														<span id="otherCostGasElement0span" style="padding-left: 1px;">Other</span>
														<div id="otherCostGasElement0List" class="listitem-hidden">
															<ul class="format-listitem">
																<li>
																	<div id="managementfeeCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="managementfeeCostGasElement0radio" guid="0"><span id="managementfeeCostGasElement0span" style="padding-left: 1px;">Management Fee</span>
																</li>
																<li>
																	<div id="aahedcCostGasElement0" style="padding-right: 4px;" class="far fa-times-circle"></div>
																	<input type="radio" name="gasGroup0" id="aahedcCostGasElement0radio" guid="0"><span id="aahedcCostGasElement0span" style="padding-left: 1px;">Hydro Charge</span>
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
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<div id="cardDiv">
					</div>
				</div>
			</div>	
		</div>
	</div>
	<br>
</body>

<script src="varianceanalysis.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="varianceanalysis.json"></script>

<script type="text/javascript"> 
	loadPage();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>