<?php 
	$PAGE_TITLE = "Opportunities Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="opportunitiesdashboard.css">
</head>

<body>
	<div id="approveRejectOpportunityPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="approveRejectOpportunityTitle"></span><br><br>
				<span id="approveRejectOpportunitySiteTitle" style="display: none;"></span>
			</div>
			<br>
			<div>Estimated Annual Savings:</div><span id="approveRejectOpportunityEstimatedAnnualSavings"></span><br><br>
			<label id="approveRejectOpportunityNotesLabel" style="display: none;" for="approveRejectOpportunityNotes">Please let us know why this opportunity is being rejected:</label><br>
			<textarea id="approveRejectOpportunityNotes" name="approveRejectOpportunityNotes" style="display: none;"></textarea><br><br>
			<input type="submit" style="float: right;" id="approveRejectOpportunitySubmit" onclick="approveRejectOpportunity();"></input>
		</div>
	</div>
	<div id="requestVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span>Request Visit</span>
			</div>
			<br>
			<form action="" onsubmit="requestVisit(); return false;">
				<div class="requiredMessage" id="requestVisitSiteRequiredMessage"><i class="fas fa-exclamation-circle">Please select a site below</i></div>
				<div id="requestVisitSiteDiv" class="tree-div">
				</div>
				<br>
				<div class="requiredMessage" id="requestVisitVisitDateRequiredMessage"></div>
				<label for="requestVisitVisitDate">Select Visit Date:</label>
				<input type="date" id="requestVisitVisitDate" name="visitDate"></input>
				<br>
				<br>
				<label for="requestVisitNotes">Enter Notes:</label>
				<br>
				<textarea id="requestVisitNotes" name="requestVisitNotes" title="Enter any notes you would like associated with this visit:"></textarea>
				<br>
				<br>
				<input type="submit" style="float: right;" id="requestVisitSubmit"></input>
				<br>
			</form>
		</div>
	</div>
	<div id="futureSiteVisitPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="futureSiteVisitTitle"></span>
			</div>
			<br>
			<span>Assigned Engineer:</span>&nbsp<span id="futureSiteVisitAssignedEngineer"></span><br><br>
			<div>Site(s) To Visit:</div><span id="futureSiteVisitSiteList"></span><br><br>
			<div>Attached Notes:</div><span id=futureSiteVisitNotes></span>
		</div>
	</div>
	<div id="historicalSiteVisitPopup" class="popup">
		<div class="modal-content-wide">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="historicalSiteVisitTitle"></span>
			</div>
			<span>Assigned Engineer:</span>&nbsp<span id="historicalSiteVisitAssignedEngineer"></span><br><br>
			<div>Site(s) Visited:</div><span id="historicalSiteVisitSiteList"></span><br><br>
			<div>Recommended Opportunities:</div>
			<span id="historicalSiteVisitRecommendedOpportunitiesList">
				<div id="historicalSiteVisitRecommendedOpportunitiesSpreadsheet"></div>
			</span><br><br>
			<div>Pending & Active Opportunities:</div>
			<span id="historicalSiteVisitPendingActiveOpportunitiesList">
				<div id="historicalSiteVisitPendingActiveOpportunitiesSpreadsheet"></div>
			</span><br><br>
			<div>Finished Opportunities:</div>
			<span id="historicalSiteVisitFinishedOpportunitiesList">
				<div id="historicalSiteVisitFinishedOpportunitiesSpreadsheet"></div>
			</span>
			<br><br>
			<div>Attached Notes:</div>
			<span id=historicalSiteVisitNotes></span>
		</div>
	</div>
	<div id="siteRankingPopup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="siteRankingTitle"></span>
			</div>
			<br>
			<div>Opportunities:</div><span id="siteRankingOpportunityList"></span>
		</div>
	</div>
	<div id="outerContainer">
		<div id="mainContainer">
			<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
			<div class="final-column" style="text-align: center;">
				<div class="divcolumn pad-container">
					<div class="expander-header" style="margin-bottom: 9px;">
						<span>Opportunities Summary</span>
						<i id="opportunitiesSummary" class="far fa-plus-square expander-container-control openExpander show-pointer"></i>
					</div>
					<div id="opportunitiesSummaryList" class="expander-container">
						<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" target="_blank" class="dashboard-item-small" style="background-color: #3b7e84; margin-right: 9px;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span class="tooltip">Pending Opportunities<span class="tooltiptext">Opportunities that have been approved but have not yet started</span></span><br>
							<span style="font-size: 15px;">Count: 10</span><br>
							<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
							<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
						</a>
						<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" target="_blank" class="dashboard-item-small" style="background-color: #69566c; margin-right: 9px;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span class="tooltip">Active Opportunities<span class="tooltiptext">Opportunities that have been approved and have started</span></span><br>
							<span style="font-size: 15px;">Count: 2</span><br>
							<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
							<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
						</a>
						<a href="/Internal/EnergyEfficiency/FinishedOpportunities/" target="_blank" class="dashboard-item-small" style="background-color: #97a3af; margin-right: 9px;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span class="tooltip">Finished Opportunities<span class="tooltiptext">Opportunities that have been completed</span></span><br>
							<span style="font-size: 15px;">Count: 5</span><br>
							<span style="font-size: 11px;">Total kWh Savings: 10,000</span><br>
							<span style="font-size: 11px;">Total £ Savings: £10,000</span><br>
							<span style="font-size: 11px;">kWh Savings over past 12 months: 10,000</span><br>
							<span style="font-size: 11px;">£ Savings over past 12 months: £10,000</span>
						</a>
						<div class="dashboard-item-small" style="background-color: #333333;">
							<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
							<span>Sub Meters</span><br><br><br>
							<span style="font-size: 15px;">Installed: 1</span><br>
							<span style="font-size: 15px;">To Be Installed: 5</span>
						</div>
					</div>
				</div>
				<div class="divcolumn pad-container outer-container" style="overflow: auto">
					<div class="expander-header" style="margin-bottom: 9px;">
						<span>Recommended Opportunities</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Recommended Opportunities To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Recommended Opportunities"></i>
						<i id="recommendedOpportunities" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
					</div>
					<div id="recommendedOpportunitiesList" class="expander-container">
						<div class="divider-column"></div>
						<div class="opportunity-column">
							<span>Opportunity</span>
						</div>
						<div class="divider-column"></div>
						<div class="opportunity-column">
							<span>Approve/Reject</span>
						</div>
						<div class="divider-column"></div>
						<div class="opportunity-column">
							<span>Estimated Annual Savings</span>
						</div>
						<div class="divider-column"></div>
						<br>
						<div class="tree-div expander-container">
							<div class="divider-column"></div>
							<div class="opportunity-column" style="padding-top: 10px;">
								<i id="Project1" class="far fa-plus-square show-pointer expander"></i>
								<span id="Project1span">LED Lighting Installation</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" style="height: 36px;" id="Project1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" style="height: 36px;" id="Project1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject</button>
							</div>
							<div class="divider-column"></div>
							<div id="Project1EstimatedAnnualSavings" class="opportunity-column">
								<span>kWh Savings: 10,000</span>
								<br>
								<span>£ Savings: £10,000</span>
							</div>
							<div class="divider-column"></div>
							<br><br>
							<div id="Project1List" class="listitem-hidden">
								<div class="divider-column"></div>
								<div class="opportunity-column" style="padding-top: 10px;">
									<i id="Site1" class="far fa-times-circle"></i>
									<span id="Site1span">Site X</span>
								</div>
								<div class="divider-column"></div>
								<div class="opportunity-column">
									<button class="show-pointer approve" style="height: 36px;" id="Project1Site1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site X Only</button>
									&nbsp&nbsp&nbsp&nbsp
									<button class="show-pointer reject" style="height: 36px;" id="Project1Site1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site X Only</button>
								</div>
								<div class="divider-column"></div>
								<div id="Project1Site1EstimatedAnnualSavings" class="opportunity-column">
									<span>kWh Savings: 5,000</span>
									<br>
									<span>£ Savings: £5,000</span>
								</div>
								<div class="divider-column"></div>
								<br><br>
								<div class="divider-column"></div>
								<div class="opportunity-column" style="padding-top: 10px;">
									<i id="Site2" class="far fa-times-circle"></i>
									<span id="Site2span">Site Y</span>
								</div>
								<div class="divider-column"></div>
								<div class="opportunity-column">
									<button class="show-pointer approve" style="height: 36px;" id="Project1Site2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Y Only</button>
									&nbsp&nbsp&nbsp&nbsp
									<button class="show-pointer reject" style="height: 36px;" id="Project1Site2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Y Only</button>
								</div>
								<div class="divider-column"></div>
								<div id="Project1Site2EstimatedAnnualSavings" class="opportunity-column">
									<span>kWh Savings: 2,000</span>
									<br>
									<span>£ Savings: £2,000</span>
								</div>
								<div class="divider-column"></div>
								<br><br>
								<div class="divider-column"></div>
								<div class="opportunity-column" style="padding-top: 10px;">
									<i id="Site3" class="far fa-times-circle"></i>
									<span id="Site3span">Site Z</span>
								</div>
								<div class="divider-column"></div>
								<div class="opportunity-column">
									<button class="show-pointer approve" style="height: 36px;" id="Project1Site3ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Z Only</button>
									&nbsp&nbsp&nbsp&nbsp
									<button class="show-pointer reject" style="height: 36px;" id="Project1Site3RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Z Only</button>
								</div>
								<div class="divider-column"></div>
								<div id="Project1Site3EstimatedAnnualSavings" class="opportunity-column">
									<span>kWh Savings: 3,000</span>
									<br>
									<span>£ Savings: £3,000</span>
								</div>
								<div class="divider-column"></div>
								<br><br>
							</div>
						</div>
						<div class="tree-div expander-container">
							<div>
								<div class="divider-column"></div>
								<div class="opportunity-column" style="padding-top: 10px;">
									<i id="Project2" class="far fa-plus-square show-pointer expander"></i>
									<span id="Project2span">LED Lighting Installation 2</span>
								</div>
								<div class="divider-column"></div>
								<div class="opportunity-column">
									<button class="show-pointer approve" style="height: 36px;" id="Project2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve</button>
									&nbsp&nbsp&nbsp&nbsp
									<button class="show-pointer reject" style="height: 36px;" id="Project2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject</button>
								</div>
								<div class="divider-column"></div>
								<div id="Project2EstimatedAnnualSavings" class="opportunity-column">
									<span>kWh Savings: 10,000</span>
									<br>
									<span>£ Savings: £10,000</span>
								</div>
								<div class="divider-column"></div>
							</div>
							<br><br>
							<div id="Project2List" class="listitem-hidden">
								<div>
									<div class="divider-column"></div>
									<div class="opportunity-column" style="padding-top: 10px;">
										<i id="Site1" class="far fa-times-circle"></i>
										<span id="Site1span">Site X</span>
									</div>
									<div class="divider-column"></div>
									<div class="opportunity-column">
										<button class="show-pointer approve" style="height: 36px;" id="Project2Site1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site X Only</button>
										&nbsp&nbsp&nbsp&nbsp
										<button class="show-pointer reject" style="height: 36px;" id="Project2Site1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site X Only</button>
									</div>
									<div class="divider-column"></div>
									<div id="Project2Site1EstimatedAnnualSavings" class="opportunity-column">
										<span>kWh Savings: 5,000</span>
										<br>
										<span>£ Savings: £5,000</span>
									</div>
									<div class="divider-column"></div>
								</div>
								<br><br>
								<div>
									<div class="divider-column"></div>
									<div class="opportunity-column" style="padding-top: 10px;">
										<i id="Site2" class="far fa-times-circle"></i>
										<span id="Site2span">Site Y</span>
									</div>
									<div class="divider-column"></div>
									<div class="opportunity-column">
										<button class="show-pointer approve" style="height: 36px;" id="Project2Site2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Y Only</button>
										&nbsp&nbsp&nbsp&nbsp
										<button class="show-pointer reject" style="height: 36px;" id="Project2Site2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Y Only</button>
									</div>
									<div class="divider-column"></div>
									<div id="Project2Site2EstimatedAnnualSavings" class="opportunity-column">
										<span>kWh Savings: 2,000</span>
										<br>
										<span>£ Savings: £2,000</span>
									</div>
									<div class="divider-column"></div>
								</div>
								<br><br>
								<div>
									<div class="divider-column"></div>
									<div class="opportunity-column" style="padding-top: 10px;">
										<i id="Site3" class="far fa-times-circle"></i>
										<span id="Site3span">Site Z</span>
									</div>
									<div class="divider-column"></div>
									<div class="opportunity-column">
										<button class="show-pointer approve" style="height: 36px;" id="Project2Site3ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Z Only</button>
										&nbsp&nbsp&nbsp&nbsp
										<button class="show-pointer reject" style="height: 36px;" id="Project2Site3RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Z Only</button>
									</div>
									<div class="divider-column"></div>
									<div id="Project2Site3EstimatedAnnualSavings" class="opportunity-column">
										<span>kWh Savings: 3,000</span>
										<br>
										<span>£ Savings: £3,000</span>
									</div>
									<div class="divider-column"></div>
								</div>
								<br><br>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div style="clear: both;"></div>
			<div class="final-column">
				<div class="divcolumn pad-container outer-container">
					<div class="expander-header" style="margin-bottom: 9px;">
						<span>Site Visits</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Site Visits To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Site Visits"></i>
						<i id="siteVisits" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
					</div>
					<div id="siteVisitsList" class="expander-container">
						<div class="divcolumn">
							<div class="expander-header">
								<span>Future Site Visits</span>
								<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Future Site Visits To Download Basket"></i>
								<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Future Site Visits"></i>
								<i id="futureSiteVisits" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
							</div>
							<div id="futureSiteVisitsList" class="expander-container" style="text-align: center;">
								<div id="futureSiteVisitSpreadsheet"></div>
							</div>
						</div>
						<div class="divcolumn" style="float: right;">
							<div class="expander-header">
								<span>Historical Site Visits</span>
								<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Historical Site Visits To Download Basket"></i>
								<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Historical Site Visits"></i>
								<i id="historicalSiteVisits" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
							</div>
							<div id="historicalSiteVisitsList" class="expander-container" style="text-align: center;">
								<div id="historicalSiteVisitSpreadsheet"></div>
							</div>
							<div class="expander-container"></div>
						</div>
						<div style="clear: both;"></div>
						<button id="requestVisitButton" class="show-pointer" style="width: 25%; float: right;">Request Visit</button>
					</div>
				</div>
				<div class="divcolumn pad-container outer-container">
					<div class="expander-header" style="margin-bottom: 9px;">
						<span>Site Ranking</span>
						<i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Site Ranking To Download Basket"></i>
						<i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Site Ranking"></i>
						<i id="siteRanking" class="far fa-plus-square expander-container-control openExpander show-pointer" style="float: right;"></i>
					</div>
					<div id="siteRankingList" class="expander-container" style="text-align: center;">
						<div id="siteRankingSpreadsheet"></div>
					</div>
				</div>
			</div>
			<div style="clear: both;"></div>
		</div>
	</div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>

<script src="opportunitiesdashboard.json"></script>
<script src="opportunitiesdashboard.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript">
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>