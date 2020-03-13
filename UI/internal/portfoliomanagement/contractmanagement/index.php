<?php 
	$PAGE_TITLE = "Contract Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="contractmanagement.css">
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
					<button style="width: 100%; margin-top: 5px;">Upload Contract</button>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<br>
			<div>
					<span>Out Of Contract Meters</span>
					<div id="outOfContract"></div><br>
					<span>Active Contracts</span>
					<div id="active"></div><br>
					<span>Pending Contracts</span>
					<div id="pending"></div><br>
					<span>Finished Contracts</span>
					<div id="finished"></div><br>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="contractmanagement.js"></script>
<script type="text/javascript" src="contractmanagement.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>