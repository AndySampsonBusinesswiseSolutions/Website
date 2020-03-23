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
	<div id="requestVisitPopup" class="popup">
		<div class="modal-content">
			<span>Request Visit</span><span class="close" title="Close">&times;</span><br><br>
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
				<textarea id="requestVisitNotes" name="requestVisitNotes" class="roundborder" title="Enter any notes you would like associated with this visit"></textarea>
				<br>
				<br>
				<input type="submit" style="float: right;" id="requestVisitSubmit"></input>
				<br>
			</form>
		</div>
	</div>
	<div id="futureSiteVisitPopup" class="popup">
		<div class="modal-content">
			<span class="title" id="futureSiteVisitTitle"></span><span class="close" title="Close">&times;</span><br><br>
			<span>Assigned Engineer:</span>&nbsp<span id="futureSiteVisitAssignedEngineer"></span><br><br>
			<div>Site(s) To Visit:</div><span id="futureSiteVisitSiteList"></span><br><br>
			<div>Attached Notes:</div><span id=futureSiteVisitNotes></span>
		</div>
	</div>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="roundborder divcolumn left" style="background-color: #e9eaee; text-align: center; overflow: auto">
			<div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Pending Opportunities</span><br>
					<span style="font-size: 15px;">Count: 10</span><br>
					<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
					<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Active Opportunities</span><br>
					<span style="font-size: 15px;">Count: 2</span><br>
					<span style="font-size: 15px;">Estimated kWh<br>Savings (pa): 10,000</span><br>
					<span style="font-size: 15px;">Estimated £<br>Savings (pa): £10,000</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Finished Opportunities</span><br>
					<span style="font-size: 15px;">Count: 5</span><br>
					<span style="font-size: 11px;">Total kWh Savings: 10,000</span><br>
					<span style="font-size: 11px;">Total £ Savings: £10,000</span><br>
					<span style="font-size: 11px;">kWh Savings over past 12 months: 10,000</span><br>
					<span style="font-size: 11px;">£ Savings over past 12 months: £10,000</span>
				</div>
				<div class="roundborder dashboard-item-small">
					<i class="fas fa-tools fa-4x" style="margin-top: 2px;"></i><br>
					<span>Sub Meters</span><br><br><br>
					<span style="font-size: 15px;">Installed: 1</span><br>
					<span style="font-size: 15px;">To Be Installed: 5</span>
				</div>
			</div>
		</div>
		<div class="divcolumn middle"></div>
		<div class="roundborder divcolumn right" style="background-color: #e9eaee;">
			<div style="text-align: center;">
				<div style="border-bottom: solid black 1px;">
					<span>Recommended Opportunities</span>
				</div>
				<br>
				<div>
					<div class="first" style="float: left;"></div>
					<div style="width: 30%; float: left;">
						<span style="border-bottom: solid black 1px;">Opportunity</span>
					</div>
					<div class="middle" style="float: left;"></div>
					<div style="width: 30%; float: left;">
						<span style="border-bottom: solid black 1px;">Approve/Reject</span>
					</div>
					<div class="middle" style="float: left;"></div>
					<div style="width: 30%; float: left;">
						<span style="border-bottom: solid black 1px;">Estimated Annual Savings</span>
					</div>
					<div class="last" style="float: left;"></div>
				</div>
				<br><br>
				<div class="roundborder">
					<div>
						<div class="first" style="float: left;"></div>
						<div style="width: 30%; float: left; padding-top: 10px;">
							<div id="Project1" class="far fa-plus-square show-pointer"></div>
							<span id="Project1span">LED Lighting Installation</span><i class="fas fa-search show-pointer"></i>
						</div>
						<div class="middle" style="float: left;"></div>
						<div style="width: 30%; float: left;">
							<button class="show-pointer"style="width: 45%; background-color: green;">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer"style="width: 45%; background-color: red;">Reject Opportunity</button>
						</div>
						<div class="middle" style="float: left;"></div>
						<div style="width: 30%; float: left;">
							<span>kWh Savings: 10,000</span>
							<br>
							<span>£ Savings: £10,000</span>
						</div>
						<div class="last" style="float: left;"></div>
					</div>
					<br><br>
					<div id="Project1List" class="listitem-hidden">
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site1" class="far fa-times-circle"></div>
								<span id="Site1span">Site X</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site X Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site X Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 5,000</span>
								<br>
								<span>£ Savings: £5,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site2" class="far fa-times-circle"></div>
								<span id="Site2span">Site Y</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site Y Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site Y Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 2,000</span>
								<br>
								<span>£ Savings: £2,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site3" class="far fa-times-circle"></div>
								<span id="Site3span">Site Z</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site Z Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site Z Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 3,000</span>
								<br>
								<span>£ Savings: £3,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
					</div>
				</div>
				<br>
				<div class="roundborder">
					<div>
						<div class="first" style="float: left;"></div>
						<div style="width: 30%; float: left; padding-top: 10px;">
							<div id="Project2" class="far fa-plus-square show-pointer"></div>
							<span id="Project2span">LED Lighting Installation 2</span><i class="fas fa-search show-pointer"></i>
						</div>
						<div class="middle" style="float: left;"></div>
						<div style="width: 30%; float: left;">
							<button class="show-pointer"style="width: 45%; background-color: green;">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer"style="width: 45%; background-color: red;">Reject Opportunity</button>
						</div>
						<div class="middle" style="float: left;"></div>
						<div style="width: 30%; float: left;">
							<span>kWh Savings: 10,000</span>
							<br>
							<span>£ Savings: £10,000</span>
						</div>
						<div class="last" style="float: left;"></div>
					</div>
					<br><br>
					<div id="Project2List" class="listitem-hidden">
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site1" class="far fa-times-circle"></div>
								<span id="Site1span">Site X</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site X Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site X Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 5,000</span>
								<br>
								<span>£ Savings: £5,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site2" class="far fa-times-circle"></div>
								<span id="Site2span">Site Y</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site Y Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site Y Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 2,000</span>
								<br>
								<span>£ Savings: £2,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
						<div>
							<div class="first" style="float: left;"></div>
							<div style="width: 30%; float: left; padding-top: 10px;">
								<div id="Site3" class="far fa-times-circle"></div>
								<span id="Site3span">Site Z</span>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<button class="show-pointer"style="width: 45%; background-color: green;">Approve<br>Site Z Only</button>
								&nbsp&nbsp&nbsp&nbsp
								<button class="show-pointer"style="width: 45%; background-color: red;">Reject<br>Site Z Only</button>
							</div>
							<div class="middle" style="float: left;"></div>
							<div style="width: 30%; float: left;">
								<span>kWh Savings: 3,000</span>
								<br>
								<span>£ Savings: £3,000</span>
							</div>
							<div class="last" style="float: left;"></div>
						</div>
						<br><br>
					</div>
				</div>
			</div>
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="roundborder divcolumn left" style="background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Visits</span>
			</div>
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div style="text-align: center;">
						<span style="border-bottom: solid black 1px;">Future Site Visits</span>
					</div>
					<br>
					<div id="futureSiteVisitSpreadsheet"></div>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn right">
					<div style="text-align: center;">
						<span style="border-bottom: solid black 1px;">Historical Site Visits</span>
					</div>
					<br>
					<div id="historicalSiteVisitSpreadsheet"></div>
				</div>
				<div class="divcolumn last"></div>
			</div>
			<div>
				<div class="first"></div>
				<div>
					<button id="requestVisitButton" class="show-pointer" style="width: 100%;">Request Visit</button>
				</div>
				<div class="last"></div>
			</div>
		</div>
		<div class="divcolumn middle"></div>
		<div class="roundborder divcolumn right" style="background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Ranking</span>
			</div>
			<br>
			<div id="siteRankingSpreadsheet"></div>		
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
</body>

<script src="opportunitiesdashboard.json"></script>
<script src="opportunitiesdashboard.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript">
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>