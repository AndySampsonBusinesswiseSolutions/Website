function getClone(item){
	return JSON.parse(JSON.stringify(item));
}

function updateClassOnClick(elementId, firstClass, secondClass){
	var element = document.getElementById(elementId);
	if(hasClass(element, firstClass)){
		element.classList.remove(firstClass);

		if(secondClass != ""){
			element.classList.add(secondClass);
		}
	}
	else {
		if(secondClass != ""){
			element.classList.remove(secondClass);
		}
		
		element.classList.add(firstClass);
	}
}
  
function hasClass(elem, className) {
	return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName("fa-plus-square");
	for(var i=0; i< expanders.length; i++){
		expanders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-plus-square", "fa-minus-square")
			updateClassOnClick(this.id.concat('List'), "listitem-hidden", "")
		});
	}
}

function addArrowOnClickEvents() {
	var arrows = document.getElementsByClassName("fa-angle-double-down");
	for(var i=0; i< arrows.length; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-angle-double-down", "fa-angle-double-up")
			updateClassOnClick(this.id.replace("Arrow", "SubMenu"), "listitem-hidden", "")
		});
	}

	arrows = document.getElementsByClassName("fa-angle-double-left");
	for(var i=0; i< arrows.length; i++){
		arrows[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-angle-double-left", "fa-angle-double-right")
		});
	}

	var arrowHeaders = document.getElementsByClassName("arrow-header");
	for(var i=0; i< arrowHeaders.length; i++){
		arrowHeaders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id.concat('Arrow'), "fa-angle-double-down", "fa-angle-double-up")
			updateClassOnClick(this.id.concat('SubMenu'), "listitem-hidden", "")
		});
	}
}

function formatDate(dateToBeFormatted, format) {
	switch(format) {
		case "yyyy-MM-dd hh:mm:ss":
			var aaaa = dateToBeFormatted.getFullYear();
			var gg = dateToBeFormatted.getDate();
			var mm = (dateToBeFormatted.getMonth() + 1);
		
			if (gg < 10)
				gg = "0" + gg;
		
			if (mm < 10)
				mm = "0" + mm;
		
			var cur_day = aaaa + "-" + mm + "-" + gg;
		
			var hours = dateToBeFormatted.getHours()
			var minutes = dateToBeFormatted.getMinutes()
			var seconds = dateToBeFormatted.getSeconds();
		
			if (hours < 10)
				hours = "0" + hours;
		
			if (minutes < 10)
				minutes = "0" + minutes;
		
			if (seconds < 10)
				seconds = "0" + seconds;
		
			return cur_day + " " + hours + ":" + minutes + ":" + seconds;
		case "yyyy-MM-dd":
			var aaaa = dateToBeFormatted.getFullYear();
			var gg = dateToBeFormatted.getDate();
			var mm = (dateToBeFormatted.getMonth() + 1);
		
			if (gg < 10)
				gg = "0" + gg;
		
			if (mm < 10)
				mm = "0" + mm;
		
				return aaaa + "-" + mm + "-" + gg;
	}
}