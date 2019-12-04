<?php 
	$PAGE_TITLE = "Site Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div style="margin-left: 15px;">
		<div>Select Commodity To Display</div>
		<span>Electricity</span><span class="show-pointer">&nbsp;<i class="fas fa-angle-double-left" id="electricityGasSelector"></i>&nbsp;</span><span>Gas</span>
	</div>
	<div id="electricityDiv">
		<div class="row">
			<div class="tree-column">
				<div>
					<br><b style="padding-left: 15px;">Electricity Meters</b>
					<div class="tree-column">
						<div id="electricityTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				
			</div>	
		</div>
	</div>
	<div id="gasDiv" class="listitem-hidden">
		<div class="row">
		<div class="tree-column">
				<div>
					<br><b style="padding-left: 15px;">Gas Meters</b>
					<div class="tree-column">
						<div id="gasTreeDiv" class="tree-div">
						</div>
					</div>
				</div>
			</div>
			<div class="fill-column"></div>
			<div class="final-column">
				
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/tree.js"></script>
<script type="text/javascript" src="/basedata/data.json"></script>

<script>
	function addCommoditySelectorOnClickEvent() {
		var commoditySelector = document.getElementById("electricityGasSelector");
		commoditySelector.addEventListener('click', function(event) {
			updateClassOnClick("electricityDiv", "listitem-hidden", "")
			updateClassOnClick("gasDiv", "listitem-hidden", "")
		})	
	}
</script>

<script type="text/javascript"> 
	createTree(data, "Hierarchy", "electricityTreeDiv", "electricity", "");
	createTree(data, "Hierarchy", "gasTreeDiv", "gas", "");
	addExpanderOnClickEvents();
	addArrowOnClickEvents();
	addCommoditySelectorOnClickEvent();	
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>