<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<?php 
		if(!session_id()) session_start();
	?>
	<link rel="stylesheet" href="/css/base.css">
	<link rel="stylesheet" href="/css/icon.css">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.11.2/css/all.css">
  	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.11.2/css/v4-shims.css">
	<script src="https://cdn.jsdelivr.net/npm/chart.js@2.9.2"></script>
</head>