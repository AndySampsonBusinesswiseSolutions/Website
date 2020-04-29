function setupSidebarHeight() {
    var navBar = document.getElementsByClassName('fusion-header-wrapper')[0];
    var footer = document.getElementsByClassName('footer')[0];
    var sidebar = document.getElementById("mySidenav");

    if(!sidebar) {
        return;
    }

    var navBarHeight = navBar.clientHeight;
    var footerHeight = footer.clientHeight;
    var reduction = (window.innerHeight + window.scrollY) - (document.body.offsetHeight - footerHeight);
      
    sidebar.style.height = "calc(100% - " + (reduction > 0 ? reduction : 0) + "px)";
    sidebar.style.marginTop = window.pageYOffset >= navBarHeight ? '0px' : (navBarHeight - window.pageYOffset) + 'px';
}
  
function getMousePos(e) {
      return {x:e.clientX,y:e.clientY};
}
  
function setupSidebar(e) {
    var sidebar = document.getElementById("mySidenav");
    var currentSidebarWidth = sidebar.style.width == '' ? 0 : parseInt(sidebar.style.width.replace('px', ''));
    var newSidebarWidth = (sidebar.scrollHeight > sidebar.clientHeight ? 415 : 400);
  
    if(currentSidebarWidth > 0 && newSidebarWidth != currentSidebarWidth) {
      sidebar.style.width = newSidebarWidth + "px";
    }
  
    var mousecoords = getMousePos(e);
    if(currentSidebarWidth == 0 && mousecoords.x <= 15) {
      openNav(sidebar, newSidebarWidth);
    }  
    else if(currentSidebarWidth > 0 && mousecoords.x >= newSidebarWidth) {
      closeNav(sidebar);
    }  
}
  
function lockSidebar() {
    updateClassOnClick('lock', 'fa-unlock', 'fa-lock'); 
    var lock = document.getElementsByClassName('lock')[0];
    var documentBody = document.getElementsByClassName('final-column')[0];
    var sidebar = document.getElementById("mySidenav"); 
  
    if(lock.classList.contains('fa-lock')) {
      documentBody.style.marginLeft = sidebar.style.width;
      lock.title = "Click To Unlock Sidebar";
    }
    else {
      documentBody.style.marginLeft = "0px";
      lock.title = "Click To Lock Sidebar";
    }
}
  
function openNav(sidebar, newSidebarWidth) {
    sidebar.style.width = newSidebarWidth + "px";
    document.getElementById("openNav").style.color = "#b62a51";
}
  
function closeNav(sidebar) {
    var lock = document.getElementsByClassName('lock')[0];

    if(lock.classList.contains('fa-unlock')) {
        document.getElementById("openNav").style.color = "white";
        sidebar.style.width = "0px";
    }
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
	var expanders = document.getElementsByClassName('expander');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
    addExpanderOnClickEventsByElement(expanders[i]);
  }
}

function addExpanderOnClickEventsByElement(element) {
  element.setAttribute('onclick',
    "updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square'); " +
    "updateClassOnClick(this.id.concat('List'), 'listitem-hidden', ''); " +
    "setupSidebar(event); " +
    "setupSidebarHeight();");
}

function setOpenExpanders() {
    var openExpanders = document.getElementsByClassName('openExpander');
    for(var i = 0; i < openExpanders.length; i++) {
      updateClassOnClick(openExpanders[i].id, 'fa-plus-square', 'fa-minus-square');
    }
}

function preciseRound(num, dec){
	if ((typeof num !== 'number') || (typeof dec !== 'number')) {
		return false; 
	}	

	var num_sign = num >= 0 ? 1 : -1;
		
	return Number((Math.round((num*Math.pow(10,dec))+(num_sign*0.0001))/Math.pow(10,dec)).toFixed(dec));
}

