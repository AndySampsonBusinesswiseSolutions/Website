function pageLoad() {
	createTree(data, "Hierarchy", "treeDiv", "", "filterContractsByStatus()");
	filterContractsByStatus(null);
	addExpanderOnClickEvents();	
	
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

function showRates(contractType, contractIndex, contractDetailIndex, mpxnIndex) {
	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Rate'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	table.appendChild(tableRow);
	var contracts = [];

	switch(contractType) {
		case "outOfContract":
			contracts = ooccontracts;
			break;
		case "active":
			contracts = activecontracts;
			break;
		case "pending":
			contracts = pendingcontracts;
			break;
		case "finished":
			contracts = finishedcontracts;
			break;
	}

	var mpxn = contracts[contractIndex].Contract[contractDetailIndex].MPXN[mpxnIndex];
	var rates = mpxn.Rates;
	var ratesLength = rates.length;

	for(var i = 0; i < ratesLength; i++) {
		var rate = rates[i];
		var tableRow = document.createElement('tr');

		for(var j = 0; j < 2; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			if(j == 0) {
				tableDatacell.innerHTML = rate.RateName;
			}
			else {
				tableDatacell.innerHTML = rate.Rate;
			}

			tableRow.appendChild(tableDatacell);
			table.appendChild(tableRow);
		}
	}

	var modal = document.getElementById("ratePopup");
	var title = document.getElementById("rateTitle");
	var span = modal.getElementsByClassName("close")[0];
	var rateText = document.getElementById('rateText');

	clearElement(rateText);
	rateText.appendChild(table);

    finalisePopup(title, 'Rates<br><br>', modal, span);
}

function finalisePopup(title, titleHTML, modal, span) {
    title.innerHTML = titleHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function buildContractDataGrids(contracts, contractType, applyClickEvent) {
	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%; border: solid black 1px; text-align: center;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('', 'Supplier'));
	tableRow.appendChild(createTableHeader('', 'Contract Reference'));
	tableRow.appendChild(createTableHeader('', 'MPXN'));
	tableRow.appendChild(createTableHeader('', 'Contract Start Date'));
	tableRow.appendChild(createTableHeader('', 'Contract End Date'));
	tableRow.appendChild(createTableHeader('', 'Product Type'));
	tableRow.appendChild(createTableHeader('', 'Rates'));
	tableRow.appendChild(createTableHeader('', 'Is Businesswise Contract?'));
	table.appendChild(tableRow);
	
	var contractLength = contracts.length;

	for(var i = 0; i < contractLength; i++) {
		var contract = contracts[i];
		var contractDetailLength = contract.Contract.length;
		var hasMultipleContracts = contractDetailLength > 1;
		var mpxnDatacell = getAttributeBySupplier(contract.Contract, 'Identifier');

		var tableRow = document.createElement('tr');
		tableRow.appendChild(getSupplier(contractType, contract.Supplier, hasMultipleContracts, true, applyClickEvent));
		tableRow.appendChild(getContractReferenceBySupplier(contractType, contract.Contract[0].ContractReference, hasMultipleContracts, mpxnDatacell.innerText == 'Multiple', '', applyClickEvent));
		tableRow.appendChild(mpxnDatacell);
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'ContractStartDate'));
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'ContractEndDate'));
		tableRow.appendChild(getAttributeByMPXN(contract.Contract[0].MPXN[0], 'ProductType'));
		tableRow.appendChild(getRates(contractType, i, 0, 0, mpxnDatacell.innerText != 'Multiple'));
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'IsBusinesswiseContract'));
		table.appendChild(tableRow);		

		for(var j = 0; j < contractDetailLength; j++) {
			var contractDetail = contract.Contract[j];
			var mpxnLength = contractDetail.MPXN.length;
			var hasMultipleMPXNs = mpxnLength > 1;

			if(hasMultipleContracts) {
				var tableRow = document.createElement('tr');
				tableRow.appendChild(getSupplier(contractType, '', hasMultipleMPXNs, false, applyClickEvent));
				tableRow.appendChild(getContractReferenceByContract(contractType, contractDetail.ContractReference, hasMultipleMPXNs, true, contract.Supplier, applyClickEvent));
				tableRow.appendChild(getAttributeByMPXN(contractDetail.MPXN[0], 'Identifier', hasMultipleMPXNs));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'ContractStartDate'));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'ContractEndDate'));
				tableRow.appendChild(getAttributeByMPXN(contractDetail.MPXN[0], 'ProductType'));
				tableRow.appendChild(getRates(contractType, i, j, 0, !hasMultipleMPXNs));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'IsBusinesswiseContract'));
	
				tableRow.setAttribute('class', 'listitem-hidden '.concat(contractType.concat(contract.Supplier).concat('List')));
				table.appendChild(tableRow);
			}						

			if(hasMultipleMPXNs) {
				for(var k = 0; k < mpxnLength; k++) {
					var mpxn = contractDetail.MPXN[k];
	
					var tableRow = document.createElement('tr');
					tableRow.appendChild(getSupplier(contractType, '', false, false, applyClickEvent));
					tableRow.appendChild(getContractReferenceByContract(contractType, '', false, false, '', applyClickEvent));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'Identifier'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ContractStartDate'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ContractEndDate'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ProductType'));
					tableRow.appendChild(getRates(contractType, i, j, k, true));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'IsBusinesswiseContract'));
					
					tableRow.setAttribute('class', 'listitem-hidden '.concat(contractType.concat(contractDetail.ContractReference).concat('List')));
					table.appendChild(tableRow);
				}
			}			
		}
	}

	var divToAppendTo = document.getElementById(contractType);
	clearElement(divToAppendTo);
	divToAppendTo.appendChild(table);
}

