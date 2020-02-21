<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link rel="stylesheet" href="/css/tree.css">

<style>
  .dragable { padding: 0.5em; float: left; margin: 0 10px 10px 0; border: solid 1px black; }
  #containment-wrapper { width: 100%; height:650px; padding: 10px; }
</style>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div>
		<div class="row">
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
				<div class="chart">
					<div id="containment-wrapper">
					</div>
				</div>
				<br>
			</div>	
		</div>
	</div>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/dashboardtree.js"></script>
<script src="/javascript/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script type="text/javascript" src="/basedata/dashboard.json"></script>
<script>
	var data = dashboard;
	createTree(data, "treeDiv", "addDashboardItem()");

  window.onload = function(){
		resizeFinalColumns(365);
	}

	window.onresize = function(){
		resizeFinalColumns(365);
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>