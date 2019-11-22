<div>
	<br><b style="padding-left: 15px;">Electricity Meters</b>
	<div class="tree-column">
		<div id="electricityGroupDiv" class="group-div">
			<div id="electricityGroupByDiv" class="group-by-div">
				<div style="width: 30%; display: inline-block;">
					<span>Group By:</span>
				</div>
				<div style="float: right;" class="show-pointer">
					<span class="arrow-header" style="padding-right: 5px;" id="selectElectricityGroupByType">Device Type</span>
					<span class="simple-divider"></span>
					<span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px; padding-right: 5px;" id="selectElectricityGroupByTypeArrow"></span>
						<div class="listitem-hidden dropdown-border" style="position: relative;" id="selectElectricityGroupByTypeSubMenu">
							<ul class="format-listitem" style="padding: 1px; margin-bottom: 0">
								<li onclick="updateGroupByType('Device Type','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Device Type</li>
								<li onclick="updateGroupByType('Zone>Panel','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Zone>Panel</li>
								<li onclick="updateGroupByType('Device Groups','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Device Groups</li>
								<li onclick="updateGroupByType('Hierarchy','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Hierarchy</li>
								<li onclick="updateGroupByType('Alphabetically','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Alphabetically</li>
								<li onclick="updateGroupByType('Sensor Type','selectElectricityGroupByType','selectElectricityGroupByTypeSubMenu')">Sensor Type</li>
							</ul>
						</div>
					</span>
				</div>
			</div>
		</div>
		<div id="electricityTreeDiv" class="tree-div">
			<div class="scrolling-wrapper">
				<ul class="format-listitem">
					<li>
						<div class="far fa-plus-square" id="electricitySite1"></div>
						<input type="checkbox"></input>
						<i class="fas fa-map-marker-alt"></i>
						<span>Sulzer Pumps: Leeds</span>
						<div class="listitem-hidden" id="electricitySite1List">
							<ul class="format-listitem">
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType1"></div>
									<input type="checkbox"></input>
									<i class="fas fa-plug"></i>
									<span>Mains</span>
									<div class="listitem-hidden" id="electricityDeviceType1List">
										<ul class="format-listitem">
											<li>
												<div class="far fa-plus-square" id="electricityDeviceSubType1"></div>
												<input type="checkbox"></input>
												<i class="fas fa-plug"></i>
												<span>Mains</span>
											</li>
											<li>
												<div class="far fa-plus-square" id="electricityDeviceSubType2"></div>
												<input type="checkbox"></input>
												<i class="fas fa-plug"></i>
												<span>Sub-mains</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType2"></div>
									<input type="checkbox"></input>
									<i class="fas fa-lightbulb"></i>
									<span>Lighting</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType3"></div>
									<input type="checkbox"></input>
									<i class="fas fa-cogs"></i>
									<span>Machinery</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType4"></div>
									<input type="checkbox"></input>
									<i class="fas fa-building"></i>
									<span>Office Appliances</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType5"></div>
									<input type="checkbox"></input>
									<i class="fas fa-question-circle"></i>
									<span>Unknown</span>
								</li>
							</ul>
						</div>
					</li>
					<li>
						<div class="far fa-plus-square" id="electricitySite2"></div>
						<input type="checkbox"></input>
						<i class="fas fa-map-marker-alt"></i>
						<span>Sulzer Pumps: Manchester</span>
						<div class="listitem-hidden" id="electricitySite2List">
							<ul class="format-listitem">
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType6"></div>
									<input type="checkbox"></input>
									<i class="fas fa-plug"></i>
									<span>Mains</span>
									<div class="listitem-hidden" id="electricityDeviceType6List">
										<ul class="format-listitem">
											<li>
												<div class="far fa-plus-square" id="electricityDeviceSubType3"></div>
												<input type="checkbox"></input>
												<i class="fas fa-plug"></i>
												<span>Mains</span>
											</li>
											<li>
												<div class="far fa-plus-square" id="electricityDeviceSubType4"></div>
												<input type="checkbox"></input>
												<i class="fas fa-plug"></i>
												<span>Sub-mains</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType7"></div>
									<input type="checkbox"></input>
									<i class="fas fa-lightbulb"></i>
									<span>Lighting</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType8"></div>
									<input type="checkbox"></input>
									<i class="fas fa-cogs"></i>
									<span>Machinery</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType9"></div>
									<input type="checkbox"></input>
									<i class="fas fa-building"></i>
									<span>Office Appliances</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="electricityDeviceType10"></div>
									<input type="checkbox"></input>
									<i class="fas fa-question-circle"></i>
									<span>Unknown</span>
								</li>
							</ul>
						</div>
					</li>						
				</ul>
			</div>
		</div>
	</div>
</div>

<script>
	function updateGroupByType(groupBy, groupByType, groupByTypeSubMenu){
		var groupByTypeElement = document.getElementById(groupByType);
		groupByTypeElement.innerText = groupBy;

		updateClassOnClick(groupByTypeSubMenu, "listitem-hidden", "")
		updateClassOnClick(groupByType.concat('Arrow'), "fa-angle-double-down", "fa-angle-double-up")
	}
</script>