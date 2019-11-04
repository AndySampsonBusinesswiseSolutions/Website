<?php 
	$PAGE_TITLE = "Opportunity";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<p>Tree showing all meters/sites</p>

	<p>
		When meter/site selected, show forecast spend and actual spend
	</p>
	<p>
		When meter/site selected, show forecast usage and actual usage
	</p>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>