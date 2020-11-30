function pageLoad(isPageLoad) {
  createTree(data, "siteDiv", "updateDashboard", "Site");
  createTree(dashboard, "dashboardDiv", "addDashboardItem", "Dashboard");

  addExpanderOnClickEvents();
  if(isPageLoad) {
    document.getElementById('electricityCommoditycheckbox').checked = true;
    document.getElementById('gasCommoditycheckbox').checked = true;
    document.getElementById('sitesLocationcheckbox').checked = true;
    document.getElementById('metersLocationcheckbox').checked = false;
    setOpenExpanders();
  }
}

function loadDatagrid(checkBoxes) {
  clearElement(document.getElementById('spreadsheet'));

  var displayData = [];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var guid = checkBoxes[i].getAttribute("guid");
    var linkedSite = checkBoxes[i].attributes['LinkedSite'].nodeValue;
    var branch = checkBoxes[i].getAttribute('Branch');
    var row = null;

    if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
      if(branch == 'Meter') {
        row = getDatagridRow(guid, linkedSite);
        displayData.push(row);
      }
    }
    else if(metersLocationcheckbox.checked) {
      row = getDatagridRow(guid, linkedSite);
      displayData.push(row);
    }
    else {
      for(var j= 0; j < data.length; j++) {
        if(data[j]["GUID"] == guid) {
          var meters = data[j].Meters;

          if(meters) {
            for(var k = 0; k < meters.length; k++) {
              row = getDatagridRow(meters[k]["GUID"], linkedSite);
              displayData.push(row);
            }
          }

          break;
        }
      }
    }
  }

  var displayColumns = [
    {type:'text', width:'215px', name:'address', title:'Address', readOnly: true},
    {type:'text', width:'140px', name:'meterpoint', title:'Meter Point', readOnly: true},
    {type:'text', width:'125px', name:'annualvolume', title:'Annual Volume<br>(kWh)', readOnly: true},
    {type:'text', width:'125px', name:'annualcost', title:'Annual Cost<br>(£)', readOnly: true},
    {type:'text', width:'115px', name:'carbon', title:'Carbon<br>(tonnes)', readOnly: true},
  ];

  jexcel(document.getElementById('spreadsheet'), {
    pagination:6,
    allowInsertRow: false,
    allowManualInsertRow: false,
    allowInsertColumn: false,
    allowManualInsertColumn: false,
    allowDeleteRow: false,
    allowDeleteColumn: false,
    allowRenameColumn: false,
    wordWrap: true,
    data: displayData,
    columns: displayColumns
  }); 
}

function getDatagridRow(meterGuid, linkedSite) {
  var meter = getMeterByGUID(meterGuid);
  var siteAttributes = getSiteAttributesByName(linkedSite);
  var siteAddress = getAttribute(siteAttributes, 'GoogleAddress');
  var meterPointIdentifier = getAttribute(meter.Attributes, 'Identifier');
  var meterPointAnnualVolume = getAttribute(meter.Attributes, 'AnnualVolume');
  var meterPointAnnualCost = getAttribute(meter.Attributes, 'AnnualCost');
  var meterPointCarbon = getAttribute(meter.Attributes, 'Carbon');

  return {address:siteAddress,	meterpoint:meterPointIdentifier.toLocaleString(), annualvolume:meterPointAnnualVolume.toLocaleString(), annualcost:meterPointAnnualCost.toLocaleString(), carbon:meterPointCarbon.toLocaleString()};
}

function loadMap(checkBoxes) {
  clearElement(document.getElementById('map-canvas'));
    var mapOptions = {
      zoom: 5,
      center: new google.maps.LatLng(55, -5),
      mapTypeId: google.maps.MapTypeId.TERRAIN
    }
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    var addresses = [];
    var positions = [];

    var checkBoxLength = checkBoxes.length;
    for(var i = 0; i < checkBoxLength; i++) {
      if(!checkBoxes[i].attributes['LinkedSite']) {
        continue;
      }

      var linkedSite = checkBoxes[i].attributes['LinkedSite'].nodeValue;
      var siteAttributes = getSiteAttributesByName(linkedSite);

      var address = getAttribute(siteAttributes, 'GoogleAddress');
      if(address && !addresses.includes(address)) {
        addresses.push(address);

        var latitude = getAttribute(siteAttributes, 'lat');
        var longitude = getAttribute(siteAttributes, 'lng');
        var latLng = {lat: latitude, lng: longitude};

        positions.push(latLng);
      }
    }
    
    var addressLength = addresses.length;
    for(var i = 0; i < addressLength; i++) {

      var marker = new google.maps.Marker({
          map: map,
          position: positions[i],
          title: addresses[i]
      });
    }    
}

