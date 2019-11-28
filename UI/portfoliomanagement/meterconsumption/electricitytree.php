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
						<span class="arrow-header" id="electricityChartHeaderPeriod">
							<select class="show-pointer" onchange="createTree(data, this.value, 'electricityTreeDiv', 'electricity'); addExpanderOnClickEvents();">
								<option value="Device Type">Device Type</option>
								<option value="Zone">Zone>Panel</option>
								<option value="Hierarchy">Hierarchy</option>
								<option value="3Device Groups">Device Groups</option>
								<option value="Alphabetically">Alphabetically</option>
								<option value="Sensor Type">Sensor Type</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>
					</span>
				</div>
			</div>
		</div>
		<br>
		<div id="electricityTreeDiv" class="tree-div">
		</div>
	</div>
</div>