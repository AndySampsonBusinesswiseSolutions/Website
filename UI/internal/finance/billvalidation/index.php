<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Invoice Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="billvalidation.css">
</head>

<body>

	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<div id="mySidenav" class="sidenav" style="display: none;">
					<div class="header">
						<button class="closebtn" onclick="closeNav()">Close</button>
						<i class="fas fa-filter sidenav-icon-close"></i>
					</div>
					<div class="tree-column">
						<div id="billTree" class="tree-div ">
						</div>
					</div>
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="resetTree()">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="DO STUFF WITH BILLS"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
				<div class="outer-container pad-container">
					<div class="tabDiv" id="tabDiv"></div>
					<div id="cardDiv"></div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script src="billvalidation.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="billvalidation.json"></script>
<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>