function getSiteAttributesByName(siteName) {
  var dataLength = data.length;
  for(var i = 0; i < dataLength; i++) {
    var base = data[i];
    var baseName = getAttribute(base.Attributes, 'BaseName');

    if(baseName == siteName) {
      return base.Attributes;
    }
  }
}

function getMeterByGUID(guid) {
	var dataLength = data.length;
	for(var i = 0; i < dataLength; i++) {
		var site = data[i];
    var meterLength = site.Meters.length;
    for(var j = 0; j < meterLength; j++) {
      var meter = site.Meters[j];
      if(meter.GUID == guid) {
        return meter;
      }
    }
	}
	
	return null;
}

function createTree(baseData, divId, checkboxFunction, dataName) {
  var div = document.getElementById(divId);
  clearElement(div);

  var tree = document.createElement('div');
  tree.classList.add('scrolling-wrapper');
  tree.classList.add('expander-container');
  
  var headerDiv = createHeaderDiv(divId.concat('Header'), 
    dataName != "Dashboard" ? "Location" : "Add Dashboard Items", 
    true, true,
    dataName != "Dashboard" ? "Location" : "Add Dashboard Items");
  var ul = createBranchUl(divId.concat('Selector'), false, true);

  tree.appendChild(ul);

  buildTree(baseData, ul, checkboxFunction, dataName);

  div.appendChild(headerDiv);
  div.appendChild(tree);

  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';
  breakDisplayListItem.classList.add('format-listitem');
  ul.appendChild(breakDisplayListItem);

  var selectButtonsListItem = document.createElement('li');
  selectButtonsListItem.classList.add('format-listitem');
  selectButtonsListItem.classList.add('listItemWithoutPadding');
  selectButtonsListItem.innerHTML = '<div>'
    +'<button id="selectAll' + divId + '" style="width: 45%; float: left;" onclick="' + camelize('select ' + divId.replace('Div', '')) + '(true)">Select All</button>'
    +'<button id="deselectAll' + divId + '" style="width: 45%; float: right;" onclick="' + camelize('select ' + divId.replace('Div', '')) + '(false)">Deselect All</button>'
    +'</div>'
    +'<div style="clear: both;"></div>';
  ul.appendChild(selectButtonsListItem);

  if(dataName != "Dashboard") {
    var inputs = ul.getElementsByTagName('input');
    [...inputs].forEach(input => {
      if(input.type == 'checkbox') {
        input.checked = true;
      }
    });

    updateDashboard();

    var recurseSelectionListItem = document.createElement('li');
    recurseSelectionListItem.classList.add('format-listitem');
    recurseSelectionListItem.classList.add('listItemWithoutPadding');

    var recurseSelectionCheckbox = createBranchCheckbox('recurse' + divId + 'Selection', '', 'recurseSelection', 'checkbox', 'recurseSelection', false);
    var recurseSelectionSpan = createBranchSpan('recurse' + divId + 'SelectionSpan', 'Recurse Selection?');
    recurseSelectionListItem.appendChild(recurseSelectionCheckbox);
    recurseSelectionListItem.appendChild(recurseSelectionSpan);

    ul.appendChild(breakDisplayListItem.cloneNode(true));
    ul.appendChild(recurseSelectionListItem);
  }
}

function selectSite(checked) {
  var inputs = siteDiv.getElementsByTagName('input');
  [...inputs].forEach(input => {
    if(input.type == 'checkbox') {
      input.checked = checked;
    }
  });
  updateDashboard();
}

function selectDashboard(checked) {
  var inputs = dashboardDiv.getElementsByTagName('input');
  [...inputs].forEach(input => {
    if(input.type == 'checkbox') {
      input.checked = checked;
      addDashboardItem(input)
    }
  });
}

function camelize(str) {
  return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function(word, index) {
    return index === 0 ? word.toLowerCase() : word.toUpperCase();
  }).replace(/\s+/g, '');
}

function buildTree(baseData, baseElement, checkboxFunction, dataName) {
  var dataLength = baseData.length;
  for(var i = 0; i < dataLength; i++){
      var base = baseData[i];
      var baseName = getAttribute(base.Attributes, 'BaseName');
      var li = document.createElement('li');
      var ul = createUL();

      if(dataName == "Dashboard") {
        buildDashboardHierarchy(base.Charts, ul, commodity, checkboxFunction, baseName);
        appendListItemChildren(li, 'Dashboard'.concat(base.GUID), '', 'Dashboard', baseName, commodity, ul, baseName, base.GUID, true, base.GUID == '0' || base.GUID == '1');
      }
      else {
        var commodity = 'None';
        if (document.getElementById('electricityCommoditycheckbox').checked
          && document.getElementById('gasCommoditycheckbox').checked) {
          commodity = '';
        }
        else if (document.getElementById('electricityCommoditycheckbox').checked) {
          commodity = 'Electricity';
        }
        else if (document.getElementById('gasCommoditycheckbox').checked) {
          commodity = 'Gas';
        }

        if(!commoditySiteMatch(base, commodity)) {
          continue;
        }

        if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
          if(base.hasOwnProperty('Meters')) {
            buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName);
            appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, true);
          }
        }
        else if(sitesLocationcheckbox.checked) {
          appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, false);
        }
        else if(metersLocationcheckbox.checked) {
          buildIdentifierHierarchy(base.Meters, baseElement, commodity, checkboxFunction, baseName);
        }
      }

      baseElement.appendChild(li);        
  }
}

