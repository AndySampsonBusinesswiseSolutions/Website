var siteCardViewAttributes = [
	"Address Line 1",
	"Address Line 2",
	"Address Line 3",
	"Address Line 4",
	"Postcode",
	"Contact Name",
	"Contact Telephone Number",
	"Sq. ft"
]

function openTab(evt, tabName, guid, branch) {
	var i, tabcontent, tablinks;
	var cardDiv = document.getElementById('cardDiv');

	tabcontent = document.getElementsByClassName("tabcontent");
	for (i = 0; i < tabcontent.length; i++) {
	  cardDiv.removeChild(tabcontent[i]);
	}
	tablinks = document.getElementsByClassName("tablinks");
	for (i = 0; i < tablinks.length; i++) {
	  tablinks[i].className = tablinks[i].className.replace(" active", "");
	}
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName;
	cardDiv.appendChild(newDiv);
	
	switch(branch) {
		case "Site":
			createCard(guid, newDiv, 'Site', 'SiteName');
			break;
		case "Meter":
			createCard(guid, newDiv, 'Meter', 'Identifier');
			break;
		case "SubMeter":
			createCard(guid, newDiv, 'SubMeter', 'Identifier');
			break;
	}

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
			button.innerHTML = checkbox.getAttribute('linkedsite').concat(' - ').concat(span.innerHTML);
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
    tabDiv.children[0].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDiv.children.length).concat('px;'));
    for(var i = 1; i < tabDiv.children.length; i++) {
        tabDiv.children[i].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDiv.children.length).concat('px; border-left: solid black 1px;'));
    }
}

function createCard(guid, divToAppendTo, type, identifier) {
    var site = getEntityByGUID(guid, type);

	buildCardView(type, site, divToAppendTo);
    buildDataTable(type, site, identifier, divToAppendTo);
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

	var cardViewAttributes;
	switch(type) {
		case 'Site':
			cardViewAttributes = siteCardViewAttributes;
			break;
		case 'Meter':
			break;
		case 'SubMeter':
			break;
	}

	for(var i = 0; i < cardViewAttributes.length; i++) {
		var tableRow = document.createElement('tr');
		var tableDatacellAttribute = document.createElement('td');
		var tableDatacellAttributeValue = document.createElement('td');

		tableDatacellAttribute.innerHTML = cardViewAttributes[i];
		tableDatacellAttributeValue.innerHTML = getAttribute(entity.Attributes, cardViewAttributes[i]) || '';

		tableRow.appendChild(tableDatacellAttribute);
		tableRow.appendChild(tableDatacellAttributeValue);

		if(i == 0) {
			var tableDatacellMap = document.createElement('td');
			tableDatacellMap.setAttribute('rowspan', cardViewAttributes.length);

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

	for(var i = 0; i < cardViewAttributes.length; i++) {
		if(cardViewAttributes[i].includes('Address Line')
			|| cardViewAttributes[i].includes('Postcode')) {
				var attribute = getAttribute(entity.Attributes, cardViewAttributes[i]);

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

function buildDataTable(type, entity, attributeRequired, divToAppendTo){
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

	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border: solid black 1px;', 'Identifier'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
    displayAttributes(type, getAttribute(entity.Attributes, attributeRequired), entity.Attributes, table);

	treeDiv.appendChild(table);
}

function createTableHeader(style, value) {
	var tableHeader = document.createElement('th');
	tableHeader.setAttribute('style', style);
	tableHeader.innerHTML = value;
	return tableHeader;
}

function displayAttributes(type, identifier, attributes, table) {
	if(!attributes) {
		return;
	}

	for(var i = 0; i < attributes.length; i++) {
		var tableRow = document.createElement('tr');
		tableRow.id = 'row' + i;

		for(var j = 0; j < 4; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(j) {
				case 0:
					tableDatacell.innerHTML = type;
					break;	
				case 1:
					tableDatacell.innerHTML = identifier;
					break;
				case 2:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					
					tableDatacell.id = 'attribute' + i;
					break;
				case 3:
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

		var editIcon = createIcon('editRow' + i, 'fas fa-edit', 'cursor: pointer;', 'showDetailEditor(' + i + ')');
		var deleteIcon = createIcon('deleteRow' + i, 'fas fa-trash-alt', 'cursor: pointer;', 'deleteRow(' + i + ')');
		var saveChangeIcon = createIcon('saveRow' + i, 'fas fa-save', 'display: none;', 'saveRow(' + i + ')');
		var undoChangeIcon = createIcon('undoRow' + i, 'fas fa-undo', 'display: none;', 'undoRow(' + i + ')');
		var cancelChangeIcon = createIcon('cancelRow' + i, 'far fa-window-close', 'display: none;', 'cancelRow(' + i + ')');

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