function pageLoad() {
	updatePage();
    
    document.onmousemove = function(e) {
        setupSidebarHeight();
        setupSidebar(e);
    };
    
    window.onscroll = function() {
        setupSidebarHeight();
    };
}

function updatePage() {
    createTree(usageSites, "siteDiv");
    addExpanderOnClickEvents();
    setOpenExpanders();
    setupDataGrids();
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

    var savings = document.getElementById(type + 'OpportunitiesEstimatedDates' + row).innerHTML;
    data.push(savings.split('<br>')[0].replace('Start: ', ''));
    data.push(savings.split('<br>')[1].replace('Finish: ', ''));

    data.push(document.getElementById(type + 'OpportunitiesPercentageSaving' + row).innerHTML);
    data.push(document.getElementById(type + 'OpportunitiesEstimatedCost' + row).innerHTML);

    var savings = document.getElementById(type + 'OpportunitiesEstimatedSavings' + row).innerHTML;
    data.push(savings.split('<br>')[0].replace('kWh: ', ''));
    data.push(savings.split('<br>')[1].replace('£: ', ''));

    var savings = document.getElementById(type + 'OpportunitiesROI' + row).innerHTML;
    data.push(savings.split('<br>')[0].replace('Total: ', ''));
    data.push(savings.split('<br>')[1].replace('Remaining: ', ''));

    var detailSpans = modal.getElementsByClassName('manageOpportunityOpportunityDetailSpan');
    var detailLength = detailSpans.length;

    for(var i = 0; i < detailLength; i++) {
        detailSpans[i].innerHTML = data[i];
    }

    finalisePopup(title, 'Manage Opportunity<br>' + data[0] + ' - ' + data[2], modal, span);
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

function displayContactPopup(type, row) {
    var modal = document.getElementById("contactPopup");
	var title = document.getElementById("contactTitle");
    var span = modal.getElementsByClassName("close")[0];
    var contact = document.getElementById("contact");
    var customer = document.getElementById(type + 'Customer' + row).innerText;

    var contactText = 'BWS Contact: ' + (type == 'requestedVisits' ? 'Main Office' : 'En Gineer') + ' - 07777 777777<br><br>'
                    + customer + ' Contact(s):<br>'
                    + '<span style="margin-left: 5px;">Main Office: 01234 567890</span><br><br>'
                    + '<span style="margin-left: 5px;">Leeds:</span><br>'
                    + '<span style="margin-left: 10px;">Site Contact: David Ford - 07890 123456</span><br>'
                    + '<span style="margin-left: 10px;">Engineer Contact: Andrew Sampson - 07890 654321</span>'

    contact.innerHTML = contactText;
    
    finalisePopup(title, 'Contacts<br>', modal, span);
}

function setupRequestedVisitsDataGrid() {
    clearElement(document.getElementById('requestedVisitsSpreadsheet'));
    var data = [];
    var row = {
        customer:'<div id="requestedVisitsCustomer1">David Ford Trading Ltd</div>', 
		site:'Leeds <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'requestedVisits'" + ', 1)"></i>',
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
    clearElement(document.getElementById('scheduledVisitsSpreadsheet'));
    var data = [];
    var row = {
        customer:'<div id="scheduledVisitsCustomer1">David Ford Trading Ltd</div>', 
		site:'Leeds <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'scheduledVisits'" + ', 1)"></i>',
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
    clearElement(document.getElementById('recommendedOpportunitiesSpreadsheet'));
    var data = [];
    var row = {
        customer:'<div id="recommendedOpportunitiesCustomer1">David Ford Trading Ltd</div>', 
        opportunityType:'<div id="recommendedOpportunitiesOpportunityType1">Custom</div>',
        opportunityName:'<div id="recommendedOpportunitiesOpportunityName1">LED Lighting 2</div>',
		site:'<div id="recommendedOpportunitiesSite1">Leeds <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'recommendedOpportunities'" + ', 1)"></i></div>',
        meter:'<div id="recommendedOpportunitiesMeter1">1234567890123</div>',
        subMeter:'<div id="recommendedOpportunitiesSubMeter1">Sub Meter</div>',
        estimatedDates:'<div id="recommendedOpportunitiesEstimatedDates1">Start: 01/04/2020<br>Finish: 30/06/2020</div>',
        percentageSaving:'<div id="recommendedOpportunitiesPercentageSaving1">10%</div>',
        estimatedCost:'<div id="recommendedOpportunitiesEstimatedCost1">£100,000</div>',
        estimatedSavings:'<div id="recommendedOpportunitiesEstimatedSavings1">kWh: 10,000<br>£: £15,000</div>',
        roi:'<div id="recommendedOpportunitiesROI1">Total: 84<br>Remaining: 84</div>',
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
            {type:'text', width:'175px', name:'estimatedDates', title:'Estimated<br>Dates', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'120px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'125px', name:'estimatedSavings', title:'Estimated <br>Savings (pa)', readOnly: true},
            {type:'text', width:'140px', name:'roi', title:'ROI<br>Months', readOnly: true},
            {type:'text', width:'197px', name:'manageOpportunity', title:'Manage Opportunity', readOnly: true},
            {type:'text', width:'195px', name:'approveReject', title:'Approve/Reject<br>Opportunity', readOnly: true},
		 ]
	  }); 
}

