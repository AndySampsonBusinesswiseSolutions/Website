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

	createCard(guid, newDiv, 'Customer');

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
	var customer;
	
	var customerLength = data.length;
	for(var i = 0; i < customerLength; i++) {
		customer = data[i];

		if(customer.GUID == guid) {
			break;
		}
	}

	buildCardView(customer, divToAppendTo);
	buildCustomerDataTable(customer, identifier, divToAppendTo);
	buildChildCustomerDataTable(customer, identifier, divToAppendTo);
}

function buildCardView(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'cardView';
	div.setAttribute('class', 'group-div');

	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	table.appendChild(createTableHeader('width: 50%', ''));
	table.appendChild(createTableHeader('width: 50%', ''));

	var cardViewAttributes = customerCardViewAttributes;
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
	addDetailsButton.setAttribute('onclick', 'addDetails("' + entity.Attributes[0]["CustomerName"] + '")');
	addDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(addDetailsButton);	

	var editDetailsButton = document.createElement('button');
	editDetailsButton.id = 'editDetailsButton';
	editDetailsButton.innerHTML = 'Edit Details';
	editDetailsButton.setAttribute('onclick', 'displayCustomerDataTable()');
	editDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editDetailsButton);	

	var addChildCustomersButton = document.createElement('button');
	addChildCustomersButton.id = 'addChildCustomersButton';
	addChildCustomersButton.innerHTML = 'Add Child Customers';
	addChildCustomersButton.setAttribute('onclick', 'addChildCustomers("' + entity.Attributes[0]["CustomerName"] + '")');
	addChildCustomersButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(addChildCustomersButton);	

	var editChildCustomersButton = document.createElement('button');
	editChildCustomersButton.id = 'editChildCustomersButton';
	editChildCustomersButton.innerHTML = 'Edit Child Customers';
	editChildCustomersButton.setAttribute('onclick', 'displayChildCustomerDataTable()');
	editChildCustomersButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editChildCustomersButton);	
}

function displayCustomerDataTable() {
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

function buildCustomerDataTable(entity, attributeRequired, divToAppendTo){
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
	displayAttributes(getAttribute(entity.Attributes, attributeRequired), entity.Attributes, table, 'Customer');

	treeDiv.appendChild(table);
}

function displayChildCustomerDataTable() {
	var button = document.getElementById('editChildCustomersButton');
	var div = document.getElementById('displayChildCustomers');

	if(button.innerHTML == 'Edit Child Customers') {
		div.setAttribute('style', '');
		button.innerText = 'Hide Child Customers';
	}
	else {
		div.setAttribute('style', 'display: none');
		button.innerText = 'Edit Child Customers'
	}
}

function buildChildCustomerDataTable(entity, attributeRequired, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayChildCustomers';
	div.setAttribute('style', 'display: none');
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayChildCustomers');
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
	displayAttributes(getAttribute(entity.ChildCustomers, attributeRequired), entity.ChildCustomers, table, 'ChildCustomer');

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

function addDetails(customerName) {
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
		title: 'Add Details For '.concat(customerName)
	});
}

function addChildCustomers(customerName) {
	var table = document.createElement('table');
	table.id = 'addChildCustomersTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Child Customer'));
	table.appendChild(tableRow);

	var firstChildCustomerTableRow = document.createElement('tr');
	var tableDatacell = document.createElement('td');
	tableDatacell.setAttribute('style', 'border: solid black 1px;');
	tableDatacell.innerHTML = 'Select Child Customer';
	firstChildCustomerTableRow.appendChild(tableDatacell);
	table.appendChild(firstChildCustomerTableRow);

	xdialog.confirm(table.outerHTML, function() {}, 
	{
		style: 'width:50%;font-size:0.8rem;',
		buttons: {
			ok: {
				text: 'Save & Close',
				style: 'background: Green;'
			}
		},
		title: 'Add Child Customers For '.concat(customerName)
	});
}