<?php 
	$PAGE_TITLE = "My Documents";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div>
		<div id="loaHeader">
			<div style="padding: 15px;">
				<div class="datagrid" style="height: 100px;">
					Your current LOA details:
					<br><span style="font-size: 20px; color: red;">Your current LOA is 5 days away from expiring!</span>
					<br><span>Here we can put any other info about the current LOA or anything we so desire</span>
				</div>
			</div>
		<div>
	</div>	
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
			<div>
				<div class="group-by-div" id="cardDiv" style="display: none;">
					<div class="tabDiv" id="tabDiv" style="overflow-y: auto; overflow: auto;"></div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/documenttree.js"></script>
<script src="/javascript/documenttab.js"></script>
<script type="text/javascript" src="/basedata/document.json"></script>

<script type="text/javascript"> 
	var data = documents;
	createTree(data, "treeDiv", "createCardButton");
	addExpanderOnClickEvents();	

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>