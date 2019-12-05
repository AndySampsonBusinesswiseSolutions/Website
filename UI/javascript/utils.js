function getClone(item){
	return JSON.parse(JSON.stringify(item));
}

function updateClassOnClick(elementId, firstClass, secondClass){
	var element = document.getElementById(elementId);
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
	for(var i=0; i< expanders.length; i++){
		expanders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
			updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
		});
	}
}

function addArrowOnClickEvents() {
	var arrows = document.getElementsByClassName('fa-angle-double-down');
	for(var i=0; i< arrows.length; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, 'fa-angle-double-down', 'fa-angle-double-up')
			updateClassOnClick(this.id.replace('Arrow', 'SubMenu'), 'listitem-hidden', '')
		});
	}

	arrows = document.getElementsByClassName('fa-angle-double-left');
	for(var i=0; i< arrows.length; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, 'fa-angle-double-left', 'fa-angle-double-right')
		});
	}

	var arrowHeaders = document.getElementsByClassName('arrow-header');
	for(var i=0; i< arrowHeaders.length; i++){
		arrowHeaders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id.concat('Arrow'), 'fa-angle-double-down', 'fa-angle-double-up')
			updateClassOnClick(this.id.concat('SubMenu'), 'listitem-hidden', '')
		});
	}
}

function formatDate(dateToBeFormatted, format) {
	var baseDate = new Date(dateToBeFormatted);

	switch(format) {
		case 'yyyy-MM-dd':
			var aaaa = baseDate.getFullYear();
			var gg = baseDate.getDate();
			var mm = (baseDate.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return aaaa + '-' + mm + '-' + gg;
		case 'yyyy-MM-dd hh:mm:ss':
			var hours = baseDate.getHours()
			var minutes = baseDate.getMinutes()
			var seconds = baseDate.getSeconds();
		
			if (hours < 10) {
				hours = '0' + hours;
			}				
		
			if (minutes < 10) {
				minutes = '0' + minutes;
			}				
		
			if (seconds < 10) {
				seconds = '0' + seconds;
			}			
		
			return formatDate(baseDate, 'yyyy-MM-dd') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return baseDate.getFullYear();
		case 'yyyy-MM-dd to yyyy-MM-dd':
			var startDate = getMonday(baseDate);
			var endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + 6);

			return formatDate(startDate, 'yyyy-MM-dd') + ' to ' + formatDate(endDate, 'yyyy-MM-dd')
	}
}

function convertMonthIdToFullText(monthId) {
	switch(monthId) {
		case 1:
			return 'January';
		case 2:
			return 'February';
		case 3:
			return 'March';
		case 4:
			return 'April';
		case 5:
			return 'May';
		case 6:
			return 'June';
		case 7:
			return 'July';
		case 8:
			return 'August';
		case 9:
			return 'September';
		case 10:
			return 'October';
		case 11:
			return 'November';
		case 12:
			return 'December';
	}
}

function convertMonthIdToShortCode(monthId) {
	return convertMonthIdToFullText(monthId).slice(0, 3).toUpperCase();
}

function getMonday(date) {
	var mondayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
	var day = mondayDate.getDay() || 7;  

	if( day !== 1 ) {
		mondayDate.setHours(-24 * (day - 1)); 
	}
	
	return mondayDate;
}

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

function resizeFinalColumns(windowWidthReduction){
	var finalColumns = document.getElementsByClassName('final-column');
	var elementWidth = window.innerWidth - windowWidthReduction;
  
	for(var i=0; i<finalColumns.length; i++){
	  finalColumns[i].setAttribute('style', 'width: '+elementWidth+'px;');
	}
  }

function buildDataTable(){
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.setAttribute('style', 'width: 100%;');

	var tableRow = document.createElement('tr');
	tableRow.setAttribute('style', 'border-bottom: solid black 1px;');

	var typeTableHeader = document.createElement('th');
	typeTableHeader.setAttribute('style', 'padding-right: 50px; width: 15%; border-right: solid black 1px;');
	typeTableHeader.innerHTML = 'Type';
	tableRow.appendChild(typeTableHeader);

	var identifierTableHeader = document.createElement('th');
	identifierTableHeader.setAttribute('style', 'padding-right: 50px; width: 15%; border-right: solid black 1px;');
	identifierTableHeader.innerHTML = 'Identifier';
	tableRow.appendChild(identifierTableHeader);

	var attributeTableHeader = document.createElement('th');
	attributeTableHeader.setAttribute('style', 'padding-right: 50px; width: 30%; border-right: solid black 1px;');
	attributeTableHeader.innerHTML = 'Attribute';
	tableRow.appendChild(attributeTableHeader);

	var valueTableHeader = document.createElement('th');
	valueTableHeader.setAttribute('style', 'padding-right: 50px; border-right: solid black 1px;');
	valueTableHeader.innerHTML = 'Value';
	tableRow.appendChild(valueTableHeader);

	var actionTableHeader = document.createElement('th');
	actionTableHeader.setAttribute('style', 'width: 5%;');
	tableRow.appendChild(actionTableHeader);

	table.appendChild(tableRow);

	for(var siteCount = 0; siteCount < data.length; siteCount++) {
		var site = data[siteCount];
		displayAttributes('Site', site.SiteName, site.Attributes, table);

		for(var meterCount = 0; meterCount < site.Meters.length; meterCount++) {
			var meter = site.Meters[meterCount];
			displayAttributes('Meter', meter.Identifier, meter.Attributes, table);

			if(meter.SubMeters) {
				for(var subMeterCount = 0; subMeterCount < meter.SubMeters.length; subMeterCount++) {
					var subMeter = meter.SubMeters[subMeterCount];
					displayAttributes('Submeter', subMeter.Identifier, subMeter.Attributes, table);
				}
			}
		}
	}

	treeDiv.appendChild(table);
}

function displayAttributes(type, identifier, attributes, table) {
	if(!attributes) {
		return;
	}

	for(var i = 0; i < attributes.length; i++) {
		var tableRow = document.createElement('tr');

		for(var j = 0; j < 4; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border-right: solid black 1px;');

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
					break;
				case 3:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		var tableDatacell = document.createElement('td');

		var editIcon = document.createElement('i');
		editIcon.setAttribute('class', 'fas fa-edit');

		var deleteIcon = document.createElement('i');
		deleteIcon.setAttribute('class', 'fas fa-trash-alt');

		tableDatacell.appendChild(editIcon);
		tableDatacell.appendChild(deleteIcon);

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}