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
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<button style="width: 100%; margin-top: 5px;" onclick="displayUploadContract();">Upload Contract</button>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>

	<div class="final-column">
		<br>
		<div class="roundborder divcolumn">
            <div style="text-align: center; border-bottom: solid black 1px;">
                <span>Out Of Contract Meters</span>
                <div id="outOfContractMeters" class="far fa-plus-square show-pointer"></div>
            </div>
            <div id="outOfContractMetersList" style="margin: 5px;">
                <div id="outOfContract"></div>
            </div>
        </div>
		<div class="roundborder divcolumn" style="margin-top: 5px;">
            <div style="text-align: center; border-bottom: solid black 1px;">
                <span>Active Contracts</span>
                <div id="activeContracts" class="far fa-plus-square show-pointer"></div>
            </div>
            <div id="activeContractsList" class="listitem-hidden" style="margin: 5px;">
                <div id="active"></div>
            </div>
        </div>
		<div class="roundborder divcolumn" style="margin-top: 5px;">
            <div style="text-align: center; border-bottom: solid black 1px;">
                <span>Pending Contracts</span>
                <div id="pendingContracts" class="far fa-plus-square show-pointer"></div>
            </div>
            <div id="pendingContractsList" class="listitem-hidden" style="margin: 5px;">
                <div id="pending"></div>
            </div>
        </div>
		<div class="roundborder divcolumn" style="margin-top: 5px;">
            <div style="text-align: center; border-bottom: solid black 1px;">
                <span>Finished Contracts</span>
                <div id="finishedContracts" class="far fa-plus-square show-pointer"></div>
            </div>
            <div id="fnishedContractsList" style="margin: 5px;">
                <div id="finished"></div>
            </div>
        </div>
	</div>
	<br>
</body>

<script type="text/javascript" src="contractmanagement.js"></script>
<script type="text/javascript" src="contractmanagement.json"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDEzl4cfd2OyotR5jHTowAoxwRzOyX8jws"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>