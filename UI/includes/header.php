<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<?php 
		if(!session_id()) session_start();
		include($_SERVER['DOCUMENT_ROOT']."/includes/css.php");
	?>
	<SCRIPT src="https://cdn.jsdelivr.net/npm/chart.js@2.8.0"></SCRIPT>
</head>