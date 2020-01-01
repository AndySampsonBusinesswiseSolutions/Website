function showRates(contractIndex, contractDetailIndex, mpxnIndex) {
	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Rate'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	table.appendChild(tableRow);

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

	xdialog.confirm(table.outerHTML, function() {}, 
	{
		style: 'width:50%;font-size:0.8rem;',
		buttons: {
			ok: {
				text: 'Close',
				style: 'background: Green;'
			}
		},
		title: mpxn.ProductType.concat(' Rates For ').concat(mpxn.Identifier)
	});
}

function buildContractDataGrids(contracts, applyClickEvent) {
	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%; border: solid black 1px;');

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
		tableRow.appendChild(getSupplier(contract.Supplier, hasMultipleContracts, true, applyClickEvent));
		tableRow.appendChild(getContractReferenceBySupplier(contract.Contract[0].ContractReference, hasMultipleContracts, mpxnDatacell.innerText == 'Multiple', '', applyClickEvent));
		tableRow.appendChild(mpxnDatacell);
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'ContractStartDate'));
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'ContractEndDate'));
		tableRow.appendChild(getAttributeByMPXN(contract.Contract[0].MPXN[0], 'ProductType'));
		tableRow.appendChild(getRates(i, 0, 0, mpxnDatacell.innerText != 'Multiple'));
		tableRow.appendChild(getAttributeBySupplier(contract.Contract, 'IsBusinesswiseContract'));
		table.appendChild(tableRow);		

		for(var j = 0; j < contractDetailLength; j++) {
			var contractDetail = contract.Contract[j];
			var mpxnLength = contractDetail.MPXN.length;
			var hasMultipleMPXNs = mpxnLength > 1;

			if(hasMultipleContracts) {
				var tableRow = document.createElement('tr');
				tableRow.appendChild(getSupplier('', hasMultipleMPXNs, false, applyClickEvent));
				tableRow.appendChild(getContractReferenceByContract(contractDetail.ContractReference, hasMultipleMPXNs, true, contract.Supplier, applyClickEvent));
				tableRow.appendChild(getAttributeByMPXN(contractDetail.MPXN[0], 'Identifier', hasMultipleMPXNs));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'ContractStartDate'));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'ContractEndDate'));
				tableRow.appendChild(getAttributeByMPXN(contractDetail.MPXN[0], 'ProductType'));
				tableRow.appendChild(getRates(i, j, 0, !hasMultipleMPXNs));
				tableRow.appendChild(getAttributeByContract(contractDetail, 'IsBusinesswiseContract'));
	
				tableRow.setAttribute('class', 'listitem-hidden '.concat('OutOfContract'.concat(contract.Supplier).concat('List')));
				table.appendChild(tableRow);
			}						

			if(hasMultipleMPXNs) {
				for(var k = 0; k < mpxnLength; k++) {
					var mpxn = contractDetail.MPXN[k];
	
					var tableRow = document.createElement('tr');
					tableRow.appendChild(getSupplier('', false, false, applyClickEvent));
					tableRow.appendChild(getContractReferenceByContract('', false, false, '', applyClickEvent));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'Identifier'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ContractStartDate'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ContractEndDate'));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'ProductType'));
					tableRow.appendChild(getRates(i, j, k, true));
					tableRow.appendChild(getAttributeByMPXN(mpxn, 'IsBusinesswiseContract'));
					
					tableRow.setAttribute('class', 'listitem-hidden '.concat('OutOfContract'.concat(contractDetail.ContractReference).concat('List')));
					table.appendChild(tableRow);
				}
			}			
		}
	}

	var divToAppendTo = document.getElementById('outOfContract');
	clearElement(divToAppendTo);
	divToAppendTo.appendChild(table);
}

function getSupplier(supplier, hasMultipleRecords, applyGroupIcon, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon('OutOfContract'.concat(supplier), 'far fa-plus-square', 'padding-right: 10px;', null, applyClickEvent);

		tableDatacell.appendChild(icon);
	}
	
	tableDatacell.appendChild(createSpan('', supplier));
	return tableDatacell;
}

function getContractReferenceBySupplier(contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(!hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon('OutOfContract'.concat(contractReference), 'far fa-plus-square', 'padding-right: 10px;', contractSupplier, applyClickEvent);

		tableDatacell.appendChild(icon);

		if(applyGroupIcon) {
			tableDatacell.appendChild(createSpan('', contractReference));
		}
	}

	return tableDatacell;
}

function getContractReferenceByContract(contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords && applyGroupIcon) {
		var icon = createGroupByIcon('OutOfContract'.concat(contractReference), 'far fa-plus-square', 'padding-right: 10px;', contractSupplier, applyClickEvent);

		tableDatacell.appendChild(icon);
	}

	if(!hasMultipleRecords || (hasMultipleRecords && applyGroupIcon)) {
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	return tableDatacell;
}

function createGroupByIcon(iconId, className, style, contractSupplier, applyClickEvent) {
	var icon = createIcon(iconId, className.concat(' show-pointer'), style);

	if(contractSupplier) {
		icon.setAttribute('additionalcontrols', 'OutOfContract'.concat(contractSupplier));
	}

	if(applyClickEvent) {
		addExpanderOnClickEventsByElement(icon);
	}

	return icon;
}

function getRates(contractIndex, contractDetailIndex, mpxnIndex, canShowRates) {
	var tableDatacell = document.createElement('td');

	if(canShowRates) {
		var icon = createIcon('', 'fas fa-search show-pointer', null, 'showRates(' + contractIndex + ',' + contractDetailIndex + ',' + mpxnIndex + ')');

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

function filterContracts(element) {
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
		buildContractDataGrids(contracts, true);
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

		buildContractDataGrids(filteredContracts, true);
	}
}