function buildDashboardHierarchy(charts, baseElement, commodity, checkboxFunction, linkedSite) {
  var chartsLength = charts.length;
  for(var i = 0; i < chartsLength; i++){
      var chart = charts[i];
      var chartAttributes = chart.Attributes;
      var identifier = getAttribute(chartAttributes, 'BaseName');
      var li = document.createElement('li');
      var branchId = 'Chart'.concat(chart.GUID);
      var branchDiv = createBranchDiv(branchId, false);

      li.appendChild(branchDiv);
      li.appendChild(createCheckbox(branchId, checkboxFunction, 'Chart', linkedSite, chart.GUID));
      li.appendChild(createTreeIcon('Chart', commodity));
      li.appendChild(createSpan(branchId, identifier));  

      baseElement.appendChild(li); 
  }
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite) {
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
      li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID));
      li.appendChild(createTreeIcon(deviceType, meterCommodity));
      li.appendChild(createSpan(branchId, identifier));  

      baseElement.appendChild(li); 
  }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, childrenCreated, isOpen = false) {
  li.appendChild(createBranchDiv(id, childrenCreated, isOpen));

  if(checkboxFunction != '') {
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
  }
  
  li.appendChild(createTreeIcon(branchOption, commodity));
  li.appendChild(createSpan(id, branchOption));
  li.appendChild(createBranchListDiv(id.concat('List'), ul, isOpen));
}

function createBranchListDiv(branchListDivId, ul, isOpen = false) {
  var branchListDiv = document.createElement('div');
  branchListDiv.id = branchListDivId;

  if(!isOpen) {
    branchListDiv.setAttribute('class', 'listitem-hidden');
  }
  
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
    case 'Chart':
    case 'Flexible Purchasing':
    case 'Monthly Usage':
    case 'Monthly Cost':
    case 'Monthly Rate':
    case 'Monthly Savings':
      return 'fas fa-chart-bar';
    default:
      return 'fas fa-map-marker-alt';
  }
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
    checkBox.setAttribute('class', 'show-pointer');

    if(checkBox.id == 'Chart11checkbox' || checkBox.id == 'Chart00checkbox') {
      checkBox.setAttribute('checked', true);
    }

    functionArguments.push(checkBox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
          var argument = functionArray[1].split(',')[i];

          if(argument != '') {
            functionArguments.push(argument);
          }
        }
    }

    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    functionName = "recurseSelection(" + checkBox.id + ", 'recursesiteDivSelectioncheckbox');" + functionName;
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function addCustomDashboardItems(checkBoxes) {
  loadUsageChart(checkBoxes);
  loadElectricityPriceChart();
  loadElectricityUsageChart();
  loadGasPriceChart();
  loadGasUsageChart();
}