function setupPendingActiveOpportunitiesDataGrid() {
    clearElement(document.getElementById('pendingActiveOpportunitiesSpreadsheet'));
    var data = [];
    var row = {
        customer:'<div id="pendingActiveOpportunitiesCustomer1">David Ford Trading Ltd</div>', 
        opportunityType:'<div id="pendingActiveOpportunitiesOpportunityType1">Custom</div>',
        opportunityName:'<div id="pendingActiveOpportunitiesOpportunityName1">LED Lighting</div>',
		site:'<div id="pendingActiveOpportunitiesSite1">Leeds <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'pendingActiveOpportunities'" + ', 1)"></i></div>',
        meter:'<div id="pendingActiveOpportunitiesMeter1">12345678910124</div>',
        subMeter:'<div id="pendingActiveOpportunitiesSubMeter1">Sub Meter</div>',
        estimatedDates:'<div id="pendingActiveOpportunitiesEstimatedDates1">Start: 01/04/2020<br>Finish: 30/06/2020</div>',
        percentageSaving:'<div id="pendingActiveOpportunitiesPercentageSaving1">15%</div>',
        estimatedCost:'<div id="pendingActiveOpportunitiesEstimatedCost1">£150,000</div>',
        estimatedSavings:'<div id="pendingActiveOpportunitiesEstimatedSavings1">kWh: 20,000<br>£: £25,000</div>',
        roi:'<div id="pendingActiveOpportunitiesROI1">Total: 72<br>Remaining: 72</div>',
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
            {type:'text', width:'175px', name:'estimatedDates', title:'Estimated<br>Dates', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'120px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'125px', name:'estimatedSavings', title:'Estimated <br>Savings (pa)', readOnly: true},
            {type:'text', width:'140px', name:'roi', title:'ROI<br>Months', readOnly: true},
            {type:'text', width:'197px', name:'manageOpportunity', title:'Manage Opportunity', readOnly: true},
            {type:'text', width:'195px', name:'close', title:'Close<br>Opportunity', readOnly: true},
		 ]
	  }); 
}

