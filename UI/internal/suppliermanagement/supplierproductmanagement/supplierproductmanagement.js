function pageLoad() {    
	createTree(supplierproduct, "treeDiv", "createCardButton");
	
	document.onmousemove=function(e) {
		var mousecoords = getMousePos(e);
		if(mousecoords.x <= 25) {
			openNav();
		}  
		else if(mousecoords.x >= 400) {
			closeNav();
		}  
	};
}

function getMousePos(e) {
	return {x:e.clientX,y:e.clientY};
}

function openNav() {
	document.getElementById("mySidenav").style.width = "400px";
	document.getElementById("openNav").style.color = "#b62a51";
}

function closeNav() {
	document.getElementById("openNav").style.color = "white";
	document.getElementById("mySidenav").style.width = "0px";
}

function openTab(callingElement, tabName, guid, branch) {
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

	if(checkbox.checked){
		cardDiv.setAttribute('style', '');
		var button = document.createElement('button');
		button.setAttribute('class', 'tablinks');
		button.setAttribute('onclick', 'openTab(this, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '", "' + checkbox.getAttribute('branch') + '")');

		var meterTypeNode = span.parentNode.parentNode.parentNode.parentNode.children[2];
		var commodityNode = meterTypeNode.parentNode.parentNode.parentNode.parentNode.children[2];
		var supplierNode = commodityNode.parentNode.parentNode.parentNode.parentNode.children[2];

		button.innerHTML = supplierNode.innerText.concat(' - ').concat(commodityNode.innerText.concat(' - ').concat(meterTypeNode.innerText.concat(' - ').concat(span.innerHTML)));
		button.id = span.id.replace('span', 'button');
		tabDiv.appendChild(button);
	}
	else {
		var button = document.getElementById(span.id.replace('span', 'button'));
		tabDiv.removeChild(button);
	}	

	updateTabDiv();
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

	if(tabDivChildrenLength == 0) {
		document.getElementById('Product0checkbox').checked = true;
		createCardButton(document.getElementById('Product0checkbox'));
		openTab(document.getElementById('Product0button'), 'Product0button', '0', 'Product');
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
	var productEntity;
	
	var supplierproductLength = supplierproduct.length;
	for(var i = 0; i < supplierproductLength; i++) {
		var site = supplierproduct[i];
		var commodities = site.Commodities;
		
		var commodityLength = commodities.length;
		for(var j = 0; j < commodityLength; j++) {
			var commodity = commodities[j];
			var profileClasses = commodity.ProfileClasses;

			var profileClassLength = profileClasses.length;
			for(var k = 0; k < profileClassLength; k++) {
				var profileClass = profileClasses[k];
				var products = profileClass.Products;

				var productLength = products.length;
				for(var l = 0; l < productLength; l++) {
					var product = products[l];

					if(product.GUID == guid) {
						productEntity = product;
						break;
					}
				}

				if(productEntity) {
					break;
				}
			}

			if(productEntity) {
				break;
			}
		}

		if(productEntity) {
			break;
		}
	}

	buildCardView(productEntity, divToAppendTo);
	buildProductDataTable(productEntity, divToAppendTo);
	divToAppendTo.appendChild(document.createElement('br'));
	buildCostElementDataTable(productEntity, divToAppendTo);
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
		"ProductName",
		"DayPeriod",
		"NightPeriod",
		"Evening&WeekendPeriod"
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
	editDetailsButton.setAttribute('onclick', 'displayProductDataTable()');
	editDetailsButton.setAttribute('style', 'margin-top: 5px; margin-right: 5px; margin-bottom: 5px;')
	divToAppendTo.appendChild(editDetailsButton);	

	var editCostElementsButton = document.createElement('button');
	editCostElementsButton.id = 'editCostElementsButton';
	editCostElementsButton.innerHTML = 'Edit Cost Elements';
	editCostElementsButton.setAttribute('onclick', 'displayCostElementDataTable()');
	divToAppendTo.appendChild(editCostElementsButton);	
}

function displayProductDataTable() {
	var button = document.getElementById('editDetailsButton');
	var div = document.getElementById('displayAttributes');

	if(button.innerHTML == 'Edit Details') {
		div.setAttribute('style', 'margin-top: 5px;');
		button.innerText = 'Hide Details';
	}
	else {
		div.setAttribute('style', 'display: none;');
		button.innerText = 'Edit Details'
	}
}

function displayCostElementDataTable() {
	var button = document.getElementById('editCostElementsButton');
	var div = document.getElementById('displayCostElements');

	if(button.innerHTML == 'Edit Cost Elements') {
		div.setAttribute('style', '');
		button.innerText = 'Hide Cost Elements';
	}
	else {
		div.setAttribute('style', 'display: none');
		button.innerText = 'Edit Cost Elements'
	}
}

function buildProductDataTable(entity, divToAppendTo){
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
	displayAttributes(entity.Attributes, table, 'Product');

	treeDiv.appendChild(table);
}

function buildCostElementDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayCostElements';
	div.setAttribute('style', 'display: none');
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayCostElements');
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
	displayAttributes(entity.CostElements, table, 'Cost Element');

	treeDiv.appendChild(table);
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
	attributeTableDatacell.innerHTML = '<select style="width: 100%;"><option value=""></option><option value="Attribute 1">Attribute 1</option><option value="Attribute 2">Attribute 2</option></select>'
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

	var modal = document.getElementById("deleteRowPopup");
	var title = document.getElementById("deleteRowTitle");
	var span = modal.getElementsByClassName("close")[0];
	var deleteRowText = document.getElementById('deleteRowText');

	deleteRowText.innerText = "Are you sure you want to delete the '" + attribute.innerText + "' attribute with current value of '" + value.innerText + "'?";

    finalisePopup(title, 'Delete Attribute?<br><br>', modal, span);
}

function finalisePopup(title, titleHTML, modal, span) {
    title.innerHTML = titleHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
	var ul = createUL();
	ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Product(s) <i class="far fa-plus-square" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
	div.appendChild(tree);
	
	var guids = [0, 1, 4];
	guids.forEach(function(guid) {
		document.getElementById('Product' + guid + 'checkbox').checked = true;
		createCardButton(document.getElementById('Product' + guid + 'checkbox'));
	});
	
	openTab(document.getElementById('Product0button'), 'Product0button', '0', 'Product');

	addExpanderOnClickEvents();
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();

        buildCommodity(base.Commodities, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Site'.concat(base.GUID), baseName, ul);

        baseElement.appendChild(li);        
    }
}

function buildCommodity(commodities, baseElement, checkboxFunction, linkedSite) {
    var commoditiesLength = commodities.length;
    for(var i = 0; i < commoditiesLength; i++) {
        var commodity = commodities[i];
        var li = document.createElement('li');

        var ul = createUL();
        buildProfileClass(commodity.ProfileClasses, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Commodity'.concat(branchCount), commodity.CommodityName, ul);

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildProfileClass(profileClasses, baseElement, checkboxFunction, linkedSite) {
    var profileClassesLength = profileClasses.length;
    for(var i = 0; i < profileClassesLength; i++) {
        var profileClass = profileClasses[i];
        var li = document.createElement('li');
        var ul = createUL();
        buildProduct(profileClass.Products, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'ProfileClass'.concat(subBranchCount), profileClass.MeterType, ul);

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function appendListItemChildren(li, id, branchOption, ul) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createTreeIcon(branchOption));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildProduct(products, baseElement, checkboxFunction, linkedSite) {
    var productsLength = products.length;
    for(var i = 0; i < productsLength; i++){
        var product = products[i];
        var productAttributes = product.Attributes;
        var identifier = getAttribute(productAttributes, 'ProductName');
        var li = document.createElement('li');
        var branchId = 'Product'.concat(product.GUID);
        var branchDiv = createBranchDiv(branchId);
        
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Product', linkedSite, product.GUID));
        li.appendChild(createTreeIcon('Product'));
        li.appendChild(createSpan(branchId, identifier));

        baseElement.appendChild(li); 
    }
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    branchDiv.setAttribute('class', 'far fa-plus-square');
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
    switch (branch) {
        case 'Unknown':
            return 'fas fa-question-circle';
        default:
            return 'fas fa-map-marker-alt';
    }
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

	updateClassOnClick('treeDivSelector', 'fa-plus-square', 'fa-minus-square')
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
}