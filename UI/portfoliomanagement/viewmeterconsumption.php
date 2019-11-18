<?php 
	$PAGE_TITLE = "View Meter Consumption";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">  

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div id="mainDiv">
		<br>
		<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/electricity.php") ?>
		<br>
		<br>
		<br>
		<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/meterconsumption/gas.php") ?>
		<br>
	</div>
	
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

<!-- 
<script>
	var datasets = [,];
	getDummyDataSets(datasets);
	initialiseTree(datasets);
	initialiseGraph(window, document, datasets);
</script> -->

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>