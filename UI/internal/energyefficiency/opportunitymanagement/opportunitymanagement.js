function pageLoad() {
    var data = customer;
	createTree(data, "treeDiv", "");
    addExpanderOnClickEvents();
    setupDataGrids();
    
    document.onmousemove=function(e) {
        var mousecoords = getMousePos(e);
        if(mousecoords.x <= 25) {
          openNav();
        }  
        else if(mousecoords.x >= 400) {
          closeNav();
        }  
    };
}

function getMousePos(e) {
    return {x:e.clientX,y:e.clientY};
}

function openNav() {
    document.getElementById("mySidenav").style.width = "400px";
    document.getElementById("openNav").style.color = "#b62a51";
}

function closeNav() {
    document.getElementById("openNav").style.color = "white";
    document.getElementById("mySidenav").style.width = "0px";
}

function closeElement(elementId){
	document.getElementById(elementId).style.display="none";
}

function displayScheduleRequestedVisitPopup(row) {
	var modal = document.getElementById("scheduleRequestedVisitPopup");
	var title = document.getElementById("scheduleRequestedVisitTitle");
    var span = modal.getElementsByClassName("close")[0];
    var customer = document.getElementById('requestedVisitsCustomer' + row).innerText;
    var visitDate = document.getElementById('requestedVisitsVisitDate' + row).innerText;

    var scheduleRequestedVisitRequestedVisitDate = document.getElementById('scheduleRequestedVisitRequestedVisitDate');
    var scheduleRequestedVisitScheduledVisitDate = document.getElementById('scheduleRequestedVisitScheduledVisitDate');

    scheduleRequestedVisitRequestedVisitDate.innerText = visitDate;
    scheduleRequestedVisitScheduledVisitDate.value = "2020-04-01";

    title.innerText = 'Schedule Visit - ' + customer;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function scheduleRequestedVisit() {
    var modal = document.getElementById("scheduleRequestedVisitPopup");
    var visitDate = document.getElementById('scheduleRequestedVisitScheduledVisitDate').value;
    var engineer = document.getElementById('scheduleRequestedVisitAssignEngineer').value;

    var popupIsValid = true;
    if(visitDate == "") {
		popupIsValid = false;
		scheduleRequestedVisitScheduledVisitDateRequiredMessage.style.display = 'block';
		scheduleRequestedVisitScheduledVisitDateRequiredMessage.innerHTML = '<i class="fas fa-exclamation-circle">Please select a visit date</i>'
		window.setTimeout("closeElement('scheduleRequestedVisitScheduledVisitDateRequiredMessage');", 2000);
	}
	else {
		var requestVisitDate = new Date(visitDate);

		if(requestVisitDate < new Date()) {
			popupIsValid = false;
			scheduleRequestedVisitScheduledVisitDateRequiredMessage.style.display = 'block';
			scheduleRequestedVisitScheduledVisitDateRequiredMessage.innerHTML = '<i class="fas fa-exclamation-circle">Please select a visit date in the future</i>'
			window.setTimeout("closeElement('scheduleRequestedVisitScheduledVisitDateRequiredMessage');", 2000);
		}
		else {
			scheduleRequestedVisitScheduledVisitDateRequiredMessage.style.display = 'none';
		}
    }
    
    if(engineer == "") {
        popupIsValid = false;
		scheduleRequestedVisitAssignEngineerRequiredMessage.style.display = 'block';
		scheduleRequestedVisitAssignEngineerRequiredMessage.innerHTML = '<i class="fas fa-exclamation-circle">Please select an engineer</i>'
		window.setTimeout("closeElement('scheduleRequestedVisitAssignEngineerRequiredMessage');", 2000);
    }

	if(popupIsValid) {
		modal.style.display = "none";
	}
	else {
		event.preventDefault();
		return false;
    }
    
	modal.style.display = "none";
}

function displayRejectRequestedVisitPopup(row) {
	var modal = document.getElementById("rejectRequestedVisitPopup");
	var title = document.getElementById("rejectRequestedVisitTitle");
    var span = modal.getElementsByClassName("close")[0];
    var customer = document.getElementById('requestedVisitsCustomer' + row).innerText;
    var visitDate = document.getElementById('requestedVisitsVisitDate' + row).innerText;

    title.innerText = 'Reject Visit - ' + customer + ' - ' + visitDate;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function rejectRequestedVisit() {
    var modal = document.getElementById("rejectRequestedVisitPopup");
	modal.style.display = "none";
}

function displayRejectRequestedVisitPopup(row) {
	var modal = document.getElementById("rejectRequestedVisitPopup");
	var title = document.getElementById("rejectRequestedVisitTitle");
    var span = modal.getElementsByClassName("close")[0];
    var customer = document.getElementById('requestedVisitsCustomer' + row).innerText;
    var visitDate = document.getElementById('requestedVisitsVisitDate' + row).innerText;

    title.innerText = 'Reject Visit - ' + customer + ' - ' + visitDate;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function rejectRequestedVisit() {
    var modal = document.getElementById("rejectRequestedVisitPopup");
	modal.style.display = "none";
}

function displayApproveScheduledVisitPopup(row) {
	var modal = document.getElementById("approveScheduledVisitPopup");
	var title = document.getElementById("approveScheduledVisitTitle");
    var span = modal.getElementsByClassName("close")[0];
    var customer = document.getElementById('scheduledVisitsCustomer' + row).innerText;
    var visitDate = document.getElementById('scheduledVisitsVisitDate' + row).innerText;

    title.innerText = 'Start Visit - ' + customer + ' - ' + visitDate;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function approveScheduledVisit() {
    var modal = document.getElementById("approveScheduledVisitPopup");
    modal.style.display = "none";
    
    window.open("../CreateOpportunities", "_self");
}

function displayRejectScheduledVisitPopup(row) {
	var modal = document.getElementById("rejectScheduledVisitPopup");
	var title = document.getElementById("rejectScheduledVisitTitle");
    var span = modal.getElementsByClassName("close")[0];
    var customer = document.getElementById('scheduledVisitsCustomer' + row).innerText;
    var visitDate = document.getElementById('scheduledVisitsVisitDate' + row).innerText;

    title.innerText = 'Reject Visit - ' + customer + ' - ' + visitDate;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function rejectScheduledVisit() {
    var modal = document.getElementById("rejectScheduledVisitPopup");
	modal.style.display = "none";
}

function displayApproveOpportunityPopup(row) {
	var modal = document.getElementById("approveOpportunityPopup");
	var title = document.getElementById("approveOpportunityTitle");
	var estimatedAnnualSavings = document.getElementById("approveOpportunityEstimatedAnnualSavings");
	var span = modal.getElementsByClassName("close")[0];

    title.innerText = 'Approve Opportunity - ' + document.getElementById('opportunityName' + row).innerText;
	estimatedAnnualSavings.innerHTML = document.getElementById('estimatedSavings' + row).innerHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function approveOpportunity() {
    var modal = document.getElementById("approveOpportunityPopup");
	modal.style.display = "none";
}

function displayRejectOpportunityPopup(row) {
	var modal = document.getElementById("rejectOpportunityPopup");
	var title = document.getElementById("rejectOpportunityTitle");
	var estimatedAnnualSavings = document.getElementById("rejectOpportunityEstimatedAnnualSavings");
	var span = modal.getElementsByClassName("close")[0];

    title.innerText = 'Reject Opportunity - ' + document.getElementById('opportunityName' + row).innerText;
	estimatedAnnualSavings.innerHTML = document.getElementById('estimatedSavings' + row).innerHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function rejectOpportunity() {
    var modal = document.getElementById("rejectOpportunityPopup");
	modal.style.display = "none";
}

function displayCloseOpportunityPopup(row) {
	var modal = document.getElementById("closeOpportunityPopup");
	var title = document.getElementById("closeOpportunityTitle");
	var estimatedAnnualSavings = document.getElementById("closeOpportunityEstimatedAnnualSavings");
	var span = modal.getElementsByClassName("close")[0];

    title.innerText = 'Close Opportunity - ' + document.getElementById('opportunityName' + row).innerText;
	estimatedAnnualSavings.innerHTML = document.getElementById('estimatedSavings' + row).innerHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function closeOpportunity() {
    var modal = document.getElementById("closeOpportunityPopup");
	modal.style.display = "none";
}

function setupDataGrids() {
    setupRequestedVisitsDataGrid();
    setupScheduledVisitsDataGrid();
    setupRecommendedOpportunitiesDataGrid();
    setupPendingActiveOpportunitiesDataGrid();
    setupRejectedOpportunitiesDataGrid();
}

function setupRequestedVisitsDataGrid() {
    var data = [];
    var row = {
        customer:'<div id="requestedVisitsCustomer1">David Ford Trading Ltd</div>', 
		site:'Site X',
        visitDate:'<div id="requestedVisitsVisitDate1">01/04/2020</div>',
        manageVisit: '<button class="show-pointer btn approve" onclick="displayScheduleRequestedVisitPopup(1)">Schedule Visit</button>'
                    +'<button class="show-pointer btn reject" onclick="displayRejectRequestedVisitPopup(1)">Reject Visit</button>'
	}
    data.push(row);
    
    jexcel(document.getElementById('requestedVisitsSpreadsheet'), {
		pagination:5,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: [
            {type:'text', width:'210px', name:'customer', title:'Customer', readOnly: true},
            {type:'text', width:'210px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'210px', name:'visitDate', title:'Requested Visit Date', readOnly: true},
            {type:'text', width:'215px', name:'manageVisit', title:'Manage Visit', readOnly: true},
		 ]
	  }); 
}

function setupScheduledVisitsDataGrid() {
    var data = [];
    var row = {
        customer:'<div id="scheduledVisitsCustomer1">David Ford Trading Ltd</div>', 
		site:'Site X',
        engineer:'En Gineer',
        visitDate:'<div id="scheduledVisitsVisitDate1">01/04/2020</div>',
        manageVisit: '<button class="show-pointer btn approve" onclick="displayApproveScheduledVisitPopup(1)">Start Visit</button>'
                    +'<button class="show-pointer btn reject" onclick="displayRejectScheduledVisitPopup(1)">Reject Visit</button>'
	}
    data.push(row);
    
    jexcel(document.getElementById('scheduledVisitsSpreadsheet'), {
		pagination:5,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: [
            {type:'text', width:'165px', name:'customer', title:'Customer', readOnly: true},
            {type:'text', width:'165px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'165px', name:'engineer', title:'Engineer', readOnly: true},
            {type:'text', width:'165px', name:'visitDate', title:'Scheduled Visit Date', readOnly: true},
            {type:'text', width:'185px', name:'manageVisit', title:'Manage Visit', readOnly: true},
		 ]
	  }); 
}

function setupRecommendedOpportunitiesDataGrid() {
    var data = [];
    var row = {
        customer:'David Ford Trading Ltd', 
        opportunityType:'Custom',
        opportunityName:'<div id="opportunityName1">LED Lighting</div>',
		site:'Site X',
        meter:'12345678910125',
        subMeter:'Sub Meter 2',
        engineer:'En Gineer',
        estimatedStartDate:'01/04/2020',
        estimatedFinishDate:'30/06/2020',
        percentageSaving:'10%',
        estimatedCost:'£100,000',
        estimatedSavings:'<div id="estimatedSavings1">kWh: 10,000<br>£: £15,000</div>',
        approveReject: '<button class="show-pointer btn approve" onclick="displayApproveOpportunityPopup(1)">Approve Opportunity</button>'
                          +'<button class="show-pointer btn reject" onclick="displayRejectOpportunityPopup(1)">Reject Opportunity</button>',
        manageOpportunity: '<button class="show-pointer btn">Manage Opportunity</button>'
                          +'<button class="show-pointer btn" onclick="approveScheduledVisit()">Add Opportunities</button>'
	}
    data.push(row);
    
    jexcel(document.getElementById('recommendedOpportunitiesSpreadsheet'), {
		pagination:5,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: [
            {type:'text', width:'150px', name:'customer', title:'Customer', readOnly: true},
            {type:'text', width:'100px', name:'opportunityType', title:'Opportunity<Br>Type', readOnly: true},
            {type:'text', width:'100px', name:'opportunityName', title:'Opportunity<br>Name', readOnly: true},
            {type:'text', width:'150px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'100px', name:'subMeter', title:'Sub Meter', readOnly: true},
            {type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
            {type:'text', width:'105px', name:'estimatedStartDate', title:'Estimated<br>Start Date', readOnly: true},
            {type:'text', width:'105px', name:'estimatedFinishDate', title:'Estimated<br>Finish Date', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'120px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'125px', name:'estimatedSavings', title:'Estimated <br>Savings (pa)', readOnly: true},
            {type:'text', width:'197px', name:'manageOpportunity', title:'Manage Opportunity', readOnly: true},
            {type:'text', width:'197px', name:'approveReject', title:'Approve/Reject<br>Opportunity', readOnly: true},
		 ]
	  }); 
}

function setupPendingActiveOpportunitiesDataGrid() {
    var data = [];
    var row = {
        customer:'David Ford Trading Ltd', 
        opportunityType:'Custom',
        opportunityName:'LED Lighting',
		site:'Site X',
        meter:'12345678910125',
        subMeter:'Sub Meter 2',
        engineer:'En Gineer',
        estimatedStartDate:'01/04/2020',
        estimatedFinishDate:'30/06/2020',
        percentageSaving:'10%',
        estimatedCost:'£100,000',
        estimatedSavings:'kWh: 10,000<br>£: £15,000',
        close: '<button class="show-pointer btn reject" onclick="displayCloseOpportunityPopup(1)">Close Opportunity</button>',
        manageOpportunity: '<button class="show-pointer btn">Manage Opportunity</button>'
                          +'<button class="show-pointer btn" onclick="approveScheduledVisit()">Add Opportunities</button>'
	}
    data.push(row);
    
    jexcel(document.getElementById('pendingActiveOpportunitiesSpreadsheet'), {
		pagination:5,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: [
            {type:'text', width:'150px', name:'customer', title:'Customer', readOnly: true},
            {type:'text', width:'100px', name:'opportunityType', title:'Opportunity<Br>Type', readOnly: true},
            {type:'text', width:'100px', name:'opportunityName', title:'Opportunity<br>Name', readOnly: true},
            {type:'text', width:'150px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'100px', name:'subMeter', title:'Sub Meter', readOnly: true},
            {type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
            {type:'text', width:'105px', name:'estimatedStartDate', title:'Estimated<br>Start Date', readOnly: true},
            {type:'text', width:'105px', name:'estimatedFinishDate', title:'Estimated<br>Finish Date', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'120px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'125px', name:'estimatedSavings', title:'Estimated <br>Savings (pa)', readOnly: true},
            {type:'text', width:'197px', name:'manageOpportunity', title:'Manage Opportunity', readOnly: true},
            {type:'text', width:'197px', name:'close', title:'Close<br>Opportunity', readOnly: true},
		 ]
	  }); 
}

function setupRejectedOpportunitiesDataGrid() {
    var data = [];
    var row = {
        customer:'David Ford Trading Ltd', 
        opportunityType:'Custom',
        opportunityName:'LED Lighting',
		site:'Site X',
        meter:'12345678910125',
        subMeter:'Sub Meter 2',
        engineer:'En Gineer',
        estimatedStartDate:'01/04/2020',
        estimatedFinishDate:'30/06/2020',
        percentageSaving:'10%',
        estimatedCost:'£100,000',
        estimatedSavings:'kWh: 10,000<br>£: £15,000',
        notes: 'Rejected due to site being sold',
	}
    data.push(row);
    
    jexcel(document.getElementById('rejectedOpportunitiesSpreadsheet'), {
		pagination:5,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: [
            {type:'text', width:'150px', name:'customer', title:'Customer', readOnly: true},
            {type:'text', width:'100px', name:'opportunityType', title:'Opportunity<Br>Type', readOnly: true},
            {type:'text', width:'100px', name:'opportunityName', title:'Opportunity<br>Name', readOnly: true},
            {type:'text', width:'150px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'100px', name:'subMeter', title:'Sub Meter', readOnly: true},
            {type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
            {type:'text', width:'105px', name:'estimatedStartDate', title:'Estimated<br>Start Date', readOnly: true},
            {type:'text', width:'105px', name:'estimatedFinishDate', title:'Estimated<br>Finish Date', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'120px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'125px', name:'estimatedSavings', title:'Estimated <br>Savings (pa)', readOnly: true},
            {type:'text', width:'394px', name:'notes', title:'Notes', readOnly: true},
		 ]
	  }); 
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'CustomerName');
        var li = document.createElement('li');
        var ul = createUL();

        buildChildCustomer(base.ChildCustomers, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, ul, baseName, base.GUID, base.ChildCustomers.length > 0);

        baseElement.appendChild(li);        
    }
}

function buildChildCustomer(childCustomers, baseElement, checkboxFunction, linkedSite) {
    var childCustomersLength = childCustomers.length;
    for(var i = 0; i < childCustomersLength; i++) {
        var childCustomer = childCustomers[i];
        var li = document.createElement('li');
        var ul = createUL();

        var hasChildren = false;
        if(childCustomer.childCustomers) {
            hasChildren = childCustomer.ChildCustomers.length > 0;
        }

        appendListItemChildren(li, 'ChildCustomer'.concat(branchCount), checkboxFunction, 'ChildCustomer', childCustomer.CustomerName, ul, linkedSite, '', hasChildren);

        baseElement.appendChild(li);
        branchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid, hasChildren) {
    li.appendChild(createBranchDiv(id, hasChildren));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(branchOption));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function createBranchDiv(branchDivId, hasChildren) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(hasChildren) {
        branchDiv.setAttribute('class', 'far fa-plus-square');
    }
    else {
        branchDiv.setAttribute('class', 'far fa-times-circle');
    }
    
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

function createTreeIcon(branch) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch));
    icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
    return icon;
}

function createSpan(spanId, innerHTML) {
    var span = document.createElement('span');
    span.id = spanId.concat('span');
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid) {
    var functionArray = checkboxFunction.replace(')', '').split('(');
    var functionArrayLength = functionArray.length;
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement('input');
    checkBox.type = 'checkbox';  
    checkBox.id = checkboxId.concat('checkbox');
    checkBox.setAttribute('Branch', branch);
    checkBox.setAttribute('LinkedSite', linkedSite);
    checkBox.setAttribute('GUID', guid);

    functionArguments.push(checkBox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function getIconByBranch(branch) {
    return 'fas fa-customer';
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
    
    updateClassOnClick('recommendedOpportunities', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('pendingActiveOpportunities', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
}