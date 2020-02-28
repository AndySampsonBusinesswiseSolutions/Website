function pageLoad() {
	createTree(managecustomers, "treeDiv", "createCardButton");
	addExpanderOnClickEvents();
}

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
	
	var customerLength = managecustomers.length;
	for(var i = 0; i < customerLength; i++) {
		customer = managecustomers[i];

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

	var cardViewAttributes = [
		"Address Line 1",
		"Address Line 2",
		"Address Line 3",
		"Address Line 4",
		"Postcode",
		"Contact Name",
		"Contact Telephone Number",
		"Email"
	];
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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'CustomerName');
        var li = document.createElement('li');
        var ul = createUL();

        buildChildCustomer(base.ChildCustomers, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, ul, baseName, base.GUID, base.ChildCustomers.length > 0);

        baseElement.appendChild(li);        
    }
}

function buildChildCustomer(childCustomers, baseElement, checkboxFunction, linkedSite) {
    var childCustomersLength = childCustomers.length;
    for(var i = 0; i < childCustomersLength; i++) {
        var childCustomer = childCustomers[i];
        var li = document.createElement('li');
        var ul = createUL();

        var hasChildren = false;
        if(childCustomer.childCustomers) {
            hasChildren = childCustomer.ChildCustomers.length > 0;
        }

        appendListItemChildren(li, 'ChildCustomer'.concat(branchCount), checkboxFunction, 'ChildCustomer', childCustomer.CustomerName, ul, linkedSite, '', hasChildren);

        baseElement.appendChild(li);
        branchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid, hasChildren) {
    li.appendChild(createBranchDiv(id, hasChildren));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(branchOption));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function createBranchDiv(branchDivId, hasChildren) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(hasChildren) {
        branchDiv.setAttribute('class', 'far fa-plus-square');
    }
    else {
        branchDiv.setAttribute('class', 'far fa-times-circle');
    }
    
    branchDiv.setAttribute('style', 'padding-right: 4px;');
    
    return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
    var branchListDiv = document.createElement('div');
    branchListDiv.id = branchListDivId;
    branchListDiv.setAttribute('class', 'listitem-hidden');
    branchListDiv.appendChild(ul);
    return branchListDiv;
}

function createUL() {
    var ul = document.createElement('ul');
    ul.setAttribute('class', 'format-listitem');
    return ul;
}

function createTreeIcon(branch) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch));
    icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
    return icon;
}

