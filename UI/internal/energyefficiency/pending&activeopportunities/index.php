<?php 
	$PAGE_TITLE = "Pending & Active Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>
	
	<link rel="stylesheet" href="pending&activeopportunities.css">
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
					<br>
					<div class="datagrid" style="background-color: #e9eaee;">
						<span style="padding-left: 5px;">Select Project Status</span>
						<ul class="format-listitem">
							<li>
								<input type="radio" name="group2" value="All" id="allStatusradio" checked guid="0" onclick='buildDataGrid(); buildGanttChart()'><span id="allStatusspan" style="padding-left: 1px;">All</span>
							</li>
							<li>
								<input type="radio" name="group2" value="Pending" id="pendingStatusradio" guid="0" onclick='buildDataGrid(); buildGanttChart()'><span id="pendingStatusspan" style="padding-left: 1px;">Pending</span>
							</li>
							<li>
								<input type="radio" name="group2" value="Active" id="activeStatusradio" guid="0" onclick='buildDataGrid(); buildGanttChart()'><span id="activeStatusspan" style="padding-left: 1px;">Active</span>
							</li>
						</ul>
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<div>
				<div id="ganttChart">
				</div>
				<br>
				<div id="spreadsheet"></div><br>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="jquery-1.4.2.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="pending&activeopportunities.js"></script>
<script type="text/javascript" src="pending&activeopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>