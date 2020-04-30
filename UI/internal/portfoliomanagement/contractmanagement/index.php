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
		<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="selectOptionsSpan">Select Options</span>
					<div id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></div>
				</div>
				<div id="selectOptionsList" class="expander-container">
					<div id="siteTree" class="tree-div roundborder">
					</div>
				</div>
			</div>
			<br>
			<button style="width: 100%;" onclick="displayUploadContract();">Upload Contract</button>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<div class="final-column">
		<br>
		<div class="roundborder divcolumn">
            <div class="expander-header">
                <span>Out Of Contract Meters</span>
                <div id="outOfContractMeters" class="far fa-plus-square show-pointer expander openExpander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Out Of Contract Meters To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Out Of Contract Meters"></div>
            </div>
            <div id="outOfContractMetersList" class="expander-container">
                <div id="outOfContract"></div>
            </div>
        </div>
		<div class="roundborder divcolumn expander-container">
            <div class="expander-header">
                <span>Active Contracts</span>
                <div id="activeContracts" class="far fa-plus-square show-pointer expander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Active Contracts To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Active Contracts"></div>
            </div>
            <div id="activeContractsList" class="listitem-hidden expander-container">
                <div id="active"></div>
            </div>
        </div>
		<div class="roundborder divcolumn expander-container">
            <div class="expander-header">
                <span>Pending Contracts</span>
                <div id="pendingContracts" class="far fa-plus-square show-pointer expander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Pending Contracts To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Pending Contracts"></div>
            </div>
            <div id="pendingContractsList" class="listitem-hidden expander-container">
                <div id="pending"></div>
            </div>
        </div>
		<div class="roundborder divcolumn expander-container">
            <div class="expander-header">
                <span>Finished Contracts</span>
                <div id="finishedContracts" class="far fa-plus-square show-pointer expander openExpander"></div>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Finished Contracts To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Finished Contracts"></div>
            </div>
            <div id="finishedContractsList" class="expander-container">
                <div id="finished"></div>
            </div>
        </div>
	</div>
	<br>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="contractmanagement.js"></script>
<script type="text/javascript" src="contractmanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>