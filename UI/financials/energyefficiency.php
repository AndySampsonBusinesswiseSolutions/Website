<?php 
	$PAGE_TITLE = "Energy Efficiency";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<p></p>
	Break out into new navigation section - Energy Efficiency Opportunities
	Active
	Identified
	New

	<p>Ability to select 
	Preset opportunities
	-> Triad avoidance
	-> DUoS Super Red Rate/Red Rate avoidance</p>

	<p>
		Custom opportunities
		-> Workflow process
		-> Select sites/meters
		-> Note as to what project is related to
		-> Cost of project
		-> Assumptions regarding how much energy currently consumed
		-> Submeter installed? -> If not, then assumptions above used to calculate savings
		-> Ability to show actual waste (i.e. projects not started on time but using assumptions around savings)
		-> Graphs showing lots of things
	</p>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>