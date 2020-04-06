function pageLoad() {
    createTree(data, "treeDiv", "", "", true);
    addExpanderOnClickEvents();
    loadDataGrids();
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Site(s)/Meter(s)/Sub Meter(s)';

    div.appendChild(header);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, commodity, checkboxFunction, showSubMeters) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];

        if(!commoditySiteMatch(base, commodity)) {
            continue;
        }
        
        var li = document.createElement('li');
        var ul = createUL();

        buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, base.SiteName, showSubMeters);
        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), base.SiteName, commodity, ul, true);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, branchOption, commodity, ul, childrenCreated) {
    li.appendChild(createBranchDiv(id, childrenCreated));
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(!commodityMeterMatch(meter, commodity)) {
            continue;
        }

        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meter.GUID);
        var branchDiv = createBranchDiv(branchId);
        
        if(!showSubMeters || !meter.hasOwnProperty('SubMeters')) {
            branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer');
            branchDiv.setAttribute('class', 'far fa-times-circle');
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID));
        li.appendChild(createTreeIcon('', meter.Commodity));
        li.appendChild(createSpan(branchId, meter.Identifier));

        if(showSubMeters && meter.hasOwnProperty('SubMeters')) {
            var ul = createUL();
            buildSubMeterHierarchy(meter['SubMeters'], ul, '', meter.Commodity, checkboxFunction, linkedSite);

            li.appendChild(createBranchListDiv(branchId.concat('List'), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity, checkboxFunction, linkedSite) {
    var subMetersLength = subMeters.length;
    for(var i = 0; i < subMetersLength; i++){
        var subMeter = subMeters[i];
        var li = document.createElement('li');

        var branchDiv = createBranchDiv(subMeter.GUID);
        branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox('SubMeter'.concat(subMeter.GUID), checkboxFunction, 'SubMeter', linkedSite, subMeter.GUID));
        li.appendChild(createTreeIcon(deviceType, commodity));
        li.appendChild(createSpan('SubMeter'.concat(subMeter.GUID), subMeter.Identifier));   

        baseElement.appendChild(li); 
    }
}

function createBranchDiv(branchDivId, childrenCreated = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(childrenCreated) {
        branchDiv.setAttribute('class', 'far fa-plus-square show-pointer');
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

function createTreeIcon(branch, commodity) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch, commodity));
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

function commoditySiteMatch(site, commodity) {
    if(commodity == '') {
        return true;
    }

    if(!site.hasOwnProperty('Meters')) {
        return false;
    }

    var metersLength = site.Meters.length;
    for(var i = 0; i < metersLength; i++) {
        if(commodityMeterMatch(site.Meters[i], commodity)) {
            return true;
        }
    }

    return false;
}

function commodityMeterMatch(meter, commodity) {
    if(commodity == '') {
        return true;
    }

    var meterCommodity = getAttribute(meter.Attributes, 'Commodity');
    return meterCommodity.toLowerCase() == commodity.toLowerCase();
}

function getIconByBranch(branch, commodity) {
    switch (branch) {
        case 'Mains':
            if(commodity == 'Gas') {
                return 'fas fa-burn';
            }
            else {
                return 'fas fa-plug';
            }
        case 'Lighting':
            return 'fas fa-lightbulb';
        case 'Unknown':
            return 'fas fa-question-circle';
        default:
            return 'fas fa-map-marker-alt';
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
    
    updateClassOnClick('identifiedOpportunities', 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick('createNewOpportunity', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
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

function displayContactPopup(type, row) {
    var modal = document.getElementById("popup");
	var title = document.getElementById("title");
    var span = modal.getElementsByClassName("close")[0];
    var popupText = document.getElementById("popupText");
    var site = document.getElementById(type + 'Site' + row).innerText;

    var text = 'BWS Contact: En Gineer - 07777 777777<br><br>'
                    + 'David Ford Trading Ltd Contact(s):<br>'
                    + '<span style="margin-left: 5px;">Main Office: 01234 567890</span><br><br>'
                    + '<span style="margin-left: 5px;">' + site + ':</span><br>'
                    + '<span style="margin-left: 10px;">Site Contact: David Ford - 07890 123456</span><br>'
                    + '<span style="margin-left: 10px;">Engineer Contact: Andrew Sampson - 07890 654321</span>';

    popupText.innerHTML = text;
    
    finalisePopup(title, 'Contacts<br>', modal, span);
}

function displayTimePeriodPopup(type, row) {
    var modal = document.getElementById("popup");
	var title = document.getElementById("title");
    var span = modal.getElementsByClassName("close")[0];
    var popupText = document.getElementById("popupText");

    var text = 'Time Periods Selected<br><br>'
                    + 'Months: November, December, January, February<br>'
                    + 'Days: Tuesday, Wednesday, Thursday<br>'
                    + 'Periods: 16:30 - 17:00, 17:00 - 17:30, 17:30 - 18:00';

    popupText.innerHTML = text;
    
    finalisePopup(title, 'Time Periods<br>', modal, span);
}

function finalisePopup(title, titleHTML, modal, span) {
    title.innerHTML = titleHTML;

	modal.style.display = "block";

	span.onclick = function() {
		modal.style.display = "none";
	}
}

function loadDataGrids() {
    var identifiedOpportunitiesData = [];
    var createNewOpportunityData = [];

    var row = {
		group:'1',
        opportunityType:'Custom',
        opportunityName:'LED Lighting',
        status:'Recommend',
        site:'<div id="identifiedOpportunitiesSite1">Site X <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'identifiedOpportunities'" + ', 1)"></i></div>',
        meter:'12345678910125',
        subMeter:'Sub Meter 2',
        estimatedStartDate:'01/04/2020',
        timePeriod:'All Months<br>All Days<br>All Periods',
        estimatedCost:'£5,000',
        percentageSaving:'10%',
        estimatedSavings:'kWh: 5,000<br>£: £1,000',
        roi:'Total: 60<br>Remaining: 60',
        actions:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>'
	}
	identifiedOpportunitiesData.push(row);

	row = {
		group:'2',
        opportunityType:'Custom',
        opportunityName:'LED Lighting',
        status:'Approved',
        site:'<div id="identifiedOpportunitiesSite2">Site X <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'identifiedOpportunities'" + ', 2)"></i></div>',
        meter:'12345678910126',
        subMeter:'New Sub Meter Required',
        estimatedStartDate:'01/04/2020',
        timePeriod:'Multiple Months<br>Multiple Days<br>Multiple Months <i class="fas fa-search show-pointer"onclick="displayTimePeriodPopup(' + "'identifiedOpportunities'" + ', 2)"></i>',
        estimatedCost:'£5,000,000',
        percentageSaving:'5%',
        estimatedSavings:'kWh: N/A<br>£: N/A',
        roi:'Total: N/A<br>Remaining: N/A',
        actions:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>'
	}
    identifiedOpportunitiesData.push(row);
    
    row = {
        site:'<div id="createNewOpportunitySite1">Site Z <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'createNewOpportunity'" + ', 1)"></i></div>',
        meter:'12345678910123',
        subMeter:'Sub Meter 1',
        month:'All Months',
        dayOfWeek:'All Days',
        timePeriod:'All Periods',
        percentageSaving:'',
        estimatedSavings:'kWh: 5,000<br>£: £10,000',
        actions:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>'
	}
    createNewOpportunityData.push(row);
    
    row = {
        site:'<div id="createNewOpportunitySite2">Site Y <i class="fas fa-search show-pointer" onclick="displayContactPopup(' + "'createNewOpportunity'" + ', 2)"></i></div>',
        meter:'12345678910124',
        subMeter:'New Sub Meter Required',
        month:'Multiple <i class="fas fa-search show-pointer"></i>',
        dayOfWeek:'Multiple <i class="fas fa-search show-pointer"></i>',
        timePeriod:'Multiple <i class="fas fa-search show-pointer"></i>',
        percentageSaving:'',
        estimatedSavings:'kWh: N/A<br>£: N/A',
        actions:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>'
	}
	createNewOpportunityData.push(row);

    jexcel(document.getElementById('identifiedOpportunitiesSpreadsheet'), {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: identifiedOpportunitiesData,
		columns: [
            {type:'text', width:'50px', name:'group', title:'Group'},
            {type:'text', width:'150px', name:'opportunityType', title:'Opportunity Type', readOnly: true},
            {type:'text', width:'150px', name:'opportunityName', title:'Opportunity Name', readOnly: true},
            {type:'text', width:'100px', name:'status', title:'Status', readOnly: true},
            {type:'text', width:'178px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'125px', name:'subMeter', title:'Sub Meter', readOnly: true},
            {type:'text', width:'110px', name:'estimatedStartDate', title:'Estimated<br>Start Date', readOnly: true},
            {type:'text', width:'175px', name:'timePeriod', title:'Time Periods', readOnly: true},
            {type:'text', width:'100px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving', readOnly: true},
            {type:'text', width:'150px', name:'estimatedSavings', title:'Estimated<br>Savings (pa)', readOnly: true},
            {type:'text', width:'160px', name:'roi', title:'ROI Months', readOnly: true},
            {type:'text', width:'100px', name:'actions', title:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>', readOnly: true},
		 ]
      }); 
      
    jexcel(document.getElementById('createNewOpportunitySpreadsheet'), {
		pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: createNewOpportunityData,
		columns: [
            {type:'text', width:'200px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'200px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'200px', name:'subMeter', title:'Sub Meter', readOnly: true},
            {type:'text', width:'100px', name:'month', title:'Month', readOnly: true},
            {type:'text', width:'100px', name:'dayOfWeek', title:'Day Of Week', readOnly: true},
            {type:'text', width:'100px', name:'timePeriod', title:'Time Period', readOnly: true},
            {type:'text', width:'100px', name:'percentageSaving', title:'Percentage<br>Saving'},
            {type:'text', width:'150px', name:'estimatedSavings', title:'Estimated<br>Savings (pa)', readOnly: true},
            {type:'text', width:'100px', name:'actions', title:'<input type="checkbox" class="show-pointer"></input>&nbsp<i class="fas fa-trash-alt show-pointer"></i>', readOnly: true},
		 ]
	  }); 
}