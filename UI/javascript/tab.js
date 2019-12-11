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
		cardDiv.removeChild(document.getElementById(span.id.replace('span', 'div')));

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

    buildDataTable(type, site, identifier, divToAppendTo);
}

function buildDataTable(type, entity, attributeRequired, divToAppendTo){
	var div = document.createElement('div');
    div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.setAttribute('style', 'border-bottom: solid black 1px;');

	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border-right: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border-right: solid black 1px;', 'Identifier'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 30%; border-right: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; border-right: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%;', ''));

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

		for(var j = 0; j < 4; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border-right: solid black 1px;');

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
					break;
				case 3:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		var tableDatacell = document.createElement('td');

		var editIcon = document.createElement('i');
		editIcon.setAttribute('class', 'fas fa-edit');

		var deleteIcon = document.createElement('i');
		deleteIcon.setAttribute('class', 'fas fa-trash-alt');

		tableDatacell.appendChild(editIcon);
		tableDatacell.appendChild(deleteIcon);

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}