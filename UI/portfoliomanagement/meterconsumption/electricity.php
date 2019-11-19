<div id="electricityDiv">
	<span>Electricity Div</span>
	<div>
		<div class="tree-column">
			<div id="electricityGroupDiv" class="group-div">
				<div id="electricityGroupByDiv" class="group-by-div">
					<div style="display: inline;">Group By:</div>
					<div style="display: inline; cursor: pointer;" id="selectElectricityGroupByType" onmouseover="">
						<div style="display: inline;">
							<div style="display: inline; padding-left: 10px; padding-right: 50px;">Device Type</div>
							<div style="display: inline; border-left: solid black 1px;"></div>
							<div class="fa fa-angle-double-down" style="display: inline; padding-left: 10px;" id="electricityGroupArrow"></div>				
						</div>
					</div>
				</div>
			</div>
			<div id="electricityTreeDiv" class="tree-div">
				<div class="scrolling-wrapper">
					<ul style="padding-left: 0; margin-left: 0;">
						<li style="display: inline; padding-left: 15px;">
							<div class="far fa-plus-square" id="Site1"></div>
							<input type="checkbox"></input>
							<i class="fas fa-map-marker-alt"></i>
							<div style="display: inline-block;">Sulzer Pumps: Leeds</div>
						</li>
						<div class="listitem-hidden" id="Site1List">
							<ul>
								<li style="display: inline;">
									<div class="far fa-plus-square" id="DeviceType1"></div>
									<input type="checkbox"></input>
									<i class="fas fa-plug"></i>
									<div style="display: inline-block;">Mains</div>
								</li>
							</ul>
						</div>
					</ul>
				</div>
			</div>
		</div>
	</div>
	<div></div>

	<script type="text/javascript"> 
        document.getElementById('selectElectricityGroupByType')
			.addEventListener('click', function (event) {
				updateClassOnClick("electricityGroupArrow", "fa-angle-double-down", "fa-angle-double-up")
			});

		var expanders = document.getElementsByClassName("fa-plus-square");
		for(var i=0; i< expanders.length; i++){
			expanders[i].addEventListener('click', function (event) {
				updateClassOnClick(this.id, "fa-plus-square", "fa-minus-square")
				updateClassOnClick(this.id.concat('List'), "listitem-hidden", "")
			});
		}
    </script> 
</div>