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

	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border-right: solid black 1px;', 'Type'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 15%; border-right: solid black 1px;', 'Identifier'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; width: 30%; border-right: solid black 1px;', 'Attribute'));
	tableRow.appendChild(createTableHeader('padding-right: 50px; border-right: solid black 1px;', 'Value'));
	tableRow.appendChild(createTableHeader('width: 5%;', ''));

	table.appendChild(tableRow);

	for(var siteCount = 0; siteCount < data.length; siteCount++) {
		var site = data[siteCount];
		displayAttributes('Site', getAttribute(site.Attributes, 'SiteName'), site.Attributes, table);

		for(var meterCount = 0; meterCount < site.Meters.length; meterCount++) {
			var meter = site.Meters[meterCount];
			displayAttributes('Meter', getAttribute(meter.Attributes, 'Identifier'), meter.Attributes, table);

			if(meter.SubMeters) {
				for(var subMeterCount = 0; subMeterCount < meter.SubMeters.length; subMeterCount++) {
					var subMeter = meter.SubMeters[subMeterCount];
					displayAttributes('Submeter', getAttribute(subMeter.Attributes, 'Identifier'), subMeter.Attributes, table);
				}
			}
		}
	}

	treeDiv.appendChild(table);
}

function createTableHeader(style, value) {
	var tableHeader = document.createElement('th');
	tableHeader.setAttribute('style', style);
	tableHeader.innerHTML = value;
	return tableHeader;
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

function openTab(evt, tabName) {
	var i, tabcontent, tablinks;
	tabcontent = document.getElementsByClassName("tabcontent");
	for (i = 0; i < tabcontent.length; i++) {
	  tabcontent[i].style.display = "none";
	}
	tablinks = document.getElementsByClassName("tablinks");
	for (i = 0; i < tablinks.length; i++) {
	  tablinks[i].className = tablinks[i].className.replace(" active", "");
	}
	document.getElementById(tabName).style.display = "block";
	evt.currentTarget.className += " active";
  }

function createCard(checkbox){
	var cardDiv = document.getElementById('cardDiv');
	var tabDiv = document.getElementById('tabDiv');
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));

	if(checkbox.checked){
		cardDiv.setAttribute('style', '');
		var button = document.createElement('button');
		button.setAttribute('class', 'tablinks');
		button.setAttribute('onclick', 'openTab(event, "' + span.id.replace('span', 'div') +'")');

		if(checkbox.getAttribute('branch') == "SubMeter") {
			button.innerHTML = checkbox.getAttribute('linkedsite').concat(' - ').concat(span.innerHTML);
		}
		else {
			button.innerHTML = span.innerHTML;
		}
		
		button.id = span.id.replace('span', 'button');
		tabDiv.appendChild(button);
	
		var newDiv = document.createElement('div');
		newDiv.setAttribute('class', 'tabcontent');
		newDiv.id = span.id.replace('span', 'div');
		newDiv.innerHTML = span.innerHTML.concat(' is a ').concat(checkbox.getAttribute('branch'));
		cardDiv.appendChild(newDiv);

		for(var i = 0; i < tabDiv.children.length; i++) {
			var style = 'width: '.concat(tabDiv.clientWidth/tabDiv.children.length).concat('px; vertical-align: middle;');
			if(i > 0) {
				style = style.concat(' border-left: solid black 1px;');
			}
			tabDiv.children[i].setAttribute('style', style);
		}
	}
	else {
		tabDiv.removeChild(document.getElementById(span.id.replace('span', 'button')));
		cardDiv.removeChild(document.getElementById(span.id.replace('span', 'div')));

		if(tabDiv.children.length == 0) {
			cardDiv.setAttribute('style', 'display: none;');
		}
		else {
			for(var i = 0; i < tabDiv.children.length; i++) {
				var style = 'width: '.concat(tabDiv.clientWidth/tabDiv.children.length).concat('px;');
				if(i < tabDiv.children.length - 1) {
					style = style.concat(' border-right: solid black 1px;');
				}
				tabDiv.children[i].setAttribute('style', style);
			}
		}
	}	
}

