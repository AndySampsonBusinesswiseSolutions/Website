<?php 
	$PAGE_TITLE = "Manage Customers";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="managecustomers.css">
</head>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div class="row">
		<div class="tree-column">
			<div>
				<br>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
					<button style="margin-top: 5px; margin-bottom: 5px; width: 100%;" onclick='addCustomer()'>Add New Customer</button>
					<button style="margin-bottom: 5px; width: 100%;" onclick='deleteCustomers()'>Delete Selected Customers</button>
					<button style="width: 100%;" onclick='reinstateCustomers()'>Reinstate Deleted Customers</button>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<br>
			<div>
				<div class="group-by-div" id="cardDiv" style="display: none;">
					<div class="tabDiv" id="tabDiv" style="overflow-y: auto; overflow: auto;"></div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script type="text/javascript" src="managecustomers.js"></script>
<script type="text/javascript" src="managecustomers.json"></script>

<script type="text/javascript"> 
	pageLoad()
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>