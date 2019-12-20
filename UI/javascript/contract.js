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
		title: mpxn.ProductType.concat(' For ').concat(mpxn.Identifier)
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
		var mpxnDatacell = getMPXNBySupplier(contract.Contract);

		var tableRow = document.createElement('tr');
		tableRow.appendChild(getSupplier(contract.Supplier, hasMultipleContracts, true, applyClickEvent));
		tableRow.appendChild(getContractReferenceBySupplier(contract.Contract[0].ContractReference, hasMultipleContracts, mpxnDatacell.innerText == 'Multiple', '', applyClickEvent));
		tableRow.appendChild(mpxnDatacell);
		tableRow.appendChild(getContractStartDateBySupplier(contract.Contract));
		tableRow.appendChild(getContractEndDateBySupplier(contract.Contract));
		tableRow.appendChild(getProductType());
		tableRow.appendChild(getRates(i, 0, 0, mpxnDatacell.innerText != 'Multiple'));
		tableRow.appendChild(getIsBusinesswiseContractBySupplier(contract.Contract));
		table.appendChild(tableRow);		

		for(var j = 0; j < contractDetailLength; j++) {
			var contractDetail = contract.Contract[j];
			var mpxnLength = contractDetail.MPXN.length;
			var hasMultipleMPXNs = mpxnLength > 1;

			if(hasMultipleContracts) {
				var tableRow = document.createElement('tr');
				tableRow.appendChild(getSupplier('', hasMultipleMPXNs, false, applyClickEvent));
				tableRow.appendChild(getContractReferenceByContract(contractDetail.ContractReference, hasMultipleMPXNs, true, contract.Supplier, applyClickEvent));
				tableRow.appendChild(getMPXNByContractOrMPXN(contractDetail.MPXN[0].Identifier, hasMultipleMPXNs));
				tableRow.appendChild(getContractStartDateByContract(contractDetail));
				tableRow.appendChild(getContractEndDateByContract(contractDetail));
				tableRow.appendChild(getProductType());
				tableRow.appendChild(getRates(i, j, 0, !hasMultipleMPXNs));
				tableRow.appendChild(getIsBusinesswiseContractByContract(contractDetail));
	
				tableRow.setAttribute('class', 'listitem-hidden '.concat('OutOfContract'.concat(contract.Supplier).concat('List')));
				table.appendChild(tableRow);
			}						

			if(hasMultipleMPXNs) {
				for(var k = 0; k < mpxnLength; k++) {
					var mpxn = contractDetail.MPXN[k];
	
					var tableRow = document.createElement('tr');
					tableRow.appendChild(getSupplier('', false, false, applyClickEvent));
					tableRow.appendChild(getContractReferenceByContract('', false, false, '', applyClickEvent));
					tableRow.appendChild(getMPXNByContractOrMPXN(mpxn.Identifier, false));
					tableRow.appendChild(getContractStartDateByMPXN(mpxn));
					tableRow.appendChild(getContractEndDateByMPXN(mpxn));
					tableRow.appendChild(getProductType());
					tableRow.appendChild(getRates(i, j, k, true));
					tableRow.appendChild(getIsBusinesswiseContractByMPXN(mpxn));
					
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
		var icon = document.createElement('i');
		icon.setAttribute('class', 'far fa-plus-square');
		icon.setAttribute('style', 'padding-right: 10px;');
		icon.id = 'OutOfContract'.concat(supplier)

		if(applyClickEvent) {
			addExpanderOnClickEventsByElement(icon);
		}

		tableDatacell.appendChild(icon);
	}
	
	tableDatacell.appendChild(createSpan('', supplier));
	return tableDatacell;
}

function getContractReferenceBySupplier(contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(!hasMultipleRecords && applyGroupIcon) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'far fa-plus-square');
		icon.setAttribute('style', 'padding-right: 10px;');

		if(contractSupplier) {
			icon.setAttribute('additionalcontrols', 'OutOfContract'.concat(contractSupplier));
		}
		
		icon.id = 'OutOfContract'.concat(contractReference)

		if(applyClickEvent) {
			addExpanderOnClickEventsByElement(icon);
		}

		tableDatacell.appendChild(icon);
	}

	if(!hasMultipleRecords) {
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	return tableDatacell;
}

function getContractReferenceByContract(contractReference, hasMultipleRecords, applyGroupIcon, contractSupplier, applyClickEvent) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords && applyGroupIcon) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'far fa-plus-square');
		icon.setAttribute('style', 'padding-right: 10px;');

		if(contractSupplier) {
			icon.setAttribute('additionalcontrols', 'OutOfContract'.concat(contractSupplier));
		}
		
		icon.id = 'OutOfContract'.concat(contractReference);

		if(applyClickEvent) {
			addExpanderOnClickEventsByElement(icon);
		}

		tableDatacell.appendChild(icon);
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	if(!hasMultipleRecords) {
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	return tableDatacell;
}

function getMPXNBySupplier(contracts) {
	var tableDatacell = document.createElement('td');
	var mpxnIdentifier = contracts[0].MPXN[0].Identifier;
	var contractDetailLength = contracts.length;

	for(var i = 0; i < contractDetailLength; i++) {
		var contractDetail = contracts[i];
		var mpxnLength = contractDetail.MPXN.length;

		for(var j = 0; j < mpxnLength; j++) {
			var mpxn = contractDetail.MPXN[j];
			if(mpxn.Identifier != mpxnIdentifier) {
				mpxnIdentifier = 'Multiple';
				break;
			}
		}

		if(mpxnIdentifier == 'Multiple') {
			break;
		}
	}


	tableDatacell.innerText = mpxnIdentifier;
	return tableDatacell;
}

