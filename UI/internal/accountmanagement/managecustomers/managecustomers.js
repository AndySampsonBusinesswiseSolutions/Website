function pageLoad() {    
	createTree(managecustomers, "customerTree", "createCardButton");
}

function resetPage() {
	pageLoad();

	var inputs = siteSelectorList.getElementsByTagName('input');
	for(var i = 0; i < inputs.length; i++) {
		createCardButton(inputs[i]);
	}
}

function openTab(callingElement, tabName, guid) {
	var tabLinks = document.getElementsByClassName("tablinks");
	var tabLinksLength = tabLinks.length;
	for (var i = 0; i < tabLinksLength; i++) {
		tabLinks[i].className = tabLinks[i].className.replace(" active", "");
	}
	callingElement.className += " active";
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'card');
	newDiv.id = tabName;

	var cardDiv = document.getElementById('cardDiv');
	clearElement(cardDiv);
	cardDiv.appendChild(newDiv);
	
	createCard(guid, newDiv);

	newDiv.style.display = "block";
  }

function createCardButton(checkbox){
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));
	var button = document.getElementById(span.id.replace('span', 'button'));

	if(checkbox.checked){
		if(!button) {
			cardDiv.setAttribute('style', '');
			var button = document.createElement('button');
			button.setAttribute('class', 'tablinks');
			button.setAttribute('onclick', 'openTab(this, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '")');

			if(checkbox.getAttribute('branch') == "ChildCustomer") {
				var parentCustomerNode = span.parentNode.parentNode.parentNode.parentNode.children[3];

				button.innerHTML = parentCustomerNode.innerText.concat(' - ').concat(span.innerHTML);
			}
			else {
				button.innerHTML = span.innerHTML;
			}

			if(!span.innerText.includes('Add New')) {
				button.innerHTML += '<i class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add ' + span.innerText + ' To Download Basket"></i>'
				+ '<i class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download ' + span.innerText + '"></i>';
			}
			
			button.id = span.id.replace('span', 'button');
			tabDiv.appendChild(button);
		}
	}
	else {
		if(button) {
			tabDiv.removeChild(button);
		}
	}	

	updateTabDiv();
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

	if(tabDivChildrenLength == 0) {
		document.getElementById('Customer0checkbox').checked = true;
		createCardButton(document.getElementById('Customer0checkbox'));
		openTab(document.getElementById('Customer0button'), 'Customer0div', '0');
	}
	else {
		var percentage = (1 / tabDivChildrenLength) * 100;
		tabDivChildren[0].setAttribute('style', 'width: '.concat(percentage).concat('%;'));
		for(var i = 1; i < tabDivChildrenLength; i++) {
			tabDivChildren[i].setAttribute('style', 'width: '.concat(percentage).concat('%; border-left: solid black 1px;'));
		}
		
		tabDiv.style.display = '';

		for(var i = 0; i < tabDivChildrenLength; i++) {
			if(hasClass(tabDivChildren[i], 'active')) {
				return;
			}
		}

		var lastChild = tabDivChildren[i - 1];
		lastChild.className += " active";
		lastChild.dispatchEvent(new Event('click'));
	}
}

function createCard(guid, divToAppendTo) {
	var customer = getEntityByGUID(guid);	

	buildCardView(customer, divToAppendTo);
	buildCustomerDataTable(customer, divToAppendTo);
	buildChildCustomerDataTable(customer, divToAppendTo);
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
		"Address Lines",
        "Address Town",
        "Address County",
		"Address PostCode",
		"Contact Name",
		"Contact Telephone Number",
		"Contact Email Address"
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

	var editDetailsButton = document.createElement('button');
	editDetailsButton.id = 'editDetailsButton';
	editDetailsButton.innerHTML = 'Edit Details';
	editDetailsButton.setAttribute('onclick', 'displayCustomerDataTable()');
	editDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editDetailsButton);	

	var editChildCustomersButton = document.createElement('button');
	editChildCustomersButton.id = 'editChildCustomersButton';
	editChildCustomersButton.innerHTML = 'Edit Child Customers';
	editChildCustomersButton.setAttribute('onclick', 'displayChildCustomerDataTable()');
	editChildCustomersButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editChildCustomersButton);
	
	var name = getAttribute(entity.Attributes, 'Customer Name');
	if(name && name.startsWith('Add New')) {
		var addNewbutton = document.createElement('button');
		addNewbutton.id = 'addNewButton';
		addNewbutton.innerHTML = 'Save New Customer';
		addNewbutton.setAttribute('style', 'margin-left: 10px;');
		addNewbutton.setAttribute('onclick', 'saveNewCustomer()')
		divToAppendTo.appendChild(addNewbutton);
	}
}

