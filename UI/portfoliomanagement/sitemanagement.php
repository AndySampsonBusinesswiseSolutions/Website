<?php 
	$PAGE_TITLE = "Site Management";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<style>
	th {
		text-align: center;
	}

	tr:nth-child(even) {
		background-color: #dddddd;
	}
</style>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div class="row">
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
				<div class="group-by-div">
					<div style="overflow-y:auto;" class="tree-div">
						<table style="width: 100%;">
							<tr style = "border-bottom: solid black 1px;">
								<th style = "padding-right: 50px; width: 15%; border-right: solid black 1px;">Type</th>
								<th style = "padding-right: 50px; width: 15%; border-right: solid black 1px;">Identifier</th>
								<th style = "padding-right: 50px; width: 30%; border-right: solid black 1px;">Attribute</th>
								<th style = "padding-right: 50px; border-right: solid black 1px;">Value</th>
								<th style = "width: 5%;"></th>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Site</td>
								<td style = "border-right: solid black 1px;">Leeds</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Meter</td>
								<td style = "border-right: solid black 1px;">1234567890123</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
							<tr>
								<td style = "border-right: solid black 1px;">Sub Meter</td>
								<td style = "border-right: solid black 1px;">Sub Meter 1</td>
								<td style = "border-right: solid black 1px;">Attribute</td>
								<td style = "border-right: solid black 1px;">Value</td>
								<td>
									<i class="fas fa-edit"></i>
									<i class="fas fa-trash-alt"></i>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
		</div>	
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/tree.js"></script>
<script type="text/javascript" src="/basedata/data.json"></script>

<script type="text/javascript"> 
	createTree(data, "Hierarchy", "treeDiv", "", "");
	addExpanderOnClickEvents();
	addArrowOnClickEvents();

	window.onload = function(){
		resizeFinalColumns(365);
	}

	window.onresize = function(){
		resizeFinalColumns(365);
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>