<?php 
	$PAGE_TITLE = "Finished Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="finishedopportunities.css">
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
			<div class="roundborder">
				<div id="cumulativeSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="cumulativeSavingspan">Cumulative Saving Chart</span>
				<div id="cumulativeSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="cumulativeSavingChart"></div>
				</div>
			</div>			
			<br>
			<div class="roundborder">
				<div id="costSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="costSavingspan">Cost Saving Chart</span>
				<div id="costSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="costSavingChart"></div>
				</div>
			</div>			
			<br>
			<div class="roundborder">
				<div id="volumeSaving" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
				<span id="volumeSavingspan">Volume Saving Chart</span>
				<div id="volumeSavingList"  class="listitem-hidden chart" style="margin: 5px;">
					<div id="volumeSavingChart"></div>
				</div>
			</div>			
			<br>
			<div id="tableContainer" class="tableContainer3">
				<table style="width: 100%;">
					<thead>
						<tr>
							<th id="th1" style="border: solid black 1px;">Project Name</th>
							<th id="th2" style="border: solid black 1px;">Site</th>
							<th id="th3" style="border: solid black 1px;">Meter</th>
							<th id="th4" style="border: solid black 1px;">Engineer</th>
							<th id="th10" style="border: solid black 1px;">Notes</th>
							<th id="th5" style="border: solid black 1px;">Start Date</th>
							<th id="th6" style="border: solid black 1px;">Finish Date</th>
							<th id="th7" style="border: solid black 1px;">Cost</th>
							<th id="th8" style="border: solid black 1px;">Actual<br>kWh Savings (pa)</th>
							<th id="th9" style="border: solid black 1px;">Actual<br>£ Savings (pa)</th>
							<th id="th11" style="border: solid black 1px;">Estimated<br>kWh Savings (pa)</th>
							<th id="th12" style="border: solid black 1px;">Estimated<br>£ Savings (pa)</th>
							<th id="th13" style="border: solid black 1px;">Net<br>kWh Savings (pa)</th>
							<th id="th14" style="border: solid black 1px;">Net<br>£ Savings (pa)</th>
							<th id="th15" style="border: solid black 1px;">Total<br>ROI Months</th>
							<th id="th16" style="border: solid black 1px;">Remaining<br>ROI Months</th>
						</tr>
					</thead>
					<tbody class="scrollContent3">
					<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site X</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th10" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
							<td headers="th5" style="border: solid black 1px;">01/01/2017</td>
							<td headers="th6" style="border: solid black 1px;">28/02/2017</td>
							<td headers="th7" style="border: solid black 1px;">£55,000</td>
							<td headers="th8" style="border: solid black 1px;">10,000</td>
							<td headers="th9" style="border: solid black 1px;">£65,000</td>
							<td headers="th11" style="border: solid black 1px;">9,000</td>
							<td headers="th12" style="border: solid black 1px;">£60,000</td>
							<td headers="th13" style="border: solid black 1px;">45,000</td>
							<td headers="th14" style="border: solid black 1px;">£140,000</td>
							<td headers="th15" style="border: solid black 1px;">9</td>
							<td headers="th16" style="border: solid black 1px;">0</td>
						</tr>
						<tr>
							<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
							<td headers="th2" style="border: solid black 1px;">Site Y</td>
							<td headers="th3" style="border: solid black 1px;">N/A</td>
							<td headers="th4" style="border: solid black 1px;">En Gineer</td>
							<td headers="th10" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
							<td headers="th5" style="border: solid black 1px;">01/08/2019</td>
							<td headers="th6" style="border: solid black 1px;">15/08/2019</td>
							<td headers="th7" style="border: solid black 1px;">£10,000</td>
							<td headers="th8" style="border: solid black 1px;">5,000</td>
							<td headers="th9" style="border: solid black 1px;">£60,000</td>
							<td headers="th11" style="border: solid black 1px;">9,000</td>
							<td headers="th12" style="border: solid black 1px;">£160,000</td>
							<td headers="th13" style="border: solid black 1px;">45,000</td>
							<td headers="th14" style="border: solid black 1px;">£20,000</td>
							<td headers="th15" style="border: solid black 1px;">2</td>
							<td headers="th16" style="border: solid black 1px;">0</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="finishedopportunities.js"></script>
<script type="text/javascript" src="finishedopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>