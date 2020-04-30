function pageLoad() {
  createTree(data, "siteDiv", "updateDashboard", "Site");
  createTree(dashboard, "dashboardDiv", "addDashboardItem", "Dashboard");
  
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

function loadDatagrid(checkBoxes) {
  clearElement(document.getElementById('spreadsheet'));

  var displayData = [];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var guid = checkBoxes[i].getAttribute("guid");
    var meter = getMeterByGUID(guid);

    var linkedSite = checkBoxes[i].attributes['LinkedSite'].nodeValue;
    var siteAttributes = getSiteAttributesByName(linkedSite);
    var siteAddress = getAttribute(siteAttributes, 'GoogleAddress');
    var meterPointIdentifier = getAttribute(meter.Attributes, 'Identifier');
    var meterPointAnnualVolume = getAttribute(meter.Attributes, 'AnnualVolume');
    var meterPointAnnualCost = getAttribute(meter.Attributes, 'AnnualCost');
    var meterPointCarbon = getAttribute(meter.Attributes, 'Carbon');

    var row = {address:siteAddress,	meterpoint:meterPointIdentifier.toLocaleString(), annualvolume:meterPointAnnualVolume.toLocaleString(), annualcost:meterPointAnnualCost.toLocaleString(), carbon:meterPointCarbon.toLocaleString()};
    displayData.push(row);
  }

  jexcel(document.getElementById('spreadsheet'), {
    pagination:5,
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
        {type:'text', width:'150px', name:'address', title:'Address', readOnly: true},
        {type:'text', width:'130px', name:'meterpoint', title:'Meter Point', readOnly: true},
        {type:'text', width:'175px', name:'annualvolume', title:'Annual Volume (kWh)', readOnly: true},
        {type:'text', width:'175px', name:'annualcost', title:'Annual Cost (£)', readOnly: true},
        {type:'text', width:'175px', name:'carbon', title:'Carbon (tonnes)', readOnly: true},
     ]
  }); 
}

function loadMap(checkBoxes) {
  clearElement(document.getElementById('map-canvas'));
    var mapOptions = {
      zoom: 3.75,
      center: new google.maps.LatLng(56, -5),
      mapTypeId: google.maps.MapTypeId.SATELLITE
    }
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    var addresses = [];
    var positions = [];

    var checkBoxLength = checkBoxes.length;
    for(var i = 0; i < checkBoxLength; i++) {
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

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction, dataName) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var headerDiv = createHeaderDiv(divId.concat('Header'), dataName != "Dashboard" ? "Sites/Meters" : "Custom Dashboard Items", true);
    var ul = createBranchUl(divId.concat('Selector'), false, true);

    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction, dataName);

    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
    div.appendChild(tree);

    if(dataName != "Dashboard") {
      updateDashboard();
    }
}

