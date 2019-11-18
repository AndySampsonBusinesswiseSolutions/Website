<div id="gasDiv">
	<span>Gas Div</span>
	<div>
		<div class="tree-column">
			<div id="gasGroupDiv" class="group-div">
				<div id="gasGroupByDiv" class="group-by-div">
					<div style="display: inline;">Group By:</div>
					<div style="display: inline; cursor: pointer;" id="selectGasGroupByType" onmouseover="">
						<div style="display: inline;">
							<div style="display: inline; padding-left: 10px; padding-right: 50px;">Device Type</div>
							<div style="display: inline; border-left: solid black 1px;"></div>
							<div class="fa fa-angle-double-down" style="display: inline; padding-left: 10px;" id="gasGroupArrow"></div>				
						</div>
					</div>
				</div>
			</div>
			<div id="gasTreeDiv" class="tree-div">
				Tree here
			</div>
		</div>
	</div>

	<script type="text/javascript"> 
        document.getElementById('selectGasGroupByType')
        .addEventListener('click', function (event) {
			updateClassOnClick("gasGroupArrow", "fa-angle-double-down", "fa-angle-double-up")
		});
    </script> 
</div>