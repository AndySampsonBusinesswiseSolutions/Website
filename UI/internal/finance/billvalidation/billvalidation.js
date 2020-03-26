function pageLoad(){  
	createTree(billvalidation, "treeDiv", "createCardButton");

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

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
	}

	updateClassOnClick('treeDivSelector', 'fa-plus-square', 'fa-minus-square');
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

var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
	var ul = createUL();
	ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Bills <i class="far fa-plus-square" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
	div.appendChild(tree);
	
	document.getElementById('Period13checkbox').checked = true;
	createCardButton(document.getElementById('Period13checkbox'));
	openTab(document.getElementById('Bill15button'), 'Bill15button', '15');
	addExpanderOnClickEvents();
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

function openTab(callingElement, tabName, guid) {
	var tabLinks = document.getElementsByClassName("tablinks");
	var tabLinksLength = tabLinks.length;
	for (var i = 0; i < tabLinksLength; i++) {
		tabLinks[i].className = tabLinks[i].className.replace(" active", "");
	}
	callingElement.className += " active";
	
	var newDiv = document.createElement('div');
	newDiv.setAttribute('class', 'card');
	newDiv.id = tabName;

	var cardDiv = document.getElementById('cardDiv');
	clearElement(cardDiv);
	cardDiv.appendChild(newDiv);

	createCard(guid, newDiv);

	newDiv.style.display = "block";
  }

function createCardButton(checkbox){
	var tabDiv = document.getElementById('tabDiv');	
	var id = checkbox.id.replace('checkbox', 'List');

	switch(checkbox.getAttribute('branch')) {
		case 'Period':
			createPeriodButtons(id, tabDiv);
			break;
		case 'Site':
			createSiteButtons(id, tabDiv);
			break;
		case 'Meter':
			createMeterButtons(id, tabDiv);
			break;
		default:
			createBillButton(checkbox, tabDiv);
			break;
	}	

	updateTabDiv();
}

function createPeriodButtons(id, tabDiv) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;

    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox'
		&& input.getAttribute('branch') == 'Site') {
			input.checked = !input.checked;
			createSiteButtons(input.id.replace('checkbox', 'List'), tabDiv);
		}
	}
}

function createSiteButtons(id, tabDiv) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox'
		&& input.getAttribute('branch') == 'Meter') {
			input.checked = !input.checked;
			createMeterButtons(input.id.replace('checkbox', 'List'), tabDiv);
		}
	}
}

function createMeterButtons(id, tabDiv) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;

    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox') {
			input.checked = !input.checked;
			createBillButton(input, tabDiv);
		}
	}	
}

function createBillButton(checkbox, tabDiv) {
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));

	if(checkbox.checked) {	
		var button = document.createElement('button');
		button.setAttribute('class', 'tablinks');
		button.setAttribute('onclick', 'openTab(this, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '")');
	
		var meterTypeNode = span.parentNode.parentNode.parentNode.parentNode.children[3];
		var siteNode = meterTypeNode.parentNode.parentNode.parentNode.parentNode.children[3];
		var periodNode = siteNode.parentNode.parentNode.parentNode.parentNode.children[3];
	
		button.innerHTML = periodNode.innerText.concat(' - ').concat(siteNode.innerText.concat(' - ').concat(meterTypeNode.innerText.concat(' - ').concat(span.innerHTML)));
		button.id = span.id.replace('span', 'button');
		tabDiv.appendChild(button);
	}
	else {
		var button = document.getElementById(span.id.replace('span', 'button'));
		tabDiv.removeChild(button);
	}
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

	if(tabDivChildrenLength == 0) {
		cardDiv.style.display = 'none';
		tabDiv.style.display = 'none';
	}
	else {
		var percentage = (1 / tabDivChildrenLength) * 100;
		tabDivChildren[0].setAttribute('style', 'width: '.concat(percentage).concat('%;'));
		for(var i = 1; i < tabDivChildrenLength; i++) {
			tabDivChildren[i].setAttribute('style', 'width: '.concat(percentage).concat('%; border-left: solid black 1px;'));
		}
		
		tabDiv.style.display = '';
	}	
}

function createCard(guid, divToAppendTo) {
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
						populateCard(bill, divToAppendTo);
						return;
					}
				}
			}
		}
	}
}

