<?php 
	$PAGE_TITLE = "Contract Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="contractmanagement.css">
</head>

<body>
	<div id="uploadContractPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="uploadContractTitle"></span>
			</div>
			<div id="drop-area">
				<input type="file" id="fileElem" multiple accept="*" onchange="handleFiles(this.files)"><br>
				<label class="button" for="fileElem">Select some files</label><br><br>
				<progress id="progress-bar" max=100 value=0 style="width: 100%;"></progress><br>
				<div id="gallery"></div>
			</div>
			<button style="float: right; margin-top: 5px;" class="show-pointer approve">Upload Contract</button>
			<br>
		</div>
	</div>
	<div id="ratePopup" class="popup">
		<div class="modal-content-wide">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="rateTitle"></span>
			</div>
			<br>
			<div id="rateText"></div>
		</div>
	</div>
	<div id="mySidenav" class="sidenav">
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteTree" class="sidebar-tree-div dashboard roundborder">
			</div>
			<br>
			<div class="tree-div dashboard roundborder scrolling-wrapper">
				<div class="sidebar-expander-header">
					<span id="configureSelectorSpan">Configure</span>
					<i id="configureSelector" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureSelectorList" class="expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder scrolling-wrapper">
						<div class="sidebar-expander-header">
							<span id="locationSelectorSpan">Location</span>
							<i id="locationSelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="locationSelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Show Sites</span>
								<label class="switch"><input type="checkbox" id="sitesLocationcheckbox" checked onclick='createTree(data, "siteTree", "filterContractsByStatus()");'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Show Meters</span>
								<label class="switch"><input type="checkbox" id="metersLocationcheckbox" checked onclick='createTree(data, "siteTree", "filterContractsByStatus()");'></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
				</div>
			</div>
			<br>
			<button style="width: 100%;" onclick="displayUploadContract();">Upload Contract</button>
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
				<div class="roundborder divcolumn dashboard">
					<div class="expander-header">
						<span>Out Of Contract Meters</span>
						<i id="outOfContractMeters" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Out Of Contract Meters To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Out Of Contract Meters"></i>
					</div>
					<div id="outOfContractMetersList" class="tree-div expander-container">
						<div id="outOfContract"></div>
					</div>
				</div>
				<div class="roundborder divcolumn expander-container dashboard">
					<div class="expander-header">
						<span>Active Contracts</span>
						<i id="activeContracts" class="far fa-plus-square show-pointer expander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Active Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Active Contracts"></i>
					</div>
					<div id="activeContractsList" class="listitem-hidden tree-div expander-container">
						<div id="active"></div>
					</div>
				</div>
				<div class="roundborder divcolumn expander-container dashboard">
					<div class="expander-header">
						<span>Pending Contracts</span>
						<i id="pendingContracts" class="far fa-plus-square show-pointer expander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending Contracts"></i>
					</div>
					<div id="pendingContractsList" class="listitem-hidden tree-div expander-container">
						<div id="pending"></div>
					</div>
				</div>
				<div class="roundborder divcolumn expander-container dashboard">
					<div class="expander-header">
						<span>Finished Contracts</span>
						<i id="finishedContracts" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Finished Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Finished Contracts"></i>
					</div>
					<div id="finishedContractsList" class="tree-div expander-container">
						<div id="finished"></div>
					</div>
				</div>
			</div>
			<br>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="contractmanagement.js"></script>
<script type="text/javascript" src="contractmanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>