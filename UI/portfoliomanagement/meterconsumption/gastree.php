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
						<span class="arrow-header" id="gasChartHeaderPeriod">
							<select style="z-index: 99;" class="show-pointer">
								<option value="0">Device Type</option>
								<option value="1">Zone>Panel</option>
								<option value="2">Device Groups</option>
								<option value="3">Hierarchy</option>
								<option value="4">Alphabetically</option>
								<option value="5">Sensor Type</option>
							</select>
						</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="gasChartHeaderPeriodArrow"></span>
					</span>
				</div>
			</div>
		</div>
		<br>
		<div id="gasTreeDiv" class="tree-div">
			<div class="scrolling-wrapper">
				<ul class="format-listitem">
					<li>
						<div class="far fa-plus-square" id="gasSite1"></div>
						<input type="checkbox"></input>
						<i class="fas fa-map-marker-alt"></i>
						<span>Sulzer Pumps: Leeds</span>
						<div class="listitem-hidden" id="gasSite1List">
							<ul class="format-listitem">
								<li>
									<div class="far fa-plus-square" id="gasDeviceType1"></div>
									<input type="checkbox"></input>
									<i class="fas fa-burn"></i>
									<span>Mains</span>
									<div class="listitem-hidden" id="gasDeviceType1List">
										<ul class="format-listitem">
											<li>
												<div class="far fa-plus-square" id="gasDeviceSubType1"></div>
												<input type="checkbox"></input>
												<i class="fas fa-burn"></i>
												<span>Mains</span>
											</li>
											<li>
												<div class="far fa-plus-square" id="gasDeviceSubType2"></div>
												<input type="checkbox"></input>
												<i class="fas fa-burn"></i>
												<span>Sub-mains</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType2"></div>
									<input type="checkbox"></input>
									<i class="fas fa-lightbulb"></i>
									<span>Lighting</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType3"></div>
									<input type="checkbox"></input>
									<i class="fas fa-cogs"></i>
									<span>Machinery</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType4"></div>
									<input type="checkbox"></input>
									<i class="fas fa-building"></i>
									<span>Office Appliances</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType5"></div>
									<input type="checkbox"></input>
									<i class="fas fa-question-circle"></i>
									<span>Unknown</span>
								</li>
							</ul>
						</div>
					</li>
					<li>
						<div class="far fa-plus-square" id="gasSite2"></div>
						<input type="checkbox"></input>
						<i class="fas fa-map-marker-alt"></i>
						<span>Sulzer Pumps: Manchester</span>
						<div class="listitem-hidden" id="gasSite2List">
							<ul class="format-listitem">
								<li>
									<div class="far fa-plus-square" id="gasDeviceType6"></div>
									<input type="checkbox"></input>
									<i class="fas fa-burn"></i>
									<span>Mains</span>
									<div class="listitem-hidden" id="gasDeviceType6List">
										<ul class="format-listitem">
											<li>
												<div class="far fa-plus-square" id="gasDeviceSubType3"></div>
												<input type="checkbox"></input>
												<i class="fas fa-burn"></i>
												<span>Mains</span>
											</li>
											<li>
												<div class="far fa-plus-square" id="gasDeviceSubType4"></div>
												<input type="checkbox"></input>
												<i class="fas fa-burn"></i>
												<span>Sub-mains</span>
											</li>
										</ul>
									</div>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType7"></div>
									<input type="checkbox"></input>
									<i class="fas fa-lightbulb"></i>
									<span>Lighting</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType8"></div>
									<input type="checkbox"></input>
									<i class="fas fa-cogs"></i>
									<span>Machinery</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType9"></div>
									<input type="checkbox"></input>
									<i class="fas fa-building"></i>
									<span>Office Appliances</span>
								</li>
								<li>
									<div class="far fa-plus-square" id="gasDeviceType10"></div>
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