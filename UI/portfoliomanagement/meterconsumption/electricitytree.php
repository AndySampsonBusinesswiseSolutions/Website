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
							<select style="z-index: 99;" class="show-pointer">
								<option value="0">Device Type</option>
								<option value="1">Zone>Panel</option>
								<option value="2">Device Groups</option>
								<option value="3">Hierarchy</option>
								<option value="4">Alphabetically</option>
								<option value="5">Sensor Type</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>
					</span>
				</div>
			</div>
		</div>
		<br>
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