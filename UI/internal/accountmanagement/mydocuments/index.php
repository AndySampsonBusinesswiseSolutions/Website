<?php 
	$PAGE_TITLE = "My Documents";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="mydocuments.css">
</head>

<body>
	<div id="uploadDocumentPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="uploadDocumentTitle"></span>
			</div>
			<div id="drop-area">
				<input type="file" id="fileElem" multiple accept="*" onchange="handleFiles(this.files)"><br>
				<label class="button" for="fileElem">Select some files</label><br><br>
				<progress id="progress-bar" max=100 value=0 style="width: 100%;"></progress><br>
				<div id="gallery"></div>
			</div>
			<button style="float: right; margin-top: 5px;" class="show-pointer approve">Upload Document</button>
			<br>
		</div>
	</div>
	<div id="fileDetailsPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="fileDetailsTitle"></span>
			</div>
			<br>
			<span id="fileDetailsText" style="font-size: 15px;"></span><br><br>
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
					<div id="documentTree" class="tree-div roundborder">
					</div>
				</div>
			</div>
			<br>
			<button style="width: 100%;" onclick='uploadDocument()'>Upload Document</button>
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
				<div class="dashboard tree-div roundborder scrolling-wrapper outer-container">
					<div class="expander-header">
						<span>Your Current LOA Details</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="loaDetails"></i>
					</div>
					<div id="loaDetailsList">
						<span style="font-size: 20px; color: red;">Your current LOA is 5 days away from expiring!</span>
						<br><span>Here we can put any other info about the current LOA or anything we so desire</span>
					</div>
				</div>
				<br>
				<div class="dashboard tree-div roundborder scrolling-wrapper outer-container">
					<div class="expander-header">
						<span>Download Basket</span>
						<i class="far fa-plus-square show-pointer expander" id="downloadBasket"></i>
					</div>
					<div id="downloadBasketList" class="listitem-hidden" style="padding: 5px;">
						<table style="width: 100%;">
							<tr>
								<td class="table-cell">File Type</td>
								<td class="table-cell">File Name</td>
								<td class="table-cell">Date Added To Basket</td>
								<td class="table-cell">Actions <i class='fas fa-download show-pointer' title='Download All Files' style="padding-left: 15px; padding-right: 15px;"></i><i class='fas fa-trash-alt show-pointer' title='Delete All Files From Basket'></i></td>
							</tr>
							<tr>
								<td class="table-cell">Letter Of Authority</td>
								<td class="table-cell">Businesswise Solutions LOA 2019.pdf</td>
								<td class="table-cell">01/04/2020</td>
								<td class="table-cell"><i class='fas fa-download show-pointer' title='Download File' style="padding-right: 15px;"></i><i class='fas fa-trash-alt show-pointer' title='Delete File From Basket'></i></td>
							</tr>
							<tr>
								<td class="table-cell">View Meter Consumption - Usage Chart</td>
								<td class="table-cell">View Meter Consumption - Usage Chart 319df950-d903-4c24-9d7c-575998ed4c43.xlsx<i class='fas fa-search show-pointer' onclick="displayFileDetailsPopup(2);" title='View File Details' style="padding-left: 15px;"></i></td>
								<td class="table-cell">01/04/2020</td>
								<td class="table-cell"><i class='fas fa-download show-pointer' title='Download File' style="padding-right: 15px;"></i><i class='fas fa-trash-alt show-pointer' title='Delete File From Basket'></i></td>
							</tr>
						</table>
					</div>
				</div>
				<br>
				<div class="tabDiv" id="tabDiv"></div>
				<div id="cardDiv"></div>
			</div>
			<br>
		</div>
	</div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="mydocuments.js"></script>
<script type="text/javascript" src="mydocuments.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>