function saveNewCustomer() {
	showLoader(true);

	var customerData = [];
	var customerAttributeDiv = document.getElementById('displayAttributes');
	var table = customerAttributeDiv.children[0];
	for (var i = 1, row; row = table.rows[i]; i++) {
		var attributeValue = row.children[1].innerText;
		var valueValue = row.children[2].innerText;
		var record = {attribute: attributeValue, value:valueValue};
		customerData.push(record);
	}

	var childCustomerData = [];
	var childCustomerDiv = document.getElementById('displayChildCustomers');
	var table = childCustomerDiv.children[0];
	for (var i = 1, row; row = table.rows[i]; i++) {
		var attributeValue = row.children[1].innerText;
		var record = {childCustomerName: attributeValue};
		customerData.push(record);
	}

	var processQueueGUID = CreateGUID();
	var customerGUID = CreateGUID();

	var postSuccessful = postData(
		{
			ProcessQueueGUID: processQueueGUID, 
			PageGUID: "80B1CC99-7C91-4D07-A541-9D69AC4CC304", 
			ProcessGUID: "D39E768A-D06D-4EB3-80E3-895EDC556A6B",
			CustomerGUID: customerGUID,
			CustomerData: JSON.stringify(customerData),
			ChildCustomerData: JSON.stringify(childCustomerData)
		}
	);

	if(postSuccessful) {
        getProcessResponse(processQueueGUID)
        .then(response => {
          processResponse(response, processQueueGUID);
        })
	}
	else {
        processResponse(null);
	}
}

async function getProcessResponseDetail(data) {
	try {
	  const response = await fetch(uri + '/GetProcessResponseDetail', {
		method: 'POST',
		mode: 'cors',
		cache: 'no-cache',
		credentials: 'same-origin',
		headers: {
		  'Content-Type': 'application/json',
		},
		redirect: 'follow',
		referrerPolicy: 'no-referrer',
		body: JSON.stringify(data)
	  });
	
	  return response.json();
	}
	catch {
	  return null;
	}
  }

async function processResponse(response, processQueueGUID) {
	if(response) {
	  if(response.message == "OK") {
		location.reload();
	  }
	  else {
			//Get response from GetProcessResponseDetail
		  	var responseDetail = await getProcessResponseDetail({ProcessQueueGUID:processQueueGUID, APIGUID:"1B2E2BA3-D538-47E0-9044-BBBFC6BF3892"});
			if(responseDetail.message == "Success") {
				alert('Customer has been created but some attributes were not saved');
				showLoader(false);
				location.reload();
			}
			else {
				alert('Customer has not been created');
				showLoader(false);
			}
			//This needs to be an in-page error rather than alert
	  }
	}
	else {
		//display alert
	}
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

function buildCustomerDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('style', 'display: none');
	divToAppendTo.appendChild(div);
	
	var customerTree = document.getElementById('displayAttributes');
	clearElement(customerTree);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 15%; border: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(entity.Attributes, table, 'Customer');

	customerTree.appendChild(table);
}

function displayChildCustomerDataTable() {
	var button = document.getElementById('editChildCustomersButton');
	var div = document.getElementById('displayChildCustomers');

	if(button.innerHTML == 'Edit Child Customers') {
		div.setAttribute('style', 'margin-top: 5px;');
		button.innerText = 'Hide Child Customers';
	}
	else {
		div.setAttribute('style', 'margin-top: 5px; display: none');
		button.innerText = 'Edit Child Customers'
	}
}

function buildChildCustomerDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayChildCustomers';
	div.setAttribute('style', 'margin-top: 5px; display: none');
	divToAppendTo.appendChild(div);
	
	var customerTree = document.getElementById('displayChildCustomers');
	clearElement(customerTree);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 15%; border: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(entity.ChildCustomers, table, 'ChildCustomer');

	customerTree.appendChild(table);
}

