<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Bill Validation";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="billvalidation.css">
</head>

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
				<div class="group-by-div" id="cardDiv" style="display: none;">
					<div class="tabDiv" id="tabDiv" style="overflow-y: auto; overflow: auto;"></div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="billvalidation.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="billvalidation.json"></script>
<script type="text/javascript"> 
	loadPage();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>