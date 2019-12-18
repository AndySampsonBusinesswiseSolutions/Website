<?php 
	$PAGE_TITLE = "Contract Management";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div class="row"> -->
		<div class="tree-column">
			<div>
				<br>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<br>
			<div>
				<div class="group-by-div" id="outOfContract">
					<span>Out Of Contract Meters</span>
					<table style="border: solid black 1px; width: 100%;">
						<tr>
							<th>Supplier</th>
							<th>Contract Reference</th>
							<th>MPXN</th>
							<th>Contract Start Date</th>
							<th>Contract End</th>
							<th>Product Type</th>
							<th>Rates</th>
							<th>Is Businesswise Contract?</th>
						</tr>
						<tr>
							<td><i class="far fa-plus-square" id="oocContractHaven" style="padding-right: 10px;"></i>Haven</td>
							<td></td>
							<td>Multiple</td>
							<td>01/12/2019</td>
							<td>31/12/2019</td>
							<td>Out Of Contract</td>
							<td></td>
							<td>Multiple</td>
						</tr>
						<tbody id="oocContractHavenList" class="listitem-hidden">
							<tr>
								<td></td>
								<td>1</td>
								<td>987654</td>
								<td>01/12/2019</td>
								<td></td>
								<td>Out Of Contract</td>
								<td><i class="fas fa-search show-pointer"></i></td>
								<td>Yes</td>
							</tr>
							<tr>
								<td></td>
								<td><i class="far fa-plus-square" id="oocContract2" style="padding-right: 10px;" additionalcontrols="oocContractHaven"></i>2</td>
								<td>Multiple</td>
								<td>01/12/2019</td>
								<td>31/12/2019</td>
								<td>Out Of Contract</td>
								<td></td>
								<td>No</td>
							</tr>
							<tbody id="oocContract2List" class="listitem-hidden">
								<tr>
									<td></td>
									<td></td>
									<td>1234567890123</td>
									<td>01/12/2019</td>
									<td>31/12/2019</td>
									<td>Out Of Contract</td>
									<td><i class="fas fa-search show-pointer"></i></td>
									<td>No</td>
								</tr>
								<tr>
									<td></td>
									<td></td>
									<td>1234567890124</td>
									<td>01/12/2019</td>
									<td>31/12/2019</td>
									<td>Out Of Contract</td>
									<td><i class="fas fa-search show-pointer"></i></td>
									<td>No</td>
								</tr>
							</tbody>
						</tbody>
					</table>
				</div>
			</div>
			<!-- <br>
			<div>
				<div class="group-by-div" id="activeContract">
					<span>Active Contracts</span>
					<table style="border: solid black 1px; width: 100%;">
						<tr>
							<th>Contract ID</th>
							<th>Supplier</th>
							<th>MPXN</th>
							<th>Contract Start Date</th>
							<th>Contract End</th>
							<th>Contract Signed Date</th>
							<th>Contract Signee</th>
							<th>Product Type</th>
							<th>Unit Rate 1</th>
							<th>Unit Rate 2</th>
							<th>Capacity Charge</th>
							<th>Standing Charge</th>
							<th>Is Businesswise Contract?</th>
						</tr>
						<tr>
							<td><i class="far fa-plus-square" id="activeContract1"></i>3</td>
							<td>Haven</td>
							<td>Multiple</td>
							<td>01/12/2019</td>
							<td>31/12/2020</td>
							<td>01/11/2019</td>
							<td>David Ford</td>
							<td>Multiple</td>
							<td>Multiple</td>
							<td>Multiple</td>
							<td>Multiple</td>
							<td>Multiple</td>
							<td>Multiple</td>
						</tr>
						<tr class="activeContract1List listitem-hidden">
							<td style="padding-left: 20px;"><i class="fas fa-greater-than"></i></td>
							<td>Haven</td>
							<td>1234567890123</td>
							<td>01/12/2019</td>
							<td>31/12/2020</td>
							<td>01/11/2019</td>
							<td>David Ford</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
							<td>No</td>
						</tr>
						<tr class="activeContract1List listitem-hidden">
							<td style="padding-left: 20px;"><i class="fas fa-greater-than"></i></td>
							<td>Haven</td>
							<td>1234567890124</td>
							<td>01/12/2019</td>
							<td>31/12/2020</td>
							<td>01/11/2019</td>
							<td>David Ford</td>
							<td>Passthrough</td>
							<td>5.2p/kWh</td>
							<td>2.4p/kWh</td>
							<td></td>
							<td>£0.1/day</td>
							<td>Yes</td>
						</tr>
					</table>
				</div>
			</div>
			<br>
			<div>
				<div class="group-by-div" id="futureContract">
					<span>Future Contracts</span>
					<table style="border: solid black 1px; width: 100%;">
						<tr>
							<th>Contract ID</th>
							<th>Supplier</th>
							<th>MPXN</th>
							<th>Contract Start Date</th>
							<th>Contract End</th>
							<th>Contract Signed Date</th>
							<th>Contract Signee</th>
							<th>Product Type</th>
							<th>Unit Rate 1</th>
							<th>Unit Rate 2</th>
							<th>Capacity Charge</th>
							<th>Standing Charge</th>
						</tr>
						<tr>
							<td>4</td>
							<td>Hudson</td>
							<td>987650</td>
							<td>01/01/2020</td>
							<td>31/12/2020</td>
							<td>05/11/2019</td>
							<td>David Ford</td>
							<td>Fixed</td>
							<td>5.5p/thm</td>
							<td></td>
							<td></td>
							<td>£5/day</td>
						</tr>
						<tr>
							<td><i class="far fa-plus-square" id="futureContract1"></i>5</td>
							<td>British Gas</td>
							<td>Multiple</td>
							<td>01/01/2020</td>
							<td>31/12/2022</td>
							<td>08/10/2019</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
						</tr>
						<tr class="futureContract1List listitem-hidden">
							<td style="padding-left: 20px;"><i class="fas fa-greater-than"></i></td>
							<td>British Gas</td>
							<td>1234567890123</td>
							<td>01/01/2020</td>
							<td>31/12/2022</td>
							<td>08/10/2019</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
						</tr>
						<tr class="futureContract1List listitem-hidden">
							<td style="padding-left: 20px;"><i class="fas fa-greater-than"></i></td>
							<td>British Gas</td>
							<td>1234567890124</td>
							<td>01/01/2020</td>
							<td>31/12/2022</td>
							<td>08/10/2019</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
						</tr>
					</table>
				</div>
			</div>
			<br>
			<div>
				<div class="group-by-div" id="expiredContract">
					<span>Expired Contracts</span>
					<table style="border: solid black 1px; width: 100%;">
						<tr>
							<th>Contract ID</th>
							<th>Supplier</th>
							<th>MPXN</th>
							<th>Contract Start Date</th>
							<th>Contract End</th>
							<th>Contract Signed Date</th>
							<th>Contract Signee</th>
							<th>Product Type</th>
							<th>Unit Rate 1</th>
							<th>Unit Rate 2</th>
							<th>Capacity Charge</th>
							<th>Standing Charge</th>
						</tr>
						<tr>
							<td>2753727532</td>
							<td>British Gas</td>
							<td>987650</td>
							<td>01/01/2018</td>
							<td>31/10/2019</td>
							<td>08/10/2017</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/thm</td>
							<td></td>
							<td></td>
							<td>£2/day</td>
						</tr>
						<tr>
							<td>2753727533</td>
							<td>British Gas</td>
							<td>1234567890123</td>
							<td>01/01/2018</td>
							<td>31/10/2019</td>
							<td>08/10/2016</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
						</tr>
						<tr>
							<td>2753727534</td>
							<td>British Gas</td>
							<td>1234567890124</td>
							<td>01/01/2018</td>
							<td>31/10/2019</td>
							<td>08/10/2016</td>
							<td>Andrew Sampson</td>
							<td>Fixed</td>
							<td>20p/kWh</td>
							<td>15p/kWh</td>
							<td>3.5p/kVa/day</td>
							<td>£2/day</td>
						</tr>
					</table>
				</div>
			</div>
			<br>
			<div>
				<div class="group-by-div" id="noContract">
				<span>No Contract Information</span>
					<table style="border: solid black 1px; width: 100%;">
						<tr>
							<th>Contract ID</th>
							<th>Supplier</th>
							<th>MPXN</th>
							<th>Contract Start Date</th>
							<th>Contract End</th>
							<th>Contract Signed Date</th>
							<th>Contract Signee</th>
							<th>Product Type</th>
							<th>Unit Rate 1</th>
							<th>Unit Rate 2</th>
							<th>Capacity Charge</th>
							<th>Standing Charge</th>
						</tr>
						<tr>
							<td>2753727532</td>
							<td>British Gas</td>
							<td>987650</td>
							<td>01/11/2019</td>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
							<td></td>
						</tr>
					</table>
				</div>
			</div> -->
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/tree.js"></script>
<script type="text/javascript" src="/basedata/data.json"></script>
<script type="text/javascript" src="/basedata/contract.json"></script>

<script type="text/javascript"> 
	createTree(data, "Hierarchy", "treeDiv", "", "");
	addExpanderOnClickEvents();

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>