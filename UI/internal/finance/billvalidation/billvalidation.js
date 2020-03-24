function pageLoad(){  
	createTree(billvalidation, "treeDiv", "createCardButton");
	addExpanderOnClickEvents();
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
}

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
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
        var baseName = getAttribute(base.Attributes, 'Period');
        var li = document.createElement('li');
        var ul = createUL();

        buildSite(base.Sites, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Period'.concat(base.GUID), checkboxFunction, 'Period', baseName, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function buildSite(sites, baseElement, checkboxFunction, linkedSite) {
    var sitesLength = sites.length;
    for(var i = 0; i < sitesLength; i++) {
        var site = sites[i];
        var li = document.createElement('li');
        var ul = createUL();
        buildMeter(site.Meters, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Site'.concat(subBranchCount), checkboxFunction, 'Site', site.SiteName, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function buildMeter(meters, baseElement, checkboxFunction, linkedSite) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        var li = document.createElement('li');
        var ul = createUL();
        buildBill(meter.Bills, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Meter'.concat(meter.GUID), checkboxFunction, 'Meter', meter.Identifier, ul, linkedSite, '');

        baseElement.appendChild(li); 
    }
}

function buildBill(bills, baseElement, checkboxFunction, linkedSite) {
    var billsLength = bills.length;
    for(var i = 0; i < billsLength; i++){
        var bill = bills[i];

        var li = document.createElement('li');
        var ul = createUL();
        var branchId = 'Bill'.concat(bill.GUID);
        appendListItemChildren(li, branchId, checkboxFunction, 'Bill'.concat(bill.Status), bill.BillNumber, ul, linkedSite, bill.GUID);

        var branchDiv = li.children[branchId];
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        var branchIcon = li.children['Bill'.concat(bill.GUID).concat('span')];
        branchIcon.style.color = getBillStatusColour(bill.Status);

        baseElement.appendChild(li);         
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(checkboxBranch));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    branchDiv.setAttribute('class', 'far fa-plus-square');
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
    switch(branch) {
        case 'Period':
            return "far fa-calendar-alt";
        case "BillValid":
            return "fas fa-check-circle";
        case "BillInvestigation":
            return "fas fa-question-circle";
        case "BillInvalid":
            return "fas fa-times-circle";
    }    
}

function getBillStatusColour(status) {
    switch(status) {
        case "Valid":
            return "green";
        case "Investigation":
            return "orange";
        case "Invalid":
            return "red";
    }
}

function openTab(evt, tabName, guid) {
	var cardDiv = document.getElementById('cardDiv');

	var tabContent = document.getElementsByClassName("tabcontent");
	var tabContentLength = tabContent.length;
	for (var i = 0; i < tabContentLength; i++) {
	  cardDiv.removeChild(tabContent[i]);
	}

	var tabLinks = document.getElementsByClassName("tablinks");
	var tabLinksLength = tabLinks.length;
	for (var i = 0; i < tabLinksLength; i++) {
		tabLinks[i].className = tabLinks[i].className.replace(" active", "");
	}
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'tabcontent');
	newDiv.id = tabName;
	cardDiv.appendChild(newDiv);

	createCard(guid, newDiv);

	document.getElementById(tabName).style.display = "block";
	evt.currentTarget.className += " active";
  }

function createCardButton(checkbox){
	var cardDiv = document.getElementById('cardDiv');
	var tabDiv = document.getElementById('tabDiv');	

	if(checkbox.checked){
		cardDiv.setAttribute('style', '');
	}

	switch(checkbox.getAttribute('branch')) {
		case 'Period':
			createPeriodButtons(checkbox, tabDiv, cardDiv);
			break;
		case 'Site':
			createSiteButtons(checkbox, tabDiv, cardDiv);
			break;
		case 'Meter':
			createMeterButtons(checkbox, tabDiv, cardDiv);
			break;
		default:
			createBillButton(checkbox, tabDiv, cardDiv);
			break;
	}	

	if(tabDiv.children.length == 0) {
		cardDiv.setAttribute('style', 'display: none;');
	}
	else {
		updateTabDiv();
	}
}

function createPeriodButtons(checkbox, tabDiv, cardDiv) {
	var listdiv = document.getElementById(checkbox.id.replace('checkbox', 'List'));
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
	  if(inputs[i].type.toLowerCase() == 'checkbox'
	  && inputs[i].getAttribute('branch') == 'Site') {
		inputs[i].checked = !inputs[i].checked;
		createSiteButtons(inputs[i], tabDiv, cardDiv)
      }
    }
}

function createSiteButtons(checkbox, tabDiv, cardDiv) {
	var listdiv = document.getElementById(checkbox.id.replace('checkbox', 'List'));
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
	  if(inputs[i].type.toLowerCase() == 'checkbox'
	  && inputs[i].getAttribute('branch') == 'Meter') {
		inputs[i].checked = !inputs[i].checked;
		createMeterButtons(inputs[i], tabDiv, cardDiv)
      }
    }
}

function createMeterButtons(checkbox, tabDiv, cardDiv) {
	var listdiv = document.getElementById(checkbox.id.replace('checkbox', 'List'));
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
      if(inputs[i].type.toLowerCase() == 'checkbox') {
		inputs[i].checked = !inputs[i].checked;
		createBillButton(inputs[i], tabDiv, cardDiv)
      }
    }
}

