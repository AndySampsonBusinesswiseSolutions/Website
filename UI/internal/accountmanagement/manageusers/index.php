<?php 
	$PAGE_TITLE = "Manage Users";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="manageusers.css">
</head>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div class="row">
		<div class="tree-column">
			<div>
				<br>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
					<button style="margin-top: 5px; margin-bottom: 5px; width: 100%;" onclick='deleteUsers()'>Delete Selected Users</button>
					<button style="width: 100%;" onclick='reinstateUsers()'>Reinstate Deleted Users</button>
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

<script type="text/javascript" src="manageusers.js"></script>
<script type="text/javascript" src="manageusers.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>