function getSupplier(contractType, supplier, hasMultipleRecords, applyGroupIcon, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon(contractType.concat(supplier), 'far fa-plus-square', 'padding-right: 10px;', contractType, null, applyClickEvent);

		tableDatacell.appendChild(icon);
	}
	
	tableDatacell.appendChild(createSpan('', supplier));
	return tableDatacell;
}

function getContractReferenceBySupplier(contractType, contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(!hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon(contractType.concat(contractReference), 'far fa-plus-square', 'padding-right: 10px;', contractType, contractSupplier, applyClickEvent);

		tableDatacell.appendChild(icon);

		if(applyGroupIcon) {
			tableDatacell.appendChild(createSpan('', contractReference));
		}
	}

	return tableDatacell;
}

function getContractReferenceByContract(contractType, contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon(contractType.concat(contractReference), 'far fa-plus-square', 'padding-right: 10px;', contractType, contractSupplier, applyClickEvent);

		tableDatacell.appendChild(icon);
	}

	if(!hasMultipleRecords || (hasMultipleRecords && applyGroupIcon)) {
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	return tableDatacell;
}

function createGroupByIcon(iconId, className, style, contractType, contractSupplier, applyClickEvent) {
	var icon = createIcon(iconId, className.concat(' show-pointer'), style);

	if(contractSupplier) {
		icon.setAttribute('additionalcontrols', contractType.concat(contractSupplier));
	}

	if(applyClickEvent) {
		addExpanderOnClickEventsByElement(icon);
	}

	return icon;
}

function getRates(contractType, contractIndex, contractDetailIndex, mpxnIndex, canShowRates) {
	var tableDatacell = document.createElement('td');

	if(canShowRates) {
		var icon = createIcon('', 'fas fa-search show-pointer', null, 'showRates("' + contractType + '",' + contractIndex + ',' + contractDetailIndex + ',' + mpxnIndex + ')');

		tableDatacell.appendChild(icon);
	}

	return tableDatacell;
}

function getAttributeBySupplier(contracts, attribute) {
	var tableDatacell = document.createElement('td');
	var attributeValue = contracts[0].MPXN[0][attribute];
	var contractLength = contracts.length;

	for(var i = 0; i < contractLength; i++) {
		attributeValue = getAttributeByContract(contracts[i], attribute, attributeValue).innerText;

		if(attributeValue == 'Multiple') {
			break;
		}
	}

	tableDatacell.innerText = attributeValue;
	return tableDatacell;
}

function getAttributeByContract(contract, attribute, attributeValue) {
	var tableDatacell = document.createElement('td');

	if(!attributeValue) {
		attributeValue = contract.MPXN[0][attribute];
	}

	var mpxnLength = contract.MPXN.length;

	for(var i = 0; i < mpxnLength; i++) {
		if(contract.MPXN[i][attribute] != attributeValue) {
			attributeValue = 'Multiple';
			break;
		}
	}

	tableDatacell.innerText = attributeValue;
	return tableDatacell;
}

function getAttributeByMPXN(mpxn, attribute, hasMultipleRecords) {
	var tableDatacell = document.createElement('td');
	tableDatacell.innerText = hasMultipleRecords ? 'Multiple' : mpxn[attribute];
	return tableDatacell;
}

