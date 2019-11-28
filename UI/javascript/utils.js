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