function loadUsageChart(checkBoxes) {
  var electricityCategories = [
    'JAN-20', 'FEB-20', 'MAR-20', 'APR-20', 'MAY-20', 'JUN-20', 'JUL-20', 'AUG-20', 'SEP-20', 'OCT-20', 'NOV-20', 'DEC-20',
    'JAN-21', 'FEB-21', 'MAR-21', 'APR-21', 'MAY-21', 'JUN-21', 'JUL-21', 'AUG-21', 'SEP-21', 'OCT-21', 'NOV-21', 'DEC-21',
    'JAN-22', 'FEB-22', 'MAR-22', 'APR-22', 'MAY-22', 'JUN-22', 'JUL-22', 'AUG-22', 'SEP-22', 'OCT-22', 'NOV-22', 'DEC-22',
    'JAN-23', 'FEB-23', 'MAR-23', 'APR-23', 'MAY-23', 'JUN-23', 'JUL-23', 'AUG-23', 'SEP-23', 'OCT-23', 'NOV-23', 'DEC-23',
    'JAN-24', 'FEB-24', 'MAR-24', 'APR-24', 'MAY-24', 'JUN-24', 'JUL-24', 'AUG-24', 'SEP-24'
    ];
  var electricityCategoriesLength = electricityCategories.length;

  var forecastVolume = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var guid = checkBoxes[i].getAttribute("guid");
    var branch = checkBoxes[i].getAttribute('Branch');

    if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
      if(branch == 'Meter') {
        var meter = getMeterByGUID(guid);
        var meterUsage = getAttribute(meter.Attributes, "MonthlyUsage");

        for(var j = 0; j < electricityCategoriesLength; j++) {
          forecastVolume[j] += meterUsage[j][electricityCategories[j]];
        }
      }
    }
    else if(metersLocationcheckbox.checked) {
      var meter = getMeterByGUID(guid);
      var meterUsage = getAttribute(meter.Attributes, "MonthlyUsage");

      for(var j = 0; j < electricityCategoriesLength; j++) {
        forecastVolume[j] += meterUsage[j][electricityCategories[j]];
      }
    }
    else {
      for(var j= 0; j < data.length; j++) {
        if(data[j]["GUID"] == guid) {
          var meters = data[j].Meters;

          if(meters) {
            for(var k = 0; k < meters.length; k++) {
              var meter = getMeterByGUID(meters[k]["GUID"]);
              var meterUsage = getAttribute(meter.Attributes, "MonthlyUsage");

              for(var j = 0; j < electricityCategoriesLength; j++) {
                forecastVolume[j] += meterUsage[j][electricityCategories[j]];
              }
            }
          }

          break;
        }
      }
    }
  }

  electricityCategories = [
    'JAN-20', '', '', 'APR-20', '', '', 'JUL-20', '', '', 'OCT-20', '', '',
    'JAN-21', '', '', 'APR-21', '', '', 'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', '', 'OCT-23', '', '',
    'JAN-24', '', '', 'APR-24', '', '', 'JUL-24', '', ''
    ];

  var electricityVolumeSeries = [{
    name: 'Forecast Usage (kWh)',
    data: forecastVolume
    }];  

  var electricityVolumeOptions = {
      chart: {
        type: 'bar',
      },
      title: {
        text: 'Forecast Portfolio Usage',
      },
      tooltip: {
          x: {
          format: getChartTooltipXFormat("Yearly")
          }
      },
      xaxis: {
          title: {
          text: ''
          },
          labels: {
            rotate: -45,
            rotateAlways: true,
            hideOverlappingLabels: true,
            style: {
              fontSize: '9px',
              fontFamily: 'Helvetica, Arial, sans-serif',
              fontWeight: 400,
            },
          },
          categories: electricityCategories
      },
      yaxis: [{
        title: {
          style: {
            fontSize: '10px',
            fontFamily: 'Helvetica, Arial, sans-serif',
            fontWeight: 400,
          },
          text: 'kWh'
        },
        forceNiceScale: true,
        labels: {
          formatter: function(val) {
            return val.toLocaleString();
          }
        }
      }]
    };

  refreshChart(electricityVolumeSeries, "#totalUsageChart", electricityVolumeOptions);
}

function loadElectricityPriceChart() {
  var electricityPriceSeries = [{
      name: 'Cap Price',
      type: 'line',
      data: [
                5.718,5.718,5.718,5.718,5.718,5.718,
                4.888,4.888,4.888,4.888,4.888,4.888,
                5.536,5.536,5.536,5.536,5.536,5.536,
                4.905,4.905,4.905,4.905,4.905,4.905,
                5.558,5.558,5.558,5.558,5.558,5.558,
                4.907,4.907,4.907,4.907,4.907,4.907
              ]
    }, {
      name: 'ECP',
      type: 'line',
      data: [
                4.990,4.990,4.990,4.990,4.990,4.990,
                4.310,4.310,4.310,4.310,4.310,4.310,
                5.030,5.030,5.030,5.030,5.030,5.030,
                4.340,4.340,4.340,4.340,4.340,4.340,
                5.010,5.010,5.010,5.010,5.010,5.010,
                4.350,4.350,4.350,4.350,4.350,4.350,
            ]
    }, {
      name: 'Market',
      type: 'line',
      data: [
                4.886,4.886,4.886,4.886,4.886,4.886,
                4.270,4.270,4.270,4.270,4.270,4.270,
                4.990,4.990,4.990,4.990,4.990,4.990,
                4.296,4.296,4.296,4.296,4.296,4.296,
                4.973,4.973,4.973,4.973,4.973,4.973,
                4.301,4.301,4.301,4.301,4.301,4.301,
            ]
    }, {
      name: 'Day1Price',
      type: 'line',
      data: [
                5.918,5.918,5.918,5.918,5.918,5.918,
                4.444,4.444,4.444,4.444,4.444,4.444,
                5.033,5.033,5.033,5.033,5.033,5.033,
                4.459,4.459,4.459,4.459,4.459,4.459,
                5.053,5.053,5.053,5.053,5.053,5.053,
                4.461,4.461,4.461,4.461,4.461,4.461,
            ]
    }];

  var electricityCategories = [
    'OCT-20', '', '',
    'JAN-21', '', '', 'APR-21', '', '', 'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
    ];
  
  var electricityPriceOptions = {
    chart: {
      type: 'line',
      stacked: false
    },
    title: {
      text: 'Flex Electricity Price',
    },
    tooltip: {
        x: {
        format: 'dd/MM/yyyy'
        }
    },
    xaxis: {
        title: {
        text: ''
        },
        labels: {
          rotate: -45,
          rotateAlways: true,
          hideOverlappingLabels: true,
          style: {
            fontSize: '9px',
            fontFamily: 'Helvetica, Arial, sans-serif',
            fontWeight: 400,
          },
        },
        categories: electricityCategories
    },
    yaxis: [{
      title: {
        style: {
          fontSize: '10px',
          fontFamily: 'Helvetica, Arial, sans-serif',
          fontWeight: 400,
        },
        text: 'p/kWh'
      },
      forceNiceScale: true,
      labels: {
        formatter: function(val) {
          return preciseRound(val, 2).toLocaleString();
        }
      }
    }]
  };

  refreshChart(electricityPriceSeries, "#electricityPriceChart", electricityPriceOptions);
}

