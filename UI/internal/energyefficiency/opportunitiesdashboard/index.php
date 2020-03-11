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
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Recommended Opportunities</span>
				<br>
				<div class="divcolumn first"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
								<span style="border-bottom: solid black 1px;">Opportunity</span>
							</div>
						</li>
						<br>
						<li>
							<div id="Project1" class="far fa-plus-square" additionallists="Project1ListButtons,Project1ListSavings"></div>
							<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
							<span id="Project1span">LED Lighting Installation</span><i class="fas fa-search show-pointer"></i>
							<div id="Project1List" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<div id="Site1" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site1span">Site X</span>
									</li>
									<li>
										<div id="Site2" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site2span">Site Y</span>
									</li>
									<li>
										<div id="Site3" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site2span">Site Z</span>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
							<span style="border-bottom: solid black 1px;">Approve/Reject</span>
							</div>
						</li>
						<br>
						<li>
							<button class="show-pointer"style="width: 45%; background-color: green;">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer"style="width: 45%; background-color: red;">Reject Opportunity</button>
							<div id="Project1ListButtons" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site X</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site X</button>
									</li>
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site Y</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site Y</button>
									</li>
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site Z</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site Z</button>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
							<span style="border-bottom: solid black 1px;">Estimated Annual Savings</span>
							</div>
						</li>
						<br>
						<li>
							<span>kWh Savings: 10,000</span>
							<br>
							<span>£ Savings: £10,000</span>
							<div id="Project1ListSavings" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<span>kWh Savings: 5,000</span>
										<br>
										<span>£ Savings: £5,000</span>
									</li>
									<li>
										<span>kWh Savings: 3,000</span>
										<br>
										<span>£ Savings: £3,000</span>
									</li>
									<li>
										<span>kWh Savings: 2,000</span>
										<br>
										<span>£ Savings: £2,000</span>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn last"></div>
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
					<button class="show-pointer" style="width: 100%;">Arrange Visit</button>
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

<script src="opportunitiesdashboard.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript">
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>