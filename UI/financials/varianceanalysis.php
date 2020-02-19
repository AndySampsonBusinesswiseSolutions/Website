<?php 
	$PAGE_TITLE = "Variance Analysis";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">  

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/variance/electricitytree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/variance/electricitychart.php") ?>
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
			<div class="tree-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/gastree.php") ?>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				<?php include($_SERVER['DOCUMENT_ROOT']."/financials/actuals/gaschart.php") ?>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/variancechart3.js"></script>
<script src="/javascript/actualsvbudgettree.js"></script>
<script src="/javascript/variancetab3.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="/basedata/variance.json"></script>

<script>
	function addCommoditySelectorOnClickEvent() {
		var commoditySelector = document.getElementById("electricityGasSelector");
		commoditySelector.addEventListener('click', function(event) {
			updateClassOnClick("electricityDiv", "listitem-hidden", "")
			updateClassOnClick("gasDiv", "listitem-hidden", "")
		})	
	}
</script>

<script type="text/javascript"> 
	window.onload = function(){
		resizeFinalColumns(365);

		createTree(data, "electricityTreeDiv", "electricity", "updateChart(electricityChart)", true);
		createTree(data, "gasTreeDiv", "gas", "updateChart(gasChart)", true);

		addExpanderOnClickEvents();
		addArrowOnClickEvents();
		addCommoditySelectorOnClickEvent();
		createCardButtons();
	}

	window.onresize = function(){
		resizeFinalColumns(365);
	}	
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>