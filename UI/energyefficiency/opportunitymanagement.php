<?php 
	$PAGE_TITLE = "Opportunity Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="tree-column">
			<div>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="divcolumn left" style="border: solid black 1px"></div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn right" style="border: solid black 1px"></div>
				<div class="divcolumn last"></div>
			</div>
			<br>
			<div class="row">
				<div class="divcolumn first"></div>
				<div style="border: solid black 1px; width: 96%;"></div>
				<div class="divcolumn last"></div>
			</div>			
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>

<script type="text/javascript"> 
	window.onload = function(){
		resizeFinalColumns(375);
	}

	window.onresize = function(){
		resizeFinalColumns(375);
	}
	</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>