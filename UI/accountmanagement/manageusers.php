<?php 
	$PAGE_TITLE = "Manage Users";
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
					<button style="margin-top: 5px; margin-bottom: 5px; width: 100%;" onclick='addUser()'>Add New User</button>
					<button style="margin-bottom: 5px; width: 100%;" onclick='deleteUsers()'>Delete Selected Users</button>
					<button style="width: 100%;" onclick='reinstateUsers()'>Reinstate Deleted Users</button>
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
<script src="/javascript/usertree.js"></script>
<script src="/javascript/usertab.js"></script>
<script type="text/javascript" src="/basedata/user.json"></script>

<link href="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.css" rel="stylesheet"/>
<script src="https://cdn.jsdelivr.net/gh/xxjapp/xdialog@3/xdialog.min.js"></script>

<script type="text/javascript"> 
var userCardViewAttributes = [
	"Address Line 1",
	"Address Line 2",
	"Address Line 3",
	"Address Line 4",
	"Postcode",
	"Contact Name",
	"Contact Telephone Number",
	"Email"
]
	var data = user;
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
	function addUser() {
		var div = document.createElement('div');

		var userNameTable = document.createElement('table');
		userNameTable.id = 'userNameTable';
		userNameTable.setAttribute('style', 'width: 100%;');

		var addDetailsTable = document.createElement('table');
		addDetailsTable.id = 'addDetailsTable';
		addDetailsTable.setAttribute('style', 'width: 100%;');

		var addRolesTable = document.createElement('table');
		addRolesTable.id = 'addRolesTable';
		addRolesTable.setAttribute('style', 'width: 100%;');

		var userNameTableRow = document.createElement('tr');
		userNameTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Role'));
		userNameTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
		userNameTableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
		userNameTable.appendChild(userNameTableRow);
		userNameTable.appendChild(document.createElement('br'));

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

		var addUserNameTableRow = document.createElement('tr');
		var addDetailsTableRow = document.createElement('tr');
		var addRolessTableRow = document.createElement('tr');

		for(var j = 0; j < 3; j++) {
			var userNameTableDatacell = document.createElement('td');
			var detailTableDatacell = document.createElement('td');
			var roleTableDatacell = document.createElement('td');

			userNameTableDatacell.setAttribute('style', 'border: solid black 1px;');
			detailTableDatacell.setAttribute('style', 'border: solid black 1px;');
			roleTableDatacell.setAttribute('style', 'border: solid black 1px;');
			
			if(j == 0) {
				userNameTableDatacell.innerHTML = 'User Name';
				detailTableDatacell.innerHTML = 'Select Detail Type';
				roleTableDatacell.innerHTML = 'Select Role';
			}
			else if(j == 1) {
				userNameTableDatacell.innerHTML = 'Enter User Name';
				detailTableDatacell.innerHTML = 'Enter/Select Detail Value';
				roleTableDatacell.innerHTML = 'Enter/Select Role Value';
			}
			else {
				detailTableDatacell.innerHTML = 'Add/Delete Icon';
				roleTableDatacell.innerHTML = 'Add/Delete Icon';
			}

			addUserNameTableRow.appendChild(userNameTableDatacell);
			addDetailsTableRow.appendChild(detailTableDatacell);
			addRolessTableRow.appendChild(roleTableDatacell);
		}

		userNameTable.appendChild(addUserNameTableRow);
		addDetailsTable.appendChild(addDetailsTableRow);
		addRolesTable.appendChild(addRolessTableRow);

		div.appendChild(userNameTable);
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
			title: 'Add New User'
		});
	}

	function deleteUsers() {
		xdialog.confirm('List users to delete here', function() {
		}, {
			style: 'width:420px;font-size:0.8rem;',
			title: 'Are You Sure You Want To Delete These Users?',
			buttons: {
				ok: {
					text: 'Delete Users',
					style: 'background: red;',
				},
				cancel: {
					text: 'Cancel',
					style: 'background: Green;',
				}
			}
		});
	}

	function reinstateUsers() {
		xdialog.confirm('List users that can be reinstated here', function() {
		}, {
			style: 'width:420px;font-size:0.8rem;',
			title: 'Select Users To Reinstate',
			buttons: {
				ok: {
					text: 'Reinstate Users',
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