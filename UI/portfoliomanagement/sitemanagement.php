<?php 
	$PAGE_TITLE = "Site Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<SCRIPT SRC="/javascript/utils.js"></SCRIPT>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<span>Click on the text to show/hide meters and submeters</span>
	<div class="row">
		<div class="column">
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
			<div><div class="chartjs-size-monitor"><div class="chartjs-size-monitor-expand"><div class=""></div></div><div class="chartjs-size-monitor-shrink"><div class=""></div></div></div>
				<canvas id="canvas" class="chartjs-render-monitor"></canvas>
			</div>
			<br>
			<br>

			<button id="addData">Add Data</button>
			<button id="removeData">Remove Data</button>
		</div>
	</div>

	<script>
		var datasets = [,];
		initialiseTree();
		getDummyDataSets(datasets);
		initialiseGraph(window, document, datasets);
/* 
		document.getElementById('addData').addEventListener('click', function() {
			if (config.data.datasets.length > 0) {
				var month = MONTHS[config.data.labels.length % MONTHS.length];
				config.data.labels.push(month);

				config.data.datasets.forEach(function(dataset) {
					dataset.data.push(randomScalingFactor());
				});

				window.myLine.update();
			}
		});

		document.getElementById('removeData').addEventListener('click', function() {
			config.data.labels.splice(-1, 1); // remove the label first

			config.data.datasets.forEach(function(dataset) {
				dataset.data.pop();
			});

			window.myLine.update();
		}); */
	</script>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>