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
</head>

<body>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<div class="section-header-text">Welcome To The Businesswise Solutions Energy Portal</div>
			</div>
			<span>In here we're going to have lots of lovely marketing things</span>
		</div>
	</div>
	<br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>