function pageLoad() {    
	createTree(documents, "documentTree", "createCardButton");
	
	document.onmousemove = function(e) {
		setupSidebarHeight();
		setupSidebar(e);
	};

	window.onscroll = function() {
		setupSidebarHeight();
	};
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
		document.getElementById('User0checkbox').checked = true;
		createCardButton(document.getElementById('User0checkbox'));
		openTab(document.getElementById('User0button'), 'User0button', '0');
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
	var document = getEntityByGUID(guid);

	buildDocumentDataTable(document, divToAppendTo);
}

function buildDocumentDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('class', 'tree-div scrolling-wrapper');
	divToAppendTo.appendChild(div);
	clearElement(div);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	displayAttributes(entity.Attributes, table);

	div.appendChild(table);
}

function displayAttributes(attributes, table) {
	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('Document Name', 0));

	var headers = attributes.Headers;
	var headerLength = headers.length;

	for(var i = 0; i < headerLength; i++) {
		tableRow.appendChild(createTableHeader(headers[i], i + 1));
	}

	tableRow.appendChild(createTableHeader('Uploaded By', headerLength + 1));
	tableRow.appendChild(createTableHeader('Uploaded Date', headerLength + 2));
	tableRow.appendChild(createTableHeader('Actions', headerLength + 3));

	table.appendChild(tableRow);
	
	var documents = attributes.Documents;
	var documentLength = documents.length;

	for(var i = 0; i < documentLength; i++) {
		var records = documents[i];
		var tableRow = document.createElement('tr');
		tableRow.id = 'row' + i;

		records.Attributes.forEach(record => {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('class', 'table-cell');
			tableDatacell.innerHTML = record.Value;
			tableRow.appendChild(tableDatacell);
		});

		var tableDatacell = document.createElement('td');
		tableDatacell.setAttribute('class', 'table-cell');

		var downloadIcon = document.createElement('i');
		downloadIcon.setAttribute('class', 'fas fa-download show-pointer');
		downloadIcon.setAttribute('title', 'Download Now');
		downloadIcon.setAttribute('style', 'padding-right: 25px;');

		var addToDownloadBasketIcon = document.createElement('i');
		addToDownloadBasketIcon.setAttribute('class', 'fas fa-cart-arrow-down show-pointer');
		addToDownloadBasketIcon.setAttribute('title', 'Add To Download Cart');

		tableDatacell.appendChild(downloadIcon);
		tableDatacell.appendChild(addToDownloadBasketIcon);
		
		tableRow.appendChild(tableDatacell);
		table.appendChild(tableRow);
	}
}

function uploadDocument() {
	var modal = document.getElementById("uploadDocumentPopup");
	var title = document.getElementById("uploadDocumentTitle");
	var span = modal.getElementsByClassName("close")[0];

    finalisePopup(title, 'Upload Document<br><br>', modal, span);
}

function displayFileDetailsPopup(row) {
	var modal = document.getElementById("fileDetailsPopup");
	var title = document.getElementById("fileDetailsTitle");
	var span = modal.getElementsByClassName("close")[0];
	var text = document.getElementById("fileDetailsText");

	text.innerHTML = 'Sites/Meters/Sub Meters chosen:<br>'
				   + '<div style="padding-left:15px;">Site: Leeds</div>'
				   + '<div style="padding-left:30px;">Meter: 987654</div>'
				   + '<div style="padding-left:30px;">Meter: 1234567890123</div>'
				   + '<div style="padding-left:45px;">Sub Meter: Sub Meter 1</div>'
				   + '<div style="padding-left:45px;">Sub Meter: Sub Meter 2</div><br>'
				   + '<div style="padding-left:15px;">Meter: 1234567890120</div><br>'
				   + 'Date Range chosen:<br>'
				   + '<div style="padding-left:15px;">01/11/2019 to 26/11/2019</div><br>'
				   + 'Time Span chosen:<br>'
				   + '<div style="padding-left:15px;">Daily</div><br>'

    finalisePopup(title, 'File Details<br><br>', modal, span);
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
	var headerDiv = createHeaderDiv("siteHeader", 'Document Type', true);
  	var ul = createBranchUl("siteSelector", false, true);

    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
	div.appendChild(tree);
	
	for(var i = 0; i < 6; i++) {
		document.getElementById('Document'+ i + 'checkbox').checked = true;
		createCardButton(document.getElementById('Document'+ i + 'checkbox'));	
	}

	openTab(document.getElementById('Document0button'), 'Document0button', '0');

	addExpanderOnClickEvents();
	setOpenExpanders();
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = base.Attributes.DocumentType;
        var li = document.createElement('li');
        var ul = createUL();

        appendListItemChildren(li, 'Document'.concat(base.GUID), checkboxFunction, 'Document', baseName, ul, baseName, base.GUID);

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
	return 'far fa-file';
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

function createTableHeader(value, id) {
	var tableHeader = document.createElement('th');
	tableHeader.id = "tableHeader" + id;
	tableHeader.setAttribute('class', 'table-header');
	tableHeader.innerHTML = value;
	return tableHeader;
}

function getEntityByGUID(guid) {
	var dataLength = documents.length;
	for(var i = 0; i < dataLength; i++) {
		var entity = documents[i];
		if(entity.GUID == guid) {
			return entity;
		}
	}
	
	return null;
}