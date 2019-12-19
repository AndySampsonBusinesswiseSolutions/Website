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

function buildContractDataGrids(contracts) {
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
		var hasMultipleContracts = contract.Contract.length > 1;

		var tableRow = document.createElement('tr');
		tableRow.appendChild(getSupplier(contract.Supplier, hasMultipleContracts, true));
		tableRow.appendChild(getContractReference(contract.Contract[0].ContractReference, hasMultipleContracts, false));
		tableRow.appendChild(getMPXN(contract.Contract[0].MPXN[0].Identifier, hasMultipleContracts));
		tableRow.appendChild(getContractStartDateBySupplier(contract.Contract, hasMultipleContracts));
		tableRow.appendChild(getContractEndDateBySupplier(contract.Contract, hasMultipleContracts));
		tableRow.appendChild(getProductType());
		tableRow.appendChild(getRates(i, 0, 0, hasMultipleContracts));
		tableRow.appendChild(getIsBusinesswiseContractBySupplier(contract.Contract, hasMultipleContracts));
		table.appendChild(tableRow);

		if(hasMultipleContracts) {
			var contractDetailLength = contract.Contract.length;

			for(var j = 0; j < contractDetailLength; j++) {
				var contractDetail = contract.Contract[j];
				var hasMultipleContracts = contractDetail.MPXN.length > 1;

				var tableRow = document.createElement('tr');
				tableRow.appendChild(getSupplier('', hasMultipleContracts, false));
				tableRow.appendChild(getContractReference(contractDetail.ContractReference, hasMultipleContracts, true, contract.Supplier));
				tableRow.appendChild(getMPXN(contractDetail.MPXN[0].Identifier, hasMultipleContracts));
				tableRow.appendChild(getContractStartDateByContract(contractDetail, hasMultipleContracts));
				tableRow.appendChild(getContractEndDateByContract(contractDetail, hasMultipleContracts));
				tableRow.appendChild(getProductType());
				tableRow.appendChild(getRates(i, j, 0, hasMultipleContracts));
				tableRow.appendChild(getIsBusinesswiseContractByContract(contractDetail, hasMultipleContracts));

				tableRow.setAttribute('class', 'listitem-hidden '.concat('OutOfContract'.concat(contract.Supplier).concat('List')));
				table.appendChild(tableRow);

				if(hasMultipleContracts) {
					var mpxnLength = contractDetail.MPXN.length;

					for(var k = 0; k < mpxnLength; k++) {
						var mpxn = contractDetail.MPXN[k];

						var tableRow = document.createElement('tr');
						tableRow.appendChild(getSupplier('', false, false));
						tableRow.appendChild(getContractReference('', false, false));
						tableRow.appendChild(getMPXN(mpxn.Identifier, false));
						tableRow.appendChild(getContractStartDateByMPXN(mpxn, false));
						tableRow.appendChild(getContractEndDateByMPXN(mpxn, false));
						tableRow.appendChild(getProductType());
						tableRow.appendChild(getRates(i, j, k, false));
						tableRow.appendChild(getIsBusinesswiseContractByMPXN(mpxn, false));
						
						tableRow.setAttribute('class', 'listitem-hidden '.concat('OutOfContract'.concat(contractDetail.ContractReference).concat('List')));
						table.appendChild(tableRow);
					}
				}
			}
		}
	}

	var divToAppendTo = document.getElementById('outOfContract');
	divToAppendTo.appendChild(table);
}

function getSupplier(supplier, hasMultipleContracts, applyGroupIcon) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleContracts && applyGroupIcon) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'far fa-plus-square');
		icon.setAttribute('style', 'padding-right: 10px;');
		icon.id = 'OutOfContract'.concat(supplier)

		tableDatacell.appendChild(icon);
	}
	
	tableDatacell.appendChild(createSpan('', supplier));
	return tableDatacell;
}

function getContractReference(contractReference, hasMultipleContracts, applyGroupIcon, contractSupplier) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleContracts && applyGroupIcon) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'far fa-plus-square');
		icon.setAttribute('style', 'padding-right: 10px;');

		if(contractSupplier) {
			icon.setAttribute('additionalcontrols', 'OutOfContract'.concat(contractSupplier));
		}
		
		icon.id = 'OutOfContract'.concat(contractReference)

		tableDatacell.appendChild(icon);
	}

	if(!hasMultipleContracts || applyGroupIcon) {
		tableDatacell.appendChild(createSpan('', contractReference));
	}

	return tableDatacell;
}

function getMPXN(mpxn, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');

	if(hasMultipleContracts) {
		tableDatacell.innerText = 'Multiple';
	}
	else {
		tableDatacell.innerText = mpxn;
	}

	return tableDatacell;
}

function getContractStartDateBySupplier(contracts, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var contractStartDate = contracts[0].MPXN[0].ContractStartDate;
	var contractLength = contracts.length;

	if(hasMultipleContracts){
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
	}

	tableDatacell.innerText = contractStartDate;
	return tableDatacell;
}

function getContractStartDateByContract(contract, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var contractStartDate = contract.MPXN[0].ContractStartDate;
	var mpxnLength = contract.MPXN.length;

	if(hasMultipleContracts){
		for(var j = 0; j < mpxnLength; j++) {
			if(contract.MPXN[j].ContractStartDate != contractStartDate) {
				contractStartDate = 'Multiple';
				break;
			}
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

function getContractEndDateBySupplier(contracts, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var contractEndDate = contracts[0].MPXN[0].ContractEndDate;
	var contractLength = contracts.length;

	if(hasMultipleContracts){
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
	}

	tableDatacell.innerText = contractEndDate;
	return tableDatacell;
}

function getContractEndDateByContract(contract, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var contractEndDate = contract.MPXN[0].ContractEndDate;
	var mpxnLength = contract.MPXN.length;

	if(hasMultipleContracts){
		for(var j = 0; j < mpxnLength; j++) {
			if(contract.MPXN[j].ContractEndDate != contractEndDate) {
				contractEndDate = 'Multiple';
				break;
			}
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

function getRates(contractIndex, contractDetailIndex, mpxnIndex, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');

	if(!hasMultipleContracts) {
		var icon = document.createElement('i');
		icon.setAttribute('class', 'fas fa-search show-pointer');
		icon.setAttribute('onclick', 'showRates(' + contractIndex + ',' + contractDetailIndex + ',' + mpxnIndex + ')');

		tableDatacell.appendChild(icon);
	}

	return tableDatacell;
}

function getIsBusinesswiseContractBySupplier(contracts, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var isBusinesswiseContract = contracts[0].MPXN[0].IsBusinesswiseContract;
	var contractLength = contracts.length;

	if(hasMultipleContracts){
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
	}

	tableDatacell.innerText = isBusinesswiseContract;
	return tableDatacell;
}

function getIsBusinesswiseContractByContract(contract, hasMultipleContracts) {
	var tableDatacell = document.createElement('td');
	var isBusinesswiseContract = contract.MPXN[0].IsBusinesswiseContract;
	var mpxnLength = contract.MPXN.length;

	if(hasMultipleContracts){
		for(var i = 0; i < mpxnLength; i++) {
			if(contract.MPXN[i].IsBusinesswiseContract != isBusinesswiseContract) {
				isBusinesswiseContract = 'Multiple';
				break;
			}
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