function renderChart(chartId, options) {
    var chart = new ApexCharts(document.querySelector(chartId), options);
    chart.render();
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

function getCheckedElements(inputs) {
  var elements = [];
  var inputLength = inputs.length;

  for(var i = 0; i < inputLength; i++) {
    if(inputs[i].checked) {
      elements.push(inputs[i]);
    }
  }

  return elements;
}

function commodityMatch(entity, commodity) {
    if(commodity == '') {
        return true;
    }
  
    var entityCommodities = getAttribute(entity.Attributes, 'Commodities');
    return entityCommodities && entityCommodities.includes(commodity);
}
  
function appendListItemChildren(id, hasChildren, functions, attributes, branch, isChecked = false, elementType = 'checkbox', groupName = '') {
    var li = document.createElement('li');
    li.appendChild(createBranchDiv(id, hasChildren));
  
    if(functions) {
      li.appendChild(createBranchCheckbox(id, functions, branch, elementType, groupName, isChecked));
    }  
  
    if(getAttribute(attributes, 'Icon')) {
      li.appendChild(createBranchIcon(getAttribute(attributes, 'Icon')));
    }
    
    li.appendChild(createBranchSpan(id, getAttribute(attributes, 'Name')));
  
    if(hasChildren) {
      li.appendChild(createBranchUl(id));
    }
  
    return li;
}

function createBranchUl(id, hideUl = true, isTopUl = false) {
    var ul = document.createElement('ul');
    ul.id = id + 'List';
    ul.setAttribute('class', 'format-listitem'
     + (hideUl ? ' listitem-hidden' : '')
     + (isTopUl ? ' topListItem' : ''));
    return ul;
}
  
function createBranchDiv(id, hasChildren = true, isOpen = false) {
      var branchDiv = document.createElement('i');
      branchDiv.id = id;
      branchDiv.setAttribute('class', 
        (hasChildren ? 'far fa-plus-square show-pointer' : 'far fa-times-circle') 
        + (isOpen ? ' openExpander' : '') 
        + ' expander');
      return branchDiv;
}

function createBranchCheckbox(id, functions, branch, elementType, groupName, isChecked) {
    var checkbox = document.createElement('input');
    checkbox.type = elementType;  
    checkbox.id = id.concat(elementType);
    checkbox.setAttribute('branch', branch);
    checkbox.setAttribute('name', groupName);
    checkbox.checked = isChecked;
  
    var functionArray = functions.replace(')', '').split('(');
    var functionArrayLength = functionArray.length;
    var functionName = functionArray[0];
    var functionArguments = [];
  
    functionArguments.push(checkbox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkbox.setAttribute('onclick', functionName);
    return checkbox;
}
  
function createBranchIcon(iconClass) {
    var icon = document.createElement('i');
    icon.setAttribute('class', iconClass + ' listitem-icon');
    return icon;
}

function createBranchSpan(id, innerHTML) {
    var span = document.createElement('span');
    span.id = id.concat('span');
    span.innerHTML = innerHTML;
    return span;
}

function finalisePopup(title, titleHTML, modal, span) {
    title.innerHTML = titleHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function createHeaderDiv(id, headerText, isOpen = false) {
    var headerDiv = document.createElement('div');
    headerDiv.id = id;
    headerDiv.setAttribute('class', 'expander-header');
  
    var header = document.createElement('span');
    header.innerText = headerText;
  
    var headerExpander = createBranchDiv(id.replace('Header', 'Selector'), true, isOpen);
  
    headerDiv.appendChild(header);
    headerDiv.appendChild(headerExpander);
  
    return headerDiv;
}

function getBranchCheckboxes(inputs, branch) {
    var checkBoxes = [];
    var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
      if(inputs[i].type.toLowerCase() == 'checkbox') {
        if(inputs[i].getAttribute('branch') == branch) {
          inputs[i].checked = true;
          checkBoxes.push(inputs[i]);
        }
      }
    }
  
    return checkBoxes;
}

function alertMessage() {
    alert('Something needs to happen here when you click this thing......what is it??');
}