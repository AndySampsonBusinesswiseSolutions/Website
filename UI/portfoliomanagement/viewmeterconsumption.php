<?php 
	$PAGE_TITLE = "View Meter Consumption";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">  

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div>
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/electricitytree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/electricitychart.php") ?>
			</div>	
		</div>
	</div>
	<div>
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/gastree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/gaschart.php") ?>
			</div>
		</div>
	</div>
	<br>
	
	<!-- <div class="row">
		<div class="column">
			<span>Click on the text to show/hide meters and submeters.</span>
			<div></div>
			<span>View as: </span>
			<select id="chartType" onchange="updateChartType()" class="dropdown">
				<option value = "line">Line Graph</option>
				<option value = "bar">Bar Chart</option>
				<option value = "stackedBar">Stacked Bar Chart</option>
				<option value = "horizontalBar">Horizontal Bar Chart</option>
				<option value = "horizontalStackedBar">Horizontal Stacked Bar Chart</option>
			</select>
			<div>
				<input class="child-check-input" type="checkbox" name="Total Of All Sites" id="0"><label>Total Of All Sites</label>
			</div>
			<div class="parent-check">
				<input class="child-check-input" type="checkbox" name="Site 1" id="1"><label>Site 1</label>
				<div class="child-check">
					<input class="child-check-input" type="checkbox" name="MPAN 1" id="2"><label>MPAN 1</label>
					<div class="child-check">
						<input class="child-check-input" type="checkbox" name="SubMeter 1" id="3"><label>SubMeter 1</label>
					</div>
					<div class="child-check">
						<input class="child-check-input" type="checkbox" name="SubMeter 2" id="4"><label>SubMeter 2</label>
					</div>
				</div>
			</div>
			<div class="parent-check">
				<input class="child-check-input" type="checkbox" name="Site 2" id="5"><label>Site 2</label>
				<div class="child-check">
					<input class="child-check-input" type="checkbox" name="MPAN 2" id="6"><label>MPAN 2</label>
				</div>
			</div>
		</div>
		<div class="finalcolumn" id="rightFrame">
			<div>
				<div class="chartjs-size-monitor">
					<div class="chartjs-size-monitor-expand">
						<div class=""></div>
					</div>
					<div class="chartjs-size-monitor-shrink">
						<div class=""></div>
					</div>
				</div>
				<canvas id="canvas" class="chartjs-render-monitor"></canvas>
			</div>
		</div>
	</div> -->
</body>

<script src="/javascript/utils.js"></script>

<script type="text/javascript"> 
	var arrows = document.getElementsByClassName("fa-angle-double-down");
	for(var i=0; i< arrows.length; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-angle-double-down", "fa-angle-double-up")
		});
	}

	var arrowHeaders = document.getElementsByClassName("arrow-header");
	for(var i=0; i< arrowHeaders.length; i++){
		arrowHeaders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id.concat('Arrow'), "fa-angle-double-down", "fa-angle-double-up")
		});
	}

	var expanders = document.getElementsByClassName("fa-plus-square");
	for(var i=0; i< expanders.length; i++){
		expanders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-plus-square", "fa-minus-square")
			updateClassOnClick(this.id.concat('List'), "listitem-hidden", "")
		});
	}

	window.onload = function(){
		resizeCharts();
	}

	window.onresize = function(){
		resizeCharts();
	}

	function resizeCharts(){
		var finalColumns = document.getElementsByClassName("final-column");
		var chartWidth = window.innerWidth - 365;
		var chartHeaderWidth = chartWidth - 705;
		
		for(var i=0; i<finalColumns.length; i++){
			finalColumns[i].setAttribute("style", "width: "+chartWidth+"px;");
		}

		var chartHeaders = document.getElementsByClassName("chart-header");
		for(var i=0; i<finalColumns.length; i++){
			chartHeaders[i].setAttribute("style", "padding-right: "+chartHeaderWidth+"px; display: inline;");
		}
	}
</script> 

<!-- 
<script>
	var datasets = [,];
	getDummyDataSets(datasets);
	initialiseTree(datasets);
	initialiseGraph(window, document, datasets);
</script> -->

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>