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
							<select class="show-pointer">
								<option value="0">Energy</option>
								<option value="1">Power</option>
								<option value="2">Current</option>
								<option value="3">Cost</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderShowByArrow"></span>
					</span>          
					<span style="padding-right: 5px;" class="simple-divider"></span>
					<span>Period:</span>
					<span title="Period" class="show-pointer">
						<span class="arrow-header" id="electricityChartHeaderPeriod">
							<select class="show-pointer">
								<option value="0">6 Hours</option>
								<option value="1">Daily</option>
								<option value="2">Weekly</option>
								<option value="3">Monthly</option>
								<option value="4">Yearly</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>
					</span>
					<span class="simple-divider" style="padding-left: 5px;"></span>
					<input type="date" name="calendar" id="calendar" value="2019-11-25">
					<span class="simple-divider"></span>
					<span style="padding-left: 5px;">
						<select class="show-pointer">
							<option value="0">Line</option>
							<option value="1">Bar</option>
							<option value="2">Stacked Line</option>
							<option value="3">Stacked Bar</option>
							<option value="4">Stacked %</option>
						</select>
					</span>
					<span title="Chart Type" class="fas fa-chart-bar show-pointer"></span>
					<span class="simple-divider"></span>
					<span style="padding-left: 5px;">
						<select class="show-pointer">
							<option value="0">Outside Temp</option>
							<option value="1">On/Off Hours</option>
							<option value="2">Sensor Level (RSSI)</option>
							<option value="3">Bulk</option>
							<option value="4">Error Correction</option>
						</select>
					</span>
					<span title="Layers" class="fas fa-layer-group show-pointer"></span>
					<span class="simple-divider"></span>
					<span style="padding-left: 5px;">
						<select class="show-pointer">
							<option value="0">Xlsx</option>
							<option value="1">Csv</option>
							<option value="2">Image</option>
							<option value="3">Pdf</option>
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
	<div class="tree-div">
	</div>
</div>