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
			<div style="text-align: center;">
				<img src="/images/EMaaS.PNG" style="width: 100%; height: 72.7%;">
			</div>
		</div>
	</div>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>