<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Login";
	$errorMessage = "";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
	<link rel="stylesheet" href="/index.css">
</head>

<body>
	<div style="min-height: 800px;">
		<span>In here we're going to have lots of lovely marketing things</span>
	</div>
	<br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>