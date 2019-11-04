<?php 
	$PAGE_TITLE = "Position Management";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>

	<p>
		Graph showing positions for Flex contract. If more than one flex contract, allow user to choose. If no flex contracts for customer, don't show page
	</p>
	<p>
		Data grid showing data from chart but at seasonal/monthly level
		Data grid showing information about all trades linked to flex contract chosen above
	</p>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>