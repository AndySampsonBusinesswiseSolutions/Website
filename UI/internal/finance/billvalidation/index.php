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
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="selectOptionsSpan">Select Options</span>
					<div id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></div>
				</div>
				<div id="selectOptionsList" class="expander-container">
					<div id="billTree" class="tree-div roundborder">
					</div>
				</div>
			</div>
			
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
			</div>
			<div class="final-column">
				<br>
				<div class="tabDiv" id="tabDiv"></div>
				<div id="cardDiv"></div>
			</div>
			<br>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script src="billvalidation.js"></script>
<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="billvalidation.json"></script>
<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>