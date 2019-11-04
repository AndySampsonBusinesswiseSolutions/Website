<?php 
	$PAGE_TITLE = "Contract Management";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<p>
		Show datagrids of
		-> OOC meters - order by most costly
		-> Live contracts ordered by signed date
		-> Future contracts ordered by start date with estimated spend
		-> Past contracts ordered by end date showing original estimated spend against actual spend

		(Show contracts in datagrid grouped by contractID. Selecting a supplier will show all contracts under that supplier with details)
	</p>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>