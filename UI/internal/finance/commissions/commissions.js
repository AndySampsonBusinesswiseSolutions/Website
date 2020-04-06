function pageLoad() {
  createTree(data, "treeDiv", "", "updateChart(commissionChart)");
  addExpanderOnClickEvents();

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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, commodity, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    ul.id = divId.concat('SelectorList');
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, commodity, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);

    var header = document.createElement('span');
    header.style = "padding-left: 5px;";
    header.innerHTML = 'Select Sites/Meters <i class="far fa-plus-square show-pointer"" id="' + divId.concat('Selector') + '"></i>';

    div.appendChild(header);
    div.appendChild(tree);

    updateChart(null, commissionChart);
}

function buildTree(baseData, baseElement, commodity, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();

        buildCustomerHierarchy(base.Customers, ul, commodity, checkboxFunction, baseName);
        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, linkedCustomer) {
    li.appendChild(createBranchDiv(id, true));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid, linkedCustomer));
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildCustomerHierarchy(customers, baseElement, commodity, checkboxFunction, linkedSite) {
    var customersLength = customers.length;
    for(var i = 0; i < customersLength; i++){
        var customer = customers[i];
        if(!commoditySiteMatch(customer, commodity)) {
            continue;
        }

        var customerAttributes = customer.Attributes;
        var identifier = getAttribute(customerAttributes, 'CustomerName');
        var li = document.createElement('li');
        var ul = createUL();

        buildIdentifierHierarchy(customer.Meters, ul, commodity, checkboxFunction, linkedSite, identifier);
        appendListItemChildren(li, commodity.concat('Customer').concat(customer.GUID), checkboxFunction, 'Customer', identifier, commodity, ul, linkedSite, customer.GUID, identifier);

        baseElement.appendChild(li); 
    }
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, linkedCustomer) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(!commodityMeterMatch(meter, commodity)) {
            continue;
        }

        var meterAttributes = meter.Attributes;
        var identifier = getAttribute(meterAttributes, 'Identifier');
        var meterCommodity = getAttribute(meterAttributes, 'Commodity');
        var deviceType = getAttribute(meterAttributes, 'DeviceType');
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meter.GUID);
        var branchDiv = createBranchDiv(branchId, false);

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID, linkedCustomer));
        li.appendChild(createTreeIcon(deviceType, meterCommodity));
        li.appendChild(createSpan(branchId, identifier));

        baseElement.appendChild(li); 
    }
}

