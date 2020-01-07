<?php 
	$PAGE_TITLE = "Supplier Product Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

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
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/supplierproducttree.js"></script>
<script src="/javascript/supplierproducttab.js"></script>
<script type="text/javascript" src="/basedata/supplierproduct.json"></script>

<script type="text/javascript"> 
var siteCardViewAttributes = [
	"ProductName",
	"DayPeriod",
	"NightPeriod",
	"Evening&WeekendPeriod"
]
	var data = supplierproduct;
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