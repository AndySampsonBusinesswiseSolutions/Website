<?php 
	$PAGE_TITLE = "Opportunity Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="opportunitymanagement.css">
</head>

<body>
	<div id="scheduleRequestedVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="scheduleRequestedVisitTitle"></span><br><br>
			</div>
			<br>
			<form action="" onsubmit="scheduleRequestedVisit(); return false;">
				<div>Requested Visit Date:</div><span id="scheduleRequestedVisitRequestedVisitDate"></span><br><br>
				<div class="requiredMessage roundborder" id="scheduleRequestedVisitScheduledVisitDateRequiredMessage"></div>
				<label for="scheduleRequestedVisitScheduledVisitDate">Scheduled Visit Date:</label><br>
				<input type="date" id="scheduleRequestedVisitScheduledVisitDate"></input>
				<br><br>
				<div class="requiredMessage roundborder" id="scheduleRequestedVisitAssignEngineerRequiredMessage"></div>
				<label for="scheduleRequestedVisitAssignEngineer">Assign Engineer:</label><br>
				<select id="scheduleRequestedVisitAssignEngineer">
					<option value="">Select Engineer</option>
					<option value="En Gineer">En Gineer</option>
				</select>
				<br><br>
				<label id="scheduleRequestedVisitNotesLabel" for="scheduleRequestedVisitNotes">Enter any notes for this visit:</label><br>
				<textarea id="scheduleRequestedVisitNotes" name="scheduleRequestedVisitNotes" class="roundborder"></textarea><br><br>
				<input type="submit" style="float: right;" id="scheduleRequestedVisitSubmit"></input>
			</form>
		</div>
	</div>
	<div id="rejectRequestedVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="rejectRequestedVisitTitle"></span><br><br>
			</div>
			<br>
			<label id="rejectRequestedVisitNotesLabel" for="rejectRequestedVisitNotes">Enter why this visit is being rejected:</label><br>
			<textarea id="rejectRequestedVisitNotes" name="rejectRequestedVisitNotes" class="roundborder"></textarea><br><br>
			<input type="submit" style="float: right;" id="rejectRequestedVisitSubmit" onclick="rejectRequestedVisit();"></input>
		</div>
	</div>
	<div id="approveScheduledVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span><br><br>
			<div class="title">
				<span id="approveScheduledVisitTitle"></span><br><br>
			</div>
			<br>
			<button style="float: right;" id="approveScheduledVisitSubmit" onclick="approveScheduledVisit();">Click to Start Visit</button><br>
		</div>
	</div>
	<div id="rejectScheduledVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="rejectScheduledVisitTitle"></span><br><br>
			</div>
			<br>
			<label id="rejectScheduledVisitNotesLabel" for="rejectScheduledVisitNotes">Enter why this visit is being rejected:</label><br>
			<textarea id="rejectScheduledVisitNotes" name="rejectScheduledVisitNotes" class="roundborder"></textarea><br><br>
			<input type="submit" style="float: right;" id="rejectScheduledVisitSubmit" onclick="rejectScheduledVisit();"></input>
		</div>
	</div>
	<div id="approveOpportunityPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="approveOpportunityTitle"></span><br><br>
			</div>
			<br>
			<div>Estimated Annual Savings:</div><span id="approveOpportunityEstimatedAnnualSavings"></span><br><br>
			<input type="submit" style="float: right;" id="approveOpportunitySubmit" onclick="approveOpportunity();"></input>
		</div>
	</div>
	<div id="rejectOpportunityPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="rejectOpportunityTitle"></span><br><br>
			</div>
			<br>
			<div>Estimated Annual Savings:</div><span id="rejectOpportunityEstimatedAnnualSavings"></span><br><br>
			<label id="rejectOpportunityNotesLabel" for="rejectOpportunityNotes">Enter why this opportunity is being rejected:</label><br>
			<textarea id="rejectOpportunityNotes" name="rejectOpportunityNotes" class="roundborder"></textarea><br><br>
			<input type="submit" style="float: right;" id="rejectOpportunitySubmit" onclick="rejectOpportunity();"></input>
		</div>
	</div>
	<div id="closeOpportunityPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="closeOpportunityTitle"></span><br><br>
			</div>
			<br>
			<div>Estimated Annual Savings:</div><span id="closeOpportunityEstimatedAnnualSavings"></span><br><br>
			<label id="closeOpportunityNotesLabel" for="closeOpportunityNotes">Enter why this opportunity is being closed:</label><br>
			<textarea id="closeOpportunityNotes" name="closeOpportunityNotes" class="roundborder"></textarea><br><br>
			<input type="submit" style="float: right;" id="closeOpportunitySubmit" onclick="closeOpportunity();"></input>
		</div>
	</div>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
			</div>
			<br>
			<div id="opportunityStatusDiv" class="tree-div roundborder">
				<span style="padding-left: 5px;">Select Opportunity Status <i class="far fa-plus-square" id="opportunityStatusSelector"></i></span>
				<ul class="format-listitem" id="opportunityStatusSelectorList">
					<li>
						<input type="checkbox" onclick="updateClassOnClick('rejectedOpportunitiesDiv', 'listitem-hidden', '');"><span style="padding-left: 1px;">Rejected</span>
					</li>
					<li>
						<input type="checkbox" checked onclick="updateClassOnClick('recommendedOpportunitiesDiv', 'listitem-hidden', '');"><span style="padding-left: 1px;">Recommended</span>
					</li>
					<li>
						<input type="checkbox" checked onclick="updateClassOnClick('pendingActiveOpportunitiesDiv', 'listitem-hidden', '');"><span style="padding-left: 1px;">Pending & Active</span>
					</li>
				</ul>
			</div>
		</div>
	</div>

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>
	<br>
	<div class="final-column">
		<div>
			<div class="roundborder divcolumn left">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Requested Visits</span>
					<div id="requestedVisits" class="far fa-plus-square show-pointer"></div>
				</div>
				<br>
				<div id="requestedVisitsList">
					<div id="requestedVisitsSpreadsheet"></div>
				</div>
			</div>
			<div class="middle"></div>
			<div class="roundborder divcolumn right">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Scheduled Visits</span>
					<div id="scheduledVisits" class="far fa-plus-square show-pointer"></div>
				</div>
				<br>
				<div id="scheduledVisitsList">
					<div id="scheduledVisitsSpreadsheet"></div>
				</div>
			</div>
		</div>
		<div style="clear: left;"></div>
		<br>
		<div id="rejectedOpportunitiesDiv" class="listitem-hidden">
			<div class="roundborder divcolumn">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Rejected Opportunities</span>
					<div id="rejectedOpportunities" class="far fa-plus-square show-pointer"></div>
				</div>
				<br>
				<div id="rejectedOpportunitiesList">
					<div id="rejectedOpportunitiesSpreadsheet"></div>
				</div>
			</div>
			<div style="clear: left;"></div>
			<br>
		</div>
		<div id="recommendedOpportunitiesDiv">
			<div class="roundborder divcolumn">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Recommended Opportunities</span>
					<div id="recommendedOpportunities" class="far fa-plus-square show-pointer"></div>
				</div>
				<br>
				<div id="recommendedOpportunitiesList">
					<div id="recommendedOpportunitiesSpreadsheet"></div>
				</div>
			</div>
			<div style="clear: left;"></div>
			<br>
		</div>
		<div id="pendingActiveOpportunitiesDiv">
			<div class="roundborder divcolumn">
				<div style="text-align: center; border-bottom: solid black 1px;">
					<span>Pending & Active Opportunities</span>
					<div id="pendingActiveOpportunities" class="far fa-plus-square show-pointer"></div>
				</div>
				<br>
				<div id="pendingActiveOpportunitiesList">
					<div id="pendingActiveOpportunitiesSpreadsheet"></div>
				</div>
			</div>
			<div style="clear: left;"></div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="opportunitymanagement.js"></script>
<script type="text/javascript" src="opportunitymanagement.json"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>