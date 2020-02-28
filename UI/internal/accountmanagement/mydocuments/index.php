<?php 
	$PAGE_TITLE = "My Documents";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="mydocuments.css">
</head>

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
					<br>
					<button style="width: 100%;" onclick='addDocument()'>Upload Document</button>
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

<script type="text/javascript" src="mydocuments.js"></script>
<script type="text/javascript" src="mydocuments.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>