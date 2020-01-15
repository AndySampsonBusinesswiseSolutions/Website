function openTab(evt, tabName, guid, branch) {
	var cardDiv = document.getElementById('cardDiv');

	var tabContent = document.getElementsByClassName("tabcontent");
	var tabContentLength = tabContent.length;
	for (var i = 0; i < tabContentLength; i++) {
	  cardDiv.removeChild(tabContent[i]);
	}

	var tabLinks = document.getElementsByClassName("tablinks");
	var tabLinksLength = tabLinks.length;
	for (var i = 0; i < tabLinksLength; i++) {
		tabLinks[i].className = tabLinks[i].className.replace(" active", "");
	}
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName;
	cardDiv.appendChild(newDiv);

	createCard(guid, newDiv, 'User');

	document.getElementById(tabName).style.display = "block";
	evt.currentTarget.className += " active";
  }

function createCardButton(checkbox){
	var cardDiv = document.getElementById('cardDiv');
	var tabDiv = document.getElementById('tabDiv');
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));

	if(checkbox.checked){
		cardDiv.setAttribute('style', '');
		var button = document.createElement('button');
		button.setAttribute('class', 'tablinks');
		button.setAttribute('onclick', 'openTab(event, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '", "' + checkbox.getAttribute('branch') + '")');

		button.innerHTML = span.innerHTML;
		button.id = span.id.replace('span', 'button');
		tabDiv.appendChild(button);
	
		updateTabDiv();
	}
	else {
		tabDiv.removeChild(document.getElementById(span.id.replace('span', 'button')));

		var divToRemove = document.getElementById(span.id.replace('span', 'div'));
		if(divToRemove) {
			cardDiv.removeChild();
		}

		if(tabDiv.children.length == 0) {
			cardDiv.setAttribute('style', 'display: none;');
		}
		else {
            updateTabDiv();
		}
	}	
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

    tabDivChildren[0].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px;'));
    for(var i = 1; i < tabDivChildrenLength; i++) {
        tabDivChildren[i].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px; border-left: solid black 1px;'));
    }
}

function createCard(guid, divToAppendTo, identifier) {
	var user;
	
	var userLength = data.length;
	for(var i = 0; i < userLength; i++) {
		user = data[i];

		if(user.GUID == guid) {
			break;
		}
	}

	buildCardView(user, divToAppendTo);
	buildUserDataTable(user, identifier, divToAppendTo);
	divToAppendTo.appendChild(document.createElement('br'));
	buildRoleDataTable(user, identifier, divToAppendTo);
}

function buildCardView(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'cardView';
	div.setAttribute('class', 'group-div');

	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	table.appendChild(createTableHeader('width: 50%', ''));
	table.appendChild(createTableHeader('width: 50%', ''));

	var cardViewAttributes = userCardViewAttributes;
	var cardViewAttributesLength = cardViewAttributes.length;
	var entityAttributes = entity.Attributes;

	for(var i = 0; i < cardViewAttributesLength; i++) {
		var cardViewAttribute = cardViewAttributes[i];
		var tableRow = document.createElement('tr');
		var tableDatacellAttribute = document.createElement('td');
		var tableDatacellAttributeValue = document.createElement('td');

		tableDatacellAttribute.innerHTML = cardViewAttribute;
		tableDatacellAttributeValue.innerHTML = getAttribute(entityAttributes, cardViewAttribute) || '';

		tableRow.appendChild(tableDatacellAttribute);
		tableRow.appendChild(tableDatacellAttributeValue);
		table.appendChild(tableRow);
	}

	div.appendChild(table);
	divToAppendTo.appendChild(div);

	var addDetailsButton = document.createElement('button');
	addDetailsButton.id = 'addDetailsButton';
	addDetailsButton.innerHTML = 'Add Details';
	addDetailsButton.setAttribute('onclick', 'addDetails("' + entity.Attributes[0]["UserName"] + '")');
	addDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(addDetailsButton);	

	var editDetailsButton = document.createElement('button');
	editDetailsButton.id = 'editDetailsButton';
	editDetailsButton.innerHTML = 'Edit Details';
	editDetailsButton.setAttribute('onclick', 'displayUserDataTable()');
	editDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editDetailsButton);	

	var addRolesButton = document.createElement('button');
	addRolesButton.id = 'addRolesButton';
	addRolesButton.innerHTML = 'Add Roles';
	addRolesButton.setAttribute('onclick', 'addRoles("' + entity.Attributes[0]["UserName"] + '")');
	addRolesButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(addRolesButton);	

	var editRolesButton = document.createElement('button');
	editRolesButton.id = 'editRolesButton';
	editRolesButton.innerHTML = 'Edit Roles';
	editRolesButton.setAttribute('onclick', 'displayRoleDataTable()');
	editRolesButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editRolesButton);	
}

