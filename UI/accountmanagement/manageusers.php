<?php 
	$PAGE_TITLE = "Manage Users";
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
<script src="/javascript/usertree.js"></script>
<script src="/javascript/usertab.js"></script>
<script type="text/javascript" src="/basedata/user.json"></script>

<link href="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.css" rel="stylesheet"/>
<script src="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.js"></script>

<script type="text/javascript"> 
var userCardViewAttributes = [
	"Address Line 1",
	"Address Line 2",
	"Address Line 3",
	"Address Line 4",
	"Postcode",
	"Contact Name",
	"Contact Telephone Number",
	"Email"
]
	var data = user;
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