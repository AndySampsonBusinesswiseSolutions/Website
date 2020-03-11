function pageLoad() {
	addExpanderOnClickEvents();
	loadDataGrids();
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