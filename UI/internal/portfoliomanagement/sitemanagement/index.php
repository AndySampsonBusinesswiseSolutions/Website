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
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteTree" class="tree-div dashboard roundborder">
			</div>
			<br>
			<button style="width: 100%;" onclick="displayUploadUsage();">Upload Usage</button>
			<br><br>
			<div class="tree-div dashboard roundborder scrolling-wrapper">
				<div class="expander-header">
					<span id="configureSelectorSpan">Configure</span>
					<i id="configureSelector" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureSelectorList" class="expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder scrolling-wrapper">
						<div class="expander-header">
							<span id="locationSelectorSpan">Location</span>
							<i id="locationSelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="locationSelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Sites</span>
								<label class="switch"><input type="checkbox" id="sitesLocationcheckbox" checked onclick='createTree("createCardButton");'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Show Meters</span>
								<label class="switch"><input type="checkbox" id="metersLocationcheckbox" checked onclick='createTree("createCardButton");'></input><div class="switch-btn"></div></label>
							</div>
						</div>
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
				<div class="outer-container">
					<div class="tabDiv" id="tabDiv"></div>
					<div id="cardDiv"></div>
				</div>
			</div>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="sitemanagement.js"></script>
<script type="text/javascript" src="sitemanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>