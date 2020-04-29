function pageLoad() {
	addExpanderOnClickEvents();
	setOpenExpanders();
	loadDataGrids();
	setupRequestVisitPopup();
}

function setupRequestVisitPopup() {
	createTree(data, "requestVisitSiteDiv");

	var modal = document.getElementById("requestVisitPopup");
	var btn = document.getElementById("requestVisitButton");
	var span = modal.getElementsByClassName("close")[0];

	btn.onclick = function() {
		modal.style.display = "block";
	}

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function openApproveRejectOpportunityPopup(element) {
	var modal = document.getElementById("approveRejectOpportunityPopup");
	var title = document.getElementById("approveRejectOpportunityTitle");
	var estimatedAnnualSavings = document.getElementById("approveRejectOpportunityEstimatedAnnualSavings");
	var span = modal.getElementsByClassName("close")[0];
	var projectSpan;
	var approve = hasClass(element, 'approve');
	var label = document.getElementById("approveRejectOpportunityNotesLabel");
	var notes = document.getElementById("approveRejectOpportunityNotes");

	if(element.id.includes('Site')) {
		var list = element.parentNode.parentNode.parentNode.id;
		projectSpan = document.getElementById(list.replace('List', 'span'));

		var siteTitle = document.getElementById("approveRejectOpportunitySiteTitle");
		siteTitle.style.display = "";
		siteTitle.innerText = 'For ' + element.innerHTML.replace((approve ? 'Approve<br>' : 'Reject<br>'), '');
	}
	else {
		projectSpan = document.getElementById(element.id.replace((approve ? 'ApproveOpportunityButton' : 'RejectOpportunityButton'), 'span'));
	}

	if(approve) {
		title.innerText = 'Approve Opportunity - ' + projectSpan.innerText;
		label.style.display = "none";
		notes.style.display = "none";
	}
	else {
		title.innerText = 'Reject Opportunity - ' + projectSpan.innerText;
		label.style.display = "";
		notes.style.display = "";
	}

	estimatedAnnualSavings.innerHTML = document.getElementById(element.id.replace((approve ? 'ApproveOpportunityButton' : 'RejectOpportunityButton'), 'EstimatedAnnualSavings')).innerHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function displayFutureSiteVisitPopup(row) {
	var modal = document.getElementById("futureSiteVisitPopup");
	var span = modal.getElementsByClassName("close")[0];
	var title = modal.getElementsByClassName("title")[0];
	modal.style.display = "block";

	var btn = document.getElementById("futureSiteVisitButton" + row);
	var visitDate = btn.getAttribute('dateOfVisit');
	var engineer = btn.getAttribute('engineer');

	title.innerHTML = 'Site Visit Request for ' + visitDate;

	var futureSiteVisitAssignedEngineer = document.getElementById("futureSiteVisitAssignedEngineer");
	futureSiteVisitAssignedEngineer.innerHTML = engineer;

	var futureSiteVisitSiteList = document.getElementById("futureSiteVisitSiteList");
	futureSiteVisitSiteList.innerHTML = "Site X<br>Site Y<br>";

	var futureSiteVisitNotes = document.getElementById("futureSiteVisitNotes");
	futureSiteVisitNotes.innerHTML = "Here is where we will list all notes currently joined to this site visit<br>It can include notes from customer, notes from internal users, any rearrangement dates that have been agreed etc";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function displayHistoricalSiteVisitPopup(row) {
	var modal = document.getElementById("historicalSiteVisitPopup");
	var span = modal.getElementsByClassName("close")[0];
	var title = document.getElementById("historicalSiteVisitTitle");
	modal.style.display = "block";

	var btn = document.getElementById("historicalSiteVisitButton" + row);
	var visitDate = btn.getAttribute('dateOfVisit');
	var engineer = btn.getAttribute('engineer');

	title.innerText = 'Site Visit Request for ' + visitDate;

	var historicalSiteVisitAssignedEngineer = document.getElementById("historicalSiteVisitAssignedEngineer");
	historicalSiteVisitAssignedEngineer.innerHTML = engineer;

	var historicalSiteVisitSiteList = document.getElementById("historicalSiteVisitSiteList");
	historicalSiteVisitSiteList.innerHTML = "Site X";

	var historicalSiteVisitRecommendedOpportunities = [{
		customer:'David Ford Trading Ltd',
		opportunityType:'Custom',
		opportunityName:'LED Lighting',
		site:'Site X',
		meter:'12345678910125',
		engineer:'En Gineer',
		startDate:'01/01/2017',
		finishDate:'28/02/2017',
		percentageSaving:'10%',
		estimatedCost:'£100,000',
		estimatedVolumeSavings: '10,000',
		estimatedCostSavings:'£15,000'
	}];

	var historicalSiteVisitPendingActiveOpportunities = [{
		customer:'David Ford Trading Ltd',
		opportunityType:'Custom',
		opportunityName:'LED Lighting',
		site:'Site X',
		meter:'12345678910125',
		engineer:'En Gineer',
		startDate:'01/01/2017',
		finishDate:'28/02/2017',
		percentageSaving:'10%',
		estimatedCost:'£100,000',
		estimatedVolumeSavings: '10,000',
		estimatedCostSavings:'£15,000'
	}];

	var historicalSiteVisitFinishedOpportunities = [{
		projectName:'LED Lighting',
		site:'Site X',
		meter:'N/A',
		engineer:'En Gineer',
		startDate:'01/01/2017',
		finishDate:'28/02/2017',
		cost:'£55,000',
		actualVolumeSavings:'10,000',
		actualCostSavings:'£65,000',
		estimatedVolumeSavings: '9,000',
		estimatedCostSavings:'£60,000',
		netVolumeSavings:'45,000',
		netCostSavings:'£140,000',
		totalROIMonths:'9',
		remainingROIMonths:'0'
	}];

	clearElement(document.getElementById('historicalSiteVisitRecommendedOpportunitiesSpreadsheet'));
	clearElement(document.getElementById('historicalSiteVisitPendingActiveOpportunitiesSpreadsheet'));
	clearElement(document.getElementById('historicalSiteVisitFinishedOpportunitiesSpreadsheet'));

	jexcel(document.getElementById('historicalSiteVisitRecommendedOpportunitiesSpreadsheet'), {
		pagination:5,
		data: historicalSiteVisitRecommendedOpportunities,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		columns: [
			{type:'text', width:'150px', name:'customer', title:'Customer', readOnly: true},
			{type:'text', width:'150px', name:'opportunityType', title:'Opportunity Type', readOnly: true},
			{type:'text', width:'150px', name:'opportunityName', title:'Opportunity Name', readOnly: true},
			{type:'text', width:'100px', name:'site', title:'Site', readOnly: true},
			{type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
			{type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
			{type:'text', width:'100px', name:'startDate', title:'Estimated<br>Start Date', readOnly: true},
			{type:'text', width:'100px', name:'finishDate', title:'Estimated<br>Finish Date', readOnly: true},
			{type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
			{type:'text', width:'100px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
			{type:'text', width:'125px', name:'estimatedVolumeSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true}
		 ]
	});	
	
	jexcel(document.getElementById('historicalSiteVisitPendingActiveOpportunitiesSpreadsheet'), {
		pagination:5,
		data: historicalSiteVisitPendingActiveOpportunities,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		columns: [
			{type:'text', width:'150px', name:'customer', title:'Customer', readOnly: true},
			{type:'text', width:'150px', name:'opportunityType', title:'Opportunity Type', readOnly: true},
			{type:'text', width:'150px', name:'opportunityName', title:'Opportunity Name', readOnly: true},
			{type:'text', width:'100px', name:'site', title:'Site', readOnly: true},
			{type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
			{type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
			{type:'text', width:'100px', name:'startDate', title:'Estimated<br>Start Date', readOnly: true},
			{type:'text', width:'100px', name:'finishDate', title:'Estimated<br>Finish Date', readOnly: true},
			{type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
			{type:'text', width:'100px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
			{type:'text', width:'125px', name:'estimatedVolumeSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true}
		 ]
	});	
	
	jexcel(document.getElementById('historicalSiteVisitFinishedOpportunitiesSpreadsheet'), {
		pagination:5,
		data: historicalSiteVisitFinishedOpportunities,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		columns: [
			{type:'text', width:'150px', name:'projectName', title:'Project Name', readOnly: true},
			{type:'text', width:'100px', name:'site', title:'Site', readOnly: true},
			{type:'text', width:'100px', name:'meter', title:'Meter', readOnly: true},
			{type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
			{type:'text', width:'100px', name:'startDate', title:'Start Date', readOnly: true},
			{type:'text', width:'100px', name:'finishDate', title:'Finish Date', readOnly: true},
			{type:'text', width:'100px', name:'cost', title:'Cost', readOnly: true},
			{type:'text', width:'100px', name:'actualVolumeSavings', title:'Actual kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'100px', name:'actualCostSavings', title:'Actual £<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'estimatedVolumeSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true},
			{type:'text', width:'100px', name:'netVolumeSavings', title:'Net kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'100px', name:'netCostSavings', title:'Net £<br>Savings (pa)', readOnly: true},
			{type:'text', width:'100px', name:'totalROIMonths', title:'Total<br>ROI Months', readOnly: true},
			{type:'text', width:'100px', name:'remainingROIMonths', title:'Remaining<br>ROI Months', readOnly: true},
		 ]
    });	

	var historicalSiteVisitNotes = document.getElementById("historicalSiteVisitNotes");
	historicalSiteVisitNotes.innerHTML = "Here is where we will list all notes currently joined to this site visit<br>It can include notes from customer, notes from internal users, any rearrangement dates that have been agreed etc";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function displaySiteRankingPopup(row) {
	var modal = document.getElementById("siteRankingPopup");
	var span = modal.getElementsByClassName("close")[0];
	var title = modal.getElementsByClassName("title")[0];
	modal.style.display = "block";

	var btn = document.getElementById("SiteRanking" + row);
	var site = btn.getAttribute('site');

	title.innerHTML = 'Site Ranking for ' + site;

	var siteRankingOpportunityList = document.getElementById("siteRankingOpportunityList");

	if(row == 1) {
		siteRankingOpportunityList.innerHTML = 
			"Project: LED Lighting Installation (Recommended)<br>"
			+ "kWh Savings: 5,000<br>"
			+ "£ Savings: £5,000<br><br>"
			+ "Project: LED Lighting Installation 2 (Recommended)<br>"
			+ "kWh Savings: 5,000<br>"
			+ "£ Savings: £5,000<br>";
	}
	else {
		siteRankingOpportunityList.innerHTML = 
			"Project: LED Lighting Installation (Recommended)<br>"
			+ "kWh Savings: 2,000<br>"
			+ "£ Savings: £2,000<br><br>"
			+ "Project: LED Lighting Installation 2 (Recommended)<br>"
			+ "kWh Savings: 2,000<br>"
			+ "£ Savings: £2,000<br><br>"
			+ "Project: Freezer Management (Under Analysis)<br>"
			+ "kWh Savings: 46,000<br>"
			+ "£ Savings: £46,000<br>";
	}
	
	span.onclick = function() {
		modal.style.display = "none";
	}
}

function siteIsSelected() {
	var inputs = document.getElementById("requestVisitSiteDiv").getElementsByTagName('input');
	var inputLength = inputs.length;
	
	for(var i = 0; i < inputLength; i++) {
	  var input = inputs[i];
	  if(input.type.toLowerCase() == 'checkbox') {
		if(input.checked) {
			return true;
		}
	  }
	}

	return false;
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
    header.innerText = "Select Site(s)";

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
	var modal = document.getElementById("requestVisitPopup");
	var requestVisitSiteRequiredMessage = document.getElementById("requestVisitSiteRequiredMessage");
	var requestVisitVisitDateRequiredMessage = document.getElementById("requestVisitVisitDateRequiredMessage");
	var visitDate = document.getElementById("requestVisitVisitDate").value;
	var notes = document.getElementById("requestVisitNotes").value;

	var popupIsValid = true;
	if(!siteIsSelected()) {
		popupIsValid = false;
		requestVisitSiteRequiredMessage.style.display = 'block';
		window.setTimeout("closeElement('requestVisitSiteRequiredMessage');", 2000);
	}
	else {
		requestVisitSiteRequiredMessage.style.display = 'none';
	}

	if(visitDate == "") {
		popupIsValid = false;
		requestVisitVisitDateRequiredMessage.style.display = 'block';
		requestVisitVisitDateRequiredMessage.innerHTML = '<i class="fas fa-exclamation-circle">Please select a visit date</i>'
		window.setTimeout("closeElement('requestVisitVisitDateRequiredMessage');", 2000);
	}
	else {
		var requestVisitDate = new Date(visitDate);

		if(requestVisitDate < new Date()) {
			popupIsValid = false;
			requestVisitVisitDateRequiredMessage.style.display = 'block';
			requestVisitVisitDateRequiredMessage.innerHTML = '<i class="fas fa-exclamation-circle">Please select a visit date in the future</i>'
			window.setTimeout("closeElement('requestVisitVisitDateRequiredMessage');", 2000);
		}
		else {
			requestVisitVisitDateRequiredMessage.style.display = 'none';
		}
	}

	if(popupIsValid) {
		modal.style.display = "none";
	}
	else {
		event.preventDefault();
		return false;
	}
}

function approveRejectOpportunity() {
	var modal = document.getElementById("approveRejectOpportunityPopup");
	var title = document.getElementById("approveRejectOpportunityTitle");
	
	if(title.innerText.includes('Approve')) {

	}
	else {

	}

	modal.style.display = "none";
}

function closeElement(elementId){
	document.getElementById(elementId).style.display="none";
}

function loadDataGrids() {
	var futureSiteVisitData = [];
	var historicalSiteVisitData = [];
	var siteRankings = [];

	for(var i = 1; i < 6; i++) {
		var visitDate = new Date(new Date().getFullYear(), new Date().getMonth() + i, new Date().getDate()).toDateString();
		var row = {
			dateOfVisit: visitDate, 
			engineer:'En Gineer',
			notes:'<i class="fas fa-search show-pointer" id="futureSiteVisitButton' + i + '" dateOfVisit="' + visitDate + '" engineer="En Gineer" onclick="displayFutureSiteVisitPopup(' + i + ')"></i>',
		}
		futureSiteVisitData.push(row);

		visitDate = new Date(new Date().getFullYear(), new Date().getMonth() - i, new Date().getDate()).toDateString();
		row = {
			dateOfVisit: visitDate, 
			engineer:'En Gineer',
			notes:'<i class="fas fa-search show-pointer" id="historicalSiteVisitButton' + i + '" dateOfVisit="' + visitDate + '" engineer="En Gineer" onclick="displayHistoricalSiteVisitPopup(' + i + ')"></i>',
		}
		historicalSiteVisitData.push(row);
	}

	var row = {
		siteName:'Site X <i onclick="displaySiteRankingPopup(1)" class="fas fa-search show-pointer"></i>', 
		ranking:'<div id="SiteRanking1" site="Site X" initialValue="3""></div>',
		savingskWh: '10,000',
		savingsCost: '£10,000',
		capex: '£10,000',
		opex: '£10,000',
	}
	siteRankings.push(row);

	row = {
		siteName:'Site Y <i onclick="displaySiteRankingPopup(2)" class="fas fa-search show-pointer"></i>', 
		ranking:'<div id="SiteRanking2" site="Site Y" initialValue="5""></div>',
		savingskWh: '50,000',
		savingsCost: '£50,000',
		capex: '£50,000',
		opex: '£50,000',
	}
	siteRankings.push(row);

	jexcel(document.getElementById('futureSiteVisitSpreadsheet'), {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		minDimensions:[3,10],
		data: futureSiteVisitData,
		columns: [
			{type:'text', width:'150px', name:'dateOfVisit', title:'Date Of Visit', readOnly: true},
			{type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
			{type:'text', width:'100px', name:'notes', title:'Notes', readOnly: true},
		 ]
	  }); 

	jexcel(document.getElementById('historicalSiteVisitSpreadsheet'), {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		minDimensions:[3,10],
		data: historicalSiteVisitData,
		columns: [
			{type:'text', width:'150px', name:'dateOfVisit', title:'Date Of Visit', readOnly: true},
			{type:'text', width:'100px', name:'engineer', title:'Engineer', readOnly: true},
			{type:'text', width:'100px', name:'notes', title:'Notes', readOnly: true},
		 ]
	  }); 

	jexcel(document.getElementById('siteRankingSpreadsheet'), {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		minDimensions:[6,10],
		data: siteRankings,
		columns: [
			{type:'text', width:'165px', name:'siteName', title:'Site Name', readOnly: true},
			{type:'text', width:'150px', name:'ranking', title:'Ranking', readOnly: true},
			{type:'text', width:'125px', name:'savingskWh', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'savingsCost', title:'Estimated £<br>Savings (pa)', readOnly: true},
			{type:'text', width:'125px', name:'capex', title:'CAPEX<br>spend', readOnly: true},
			{type:'text', width:'125px', name:'opex', title:'OPEX<br>spend', readOnly: true},
		 ]
	  }); 

	jSuites.rating(document.getElementById("SiteRanking1"), 
		{
			value: 3,
			onchange: function(el, val) {
				resetRating(el, val);
			}
		});
	jSuites.rating(document.getElementById("SiteRanking2"), 
		{
			value: 5,
			onchange: function(el, val) {
				resetRating(el, val);
			}
		});
}

function resetRating(el, val) {
	var initialValue = el.getAttribute('initialValue');
	if(initialValue != val) {
		var i = 0;
		for(i = 1; i <= initialValue; i++) {
			el.children[i - 1].setAttribute('class', 'jrating-selected');
		}

		for(var j = i; j <= 5; j++) {
			el.children[j - 1].setAttribute('class', '');
		}
	}
}