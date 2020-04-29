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
			<textarea id="approveRejectOpportunityNotes" name="approveRejectOpportunityNotes" class="roundborder" style="display: none;"></textarea><br><br>
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
				<div class="requiredMessage roundborder" id="requestVisitSiteRequiredMessage"><i class="fas fa-exclamation-circle">Please select a site below</i></div>
				<div id="requestVisitSiteDiv" class="tree-div roundborder">
				</div>
				<br>
				<div class="requiredMessage roundborder" id="requestVisitVisitDateRequiredMessage"></div>
				<label for="requestVisitVisitDate">Select Visit Date:</label>
				<input type="date" id="requestVisitVisitDate" name="visitDate"></input>
				<br>
				<br>
				<label for="requestVisitNotes">Enter Notes:</label>
				<br>
				<textarea id="requestVisitNotes" name="requestVisitNotes" class="roundborder" title="Enter any notes you would like associated with this visit:"></textarea>
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
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="final-column" style="text-align: center;">
		<div class="roundborder divcolumn">
			<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" target="_blank"   class="roundborder dashboard-item-small">
				<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
				<span>Pending Opportunities</span><br>
				<span style="font-size: 15px;">Count: 10</span><br>
				<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
				<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
			</a>
			<a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/" target="_blank"  class="roundborder dashboard-item-small">
				<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
				<span>Active Opportunities</span><br>
				<span style="font-size: 15px;">Count: 2</span><br>
				<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
				<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
			</a>
			<a href="/Internal/EnergyEfficiency/FinishedOpportunities/" target="_blank" class="roundborder dashboard-item-small">
				<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
				<span>Finished Opportunities</span><br>
				<span style="font-size: 15px;">Count: 5</span><br>
				<span style="font-size: 11px;">Total kWh Savings: 10,000</span><br>
				<span style="font-size: 11px;">Total £ Savings: £10,000</span><br>
				<span style="font-size: 11px;">kWh Savings over past 12 months: 10,000</span><br>
				<span style="font-size: 11px;">£ Savings over past 12 months: £10,000</span>
			</a>
			<div class="roundborder dashboard-item-small">
				<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
				<span>Sub Meters</span><br><br><br>
				<span style="font-size: 15px;">Installed: 1</span><br>
				<span style="font-size: 15px;">To Be Installed: 5</span>
			</div>
		</div>
		<div class="divider-column"></div>
		<div class="roundborder divcolumn" style="overflow: auto">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Recommended Opportunities</span>
				<i class="far fa-plus-square show-pointer expander openExpander" id="recommendedOpportunities"></i>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Recommended Opportunities To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Recommended Opportunities"></div>
			</div>
			<div id="recommendedOpportunitiesList" style="margin-top: 5px;">
				<div>
					<div class="divider-column"></div>
					<div class="opportunity-column">
						<span style="border-bottom: solid black 1px;">Opportunity</span>
					</div>
					<div class="divider-column"></div>
					<div class="opportunity-column">
						<span style="border-bottom: solid black 1px;">Approve/Reject</span>
					</div>
					<div class="divider-column"></div>
					<div class="opportunity-column">
						<span style="border-bottom: solid black 1px;">Estimated Annual Savings</span>
					</div>
					<div class="divider-column"></div>
				</div>
				<br>
				<div class="roundborder" style="margin-top: 5px;">
					<div>
						<div class="divider-column"></div>
						<div class="opportunity-column" style="padding-top: 10px;">
							<div id="Project1" class="far fa-plus-square show-pointer expander"></div>
							<span id="Project1span">LED Lighting Installation</span>
						</div>
						<div class="divider-column"></div>
						<div class="opportunity-column">
							<button class="show-pointer approve" id="Project1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer reject" id="Project1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject Opportunity</button>
						</div>
						<div class="divider-column"></div>
						<div id="Project1EstimatedAnnualSavings" class="opportunity-column">
							<span>kWh Savings: 10,000</span>
							<br>
							<span>£ Savings: £10,000</span>
						</div>
						<div class="divider-column"></div>
					</div>
					<br><br>
					<div id="Project1List" class="listitem-hidden">
						<div>
							<div class="divider-column"></div>
							<div class="opportunity-column" style="padding-top: 10px;">
								<div id="Site1" class="far fa-times-circle"></div>
								<span id="Site1span">Site X</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project1Site1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site X Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project1Site1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site X Only</button>
							</div>
							<div class="divider-column"></div>
							<div id="Project1Site1EstimatedAnnualSavings" class="opportunity-column">
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
								<div id="Site2" class="far fa-times-circle"></div>
								<span id="Site2span">Site Y</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project1Site2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Y Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project1Site2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Y Only</button>
							</div>
							<div class="divider-column"></div>
							<div id="Project1Site2EstimatedAnnualSavings" class="opportunity-column">
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
								<div id="Site3" class="far fa-times-circle"></div>
								<span id="Site3span">Site Z</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project1Site3ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Z Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project1Site3RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Z Only</button>
							</div>
							<div class="divider-column"></div>
							<div id="Project1Site3EstimatedAnnualSavings" class="opportunity-column">
								<span>kWh Savings: 3,000</span>
								<br>
								<span>£ Savings: £3,000</span>
							</div>
							<div class="divider-column"></div>
						</div>
						<br><br>
					</div>
				</div>
				<br>
				<div class="roundborder">
					<div>
						<div class="divider-column"></div>
						<div class="opportunity-column" style="padding-top: 10px;">
							<div id="Project2" class="far fa-plus-square show-pointer expander"></div>
							<span id="Project2span">LED Lighting Installation 2</span>
						</div>
						<div class="divider-column"></div>
						<div class="opportunity-column">
							<button class="show-pointer approve" id="Project2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer reject" id="Project2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject Opportunity</button>
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
								<div id="Site1" class="far fa-times-circle"></div>
								<span id="Site1span">Site X</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project2Site1ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site X Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project2Site1RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site X Only</button>
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
								<div id="Site2" class="far fa-times-circle"></div>
								<span id="Site2span">Site Y</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project2Site2ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Y Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project2Site2RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Y Only</button>
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
								<div id="Site3" class="far fa-times-circle"></div>
								<span id="Site3span">Site Z</span>
							</div>
							<div class="divider-column"></div>
							<div class="opportunity-column">
								<button class="show-pointer approve" id="Project2Site3ApproveOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Approve<br>Site Z Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer reject" id="Project2Site3RejectOpportunityButton" onclick="openApproveRejectOpportunityPopup(this);">Reject<br>Site Z Only</button>
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
	<br>
	<div class="final-column">
		<div class="roundborder divcolumn">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Visits</span>
				<i class="far fa-plus-square show-pointer expander openExpander" id="siteVisits"></i>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Site Visits To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Site Visits"></div>
			</div>
			<div id="siteVisitsList" style="margin-top: 5px;">
				<div class="divcolumn" style="float: left;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Future Site Visits</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="futureSiteVisits"></i>
						<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Future Site Visits To Download Basket"></div>
						<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Future Site Visits"></div>
					</div>
					<div id="futureSiteVisitsList" style="margin-top: 5px;">
						<div id="futureSiteVisitSpreadsheet"></div>
					</div>
				</div>
				<div class="divider-column"></div>
				<div class="divcolumn" style="float: left;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Historical Site Visits</span>
						<i class="far fa-plus-square show-pointer expander openExpander" id="historicalSiteVisits"></i>
						<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Historical Site Visits To Download Basket"></div>
						<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Historical Site Visits"></div>
					</div>
					<div id="historicalSiteVisitsList" style="margin-top: 5px;">
						<div id="historicalSiteVisitSpreadsheet"></div>
					</div>
					<div style="margin-top: 5px;"></div>
					<button id="requestVisitButton" class="show-pointer" style="width: 100%; float: right;">Request Visit</button>
				</div>
			</div>
		</div>
		<div class="divider-column"></div>
		<div class="roundborder divcolumn">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Ranking</span>
				<i class="far fa-plus-square show-pointer expander openExpander" id="siteRanking"></i>
				<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Site Ranking To Download Basket"></div>
				<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Site Ranking"></div>
			</div>
			<div id="siteRankingList" style="margin-top: 5px;">
				<div id="siteRankingSpreadsheet"></div>
			</div>
		</div>
	</div>
	<div style="clear: both;"></div>
	<br>
</body>

<script src="/includes/base.js"></script>

<script src="opportunitiesdashboard.json"></script>
<script src="opportunitiesdashboard.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript">
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>