function populateCard(bill, divToAppendTo) {
	if(bill.Status == "Valid")  {
		buildBillChart(bill, divToAppendTo);
	}

	buildBillDataTable(bill, divToAppendTo);
}

function buildBillChart(bill, divToAppendTo) {
	var firstChartDiv = document.createElement('div');
	firstChartDiv.id = "firstChartDiv";
	firstChartDiv.setAttribute('class', 'roundborder chart');
	firstChartDiv.setAttribute('style', 'margin-right: 5px;');
	divToAppendTo.appendChild(firstChartDiv);

	var secondChartDiv = document.createElement('div');
	secondChartDiv.id = "secondChartDiv";
	secondChartDiv.setAttribute('class', 'roundborder chart');
	divToAppendTo.appendChild(secondChartDiv);

	var firstChart = document.createElement('div');
	firstChart.id = "firstChart";
	firstChartDiv.appendChild(firstChart);

	var secondChart = document.createElement('div');
	secondChart.id = "secondChart";
	secondChartDiv.appendChild(secondChart);

	var clearDiv = document.createElement('div');
	clearDiv.setAttribute('style', 'clear: left;')
	divToAppendTo.appendChild(clearDiv);
	divToAppendTo.appendChild(document.createElement('br'));

	var options = {
		series: [{
			name: "Expected Usage",
			data: [getAttribute(bill.Details, "Expected Usage")]
		  },
		  {
			name: "Actual Usage",
			data: [getAttribute(bill.Details, "Actual Usage")]
	  	}],
	  	chart: {
		height: '100%',
		width: '100%',
		type: 'bar',
		stacked: false,
		zoom: {
			type: 'x',
			enabled: true,
			autoScaleYaxis: true
		},
		animations: {
			enabled: true,
			easing: 'easeout',
			speed: 800,
			animateGradually: {
				enabled: true,
				delay: 150
			},
			dynamicAnimation: {
				enabled: true,
				speed: 350
			}
		},
		toolbar: {
			autoSelected: 'zoom',
			tools: {
				download: false
			}
		}
		},
		dataLabels: {
			enabled: false
		},
	  	title: {
			text: 'Expected Usage v Actual Usage',
			align: 'center'
		},
		legend: {
			show: true,
			showForSingleSeries: true,
			showForNullSeries: true,
			showForZeroSeries: true,
			floating: false,
			position: 'right',
			onItemClick: {
				toggleDataSeries: true
			},
			width: 100,
			offsetY: 150,
			formatter: function(seriesName) {
				return seriesName + '<br><br>';
			}
		},
		yaxis: [{
			axisTicks: {
				show: true
			},
				axisBorder: {
				show: true,
			},
			forceNiceScale: true,
			title: {
				text: 'kWh Usage',
			},
			show: true,
			decimalsInFloat: 0,
			labels: {
				formatter: function(val) {
					return val.toLocaleString();
				}
			}
		}],
		xaxis: {
			type: 'category',
			categories: ["Expected Usage", "Actual Usage"]
		}
	};

	var secondaryChartOptions = JSON.parse(JSON.stringify(options));
	secondaryChartOptions.series = [{
		name: "Expected Spend",
		data: [getAttribute(bill.Details, "Expected Spend")]
	  },
	  {
		name: "Actual Spend",
		data: [getAttribute(bill.Details, "Actual Spend")]
	  }]
	secondaryChartOptions.title.text = 'Expected Spend v Actual Spend';
	secondaryChartOptions.legend.formatter = function(seriesName) {
		return seriesName + '<br><br>';
	}
	secondaryChartOptions.yaxis[0].title.text = '£ Spend';
	secondaryChartOptions.yaxis[0].labels.formatter = function(val) {
		return '£' + val.toLocaleString();
	}
	secondaryChartOptions.xaxis.categories = ["Expected Spend", "Actual Spend"];

	renderChart('#firstChart', options);
	renderChart('#secondChart', secondaryChartOptions);
}

function renderChart(chartId, options) {
	var chart = new ApexCharts(document.querySelector(chartId), options);
	chart.render();
}

function buildBillDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	divToAppendTo.appendChild(div);

	var table = document.createElement('table');
	table.id = 'dataTable';
	table.setAttribute('style', 'width: 100%;');

	displayAttributes(entity.Details, table);

	div.appendChild(table);
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