<?php 
	$PAGE_TITLE = "Finished Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="finishedopportunities.css">
</head>

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
			<div class="roundborder">
				<div id="cumulativeSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="cumulativeSavingspan">Cumulative Saving Chart</span>
				<div id="cumulativeSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="cumulativeSavingChart"></div>
				</div>
			</div>			
			<br>
			<div class="roundborder">
				<div id="costSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="costSavingspan">Cost Saving Chart</span>
				<div id="costSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="costSavingChart"></div>
				</div>
			</div>			
			<br>
			<div class="roundborder">
				<div id="volumeSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="volumeSavingspan">Volume Saving Chart</span>
				<div id="volumeSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="volumeSavingChart"></div>
				</div>
			</div>			
			<br>
			<div class="roundborder">
				<div id="spreadsheet"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="finishedopportunities.js"></script>
<script type="text/javascript" src="finishedopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>