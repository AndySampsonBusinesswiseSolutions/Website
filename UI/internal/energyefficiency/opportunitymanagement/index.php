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
	<div id="contactPopup" class="contact-popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="contactTitle"></span><br><br>
			</div>
			<br>
			<span id="contact"></span><br>
		</div>
	</div>
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
	<div id="manageOpportunityPopup" class="popup">
		<div class="modal-content-wide">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="manageOpportunityTitle"></span><br><br>
			</div>
			<br>
			<div>
				<div class="roundborder divcolumn" style="float: left; width: 26%;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span id="opportunityDetailsSpan">Opportunity Details</span>
						<div id="opportunityDetails" class="far fa-plus-square show-pointer"></div>
					</div>
					<br>
					<div id="opportunityDetailsList" class="listitem-hidden">
						<div>
							<span style="width: 50%; text-align: right;">Customer Name:</span><span id="customerNameSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Opportunity Type:</span><span id="opportunityTypeSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Opportunity Name:</span><span id="opportunityNameSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Site:</span><span id="siteSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Meter:</span><span id="meterSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Sub Meter:</span><span id="subMeterSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Estimated Start Date:</span><span id="estimatedStartDateSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Estimated Finish Date:</span><span id="estimatedFinishDateSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Percentage Saving:</span><span id="percentageSavingSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Estimated Cost:</span><span id="estimatedCAPEXCostSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Estimated kWh Savings (pa):</span><span id="estimatedUsageSavingsSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">Estimated £ Savings (pa):</span><span id="estimatedCostSavingsSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">ROI Total Months:</span><span id="roiTotalMonthsSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
							<span style="width: 50%; text-align: right;">ROI Remaining Months:</span><span id="roiRemainingMonthsSpan" class="manageOpportunityOpportunityDetailSpan" style="margin-left: 5px;"></span><br>
						</div>
					</div>
				</div>
				<div class="middle"></div>
				<div class="roundborder divcolumn" style="float: left; width: 44%;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span id="addNewTimelineStepSpan">Add New Timeline Step</span>
						<div id="addNewTimelineStep" class="far fa-plus-square show-pointer"></div>
					</div>
					<br>
					<div id="addNewTimelineStepList" class="listitem-hidden">
						<div class="roundborder" style="float: left; width: 36%;">
							<span id="addNewTimelineStepTreeSpan" style="padding-left: 5px;">Select Sites(s)/Meter(s)</span>
							<div id="addNewTimelineStepTree" class="far fa-plus-square show-pointer"></div>
							<div class="scrolling-wrapper">
								<ul class="format-listitem" id="addNewTimelineStepTreeList">
									<li>
										<div id="Site0" class="far fa-plus-square" style="padding-right: 4px;"></div>
										<input type="checkbox" id="Site0checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Site0span">Site X</span>
										<div id="Site0List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<div id="Meter14" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="checkbox" id="Meter14checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Meter14span">1234567890123</span>
													<div id="Meter14List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<div id="Site1" class="far fa-plus-square" style="padding-right: 4px;"></div>
										<input type="checkbox" id="Site1checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Site1span">Site Y</span>
										<div id="Site1List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<div id="Meter16" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="checkbox" id="Meter16checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Meter16span">1234567890124</span>
													<div id="Meter16List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<div id="Site2" class="far fa-plus-square" style="padding-right: 4px;"></div>
										<input type="checkbox" id="Site2checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Site2span">Site Z</span>
										<div id="Site2List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<div id="Meter18" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="checkbox" id="Meter18checkbox" branch="Meter" linkedsite="LED Lighting" guid="" ><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Meter18span">1234567890125</span>
													<div id="Meter18List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
												<li>
													<div id="Meter20" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="checkbox" id="Meter20checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Meter20span">1234567890126</span>
													<div id="Meter20List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<div id="Site3" class="far fa-plus-square" style="padding-right: 4px;"></div>
										<input type="checkbox" id="Site3checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Site3span">Site A</span>
										<div id="Site3List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<div id="Meter22" style="padding-right: 4px;" class="far fa-times-circle"></div>
													<input type="checkbox" id="Meter22checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="Meter22span">1234567890120</span>
													<div id="Meter22List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									</li>
								</ul>
								<br>
								<div>
									<input type="radio" name="group1" checked>Grouped</input><br>
									<input type="radio" name="group1">Individual</input>
								</div>
							</div>
						</div>
						<div class="middle"></div>
						<div class="roundborder" style="float: left; width: 62%;">
							<div style="text-align: center;">
								<span id="addNewTimelineStepDetailSpan">New Timeline Step Details</span>
							</div>
							<div style="margin: 2%; margin-right: 2%; margin-top: 2%;">
								<label for="stepName" style="width: 19%">Step Name:</label><input id="stepName" style="margin-left: 2%; width: 79%"></input>
							</div>
							<div class="roundborder" style="margin-left: 2%; margin-right: 2%; margin-top: 2%; text-align: center;">
								<div style="margin-top: 5px;">
									<span>Start After:<span>
									<select>
										<option value="">Select Step</option>
										<option value="PurchaseSubMeter">Purchase Sub Meter</option>
										<option value="PurchaseLEDLights">Purchase LED Lights</option>
										<option value="InstallSubMeter">Install Sub Meter</option>
										<option value="ReceiveBaseSubMeterReadings">Receive Base Sub Meter Readings</option>
										<option value="InstallLEDLights">Install LED Lights</option>
									</select>
								</div>
								<span>OR</span>
								<br>
								<span>Select Start Date:</span><input type="date"></input>
								<br>
								<span>*if both are set, the new step will start at the latter of the two dates</span>
							</div>
							<div class="roundborder" style="margin-left: 2%; margin-right: 2%; margin-top: 2%; text-align: center;">
								<div style="margin-top: 5px;">
									<label for="endDays">End After:</label><input id="endDays" type="number" min="1" style="width: 15%;"></input>
									<select>
										<option value="Days">Days</option>
										<option value="Working Days">Working Days</option>
									</select>
								</div>
								<span>OR</span>
								<br>
								<span>Select End Date:</span><input type="date"></input>
								<br>
								<span>*if both are set, the new step will end at the latter of the two dates</span>
							</div>
							<div style="margin-left: 2%; margin-right: 2%; margin-top: 2%;">
								<label for="startRecording">Start Recording Savings Once Step Has Finished?:</label><input class="show-pointer" id="startRecording" type="checkbox"></input>
							</div>
							<div style="margin: 2%;">
								<button class="show-pointer approve" style="width: 49%;">Add New Step</button>
								<button class="show-pointer reject" style="width: 49%; margin-left: 1%;">Reset</button>
							</div>
						</div>
					</div>
				</div>
				<div class="middle"></div>
				<div class="roundborder divcolumn" style="float: left; width: 26%;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span id="editTimelineStepSpan">Edit Timeline Step</span>
						<div id="editTimelineStep" class="far fa-plus-square show-pointer"></div>
					</div>
					<br>
					<div id="editTimelineStepList" class="listitem-hidden">
						<div style="margin: 2%; margin-right: 2%; margin-top: 2%;">
							<label for="editTimelineStepSitesMeters" style="width: 29%">Site(s)/Meter(s):</label><span id="editTimelineStepSitesMeters" style="margin-left: 2%; width: 69%">Site X - 1234567890123<br>Site Y - 1234567890124<br>Site Z - 1234567890125 & 1234567890126</span>
						</div>
						<div style="margin: 2%; margin-right: 2%; margin-top: 2%; text-align: right;">
							<label for="editTimelineStepStepName" style="width: 29%">Step Name:</label><input id="editTimelineStepStepName" style="margin-left: 2%; width: 69%" value="Purchase Sub Meter"></input>
						</div>
						<div style="margin-left: 2%; margin-right: 2%; margin-top: 2%; text-align: right;">
							<label for="editTimelineStepStartDate" style="width: 29%">Select Start Date:</label><input id="editTimelineStepStartDate" type="date" style="margin-left: 2%; width: 69%" value="2020-04-01"></input>
						</div>
						<div style="margin-left: 2%; margin-right: 2%; margin-top: 2%; text-align: right;">
							<label for="editTimelineStepEndDate" style="width: 29%">Select End Date:</label><input id="editTimelineStepEndDate" type="date" style="margin-left: 2%; width: 69%" value="2020-04-04"></input>
						</div>
						<div style="margin-left: 2%; margin-right: 2%; margin-top: 2%; text-align: right;">
							<label for="editTimelineStepStartRecording">Start Recording Savings Once Step Has Finished?:</label><input class="show-pointer" id="editTimelineStepStartRecording" type="checkbox"></input>
						</div>
						<div style="margin: 2%;">
							<button class="show-pointer approve" style="width: 49%;">Edit Step</button>
							<button class="show-pointer reject" style="width: 49%; margin-left: 1%;">Reset</button>
						</div>
					</div>
				</div>
			</div>
			<div style="clear: left;"></div>
			<br>
			<div id="spreadsheet" class="roundborder scrolling-wrapper tree-div">
				<div id="ganttChart"></div>
			</div>
			<br>
			<button class="show-pointer approve" style="float: right;" id="manageOpportunitySubmit" onclick="manageOpportunity();">Save Changes</button><br>
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

<script type="text/javascript" src="jquery-1.4.2.js"></script>
<script type="text/javascript" src="opportunitymanagement.js"></script>
<script type="text/javascript" src="opportunitymanagement.json"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>