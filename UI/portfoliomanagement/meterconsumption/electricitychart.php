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
							<select class="show-pointer" onclick="updateChart(this, electricityChart)">
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
							<select class="show-pointer">
								<option value="Daily">Daily</option>
								<option value="6 Hours">6 Hours</option>
								<option value="Weekly">Weekly</option>
								<option value="Monthly">Monthly</option>
								<option value="Yearly">Yearly</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>
					</span>
					<span class="simple-divider" style="padding-left: 5px;"></span>
					<input type="date" name="calendar" id="calendar" value="2019-11-26">
					<span class="simple-divider"></span>
					<span style="padding-left: 5px;">
						<select class="show-pointer">
							<option value="Line">Line</option>
							<option value="Bar">Bar</option>
							<option value="Stacked Line">Stacked Line</option>
							<option value="Stacked Bar">Stacked Bar</option>
							<option value="Stacked %">Stacked %</option>
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
	<div class="tree-div">
		<div id="electricityChart">
		</div>
	</div>
</div>