function getMPXNByContractOrMPXN(mpxn, hasMultipleRecords) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleRecords) {
		tableDatacell.innerText = 'Multiple';
	}
	else {
		tableDatacell.innerText = mpxn;
	}

	return tableDatacell;
}

function getContractStartDateBySupplier(contracts) {
	var tableDatacell = document.createElement('td');
	var contractStartDate = contracts[0].MPXN[0].ContractStartDate;
	var contractLength = contracts.length;

	for(var i = 0; i < contractLength; i++) {
		var contract = contracts[i];
		var mpxnLength = contract.MPXN.length;

		for(var j = 0; j < mpxnLength; j++) {
			if(contract.MPXN[j].ContractStartDate != contractStartDate) {
				contractStartDate = 'Multiple';
				break;
			}
		}

		if(contractStartDate == 'Multiple') {
			break;
		}
	}

	tableDatacell.innerText = contractStartDate;
	return tableDatacell;
}

function getContractStartDateByContract(contract) {
	var tableDatacell = document.createElement('td');
	var contractStartDate = contract.MPXN[0].ContractStartDate;
	var mpxnLength = contract.MPXN.length;

	for(var j = 0; j < mpxnLength; j++) {
		if(contract.MPXN[j].ContractStartDate != contractStartDate) {
			contractStartDate = 'Multiple';
			break;
		}
	}

	tableDatacell.innerText = contractStartDate;
	return tableDatacell;
}

function getContractStartDateByMPXN(mpxn) {
	var tableDatacell = document.createElement('td');
	tableDatacell.innerText = mpxn.ContractStartDate;
	return tableDatacell;
}

function getContractEndDateBySupplier(contracts) {
	var tableDatacell = document.createElement('td');
	var contractEndDate = contracts[0].MPXN[0].ContractEndDate;
	var contractLength = contracts.length;

	for(var i = 0; i < contractLength; i++) {
		var contract = contracts[i];
		var mpxnLength = contract.MPXN.length;

		for(var j = 0; j < mpxnLength; j++) {
			if(contract.MPXN[j].ContractEndDate != contractEndDate) {
				contractEndDate = 'Multiple';
				break;
			}
		}

		if(contractEndDate == 'Multiple') {
			break;
		}
	}

	tableDatacell.innerText = contractEndDate;
	return tableDatacell;
}

function getContractEndDateByContract(contract) {
	var tableDatacell = document.createElement('td');
	var contractEndDate = contract.MPXN[0].ContractEndDate;
	var mpxnLength = contract.MPXN.length;

	for(var j = 0; j < mpxnLength; j++) {
		if(contract.MPXN[j].ContractEndDate != contractEndDate) {
			contractEndDate = 'Multiple';
			break;
		}
	}

	tableDatacell.innerText = contractEndDate;
	return tableDatacell;
}

function getContractEndDateByMPXN(mpxn) {
	var tableDatacell = document.createElement('td');
	tableDatacell.innerText = mpxn.ContractEndDate;
	return tableDatacell;
}

function getProductType() {
	var tableDatacell = document.createElement('td');
	tableDatacell.innerText = 'Out Of Contract';
	return tableDatacell;
}

function getRates(contractIndex, contractDetailIndex, mpxnIndex, canShowRates) {
	var tableDatacell = document.createElement('td');

	if(canShowRates) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'fas fa-search show-pointer');
		icon.setAttribute('onclick', 'showRates(' + contractIndex + ',' + contractDetailIndex + ',' + mpxnIndex + ')');

		tableDatacell.appendChild(icon);
	}

	return tableDatacell;
}

function getIsBusinesswiseContractBySupplier(contracts) {
	var tableDatacell = document.createElement('td');
	var isBusinesswiseContract = contracts[0].MPXN[0].IsBusinesswiseContract;
	var contractLength = contracts.length;

	for(var i = 0; i < contractLength; i++) {
		var contract = contracts[i];
		var mpxnLength = contract.MPXN.length;

		for(var j = 0; j < mpxnLength; j++) {
			if(contract.MPXN[j].IsBusinesswiseContract != isBusinesswiseContract) {
				isBusinesswiseContract = 'Multiple';
				break;
			}
		}

		if(isBusinesswiseContract == 'Multiple') {
			break;
		}
	}

	tableDatacell.innerText = isBusinesswiseContract;
	return tableDatacell;
}

function getIsBusinesswiseContractByContract(contract) {
	var tableDatacell = document.createElement('td');
	var isBusinesswiseContract = contract.MPXN[0].IsBusinesswiseContract;
	var mpxnLength = contract.MPXN.length;

	for(var i = 0; i < mpxnLength; i++) {
		if(contract.MPXN[i].IsBusinesswiseContract != isBusinesswiseContract) {
			isBusinesswiseContract = 'Multiple';
			break;
		}
	}

	tableDatacell.innerText = isBusinesswiseContract;
	return tableDatacell;
}

function getIsBusinesswiseContractByMPXN(mpxn) {
	var tableDatacell = document.createElement('td');
	tableDatacell.innerText = mpxn.IsBusinesswiseContract;
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