function loadElectricityUsageChart() {
  var electricityUsageSeries = [{
    name: 'Open Vol',
    data: [
                1.324, 1.324, 1.324, 1.324, 1.324, 1.324, 
              0.713, 0.713, 0.713, 0.713, 0.713, 0.713, 
              1.323, 1.323, 1.323, 1.323, 1.323, 1.323,
              0.711, 0.711, 0.711, 0.711, 0.711, 0.711,
              0.934, 0.934, 0.934, 0.934, 0.934, 0.934,
              0.711, 0.711, 0.711, 0.711, 0.711, 0.711
          ]
  }, {
    name: 'Hedge Vol',
    data: [
                1.426, 1.426, 1.426, 1.426, 1.426, 1.426, 
                1.657, 1.657, 1.657, 1.657, 1.657, 1.657, 
                1.417, 1.417, 1.417, 1.417, 1.417, 1.417,
                1.659, 1.659, 1.659, 1.659, 1.659, 1.659,
                1.806, 1.806, 1.806, 1.806, 1.806, 1.806,
                1.659, 1.659, 1.659, 1.659, 1.659, 1.659
          ]
  }];
  var electricityCategories = [
    'OCT-20', '', '',
    'JAN-21', '', '', 'APR-21', '', '', 'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
    ];
  var electricityUsageOptions = {
      chart: {
        type: 'bar',
        stacked: true
      },
      title: {
        text: 'Flex Electricity Usage',
      },
      tooltip: {
          x: {
          format: 'dd/MM/yyyy'
          }
      },
      xaxis: {
          title: {
          text: ''
          },
          labels: {
            rotate: -45,
            rotateAlways: true,
            hideOverlappingLabels: true,
            style: {
              fontSize: '9px',
              fontFamily: 'Helvetica, Arial, sans-serif',
              fontWeight: 400,
            },
          },
          categories: electricityCategories
      },
      yaxis: [{
        title: {
          style: {
            fontSize: '10px',
            fontFamily: 'Helvetica, Arial, sans-serif',
            fontWeight: 400,
          },
          text: 'MW'
        },
        forceNiceScale: true,
        labels: {
          formatter: function(val) {
            return val.toFixed(3);
          }
        }
      }]
    };
  refreshChart(electricityUsageSeries, "#electricityUsageChart", electricityUsageOptions);
}

