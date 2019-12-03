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