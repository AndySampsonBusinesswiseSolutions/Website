<?php 
	$PAGE_TITLE = "View Capacity";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">  

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/capacity/electricitytree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/portfoliomanagement/capacity/electricitychart.php") ?>
			</div>	
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/capacitychart.js"></script>
<script src="/javascript/actualsvbudgettree.js"></script>
<script src="/javascript/capacitytab.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="/basedata/capacity.json"></script>

<script type="text/javascript"> 
	window.onload = function(){
		resizeFinalColumns(365);

		createTree(data, "electricityTreeDiv", "electricity", "updateChart(electricityChart)", true);

		addExpanderOnClickEvents();
		createCardButtons();
	}

	window.onresize = function(){
		resizeFinalColumns(365);
	}	
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>