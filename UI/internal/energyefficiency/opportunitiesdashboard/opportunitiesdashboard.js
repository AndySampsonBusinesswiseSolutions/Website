function pageLoad() {
	addExpanderOnClickEvents();
	loadDataGrids();
	setupPopup();
}

function setupPopup() {
	createTree(data, "siteDiv");

	// Get the modal
	var modal = document.getElementById("arrangeVisitPopup");

	// Get the button that opens the modal
	var btn = document.getElementById("arrangeVisitButton");

	// Get the <span> element that closes the modal
	var span = document.getElementsByClassName("close")[0];

	// When the user clicks the button, open the modal 
	btn.onclick = function() {
		modal.style.display = "block";
	}

	// When the user clicks on <span> (x), close the modal
	span.onclick = function() {
		modal.style.display = "none";
	}

	// When the user clicks anywhere outside of the modal, close it
	window.onclick = function(event) {
		if (event.target == modal) {
			modal.style.display = "none";
		}
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

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerText = "Select Sites";

    div.appendChild(header);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement) {
  var dataLength = baseData.length;
  for(var i = 0; i < dataLength; i++){
	var base = baseData[i];
	var baseName = getAttribute(base.Attributes, 'BaseName');
	var li = document.createElement('li');
	var ul = createUL();

    appendListItemChildren(li, 'Site'.concat(base.GUID), 'Site', baseName, ul, baseName, base.GUID);
    baseElement.appendChild(li);        
  }
}

function appendListItemChildren(li, id, checkboxBranch, branchOption, ul, linkedSite, guid) {
	li.appendChild(createBranchDiv(id));
	li.appendChild(createCheckbox(id, checkboxBranch, linkedSite, guid));
	li.appendChild(createTreeIcon());
	li.appendChild(createSpan(id, branchOption));
	li.appendChild(createBranchListDiv(id.concat('List'), ul));
}


function createBranchDiv(branchDivId) {
	var branchDiv = document.createElement('div');
	branchDiv.id = branchDivId;
	branchDiv.setAttribute('style', 'padding-right: 4px;');
	return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
	var branchListDiv = document.createElement('div');
	branchListDiv.id = branchListDivId;
	branchListDiv.setAttribute('class', 'listitem-hidden');
	branchListDiv.appendChild(ul);
	return branchListDiv;
}

function createUL() {
	var ul = document.createElement('ul');
	ul.setAttribute('class', 'format-listitem');
	return ul;
}

function createTreeIcon() {
	var icon = document.createElement('i');
	icon.setAttribute('class', 'fas fa-map-marker-alt');
	icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
	return icon;
}

function createSpan(spanId, innerHTML) {
	var span = document.createElement('span');
	span.id = spanId.concat('span');
	span.innerHTML = innerHTML;
	return span;
}

function createCheckbox(checkboxId, branch, linkedSite, guid) {  
	var checkBox = document.createElement('input');
	checkBox.type = 'checkbox';  
	checkBox.id = checkboxId.concat('checkbox');
	checkBox.setAttribute('Branch', branch);
	checkBox.setAttribute('LinkedSite', linkedSite);
	checkBox.setAttribute('GUID', guid);
	return checkBox;
}

function requestVisit() {
	var modal = document.getElementById("arrangeVisitPopup");
	var visitDate = document.getElementById("visitDate").value;
	var notes = document.getElementById("notes").value;

	modal.style.display = "none";
}

function loadDataGrids() {
	var futureSiteVisitData = [];
	var historicalSiteVisitData = [];
	var siteRankings = [];

	for(var i = 1; i < 6; i++) {
		var row = {
			dateOfVisit: new Date(new Date().getFullYear(), new Date().getMonth() + i, new Date().getDate()).toDateString(), 
			engineer:'En Gineer',
			notes:'<i class="fas fa-search show-pointer"></i>',
		}
		futureSiteVisitData.push(row);

		row = {
			dateOfVisit: new Date(new Date().getFullYear(), new Date().getMonth() - i, new Date().getDate()).toDateString(), 
			engineer:'En Gineer',
			notes:'<i class="fas fa-search show-pointer"></i>',
		}
		historicalSiteVisitData.push(row);
	}

	var row = {
		siteName:'Site X <i class="fas fa-search show-pointer"></i>', 
		ranking:'<div id="SiteXRanking"></div>',
		savingskWh: '10,000',
		savingsCost: '£10,000',
		capex: '£10,000',
		opex: '£10,000',
	}
	siteRankings.push(row);

	jexcel(document.getElementById('futureSiteVisitSpreadsheet'), {
		pagination:10,
		minDimensions:[3,10],
		data: futureSiteVisitData,
		columns: [
			{type:'text', width:'150px', name:'dateOfVisit', title:'Date Of Visit'},
			{type:'text', width:'100px', name:'engineer', title:'Engineer'},
			{type:'text', width:'100px', name:'notes', title:'Notes'},
		 ]
	  }); 

	jexcel(document.getElementById('historicalSiteVisitSpreadsheet'), {
		pagination:10,
		minDimensions:[3,10],
		data: historicalSiteVisitData,
		columns: [
			{type:'text', width:'150px', name:'dateOfVisit', title:'Date Of Visit'},
			{type:'text', width:'100px', name:'engineer', title:'Engineer'},
			{type:'text', width:'100px', name:'notes', title:'Notes'},
		 ]
	  }); 

	jexcel(document.getElementById('siteRankingSpreadsheet'), {
		pagination:10,
		minDimensions:[6,10],
		data: siteRankings,
		columns: [
			{type:'text', width:'165px', name:'siteName', title:'Site Name'},
			{type:'text', width:'150px', name:'ranking', title:'Ranking', readOnly: true},
			{type:'text', width:'125px', name:'savingskWh', title:'Estimated kWh<br>Savings (pa)'},
			{type:'text', width:'125px', name:'savingsCost', title:'Estimated £<br>Savings (pa)'},
			{type:'text', width:'125px', name:'capex', title:'CAPEX<br>spend'},
			{type:'text', width:'125px', name:'opex', title:'OPEX<br>spend'},
		 ]
	  }); 

	jSuites.rating(document.getElementById("SiteXRanking"), {value: 3},);
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
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
	}
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});

	updateAdditionalControls(element);
	expandAdditionalLists(element);
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