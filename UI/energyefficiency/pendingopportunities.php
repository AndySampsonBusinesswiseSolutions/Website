<?php 
	$PAGE_TITLE = "Pending Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">
<link rel="stylesheet" type="text/css" href="/css/jquery-ui-1.8.4.css" />
<link rel="stylesheet" type="text/css" href="/css/jquery.ganttView.css" />

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
			<div>
				<div id="ganttChart"></div>
				<br>
				<div id="tableContainer" class="tableContainer3">
					<table style="width: 100%;">
						<thead>
							<tr>
								<th id="th1" style="border: solid black 1px;">Project Name</th>
								<th id="th2" style="border: solid black 1px;">Site</th>
								<th id="th3" style="border: solid black 1px;">Meter</th>
								<th id="th4" style="border: solid black 1px;">Engineer</th>
								<th id="th5" style="border: solid black 1px;">Estimated Start Date</th>
								<th id="th6" style="border: solid black 1px;">Estimated Finish Date</th>
								<th id="th7" style="border: solid black 1px;">Estimated Cost</th>
								<th id="th8" style="border: solid black 1px;">Estimated kWh Savings (pa)</th>
								<th id="th9" style="border: solid black 1px;">Estimated £ Savings (pa)</th>
							</tr>
						</thead>
						<tbody class="scrollContent3">
						<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
							<tr>
								<td headers="th1" style="border: solid black 1px;">LED Lighting</td>
								<td headers="th2" style="border: solid black 1px;">Site X</td>
								<td headers="th3" style="border: solid black 1px;">N/A</td>
								<td headers="th4" style="border: solid black 1px;">En Gineer</td>
								<td headers="th5" style="border: solid black 1px;">01/04/2020</td>
								<td headers="th6" style="border: solid black 1px;">30/06/2020</td>
								<td headers="th7" style="border: solid black 1px;">£100,000</td>
								<td headers="th8" style="border: solid black 1px;">10,000</td>
								<td headers="th9" style="border: solid black 1px;">£15,000</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="/javascript/jquery-1.4.2.js"></script>
<script type="text/javascript" src="/javascript/date.js"></script>
<script type="text/javascript" src="/javascript/jquery-ui-1.8.4.js"></script>
<script type="text/javascript" src="/javascript/jquery.ganttView.js"></script>
<script type="text/javascript" src="/basedata/gantt2.js"></script>
<script src="/javascript/utils.js"></script>
<script src="/javascript/activeopportunitytree.js"></script>
<script type="text/javascript" src="/basedata/activeopportunity.json"></script>

<script type="text/javascript"> 
	var data = activeopportunity;
	createTree(data, "treeDiv", "");
	addExpanderOnClickEvents();

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}
</script>

<script>
$(function () {
			$("#ganttChart").ganttView({ 
				data: ganttData,
				slideWidth: 1300
			});
		});
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>