function loadGasPriceChart() {
  var gasPriceSeries = [{
            name: 'Cap Price',
            data: [
                      1.505846706,1.517857448,1.568527763,1.692013198,1.800109871,1.88943976,2.022683923,1.986276363,1.824882025,1.601181966,1.532870874,1.512227412,1.528742182,1.565900413,1.630082813,1.786973123,1.880431704,1.930726684,1.952496153,1.901825837,1.790351144,1.589171225,1.532495539,1.51072607,1.517857448,1.558769035,1.673997086,
                    ]
          }, {
            name: 'ECP',
            data: [
                      1.3,1.31,1.33,1.54,1.59,1.66,1.74,1.73,1.64,1.43,1.35,1.35,1.37,1.39,1.42,1.63,1.66,1.71,1.74,1.71,1.64,1.44,1.35,1.35,1.36,1.38,1.45,
                  ]
          }, {
            name: 'Market',
            data: [
                      1.233148281,1.24747928,1.301732345,1.439924114,1.547065387,1.631004091,1.753158791,1.728250151,1.595176595,1.390106834,1.328347056,1.314016058,1.334147698,1.368610337,1.428664044,1.578457097,1.66307823,1.708800939,1.727226508,1.679797728,1.574703741,1.388741977,1.336877412,1.318793057,1.326982199,1.363492123,1.464832754,
                  ]
          }, {
            name: 'Day1Price',
            data: [
                      1.368951551,1.379870407,1.42593433,1.538193817,1.636463519,1.717672509,1.838803566,1.805705785,1.658983659,1.455619969,1.393518977,1.374752193,1.38976562,1.42354583,1.481893466,1.624521021,1.709483368,1.755206076,1.774996503,1.728932579,1.627591949,1.444701113,1.393177762,1.373387336,1.379870407,1.41706276,1.521815533,
                  ]
          }];
  var gasCategories = [
          'JUL-21', '', '', 'OCT-21', '', '',
          'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
          'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
          ];
  var gasPriceOptions = {
          chart: {
            type: 'line',
            stacked: false
          },
          title: {
            text: 'Flex Gas Price',
          },
          tooltip: {
              x: {
              format: 'dd/MM/yyyy'
              }
          },
          xaxis: {
              title: {
              text: ''
              },
              labels: {
                rotate: -45,
                rotateAlways: true,
                hideOverlappingLabels: true,
                style: {
                  fontSize: '9px',
                  fontFamily: 'Helvetica, Arial, sans-serif',
                  fontWeight: 400,
                },
              },
              categories: gasCategories
          },
          yaxis: [{
            title: {
              style: {
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
              text: 'p/kWh'
            },
            forceNiceScale: true,
            labels: {
              formatter: function(val) {
                return preciseRound(val, 2).toLocaleString();
              }
            }
          }]
        };
  refreshChart(gasPriceSeries, "#gasPriceChart", gasPriceOptions);
}

function loadGasUsageChart() {
  var gasUsageSeries = [{
        name: 'Open Vol',
        data: [
                    300,300,300,1000,1650,1900,2150,1900,1800,1200,300,300,300,300,300,1000,1650,1900,2150,1900,1800,1200,300,300,300,300,300
              ]
      }, {
        name: 'Hedge Vol',
        data: [
                    456,434,664,1036,1631,2032,2180,1949,1790,1232,1192,579,448,442,663,1047,1638,2007,2198,1953,1786,1218,1202,579,448,445,653
              ]
      }];
  var gasCategories = [
    'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
    ];
  var gasUsageOptions = {
        chart: {
            type: 'bar',
          stacked: true
        },
        title: {
          text: 'Flex Gas Usage'
        },
        tooltip: {
            x: {
            format: 'dd/MM/yyyy'
            }
        },
        xaxis: {
            title: {
            text: ''
            },
            labels: {
              rotate: -45,
              rotateAlways: true,
              hideOverlappingLabels: true,
              style: {
                fontSize: '9px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
            },
            categories: gasCategories
        },
        yaxis: [{
          title: {
            style: {
              fontSize: '10px',
              fontFamily: 'Helvetica, Arial, sans-serif',
              fontWeight: 400,
            },
            text: 'th/day'
          },
          forceNiceScale: true,
          labels: {
            formatter: function(val) {
              return val.toLocaleString();
            }
          }
        }]
      };
  refreshChart(gasUsageSeries, "#gasUsageChart", gasUsageOptions);
}

function addDashboardItem(checkbox) {
  var guid = checkbox.getAttribute('guid');
  var dashboardItemId = 'customDashboardItem'.concat(guid);
  var dashboardItem = document.getElementById(dashboardItemId);

  if(!dashboardItem) {
    return;
  }

  if(checkbox.checked && dashboardItem.classList.contains('listitem-hidden')) {
    updateClassOnClick(dashboardItemId, 'listitem-hidden', '');
  }
  else if(!checkbox.checked && !dashboardItem.classList.contains('listitem-hidden')) {
    updateClassOnClick(dashboardItemId, 'listitem-hidden', '');
  }

  var items = ["customDashboardItem11", "customDashboardItem00", "customDashboardItem01", "customDashboardItem02", "customDashboardItem03"];
  for(var j = 0; j < 5; j++) {
    var visibleCount = 0;
  
    for(var i = 0; i < j; i++) {
      visibleCount += (hasClass(document.getElementById(items[i]), 'listitem-hidden') ? 0 : 1);
    }
  
    var margin = "margin-left: 0px";
    if(visibleCount == 1 || visibleCount == 3) {
      margin = "margin-left: 9px";
    }
    document.getElementById(items[i]).setAttribute('style', margin);
  }
}

function getChartTooltipXFormat(period) {
  switch(period) {
    case 'Daily':
    case "Weekly":
    case "Monthly":
      return 'dd/MM/yyyy HH:mm';
    case "Yearly":
      return 'dd/MM/yyyy';
  }
}

function getChartXAxisLabelFormat(period) {
  return 'MMM-YY';
}

function refreshChart(newSeries, chartId, chartOptions) {
  var options = {
    chart: {
        height: '100%',
        width: '100%',
      type: chartOptions.chart.type,
      stacked: chartOptions.chart.stacked,
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
    title: {
      text: chartOptions.title.text,
      align: 'center',
      style: {
        fontSize: '25px',
        fontFamily: 'Arial, Helvetica, sans-serif',
        fontWeight: 'normal',
      }
    },
    dataLabels: {
      enabled: false
    },
    tooltip: {
      x: {
        format: chartOptions.tooltip.x.format
      }
    },
    legend: {
      show: true,
      showForSingleSeries: false,
      showForNullSeries: true,
      showForZeroSeries: true,
      position: 'top',
      horizontalAlign: 'center', 
      onItemClick: {
        toggleDataSeries: true
      },
      formatter: function(seriesName) {
        return seriesName;
      }
    },
    colors: ['#69566c', '#61B82E', '#1CB89D', '#3C6B20', '#851B1E', '#C36265', '#104A6B', '#B8B537', '#B8252A', '#0B6B5B'],
    series: newSeries,
    yaxis: chartOptions.yaxis,
    xaxis: chartOptions.xaxis
  };  

  clearElement(document.getElementById(chartId.replace('#', '')));
  renderChart(chartId, options);
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

function updateDashboard(callingElement) {
  var checkBoxes = getCheckedElements(document.getElementById("siteDiv").getElementsByTagName('input'));

  loadMap(checkBoxes);
  loadDatagrid(checkBoxes);
  loadDashboardHeaderNumberOfSites(checkBoxes);
  loadDashboardHeaderPortfolioAnnualisedEnergy(checkBoxes);
  loadDashboardHeaderCarbon(checkBoxes);
  loadDashboardHeaderOpportunities(checkBoxes);

  addCustomDashboardItems(checkBoxes);
}

function loadDashboardHeaderNumberOfSites(checkBoxes) {
  var addresses = [];
  var meters = 0;
  var sites = 0;

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var linkedSite = checkBoxes[i].attributes['LinkedSite'].nodeValue;
    var siteAttributes = getSiteAttributesByName(linkedSite);

    var address = getAttribute(siteAttributes, 'GoogleAddress');
    if(address && !addresses.includes(address)) {
      addresses.push(address);
    }

    if(checkBoxes[i].attributes['Branch'].nodeValue == 'Meter') {
      meters++;
    }
    else if(checkBoxes[i].attributes['Branch'].nodeValue == 'Site') {
      sites++;
    }
  }

  var element = document.getElementById("dashboardHeaderNumberOfSites");
  var siteText = sites == 0 ? '' : (sites + ' Site' + (sites == 1 ? '' : 's') + (sites == 0 ? '' : '<br>'));
  var meterText = meters == 0 ? '' : (meters + ' Meter' + (meters == 1 ? '' : 's'));

  if(sites == 0 && meters == 0) {
    element.innerHTML = 'No Sites or Meters selected';
  }
  else {
    element.innerHTML = siteText + meterText;
  }
}

function loadDashboardHeaderPortfolioAnnualisedEnergy(checkBoxes) {
  var usage = 0;
  var cost = 0;

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var guid = checkBoxes[i].getAttribute("guid");
    var branch = checkBoxes[i].getAttribute('Branch');

    if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
      if(branch == 'Meter') {
        var meter = getMeterByGUID(guid);
        var meterUsage = getAttribute(meter.Attributes, "AnnualVolume");
        var meterCost = getAttribute(meter.Attributes, "AnnualCost");

        usage += meterUsage;
        cost += meterCost;
      }
    }
    else if(metersLocationcheckbox.checked) {
      var meter = getMeterByGUID(guid);
      var meterUsage = getAttribute(meter.Attributes, "AnnualVolume");
      var meterCost = getAttribute(meter.Attributes, "AnnualCost");

      usage += meterUsage;
      cost += meterCost;
    }
    else {
      for(var j= 0; j < data.length; j++) {
        if(data[j]["GUID"] == guid) {
          var meters = data[j].Meters;

          if(meters) {
            for(var k = 0; k < meters.length; k++) {
              var meter = getMeterByGUID(meters[k]["GUID"]);
              var meterUsage = getAttribute(meter.Attributes, "AnnualVolume");
              var meterCost = getAttribute(meter.Attributes, "AnnualCost");

              usage += meterUsage;
              cost += meterCost;
            }
          }

          break;
        }
      }
    }
  }

  var usageElement = document.getElementById("dashboardHeaderPortfolioAnnualisedEnergyUsage");
  usageElement.innerHTML = usage.toLocaleString().concat(" kWh");

  var costElement = document.getElementById("dashboardHeaderPortfolioAnnualisedEnergyCost");
  costElement.innerHTML = "£".concat(cost.toLocaleString());
}

function loadDashboardHeaderCarbon(checkBoxes) {
  var carbon = 0;

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var guid = checkBoxes[i].getAttribute("guid");
    var branch = checkBoxes[i].getAttribute('Branch');

    if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
      if(branch == 'Meter') {
        var meter = getMeterByGUID(guid);
        var meterCarbon = getAttribute(meter.Attributes, "Carbon");

        carbon += meterCarbon;
      }
    }
    else if(metersLocationcheckbox.checked) {
      var meter = getMeterByGUID(guid);
      var meterCarbon = getAttribute(meter.Attributes, "Carbon");

      carbon += meterCarbon;
    }
    else {
      for(var j= 0; j < data.length; j++) {
        if(data[j]["GUID"] == guid) {
          var meters = data[j].Meters;

          if(meters) {
            for(var k = 0; k < meters.length; k++) {
              var meter = getMeterByGUID(meters[k]["GUID"]);
              var meterCarbon = getAttribute(meter.Attributes, "Carbon");

              carbon += meterCarbon;
            }
          }

          break;
        }
      }
    }
  }

  var carbonElement = document.getElementById("dashboardHeaderCarbon");
  carbonElement.innerHTML = carbon.toLocaleString().concat(" tonnes p.a");
}