function createSpan(spanId, innerHTML) {
    var span = document.createElement('span');
    span.id = spanId.concat('span');
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid) {
    var functionArray = checkboxFunction.replace(')', '').split('(');
    var functionArrayLength = functionArray.length;
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement('input');
    checkBox.type = 'checkbox';  
    checkBox.id = checkboxId.concat('checkbox');
    checkBox.setAttribute('Branch', branch);
    checkBox.setAttribute('LinkedSite', linkedSite);
    checkBox.setAttribute('GUID', guid);

    functionArguments.push(checkBox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function getIconByBranch(branch) {
    return 'fas fa-customer';
}

function addCustomer() {
	var div = document.createElement('div');

	var customerNameTable = document.createElement('table');
	customerNameTable.id = 'customerNameTable';
	customerNameTable.setAttribute('style', 'width: 100%;');

	var addDetailsTable = document.createElement('table');
	addDetailsTable.id = 'addDetailsTable';
	addDetailsTable.setAttribute('style', 'width: 100%;');

	var addChildCustomersTable = document.createElement('table');
	addChildCustomersTable.id = 'addChildCustomersTable';
	addChildCustomersTable.setAttribute('style', 'width: 100%;');

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

	var childCustomersTableRow = document.createElement('tr');
	childCustomersTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Child Customer'));
	childCustomersTableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	childCustomersTableRow.appendChild(createTableHeader('border: solid black 1px;', ''));
	addChildCustomersTable.appendChild(childCustomersTableRow);
	addChildCustomersTable.appendChild(document.createElement('br'));

	var addCustomerNameTableRow = document.createElement('tr');
	var addDetailsTableRow = document.createElement('tr');
	var addChildCustomerssTableRow = document.createElement('tr');

	for(var j = 0; j < 3; j++) {
		var customerNameTableDatacell = document.createElement('td');
		var detailTableDatacell = document.createElement('td');
		var childCustomerTableDatacell = document.createElement('td');

		customerNameTableDatacell.setAttribute('style', 'border: solid black 1px;');
		detailTableDatacell.setAttribute('style', 'border: solid black 1px;');
		childCustomerTableDatacell.setAttribute('style', 'border: solid black 1px;');
		
		if(j == 0) {
			customerNameTableDatacell.innerHTML = 'Customer Name';
			detailTableDatacell.innerHTML = 'Select Detail Type';
			childCustomerTableDatacell.innerHTML = 'Select Child Customer';
		}
		else if(j == 1) {
			customerNameTableDatacell.innerHTML = 'Enter Customer Name';
			detailTableDatacell.innerHTML = 'Enter/Select Detail Value';
			childCustomerTableDatacell.innerHTML = 'Enter/Select Child Customer Value';
		}
		else {
			detailTableDatacell.innerHTML = 'Add/Delete Icon';
			childCustomerTableDatacell.innerHTML = 'Add/Delete Icon';
		}

		addCustomerNameTableRow.appendChild(customerNameTableDatacell);
		addDetailsTableRow.appendChild(detailTableDatacell);
		addChildCustomerssTableRow.appendChild(childCustomerTableDatacell);
	}

	customerNameTable.appendChild(addCustomerNameTableRow);
	addDetailsTable.appendChild(addDetailsTableRow);
	addChildCustomersTable.appendChild(addChildCustomerssTableRow);

	div.appendChild(customerNameTable);
	div.appendChild(addDetailsTable);
	div.appendChild(addChildCustomersTable);

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

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

function getAttribute(attributes, attributeRequired) {
	for (var attribute in attributes) {
		var array = attributes[attribute];

		for(var key in array) {
			if(key == attributeRequired) {
				return array[key];
			}
		}
	}

	return null;
}

function showHideIcon(iconId, style) {
	var icon = document.getElementById(iconId);
	icon.setAttribute('style', style);
}

function createIcon(iconId, className, style, onClickEvent, title) {
	var icon = document.createElement('i');
	icon.id = iconId;
	icon.setAttribute('class', className);

	if(style) {
		icon.setAttribute('style', style);
	}

	if(onClickEvent) {
		icon.setAttribute('onclick', onClickEvent);
	}

	if(title) {
		icon.setAttribute('title', title);
	}

	return icon;
}

function createTableHeader(style, value) {
	var tableHeader = document.createElement('th');

	if(style != '') {
		tableHeader.setAttribute('style', style);
	}
	
	tableHeader.innerHTML = value;
	return tableHeader;
}

function updateClassOnClick(elementId, firstClass, secondClass){
	var elements = document.getElementsByClassName(elementId);

	if(elements.length == 0) {
		var element = document.getElementById(elementId);
		updateClass(element, firstClass, secondClass);
	}
	else {
		for(var i = 0; i< elements.length; i++) {
			updateClass(elements[i], firstClass, secondClass)
		}
	}
}

function updateClass(element, firstClass, secondClass)
{
	if(hasClass(element, firstClass)){
		element.classList.remove(firstClass);

		if(secondClass != ''){
			element.classList.add(secondClass);
		}
	}
	else {
		if(secondClass != ''){
			element.classList.remove(secondClass);
		}
		
		element.classList.add(firstClass);
	}
}
  
function hasClass(elem, className) {
	return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
	}
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});

	updateAdditionalControls(element);
	expandAdditionalLists(element);
}

function updateAdditionalControls(element) {
	var additionalcontrols = element.getAttribute('additionalcontrols');

	if(!additionalcontrols) {
		return;
	}

	var listToHide = element.id.concat('List');
	var clickEventFunction = function (event) {
		updateClassOnClick(listToHide, 'listitem-hidden', '')
	};

	var controlArray = additionalcontrols.split(',');
	for(var j = 0; j < controlArray.length; j++) {
		var controlId = controlArray[j];	

		element.addEventListener('click', function (event) {
			var controlElement = document.getElementById(controlId);
			if(hasClass(this, 'fa-minus-square')) {				
				controlElement.addEventListener('click', clickEventFunction, false);
			}
			else {
				controlElement.removeEventListener('click', clickEventFunction);
			}
		});
	}	
}

function expandAdditionalLists(element) {
	var additionalLists = element.getAttribute('additionallists');

	if(!additionalLists) {
		return;
	}

	element.addEventListener('click', function (event) {
		var controlArray = additionalLists.split(',');
		for(var j = 0; j < controlArray.length; j++) {
			var controlId = controlArray[j];
			var controlElement = document.getElementById(controlId);
			updateClass(controlElement, 'listitem-hidden', '');
		}
	});		
}