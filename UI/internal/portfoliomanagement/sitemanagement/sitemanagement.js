function pageLoad() {    
	createTree(data, "siteTree", "", "createCardButton", true);
	
	document.onmousemove = function(e) {
		setupSidebarHeight();
		setupSidebar(e);
	};

	window.onscroll = function() {
		setupSidebarHeight();
	};
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
	tree.setAttribute('class', 'scrolling-wrapper');
	
	var headerDiv = createHeaderDiv("siteHeader", 'Sites/Meters', true);
  	var ul = createBranchUl("siteSelector", false, true);

    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
	div.appendChild(tree);
	
	document.getElementById('Site0checkbox').checked = true;
	createCardButton(document.getElementById('Site0checkbox'));
	openTab(document.getElementById('Site0button'), 'Site0button', '0', 'Site');

	document.getElementById('Site6checkbox').checked = true;
	createCardButton(document.getElementById('Site6checkbox'));

	document.getElementById('Site100checkbox').checked = true;
	createCardButton(document.getElementById('Site100checkbox'));

	addExpanderOnClickEvents();
	setOpenExpanders();
}

function buildTree(baseData, baseElement, commodity, checkboxFunction, showSubMeters) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];

        if(!commoditySiteMatch(base, commodity)) {
            continue;
        }
        
        var baseName = getAttribute(base.Attributes, 'Name');
        var li = document.createElement('li');
        var ul = createUL();
        var childrenCreated = false;
        
		if(base.hasOwnProperty('Meters')) {
			buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName, showSubMeters);
			childrenCreated = true;
		}

        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, childrenCreated);

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var branchOptions = getBranchOptions(meters, groupByOption, commodity);
    var branchId;
    var groupBySubOption;

    switch (groupByOption) {
        case 'DeviceType':
            branchId = commodity.concat('DeviceType');
            groupBySubOption = 'DeviceSubType';
            break;
        case 'Zone':
            branchId = commodity.concat('Zone');
            groupBySubOption = 'Panel';
            break;
    }

    var branchOptionsLength = branchOptions.length;
    for(var i = 0; i < branchOptionsLength; i++) {
        var branchOption = branchOptions[i];
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupByOption, branchOption, commodity);
        var ul = createUL();
        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity, checkboxFunction, linkedSite, showSubMeters);
        appendListItemChildren(li, branchId.concat(branchCount), checkboxFunction, 'GroupByOption|'.concat(groupByOption), branchOption, commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var branchOptions = getBranchOptions(meters, groupBySubOption, commodity);
    var branchId; 

    switch (groupBySubOption) {
        case 'DeviceSubType':
            branchId = commodity.concat('DeviceSubType');
            break;
        case 'Panel':
            branchId = commodity.concat('Panel');
            break;
    }

    var branchOptionsLength = branchOptions.length;
    for(var i = 0; i < branchOptionsLength; i++) {
        var branchOption = branchOptions[i];
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupBySubOption, branchOption, commodity);
        var ul = createUL();
        buildIdentifierHierarchy(matchedMeters, ul, commodity, checkboxFunction, linkedSite, showSubMeters);
        appendListItemChildren(li, branchId.concat(subBranchCount), checkboxFunction, 'GroupBySubOption|'.concat(groupBySubOption), branchOption, commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, childrenCreated) {
    li.appendChild(createBranchDiv(id, childrenCreated));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(!commodityMeterMatch(meter, commodity)) {
            continue;
        }

        var meterAttributes = meter.Attributes;
        var identifier = getAttribute(meterAttributes, 'Identifier');
        var meterCommodity = getAttribute(meterAttributes, 'Commodity');
        var deviceType = getAttribute(meterAttributes, 'DeviceType');
        var hasSubMeters = meter.hasOwnProperty('SubMeters');
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meter.GUID);
        var branchDiv = createBranchDiv(branchId);
        
        if(!showSubMeters || !hasSubMeters) {
            branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer expander');
            branchDiv.setAttribute('class', 'far fa-times-circle');
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID));
        li.appendChild(createTreeIcon(deviceType, meterCommodity));
        li.appendChild(createSpan(branchId, identifier));

        if(showSubMeters && hasSubMeters) {
            var ul = createUL();
            buildSubMeterHierarchy(meter['SubMeters'], ul, deviceType, meterCommodity, checkboxFunction, linkedSite);

            li.appendChild(createBranchListDiv(branchId.concat('List'), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity, checkboxFunction, linkedSite) {
    var subMetersLength = subMeters.length;
    for(var i = 0; i < subMetersLength; i++){
        var subMeter = subMeters[i];
        var li = document.createElement('li');

        var identifier = getAttribute(subMeter.Attributes, 'Identifier');
        var branchDiv = createBranchDiv(subMeter.GUID);
        branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer expander');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox('SubMeter'.concat(subMeter.GUID), checkboxFunction, 'SubMeter', linkedSite, subMeter.GUID));
        li.appendChild(createTreeIcon(deviceType, commodity));
        li.appendChild(createSpan('SubMeter'.concat(subMeter.GUID), identifier));   

        baseElement.appendChild(li); 
    }
}

function getBranchOptions(meters, property, commodity) {
    var branchOptions = [];
    var metersLength = meters.length;

    for(var i = 0; i < metersLength; i++) {
        var meter = meters[i];
        var attribute = getAttribute(meter.Attributes, property);
        if(!branchOptions.includes(attribute)
            && commodityMeterMatch(meter, commodity)) {
            branchOptions.push(attribute);
        }        
    }

    return branchOptions;
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

function createTreeIcon(branch, commodity) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch, commodity));
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

function commoditySiteMatch(site, commodity) {
    if(commodity == '') {
        return true;
    }

    if(!site.hasOwnProperty('Meters')) {
        return false;
    }

    var metersLength = site.Meters.length;
    for(var i = 0; i < metersLength; i++) {
        if(commodityMeterMatch(site.Meters[i], commodity)) {
            return true;
        }
    }

    return false;
}

function commodityMeterMatch(meter, commodity) {
    if(commodity == '') {
        return true;
    }

    var meterCommodity = getAttribute(meter.Attributes, 'Commodity');
    return meterCommodity.toLowerCase() == commodity.toLowerCase();
}

function getIconByBranch(branch, commodity) {
    switch (branch) {
        case 'Mains':
            if(commodity == 'Gas') {
                return 'fas fa-burn';
            }
            else {
                return 'fas fa-plug';
            }
        case 'Lighting':
            return 'fas fa-lightbulb';
        case 'Unknown':
            return 'fas fa-question-circle';
        default:
            return 'fas fa-map-marker-alt';
    }
}

function getMatchedMeters(meters, attribute, branchOption, commodity) {
    var matchedMeters = [];
    var metersLength = meters.length;

    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(getAttribute(meter.Attributes, attribute) == branchOption
            && commodityMeterMatch(meter, commodity)) {
            matchedMeters.push(meter);
        }
    }

    return matchedMeters;
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
	
	switch(branch) {
		case "Site":
            createCard(guid, newDiv, 'Site');		
			break;
		case "Meter":
			createCard(guid, newDiv, 'Meter');
			break;
		case "SubMeter":
			createCard(guid, newDiv, 'SubMeter');
			break;
	}

	newDiv.style.display = "block";
  }

