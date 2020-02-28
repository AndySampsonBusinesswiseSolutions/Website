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

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<div>
					<br><b style="padding-left: 15px;">Electricity Meters</b>
					<div class="tree-column">
						<div id="electricityGroupDiv" class="group-div">
							<div id="electricityGroupByDiv" class="group-by-div">
								<div style="width: 30%; display: inline-block;">
									<span>Group By:</span>
								</div>
								<div style="float: right;" class="show-pointer">
									<span title="Period" class="show-pointer">
										<span class="arrow-header" id="electricityTreeHeaderGroupBy">
											<select class="show-pointer" onchange="createTree(data, this.value, 'electricityTreeDiv', 'electricity', 'updateChart(electricityChart)'); addExpanderOnClickEvents(); updateChart(this, electricityChart)">
												<option value="DeviceType">Device Type</option>
												<option value="Zone">Zone>Panel</option>
												<option value="Hierarchy">Hierarchy</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityTreeHeaderGroupByArrow"></span>
									</span>
								</div>
							</div>
						</div>
						<br>
						<div id="electricityTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<br>
					<br>
					<div>
						<div class="group-div">
							<div class="group-by-div">
								<div style="width: 30%; display: inline-block;">
									<span class="fas fa-chart-line" style="padding-right: 5px"></span>
									<span class="chart-header">Electricity Time View</span>
								</div>
								<div style="float: right;">
									<span style="padding-right: 5px;" class="simple-divider"></span>
									<span>Show By:</span>
									<span title="Show By" class="show-pointer">
										<span class="arrow-header" id="electricityChartHeaderShowBy">
											<select class="show-pointer" onchange="updateChart(this, electricityChart)">
												<option value="Energy">Energy</option>
												<option value="Power">Power</option>
												<option value="Current">Current</option>
												<option value="Cost">Cost</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderShowByArrow"></span>
									</span>          
									<span style="padding-right: 5px;" class="simple-divider"></span>
									<span>Period:</span>
									<span title="Period" class="show-pointer">
										<span class="arrow-header" id="electricityChartHeaderPeriod">
											<select class="show-pointer" onchange="updateChart(this, electricityChart)">
												<option value="Daily">Daily</option>
												<option value="Weekly">Weekly</option>
												<option value="Monthly">Monthly</option>
												<option value="Yearly">Yearly</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>
									</span>
									<span class="simple-divider" style="padding-left: 5px;"></span>
									<input type="date" name="calendar" id="electricityCalendar" value="2019-11-26" onchange="updateChart(this, electricityChart)">
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;" id="electricityChartHeaderType">
										<select class="show-pointer" onchange="updateChart(this, electricityChart)">
											<option value="Line">Line</option>
											<option value="Bar">Bar</option>
											<option value="Stacked Line">Stacked Line</option>
											<option value="Stacked Bar">Stacked Bar</option>
											<option value="Area">Area</option>
										</select>
									</span>
									<span title="Chart Type" class="fas fa-chart-bar show-pointer"></span>
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;">
										<select class="show-pointer">
											<option value="Outside Temp">Outside Temp</option>
											<option value="On/Off Hours">On/Off Hours</option>
											<option value="Sensor Level (RSSI)">Sensor Level (RSSI)</option>
											<option value="Bulk">Bulk</option>
											<option value="Error Correction">Error Correction</option>
										</select>
									</span>
									<span title="Layers" class="fas fa-layer-group show-pointer"></span>
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;">
										<select class="show-pointer">
											<option value="Xlsx">Xlsx</option>
											<option value="Csv">Csv</option>
											<option value="Image">Image</option>
											<option value="Pdf">Pdf</option>
										</select>
									</span>
									<span title="Download" class="fas fa-download show-pointer"></span>
									<span class="simple-divider"></span>
									<span title="Refresh" class="fas fa-sync show-pointer"></span>
								</div>
							</div>
						</div>
					</div>
					<br>
					<div class="chart">
						<div id="electricityChart">
						</div>
					</div>
				</div>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<div>
					<br><b style="padding-left: 15px;">Gas Meters</b>
					<div class="tree-column">
						<div id="gasGroupDiv" class="group-div">
							<div id="gasGroupByDiv" class="group-by-div">
								<div style="width: 30%; display: inline-block;">
									<span>Group By:</span>
								</div>
								<div style="float: right;" class="show-pointer">
									<span title="Period" class="show-pointer">
										<span class="arrow-header" id="gasTreeHeaderGroupBy">
											<select class="show-pointer" onchange="createTree(data, this.value, 'gasTreeDiv', 'gas', 'updateChart(gasChart)'); addExpanderOnClickEvents(); updateChart(this, gasChart)">
												<option value="DeviceType">Device Type</option>
												<option value="Zone">Zone>Panel</option>
												<option value="Hierarchy">Hierarchy</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="gasTreeHeaderGroupByArrow"></span>
									</span>
								</div>
							</div>
						</div>
						<br>
						<div id="gasTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<div>
					<br>
					<br>
					<div>
						<div class="group-div">
							<div class="group-by-div">
								<div style="width: 30%; display: inline-block;">
									<span class="fas fa-chart-line" style="padding-right: 5px"></span>
									<span class="chart-header">Gas Time View</span>
								</div>
								<div style="float: right;">
									<span style="padding-right: 5px;" class="simple-divider"></span>
									<span>Show By:</span>
									<span title="Show By" class="show-pointer">
										<span class="arrow-header" id="gasChartHeaderShowBy">
											<select class="show-pointer" onchange="updateChart(this, gasChart)">
												<option value="Energy">Energy</option>
												<option value="Cost">Cost</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="gasChartHeaderShowByArrow"></span>
									</span>          
									<span style="padding-right: 5px;" class="simple-divider"></span>
									<span>Period:</span>
									<span title="Period" class="show-pointer">
										<span class="arrow-header" id="gasChartHeaderPeriod">
											<select class="show-pointer" onchange="updateChart(this, gasChart)">
												<option value="Monthly">Monthly</option>
												<option value="Yearly">Yearly</option>
											</select>
										</span>
										<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="gasChartHeaderPeriodArrow"></span>
									</span>
									<span class="simple-divider" style="padding-left: 5px;"></span>
									<input type="date" name="calendar" id="gasCalendar" value="2019-11-26" onchange="updateChart(this, gasChart)">
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;" id="gasChartHeaderType">
										<select class="show-pointer" onchange="updateChart(this, gasChart)">
											<option value="Line">Line</option>
											<option value="Bar">Bar</option>
											<option value="Stacked Line">Stacked Line</option>
											<option value="Stacked Bar">Stacked Bar</option>
											<option value="Area">Area</option>
										</select>
									</span>
									<span title="Chart Type" class="fas fa-chart-bar show-pointer"></span>
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;">
										<select class="show-pointer">
											<option value="Outside Temp">Outside Temp</option>
											<option value="On/Off Hours">On/Off Hours</option>
											<option value="Sensor Level (RSSI)">Sensor Level (RSSI)</option>
											<option value="Bulk">Bulk</option>
											<option value="Error Correction">Error Correction</option>
										</select>
									</span>
									<span title="Layers" class="fas fa-layer-group show-pointer"></span>
									<span class="simple-divider"></span>
									<span style="padding-left: 5px;">
										<select class="show-pointer">
											<option value="Xlsx">Xlsx</option>
											<option value="Csv">Csv</option>
											<option value="Image">Image</option>
											<option value="Pdf">Pdf</option>
										</select>
									</span>
									<span title="Download" class="fas fa-download show-pointer"></span>
									<span class="simple-divider"></span>
									<span title="Refresh" class="fas fa-sync show-pointer"></span>
								</div>
							</div>
						</div>
					</div>
					<br>
					<div class="chart">
						<div id="gasChart">
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="viewmeterconsumption.json"></script>
<script type="text/javascript" src="viewmeterconsumption.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>