function setupRejectedOpportunitiesDataGrid() {
    clearElement(document.getElementById('rejectedOpportunitiesSpreadsheet'));
    var data = [];
    var row = {
        customer:'David Ford Trading Ltd', 
        opportunityType:'Custom',
        opportunityName:'LED Lighting',
		site:'Leeds <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'rejectedOpportunities'" + ', 1)"></i>',
        meter:'1234567890123',
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

function createTree(usageSites, functions) {
    var div = document.getElementById('siteTree');
    var inputs = div.getElementsByTagName('input');
    var checkboxes = getCheckedElements(inputs);
    var elements = div.getElementsByTagName("*");
  
    var checkboxIds = [];
    for(var i = 0; i < checkboxes.length; i++) {
      checkboxIds.push(checkboxes[i].id);
    }
  
    var elementClasses = [];
    for(var i = 0; i < elements.length; i++) {
      if(elements[i].id != '') {
        var element = {
          id: elements[i].id,
          classList: elements[i].classList
        }
    
        elementClasses.push(element);
      }    
    }
  
    clearElement(div);
  
    var headerDiv = createHeaderDiv("siteHeader", 'Locations', true);
    var ul = createBranchUl("siteSelector", false, true);
  
    var breakDisplayListItem = document.createElement('li');
    breakDisplayListItem.innerHTML = '<br>';
    breakDisplayListItem.classList.add('format-listitem');
  
    var recurseSelectionListItem = document.createElement('li');
    recurseSelectionListItem.classList.add('format-listitem');
    recurseSelectionListItem.classList.add('listItemWithoutPadding');
  
    var recurseSelectionCheckbox = createBranchCheckbox('recurseSelectionCheckbox', '', 'recurseSelection', 'checkbox', 'recurseSelection', false);
    var recurseSelectionSpan = createBranchSpan('recurseSelectionSpan', 'Recurse Selection?');
    recurseSelectionListItem.appendChild(recurseSelectionCheckbox);
    recurseSelectionListItem.appendChild(recurseSelectionSpan);
  
    buildSiteBranch(usageSites, '', ul, functions);  
  
    div.appendChild(headerDiv);
    ul.appendChild(breakDisplayListItem);
    ul.appendChild(recurseSelectionListItem);
    div.appendChild(ul);
  
    for(var i = 0; i < checkboxIds.length; i++) {
      var checkbox = document.getElementById(checkboxIds[i]);
      if(checkbox) {
        checkbox.checked = true;
      }
    }
  
    for(var i = 0; i < elementClasses.length; i++) {
      var element = document.getElementById(elementClasses[i].id);
      if(element) {
        element.classList = elementClasses[i].classList;
      }
    }  
}

//build site
function buildSiteBranch(usageSites, commodityOption, elementToAppendTo, functions) {
    var siteLength = usageSites.length;
  
    if(siteLocationcheckbox.checked) {
      for(var siteCount = 0; siteCount < siteLength; siteCount++) {
        var site = usageSites[siteCount];
  
        if(!commodityMatch(site, commodityOption)) {
          continue;
        }
  
        var listItem = appendListItemChildren(getAttribute(site.Attributes, "GUID"), site.hasOwnProperty('Areas'), functions, site.Attributes, 'Site');
        elementToAppendTo.appendChild(listItem);
  
        if(site.hasOwnProperty('Areas')) {
          var ul = listItem.getElementsByTagName('ul')[0];
          buildAreaBranch(site.Areas, commodityOption, ul, functions);
        }
      }
    }
    else {
      var areas = [];
      for(var siteCount = 0; siteCount < siteLength; siteCount++) {
        var site = usageSites[siteCount];
  
        if(!commodityMatch(site, commodityOption)) {
          continue;
        }
  
        areas.push(...site.Areas);
      }
  
      buildAreaBranch(areas, commodityOption, elementToAppendTo, functions);
    }
}

//build area
function buildAreaBranch(areas, commodityOption, elementToAppendTo, functions) {
    var areaLength = areas.length;
  
    if(areaLocationcheckbox.checked) {
      for(var areaCount = 0; areaCount < areaLength; areaCount++) {
        var area = areas[areaCount];
  
        if(!commodityMatch(area, commodityOption)) {
          continue;
        }
  
        var listItem = appendListItemChildren(getAttribute(area.Attributes, "GUID"), area.hasOwnProperty('Commodities'), functions, area.Attributes, 'Area')
        elementToAppendTo.appendChild(listItem);
  
        if(area.hasOwnProperty('Commodities')) {
          var ul = listItem.getElementsByTagName('ul')[0];
          buildCommodityBranch(area.Commodities, commodityOption, ul, functions);
        }
      }
    }
    else {
      var commodities = [];
      var commodityNames = [];
  
      for(var areaCount = 0; areaCount < areaLength; areaCount++) {
        var area = areas[areaCount];
  
        if(!commodityMatch(area, commodityOption)) {
          continue;
        }
  
        var commodityLength = area.Commodities.length;
  
        for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
          var commodity = area.Commodities[commodityCount];
  
          if(!commodityMatch(commodity, commodityOption)) {
            continue;
          }
  
          var commodityName = getAttribute(commodity.Attributes, "Name");
          var commodityIndex = commodityNames.indexOf(commodityName);
          if(!commodityNames.includes(commodityName)) {
            commodityNames.push(commodityName);
            commodities.push(JSON.parse(JSON.stringify(commodity)));
          }
          else {
            commodities[commodityIndex].Meters.push(...commodity.Meters);
          }
        }
      }
  
      buildCommodityBranch(commodities, commodityOption, elementToAppendTo, functions);
    }
}

//build commodity
function buildCommodityBranch(commodities, commodityOption, elementToAppendTo, functions) {
    var commodityLength = commodities.length;
  
    for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
      var commodity = commodities[commodityCount];
  
      if(!commodityMatch(commodity, commodityOption)) {
        continue;
      }
  
      if(commodityLocationcheckbox.checked) {
        var listItem = appendListItemChildren(getAttribute(commodity.Attributes, "GUID"), commodity.hasOwnProperty('Meters'), null, commodity.Attributes, 'Commodity')
        elementToAppendTo.appendChild(listItem);
      }
  
      if(commodity.hasOwnProperty('Meters')) {
        var ul = commodityLocationcheckbox.checked ? listItem.getElementsByTagName('ul')[0] : elementToAppendTo;
        buildMeterBranch(commodity.Meters, commodityOption, ul, functions);
      }
    }
}

