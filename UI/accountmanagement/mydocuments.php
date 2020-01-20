<?php 
	$PAGE_TITLE = "My Documents";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div>
		<div id="loaHeader">
			<div style="padding: 15px;">
				<div class="datagrid" style="height: 100px;">
					Your current LOA details:
					<br><span style="font-size: 20px; color: red;">Your current LOA is 5 days away from expiring!</span>
					<br><span>Here we can put any other info about the current LOA or anything we so desire</span>
				</div>
			</div>
		<div>
	</div>	
	<div class="row">
		<div class="tree-column">
			<div>
				<div class="tree-column">
					<div id="treeDiv" class="tree-div">
					</div>
					<br>
					<button style="width: 100%;" onclick='addDocument()'>Upload Document</button>
				</div>
			</div>
		</div>
		<div class="fill-column"></div>
		<div class="final-column">
			<div>
				<div class="group-by-div" id="cardDiv" style="display: none;">
					<div class="tabDiv" id="tabDiv" style="overflow-y: auto; overflow: auto;"></div>
				</div>
			</div>
		</div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>
<script src="/javascript/documenttree.js"></script>
<script src="/javascript/documenttab.js"></script>
<script type="text/javascript" src="/basedata/document.json"></script>

<link href="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.css" rel="stylesheet"/>
<script src="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.js"></script>

<script type="text/javascript"> 
	var data = documents;
	createTree(data, "treeDiv", "createCardButton");
	addExpanderOnClickEvents();	

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}

	function addDocument() {
		var div = 
			'<div>'+
				'<div>'+
					'<button>Select Document</button>'+
					'<span>Full path of document selected will appear here</span>'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div>'+
				'<div>'+
					'<span style="border: solid black 1px;">Select Document Type</span>'+
					'<select>'+
						'<option value="LOA">Letter Of Authority</option>'+
						'<option value="Invoice">Invoice</option>'+
						'<option value="Bill">Bill</option>'+
					'</select>'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div>'+
				'<div>'+
					'<span style="border: solid black 1px;">Select Letter Of Authority End Date</span>'+
					'<input type="date" name="calendar" id="calendar" value="2019-11-26">'+
				'</div>'+
			'</div>'+
			'<br>'+
			'<div>'+
				'<div>'+
					'<span style="border: solid black 1px;">Letter Of Authority Signed By</span>'+
					'<input></input>'+
				'</div>'+
			'</div>'+
			'<br>'

		xdialog.confirm(div, function() {}, 
		{
			style: 'width:50%;font-size:0.8rem;',
			buttons: {
				ok: {
					text: 'Save & Close',
					style: 'background: Green;'
				}
			},
			title: 'Upload Document'
		});
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>