function createBranchDiv(branchDivId, childrenCreated = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(childrenCreated) {
        branchDiv.setAttribute('class', 'far fa-plus-square show-pointer');
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

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid, linkedCustomer) {
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

    if(linkedCustomer) {
        checkBox.setAttribute('LinkedCustomer', linkedCustomer);
    }

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

function getCommodity() {
  var commodity = '';
  var electricityCommodityradio = document.getElementById('electricityCommodityradio');
  if(electricityCommodityradio.checked) {
    commodity = 'Electricity';
  }
  else {
    var gasCommodityradio = document.getElementById('gasCommodityradio');
    if(gasCommodityradio.checked) {
      commodity = 'Gas';
    }
  }
  return commodity;
}

function updateChart(callingElement, chart) {
    var treeDiv = document.getElementById('treeDiv');
    var inputs = treeDiv.getElementsByTagName('input');
    var commodity = getCommodity();
    var checkBoxes = getCheckedCheckBoxes(inputs);
    var chartTitle = "Previous 12 Months " + (commodity == '' ? '' : (commodity + ' ')) + "Commission";
    clearElement(chart);
    
    var newCategories = getNewCategories();   
    var newSeries = getNewChartSeries(checkBoxes, 'Commission', newCategories, commodity);
  
    var chartOptions = {
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
        text: chartTitle,
        align: 'center'
      },
      tooltip: {
        x: {
        format: 'MMM yyyy'
        }
      },
      legend: {
        show: true,
        showForSingleSeries: true,
        showForNullSeries: true,
        showForZeroSeries: true,
        position: 'right',
        onItemClick: {
          toggleDataSeries: true
        },
        width: 100,
        offsetY: 250,
        formatter: function(seriesName, opts) {
          return seriesName + '<br><br>';
        }
      },
      series: newSeries,
      yaxis: [{
        axisTicks: {
          show: true
        },
        axisBorder: {
          show: true,
        },
        forceNiceScale: true,
        title: {
          text: 'Commission',
        },
        show: true,
        decimalsInFloat: 0,
        labels: {
          formatter: function(val) {
            return '£' + val.toLocaleString();
          }
        }
      }],
      xaxis: {
        type: 'category',
        categories: newCategories
      }
    };

    var chart = new ApexCharts(document.querySelector('#commissionChart'), chartOptions);
    chart.render();
    
    updateDataGrid(newSeries, newCategories);
  }
  
function updateDataGrid(newSeries, newCategories) {
    var datagridDiv = document.getElementById('commissionDatagrid');
    var datagridDivWidth = datagridDiv.clientWidth;
    var monthWidth = Math.floor(datagridDivWidth/10);
    var dataWidth = Math.floor((datagridDivWidth - monthWidth)/3.1)-1;

    clearElement(datagridDiv);

    var categoryLength = newCategories.length;
    var displayData = [];

    for(var i = 0; i < categoryLength; i++) {
      var seriesLatestforecastcommission = 0;
      var seriesInvoicedcommission = 0;
      var seriesDifference = 0;

      if(newSeries[0]["data"][i]) {
        seriesLatestforecastcommission = newSeries[0]["data"][i].toLocaleString();
        seriesInvoicedcommission = newSeries[1]["data"][i].toLocaleString();
        seriesDifference = newSeries[2]["data"][i].toLocaleString();
      }

      var row = {
        month: newCategories[i], 
        latestforecastcommission:seriesLatestforecastcommission,
        invoicedcommission:seriesInvoicedcommission,
        difference:seriesDifference
      }
      displayData.push(row);
    }

	jexcel(document.getElementById('commissionDatagrid'), {
    pagination:12,
    allowInsertRow: false,
    allowManualInsertRow: false,
    allowInsertColumn: false,
    allowManualInsertColumn: false,
    allowDeleteRow: false,
    allowDeleteColumn: false,
    allowRenameColumn: false,
    wordWrap: true,
		data: displayData,
		columns: [
			{type:'text', width:monthWidth, name:'month', title:'Month', readOnly: true},
			{type:'text', width:dataWidth, name:'latestforecastcommission', title:'Latest Forecast Commission', readOnly: true},
			{type:'text', width:dataWidth, name:'invoicedcommission', title:'Invoiced Commission', readOnly: true},
			{type:'text', width:dataWidth, name:'difference', title:'Difference', readOnly: true}
		 ]
	  }); 
  }
  
  function getCheckedCheckBoxes(inputs) {
    var checkBoxes = [];
    var inputLength = inputs.length;
  
    for(var i = 0; i < inputLength; i++) {
      var input = inputs[i];
      if(input.type.toLowerCase() == 'checkbox') {
        if(input.checked) {
          if(input.attributes['Branch'].nodeValue == 'Meter') {
            if(!checkBoxes.includes(input)) {
              checkBoxes.push(input);
            }          
          }
  
          if(input.attributes['Branch'].nodeValue == 'Customer') {
            var linkedCustomer = input.attributes['LinkedCustomer'].nodeValue;
  
            for(var j = 0; j< inputLength; j++) {
              var meterInput = inputs[j];
  
              if(meterInput.type.toLowerCase() == 'checkbox'
              && meterInput.attributes['Branch'].nodeValue == 'Meter'
              && meterInput.attributes['LinkedCustomer'].nodeValue == linkedCustomer) {
                if(!checkBoxes.includes(meterInput)) {
                  checkBoxes.push(meterInput);
                } 
              }
            }
          }
  
          if(input.attributes['Branch'].nodeValue == 'Site') {
            var linkedSite = input.attributes['LinkedSite'].nodeValue;
  
            for(var j = 0; j< inputLength; j++) {
              var meterInput = inputs[j];
  
              if(meterInput.type.toLowerCase() == 'checkbox'
              && meterInput.attributes['Branch'].nodeValue == 'Meter'
              && meterInput.attributes['LinkedSite'].nodeValue == linkedSite) {
                if(!checkBoxes.includes(meterInput)) {
                  checkBoxes.push(meterInput);
                } 
              }
            }
          }
        }
      }
    }
  
    if(checkBoxes.length == 0) {
      for(var i = 0; i < inputLength; i++) {
        var input = inputs[i];
        if(input.type.toLowerCase() == 'checkbox') {
          if(input.attributes['Branch'].nodeValue == 'Meter') {
            checkBoxes.push(input);
          }
        }
      }
    }
  
    return checkBoxes;
  }
  
  function getNewChartSeries(checkBoxes, showBy, newCategories, commodity) {
    var meters = [];
    var newSeries = [];
    var checkBoxesLength = checkBoxes.length;
  
    for(var checkboxCount = 0; checkboxCount < checkBoxesLength; checkboxCount++) {
      var linkedSite = checkBoxes[checkboxCount].attributes['LinkedSite'].nodeValue;
      var linkedCustomer = checkBoxes[checkboxCount].attributes['LinkedCustomer'].nodeValue;
      var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
  
      meters.push(getMetersByAttribute('Identifier', span.innerHTML, linkedSite, linkedCustomer));
    }
  
    var series = getSeries();
  
    for(var i = 0; i < series.length; i++) {
      newSeries.push(summedMeterSeries(meters, series[i], showBy, newCategories, commodity));
    }
  
    return newSeries;
  }
  
  function getSeries() {
    return ["Latest Forecasted Commission","Invoiced Commission", "Difference"];
  }
  
  function getMetersByAttribute(attribute, value, linkedSite, linkedCustomer) {
    var meters = [];
    var dataLength = data.length;
  
    for(var siteCount = 0; siteCount < dataLength; siteCount++) {
      var site = data[siteCount];
      var customerLength = site.Customers.length;
  
      for(var i = 0; i < customerLength; i++) {
        var customer = site.Customers[i];
        var meterLength = customer.Meters.length;
  
        for(var meterCount = 0; meterCount < meterLength; meterCount++) {
          var meter = customer.Meters[meterCount];
  
          if(getAttribute(meter.Attributes, attribute) == value) {
            if(linkedSiteMatch(meter.GUID, 'Meter', 'LinkedSite', linkedSite)
            && linkedSiteMatch(meter.GUID, 'Meter', 'LinkedCustomer', linkedCustomer)) {
              meters.push(meter);
            }
          }
        }
      }
    }
  
    return meters[0];
  }
  
  function linkedSiteMatch(identifier, meterType, attribute, linkedSite) {
    var identifierCheckbox = document.getElementById(meterType.concat(identifier).concat('checkbox'));
    var identifierLinkedSite = identifierCheckbox.attributes[attribute].nodeValue;
  
    return identifierLinkedSite == linkedSite;
  }
  
  function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity) {
    var meterLength = meters.length;
    var summedMeterSeries = {
      name: seriesName,
      data: [0]
    };
  
    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = meters[meterCount];
  
      if(commodityMeterMatch(meter, commodity)) {
        var meterData = meter[showBy];
        
        if(!meterData) {
          continue;
        }
  
        var meterDataLength = meterData.length;
        for(var j = 0; j < meterDataLength; j++) {
          var i = newCategories.findIndex(n => n == meterData[j].Month);
          var value = meterData[j][seriesName];
  
          if(!value && !summedMeterSeries.data[i]){
            summedMeterSeries.data[i] = null;
          }
          else if(value && !summedMeterSeries.data[i]) {
            summedMeterSeries.data[i] = preciseRound(value,2);
          }
          else if(value && summedMeterSeries.data[i]) {
            summedMeterSeries.data[i] += preciseRound(value,2);
          }							     
        }
      }
    }
  
    return summedMeterSeries;
  }
  
  function getNewCategories() {
    return getCategoryTexts(new Date(2018, 12, 1), new Date(2019, 12, 1), 'MMM yyyy');
  }
  
  function getCategoryTexts(startDate, endDate, dateFormat) {
    var newCategories = [];
  
    for(var newDate = startDate; newDate < endDate; newDate.setDate(newDate.getDate() + 1)) {
      for(var hh = 0; hh < 48; hh++) {
        var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), dateFormat);
  
        if(!newCategories.includes(newCategoryText)) {
          newCategories.push(newCategoryText);
        }      
      }
    }
  
    return newCategories;
  }
  
  function getChartXAxisLabelFormat() {
    return 'MMM yyyy';
  }
  
  function getChartType(chartType) {
    switch(chartType){
      case 'Line':
      case 'Bar':
      case 'Area':
        return chartType.toLowerCase();
      case 'Stacked Line':
      case 'Stacked Bar':
        return chartType.replace('Stacked ', '').toLowerCase();
    }
  }
  
  function getChartYAxisTitle(showBy, commodity) {
    switch(showBy) {
      case 'Energy':
        if(commodity == 'Gas') {
          return 'Energy (Thm)';
        }
        return 'Energy (MWh)';
      case 'Power':
        return 'Power (MW)';
      case 'Current':
        return 'Current (A)';
      case 'Cost':
        return 'Cost (£)';
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
  
  updateClassOnClick('treeDivSelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('commoditySelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('commissionsChart', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
}

function formatDate(dateToBeFormatted, format) {
	var baseDate = new Date(dateToBeFormatted);

	switch(format) {
		case 'yyyy-MM-dd':
			var aaaa = baseDate.getFullYear();
			var gg = baseDate.getDate();
			var mm = (baseDate.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return aaaa + '-' + mm + '-' + gg;
		case 'yyyy-MM-dd hh:mm:ss':
			var hours = baseDate.getHours()
			var minutes = baseDate.getMinutes()
			var seconds = baseDate.getSeconds();
		
			if (hours < 10) {
				hours = '0' + hours;
			}				
		
			if (minutes < 10) {
				minutes = '0' + minutes;
			}				
		
			if (seconds < 10) {
				seconds = '0' + seconds;
			}			
		
			return formatDate(baseDate, 'yyyy-MM-dd') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return baseDate.getFullYear();
		case 'yyyy-MM-dd to yyyy-MM-dd':
			var startDate = getMonday(baseDate);
			var endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + 6);

			return formatDate(startDate, 'yyyy-MM-dd') + ' to ' + formatDate(endDate, 'yyyy-MM-dd')
	}
}

function convertMonthIdToShortCode(monthId) {
	return convertMonthIdToFullText(monthId).slice(0, 3).toUpperCase();
}

function convertMonthIdToFullText(monthId) {
	switch(monthId) {
		case 1:
			return 'January';
		case 2:
			return 'February';
		case 3:
			return 'March';
		case 4:
			return 'April';
		case 5:
			return 'May';
		case 6:
			return 'June';
		case 7:
			return 'July';
		case 8:
			return 'August';
		case 9:
			return 'September';
		case 10:
			return 'October';
		case 11:
			return 'November';
		case 12:
			return 'December';
	}
}

function preciseRound(num, dec){
	if ((typeof num !== 'number') || (typeof dec !== 'number')) {
		return false; 
	}	

	var num_sign = num >= 0 ? 1 : -1;
		
	return Number((Math.round((num*Math.pow(10,dec))+(num_sign*0.0001))/Math.pow(10,dec)).toFixed(dec));
}