function displayAttributes(attributes, table, type) {
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
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					
					tableDatacell.id = 'attribute'.concat(type + i);
					break;
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
		var deleteIcon = createIcon('deleteRow' + type + i, 'fas fa-trash-alt', 'cursor: pointer;', 'deleteRow("' + type + i + '")');
		var saveChangeIcon = createIcon('saveRow' + type + i, 'fas fa-save', 'display: none;', 'saveRow("' + type + i + '")', 'Save');
		var undoChangeIcon = createIcon('undoRow' + type + i, 'fas fa-undo', 'display: none;', 'undoRow("' + type + i + '")', 'Undo');
		var cancelChangeIcon = createIcon('cancelRow' + type + i, 'far fa-window-close', 'display: none;', 'cancelRow("' + type + i + '")', 'Cancel');

		tableDatacell.appendChild(editIcon);
		tableDatacell.appendChild(deleteIcon);
		tableDatacell.appendChild(saveChangeIcon);
		tableDatacell.appendChild(undoChangeIcon);
		tableDatacell.appendChild(cancelChangeIcon);

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	

	var tableRow = document.createElement('tr');
	tableRow.id = 'row' + i;

	var tableDatacell = document.createElement('td');
	tableDatacell.setAttribute('style', 'border: solid black 1px;');
	tableDatacell.innerHTML = type;
	tableRow.appendChild(tableDatacell);

	var attributeTableDatacell = document.createElement('td');
	attributeTableDatacell.id = 'attribute' + i;
	attributeTableDatacell.setAttribute('style', 'border: solid black 1px;');
	attributeTableDatacell.innerHTML = '<select style="width: 100%;"></select>'
	tableRow.appendChild(attributeTableDatacell);

	var inputTableDatacell = document.createElement('td');
	inputTableDatacell.id = 'value' + i;
	inputTableDatacell.setAttribute('style', 'border: solid black 1px;');
	inputTableDatacell.innerHTML = '<input style="width: 100%;"></input>'
	tableRow.appendChild(inputTableDatacell);

	var saveChangeIcon = createIcon('saveRow' + i, 'fas fa-save', 'cursor: pointer;', 'saveRow(' + i + ')', 'Save');
	var saveTableDatacell = document.createElement('td');
	saveTableDatacell.setAttribute('style', 'border: solid black 1px;');
	saveTableDatacell.appendChild(saveChangeIcon);
	tableRow.appendChild(saveTableDatacell);

	table.appendChild(tableRow);
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

	showHideIcon('editRow' + row, 'display: none');
	showHideIcon('deleteRow' + row, 'display: none');
	showHideIcon('saveRow' + row, 'cursor: pointer');
	showHideIcon('undoRow' + row, 'cursor: pointer');
	showHideIcon('cancelRow' + row, 'cursor: pointer');
}

function deleteRow(row) {
	var attribute = document.getElementById('attribute' + row);
	var value = document.getElementById('value' + row);

	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');

	text.innerText = "Are you sure you want to delete the '" + attribute.innerText + "' attribute with current value of '" + value.innerText + "'?";

    finalisePopup(title, 'Delete Attribute?<br><br>', modal, span);
}

function saveRow(row) {
	var textBox = document.getElementById('input' + row);
	var tableDatacell = document.getElementById('value' + row);

	tableDatacell.innerText = textBox.value;

	showHideIcon('editRow' + row, 'cursor: pointer');
	showHideIcon('deleteRow' + row, 'cursor: pointer');
	showHideIcon('saveRow' + row, 'display: none');
	showHideIcon('undoRow' + row, 'display: none');
	showHideIcon('cancelRow' + row, 'display: none');
}

function undoRow(row) {
	var textBox = document.getElementById('input' + row);
	textBox.value = textBox.getAttribute('originalValue');
}

function cancelRow(row) {
	var textBox = document.getElementById('input' + row);
	var tableDatacell = document.getElementById('value' + row);

	tableDatacell.innerText = textBox.getAttribute('originalValue');

	showHideIcon('editRow' + row, 'cursor: pointer');
	showHideIcon('deleteRow' + row, 'cursor: pointer');
	showHideIcon('saveRow' + row, 'display: none');
	showHideIcon('undoRow' + row, 'display: none');
	showHideIcon('cancelRow' + row, 'display: none');
}

