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

	createCard(guid, newDiv, 'Document');

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
	var document;
	
	var documentLength = data.length;
	for(var i = 0; i < documentLength; i++) {
		document = data[i];

		if(document.GUID == guid) {
			break;
		}
	}

	buildDocumentDataTable(document, identifier, divToAppendTo);
}

function buildDocumentDataTable(entity, attributeRequired, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('width: 15%; border: solid black 1px;', 'Document'));
	tableRow.appendChild(createTableHeader('width: 30%; border: solid black 1px;', 'Upload Date'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Uploaded By'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', 'Download'));

    table.appendChild(tableRow);
	displayAttributes(getAttribute(entity.Attributes, attributeRequired), entity.Attributes, table, 'Document');

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
					
					tableDatacell.id = 'attribute'.concat(type + i);
					break;
				case 3:
					tableDatacell.id = 'value'.concat(type + i);

					var downloadIcon = document.createElement('i');
					downloadIcon.setAttribute('class', 'fas fa-download');
					downloadIcon.setAttribute('title', 'Download Now');
					downloadIcon.setAttribute('style', 'padding-right: 15px;');

					var addToDownloadBasketIcon = document.createElement('i');
					addToDownloadBasketIcon.setAttribute('class', 'fas fa-cart-arrow-down');
					addToDownloadBasketIcon.setAttribute('title', 'Add To Download Cart');

					tableDatacell.appendChild(downloadIcon);
					tableDatacell.appendChild(addToDownloadBasketIcon);
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}