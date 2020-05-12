function pageLoad(){  
	createTree(billvalidation, "billTree", "createCardButton");
	addExpanderOnClickEvents();
	setOpenExpanders();

	document.onmousemove = function(e) {
		setupSidebarHeight();
		setupSidebar(e);
	};
	
	window.onscroll = function() {
		setupSidebarHeight();
	};
}

var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
	tree.setAttribute('class', 'scrolling-wrapper');
	
	var headerDiv = createHeaderDiv("siteHeader", 'Bills', true);
  	var ul = createBranchUl("siteSelector", false, true);

    tree.appendChild(ul);

    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
	div.appendChild(tree);
	
	document.getElementById('Period13checkbox').checked = true;
	createCardButton(document.getElementById('Period13checkbox'));
	openTab(document.getElementById('Bill15button'), 'Bill15div', '15');
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
        branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer');
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
			createPeriodButtons(id, tabDiv, checkbox.checked);
			break;
		case 'Site':
			createSiteButtons(id, tabDiv, checkbox.checked);
			break;
		case 'Meter':
			createMeterButtons(id, tabDiv, checkbox.checked);
			break;
		default:
			createBillButton(checkbox, tabDiv);
			break;
	}	

	updateTabDiv();
}

function createPeriodButtons(id, tabDiv, isChecked) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;

    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox'
		&& input.getAttribute('branch') == 'Site') {
			input.checked = isChecked;
			createSiteButtons(input.id.replace('checkbox', 'List'), tabDiv, isChecked);
		}
	}
}

function createSiteButtons(id, tabDiv, isChecked) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox'
		&& input.getAttribute('branch') == 'Meter') {
			input.checked = isChecked;
			createMeterButtons(input.id.replace('checkbox', 'List'), tabDiv, isChecked);
		}
	}
}

function createMeterButtons(id, tabDiv, isChecked) {
	var listdiv = document.getElementById(id);
	var inputs = listdiv.getElementsByTagName('input');
	var inputLength = inputs.length;

    for(var i = 0; i < inputLength; i++) {
		var input = inputs[i];
		if(input.type.toLowerCase() == 'checkbox') {
			input.checked = isChecked;
			createBillButton(input, tabDiv);
		}
	}	
}

function createBillButton(checkbox, tabDiv) {
	var span = document.getElementById(checkbox.id.replace('checkbox', 'span'));
	var button = document.getElementById(span.id.replace('span', 'button'));

	if(checkbox.checked) {	
		if(!button) {
			button = document.createElement('button');
			button.setAttribute('class', 'tablinks');
			button.setAttribute('onclick', 'openTab(this, "' + span.id.replace('span', 'div') +'", "' + checkbox.getAttribute('guid') + '")');
		
			var meterTypeNode = span.parentNode.parentNode.parentNode.parentNode.children[3];
			var siteNode = meterTypeNode.parentNode.parentNode.parentNode.parentNode.children[3];
			var periodNode = siteNode.parentNode.parentNode.parentNode.parentNode.children[3];
		
			button.innerHTML = periodNode.innerText.concat(' - ').concat(siteNode.innerText.concat(' - ').concat(meterTypeNode.innerText.concat(' - ').concat(span.innerHTML)));
			button.innerHTML += '<i class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Bill To Download Basket"></i>'
							  + '<i class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Bill"></i>';
	
			button.id = span.id.replace('span', 'button');
			tabDiv.appendChild(button);
		}		
	}
	else {
		if(button) {
			tabDiv.removeChild(button);
		}		
	}
}

function updateTabDiv() {
	var tabDiv = document.getElementById('tabDiv');
	var tabDivChildren = tabDiv.children;
	var tabDivChildrenLength = tabDivChildren.length;

	if(tabDivChildrenLength == 0) {
		var cardDiv = document.getElementById('cardDiv');
		clearElement(cardDiv);

		document.getElementById('Period13checkbox').checked = true;
		createCardButton(document.getElementById('Period13checkbox'));
		openTab(document.getElementById('Bill15button'), 'Bill15div', '15');
	}
	else {
		var percentage = (1 / tabDivChildrenLength) * 100;
		tabDivChildren[0].setAttribute('style', 'width: '.concat(percentage).concat('%;'));
		for(var i = 1; i < tabDivChildrenLength; i++) {
			tabDivChildren[i].setAttribute('style', 'width: '.concat(percentage).concat('%; border-left: solid black 1px;'));
		}
		
		tabDiv.style.display = '';

		for(var i = 0; i < tabDivChildrenLength; i++) {
			if(hasClass(tabDivChildren[i], 'active')) {
				cardDiv.style.display = '';
				return;
			}
		}

		var lastChild = tabDivChildren[i - 1];
		lastChild.className += " active";
		lastChild.dispatchEvent(new Event('click'));
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
	var containerDiv = document.createElement('div');
	containerDiv.setAttribute('class', 'expander-header');
	divToAppendTo.appendChild(containerDiv);

	var containerDivSpan = document.createElement('span');
	containerDivSpan.innerText = 'Charts';
	containerDiv.appendChild(containerDivSpan);

	var containerDivIcon = document.createElement('i');
	containerDivIcon.id = 'billChart';
	containerDivIcon.setAttribute('class', 'far fa-plus-square show-pointer');
	containerDivIcon.setAttribute('style', 'margin-left: 5px;');
	containerDiv.appendChild(containerDivIcon);

	var containerListDiv = document.createElement('div');
	containerListDiv.id = 'billChartList';
	containerListDiv.setAttribute('style', 'margin-top: 5px;');
	divToAppendTo.appendChild(containerListDiv);

	var firstChartDiv = document.createElement('div');
	firstChartDiv.id = "firstChartDiv";
	firstChartDiv.setAttribute('class', 'roundborder chart');
	firstChartDiv.setAttribute('style', 'float: left;');
	containerListDiv.appendChild(firstChartDiv);

	var secondChartDiv = document.createElement('div');
	secondChartDiv.id = "secondChartDiv";
	secondChartDiv.setAttribute('class', 'roundborder chart');
	firstChartDiv.setAttribute('style', 'float: right;');
	containerListDiv.appendChild(secondChartDiv);

	var firstChart = document.createElement('div');
	firstChart.id = "firstChart";
	firstChartDiv.appendChild(firstChart);

	var secondChart = document.createElement('div');
	secondChart.id = "secondChart";
	secondChartDiv.appendChild(secondChart);

	var clearDiv = document.createElement('div');
	clearDiv.setAttribute('style', 'clear: left;');
	divToAppendTo.appendChild(clearDiv);

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

	addExpanderOnClickEventsByElement(containerDivIcon);
	updateClassOnClick(containerDivIcon.id, 'fa-plus-square', 'fa-minus-square');
}

function buildBillDataTable(entity, divToAppendTo){
	var div = document.createElement('div');
	div.id = 'displayAttributes';
	div.setAttribute('style', 'margin-top: 5px;');
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