function filterContractsBySiteMeter(element, contracts, contractType) {
	if(!element) {
		return buildContractDataGrids(contracts, contractType, false);;
	}

	var span = document.getElementById(element.id.replace('checkbox', 'span'));
	var linkedCheckboxes = document.getElementsByTagName('input');
	var linkedCheckboxesLength = linkedCheckboxes.length;
	var mpxns = [];

	if(element.getAttribute('branch') == 'Site') {
		for(var i = 0; i < linkedCheckboxesLength; i++) {
			var linkedCheckbox = linkedCheckboxes[i];
			if(linkedCheckbox.type.toLowerCase() == 'checkbox' && linkedCheckbox.getAttribute('branch') == 'Meter') {
				var linkedSite = linkedCheckbox.getAttribute('linkedSite');
	
				if(linkedSite == span.innerText) {
					linkedCheckbox.checked = element.checked;
				}
			}
		}
	}

	for(var i = 0; i < linkedCheckboxesLength; i++) {
		var linkedCheckbox = linkedCheckboxes[i];
		if(linkedCheckbox.type.toLowerCase() == 'checkbox' && linkedCheckbox.getAttribute('branch') == 'Meter') {
			if(linkedCheckbox.checked) {
				mpxns.push(document.getElementById(linkedCheckbox.id.replace('checkbox', 'span')).innerText);
			}
		}
	}

	if(mpxns.length == 0) {
		buildContractDataGrids(contracts, contractType, true);
	}
	else {
		var filteredContracts = [];
		var contractLength = contracts.length;

		for(var i = 0; i < contractLength; i++) {
			var contract = contracts[i];
			var contractDetailLength = contract.Contract.length;
			var contractCopied = false;

			for(var j = 0; j < contractDetailLength; j++) {
				var contractDetail = contract.Contract[j];
				var mpxnLength = contractDetail.MPXN.length;
				var contractDetailCopied = false;

				for(var k = 0; k < mpxnLength; k++) {
					var mpxn = contractDetail.MPXN[k];

					if(mpxns.includes(mpxn.Identifier)) {
						var copiedContract;
						var copiedContractDetail;

						if(!contractCopied) {
							copiedContract = {
								"Supplier" : contract.Supplier,
								"Contract" : []
							};
							filteredContracts.push(copiedContract);
							contractCopied = true;
						}
						else {
							for(var a = 0; a < filteredContracts.length; a++) {
								if(filteredContracts[a].Supplier == contract.Supplier) {
									copiedContract = filteredContracts[a];
									break;
								}
							}
						}

						if(!contractDetailCopied) {
							copiedContractDetail = {
								"ContractReference" : contractDetail.ContractReference,
                				"MPXN" : []
							};
							copiedContract.Contract.push(copiedContractDetail);
							contractDetailCopied = true;
						}
						else {
							for(var a = 0; a < copiedContract.Contract.length; a++) {
								if(copiedContract.Contract[a].ContractReference == contractDetail.ContractReference) {
									copiedContractDetail = copiedContract.Contract[a];
									break;
								}
							}
						}

						copiedContractDetail.MPXN.push(mpxn);
					}
				}
			}
		}

		buildContractDataGrids(filteredContracts, contractType, true);
	}
}

function filterContractsByStatus(element) {
	filterContractsBySiteMeter(element, ooccontracts, 'outOfContract');
	filterContractsBySiteMeter(element, activecontracts, 'active');
	filterContractsBySiteMeter(element, pendingcontracts, 'pending');
	filterContractsBySiteMeter(element, finishedcontracts, 'finished');
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, groupByOption, baseElement, commodity, checkboxFunction, showSubMeters) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];

        if(!commoditySiteMatch(base, commodity)) {
            continue;
        }
        
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();
        var childrenCreated = false;
        
        if(groupByOption == 'Hierarchy') {
            if(base.hasOwnProperty('Meters')) {
                buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName, showSubMeters);
                childrenCreated = true;
            }

        }
        else {
            buildBranch(base.Meters, groupByOption, ul, commodity, checkboxFunction, baseName, showSubMeters);
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
            branchDiv.removeAttribute('class', 'far fa-plus-square');
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
        branchDiv.removeAttribute('class', 'far fa-plus-square');
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

function createBranchDiv(branchDivId, childrenCreated = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(childrenCreated) {
        branchDiv.setAttribute('class', 'far fa-plus-square');
    }

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

	updateClassOnClick("outOfContractMeters", 'fa-plus-square', 'fa-minus-square');
	updateClassOnClick("finishedContracts", 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});

	updateAdditionalControls(element);
	expandAdditionalLists(element);
}

function updateAdditionalControls(element) {
	var additionalcontrols = element.getAttribute('additionalcontrols');

	if(!additionalcontrols) {
		return;
	}

	var listToHide = element.id.concat('List');
	var clickEventFunction = function (event) {
		updateClassOnClick(listToHide, 'listitem-hidden', '')
	};

	var controlArray = additionalcontrols.split(',');
	for(var j = 0; j < controlArray.length; j++) {
		var controlId = controlArray[j];	

		element.addEventListener('click', function (event) {
			var controlElement = document.getElementById(controlId);
			if(hasClass(this, 'fa-minus-square')) {				
				controlElement.addEventListener('click', clickEventFunction, false);
			}
			else {
				controlElement.removeEventListener('click', clickEventFunction);
			}
		});
	}	
}

function expandAdditionalLists(element) {
	var additionalLists = element.getAttribute('additionallists');

	if(!additionalLists) {
		return;
	}

	element.addEventListener('click', function (event) {
		var controlArray = additionalLists.split(',');
		for(var j = 0; j < controlArray.length; j++) {
			var controlId = controlArray[j];
			var controlElement = document.getElementById(controlId);
			updateClass(controlElement, 'listitem-hidden', '');
		}
	});		
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

function displayUploadContract() {
	var modal = document.getElementById("uploadContractPopup");
	var title = document.getElementById("uploadContractTitle");
	var span = modal.getElementsByClassName("close")[0];

	finalisePopup(title, 'Upload Contract<br><br>', modal, span);
	//setupContractDragAndDrop();
}

let dropArea = document.getElementById("drop-area")

function setupContractDragAndDrop() {
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