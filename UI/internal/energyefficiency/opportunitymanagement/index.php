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
				<span id="contactTitle"></span>
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
				<span id="manageOpportunityTitle"></span>
				<br>
				<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Opportunity To Download Basket"></i>
				<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Opportunity"></i>
				<br>
			</div>
			<div style="margin-top: 5px;">
				<div class="roundborder divcolumn" style="float: left; width: 26%;">
					<div class="expander-header">
						<span id="opportunityDetailsSpan">Opportunity Details</span>
						<i id="opportunityDetails" class="far fa-plus-square show-pointer expander"></i>
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
					<div class="expander-header">
						<span id="addNewTimelineStepSpan">Add New Timeline Step</span>
						<i id="addNewTimelineStep" class="far fa-plus-square show-pointer expander"></i>
					</div>
					<br>
					<div id="addNewTimelineStepList" class="listitem-hidden">
						<div class="roundborder" style="float: left; width: 36%;">
							<span id="addNewTimelineStepTreeSpan" style="padding-left: 5px;">Select Sites(s)/Meter(s)</span>
							<i id="addNewTimelineStepTree" class="far fa-plus-square show-pointer expander openExpander"></i>
							<div class="scrolling-wrapper">
								<ul class="format-listitem" id="addNewTimelineStepTreeList">
									<li>
										<i id="ManageOpportunitySite0" class="far fa-plus-square show-pointer expander" style="padding-right: 4px;"></i>
										<input type="checkbox" id="ManageOpportunitySite0checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunitySite0span">Site X</span>
										<div id="ManageOpportunitySite0List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<i id="ManageOpportunityMeter14" style="padding-right: 4px;" class="far fa-times-circle"></i>
													<input type="checkbox" id="ManageOpportunityMeter14checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunityMeter14span">1234567890123</span>
													<div id="ManageOpportunityMeter14List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<i id="ManageOpportunitySite1" class="far fa-plus-square show-pointer expander" style="padding-right: 4px;"></i>
										<input type="checkbox" id="ManageOpportunitySite1checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunitySite1span">Site Y</span>
										<div id="ManageOpportunitySite1List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<i id="ManageOpportunityMeter16" style="padding-right: 4px;" class="far fa-times-circle"></i>
													<input type="checkbox" id="ManageOpportunityMeter16checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunityMeter16span">1234567890124</span>
													<div id="ManageOpportunityMeter16List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<i id="ManageOpportunitySite2" class="far fa-plus-square show-pointer expander" style="padding-right: 4px;"></i>
										<input type="checkbox" id="ManageOpportunitySite2checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunitySite2span">Site Z</span>
										<div id="ManageOpportunitySite2List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<i id="ManageOpportunityMeter18" style="padding-right: 4px;" class="far fa-times-circle"></i>
													<input type="checkbox" id="ManageOpportunityMeter18checkbox" branch="Meter" linkedsite="LED Lighting" guid="" ><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunityMeter18span">1234567890125</span>
													<div id="ManageOpportunityMeter18List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
												<li>
													<i id="ManageOpportunityMeter20" style="padding-right: 4px;" class="far fa-times-circle"></i>
													<input type="checkbox" id="ManageOpportunityMeter20checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunityMeter20span">1234567890126</span>
													<div id="ManageOpportunityMeter20List" class="listitem-hidden">
														<ul class="format-listitem"></ul>
													</div>
												</li>
											</ul>
										</div>
									</li>
									<li>
										<i id="ManageOpportunitySite3" class="far fa-plus-square show-pointer expander" style="padding-right: 4px;"></i>
										<input type="checkbox" id="ManageOpportunitySite3checkbox" branch="Site" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunitySite3span">Site A</span>
										<div id="ManageOpportunitySite3List" class="listitem-hidden">
											<ul class="format-listitem">
												<li>
													<i id="ManageOpportunityMeter22" style="padding-right: 4px;" class="far fa-times-circle"></i>
													<input type="checkbox" id="ManageOpportunityMeter22checkbox" branch="Meter" linkedsite="LED Lighting" guid=""><i class="undefined" style="padding-left: 3px; padding-right: 3px;"></i><span id="ManageOpportunityMeter22span">1234567890120</span>
													<div id="ManageOpportunityMeter22List" class="listitem-hidden">
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
					<div class="expander-header">
						<span id="editTimelineStepSpan">Edit Timeline Step</span>
						<i id="editTimelineStep" class="far fa-plus-square show-pointer expander"></i>
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
			<div id="spreadsheet" class="roundborder scrolling-wrapper tree-div" style="margin-top: 5px;">
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
		<div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
			<i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
			<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		</div>
		<div class="tree-column">
			<div id="siteTree" class="dashboard roundborder outer-container scrolling-wrapper">
			</div>
			<br>
			<div id="configureContainer" class="tree-div dashboard roundborder outer-container scrolling-wrapper">
				<div class="expander-header">
					<span id="configureOptionsSpan">Configure</span>
					<i id="configureOptions" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureOptionsList" class="slider-list expander-container">
					<div class="dashboard roundborder outer-container scrolling-wrapper">
						<div class="expander-header">
							<span id="configureLocationSelectorSpan">Location</span>
							<i id="configureLocationSelector" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="configureLocationSelectorList" class="expander-container">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Sites</span>
								<label class="switch"><input type="checkbox" id="siteLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Areas</span>
								<label class="switch"><input type="checkbox" id="areaLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>Commodities</span>
								<label class="switch"><input type="checkbox" id="commodityLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Meters</span>
								<label class="switch"><input type="checkbox" id="meterLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>SubAreas</span>
								<label class="switch"><input type="checkbox" id="subareaLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Assets</span>
								<label class="switch"><input type="checkbox" id="assetLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: left;">
								<span>SubMeters</span>
								<label class="switch"><input type="checkbox" id="submeterLocationcheckbox" checked onclick="updatePage()" branch="locationSelector"></input><div class="switch-btn"></div></label>
							</div>
						</div>
					</div>
					<br>
					<div class="tree-div dashboard roundborder outer-container">
						<div class="expander-header">
							<span id="opportunityStatusSpan">Opportunity Status</span>
							<i id="opportunityStatus" class="far fa-plus-square expander show-pointer openExpander"></i>
						</div>
						<div id="opportunityStatusList" class="expander-container">
							<ul class="format-listitem listItemWithoutPadding">
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
				<div class="divcolumn left dashboard">
					<div class="expander-header">
						<span>Requested Visits</span>
						<i id="requestedVisits" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Requested Visits To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Requested Visits"></i>
					</div>
					<br>
					<div id="requestedVisitsList" class="scrolling-wrapper" style="text-align: center;">
						<div id="requestedVisitsSpreadsheet"></div>
					</div>
				</div>
				<div class="middle"></div>
				<div class="divcolumn right dashboard">
					<div class="expander-header">
						<span>Scheduled Visits</span>
						<i id="scheduledVisits" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Scheduled Visits To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Scheduled Visits"></i>
					</div>
					<br>
					<div id="scheduledVisitsList" class="scrolling-wrapper" style="text-align: center;">
						<div id="scheduledVisitsSpreadsheet"></div>
					</div>
				</div>
				<div style="clear: left;"></div>
				<br>
				<div id="rejectedOpportunitiesDiv" class="listitem-hidden divcolumn dashboard">
					<div class="expander-header">
						<span>Rejected Opportunities</span>
						<i id="rejectedOpportunities" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Rejected Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Rejected Opportunities"></i>
					</div>
					<br>
					<div id="rejectedOpportunitiesList" class="scrolling-wrapper" style="text-align: center;">
						<div id="rejectedOpportunitiesSpreadsheet"></div>
					</div>
					<div style="clear: left;"></div>					
				</div>
				<br>
				<div id="recommendedOpportunitiesDiv" class="divcolumn dashboard">
					<div class="expander-header">
						<span>Recommended Opportunities</span>
						<i id="recommendedOpportunities" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Recommended Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Recommended Opportunities"></i>
					</div>
					<br>
					<div id="recommendedOpportunitiesList" class="scrolling-wrapper" style="text-align: center;">
						<div id="recommendedOpportunitiesSpreadsheet"></div>
					</div>
					<div style="clear: left;"></div>
				</div>
				<br>
				<div id="pendingActiveOpportunitiesDiv" class="divcolumn dashboard">
					<div class="expander-header">
						<span>Pending & Active Opportunities</span>
						<i id="pendingActiveOpportunities" class="far fa-plus-square show-pointer expander openExpander"></i>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Pending & Active Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Pending & Active Opportunities"></i>
					</div>
					<br>
					<div id="pendingActiveOpportunitiesList" class="scrolling-wrapper" style="text-align: center;">
						<div id="pendingActiveOpportunitiesSpreadsheet"></div>
					</div>
					<div style="clear: left;"></div>
				</div>
			</div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>
<script type="text/javascript" src="/includes/jquery/jquery.js"></script>
<script type="text/javascript" src="/includes/jquery/date.js"></script>
<script type="text/javascript" src="opportunitymanagement.js"></script>
<script type="text/javascript" src="opportunitymanagement.json"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>