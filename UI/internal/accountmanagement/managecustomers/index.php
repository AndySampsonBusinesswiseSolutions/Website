<?php 
	$PAGE_TITLE = "Manage Customers";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="managecustomers.css">
</head>

<body>
	<div id="popup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="title"></span><br><br>
			</div>
			<br>
			<span id="text" style="font-size: 15px;"></span><br><br>
			<button style="float: right;" class="reject" id="button">Delete Attribute</button>
			<br>
		</div>
	</div>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<button style="margin-top: 5px; margin-bottom: 5px; width: 100%;" onclick='deleteCustomers()'>Delete Selected Customers</button>
			<button style="width: 100%;" onclick='reinstateCustomers()'>Reinstate Deleted Customers</button>
		</div>
	</div>

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
</body>

<script type="text/javascript" src="managecustomers.js"></script>
<script type="text/javascript" src="managecustomers.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>