<?php 
	$PAGE_TITLE = "Actuals Vs Budget";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<p>Tree of portfolio/site/meters
		Graph showing backward budget of cost against actual cost - break down by cost element if possible
		Graph showing backward budget of consumption against actual consumption
		Forward looking budget for same above
		Datagrid for each cost element (plus volume as first item) - excel spreadsheet from David
		Ability to view bill covering an actual period
	</p>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>