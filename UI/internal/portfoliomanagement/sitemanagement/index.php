<?php 
	$PAGE_TITLE = "Site Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="sitemanagement.css">
</head>

<body>
	<div id="deleteRowPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="deleteRowTitle"></span><br><br>
			</div>
			<br>
			<span id="deleteRowText" style="font-size: 15px;"></span><br><br>
			<button style="float: right;" class="reject">Delete Attribute</button>
			<br>
		</div>
	</div>
	<div id="uploadUsagePopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="uploadUsageTitle"></span>
			</div>
			<div id="drop-area">
				<!-- <p>Upload multiple files with the file dialog or by dragging and dropping images onto the dashed region</p> -->
				<input type="file" id="fileElem" multiple accept="*" onchange="handleFiles(this.files)"><br>
				<label class="button" for="fileElem">Select some files</label><br><br>
				<progress id="progress-bar" max=100 value=0 style="width: 100%;"></progress><br>
				<div id="gallery"></div>
			</div>
			<button style="float: right; margin-top: 5px;" class="show-pointer approve">Upload Usage</button>
			<br>
		</div>
	</div>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<button style="width: 100%; margin-top: 5px;" onclick="displayUploadUsage();">Upload Usage</button>
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

<script type="text/javascript" src="sitemanagement.js"></script>
<script type="text/javascript" src="sitemanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>