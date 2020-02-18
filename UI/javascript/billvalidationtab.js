function openTab(evt, tabName, guid) {
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

	createCard(guid, newDiv);

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

		var meterTypeNode = span.parentNode.parentNode.parentNode.parentNode.children[3];
		var siteNode = meterTypeNode.parentNode.parentNode.parentNode.parentNode.children[3];
		var periodNode = siteNode.parentNode.parentNode.parentNode.parentNode.children[3];

		button.innerHTML = periodNode.innerText.concat(' - ').concat(siteNode.innerText.concat(' - ').concat(meterTypeNode.innerText.concat(' - ').concat(span.innerHTML)));
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

function createCard(guid, divToAppendTo) {
	var billEntity;
	
	var dataLength = data.length;
	for(var i = 0; i < dataLength; i++) {
		var datum = data[i];
		var sites = datum.Sites;

		var siteLength = sites.length;
		for(var j = 0; j < siteLength; j++) {
			var site = sites[j];
			var meters = site.Meters;

			var meterLength = meters.length;
			for(var k = 0; k < meterLength; k++) {
				var meter = meters[k];
				var bills = meter.Bills;

				var billLength = bills.length;
				for(var l = 0; l < billLength; l++) {
					var bill = bills[l];

					if(bill.GUID == guid) {
						billEntity = bill;
						break;
					}
				}

				if(billEntity){
					break;
				}
			}

			if(billEntity){
				break;
			}
		}

		if(billEntity){
			break;
		}
	}

	buildBillDataTable(billEntity, divToAppendTo);
}

function buildBillDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	displayAttributes(entity.Details, table);

	treeDiv.appendChild(table);
}

function displayAttributes(attributes, table) {
	if(!attributes) {
		return;
	}

	var attributesLength = attributes.length;
	for(var i = 0; i < attributesLength; i++) {
		var tableRow = document.createElement('tr');

		for(var j = 0; j < 2; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(j) {
				case 0:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					break;
				case 1:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}