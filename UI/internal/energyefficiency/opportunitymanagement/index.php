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
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="tree-column">
			<div>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="roundborder divcolumn left" style="background-color: #e9eaee;">
					<div class="divcolumn first"></div>
					<div class="divcolumn" style="width: 96%;">
						<div style="text-align: center; border-bottom: solid black 1px;">
							<span>Requested Visits</span>
						</div>
						<br>
						<div id="tableContainer" class="tableContainer4">
							<table style="width: 100%;">
								<thead>
									<tr>
										<th id="th1" style="border: solid black 1px;">Customer</th>
										<th id="th2" style="border: solid black 1px;">Site</th>
										<th id="th5" style="border: solid black 1px;">Requested Date</th>
										<th id="th10" style="border: solid black 1px;" colspan="2">Manage Visit</th>
									</tr>
								</thead>
								<tbody class="scrollContent3">
									<tr>
										<td headers="th1" style="border: solid black 1px;">David Ford Trading Ltd</td>
										<td headers="th2" style="border: solid black 1px;">Site X</td>
										<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
										<td headers="th10" style="border: solid black 1px;"><button class="show-pointer" onclick="scheduleVisit(1)">Schedule Visit</button></td>
										<td headers="th11" style="border: solid black 1px;"><button class="show-pointer" onclick="rejectVisit(1)">Reject Visit</button></td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					<div class="divcolumn last"></div>
				</div>
				<div class="divcolumn middle"></div>
				<div class="roundborder divcolumn right" style="background-color: #e9eaee;">
					<div class="divcolumn first"></div>
					<div class="divcolumn" style="width: 96%;">
						<div style="text-align: center; border-bottom: solid black 1px;">
							<span>Scheduled Visits</span>
						</div>
						<br>
						<div id="tableContainer" class="tableContainer4">
							<table style="width: 100%;">
								<thead>
									<tr>
										<th id="th0" style="border: solid black 1px;">Customer</th>
										<th id="th2" style="border: solid black 1px;">Site</th>
										<th id="th3" style="border: solid black 1px;">Meter</th>
										<th id="th4" style="border: solid black 1px;">Engineer</th>
										<th id="th5" style="border: solid black 1px;">Visit Date</th>
										<th id="th10" style="border: solid black 1px;" colspan="2">Manage Visit</th>
									</tr>
								</thead>
								<tbody class="scrollContent3">
									<tr>
										<td headers="th0" style="border: solid black 1px;">David Ford Trading Ltd</td>
										<td headers="th2" style="border: solid black 1px;">Site X</td>
										<td headers="th3" style="border: solid black 1px;">N/A</td>
										<td headers="th4" style="border: solid black 1px;">En Gineer</td>
										<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
										<td headers="th10" style="border: solid black 1px;"><button class="show-pointer" onclick="getConfirmation()">Start Visit</button></td>
										<td headers="th11" style="border: solid black 1px;"><button class="show-pointer" onclick="rejectVisit(1)">Reject Visit</button></td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					<div class="divcolumn last"></div>
				</div>
				<div class="divcolumn last"></div>
			</div>
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="roundborder divcolumn" style="width: 94%; background-color: #e9eaee;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Recommended Opportunities</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer4">
						<table style="width: 100%;">
							<thead>
								<tr>
									<th id="th0" style="border: solid black 1px;">Customer</th>
									<th id="th1" style="border: solid black 1px;">Opportunity<br>Name</th>
									<th id="th2" style="border: solid black 1px;">Site</th>
									<th id="th3" style="border: solid black 1px;">Meter</th>
									<th id="th4" style="border: solid black 1px;">Engineer</th>
									<th id="th5" style="border: solid black 1px;">Estimated<br>Start Date</th>
									<th id="th6" style="border: solid black 1px;">Estimated<br>Finish Date</th>
									<th id="th7" style="border: solid black 1px;">Estimated<br>Cost</th>
									<th id="th8" style="border: solid black 1px;">Estimated<br>kWh Savings (pa)</th>
									<th id="th9" style="border: solid black 1px;">Estimated<br>£ Savings (pa)</th>
									<th id="th11" style="border: solid black 1px;" colspan="3">Manage Opportunity</th>
								</tr>
							</thead>
							<tbody class="scrollContent3">
								<tr>
									<td headers="th0" style="border: solid black 1px;">David Ford Trading Ltd</td>
									<td headers="th1" style="border: solid black 1px;">LED Lighting <i class="fas fa-search show-pointer"></i></td>
									<td headers="th2" style="border: solid black 1px;">Site X</td>
									<td headers="th3" style="border: solid black 1px;">N/A</td>
									<td headers="th4" style="border: solid black 1px;">En Gineer</td>
									<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
									<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
									<td headers="th7" style="border: solid black 1px;">£100,000</td>
									<td headers="th8" style="border: solid black 1px;">10,000</td>
									<td headers="th9" style="border: solid black 1px;">£15,000</td>
									<td headers="th11" style="border: solid black 1px;"><button class="show-pointer">Approve Opportunity</button></td>
									<td headers="th12" style="border: solid black 1px;"><button class="show-pointer">Reject Opportunity</button></td>
									<td headers="th13" style="border: solid black 1px;"><button class="show-pointer" style="margin: 2px;">Add Opportunities</button></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="divcolumn last"></div>
			</div>	
			<br>
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="roundborder divcolumn" style="width: 94%; background-color: #e9eaee;">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Pending & Active Opportunities</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer4">
						<table style="width: 100%;">
							<thead>
								<tr>
									<th id="th0" style="border: solid black 1px;">Customer</th>
									<th id="th1" style="border: solid black 1px;">Opportunity<br>Name</th>
									<th id="th2" style="border: solid black 1px;">Site</th>
									<th id="th3" style="border: solid black 1px;">Meter</th>
									<th id="th4" style="border: solid black 1px;">Engineer</th>
									<th id="th5" style="border: solid black 1px;">Estimated<br>Start Date</th>
									<th id="th6" style="border: solid black 1px;">Estimated<br>Finish Date</th>
									<th id="th7" style="border: solid black 1px;">Estimated<br>Cost</th>
									<th id="th8" style="border: solid black 1px;">Estimated<br>kWh Savings (pa)</th>
									<th id="th9" style="border: solid black 1px;">Estimated<br>£ Savings (pa)</th>
									<th id="th11" style="border: solid black 1px;" colspan="4">Manage Opportunity</th>
								</tr>
							</thead>
							<tbody class="scrollContent3">
								<tr>
									<td headers="th0" style="border: solid black 1px;">David Ford Trading Ltd</td>
									<td headers="th1" style="border: solid black 1px;">LED Lighting <i class="fas fa-search show-pointer"></i></td>
									<td headers="th2" style="border: solid black 1px;">Site X</td>
									<td headers="th3" style="border: solid black 1px;">N/A</td>
									<td headers="th4" style="border: solid black 1px;">En Gineer</td>
									<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
									<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
									<td headers="th7" style="border: solid black 1px;">£100,000</td>
									<td headers="th8" style="border: solid black 1px;">10,000</td>
									<td headers="th9" style="border: solid black 1px;">£15,000</td>
									<td headers="th11" style="border: solid black 1px;"><button class="show-pointer" style="margin: 2px;">Close Opportunity</button></td>
									<td headers="th12" style="border: solid black 1px;"><button class="show-pointer" style="margin: 2px;">Manage Opportunity Details</button></td>
									<td headers="th13" style="border: solid black 1px;"><button class="show-pointer" style="margin: 2px;">Manage Opportunity Timelines</button></td>
									<td headers="th14" style="border: solid black 1px;"><button class="show-pointer" style="margin: 2px;">Add Opportunities</button></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="divcolumn last"></div>
			</div>			
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="opportunitymanagement.js"></script>
<script type="text/javascript" src="opportunitymanagement.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>