function createCardButton(checkbox){
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));
	var button = document.getElementById(span.id.replace('span', 'button'));

	if(checkbox.checked){
		if(!button) {
			var branch = checkbox.getAttribute('branch');
			cardDiv.setAttribute('style', '');
			var button = document.createElement('button');
			button.setAttribute('class', 'tablinks');
			button.setAttribute('onclick', 'openTab(this, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '", "' + checkbox.getAttribute('branch') + '")');

			if(branch == "Site") {
				button.innerHTML = span.innerHTML;
			}
			else {
				button.innerHTML = checkbox.parentNode.parentNode.parentNode.parentNode.children[3].innerText.concat(' - ').concat(span.innerHTML);
			}

			if(!span.innerText.includes('Add New ')) {
				button.innerHTML += '<i class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add ' + branch + ' To Download Basket"></i>'
				+ '<i class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download ' + branch + '"></i>';
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
		document.getElementById('Site100checkbox').checked = true;
		createCardButton(document.getElementById('Site100checkbox'));
		openTab(document.getElementById('Site100button'), 'Site100div', '100', 'Site');
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

function createCard(guid, divToAppendTo, type) {
    var site = getEntityByGUID(guid, type);

	buildCardView(site, divToAppendTo);
    buildDataTable(site, divToAppendTo);
}

function buildCardView(entity, divToAppendTo){
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
		"Contact Type",
        "Contact Telephone Number",
        "Sq. ft"
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

	var name = getAttribute(entity.Attributes, 'Name');
	if(name && name.startsWith('Add New')) {
		var addNewbutton = document.createElement('button');
		addNewbutton.id = 'addNewButton';
		addNewbutton.innerHTML = name;
		addNewbutton.setAttribute('style', 'margin-left: 10px;');
		divToAppendTo.appendChild(addNewbutton);	
	}

	loadMap(entityAttributes);
}

function loadMap(entityAttributes) {
	clearElement(document.getElementById('map-canvas'));

	var address = getAttribute(entityAttributes, 'GoogleAddress');
	if(!address) {
		return;
	}

	var latitude = getAttribute(entityAttributes, 'lat');
	var longitude = getAttribute(entityAttributes, 'lng');
	var latLng = {lat: latitude, lng: longitude};

	var mapOptions = {
		zoom: 19,
		center: new google.maps.LatLng(latitude, longitude),
		mapTypeId: google.maps.MapTypeId.roadmap 
	}
	var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
	var marker = new google.maps.Marker({
		map: map,
		position: latLng,
		title: address
	});
}

function displayDataTable() {
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

function buildDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('style', 'display: none;');
	divToAppendTo.appendChild(div);
	
	clearElement(div);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');

	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 30%; border: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; border: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%; border: solid black 1px;', ''));

    table.appendChild(tableRow);
	displayAttributes(entity.Attributes, table);

	div.appendChild(table);
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
	var dataLength = data.length;
	for(var i = 0; i < dataLength; i++) {
		var site = data[i];
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

function displayUploadUsage() {
	var modal = document.getElementById("uploadUsagePopup");
	var title = document.getElementById("uploadUsageTitle");
	var span = modal.getElementsByClassName("close")[0];

	finalisePopup(title, 'Upload Usage<br><br>', modal, span);
	//setupUsageDragAndDrop();
}

let dropArea = document.getElementById("drop-area")

function setupUsageDragAndDrop() {
	// ************************ Drag and drop ***************** //

	// Prevent default drag behaviors
	;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
		dropArea.addEventListener(eventName, preventDefaults, false)   
		document.body.addEventListener(eventName, preventDefaults, false)
	})

	// Highlight drop area when item is dragged over it
	;['dragenter', 'dragover'].forEach(eventName => {
		dropArea.addEventListener(eventName, highlight, false)
	})

	;['dragleave', 'drop'].forEach(eventName => {
		dropArea.addEventListener(eventName, unhighlight, false)
	})

	// Handle dropped files
	dropArea.addEventListener('drop', handleDrop, false)
}

function preventDefaults (e) {
  e.preventDefault()
  e.stopPropagation()
}

function highlight(e) {
  dropArea.classList.add('highlight')
}

function unhighlight(e) {
  dropArea.classList.remove('highlight')
}

function handleDrop(e) {
  var dt = e.dataTransfer
  var files = dt.files

  handleFiles(files)
}

let uploadProgress = []
let progressBar = document.getElementById('progress-bar')

function initializeProgress(numFiles) {
  progressBar.value = 0
  uploadProgress = []

  for(let i = numFiles; i > 0; i--) {
    uploadProgress.push(0)
  }
}

function updateProgress(fileNumber, percent) {
  uploadProgress[fileNumber] = percent
  let total = uploadProgress.reduce((tot, curr) => tot + curr, 0) / uploadProgress.length
  console.debug('update', fileNumber, percent, total)
  progressBar.value = total
}

function handleFiles(files) {
  files = [...files]
  initializeProgress(files.length)
  files.forEach(uploadFile)
  files.forEach(previewFile)
}

function previewFile(file) {
	var icon = document.createElement('i');
	icon.setAttribute('class', 'far fa-file-excel fa-9x');
	icon.setAttribute('title', file.name);
	document.getElementById('gallery').appendChild(icon)
}

function uploadFile(file, i) {
  var url = 'https://api.cloudinary.com/v1_1/joezimim007/image/upload'
  var xhr = new XMLHttpRequest()
  var formData = new FormData()
  xhr.open('POST', url, true)
  xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest')

  // Update progress (can be used to show progress indicator)
  xhr.upload.addEventListener("progress", function(e) {
    updateProgress(i, (e.loaded * 100.0 / e.total) || 100)
  })

  xhr.addEventListener('readystatechange', function(e) {
    if (xhr.readyState == 4 && xhr.status == 200) {
      updateProgress(i, 100)
    }
    else if (xhr.readyState == 4 && xhr.status != 200) {
      // Error. Inform the user
    }
  })

  formData.append('upload_preset', 'ujpu6gyk')
  formData.append('file', file)
  xhr.send(formData)
}