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
								<option value="DeviceGroups">Device Groups</option>
								<option value="Alphabetically">Alphabetically</option>
								<option value="SensorType">Sensor Type</option>
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