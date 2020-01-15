<?php 
	$PAGE_TITLE = "Manage Customers";
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

<script src="/javascript/utils.js"></script>
<script src="/javascript/customertree.js"></script>
<script src="/javascript/customertab.js"></script>
<script type="text/javascript" src="/basedata/customer.json"></script>

<link href="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.css" rel="stylesheet"/>
<script src="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.js"></script>

<script type="text/javascript"> 
var customerCardViewAttributes = [
	"Address Line 1",
	"Address Line 2",
	"Address Line 3",
	"Address Line 4",
	"Postcode",
	"Contact Name",
	"Contact Telephone Number",
	"Email"
]
	var data = customer;
	createTree(data, "treeDiv", "createCardButton");
	addExpanderOnClickEvents();

	window.onload = function(){
		resizeFinalColumns(380);
	}

	window.onresize = function(){
		resizeFinalColumns(380);
	}
</script>

<script>
	function addCustomer() {
		var div = document.createElement('div');

		var customerNameTable = document.createElement('table');
		customerNameTable.id = 'customerNameTable';
		customerNameTable.setAttribute('style', 'width: 100%;');

		var addDetailsTable = document.createElement('table');
		addDetailsTable.id = 'addDetailsTable';
		addDetailsTable.setAttribute('style', 'width: 100%;');

		var addRolesTable = document.createElement('table');
		addRolesTable.id = 'addRolesTable';
		addRolesTable.setAttribute('style', 'width: 100%;');

		var customerNameTableRow = document.createElement('tr');
		customerNameTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Role'));
		customerNameTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
		customerNameTableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
		customerNameTable.appendChild(customerNameTableRow);
		customerNameTable.appendChild(document.createElement('br'));

		var detailsTableRow = document.createElement('tr');
		detailsTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Detail'));
		detailsTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
		detailsTableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
		addDetailsTable.appendChild(detailsTableRow);
		addDetailsTable.appendChild(document.createElement('br'));

		var rolesTableRow = document.createElement('tr');
		rolesTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Role'));
		rolesTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
		rolesTableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
		addRolesTable.appendChild(rolesTableRow);
		addRolesTable.appendChild(document.createElement('br'));

		var addCustomerNameTableRow = document.createElement('tr');
		var addDetailsTableRow = document.createElement('tr');
		var addRolessTableRow = document.createElement('tr');

		for(var j = 0; j < 3; j++) {
			var customerNameTableDatacell = document.createElement('td');
			var detailTableDatacell = document.createElement('td');
			var roleTableDatacell = document.createElement('td');

			customerNameTableDatacell.setAttribute('style', 'border: solid black 1px;');
			detailTableDatacell.setAttribute('style', 'border: solid black 1px;');
			roleTableDatacell.setAttribute('style', 'border: solid black 1px;');
			
			if(j == 0) {
				customerNameTableDatacell.innerHTML = 'Customer Name';
				detailTableDatacell.innerHTML = 'Select Detail Type';
				roleTableDatacell.innerHTML = 'Select Role';
			}
			else if(j == 1) {
				customerNameTableDatacell.innerHTML = 'Enter Customer Name';
				detailTableDatacell.innerHTML = 'Enter/Select Detail Value';
				roleTableDatacell.innerHTML = 'Enter/Select Role Value';
			}
			else {
				detailTableDatacell.innerHTML = 'Add/Delete Icon';
				roleTableDatacell.innerHTML = 'Add/Delete Icon';
			}

			addCustomerNameTableRow.appendChild(customerNameTableDatacell);
			addDetailsTableRow.appendChild(detailTableDatacell);
			addRolessTableRow.appendChild(roleTableDatacell);
		}

		customerNameTable.appendChild(addCustomerNameTableRow);
		addDetailsTable.appendChild(addDetailsTableRow);
		addRolesTable.appendChild(addRolessTableRow);

		div.appendChild(customerNameTable);
		div.appendChild(addDetailsTable);
		div.appendChild(addRolesTable);

		xdialog.confirm(div.outerHTML, function() {}, 
		{
			style: 'width:50%;font-size:0.8rem;',
			buttons: {
				ok: {
					text: 'Save & Close',
					style: 'background: Green;'
				}
			},
			title: 'Add New Customer'
		});
	}

	function deleteCustomers() {
		xdialog.confirm('List customers to delete here', function() {
		}, {
			style: 'width:420px;font-size:0.8rem;',
			title: 'Are You Sure You Want To Delete These Customers?',
			buttons: {
				ok: {
					text: 'Delete Customers',
					style: 'background: red;',
				},
				cancel: {
					text: 'Cancel',
					style: 'background: Green;',
				}
			}
		});
	}

	function reinstateCustomers() {
		xdialog.confirm('List customers that can be reinstated here', function() {
		}, {
			style: 'width:420px;font-size:0.8rem;',
			title: 'Select Customers To Reinstate',
			buttons: {
				ok: {
					text: 'Reinstate Customers',
					style: 'background: green;',
				},
				cancel: {
					text: 'Cancel',
					style: 'background: grey;',
				}
			}
		});
	}
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>