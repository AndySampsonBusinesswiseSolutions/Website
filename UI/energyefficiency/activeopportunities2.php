<?php 
	$PAGE_TITLE = "Active Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<link rel="stylesheet" type="text/css" href="/css/jquery-ui-1.8.4.css" />
	<link rel="stylesheet" type="text/css" href="/css/reset.css" />
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
				<br/><br/>
				<div id="eventMessage"></div>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="/javascript/jquery-1.4.2.js"></script>
<script type="text/javascript" src="/javascript/date.js"></script>
<script type="text/javascript" src="/javascript/jquery-ui-1.8.4.js"></script>
<script type="text/javascript" src="/javascript/jquery.ganttView.js"></script>
<script type="text/javascript" src="/basedata/gantt.js"></script>
<script src="/javascript/utils.js"></script>

<script>
$(function () {
			$("#ganttChart").ganttView({ 
				data: ganttData,
				slideWidth: 1300,
				behavior: {
					onClick: function (data) { 
						var msg = "You clicked on an event: { start: " + data.start.toString("M/d/yyyy") + ", end: " + data.end.toString("M/d/yyyy") + " }";
						$("#eventMessage").text(msg);
					},
					onResize: function (data) { 
						var msg = "You resized an event: { start: " + data.start.toString("M/d/yyyy") + ", end: " + data.end.toString("M/d/yyyy") + " }";
						$("#eventMessage").text(msg);
					},
					onDrag: function (data) { 
						var msg = "You dragged an event: { start: " + data.start.toString("M/d/yyyy") + ", end: " + data.end.toString("M/d/yyyy") + " }";
						$("#eventMessage").text(msg);
					}
				}
			});
		});
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>