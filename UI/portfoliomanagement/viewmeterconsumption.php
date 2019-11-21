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
		resizeCharts(365);
	}

	window.onresize = function(){
		resizeCharts(365);
	}

	function resizeCharts(windowWidthReduction){
		var finalColumns = document.getElementsByClassName("final-column");
		var chartWidth = window.innerWidth - windowWidthReduction;

		for(var i=0; i<finalColumns.length; i++){
			finalColumns[i].setAttribute("style", "width: "+chartWidth+"px;");
		}
	}
</script> 

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>