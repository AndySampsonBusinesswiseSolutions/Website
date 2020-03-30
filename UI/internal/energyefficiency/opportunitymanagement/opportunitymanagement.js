function pageLoad() {
	createTree(customer, "treeDiv", "");
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

function finalisePopup(title, titleHTML, modal, span) {
    title.innerHTML = titleHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
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

    finalisePopup(title, 'Schedule Visit<br><br>' + customer, modal, span);
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

    finalisePopup(title, 'Reject Visit<br><br>' + customer + ' - ' + visitDate, modal, span);
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

    finalisePopup(title, 'Start Visit<br><br>' + customer + ' - ' + visitDate, modal, span);
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

    finalisePopup(title, 'Reject Visit<br><br>' + customer + ' - ' + visitDate, modal, span);
}

function rejectScheduledVisit() {
    var modal = document.getElementById("rejectScheduledVisitPopup");
	modal.style.display = "none";
}

function displayApproveOpportunityPopup(row) {
	var modal = document.getElementById("approveOpportunityPopup");
	var title = document.getElementById("approveOpportunityTitle");
    var span = modal.getElementsByClassName("close")[0];
    
    var customer = document.getElementById('recommendedOpportunitiesCustomer' + row).innerText
    var opportunityName = document.getElementById('recommendedOpportunitiesOpportunityName' + row).innerText

    var estimatedAnnualSavings = document.getElementById("approveOpportunityEstimatedAnnualSavings");
    estimatedAnnualSavings.innerHTML = document.getElementById('recommendedOpportunitiesEstimatedSavings' + row).innerHTML;
    
    finalisePopup(title, 'Approve Opportunity<br><br>' + customer + ' - ' + opportunityName, modal, span);
}

function approveOpportunity() {
    var modal = document.getElementById("approveOpportunityPopup");
	modal.style.display = "none";
}

function displayRejectOpportunityPopup(row) {
	var modal = document.getElementById("rejectOpportunityPopup");
	var title = document.getElementById("rejectOpportunityTitle");
    var span = modal.getElementsByClassName("close")[0];
    
    var customer = document.getElementById('recommendedOpportunitiesCustomer' + row).innerText
    var opportunityName = document.getElementById('recommendedOpportunitiesOpportunityName' + row).innerText

    var estimatedAnnualSavings = document.getElementById("rejectOpportunityEstimatedAnnualSavings");
    estimatedAnnualSavings.innerHTML = document.getElementById('recommendedOpportunitiesEstimatedSavings' + row).innerHTML;
    
    finalisePopup(title, 'Reject Opportunity<br><br>' + customer + ' - ' + opportunityName, modal, span);
}

function rejectOpportunity() {
    var modal = document.getElementById("rejectOpportunityPopup");
	modal.style.display = "none";
}

function displayManageOpportunityPopup(type, row) {
	var modal = document.getElementById("manageOpportunityPopup");
	var title = document.getElementById("manageOpportunityTitle");
    var span = modal.getElementsByClassName("close")[0];

    var data = [];

    data.push(document.getElementById(type + 'OpportunitiesCustomer' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesOpportunityType' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesOpportunityName' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesSite' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesMeter' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesSubMeter' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesEngineer' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesEstimatedStartDate' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesEstimatedFinishDate' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesPercentageSaving' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesEstimatedCost' + row).innerHTML);

    var savings = document.getElementById(type + 'OpportunitiesEstimatedSavings' + row).innerHTML;
    data.push(savings.split('<br>')[0].replace('kWh: ', ''));
    data.push(savings.split('<br>')[1].replace('£: ', ''));

    var detailSpans = modal.getElementsByClassName('manageOpportunityOpportunityDetailSpan');
    var detailLength = detailSpans.length;

    for(var i = 0; i < detailLength; i++) {
        detailSpans[i].innerHTML = data[i];
    }

    finalisePopup(title, 'Manage Opportunity<br><br>' + data[0] + ' - ' + data[2], modal, span);
    buildGanttChart();
}

function manageOpportunity() {
    var modal = document.getElementById("manageOpportunityPopup");
	modal.style.display = "none";
}

function displayCloseOpportunityPopup(row) {
	var modal = document.getElementById("closeOpportunityPopup");
	var title = document.getElementById("closeOpportunityTitle");
    var span = modal.getElementsByClassName("close")[0];
    
    var customer = document.getElementById('pendingActiveOpportunitiesCustomer' + row).innerText
    var opportunityName = document.getElementById('pendingActiveOpportunitiesOpportunityName' + row).innerText

    var estimatedAnnualSavings = document.getElementById("closeOpportunityEstimatedAnnualSavings");
    estimatedAnnualSavings.innerHTML = document.getElementById('pendingActiveOpportunitiesEstimatedSavings' + row).innerHTML;
    
    finalisePopup(title, 'Close Opportunity<br><br>' + customer + ' - ' + opportunityName, modal, span);
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
        customer:'<div id="recommendedOpportunitiesCustomer1">David Ford Trading Ltd</div>', 
        opportunityType:'<div id="recommendedOpportunitiesOpportunityType1">Custom</div>',
        opportunityName:'<div id="recommendedOpportunitiesOpportunityName1">LED Lighting 2</div>',
		site:'<div id="recommendedOpportunitiesSite1">Site X</div>',
        meter:'<div id="recommendedOpportunitiesMeter1">1234567890125</div>',
        subMeter:'<div id="recommendedOpportunitiesSubMeter1">Sub Meter</div>',
        engineer:'<div id="recommendedOpportunitiesEngineer1">En Gineer</div>',
        estimatedStartDate:'<div id="recommendedOpportunitiesEstimatedStartDate1">01/04/2020</div>',
        estimatedFinishDate:'<div id="recommendedOpportunitiesEstimatedFinishDate1">30/06/2020</div>',
        percentageSaving:'<div id="recommendedOpportunitiesPercentageSaving1">10%</div>',
        estimatedCost:'<div id="recommendedOpportunitiesEstimatedCost1">£100,000</div>',
        estimatedSavings:'<div id="recommendedOpportunitiesEstimatedSavings1">kWh: 10,000<br>£: £15,000</div>',
        approveReject: '<button class="show-pointer btn approve" onclick="displayApproveOpportunityPopup(1)">Approve Opportunity</button>'
                          +'<button class="show-pointer btn reject" onclick="displayRejectOpportunityPopup(1)">Reject Opportunity</button>',
        manageOpportunity: '<button class="show-pointer btn" onclick="displayManageOpportunityPopup(' + "'recommended'" +', 1)">Manage Opportunity</button>'
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
        customer:'<div id="pendingActiveOpportunitiesCustomer1">David Ford Trading Ltd</div>', 
        opportunityType:'<div id="pendingActiveOpportunitiesOpportunityType1">Custom</div>',
        opportunityName:'<div id="pendingActiveOpportunitiesOpportunityName1">LED Lighting</div>',
		site:'<div id="pendingActiveOpportunitiesSite1">Site X</div>',
        meter:'<div id="pendingActiveOpportunitiesMeter1">12345678910124</div>',
        subMeter:'<div id="pendingActiveOpportunitiesSubMeter1">Sub Meter</div>',
        engineer:'<div id="pendingActiveOpportunitiesEngineer1">En Gineer</div>',
        estimatedStartDate:'<div id="pendingActiveOpportunitiesEstimatedStartDate1">01/04/2020</div>',
        estimatedFinishDate:'<div id="pendingActiveOpportunitiesEstimatedFinishDate1">30/06/2020</div>',
        percentageSaving:'<div id="pendingActiveOpportunitiesPercentageSaving1">15%</div>',
        estimatedCost:'<div id="pendingActiveOpportunitiesEstimatedCost1">£150,000</div>',
        estimatedSavings:'<div id="pendingActiveOpportunitiesEstimatedSavings1">kWh: 20,000<br>£: £25,000</div>',
        close: '<button class="show-pointer btn reject" onclick="displayCloseOpportunityPopup(1)">Close Opportunity</button>',
        manageOpportunity: '<button class="show-pointer btn" onclick="displayManageOpportunityPopup(' + "'pendingActive'" +', 1)">Manage Opportunity</button>'
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
        meter:'1234567890125',
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
    ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Customers/Sites <i class="far fa-plus-square" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
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
    
    updateClassOnClick('rejectedOpportunities', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('recommendedOpportunities', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('pendingActiveOpportunities', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('treeDivSelector', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('opportunityStatusSelector', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('requestedVisits', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('scheduledVisits', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
}

function buildGanttChart() {
    var ganttChart = document.getElementById("ganttChart");
    clearElement(ganttChart);

    $(function () {
        var tableContainer = document.getElementById('spreadsheet');

        $("#ganttChart").ganttView({ 
            data: ganttData,
            slideWidth: tableContainer.clientWidth
        });
    });
}

(function (jQuery) {
	
    jQuery.fn.ganttView = function () {
    	
    	var args = Array.prototype.slice.call(arguments);
    	
    	if (args.length == 1 && typeof(args[0]) == "object") {
        	build.call(this, args[0]);
    	}
    	
    	if (args.length == 2 && typeof(args[0]) == "string") {
    		handleMethod.call(this, args[0], args[1]);
    	}
    };
    
    function build(options) {
    	
    	var els = this;
        var defaults = {
            showWeekends: true,
            cellWidth: 20,
            cellHeight: 50,
            slideWidth: 250,
            vHeaderWidth: 100,
            behavior: {
            	clickable: true//,
            	//draggable: true,
            	//resizable: true
            }
        };
        
        var opts = jQuery.extend(true, defaults, options);

		if (opts.data) {
			build();
		} else if (opts.dataUrl) {
			jQuery.getJSON(opts.dataUrl, function (data) { opts.data = data; build(); });
		}

		function build() {
			
			var minDays = 30;// Math.floor((opts.slideWidth / opts.cellWidth)  + 5);
			var startEnd = DateUtils.getBoundaryDatesFromData(opts.data, minDays);
			opts.start = startEnd[0];
			opts.end = startEnd[1];
			
	        els.each(function () {

	            var container = jQuery(this);
	            var div = jQuery("<div>", { "class": "ganttview" });
	            new Chart(div, opts).render();
				container.append(div);
	            container.css("width", "100%");
	            
	            new Behavior(container, opts).apply();
	        });
		}
    }

	function handleMethod(method, value) {
		
		if (method == "setSlideWidth") {
			var div = $("div.ganttview", this);
			div.each(function () {
				var vtWidth = $("div.ganttview-vtheader", div).outerWidth();
				$(div).width(vtWidth + value + 1);
				$("div.ganttview-slide-container", this).width(value);
			});
		}
	}

	var Chart = function(div, opts) {
		
		function render() {
			addVtHeader(div, opts.data, opts.cellHeight);

            var slideDiv = jQuery("<div>", {
                "class": "ganttview-slide-container",
                "css": { "width": "79%" }
            });
			
            dates = getDates(opts.start, opts.end);
            addHzHeader(slideDiv, dates, opts.cellWidth);
            addGrid(slideDiv, opts.data, dates, opts.cellWidth, opts.showWeekends);
            addBlockContainers(slideDiv, opts.data);
            addBlocks(slideDiv, opts.data, opts.cellWidth, opts.start);
            div.append(slideDiv);
            applyLastClass(div.parent());
		}
		
		var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

		// Creates a 3 dimensional array [year][month][day] of every day 
		// between the given start and end dates
        function getDates(start, end) {
            var dates = [];
			dates[start.getFullYear()] = [];
			dates[start.getFullYear()][start.getMonth()] = [start]
			var last = start;
			while (last.compareTo(end) == -1) {
				var next = last.clone().addDays(1);
				if (!dates[next.getFullYear()]) { dates[next.getFullYear()] = []; }
				if (!dates[next.getFullYear()][next.getMonth()]) { 
					dates[next.getFullYear()][next.getMonth()] = []; 
				}
				dates[next.getFullYear()][next.getMonth()].push(next);
				last = next;
			}
			return dates;
        }

        function addVtHeader(div, data, cellHeight) {
            var headerDiv = jQuery("<div>", { "class": "ganttview-vtheader" });
            for (var i = 0; i < data.length; i++) {
                var vtHeaderDiv = jQuery("<div>");
                vtHeaderDiv.append(data[i].name);
                vtHeaderDiv.append(jQuery("<br>"));

                for(var j = 0; j < data[i].sites.length; j++) {
                    var siteDiv = jQuery("<div>", { "css": { "padding-left" : "5px"}});
                    siteDiv.append(data[i].sites[j].name);
                    siteDiv.append(jQuery("<br>"));

                    for(var k = 0; k < data[i].sites[j].meters.length; k++) {
                        var meterDiv = jQuery("<div>", { "css": { "padding-left" : "10px"}});
                        meterDiv.append(data[i].sites[j].meters[k].identifier);
                        meterDiv.append(jQuery("<br>"));

                        siteDiv.append(meterDiv);
                    }

                    vtHeaderDiv.append(siteDiv);
                }

                var itemDiv = jQuery("<div>", { "class": "ganttview-vtheader-item" });
                itemDiv.append(jQuery("<div>", {
                    "class": "ganttview-vtheader-item-name",
                    "css": { "height": (data[i].series.length * cellHeight) + "px" }
                }).append(vtHeaderDiv));
                var seriesDiv = jQuery("<div>", { "class": "ganttview-vtheader-series" });
                for (var j = 0; j < data[i].series.length; j++) {
                    seriesDiv.append(jQuery("<div>", { "class": "ganttview-vtheader-series-name" })
						.append(data[i].series[j].name));
                }
                itemDiv.append(seriesDiv);
                headerDiv.append(itemDiv);
            }
            div.append(headerDiv);
        }

        function addHzHeader(div, dates, cellWidth) {
            var headerDiv = jQuery("<div>", { "class": "ganttview-hzheader" });
            var monthsDiv = jQuery("<div>", { "class": "ganttview-hzheader-months" });
            var daysDiv = jQuery("<div>", { "class": "ganttview-hzheader-days" });
            var totalW = 0;
			for (var y in dates) {
				for (var m in dates[y]) {
					var w = dates[y][m].length * cellWidth;
					totalW = totalW + w;
					monthsDiv.append(jQuery("<div>", {
						"class": "ganttview-hzheader-month",
						"css": { "width": w + "px" }
					}).append(monthNames[m] + " " + y));
					for (var d in dates[y][m]) {
						daysDiv.append(jQuery("<div>", { "class": "ganttview-hzheader-day" })
							.append(dates[y][m][d].getDate()));
					}
				}
			}
            monthsDiv.css("width", totalW + "px");
            daysDiv.css("width", totalW + "px");
            headerDiv.append(monthsDiv).append(daysDiv);
            div.append(headerDiv);
        }

        function addGrid(div, data, dates, cellWidth, showWeekends) {
            var gridDiv = jQuery("<div>", { "class": "ganttview-grid" });
            var rowDiv = jQuery("<div>", { "class": "ganttview-grid-row" });
			for (var y in dates) {
				for (var m in dates[y]) {
					for (var d in dates[y][m]) {
						var cellDiv = jQuery("<div>", { "class": "ganttview-grid-row-cell" });
						if (DateUtils.isWeekend(dates[y][m][d]) && showWeekends) { 
							cellDiv.addClass("ganttview-weekend"); 
                        }
                        if (DateUtils.isToday(dates[y][m][d])) { 
							cellDiv.addClass("ganttview-today"); 
						}
						rowDiv.append(cellDiv);
					}
				}
			}
            var w = jQuery("div.ganttview-grid-row-cell", rowDiv).length * cellWidth;
            rowDiv.css("width", w + "px");
            gridDiv.css("width", w + "px");
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    gridDiv.append(rowDiv.clone());
                }
            }
            div.append(gridDiv);
        }

        function addBlockContainers(div, data) {
            var blocksDiv = jQuery("<div>", { "class": "ganttview-blocks" });
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    blocksDiv.append(jQuery("<div>", { "class": "ganttview-block-container" }));
                }
            }
            div.append(blocksDiv);
        }

        function addBlocks(div, data, cellWidth, start) {
            var rows = jQuery("div.ganttview-blocks div.ganttview-block-container", div);
            var rowIdx = 0;
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    var series = data[i].series[j];
                    var size = DateUtils.daysBetween(series.start, series.end) + 1;
					var offset = DateUtils.daysBetween(start, series.start);
					var block = jQuery("<div>", {
                        "class": "ganttview-block",
                        "title": series.name + ": " + size + " days",
                        "css": {
                            "width": ((size * cellWidth) - 9) + "px",
                            "margin-left": ((offset * cellWidth) + 3) + "px"
                        }
                    });
                    addBlockData(block, data[i], series);
                    if (data[i].series[j].color) {
                        block.css("background-color", data[i].series[j].color);
                    }
                    block.append(jQuery("<div>", { "class": "ganttview-block-text" }));
                    jQuery(rows[rowIdx]).append(block);
                    rowIdx = rowIdx + 1;
                }
            }
        }
        
        function addBlockData(block, data, series) {
        	// This allows custom attributes to be added to the series data objects
        	// and makes them available to the 'data' argument of click, resize, and drag handlers
        	var blockData = { id: data.id, name: data.name };
        	jQuery.extend(blockData, series);
        	block.data("block-data", blockData);
        }

        function applyLastClass(div) {
            jQuery("div.ganttview-grid-row div.ganttview-grid-row-cell:last-child", div).addClass("last");
            jQuery("div.ganttview-hzheader-days div.ganttview-hzheader-day:last-child", div).addClass("last");
            jQuery("div.ganttview-hzheader-months div.ganttview-hzheader-month:last-child", div).addClass("last");
        }
		
		return {
			render: render
		};
	}

	var Behavior = function (div, opts) {
		
		function apply() {
			
			if (opts.behavior.clickable) { 
            	bindBlockClick(div, opts.behavior.onClick); 
        	}
        	
            if (opts.behavior.resizable) { 
            	bindBlockResize(div, opts.cellWidth, opts.start, opts.behavior.onResize); 
        	}
            
            if (opts.behavior.draggable) { 
            	bindBlockDrag(div, opts.cellWidth, opts.start, opts.behavior.onDrag); 
        	}
		}

        function bindBlockClick(div, callback) {
            jQuery("div.ganttview-block", div).live("click", function () {
                if (callback) { callback(jQuery(this).data("block-data")); }
            });
        }
        
        function bindBlockResize(div, cellWidth, startDate, callback) {
        	jQuery("div.ganttview-block", div).resizable({
        		grid: cellWidth, 
        		handles: "e,w",
        		stop: function () {
        			var block = jQuery(this);
        			updateDataAndPosition(div, block, cellWidth, startDate);
        			if (callback) { callback(block.data("block-data")); }
        		}
        	});
        }
        
        function bindBlockDrag(div, cellWidth, startDate, callback) {
        	jQuery("div.ganttview-block", div).draggable({
        		axis: "x", 
        		grid: [cellWidth, cellWidth],
        		stop: function () {
        			var block = jQuery(this);
        			updateDataAndPosition(div, block, cellWidth, startDate);
        			if (callback) { callback(block.data("block-data")); }
        		}
        	});
        }
        
        function updateDataAndPosition(div, block, cellWidth, startDate) {
        	var container = jQuery("div.ganttview-slide-container", div);
        	var scroll = container.scrollLeft();
			var offset = block.offset().left - container.offset().left - 1 + scroll;
			
			// Set new start date
			var daysFromStart = Math.round(offset / cellWidth);
			var newStart = startDate.clone().addDays(daysFromStart);
			block.data("block-data").start = newStart;

			// Set new end date
        	var width = block.outerWidth();
			var numberOfDays = Math.round(width / cellWidth) - 1;
			block.data("block-data").end = newStart.clone().addDays(numberOfDays);
			jQuery("div.ganttview-block-text", block).text(numberOfDays + 1);
			
			// Remove top and left properties to avoid incorrect block positioning,
        	// set position to relative to keep blocks relative to scrollbar when scrolling
			block.css("top", "").css("left", "")
				.css("position", "relative").css("margin-left", offset + "px");
        }
        
        return {
        	apply: apply	
        };
	}

    var ArrayUtils = {
	
        contains: function (arr, obj) {
            var has = false;
            for (var i = 0; i < arr.length; i++) { if (arr[i] == obj) { has = true; } }
            return has;
        }
    };

    var DateUtils = {
    	
        daysBetween: function (start, end) {
            if (!start || !end) { return 0; }
            start = Date.parse(start); end = Date.parse(end);
            if (start.getYear() == 1901 || end.getYear() == 8099) { return 0; }
            var count = 0, date = start.clone();
            while (date.compareTo(end) == -1) { count = count + 1; date.addDays(1); }
            return count;
        },
        
        isWeekend: function (date) {
            return date.getDay() % 6 == 0;
        },

        isToday: function (date) {
            const today = new Date()
            return date.getDate() == today.getDate() &&
                date.getMonth() == today.getMonth() &&
                date.getFullYear() == today.getFullYear();
        },

		getBoundaryDatesFromData: function (data, minDays) {
			var minStart = new Date(); maxEnd = new Date();
			for (var i = 0; i < data.length; i++) {
				for (var j = 0; j < data[i].series.length; j++) {
					var start = Date.parse(data[i].series[j].start);
					var end = Date.parse(data[i].series[j].end)
					if (i == 0 && j == 0) { minStart = start; maxEnd = end; }
					if (minStart.compareTo(start) == 1) { minStart = start; }
					if (maxEnd.compareTo(end) == -1) { maxEnd = end; }
				}
			}
			
			// Insure that the width of the chart is at least the slide width to avoid empty
			// whitespace to the right of the grid
			if (DateUtils.daysBetween(minStart, maxEnd) < minDays) {
				maxEnd = minStart.clone().addDays(minDays);
			}

			var newMaxEnd = maxEnd.clone().moveToLastDayOfMonth();
			
			return [minStart, newMaxEnd];
		}
    };

})(jQuery);

Date.CultureInfo = {
    name: "en-US",
    englishName: "English (United States)",
    nativeName: "English (United States)",
    dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
    abbreviatedDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
    shortestDayNames: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
    firstLetterDayNames: ["S", "M", "T", "W", "T", "F", "S"],
    monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
    abbreviatedMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
    amDesignator: "AM",
    pmDesignator: "PM",
    firstDayOfWeek: 0,
    twoDigitYearMax: 2029,
    dateElementOrder: "dmy",
    formatPatterns: {
        shortDate: "d/M/yyyy",
        longDate: "dddd, dd MMMM, yyyy",
        shortTime: "h:mm tt",
        longTime: "h:mm:ss tt",
        fullDateTime: "dddd, dd MMMM, yyyy h:mm:ss tt",
        sortableDateTime: "yyyy-MM-ddTHH:mm:ss",
        universalSortableDateTime: "yyyy-MM-dd HH:mm:ssZ",
        rfc1123: "ddd, dd MMM yyyy HH:mm:ss GMT",
        monthDay: "MMMM dd",
        yearMonth: "MMMM, yyyy"
    },
    regexPatterns: {
        jan: /^jan(uary)?/i,
        feb: /^feb(ruary)?/i,
        mar: /^mar(ch)?/i,
        apr: /^apr(il)?/i,
        may: /^may/i,
        jun: /^jun(e)?/i,
        jul: /^jul(y)?/i,
        aug: /^aug(ust)?/i,
        sep: /^sep(t(ember)?)?/i,
        oct: /^oct(ober)?/i,
        nov: /^nov(ember)?/i,
        dec: /^dec(ember)?/i,
        sun: /^su(n(day)?)?/i,
        mon: /^mo(n(day)?)?/i,
        tue: /^tu(e(s(day)?)?)?/i,
        wed: /^we(d(nesday)?)?/i,
        thu: /^th(u(r(s(day)?)?)?)?/i,
        fri: /^fr(i(day)?)?/i,
        sat: /^sa(t(urday)?)?/i,
        future: /^next/i,
        past: /^last|past|prev(ious)?/i,
        add: /^(\+|aft(er)?|from|hence)/i,
        subtract: /^(\-|bef(ore)?|ago)/i,
        yesterday: /^yes(terday)?/i,
        today: /^t(od(ay)?)?/i,
        tomorrow: /^tom(orrow)?/i,
        now: /^n(ow)?/i,
        millisecond: /^ms|milli(second)?s?/i,
        second: /^sec(ond)?s?/i,
        minute: /^mn|min(ute)?s?/i,
        hour: /^h(our)?s?/i,
        week: /^w(eek)?s?/i,
        month: /^m(onth)?s?/i,
        day: /^d(ay)?s?/i,
        year: /^y(ear)?s?/i,
        shortMeridian: /^(a|p)/i,
        longMeridian: /^(a\.?m?\.?|p\.?m?\.?)/i,
        timezone: /^((e(s|d)t|c(s|d)t|m(s|d)t|p(s|d)t)|((gmt)?\s*(\+|\-)\s*\d\d\d\d?)|gmt|utc)/i,
        ordinalSuffix: /^\s*(st|nd|rd|th)/i,
        timeContext: /^\s*(\:|a(?!u|p)|p)/i
    },
    timezones: [{
        name: "UTC",
        offset: "-000"
    }, {
        name: "GMT",
        offset: "-000"
    }, {
        name: "EST",
        offset: "-0500"
    }, {
        name: "EDT",
        offset: "-0400"
    }, {
        name: "CST",
        offset: "-0600"
    }, {
        name: "CDT",
        offset: "-0500"
    }, {
        name: "MST",
        offset: "-0700"
    }, {
        name: "MDT",
        offset: "-0600"
    }, {
        name: "PST",
        offset: "-0800"
    }, {
        name: "PDT",
        offset: "-0700"
    }]
};
(function() {
    var $D = Date,
        $P = $D.prototype,
        $C = $D.CultureInfo,
        p = function(s, l) {
            if (!l) {
                l = 2;
            }
            return ("000" + s).slice(l * -1);
        };
    $P.clearTime = function() {
        this.setHours(0);
        this.setMinutes(0);
        this.setSeconds(0);
        this.setMilliseconds(0);
        return this;
    };
    $P.setTimeToNow = function() {
        var n = new Date();
        this.setHours(n.getHours());
        this.setMinutes(n.getMinutes());
        this.setSeconds(n.getSeconds());
        this.setMilliseconds(n.getMilliseconds());
        return this;
    };
    $D.today = function() {
        return new Date().clearTime();
    };
    $D.compare = function(date1, date2) {
        if (isNaN(date1) || isNaN(date2)) {
            throw new Error(date1 + " - " + date2);
        } else if (date1 instanceof Date && date2 instanceof Date) {
            return (date1 < date2) ? -1 : (date1 > date2) ? 1 : 0;
        } else {
            throw new TypeError(date1 + " - " + date2);
        }
    };
    $D.equals = function(date1, date2) {
        return (date1.compareTo(date2) === 0);
    };
    $D.getDayNumberFromName = function(name) {
        var n = $C.dayNames,
            m = $C.abbreviatedDayNames,
            o = $C.shortestDayNames,
            s = name.toLowerCase();
        for (var i = 0; i < n.length; i++) {
            if (n[i].toLowerCase() == s || m[i].toLowerCase() == s || o[i].toLowerCase() == s) {
                return i;
            }
        }
        return -1;
    };
    $D.getMonthNumberFromName = function(name) {
        var n = $C.monthNames,
            m = $C.abbreviatedMonthNames,
            s = name.toLowerCase();
        for (var i = 0; i < n.length; i++) {
            if (n[i].toLowerCase() == s || m[i].toLowerCase() == s) {
                return i;
            }
        }
        return -1;
    };
    $D.isLeapYear = function(year) {
        return ((year % 4 === 0 && year % 100 !== 0) || year % 400 === 0);
    };
    $D.getDaysInMonth = function(year, month) {
        return [31, ($D.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
    };
    $D.getTimezoneAbbreviation = function(offset) {
        var z = $C.timezones,
            p;
        for (var i = 0; i < z.length; i++) {
            if (z[i].offset === offset) {
                return z[i].name;
            }
        }
        return null;
    };
    $D.getTimezoneOffset = function(name) {
        var z = $C.timezones,
            p;
        for (var i = 0; i < z.length; i++) {
            if (z[i].name === name.toUpperCase()) {
                return z[i].offset;
            }
        }
        return null;
    };
    $P.clone = function() {
        return new Date(this.getTime());
    };
    $P.compareTo = function(date) {
        return Date.compare(this, date);
    };
    $P.equals = function(date) {
        return Date.equals(this, date || new Date());
    };
    $P.between = function(start, end) {
        return this.getTime() >= start.getTime() && this.getTime() <= end.getTime();
    };
    $P.isAfter = function(date) {
        return this.compareTo(date || new Date()) === 1;
    };
    $P.isBefore = function(date) {
        return (this.compareTo(date || new Date()) === -1);
    };
    $P.isToday = function() {
        return this.isSameDay(new Date());
    };
    $P.isSameDay = function(date) {
        return this.clone().clearTime().equals(date.clone().clearTime());
    };
    $P.addMilliseconds = function(value) {
        this.setMilliseconds(this.getMilliseconds() + value);
        return this;
    };
    $P.addSeconds = function(value) {
        return this.addMilliseconds(value * 1000);
    };
    $P.addMinutes = function(value) {
        return this.addMilliseconds(value * 60000);
    };
    $P.addHours = function(value) {
        return this.addMilliseconds(value * 3600000);
    };
    $P.addDays = function(value) {
        this.setDate(this.getDate() + value);
        return this;
    };
    $P.addWeeks = function(value) {
        return this.addDays(value * 7);
    };
    $P.addMonths = function(value) {
        var n = this.getDate();
        this.setDate(1);
        this.setMonth(this.getMonth() + value);
        this.setDate(Math.min(n, $D.getDaysInMonth(this.getFullYear(), this.getMonth())));
        return this;
    };
    $P.addYears = function(value) {
        return this.addMonths(value * 12);
    };
    $P.add = function(config) {
        if (typeof config == "number") {
            this._orient = config;
            return this;
        }
        var x = config;
        if (x.milliseconds) {
            this.addMilliseconds(x.milliseconds);
        }
        if (x.seconds) {
            this.addSeconds(x.seconds);
        }
        if (x.minutes) {
            this.addMinutes(x.minutes);
        }
        if (x.hours) {
            this.addHours(x.hours);
        }
        if (x.weeks) {
            this.addWeeks(x.weeks);
        }
        if (x.months) {
            this.addMonths(x.months);
        }
        if (x.years) {
            this.addYears(x.years);
        }
        if (x.days) {
            this.addDays(x.days);
        }
        return this;
    };
    var $y, $m, $d;
    $P.getWeek = function() {
        var a, b, c, d, e, f, g, n, s, w;
        $y = (!$y) ? this.getFullYear() : $y;
        $m = (!$m) ? this.getMonth() + 1 : $m;
        $d = (!$d) ? this.getDate() : $d;
        if ($m <= 2) {
            a = $y - 1;
            b = (a / 4 | 0) - (a / 100 | 0) + (a / 400 | 0);
            c = ((a - 1) / 4 | 0) - ((a - 1) / 100 | 0) + ((a - 1) / 400 | 0);
            s = b - c;
            e = 0;
            f = $d - 1 + (31 * ($m - 1));
        } else {
            a = $y;
            b = (a / 4 | 0) - (a / 100 | 0) + (a / 400 | 0);
            c = ((a - 1) / 4 | 0) - ((a - 1) / 100 | 0) + ((a - 1) / 400 | 0);
            s = b - c;
            e = s + 1;
            f = $d + ((153 * ($m - 3) + 2) / 5) + 58 + s;
        }
        g = (a + b) % 7;
        d = (f + g - e) % 7;
        n = (f + 3 - d) | 0;
        if (n < 0) {
            w = 53 - ((g - s) / 5 | 0);
        } else if (n > 364 + s) {
            w = 1;
        } else {
            w = (n / 7 | 0) + 1;
        }
        $y = $m = $d = null;
        return w;
    };
    $P.getISOWeek = function() {
        $y = this.getUTCFullYear();
        $m = this.getUTCMonth() + 1;
        $d = this.getUTCDate();
        return p(this.getWeek());
    };
    $P.setWeek = function(n) {
        return this.moveToDayOfWeek(1).addWeeks(n - this.getWeek());
    };
    $D._validate = function(n, min, max, name) {
        if (typeof n == "undefined") {
            return false;
        } else if (typeof n != "number") {
            throw new TypeError(n + " is not a Number.");
        } else if (n < min || n > max) {
            throw new RangeError(n + " is not a valid value for " + name + ".");
        }
        return true;
    };
    $D.validateMillisecond = function(value) {
        return $D._validate(value, 0, 999, "millisecond");
    };
    $D.validateSecond = function(value) {
        return $D._validate(value, 0, 59, "second");
    };
    $D.validateMinute = function(value) {
        return $D._validate(value, 0, 59, "minute");
    };
    $D.validateHour = function(value) {
        return $D._validate(value, 0, 23, "hour");
    };
    $D.validateDay = function(value, year, month) {
        return $D._validate(value, 1, $D.getDaysInMonth(year, month), "day");
    };
    $D.validateMonth = function(value) {
        return $D._validate(value, 0, 11, "month");
    };
    $D.validateYear = function(value) {
        return $D._validate(value, 0, 9999, "year");
    };
    $P.set = function(config) {
        if ($D.validateMillisecond(config.millisecond)) {
            this.addMilliseconds(config.millisecond - this.getMilliseconds());
        }
        if ($D.validateSecond(config.second)) {
            this.addSeconds(config.second - this.getSeconds());
        }
        if ($D.validateMinute(config.minute)) {
            this.addMinutes(config.minute - this.getMinutes());
        }
        if ($D.validateHour(config.hour)) {
            this.addHours(config.hour - this.getHours());
        }
        if ($D.validateMonth(config.month)) {
            this.addMonths(config.month - this.getMonth());
        }
        if ($D.validateYear(config.year)) {
            this.addYears(config.year - this.getFullYear());
        }
        if ($D.validateDay(config.day, this.getFullYear(), this.getMonth())) {
            this.addDays(config.day - this.getDate());
        }
        if (config.timezone) {
            this.setTimezone(config.timezone);
        }
        if (config.timezoneOffset) {
            this.setTimezoneOffset(config.timezoneOffset);
        }
        if (config.week && $D._validate(config.week, 0, 53, "week")) {
            this.setWeek(config.week);
        }
        return this;
    };
    $P.moveToFirstDayOfMonth = function() {
        return this.set({
            day: 1
        });
    };
    $P.moveToLastDayOfMonth = function() {
        return this.set({
            day: $D.getDaysInMonth(this.getFullYear(), this.getMonth())
        });
    };
    $P.moveToNthOccurrence = function(dayOfWeek, occurrence) {
        var shift = 0;
        if (occurrence > 0) {
            shift = occurrence - 1;
        } else if (occurrence === -1) {
            this.moveToLastDayOfMonth();
            if (this.getDay() !== dayOfWeek) {
                this.moveToDayOfWeek(dayOfWeek, -1);
            }
            return this;
        }
        return this.moveToFirstDayOfMonth().addDays(-1).moveToDayOfWeek(dayOfWeek, +1).addWeeks(shift);
    };
    $P.moveToDayOfWeek = function(dayOfWeek, orient) {
        var diff = (dayOfWeek - this.getDay() + 7 * (orient || +1)) % 7;
        return this.addDays((diff === 0) ? diff += 7 * (orient || +1) : diff);
    };
    $P.moveToMonth = function(month, orient) {
        var diff = (month - this.getMonth() + 12 * (orient || +1)) % 12;
        return this.addMonths((diff === 0) ? diff += 12 * (orient || +1) : diff);
    };
    $P.getOrdinalNumber = function() {
        return Math.ceil((this.clone().clearTime() - new Date(this.getFullYear(), 0, 1)) / 86400000) + 1;
    };
    $P.getTimezone = function() {
        return $D.getTimezoneAbbreviation(this.getUTCOffset());
    };
    $P.setTimezoneOffset = function(offset) {
        var here = this.getTimezoneOffset(),
            there = Number(offset) * -6 / 10;
        return this.addMinutes(there - here);
    };
    $P.setTimezone = function(offset) {
        return this.setTimezoneOffset($D.getTimezoneOffset(offset));
    };
    $P.hasDaylightSavingTime = function() {
        return (Date.today().set({
            month: 0,
            day: 1
        }).getTimezoneOffset() !== Date.today().set({
            month: 6,
            day: 1
        }).getTimezoneOffset());
    };
    $P.isDaylightSavingTime = function() {
        return (this.hasDaylightSavingTime() && new Date().getTimezoneOffset() === Date.today().set({
            month: 6,
            day: 1
        }).getTimezoneOffset());
    };
    $P.getUTCOffset = function() {
        var n = this.getTimezoneOffset() * -10 / 6,
            r;
        if (n < 0) {
            r = (n - 10000).toString();
            return r.charAt(0) + r.substr(2);
        } else {
            r = (n + 10000).toString();
            return "+" + r.substr(1);
        }
    };
    $P.getElapsed = function(date) {
        return (date || new Date()) - this;
    };
    if (!$P.toISOString) {
        $P.toISOString = function() {
            function f(n) {
                return n < 10 ? '0' + n : n;
            }
            return '"' + this.getUTCFullYear() + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate()) + 'T' +
                f(this.getUTCHours()) + ':' +
                f(this.getUTCMinutes()) + ':' +
                f(this.getUTCSeconds()) + 'Z"';
        };
    }
    $P._toString = $P.toString;
    $P.toString = function(format) {
        var x = this;
        if (format && format.length == 1) {
            var c = $C.formatPatterns;
            x.t = x.toString;
            switch (format) {
                case "d":
                    return x.t(c.shortDate);
                case "D":
                    return x.t(c.longDate);
                case "F":
                    return x.t(c.fullDateTime);
                case "m":
                    return x.t(c.monthDay);
                case "r":
                    return x.t(c.rfc1123);
                case "s":
                    return x.t(c.sortableDateTime);
                case "t":
                    return x.t(c.shortTime);
                case "T":
                    return x.t(c.longTime);
                case "u":
                    return x.t(c.universalSortableDateTime);
                case "y":
                    return x.t(c.yearMonth);
            }
        }
        var ord = function(n) {
            switch (n * 1) {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        };
        return format ? format.replace(/(\\)?(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|S)/g, function(m) {
            if (m.charAt(0) === "\\") {
                return m.replace("\\", "");
            }
            x.h = x.getHours;
            switch (m) {
                case "hh":
                    return p(x.h() < 13 ? (x.h() === 0 ? 12 : x.h()) : (x.h() - 12));
                case "h":
                    return x.h() < 13 ? (x.h() === 0 ? 12 : x.h()) : (x.h() - 12);
                case "HH":
                    return p(x.h());
                case "H":
                    return x.h();
                case "mm":
                    return p(x.getMinutes());
                case "m":
                    return x.getMinutes();
                case "ss":
                    return p(x.getSeconds());
                case "s":
                    return x.getSeconds();
                case "yyyy":
                    return p(x.getFullYear(), 4);
                case "yy":
                    return p(x.getFullYear());
                case "dddd":
                    return $C.dayNames[x.getDay()];
                case "ddd":
                    return $C.abbreviatedDayNames[x.getDay()];
                case "dd":
                    return p(x.getDate());
                case "d":
                    return x.getDate();
                case "MMMM":
                    return $C.monthNames[x.getMonth()];
                case "MMM":
                    return $C.abbreviatedMonthNames[x.getMonth()];
                case "MM":
                    return p((x.getMonth() + 1));
                case "M":
                    return x.getMonth() + 1;
                case "t":
                    return x.h() < 12 ? $C.amDesignator.substring(0, 1) : $C.pmDesignator.substring(0, 1);
                case "tt":
                    return x.h() < 12 ? $C.amDesignator : $C.pmDesignator;
                case "S":
                    return ord(x.getDate());
                default:
                    return m;
            }
        }) : this._toString();
    };
}());
(function() {
    var $D = Date,
        $P = $D.prototype,
        $C = $D.CultureInfo,
        $N = Number.prototype;
    $P._orient = +1;
    $P._nth = null;
    $P._is = false;
    $P._same = false;
    $P._isSecond = false;
    $N._dateElement = "day";
    $P.next = function() {
        this._orient = +1;
        return this;
    };
    $D.next = function() {
        return $D.today().next();
    };
    $P.last = $P.prev = $P.previous = function() {
        this._orient = -1;
        return this;
    };
    $D.last = $D.prev = $D.previous = function() {
        return $D.today().last();
    };
    $P.is = function() {
        this._is = true;
        return this;
    };
    $P.same = function() {
        this._same = true;
        this._isSecond = false;
        return this;
    };
    $P.today = function() {
        return this.same().day();
    };
    $P.weekday = function() {
        if (this._is) {
            this._is = false;
            return (!this.is().sat() && !this.is().sun());
        }
        return false;
    };
    $P.at = function(time) {
        return (typeof time === "string") ? $D.parse(this.toString("d") + " " + time) : this.set(time);
    };
    $N.fromNow = $N.after = function(date) {
        var c = {};
        c[this._dateElement] = this;
        return ((!date) ? new Date() : date.clone()).add(c);
    };
    $N.ago = $N.before = function(date) {
        var c = {};
        c[this._dateElement] = this * -1;
        return ((!date) ? new Date() : date.clone()).add(c);
    };
    var dx = ("sunday monday tuesday wednesday thursday friday saturday").split(/\s/),
        mx = ("january february march april may june july august september october november december").split(/\s/),
        px = ("Millisecond Second Minute Hour Day Week Month Year").split(/\s/),
        pxf = ("Milliseconds Seconds Minutes Hours Date Week Month FullYear").split(/\s/),
        nth = ("final first second third fourth fifth").split(/\s/),
        de;
    $P.toObject = function() {
        var o = {};
        for (var i = 0; i < px.length; i++) {
            o[px[i].toLowerCase()] = this["get" + pxf[i]]();
        }
        return o;
    };
    $D.fromObject = function(config) {
        config.week = null;
        return Date.today().set(config);
    };
    var df = function(n) {
        return function() {
            if (this._is) {
                this._is = false;
                return this.getDay() == n;
            }
            if (this._nth !== null) {
                if (this._isSecond) {
                    this.addSeconds(this._orient * -1);
                }
                this._isSecond = false;
                var ntemp = this._nth;
                this._nth = null;
                var temp = this.clone().moveToLastDayOfMonth();
                this.moveToNthOccurrence(n, ntemp);
                if (this > temp) {
                    throw new RangeError($D.getDayName(n) + " does not occur " + ntemp + " times in the month of " + $D.getMonthName(temp.getMonth()) + " " + temp.getFullYear() + ".");
                }
                return this;
            }
            return this.moveToDayOfWeek(n, this._orient);
        };
    };
    var sdf = function(n) {
        return function() {
            var t = $D.today(),
                shift = n - t.getDay();
            if (n === 0 && $C.firstDayOfWeek === 1 && t.getDay() !== 0) {
                shift = shift + 7;
            }
            return t.addDays(shift);
        };
    };
    for (var i = 0; i < dx.length; i++) {
        $D[dx[i].toUpperCase()] = $D[dx[i].toUpperCase().substring(0, 3)] = i;
        $D[dx[i]] = $D[dx[i].substring(0, 3)] = sdf(i);
        $P[dx[i]] = $P[dx[i].substring(0, 3)] = df(i);
    }
    var mf = function(n) {
        return function() {
            if (this._is) {
                this._is = false;
                return this.getMonth() === n;
            }
            return this.moveToMonth(n, this._orient);
        };
    };
    var smf = function(n) {
        return function() {
            return $D.today().set({
                month: n,
                day: 1
            });
        };
    };
    for (var j = 0; j < mx.length; j++) {
        $D[mx[j].toUpperCase()] = $D[mx[j].toUpperCase().substring(0, 3)] = j;
        $D[mx[j]] = $D[mx[j].substring(0, 3)] = smf(j);
        $P[mx[j]] = $P[mx[j].substring(0, 3)] = mf(j);
    }
    var ef = function(j) {
        return function() {
            if (this._isSecond) {
                this._isSecond = false;
                return this;
            }
            if (this._same) {
                this._same = this._is = false;
                var o1 = this.toObject(),
                    o2 = (arguments[0] || new Date()).toObject(),
                    v = "",
                    k = j.toLowerCase();
                for (var m = (px.length - 1); m > -1; m--) {
                    v = px[m].toLowerCase();
                    if (o1[v] != o2[v]) {
                        return false;
                    }
                    if (k == v) {
                        break;
                    }
                }
                return true;
            }
            if (j.substring(j.length - 1) != "s") {
                j += "s";
            }
            return this["add" + j](this._orient);
        };
    };
    var nf = function(n) {
        return function() {
            this._dateElement = n;
            return this;
        };
    };
    for (var k = 0; k < px.length; k++) {
        de = px[k].toLowerCase();
        $P[de] = $P[de + "s"] = ef(px[k]);
        $N[de] = $N[de + "s"] = nf(de);
    }
    $P._ss = ef("Second");
    var nthfn = function(n) {
        return function(dayOfWeek) {
            if (this._same) {
                return this._ss(arguments[0]);
            }
            if (dayOfWeek || dayOfWeek === 0) {
                return this.moveToNthOccurrence(dayOfWeek, n);
            }
            this._nth = n;
            if (n === 2 && (dayOfWeek === undefined || dayOfWeek === null)) {
                this._isSecond = true;
                return this.addSeconds(this._orient);
            }
            return this;
        };
    };
    for (var l = 0; l < nth.length; l++) {
        $P[nth[l]] = (l === 0) ? nthfn(-1) : nthfn(l);
    }
}());
(function() {
    Date.Parsing = {
        Exception: function(s) {
            this.message = "Parse error at '" + s.substring(0, 10) + " ...'";
        }
    };
    var $P = Date.Parsing;
    var _ = $P.Operators = {
        rtoken: function(r) {
            return function(s) {
                var mx = s.match(r);
                if (mx) {
                    return ([mx[0], s.substring(mx[0].length)]);
                } else {
                    throw new $P.Exception(s);
                }
            };
        },
        token: function(s) {
            return function(s) {
                return _.rtoken(new RegExp("^\s*" + s + "\s*"))(s);
            };
        },
        stoken: function(s) {
            return _.rtoken(new RegExp("^" + s));
        },
        until: function(p) {
            return function(s) {
                var qx = [],
                    rx = null;
                while (s.length) {
                    try {
                        rx = p.call(this, s);
                    } catch (e) {
                        qx.push(rx[0]);
                        s = rx[1];
                        continue;
                    }
                    break;
                }
                return [qx, s];
            };
        },
        many: function(p) {
            return function(s) {
                var rx = [],
                    r = null;
                while (s.length) {
                    try {
                        r = p.call(this, s);
                    } catch (e) {
                        return [rx, s];
                    }
                    rx.push(r[0]);
                    s = r[1];
                }
                return [rx, s];
            };
        },
        optional: function(p) {
            return function(s) {
                var r = null;
                try {
                    r = p.call(this, s);
                } catch (e) {
                    return [null, s];
                }
                return [r[0], r[1]];
            };
        },
        not: function(p) {
            return function(s) {
                try {
                    p.call(this, s);
                } catch (e) {
                    return [null, s];
                }
                throw new $P.Exception(s);
            };
        },
        ignore: function(p) {
            return p ? function(s) {
                var r = null;
                r = p.call(this, s);
                return [null, r[1]];
            } : null;
        },
        product: function() {
            var px = arguments[0],
                qx = Array.prototype.slice.call(arguments, 1),
                rx = [];
            for (var i = 0; i < px.length; i++) {
                rx.push(_.each(px[i], qx));
            }
            return rx;
        },
        cache: function(rule) {
            var cache = {},
                r = null;
            return function(s) {
                try {
                    r = cache[s] = (cache[s] || rule.call(this, s));
                } catch (e) {
                    r = cache[s] = e;
                }
                if (r instanceof $P.Exception) {
                    throw r;
                } else {
                    return r;
                }
            };
        },
        any: function() {
            var px = arguments;
            return function(s) {
                var r = null;
                for (var i = 0; i < px.length; i++) {
                    if (px[i] == null) {
                        continue;
                    }
                    try {
                        r = (px[i].call(this, s));
                    } catch (e) {
                        r = null;
                    }
                    if (r) {
                        return r;
                    }
                }
                throw new $P.Exception(s);
            };
        },
        each: function() {
            var px = arguments;
            return function(s) {
                var rx = [],
                    r = null;
                for (var i = 0; i < px.length; i++) {
                    if (px[i] == null) {
                        continue;
                    }
                    try {
                        r = (px[i].call(this, s));
                    } catch (e) {
                        throw new $P.Exception(s);
                    }
                    rx.push(r[0]);
                    s = r[1];
                }
                return [rx, s];
            };
        },
        all: function() {
            var px = arguments,
                _ = _;
            return _.each(_.optional(px));
        },
        sequence: function(px, d, c) {
            d = d || _.rtoken(/^\s*/);
            c = c || null;
            if (px.length == 1) {
                return px[0];
            }
            return function(s) {
                var r = null,
                    q = null;
                var rx = [];
                for (var i = 0; i < px.length; i++) {
                    try {
                        r = px[i].call(this, s);
                    } catch (e) {
                        break;
                    }
                    rx.push(r[0]);
                    try {
                        q = d.call(this, r[1]);
                    } catch (ex) {
                        q = null;
                        break;
                    }
                    s = q[1];
                }
                if (!r) {
                    throw new $P.Exception(s);
                }
                if (q) {
                    throw new $P.Exception(q[1]);
                }
                if (c) {
                    try {
                        r = c.call(this, r[1]);
                    } catch (ey) {
                        throw new $P.Exception(r[1]);
                    }
                }
                return [rx, (r ? r[1] : s)];
            };
        },
        between: function(d1, p, d2) {
            d2 = d2 || d1;
            var _fn = _.each(_.ignore(d1), p, _.ignore(d2));
            return function(s) {
                var rx = _fn.call(this, s);
                return [
                    [rx[0][0], r[0][2]], rx[1]
                ];
            };
        },
        list: function(p, d, c) {
            d = d || _.rtoken(/^\s*/);
            c = c || null;
            return (p instanceof Array ? _.each(_.product(p.slice(0, -1), _.ignore(d)), p.slice(-1), _.ignore(c)) : _.each(_.many(_.each(p, _.ignore(d))), px, _.ignore(c)));
        },
        set: function(px, d, c) {
            d = d || _.rtoken(/^\s*/);
            c = c || null;
            return function(s) {
                var r = null,
                    p = null,
                    q = null,
                    rx = null,
                    best = [
                        [], s
                    ],
                    last = false;
                for (var i = 0; i < px.length; i++) {
                    q = null;
                    p = null;
                    r = null;
                    last = (px.length == 1);
                    try {
                        r = px[i].call(this, s);
                    } catch (e) {
                        continue;
                    }
                    rx = [
                        [r[0]], r[1]
                    ];
                    if (r[1].length > 0 && !last) {
                        try {
                            q = d.call(this, r[1]);
                        } catch (ex) {
                            last = true;
                        }
                    } else {
                        last = true;
                    }
                    if (!last && q[1].length === 0) {
                        last = true;
                    }
                    if (!last) {
                        var qx = [];
                        for (var j = 0; j < px.length; j++) {
                            if (i != j) {
                                qx.push(px[j]);
                            }
                        }
                        p = _.set(qx, d).call(this, q[1]);
                        if (p[0].length > 0) {
                            rx[0] = rx[0].concat(p[0]);
                            rx[1] = p[1];
                        }
                    }
                    if (rx[1].length < best[1].length) {
                        best = rx;
                    }
                    if (best[1].length === 0) {
                        break;
                    }
                }
                if (best[0].length === 0) {
                    return best;
                }
                if (c) {
                    try {
                        q = c.call(this, best[1]);
                    } catch (ey) {
                        throw new $P.Exception(best[1]);
                    }
                    best[1] = q[1];
                }
                return best;
            };
        },
        forward: function(gr, fname) {
            return function(s) {
                return gr[fname].call(this, s);
            };
        },
        replace: function(rule, repl) {
            return function(s) {
                var r = rule.call(this, s);
                return [repl, r[1]];
            };
        },
        process: function(rule, fn) {
            return function(s) {
                var r = rule.call(this, s);
                return [fn.call(this, r[0]), r[1]];
            };
        },
        min: function(min, rule) {
            return function(s) {
                var rx = rule.call(this, s);
                if (rx[0].length < min) {
                    throw new $P.Exception(s);
                }
                return rx;
            };
        }
    };
    var _generator = function(op) {
        return function() {
            var args = null,
                rx = [];
            if (arguments.length > 1) {
                args = Array.prototype.slice.call(arguments);
            } else if (arguments[0] instanceof Array) {
                args = arguments[0];
            }
            if (args) {
                for (var i = 0, px = args.shift(); i < px.length; i++) {
                    args.unshift(px[i]);
                    rx.push(op.apply(null, args));
                    args.shift();
                    return rx;
                }
            } else {
                return op.apply(null, arguments);
            }
        };
    };
    var gx = "optional not ignore cache".split(/\s/);
    for (var i = 0; i < gx.length; i++) {
        _[gx[i]] = _generator(_[gx[i]]);
    }
    var _vector = function(op) {
        return function() {
            if (arguments[0] instanceof Array) {
                return op.apply(null, arguments[0]);
            } else {
                return op.apply(null, arguments);
            }
        };
    };
    var vx = "each any all".split(/\s/);
    for (var j = 0; j < vx.length; j++) {
        _[vx[j]] = _vector(_[vx[j]]);
    }
}());
(function() {
    var $D = Date,
        $P = $D.prototype,
        $C = $D.CultureInfo;
    var flattenAndCompact = function(ax) {
        var rx = [];
        for (var i = 0; i < ax.length; i++) {
            if (ax[i] instanceof Array) {
                rx = rx.concat(flattenAndCompact(ax[i]));
            } else {
                if (ax[i]) {
                    rx.push(ax[i]);
                }
            }
        }
        return rx;
    };
    $D.Grammar = {};
    $D.Translator = {
        hour: function(s) {
            return function() {
                this.hour = Number(s);
            };
        },
        minute: function(s) {
            return function() {
                this.minute = Number(s);
            };
        },
        second: function(s) {
            return function() {
                this.second = Number(s);
            };
        },
        meridian: function(s) {
            return function() {
                this.meridian = s.slice(0, 1).toLowerCase();
            };
        },
        timezone: function(s) {
            return function() {
                var n = s.replace(/[^\d\+\-]/g, "");
                if (n.length) {
                    this.timezoneOffset = Number(n);
                } else {
                    this.timezone = s.toLowerCase();
                }
            };
        },
        day: function(x) {
            var s = x[0];
            return function() {
                this.day = Number(s.match(/\d+/)[0]);
            };
        },
        month: function(s) {
            return function() {
                this.month = (s.length == 3) ? "jan feb mar apr may jun jul aug sep oct nov dec".indexOf(s) / 4 : Number(s) - 1;
            };
        },
        year: function(s) {
            return function() {
                var n = Number(s);
                this.year = ((s.length > 2) ? n : (n + (((n + 2000) < $C.twoDigitYearMax) ? 2000 : 1900)));
            };
        },
        rday: function(s) {
            return function() {
                switch (s) {
                    case "yesterday":
                        this.days = -1;
                        break;
                    case "tomorrow":
                        this.days = 1;
                        break;
                    case "today":
                        this.days = 0;
                        break;
                    case "now":
                        this.days = 0;
                        this.now = true;
                        break;
                }
            };
        },
        finishExact: function(x) {
            x = (x instanceof Array) ? x : [x];
            for (var i = 0; i < x.length; i++) {
                if (x[i]) {
                    x[i].call(this);
                }
            }
            var now = new Date();
            if ((this.hour || this.minute) && (!this.month && !this.year && !this.day)) {
                this.day = now.getDate();
            }
            if (!this.year) {
                this.year = now.getFullYear();
            }
            if (!this.month && this.month !== 0) {
                this.month = now.getMonth();
            }
            if (!this.day) {
                this.day = 1;
            }
            if (!this.hour) {
                this.hour = 0;
            }
            if (!this.minute) {
                this.minute = 0;
            }
            if (!this.second) {
                this.second = 0;
            }
            if (this.meridian && this.hour) {
                if (this.meridian == "p" && this.hour < 12) {
                    this.hour = this.hour + 12;
                } else if (this.meridian == "a" && this.hour == 12) {
                    this.hour = 0;
                }
            }
            if (this.day > $D.getDaysInMonth(this.year, this.month)) {
                throw new RangeError(this.day + " is not a valid value for days.");
            }
            var r = new Date(this.year, this.month, this.day, this.hour, this.minute, this.second);
            if (this.timezone) {
                r.set({
                    timezone: this.timezone
                });
            } else if (this.timezoneOffset) {
                r.set({
                    timezoneOffset: this.timezoneOffset
                });
            }
            return r;
        },
        finish: function(x) {
            x = (x instanceof Array) ? flattenAndCompact(x) : [x];
            if (x.length === 0) {
                return null;
            }
            for (var i = 0; i < x.length; i++) {
                if (typeof x[i] == "function") {
                    x[i].call(this);
                }
            }
            var today = $D.today();
            if (this.now && !this.unit && !this.operator) {
                return new Date();
            } else if (this.now) {
                today = new Date();
            }
            var expression = !!(this.days && this.days !== null || this.orient || this.operator);
            var gap, mod, orient;
            orient = ((this.orient == "past" || this.operator == "subtract") ? -1 : 1);
            if (!this.now && "hour minute second".indexOf(this.unit) != -1) {
                today.setTimeToNow();
            }
            if (this.month || this.month === 0) {
                if ("year day hour minute second".indexOf(this.unit) != -1) {
                    this.value = this.month + 1;
                    this.month = null;
                    expression = true;
                }
            }
            if (!expression && this.weekday && !this.day && !this.days) {
                var temp = Date[this.weekday]();
                this.day = temp.getDate();
                if (!this.month) {
                    this.month = temp.getMonth();
                }
                this.year = temp.getFullYear();
            }
            if (expression && this.weekday && this.unit != "month") {
                this.unit = "day";
                gap = ($D.getDayNumberFromName(this.weekday) - today.getDay());
                mod = 7;
                this.days = gap ? ((gap + (orient * mod)) % mod) : (orient * mod);
            }
            if (this.month && this.unit == "day" && this.operator) {
                this.value = (this.month + 1);
                this.month = null;
            }
            if (this.value != null && this.month != null && this.year != null) {
                this.day = this.value * 1;
            }
            if (this.month && !this.day && this.value) {
                today.set({
                    day: this.value * 1
                });
                if (!expression) {
                    this.day = this.value * 1;
                }
            }
            if (!this.month && this.value && this.unit == "month" && !this.now) {
                this.month = this.value;
                expression = true;
            }
            if (expression && (this.month || this.month === 0) && this.unit != "year") {
                this.unit = "month";
                gap = (this.month - today.getMonth());
                mod = 12;
                this.months = gap ? ((gap + (orient * mod)) % mod) : (orient * mod);
                this.month = null;
            }
            if (!this.unit) {
                this.unit = "day";
            }
            if (!this.value && this.operator && this.operator !== null && this[this.unit + "s"] && this[this.unit + "s"] !== null) {
                this[this.unit + "s"] = this[this.unit + "s"] + ((this.operator == "add") ? 1 : -1) + (this.value || 0) * orient;
            } else if (this[this.unit + "s"] == null || this.operator != null) {
                if (!this.value) {
                    this.value = 1;
                }
                this[this.unit + "s"] = this.value * orient;
            }
            if (this.meridian && this.hour) {
                if (this.meridian == "p" && this.hour < 12) {
                    this.hour = this.hour + 12;
                } else if (this.meridian == "a" && this.hour == 12) {
                    this.hour = 0;
                }
            }
            if (this.weekday && !this.day && !this.days) {
                var temp = Date[this.weekday]();
                this.day = temp.getDate();
                if (temp.getMonth() !== today.getMonth()) {
                    this.month = temp.getMonth();
                }
            }
            if ((this.month || this.month === 0) && !this.day) {
                this.day = 1;
            }
            if (!this.orient && !this.operator && this.unit == "week" && this.value && !this.day && !this.month) {
                return Date.today().setWeek(this.value);
            }
            if (expression && this.timezone && this.day && this.days) {
                this.day = this.days;
            }
            return (expression) ? today.add(this) : today.set(this);
        }
    };
    var _ = $D.Parsing.Operators,
        g = $D.Grammar,
        t = $D.Translator,
        _fn;
    g.datePartDelimiter = _.rtoken(/^([\s\-\.\,\/\x27]+)/);
    g.timePartDelimiter = _.stoken(":");
    g.whiteSpace = _.rtoken(/^\s*/);
    g.generalDelimiter = _.rtoken(/^(([\s\,]|at|@|on)+)/);
    var _C = {};
    g.ctoken = function(keys) {
        var fn = _C[keys];
        if (!fn) {
            var c = $C.regexPatterns;
            var kx = keys.split(/\s+/),
                px = [];
            for (var i = 0; i < kx.length; i++) {
                px.push(_.replace(_.rtoken(c[kx[i]]), kx[i]));
            }
            fn = _C[keys] = _.any.apply(null, px);
        }
        return fn;
    };
    g.ctoken2 = function(key) {
        return _.rtoken($C.regexPatterns[key]);
    };
    g.h = _.cache(_.process(_.rtoken(/^(0[0-9]|1[0-2]|[1-9])/), t.hour));
    g.hh = _.cache(_.process(_.rtoken(/^(0[0-9]|1[0-2])/), t.hour));
    g.H = _.cache(_.process(_.rtoken(/^([0-1][0-9]|2[0-3]|[0-9])/), t.hour));
    g.HH = _.cache(_.process(_.rtoken(/^([0-1][0-9]|2[0-3])/), t.hour));
    g.m = _.cache(_.process(_.rtoken(/^([0-5][0-9]|[0-9])/), t.minute));
    g.mm = _.cache(_.process(_.rtoken(/^[0-5][0-9]/), t.minute));
    g.s = _.cache(_.process(_.rtoken(/^([0-5][0-9]|[0-9])/), t.second));
    g.ss = _.cache(_.process(_.rtoken(/^[0-5][0-9]/), t.second));
    g.hms = _.cache(_.sequence([g.H, g.m, g.s], g.timePartDelimiter));
    g.t = _.cache(_.process(g.ctoken2("shortMeridian"), t.meridian));
    g.tt = _.cache(_.process(g.ctoken2("longMeridian"), t.meridian));
    g.z = _.cache(_.process(_.rtoken(/^((\+|\-)\s*\d\d\d\d)|((\+|\-)\d\d\:?\d\d)/), t.timezone));
    g.zz = _.cache(_.process(_.rtoken(/^((\+|\-)\s*\d\d\d\d)|((\+|\-)\d\d\:?\d\d)/), t.timezone));
    g.zzz = _.cache(_.process(g.ctoken2("timezone"), t.timezone));
    g.timeSuffix = _.each(_.ignore(g.whiteSpace), _.set([g.tt, g.zzz]));
    g.time = _.each(_.optional(_.ignore(_.stoken("T"))), g.hms, g.timeSuffix);
    g.d = _.cache(_.process(_.each(_.rtoken(/^([0-2]\d|3[0-1]|\d)/), _.optional(g.ctoken2("ordinalSuffix"))), t.day));
    g.dd = _.cache(_.process(_.each(_.rtoken(/^([0-2]\d|3[0-1])/), _.optional(g.ctoken2("ordinalSuffix"))), t.day));
    g.ddd = g.dddd = _.cache(_.process(g.ctoken("sun mon tue wed thu fri sat"), function(s) {
        return function() {
            this.weekday = s;
        };
    }));
    g.M = _.cache(_.process(_.rtoken(/^(1[0-2]|0\d|\d)/), t.month));
    g.MM = _.cache(_.process(_.rtoken(/^(1[0-2]|0\d)/), t.month));
    g.MMM = g.MMMM = _.cache(_.process(g.ctoken("jan feb mar apr may jun jul aug sep oct nov dec"), t.month));
    g.y = _.cache(_.process(_.rtoken(/^(\d\d?)/), t.year));
    g.yy = _.cache(_.process(_.rtoken(/^(\d\d)/), t.year));
    g.yyy = _.cache(_.process(_.rtoken(/^(\d\d?\d?\d?)/), t.year));
    g.yyyy = _.cache(_.process(_.rtoken(/^(\d\d\d\d)/), t.year));
    _fn = function() {
        return _.each(_.any.apply(null, arguments), _.not(g.ctoken2("timeContext")));
    };
    g.day = _fn(g.d, g.dd);
    g.month = _fn(g.M, g.MMM);
    g.year = _fn(g.yyyy, g.yy);
    g.orientation = _.process(g.ctoken("past future"), function(s) {
        return function() {
            this.orient = s;
        };
    });
    g.operator = _.process(g.ctoken("add subtract"), function(s) {
        return function() {
            this.operator = s;
        };
    });
    g.rday = _.process(g.ctoken("yesterday tomorrow today now"), t.rday);
    g.unit = _.process(g.ctoken("second minute hour day week month year"), function(s) {
        return function() {
            this.unit = s;
        };
    });
    g.value = _.process(_.rtoken(/^\d\d?(st|nd|rd|th)?/), function(s) {
        return function() {
            this.value = s.replace(/\D/g, "");
        };
    });
    g.expression = _.set([g.rday, g.operator, g.value, g.unit, g.orientation, g.ddd, g.MMM]);
    _fn = function() {
        return _.set(arguments, g.datePartDelimiter);
    };
    g.mdy = _fn(g.ddd, g.month, g.day, g.year);
    g.ymd = _fn(g.ddd, g.year, g.month, g.day);
    g.dmy = _fn(g.ddd, g.day, g.month, g.year);
    g.date = function(s) {
        return ((g[$C.dateElementOrder] || g.mdy).call(this, s));
    };
    g.format = _.process(_.many(_.any(_.process(_.rtoken(/^(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?)/), function(fmt) {
        if (g[fmt]) {
            return g[fmt];
        } else {
            throw $D.Parsing.Exception(fmt);
        }
    }), _.process(_.rtoken(/^[^dMyhHmstz]+/), function(s) {
        return _.ignore(_.stoken(s));
    }))), function(rules) {
        return _.process(_.each.apply(null, rules), t.finishExact);
    });
    var _F = {};
    var _get = function(f) {
        return _F[f] = (_F[f] || g.format(f)[0]);
    };
    g.formats = function(fx) {
        if (fx instanceof Array) {
            var rx = [];
            for (var i = 0; i < fx.length; i++) {
                rx.push(_get(fx[i]));
            }
            return _.any.apply(null, rx);
        } else {
            return _get(fx);
        }
    };
    g._formats = g.formats(["\"yyyy-MM-ddTHH:mm:ssZ\"", "yyyy-MM-ddTHH:mm:ssZ", "yyyy-MM-ddTHH:mm:ssz", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mmZ", "yyyy-MM-ddTHH:mmz", "yyyy-MM-ddTHH:mm", "ddd, dd MMM, yyyy H:mm:ss tt", "ddd d MMM yyyy HH:mm:ss zzz", "MMddyyyy", "ddMMyyyy", "Mddyyyy", "ddMyyyy", "Mdyyyy", "dMyyyy", "yyyy", "Mdyy", "dMyy", "d"]);
    g._start = _.process(_.set([g.date, g.time, g.expression], g.generalDelimiter, g.whiteSpace), t.finish);
    g.start = function(s) {
        try {
            var r = g._formats.call({}, s);
            if (r[1].length === 0) {
                return r;
            }
        } catch (e) {}
        return g._start.call({}, s);
    };
    $D._parse = $D.parse;
    $D.parse = function(s) {
        var r = null;
        if (!s) {
            return null;
        }
        if (s instanceof Date) {
            return s;
        }
        try {
            r = $D.Grammar.start.call({}, s.replace(/^\s*(\S*(\s+\S+)*)\s*$/, "$1"));
        } catch (e) {
            return null;
        }
        return ((r[1].length === 0) ? r[0] : null);
    };
    $D.getParseFunction = function(fx) {
        var fn = $D.Grammar.formats(fx);
        return function(s) {
            var r = null;
            try {
                r = fn.call({}, s);
            } catch (e) {
                return null;
            }
            return ((r[1].length === 0) ? r[0] : null);
        };
    };
    $D.parseExact = function(s, fx) {
        return $D.getParseFunction(fx)(s);
    };
}());