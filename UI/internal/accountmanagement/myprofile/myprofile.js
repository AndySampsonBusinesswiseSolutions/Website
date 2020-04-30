function pageLoad() {    
	createTree(myprofile, "sectionTree", "createCardButton");
	
	document.onmousemove = function(e) {
		setupSidebarHeight();
		setupSidebar(e);
	};

	window.onscroll = function() {
		setupSidebarHeight();
	};
}

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
	var headerDiv = createHeaderDiv("sectionHeader", 'Sections', true);
	var ul = createBranchUl("sectionSelector", false, true);
	  
    tree.appendChild(ul);

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
	div.appendChild(tree);
	
	for(var i = 0; i < 3; i++) {
		document.getElementById('User' + i +'checkbox').checked = true;
		createCardButton(document.getElementById('User' + i +'checkbox'));
	}

	openTab(document.getElementById('User0button'), 'User0button', '0');

	addExpanderOnClickEvents();
	setOpenExpanders();
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'UserName');
        var li = document.createElement('li');
        var ul = createUL();

        appendListItemChildren(li, 'User'.concat(base.GUID), checkboxFunction, 'User', baseName, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon());
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

function createTreeIcon() {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch());
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

function getIconByBranch() {
	return 'fas fa-user';
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
			button.innerHTML = span.innerHTML;

			if(!span.innerText.includes('Change Password')) {
				button.innerHTML += '<div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add ' + span.innerText + ' To Download Basket"></div>'
				+ '<div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download ' + span.innerText + '"></div>';
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
		document.getElementById('User2checkbox').checked = true;
		createCardButton(document.getElementById('User2checkbox'));
		openTab(document.getElementById('User2button'), 'User2div', '2');
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
	var user;
	
	var userLength = myprofile.length;
	for(var i = 0; i < userLength; i++) {
		user = myprofile[i];

		if(user.GUID == guid) {
			break;
		}
	}

	if(getAttribute(user.Attributes, 'UserName') == 'Change Password') {
		buildChangePasswordForm(divToAppendTo);
	}
	else {
		buildUserDataTable(user, divToAppendTo);
	}
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

function buildChangePasswordForm(divToAppendTo) {
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('style', 'text-align: center;');
	divToAppendTo.appendChild(div);
	clearElement(div);

	var html = 
			'<div style="padding-left: 38px;">'+
				'<label for="currentPasswordInput">Current Password:</label>'+
				'<input type="password" id="currentPasswordInput" style="margin-left: 5px;"></input>'+
			'</div>'+
			'<div style="margin-top: 5px; padding-left: 62px;">'+
				'<label for="newPasswordInput">New Password:</label>'+
				'<input type="password" id="newPasswordInput" style="margin-left: 5px;"></input>'+
			'</div>'+
			'<div style="margin-top: 5px;">'+
				'<label for="confirmNewPasswordInput">Confirm New Password:</label>'+
				'<input type="password" id="confirmNewPasswordInput" style="margin-left: 5px;"></input>'+
			'</div>'+
			'<div style="margin-top: 5px;">'+
				'<button id="changePasswordButton" style="width: 25%;" class="show-pointer">Change Password</button>'+
			'</div>';

	div.innerHTML = html;
}

function buildUserDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var sectionTree = document.getElementById('displayAttributes');
	clearElement(sectionTree);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(entity.Details, table, 'User');

	sectionTree.appendChild(table);
}

function displayAttributes(attributes, table) {
	if(!attributes) {
		return;
	}

	var attributesLength = attributes.length;
	for(var i = 0; i < attributesLength; i++) {
		var tableRow = document.createElement('tr');
		tableRow.id = 'row' + i;

		for(var j = 0; j < 2; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(j) {
				case 0:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					
					tableDatacell.id = 'attribute' + i;
					break;
				case 1:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}

					tableDatacell.id = 'value' + i;
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		var tableDatacell = document.createElement('td');
		tableDatacell.setAttribute('style', 'border: solid black 1px;');

		var editIcon = createIcon('editRow' + i, 'fas fa-edit', 'cursor: pointer;', 'showDetailEditor(' + i + ')', 'Edit');
		var deleteIcon = createIcon('deleteRow' + i, 'fas fa-trash-alt', 'cursor: pointer;', 'deleteRow(' + i + ')', 'Delete');
		var saveChangeIcon = createIcon('saveRow' + i, 'fas fa-save', 'display: none;', 'saveRow(' + i + ')', 'Save');
		var undoChangeIcon = createIcon('undoRow' + i, 'fas fa-undo', 'display: none;', 'undoRow(' + i + ')', 'Undo');
		var cancelChangeIcon = createIcon('cancelRow' + i, 'far fa-window-close', 'display: none;', 'cancelRow(' + i + ')', 'Cancel');

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
	tableDatacell.id = 'attribute' + i;
	tableDatacell.setAttribute('style', 'border: solid black 1px;');
	tableDatacell.innerHTML = '<select style="width: 100%;"><option value=""></option><option value="Attribute 1">Attribute 1</option><option value="Attribute 2">Attribute 2</option></select>'
	tableRow.appendChild(tableDatacell);

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

	var modal = document.getElementById("deleteRowPopup");
	var title = document.getElementById("deleteRowTitle");
	var span = modal.getElementsByClassName("close")[0];
	var deleteRowText = document.getElementById('deleteRowText');

	deleteRowText.innerText = "Are you sure you want to delete the '" + attribute.innerText + "' attribute with current value of '" + value.innerText + "'?";

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

function showHideIcon(id, style) {
	var element = document.getElementById(id);
	element.setAttribute('style', style);
}

function getEntityByGUID(guid, type) {
	var dataLength = myprofile.length;
	for(var i = 0; i < dataLength; i++) {
		var entity = myprofile[i];
		if(entity.GUID == guid) {
			return entity;
		}
	}
	
	return null;
}

function createTableHeader(style, value) {
	var tableHeader = document.createElement('th');

	if(style != '') {
		tableHeader.setAttribute('style', style);
	}
	
	tableHeader.innerHTML = value;
	return tableHeader;
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