function deleteCustomers() {
	var customers = getCustomers().join("<br>");

	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');
	var button = document.getElementById('button');

	button.innerText = "Delete Customer(s)";
	text.innerHTML = "Are you sure you want to delete the following customers?<br>" + customers;

    finalisePopup(title, 'Delete Customer?<br><br>', modal, span);
}

function reinstateCustomers() {
	var customers = getCustomers();

	var modal = document.getElementById("popup");
	var title = document.getElementById("title");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById('text');
	var button = document.getElementById('button');

	button.innerText = "Reinstate Customer(s)";
	button.classList.replace('reject', 'approve');

	text.innerHTML = "Please select customers to reinstate:<br>";
	customers.forEach(function(customer, i) {
		var checkbox = document.createElement('input');
		checkbox.type = 'checkbox';
		checkbox.id = 'reinstateCustomer' + i + 'checkbox';
		checkbox.setAttribute('customerValue', customer);

		text.innerHTML += checkbox.outerHTML + customer + '<br>';
	});

    finalisePopup(title, 'Reinstate Customer?<br><br>', modal, span);
}

function getCustomers() {
	var inputs = document.getElementsByTagName('input');
	var inputLength = inputs.length;
	var customers = [];

	for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];

		if(input.type.toLowerCase() == 'checkbox' && input.checked) {
			var span = document.getElementById(input.id.replace('checkbox', 'span'));

			if(!span.innerText.startsWith('Add New')) {
				var button = document.getElementById(input.id.replace('checkbox', 'button'));
				customers.push(button.innerText);
			}
		}
	}

	return customers;
}

var branchCount = 0;
var subBranchCount = 0;

async function getTree() {
	var processQueueGUID = CreateGUID();
	var data = {ProcessQueueGUID: processQueueGUID, CustomerGUID: CreateGUID(), ProcessGUID: 'DC33F3E1-B925-4EB7-B2B2-74ED3DD828A1'};
	var postSuccessful = postData(data);

	if(postSuccessful) {
		var response = await getProcessResponse(processQueueGUID);
		return await processTreeResponse(response, processQueueGUID);;
	}
}

async function processTreeResponse(response, processQueueGUID) {
	if(response) {
	  if(response.message == "OK") {
		var result = await getPageRequestResult(processQueueGUID);
		return result;
	  }
	}
}

async function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
	var treeResponse = await getTree();
	tree.innerHTML = treeResponse.message;

    var div = document.getElementById(divId);
    clearElement(div);

	var headerDiv = createHeaderDiv("siteHeader", 'Customers', true);
    div.appendChild(headerDiv);
	div.appendChild(tree);
	
	document.getElementById('Customer0checkbox').checked = true;
	createCardButton(document.getElementById('Customer0checkbox'));
	openTab(document.getElementById('Customer0button'), 'Customer0button', '0');

	addExpanderOnClickEvents();
	setOpenExpanders();
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
		var base = baseData[i];
		var isChildCustomer = getAttribute(base.Attributes, 'IsChildCustomer');

		if(!isChildCustomer) {
			var baseName = getAttribute(base.Attributes, 'Customer Name');
			var li = document.createElement('li');
			var ul = createUL();
	
			buildChildCustomer(base.ChildCustomers, ul, checkboxFunction, baseName);
			appendListItemChildren(li, 'Customer'.concat(base.GUID), checkboxFunction, 'Customer', baseName, ul, baseName, base.GUID, base.ChildCustomers.length > 0);
	
			baseElement.appendChild(li); 
		}    
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

        appendListItemChildren(li, 'ChildCustomer'.concat(branchCount), checkboxFunction, 'ChildCustomer', childCustomer["Customer Name"], ul, linkedSite, '', hasChildren);

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

function getEntityByGUID(guid) {
	var dataLength = managecustomers.length;
	for(var i = 0; i < dataLength; i++) {
		var entity = managecustomers[i];
		if(entity.GUID == guid) {
			return entity;
		}
	}
	
	return null;
}