function buildTree(baseData, baseElement, checkboxFunction, dataName) {
  var dataLength = baseData.length;
  for(var i = 0; i < dataLength; i++){
      var base = baseData[i];
      var baseName = getAttribute(base.Attributes, 'BaseName');
      var li = document.createElement('li');
      var ul = createUL();

      if(dataName == "Dashboard") {
        appendListItemChildren(li, 'Dashboard'.concat(base.GUID), checkboxFunction, 'Dashboard', baseName, '', '', baseName, base.GUID, false);
      }
      else {
        var commodity = '';
        if (document.getElementById('electricityCommodityradio').checked) {
          commodity = 'Electricity';
        }
        else if (document.getElementById('gasCommodityradio').checked) {
          commodity = 'Gas';
        }

        if(!commoditySiteMatch(base, commodity)) {
          continue;
        }

        var childrenCreated = false;
        if(base.hasOwnProperty('Meters')) {
          buildIdentifierHierarchy(base.Meters, ul, commodity, checkboxFunction, baseName, false);
          childrenCreated = true;
        }

        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID, childrenCreated);
      }

      baseElement.appendChild(li);        
  }
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, showSubMeters) {
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
      var hasSubMeters = meter.hasOwnProperty('SubMeters');
      var li = document.createElement('li');
      var branchId = 'Meter'.concat(meter.GUID);
      var branchDiv = createBranchDiv(branchId);
      
      if(!showSubMeters || !hasSubMeters) {
          branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer expander');
          branchDiv.setAttribute('class', 'far fa-times-circle');
      }

      li.appendChild(branchDiv);
      li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID));
      li.appendChild(createTreeIcon(deviceType, meterCommodity));
      li.appendChild(createSpan(branchId, identifier));  

      baseElement.appendChild(li); 
  }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, childrenCreated) {
  li.appendChild(createBranchDiv(id, childrenCreated));
  li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));

  if(checkboxBranch == 'Dashboard') {
    li.appendChild(createSpan(id, branchOption));
  }
  else {
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
  }    
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

    if(checkBox.id == 'Dashboard4checkbox') {
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
    '01-20', '02-20', '03-20', '04-20', '05-20', '06-20', '07-20', '08-20', '09-20', '10-20', '11-20', '12-20',
    '01-21', '02-21', '03-21', '04-21', '05-21', '06-21', '07-21', '08-21', '09-21', '10-21', '11-21', '12-21',
    '01-22', '02-22', '03-22', '04-22', '05-22', '06-22', '07-22', '08-22', '09-22', '10-22', '11-22', '12-22',
    '01-23', '02-23', '03-23', '04-23', '05-23', '06-23', '07-23', '08-23', '09-23', '10-23', '11-23', '12-23',
    '01-24', '02-24', '03-24', '04-24', '05-24', '06-24', '07-24', '08-24', '09-24'
    ];
  var electricityCategoriesLength = electricityCategories.length;

  var forecastVolume = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var guid = checkBoxes[i].getAttribute("guid");
    var meter = getMeterByGUID(guid);

    var meterUsage = getAttribute(meter.Attributes, "MonthlyUsage");

    for(var j = 0; j < electricityCategoriesLength; j++) {
      forecastVolume[j] += meterUsage[j][electricityCategories[j]];
    }
  }

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
        align: 'center'
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
          format: getChartXAxisLabelFormat('Weekly')
          },
          categories: electricityCategories
      },
      yaxis: [{
        title: {
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
    '10 2020', '11 2020', '12 2020',
    '01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
    '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
    '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
    ];
  
  var electricityPriceOptions = {
    chart: {
      type: 'line',
      stacked: false
    },
    title: {
      text: 'Flex Electricity Price',
      align: 'center'
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
        format: 'dd/MM/yyyy'
        },
        categories: electricityCategories
    },
    yaxis: [{
      title: {
        text: 'p/kWh'
      },
      forceNiceScale: true,
      labels: {
        formatter: function(val) {
          return val.toLocaleString();
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
    '10 2020', '11 2020', '12 2020',
    '01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
    '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
    '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
    ];
  var electricityUsageOptions = {
      chart: {
        type: 'bar',
        stacked: true
      },
      title: {
        text: 'Flex Electricity Usage',
        align: 'center'
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
          format: 'dd/MM/yyyy'
          },
          categories: electricityCategories
      },
      yaxis: [{
        title: {
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
          '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
          '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
          '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
          ];
  var gasPriceOptions = {
          chart: {
            type: 'line',
            stacked: false
          },
          title: {
            text: 'Flex Gas Price',
            align: 'center'
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
              format: 'dd/MM/yyyy'
              },
              categories: gasCategories
          },
          yaxis: [{
            title: {
              text: 'p/kWh'
            },
            forceNiceScale: true,
            labels: {
              formatter: function(val) {
                return val.toLocaleString();
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
      '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
      '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
      '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
      ];
  var gasUsageOptions = {
        chart: {
            type: 'bar',
          stacked: true
        },
        title: {
          text: 'Flex Gas Usage',
          align: 'center'
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
            format: 'dd/MM/yyyy'
            },
            categories: gasCategories
        },
        yaxis: [{
          title: {
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
  updateClassOnClick(dashboardItemId, 'listitem-hidden', '')
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
  switch(period) {
    case 'Daily':
      return 'HH:mm';
    case "Weekly":
      return 'dd/MM/yyyy';
    case "Monthly":
      return 'dd';
    case "Yearly":
      return 'MMM';
  }
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
    title: chartOptions.title,
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
      showForSingleSeries: true,
      showForNullSeries: true,
      showForZeroSeries: true,
      position: 'right',
      onItemClick: {
        toggleDataSeries: true
      },
      formatter: function(seriesName) {
        return seriesName + '<br><br>';
      }
    },
    series: newSeries,
    yaxis: chartOptions.yaxis,
    xaxis: chartOptions.xaxis
  };  

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
  var checkBoxes = getCheckedCheckBoxes();

  loadMap(checkBoxes);
  loadDatagrid(checkBoxes);
  loadDashboardHeaderNumberOfSites(checkBoxes);
  loadDashboardHeaderPortfolioAnnualisedEnergy(checkBoxes);
  loadDashboardHeaderCarbon(checkBoxes);
  loadDashboardHeaderOpportunities(checkBoxes);

  addCustomDashboardItems(checkBoxes);
}

function getCheckedCheckBoxes() {
  var inputs = document.getElementById("siteDiv").getElementsByTagName('input');
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

function loadDashboardHeaderNumberOfSites(checkBoxes) {
  var addresses = [];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var linkedSite = checkBoxes[i].attributes['LinkedSite'].nodeValue;
    var siteAttributes = getSiteAttributesByName(linkedSite);

    var address = getAttribute(siteAttributes, 'GoogleAddress');
    if(address && !addresses.includes(address)) {
      addresses.push(address);
    }
  }

  var element = document.getElementById("dashboardHeaderNumberOfSites");
  element.innerHTML = addresses.length;
}

function loadDashboardHeaderPortfolioAnnualisedEnergy(checkBoxes) {
  var usage = 0;
  var cost = 0;

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var guid = checkBoxes[i].getAttribute("guid");
    var meter = getMeterByGUID(guid);

    var meterUsage = getAttribute(meter.Attributes, "AnnualVolume");
    var meterCost = getAttribute(meter.Attributes, "AnnualCost");

    usage += meterUsage;
    cost += meterCost;
  }

  var usageElement = document.getElementById("dashboardHeaderPortfolioAnnualisedEnergyUsage");
  usageElement.innerHTML = "Usage: ".concat(usage.toLocaleString().concat(" kWh"));

  var costElement = document.getElementById("dashboardHeaderPortfolioAnnualisedEnergyCost");
  costElement.innerHTML = "Cost: £".concat(cost.toLocaleString());
}

function loadDashboardHeaderCarbon(checkBoxes) {
  var carbon = 0;

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var guid = checkBoxes[i].getAttribute("guid");
    var meter = getMeterByGUID(guid);

    var meterCarbon = getAttribute(meter.Attributes, "Carbon");

    carbon += meterCarbon;
  }

  var carbonElement = document.getElementById("dashboardHeaderCarbon");
  carbonElement.innerHTML = carbon.toLocaleString().concat(" tonnes");
}

function loadDashboardHeaderOpportunities(checkBoxes) {
  var headers = ["Number","UsageSaving","CostSaving"];
  var pendingOpportunities = [0,0,0];
  var activeOpportunities = [0,0,0];
  var finishedOpportunities = [0,0,0];

  var checkBoxLength = checkBoxes.length;
  for(var i = 0; i < checkBoxLength; i++) {
    var guid = checkBoxes[i].getAttribute("guid");
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

  var pendingOpportunitiesCountElement = document.getElementById("dashboardHeaderPendingOpportunitiesCount");
  pendingOpportunitiesCountElement.innerHTML = "Count: ".concat(pendingOpportunities[0].toLocaleString());

  var pendingOpportunitiesUsageElement = document.getElementById("dashboardHeaderPendingOpportunitiesUsage");
  pendingOpportunitiesUsageElement.innerHTML = "Usage Saving: ".concat(pendingOpportunities[1].toLocaleString().concat(" kWh"));

  var pendingOpportunitiesCostElement = document.getElementById("dashboardHeaderPendingOpportunitiesCost");
  pendingOpportunitiesCostElement.innerHTML = "Cost Saving: £".concat(pendingOpportunities[2].toLocaleString());

  var activeOpportunitiesCountElement = document.getElementById("dashboardHeaderActiveOpportunitiesCount");
  activeOpportunitiesCountElement.innerHTML = "Count: ".concat(activeOpportunities[0].toLocaleString());

  var activeOpportunitiesUsageElement = document.getElementById("dashboardHeaderActiveOpportunitiesUsage");
  activeOpportunitiesUsageElement.innerHTML = "Usage Saving: ".concat(activeOpportunities[1].toLocaleString().concat(" kWh"));

  var activeOpportunitiesCostElement = document.getElementById("dashboardHeaderActiveOpportunitiesCost");
  activeOpportunitiesCostElement.innerHTML = "Cost Saving: £".concat(activeOpportunities[2].toLocaleString());

  var finishedOpportunitiesCountElement = document.getElementById("dashboardHeaderFinishedOpportunitiesCount");
  finishedOpportunitiesCountElement.innerHTML = "Count: ".concat(finishedOpportunities[0].toLocaleString());

  var finishedOpportunitiesUsageElement = document.getElementById("dashboardHeaderFinishedOpportunitiesUsage");
  finishedOpportunitiesUsageElement.innerHTML = "Usage Saving: ".concat(finishedOpportunities[1].toLocaleString().concat(" kWh"));

  var finishedOpportunitiesCostElement = document.getElementById("dashboardHeaderFinishedOpportunitiesCost");
  finishedOpportunitiesCostElement.innerHTML = "Cost Saving: £".concat(finishedOpportunities[2].toLocaleString());
}