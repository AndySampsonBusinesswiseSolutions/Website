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
      
    // sidebar.style.height = "calc(100% - " + (reduction > 0 ? 0 : 0) + "px)";
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
    document.getElementById("openNav").style.color = "rgb(233, 233, 233)";
}
  
function closeNav(sidebar) {
    var lock = document.getElementsByClassName('lock')[0];

    if(lock.classList.contains('fa-unlock')) {
        document.getElementById("openNav").style.color = "black";
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

function updateAdditionalControls(element) {
	var additionalcontrols = element.getAttribute('additionalcontrols');

	if(!additionalcontrols) {
		return;
	}

	var listToHide = element.id.concat('List');
	var clickEventFunction = function (event) {
		updateClassOnClick(listToHide, 'listitem-hidden', '')
	};

	var controlArray = additionalcontrols.split(',');
	for(var j = 0; j < controlArray.length; j++) {
		var controlId = controlArray[j];	

		element.addEventListener('click', function (event) {
			var controlElement = document.getElementById(controlId);
			if(hasClass(this, 'fa-minus-square')) {				
				controlElement.addEventListener('click', clickEventFunction, false);
			}
			else {
				controlElement.removeEventListener('click', clickEventFunction);
			}
		});
	}	
}

function expandAdditionalLists(element) {
	var additionalLists = element.getAttribute('additionallists');

	if(!additionalLists) {
		return;
	}

	element.addEventListener('click', function (event) {
		var controlArray = additionalLists.split(',');
		for(var j = 0; j < controlArray.length; j++) {
			var controlId = controlArray[j];
			var controlElement = document.getElementById(controlId);
			updateClass(controlElement, 'listitem-hidden', '');
		}
	});		
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

function createHeaderDiv(id, headerText, isOpen = false, hasChildren = true) {
    var headerDiv = document.createElement('div');
    headerDiv.id = id;
    headerDiv.setAttribute('class', 'expander-header');
  
    var header = document.createElement('span');
    header.innerText = headerText;
  
    var headerExpander = createBranchDiv(id.replace('Header', 'Selector'), hasChildren, isOpen);
  
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

function convertMonthIdToShortCode(monthId) {
	return convertMonthIdToFullText(monthId).slice(0, 3).toUpperCase();
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

function getCategoryTexts(startDate, endDate, dateFormat) {
    var newCategories = [];
  
    for(var newDate = startDate; newDate < endDate; newDate.setDate(newDate.getDate() + 1)) {
      for(var hh = 0; hh < 48; hh++) {
        var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), dateFormat);
  
        if(!newCategories.includes(newCategoryText)) {
          newCategories.push(newCategoryText);
        }      
      }
    }
  
    return newCategories;
  }

function alertMessage() {
    alert('Something needs to happen here when you click this thing......what is it??');
}