//build meter
function buildMeterBranch(meters, commodityOption, elementToAppendTo, functions) {
    var meterLength = meters.length;
  
    if(meterLocationcheckbox.checked) {
      for(var meterCount = 0; meterCount < meterLength; meterCount++) {
        var meter = meters[meterCount];
  
        if(!commodityMatch(meter, commodityOption)) {
          continue;
        }
  
        var listItem = appendListItemChildren(getAttribute(meter.Attributes, "GUID"), meter.hasOwnProperty('SubAreas'), functions, meter.Attributes, 'Meter')
        elementToAppendTo.appendChild(listItem);
  
        if(meter.hasOwnProperty('SubAreas')) {
          var ul = listItem.getElementsByTagName('ul')[0];
          buildSubAreaBranch(meter.SubAreas, commodityOption, ul, functions);
        }
      }
    }
    else {
      var subAreas = [];
      var subAreaNames = [];
  
      for(var meterCount = 0; meterCount < meterLength; meterCount++) {
        var meter = meters[meterCount];
  
        if(!commodityMatch(meter, commodityOption)) {
          continue;
        }
  
        if(meter.SubAreas) {
          var subAreaLength = meter.SubAreas.length;
  
          for(var subAreaCount = 0; subAreaCount < subAreaLength; subAreaCount++) {
            var subArea = meter.SubAreas[subAreaCount];
  
            if(!commodityMatch(subArea, commodityOption)) {
              continue;
            }
  
            var subAreaName = getAttribute(subArea.Attributes, "Name");
            var subAreaIndex = subAreaNames.indexOf(subAreaName);
            if(!subAreaNames.includes(subAreaName)) {
              subAreaNames.push(subAreaName);
              subAreas.push(JSON.parse(JSON.stringify(subArea)));
            }
            else {
              subAreas[subAreaIndex].Assets.push(...subArea.Assets);
            }
          }
        }
      }
  
      buildSubAreaBranch(subAreas, commodityOption, elementToAppendTo, functions);
    }
}

//build sub area
function buildSubAreaBranch(subAreas, commodityOption, elementToAppendTo, functions) {
    var subAreaLength = subAreas.length;
  
    for(var subAreaCount = 0; subAreaCount < subAreaLength; subAreaCount++) {
      var subArea = subAreas[subAreaCount];
  
      if(!commodityMatch(subArea, commodityOption)) {
        continue;
      }
  
      if(subareaLocationcheckbox.checked) {
        var listItem = appendListItemChildren(getAttribute(subArea.Attributes, "GUID"), subArea.hasOwnProperty('Assets'), functions, subArea.Attributes, 'SubArea')
        elementToAppendTo.appendChild(listItem);
      }
  
      if(subArea.hasOwnProperty('Assets')) {
        var ul = subareaLocationcheckbox.checked ? listItem.getElementsByTagName('ul')[0] : elementToAppendTo;
        buildAssetBranch(subArea.Assets, commodityOption, ul, functions);
      }
    }
}

//build asset
function buildAssetBranch(assets, commodityOption, elementToAppendTo, functions) {
    var assetLength = assets.length;
  
    for(var assetCount = 0; assetCount < assetLength; assetCount++) {
      var asset = assets[assetCount];
  
      if(!commodityMatch(asset, commodityOption)) {
        continue;
      }
  
      if(assetLocationcheckbox.checked) {
        var listItem = appendListItemChildren(getAttribute(asset.Attributes, "GUID"), asset.hasOwnProperty('SubMeters'), functions, asset.Attributes, 'Asset')
        elementToAppendTo.appendChild(listItem);
      }
  
      if(asset.hasOwnProperty('SubMeters')) {
        var ul = assetLocationcheckbox.checked ? listItem.getElementsByTagName('ul')[0] : elementToAppendTo;
        buildSubMeterBranch(asset.SubMeters, commodityOption, ul, functions);
      }
    }
}

//build sub meter
function buildSubMeterBranch(subMeters, commodityOption, elementToAppendTo, functions) {
    if(!submeterLocationcheckbox.checked) {
      return;
    }
  
    var subMeterLength = subMeters.length;
  
    for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++) {
      var subMeter = subMeters[subMeterCount];
  
      if(!commodityMatch(subMeter, commodityOption)) {
        continue;
      }
  
      var listItem = appendListItemChildren(getAttribute(subMeter.Attributes, "GUID"), false, functions, subMeter.Attributes, 'SubMeter');
      elementToAppendTo.appendChild(listItem);
    }
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