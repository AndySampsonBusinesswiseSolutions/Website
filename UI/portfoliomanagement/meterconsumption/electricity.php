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
				Tree here
			</div>
		</div>
	</div>

	<script type="text/javascript"> 
        document.getElementById('selectElectricityGroupByType')
        .addEventListener('click', function (event) {
			updateClassOnClick("electricityGroupArrow", "fa-angle-double-down", "fa-angle-double-up")
		});
    </script> 
</div>