function displayUserDataTable() {
	var button = document.getElementById('editDetailsButton');
	var div = document.getElementById('displayAttributes');

	if(button.innerHTML == 'Edit Details') {
		div.setAttribute('style', '');
		button.innerText = 'Hide Details';
	}
	else {
		div.setAttribute('style', 'display: none');
		button.innerText = 'Edit Details'
	}
}

function buildUserDataTable(entity, attributeRequired, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('style', 'display: none');
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 15%; border: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(getAttribute(entity.Attributes, attributeRequired), entity.Attributes, table, 'User');

	treeDiv.appendChild(table);
}

function displayRoleDataTable() {
	var button = document.getElementById('editRolesButton');
	var div = document.getElementById('displayRoles');

	if(button.innerHTML == 'Edit Roles') {
		div.setAttribute('style', '');
		button.innerText = 'Hide Roles';
	}
	else {
		div.setAttribute('style', 'display: none');
		button.innerText = 'Edit Roles'
	}
}

function buildRoleDataTable(entity, attributeRequired, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayRoles';
	div.setAttribute('style', 'display: none');
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayRoles');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 15%; border: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(getAttribute(entity.Roles, attributeRequired), entity.Roles, table, 'Role');

	treeDiv.appendChild(table);
}

function displayAttributes(identifier, attributes, table, type) {
	if(!attributes) {
		return;
	}

	var attributesLength = attributes.length;
	for(var i = 0; i < attributesLength; i++) {
		var tableRow = document.createElement('tr');
		tableRow.id = 'row'.concat(type + i);

		for(var j = 0; j < 3; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(j) {
				case 0:
					tableDatacell.innerHTML = type;
					break;	
				case 1:
				// 	tableDatacell.innerHTML = identifier;
				// 	break;
				// case 2:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					
					tableDatacell.id = 'attribute'.concat(type + i);
					break;
				// case 3:
				case 2:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}

					tableDatacell.id = 'value'.concat(type + i);
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		var tableDatacell = document.createElement('td');
		tableDatacell.setAttribute('style', 'border: solid black 1px;');

		var editIcon = createIcon('editRow' + type + i, 'fas fa-edit', 'cursor: pointer;', 'showDetailEditor("' + type + i + '")', 'Edit');
		//var deleteIcon = createIcon('deleteRow' + type + i, 'fas fa-trash-alt', 'cursor: pointer;', 'deleteRow("' + type + i + '")');
		var saveChangeIcon = createIcon('saveRow' + type + i, 'fas fa-save', 'display: none;', 'saveRow("' + type + i + '")', 'Save');
		var undoChangeIcon = createIcon('undoRow' + type + i, 'fas fa-undo', 'display: none;', 'undoRow("' + type + i + '")', 'Undo');
		var cancelChangeIcon = createIcon('cancelRow' + type + i, 'far fa-window-close', 'display: none;', 'cancelRow("' + type + i + '")', 'Cancel');

		tableDatacell.appendChild(editIcon);
		//tableDatacell.appendChild(deleteIcon);
		tableDatacell.appendChild(saveChangeIcon);
		tableDatacell.appendChild(undoChangeIcon);
		tableDatacell.appendChild(cancelChangeIcon);

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}

function showDetailEditor(row) {
	var tableDatacell = document.getElementById('value' + row);
	var textBoxValue = tableDatacell.innerText;
	tableDatacell.innerText = '';

	var textBox = document.createElement('input');
	textBox.id = 'input' + row;	
	textBox.value = textBoxValue;	
	textBox.setAttribute('originalValue', textBoxValue);
	textBox.setAttribute('style', 'width: 100%;');
	tableDatacell.appendChild(textBox);

	textBox.focus();

	showHideIcon('editRow' + row, 'display: none;');
	//showHideIcon('deleteRow' + row, 'display: none;');
	showHideIcon('saveRow' + row, 'cursor: pointer;');
	showHideIcon('undoRow' + row, 'cursor: pointer;');
	showHideIcon('cancelRow' + row, 'cursor: pointer;');
}

function saveRow(row) {
	var textBox = document.getElementById('input' + row);
	var tableDatacell = document.getElementById('value' + row);

	tableDatacell.innerText = textBox.value;

	showHideIcon('editRow' + row, 'cursor: pointer;');
	//showHideIcon('deleteRow' + row, 'cursor: pointer;');
	showHideIcon('saveRow' + row, 'display: none;');
	showHideIcon('undoRow' + row, 'display: none;');
	showHideIcon('cancelRow' + row, 'display: none;');
}

function undoRow(row) {
	var textBox = document.getElementById('input' + row);
	textBox.value = textBox.getAttribute('originalValue');
}

function cancelRow(row) {
	var textBox = document.getElementById('input' + row);
	var tableDatacell = document.getElementById('value' + row);

	tableDatacell.innerText = textBox.getAttribute('originalValue');

	showHideIcon('editRow' + row, 'cursor: pointer;');
	//showHideIcon('deleteRow' + row, 'cursor: pointer;');
	showHideIcon('saveRow' + row, 'display: none;');
	showHideIcon('undoRow' + row, 'display: none;');
	showHideIcon('cancelRow' + row, 'display: none;');
}

function addDetails(userName) {
	var table = document.createElement('table');
	table.id = 'addDetailsTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Detail'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
	table.appendChild(tableRow);

	var firstDetailTableRow = document.createElement('tr');
	for(var j = 0; j < 3; j++) {
		var tableDatacell = document.createElement('td');
		tableDatacell.setAttribute('style', 'border: solid black 1px;');

		if(j == 0) {
			tableDatacell.innerHTML = 'Select Detail Type';
		}
		else if(j == 1) {
			tableDatacell.innerHTML = 'Enter/Select Detail Value';
		}
		else {
			tableDatacell.innerHTML = 'Add/Delete Icon';
		}

		firstDetailTableRow.appendChild(tableDatacell);
	}
	table.appendChild(firstDetailTableRow);

	xdialog.confirm(table.outerHTML, function() {}, 
	{
		style: 'width:50%;font-size:0.8rem;',
		buttons: {
			ok: {
				text: 'Save & Close',
				style: 'background: Green;'
			}
		},
		title: 'Add Details For '.concat(userName)
	});
}

function addRoles(userName) {
	var table = document.createElement('table');
	table.id = 'addRolesTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Role'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
	table.appendChild(tableRow);

	var firstRoleTableRow = document.createElement('tr');
	for(var j = 0; j < 3; j++) {
		var tableDatacell = document.createElement('td');
		tableDatacell.setAttribute('style', 'border: solid black 1px;');

		if(j == 0) {
			tableDatacell.innerHTML = 'Select Role Type';
		}
		else if(j == 1) {
			tableDatacell.innerHTML = 'Enter/Select Role Value';
		}
		else {
			tableDatacell.innerHTML = 'Add/Delete Icon';
		}

		firstRoleTableRow.appendChild(tableDatacell);
	}
	table.appendChild(firstRoleTableRow);

	xdialog.confirm(table.outerHTML, function() {}, 
	{
		style: 'width:50%;font-size:0.8rem;',
		buttons: {
			ok: {
				text: 'Save & Close',
				style: 'background: Green;'
			}
		},
		title: 'Add Roles For '.concat(userName)
	});
}