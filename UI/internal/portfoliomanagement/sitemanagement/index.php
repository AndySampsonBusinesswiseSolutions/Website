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
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header">
				<div id="mySidenav" class="sidenav" style="display: none;">
					<div class="header">
						<button class="closebtn" onclick="closeNav()">Close</button>
						<i class="fas fa-filter sidenav-icon-close"></i>
					</div>
					<div class="tree-column">
						<div style="float: left;">	
							<div id="siteTree" class="tree-div ">
							</div>
							<button style="width: 100%;" onclick="displayUploadUsage();">Upload Usage</button><br><br>
							<button style="width: 100%;" onclick='deleteLocations()'>Delete Selected Locations</button><br><br>
							<button style="width: 100%;" onclick='reinstateLocations()'>Reinstate Deleted Locations</button><br><br>
						</div>
						<div style="float: right; margin-left: 15px;">
							<div class="tree-div scrolling-wrapper">
								<div class="expander-header">
									<span id="locationSelectorSpan">Location Visibility</span><i class="far fa-question-circle show-pointer" title="Choose whether to display Sites and/or Meters in the 'Location' tree on the left-hand side"></i>
									<i id="locationSelector" class="far fa-plus-square expander-container-control openExpander show-pointer"></i>
								</div>
								<div id="locationSelectorList" class="expander-container">
									<div style="width: 45%; text-align: center; float: left;">
										<span>Show Sites</span>
										<label class="switch"><input type="checkbox" id="sitesLocationcheckbox" checked onclick='pageLoad();'></input><div class="switch-btn"></div></label>
									</div>
									<div style="width: 45%; text-align: center; float: right;">
										<span>Show Meters</span>
										<label class="switch"><input type="checkbox" id="metersLocationcheckbox" onclick='pageLoad();'></input><div class="switch-btn"></div></label>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="resetPage()">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="Items can be added to the Dashboard using the 'Filter' icon on the left-hand side"></i></div>
			</div>
			<div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
				<div class="pad-container outer-container">
					<div class="tabDiv" id="tabDiv"></div>
					<div id="cardDiv"></div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script type="text/javascript" src="sitemanagement.js"></script>
<script type="text/javascript" src="sitemanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>