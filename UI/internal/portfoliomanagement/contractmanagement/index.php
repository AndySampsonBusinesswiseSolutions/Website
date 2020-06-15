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
							<div id="siteTree" class="tree-div">
							</div>
							<button style="width: 100%;" onclick="displayUploadContract();">Upload Contract</button><br><br>
							<button style="width: 100%;" onclick='deleteContracts()'>Delete Selected Contracts</button><br><br>
							<button style="width: 100%;" onclick='reinstateContracts()'>Reinstate Deleted Contracts</button><br><br>
						</div>
						<div style="float: right; margin-left: 15px;">
							<div class="tree-div  scrolling-wrapper">
								<div class="expander-header">
									<span id="locationSelectorSpan">Location Visibility</span>
									<i id="locationSelector" class="far fa-plus-square expander openExpander show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
								</div>
								<div id="locationSelectorList" class="expander-container">
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
				<div class="pad-container">
					<div class="expander-header">
						<span>Out Of Contract Meters</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Out Of Contract Meters To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Out Of Contract Meters"></i>
						<i id="outOfContractMeters" class="far fa-plus-square expander-container-control openExpander show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="outOfContractMetersList" class="tree-div expander-container">
						<div id="outOfContract"></div>
					</div>
				</div>
				<div class="pad-container expander-container ">
					<div class="expander-header">
						<span>Active Contracts</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Active Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Active Contracts"></i>
						<i id="activeContracts" class="far fa-plus-square expander-container-control openExpander show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="activeContractsList" class="listitem-hidden tree-div expander-container">
						<div id="active"></div>
					</div>
				</div>
				<div class="pad-container expander-container ">
					<div class="expander-header">
						<span>Pending Contracts</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending Contracts"></i>
						<i id="pendingContracts" class="far fa-plus-square expander-container-control openExpander show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="pendingContractsList" class="listitem-hidden tree-div expander-container">
						<div id="pending"></div>
					</div>
				</div>
				<div class="pad-container expander-container ">
					<div class="expander-header">
						<span>Finished Contracts</span>						
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Finished Contracts To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Finished Contracts"></i>
						<i id="finishedContracts" class="far fa-plus-square expander-container-control openExpander show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;"></i>
					</div>
					<div id="finishedContractsList" class="tree-div expander-container">
						<div id="finished"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script type="text/javascript" src="contractmanagement.js"></script>
<script type="text/javascript" src="contractmanagement.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>