function loadDashboardHeaderOpportunities(checkBoxes) {
  var headers = ["Number","UsageSaving","CostSaving"];
  var pendingOpportunities = [0,0,0];
  var activeOpportunities = [0,0,0];
  var finishedOpportunities = [0,0,0];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    if(!checkBoxes[i].attributes['LinkedSite']) {
      continue;
    }

    var guid = checkBoxes[i].getAttribute("guid");
    var branch = checkBoxes[i].getAttribute('Branch');

    if(sitesLocationcheckbox.checked && metersLocationcheckbox.checked) {
      if(branch == 'Meter') {
        var meter = getMeterByGUID(guid);
        var meterPendingOpportunities = getAttribute(meter.Attributes, "PendingOpportunities");
        var meterActiveOpportunities = getAttribute(meter.Attributes, "ActiveOpportunities");
        var meterFinishedOpportunities = getAttribute(meter.Attributes, "FinishedOpportunities");

        for(var j = 0; j < 3; j++) {
          pendingOpportunities[j] += meterPendingOpportunities[j][headers[j]];
          activeOpportunities[j] += meterActiveOpportunities[j][headers[j]];
          finishedOpportunities[j] += meterFinishedOpportunities[j][headers[j]];
        }
      }
    }
    else if(metersLocationcheckbox.checked) {
      var meter = getMeterByGUID(guid);
      var meterPendingOpportunities = getAttribute(meter.Attributes, "PendingOpportunities");
      var meterActiveOpportunities = getAttribute(meter.Attributes, "ActiveOpportunities");
      var meterFinishedOpportunities = getAttribute(meter.Attributes, "FinishedOpportunities");

      for(var j = 0; j < 3; j++) {
        pendingOpportunities[j] += meterPendingOpportunities[j][headers[j]];
        activeOpportunities[j] += meterActiveOpportunities[j][headers[j]];
        finishedOpportunities[j] += meterFinishedOpportunities[j][headers[j]];
      }
    }
    else {
      for(var j= 0; j < data.length; j++) {
        if(data[j]["GUID"] == guid) {
          var meters = data[j].Meters;

          if(meters) {
            for(var k = 0; k < meters.length; k++) {
              var meter = getMeterByGUID(meters[k]["GUID"]);
              var meterPendingOpportunities = getAttribute(meter.Attributes, "PendingOpportunities");
              var meterActiveOpportunities = getAttribute(meter.Attributes, "ActiveOpportunities");
              var meterFinishedOpportunities = getAttribute(meter.Attributes, "FinishedOpportunities");

              for(var j = 0; j < 3; j++) {
                pendingOpportunities[j] += meterPendingOpportunities[j][headers[j]];
                activeOpportunities[j] += meterActiveOpportunities[j][headers[j]];
                finishedOpportunities[j] += meterFinishedOpportunities[j][headers[j]];
              }
            }
          }

          break;
        }
      }
    }
  }

  var activeOpportunitiesCountElement = document.getElementById("dashboardHeaderActiveOpportunitiesCount");
  activeOpportunitiesCountElement.innerHTML = activeOpportunities[0].toLocaleString().concat(' Active');

  var activeOpportunitiesCostElement = document.getElementById("dashboardHeaderActiveOpportunitiesCost");
  activeOpportunitiesCostElement.innerHTML = "£".concat(activeOpportunities[2].toLocaleString()).concat(' proj. savings');
}