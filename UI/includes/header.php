<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<?php 
		if(!session_id()) session_start();
	?>
	<link rel="stylesheet" href="/css/base.css">
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
	<script src="https://cdn.jsdelivr.net/npm/chart.js@2.9.2"></script>
</head>