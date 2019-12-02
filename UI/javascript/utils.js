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
	switch(format) {
		case 'yyyy-MM-dd':
			var aaaa = dateToBeFormatted.getFullYear();
			var gg = dateToBeFormatted.getDate();
			var mm = (dateToBeFormatted.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return aaaa + '-' + mm + '-' + gg;
		case 'yyyy-MM-dd hh:mm:ss':
			var hours = dateToBeFormatted.getHours()
			var minutes = dateToBeFormatted.getMinutes()
			var seconds = dateToBeFormatted.getSeconds();
		
			if (hours < 10) {
				hours = '0' + hours;
			}				
		
			if (minutes < 10) {
				minutes = '0' + minutes;
			}				
		
			if (seconds < 10) {
				seconds = '0' + seconds;
			}			
		
			return formatDate(dateToBeFormatted, 'yyyy-MM-dd') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = dateToBeFormatted.getFullYear();
			var mm = (dateToBeFormatted.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return dateToBeFormatted.getFullYear();
		case 'yyyy-MM-dd to yyyy-MM-dd':
			var startDate = getMonday(dateToBeFormatted);
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