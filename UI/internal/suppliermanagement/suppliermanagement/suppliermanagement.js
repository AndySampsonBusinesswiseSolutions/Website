function pageLoad() {
    createTree(supplier, "treeDiv", "createCardButton");
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
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();

        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, ul, baseName, base.GUID);

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

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
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
	return 'fas fa-map-marker-alt';
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

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
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
	
	createCard(guid, newDiv, 'Supplier');

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

		if(checkbox.getAttribute('branch') == "SubMeter") {
			button.innerHTML = checkbox.parentNode.parentNode.parentNode.parentNode.children[3].innerText.concat(' - ').concat(span.innerHTML);
		}
		else {
			button.innerHTML = span.innerHTML;
		}
		
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

function createCard(guid, divToAppendTo, type) {
    var site = getEntityByGUID(guid, type);

	buildCardView(type, site, divToAppendTo);
    buildDataTable(site, divToAppendTo);
}

function buildCardView(type, entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'cardView';
	div.setAttribute('class', 'group-div');

	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	table.appendChild(createTableHeader('width: 30%', ''));
	table.appendChild(createTableHeader('width: 30%', ''));
	table.appendChild(createTableHeader('width: 250px', ''));

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

		if(i == 0) {
			var tableDatacellMap = document.createElement('td');
			tableDatacellMap.setAttribute('rowspan', cardViewAttributesLength);

			var mapDiv = document.createElement('div');
			mapDiv.id = 'map-canvas';
			mapDiv.setAttribute('style', 'height: 250px;')

			tableDatacellMap.appendChild(mapDiv);
			tableRow.appendChild(tableDatacellMap);
		}

		table.appendChild(tableRow);
	}

	div.appendChild(table);
	divToAppendTo.appendChild(div);

	var button = document.createElement('button');
	button.id = 'editDetailsButton';
	button.innerHTML = 'Edit Details';
	button.setAttribute('onclick', 'displayDataTable()');
	divToAppendTo.appendChild(button);	

	divToAppendTo.appendChild(document.createElement('br'));
	divToAppendTo.appendChild(document.createElement('br'));

	var address = getAddress(cardViewAttributes, entity);

	if(address) {
		//initializeMap(address);
	}
}

function getAddress(cardViewAttributes, entity) {
	var addressDetails = [];
	var cardViewAttributesLength = cardViewAttributes.length;
	var entityAttributes = entity.Attributes;

	for(var i = 0; i < cardViewAttributesLength; i++) {
		var cardViewAttribute = cardViewAttributes[i];

		if(cardViewAttribute.includes('Address Line')
			|| cardViewAttribute.includes('Postcode')) {
				var attribute = getAttribute(entityAttributes, cardViewAttribute);

				if(attribute != '') {
					addressDetails.push(attribute);
				}				
			}
	}

	return addressDetails.join(',');
}

function displayDataTable() {
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

function buildDataTable(entity, divToAppendTo){
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

	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
    displayAttributes(entity.Attributes, table);

	treeDiv.appendChild(table);
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
	showHideIcon('deleteRow' + row, 'display: none;');
	showHideIcon('saveRow' + row, 'cursor: pointer;');
	showHideIcon('undoRow' + row, 'cursor: pointer;');
	showHideIcon('cancelRow' + row, 'cursor: pointer;');
}

function deleteRow(row) {
	var attribute = document.getElementById('attribute' + row);

	xdialog.confirm('Are you sure you want to delete ' + attribute.innerText + '?', function() {
		var table = document.getElementById('dataTable');
		var tableRow = document.getElementById('row' + row);

		table.removeChild(tableRow);
	  }, {
		style: 'width:420px;font-size:0.8rem;',
		buttons: {
			ok: {
				text: 'Delete ' + attribute.innerText,
				style: 'background: red;',
			},
			cancel: {
				text: 'Cancel',
				style: 'background: Green;',
			}
		}
	  });
}

function saveRow(row) {
	var textBox = document.getElementById('input' + row);
	var tableDatacell = document.getElementById('value' + row);

	tableDatacell.innerText = textBox.value;

	showHideIcon('editRow' + row, 'cursor: pointer;');
	showHideIcon('deleteRow' + row, 'cursor: pointer;');
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
	showHideIcon('deleteRow' + row, 'cursor: pointer;');
	showHideIcon('saveRow' + row, 'display: none;');
	showHideIcon('undoRow' + row, 'display: none;');
	showHideIcon('cancelRow' + row, 'display: none;');
}

function getEntityByGUID(guid, type) {
	var dataLength = supplier.length;
	for(var i = 0; i < dataLength; i++) {
		var site = supplier[i];
		if(type == 'Site' || type == 'Supplier') {
			if(site.GUID == guid) {
				return site;
			}
		}
        else {
			var meterLength = site.Meters.length;
			for(var j = 0; j < meterLength; j++) {
				var meter = site.Meters[j];
				if(type = 'Meter') {
					if(meter.GUID == guid) {
						return meter;
					}
					else {
						var subMeters = meter.SubMeters;
						if(subMeters) {
							var subMetersLength = subMeters.length;
							for(var k = 0; k < subMetersLength; k++) {
								var subMeter = subMeters[k];
								if(subMeter.GUID == guid) {
									return subMeter;
								}
							}
						}
					}
				}
			}
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