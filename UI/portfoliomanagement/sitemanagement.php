<?php 
	$PAGE_TITLE = "Site Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<style>
	th {
		text-align: center;
	}

	tr:nth-child(even) {
		background-color: #dddddd;
	}
</style>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	

	<div class="row"> -->
		<div class="tree-column">
			<div>
				<br>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<br>
			<div>
				<div class="group-by-div" id="cardDiv" style="display: none;">
					<div class="tabDiv" id="tabDiv" style="overflow-y: auto; overflow: auto;"></div>
				<!-- <div id="map-canvas" style="width: 250px; height: 250px;"></div> -->
					<!-- <div style="overflow-y:auto;" class="datagrid" id="displayAttributes">
					</div> -->
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/tree.js"></script>
<script src="/javascript/map.js"></script>
<script src="/javascript/tab.js"></script>
<script type="text/javascript" src="/basedata/data.json"></script>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"
  type="text/javascript"></script>

<script type="text/javascript"> 
	createTree(data, "Hierarchy", "treeDiv", "", "createCardButton");
	addExpanderOnClickEvents();
	// buildDataTable();

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}

	//initializeMap('BB9 5SR');
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>