<?php 
	$PAGE_TITLE = "Manage Users";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<p>Lets users with specific roles manage additional users</p>

	<p>
		Ability to allow users to only view data for specific customer entities (can be all or customers who are child entities)
	</p>

	INTERNAL ability to assign commission payment structure
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>