function createBillButton(checkbox, tabDiv, cardDiv) {
	if(checkbox.checked) {
		var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));
		var button = document.createElement('button');
		button.setAttribute('class', 'tablinks');
		button.setAttribute('onclick', 'openTab(event, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '", "' + checkbox.getAttribute('branch') + '")');
	
		var meterTypeNode = span.parentNode.parentNode.parentNode.parentNode.children[3];
		var siteNode = meterTypeNode.parentNode.parentNode.parentNode.parentNode.children[3];
		var periodNode = siteNode.parentNode.parentNode.parentNode.parentNode.children[3];
	
		button.innerHTML = periodNode.innerText.concat(' - ').concat(siteNode.innerText.concat(' - ').concat(meterTypeNode.innerText.concat(' - ').concat(span.innerHTML)));
		button.id = span.id.replace('span', 'button');
		tabDiv.appendChild(button);
	}
	else {
		tabDiv.removeChild(document.getElementById(checkbox.id.replace('checkbox', 'button')));

		var divToRemove = document.getElementById(checkbox.id.replace('checkbox', 'div'));
		if(divToRemove) {
			if(cardDiv.children.length == 0) {
				cardDiv.setAttribute('style', 'display: none;');
			}
			else {
				cardDiv.removeChild();
			}
		}
	}
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

    tabDivChildren[0].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px;'));
    for(var i = 1; i < tabDivChildrenLength; i++) {
        tabDivChildren[i].setAttribute('style', 'width: '.concat(tabDiv.clientWidth/tabDivChildrenLength).concat('px; border-left: solid black 1px;'));
    }
}

function createCard(guid, divToAppendTo) {
	var billEntity;
	
	var dataLength = billvalidation.length;
	for(var i = 0; i < dataLength; i++) {
		var datum = billvalidation[i];
		var sites = datum.Sites;

		var siteLength = sites.length;
		for(var j = 0; j < siteLength; j++) {
			var site = sites[j];
			var meters = site.Meters;

			var meterLength = meters.length;
			for(var k = 0; k < meterLength; k++) {
				var meter = meters[k];
				var bills = meter.Bills;

				var billLength = bills.length;
				for(var l = 0; l < billLength; l++) {
					var bill = bills[l];

					if(bill.GUID == guid) {
						billEntity = bill;
						break;
					}
				}

				if(billEntity){
					break;
				}
			}

			if(billEntity){
				break;
			}
		}

		if(billEntity){
			break;
		}
	}

	if(billEntity.Status == "Valid")  {
		buildBillChart(billEntity, divToAppendTo);
	}
	buildBillDataTable(billEntity, divToAppendTo);
}

function buildBillChart(bill, divToAppendTo) {
	var chartDiv = document.createElement('div');
	chartDiv.id = "chart".concat(bill.GUID);
	chartDiv.setAttribute('class', 'chart');
	divToAppendTo.appendChild(chartDiv);

	var breakDiv = document.createElement('br');
	divToAppendTo.appendChild(breakDiv);

	// var dataSeries = [];
	// dataSeries.push({name: "Expected Volume", data: getAttribute(bill.Details, "Expected Volume")});
	// dataSeries.push({name: "Actual Volume", data: getAttribute(bill.Details, "Actual Volume")});
	// dataSeries.push({name: "Expected Spend", data: getAttribute(bill.Details, "Expected Spend")});
	// dataSeries.push({name: "Actual Spend", data: getAttribute(bill.Details, "Actual Spend")});

	// var categories = ["Expected Volume", "Actual Volume", "Expected Spend", "Actual Spend"];

	var options = {
		series: [{
		data: [getAttribute(bill.Details, "Expected Volume"), getAttribute(bill.Details, "Actual Volume"), getAttribute(bill.Details, "Expected Spend"), getAttribute(bill.Details, "Actual Spend")]
	  }],
		chart: {
		height: 350,
		type: 'bar',
	  },
	  plotOptions: {
		bar: {
		  columnWidth: '45%',
		  distributed: true
		}
	  },
	  dataLabels: {
		enabled: false
	  },
	  legend: {
		show: false
	  },
	  xaxis: {
		categories: ["Expected Volume", "Actual Volume", "Expected Spend", "Actual Spend"],
		labels: {
		  style: {
			fontSize: '12px'
		  }
		}
	  }
	  };
	
	  renderChart('#'.concat(chartDiv.id), options);
}

function renderChart(chartId, options) {
	var chart = new ApexCharts(document.querySelector(chartId), options);
	chart.render();
}

function buildBillDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);
	
	var treeDiv = document.getElementById('displayAttributes');
	clearElement(treeDiv);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	displayAttributes(entity.Details, table);

	treeDiv.appendChild(table);
}

function displayAttributes(attributes, table) {
	if(!attributes) {
		return;
	}

	var attributesLength = attributes.length;
	for(var i = 0; i < attributesLength; i++) {
		var tableRow = document.createElement('tr');

		for(var j = 0; j < 2; j++) {
			var tableDatacell = document.createElement('td');
			tableDatacell.setAttribute('style', 'border: solid black 1px;');

			switch(j) {
				case 0:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = key;
						break;
					}	
					break;
				case 1:
					for(var key in attributes[i]) {
						tableDatacell.innerHTML = attributes[i][key];
						break;
					}
					break;
			}

			tableRow.appendChild(tableDatacell);
		}

		tableRow.appendChild(tableDatacell);

		table.appendChild(tableRow);
	}	
}