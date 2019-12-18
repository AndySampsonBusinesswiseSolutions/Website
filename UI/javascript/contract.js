function showRates(row) {
	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Rate'));
	tableRow.appendChild(createTableHeader('border: solid black 1px;', 'Value'));
	table.appendChild(tableRow);

	for(var i = 0; i < 4; i++) {
		var tableRow = document.createElement('tr');
		tableRow.id = 'row' + i;

		for(var j = 0; j < 2; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(i) {
				case 0:
					if(j == 0) {
						tableDatacell.innerHTML = 'Unit Rate 1';
					}
					else {
						tableDatacell.innerHTML = '10p/kWh';
					}
					
					break;	
				case 1:
					if(j == 0) {
						tableDatacell.innerHTML = 'Unit Rate 2';
					}
					else {
						tableDatacell.innerHTML = '5p/kWh';
					}
					break;
				case 2:
					if(j == 0) {
						tableDatacell.innerHTML = 'Capacity Charge';
					}
					else {
						tableDatacell.innerHTML = '2.5p/kVa/day';
					}

					break;
				case 3:
					if(j == 0) {
						tableDatacell.innerHTML = 'Standing Charge';
					}
					else {
						tableDatacell.innerHTML = '£1/day';
					}

					break;
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
		title: 'Out Of Contract Rates For 987654'
	});
}