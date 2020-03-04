function loadPage(){
  createTree(data, "siteDiv", "", "Site");
  createTree(dashboard, "treeDiv", "addDashboardItem", "Dashboard");
  loadMap();
  loadPieChart();
  loadDatagrid();
  loadUsageChart();
  addExpanderOnClickEvents();
}

function loadUsageChart() {
  var electricityVolumeSeries = [{
    name: 'Forecast Volume',
    data: [
    null, null, null,
    null, 438772, 473960, 436016, 448107, 462373, 510771, 494156, 462111, 480764, 484943, 501687,
    475309, 423674, 474684, 436146, 447341, 462320, 510293, 493880, 462111, 480407, 485484, 502031,
    475284, 423674, 474815, 436310, 447291, 462320, 508740, 493786, 461985, 480105, 486236, 502168,
    475972, 423674, 474579, 435772, 447692, 462517, 507667, 493724, 461949, 479810, 486236, 501914,
    477400, 439032, 473482, 435740, 448459, 462890, 509699, 494370, 461067
          ]
    }];

  var electricityCategories = [
    '10 2019', '11 2019', '12 2019',
    '01 2020', '02 2020', '03 2020', '04 2020', '05 2020', '06 2020', '07 2020', '08 2020', '09 2020', '10 2020', '11 2020', '12 2020',
    '01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
    '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
    '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023', '10 2023', '11 2023', '12 2023',
    '01 2024', '02 2024', '03 2024', '04 2024', '05 2024', '06 2024', '07 2024', '08 2024', '09 2024'
    ];

  var electricityVolumeOptions = {
      chart: {
        type: 'bar'
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
          min: 0,
          max: 520000,
          decimalsInFloat: 0
      }]
    };

  refreshChart(electricityVolumeSeries, "#electricityVolumeChart", electricityVolumeOptions);
}

function loadDatagrid() {
  jexcel(document.getElementById('spreadsheet'), {
    data:[
        {address:'39-41 Buckingham Palace Road, London, SW1W 0PS',	meterpoint:'1200050469869',	annualvolume:'1,978,170',	annualcost:'238,637',carbon:'526'},
        {address:'35 Charles Street, London, W1J 5EB',	meterpoint:'1200010064476',	annualvolume:'725,508',	annualcost:'92,358',carbon:'202'},
        {address:'35 Charles Street, London, W1J 5EB',	meterpoint:'1200051256079',	annualvolume:'332,102',	annualcost:'42,042',carbon:'93'},
        {address:'Montague Street, London, WC1B 5BJ',	meterpoint:'1200050071348',	annualvolume:'977,782',	annualcost:'123,285',carbon:'273'},
        {address:'1-3 1-2 Kensington Court, London, W8 5DL',	meterpoint:'1200010015159',	annualvolume:'932,394',	annualcost:'118,214',carbon:'260'},
        {address:'9 Fore Street, Evershot, Dorchester, DT2 0JR',	meterpoint:'2000027480903',	annualvolume:'415,371',	annualcost:'52,984',carbon:'115'},
        {address:'17-19 Egerton Terrace, London, SW3 2BX',	meterpoint:'1200010016808',	annualvolume:'292,364',	annualcost:'37,047',carbon:'82'}
    ],
    columns: [
        {type:'text', width:'150px', name:'address', title:'Address'},
        {type:'text', width:'130px', name:'meterpoint', title:'Meter Point'},
        {type:'text', width:'175px', name:'annualvolume', title:'Annual Volume (kWh)'},
        {type:'text', width:'175px', name:'annualcost', title:'Annual Cost (£)'},
        {type:'text', width:'175px', name:'carbon', title:'Carbon (tonnes)'},
     ]
});
}

function loadPieChart() {
  var options = {
    series: [93306, 1950, 4500, 2800],
    chart: {
    width: 475,
    type: 'pie'
  },
  dataLabels: {
    enabled: false
  },
  labels: ['Predictive Spend', 'Heaters', 'Chillers', 'Good Practice Savings @ 3%'],
  responsive: [{
    breakpoint: 480,
    options: {
      chart: {
        width: 400
      }
    }
  }]
  };

  var chart = new ApexCharts(document.querySelector("#piechart"), options);
  chart.render();
}

function loadMap() {
  var geocoder = new google.maps.Geocoder();
    var mapOptions = {
      zoom: 4.75,
      center: new google.maps.LatLng(55, -5),
      mapTypeId: google.maps.MapTypeId.SATELLITE
    }
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    var addresses = [
      '39-41 Buckingham Palace Road, London, SW1W 0PS',
      '35 Charles Street, London, W1J 5EB',
      'Montague Street, London, WC1B 5BJ',
      '1-3 1-2 Kensington Court, London, W8 5DL',
      '9 Fore Street, Evershot, Dorchester, DT2 0JR',
      '17-19 Egerton Terrace, London, SW3 2BX'
    ]
    
    var addressLength = addresses.length;
    for(var i = 0; i < addressLength; i++) {
      geocoder.geocode( { 'address': addresses[i] }, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location,
                title: results[0].formatted_address
            });
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
      });
    }    
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction, dataName) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction, dataName);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, checkboxFunction, dataName) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');

        if(dataName == "Dashboard") {
          createUL();

          appendListItemChildren(li, 'Dashboard'.concat(base.GUID), checkboxFunction, 'Dashboard', baseName, '', '', baseName, base.GUID, false);
        }
        else {
          var electricityCommodityradio = document.getElementById('electricityCommodityradio');
          var commodity = electricityCommodityradio.checked ? 'Electricity' : 'Gas';

          if(!commoditySiteMatch(base, commodity)) {
            continue;
          }

          var ul = createUL();
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
          branchDiv.removeAttribute('class', 'far fa-plus-square');
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

  if(commodity == '') {
    li.appendChild(createSpan(id, branchOption));
  }
  else {
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
  }    
}

function createBranchDiv(branchDivId, childrenCreated = true) {
  var branchDiv = document.createElement('div');
  branchDiv.id = branchDivId;

  if(childrenCreated) {
      branchDiv.setAttribute('class', 'far fa-plus-square');
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

    if(branch == 'Dashboard') {
      var width;
      var height;
      var dataLength = dashboard.length;
      for(var i = 0; i < dataLength; i++) {
          var item = dashboard[i];
  
          if(item.GUID == guid) {
              width = getAttribute(item.Attributes, "Width");
              height = getAttribute(item.Attributes, "Height");
              break;
          }
      }
      functionArguments.push('"'.concat(height).concat('"'));
      functionArguments.push('"'.concat(width).concat('"'));
    }    

    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function addDashboardItem(checkbox, height, width) {
    var dashboard = document.getElementById('dashboard');
    var guid = checkbox.getAttribute('guid');
    var dashboardItemId = 'dashboardItem'.concat(guid);

    if(checkbox.checked) {
        var newDashboardItem = document.createElement('div');
        newDashboardItem.id = dashboardItemId;
        newDashboardItem.setAttribute('class', 'roundborder');
        newDashboardItem.setAttribute('style', 'float: left; width: ' + width + '; height: ' + height + ';');   
        dashboard.appendChild(newDashboardItem);

        var electricityVolumeSeries = [{
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
          
        var electricityVolumeOptions = {
          chart: {
            type: 'bar',
            stacked: true
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
              min: 0,
              max: 3,
              decimalsInFloat: 3
          }]
        };
        
        var electricityPriceOptions = {
          chart: {
            type: 'line',
            stacked: false
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
              min: 4,
              max: 6,
              decimalsInFloat: 3
          }]
        };
        
        var gasVolumeSeries = [{
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
          
        var gasVolumeOptions = {
          chart: {
              type: 'bar',
            stacked: true
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
              min: 0,
              max: 5000,
              decimalsInFloat: 0
          }]
        };
        var gasPriceOptions = {
          chart: {
            type: 'line',
            stacked: false
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
              min: 1.1,
              max: 2.1,
              decimalsInFloat: 3
          }]
        };

        if(guid == 0) {
            var flexElectricityPriceItem = document.createElement('div');
            flexElectricityPriceItem.id = 'electricityPriceChart';

            newDashboardItem.appendChild(flexElectricityPriceItem);
            refreshChart(electricityPriceSeries, "#electricityPriceChart", electricityPriceOptions);
        }

        if(guid == 1) {
            var flexElectricityPositionItem = document.createElement('div');
            flexElectricityPositionItem.id = 'electricityVolumeChart';

            newDashboardItem.appendChild(flexElectricityPositionItem);
            refreshChart(electricityVolumeSeries, "#electricityVolumeChart", electricityVolumeOptions);
        }

        if(guid == 2) {
            var flexGasPriceItem = document.createElement('div');
            flexGasPriceItem.id = 'gasPriceChart';

            newDashboardItem.appendChild(flexGasPriceItem);
            refreshChart(gasPriceSeries, "#gasPriceChart", gasPriceOptions);
        }

        if(guid == 3) {
            var flexGasPositionItem = document.createElement('div');
            flexGasPositionItem.id = 'gasVolumeChart';

            newDashboardItem.appendChild(flexGasPositionItem);
            refreshChart(gasVolumeSeries, "#gasVolumeChart", gasVolumeOptions);
        }
    }
    else {
        var dashboardItem = document.getElementById(dashboardItemId);
        dashboard.removeChild(dashboardItem);
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
        enabled: false
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
    tooltip: {
      x: {
        format: chartOptions.tooltip.x.format
      }
    },
    legend: {
      show: true,
      position: 'right',
      onItemClick: {
        toggleDataSeries: false
      }
    },
    series: newSeries,
    yaxis: chartOptions.yaxis,
    xaxis: chartOptions.xaxis
  };  

  renderChart(chartId, options);
}

function renderChart(chartId, options) {
  var chart = new ApexCharts(document.querySelector(chartId), options);
  chart.render();
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
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});

	updateAdditionalControls(element);
	expandAdditionalLists(element);
}

function updateAdditionalControls(element) {
	var additionalcontrols = element.getAttribute('additionalcontrols');

	if(!additionalcontrols) {
		return;
	}

	var listToHide = element.id.concat('List');
	var clickEventFunction = function (event) {
		updateClassOnClick(listToHide, 'listitem-hidden', '')
	};

	var controlArray = additionalcontrols.split(',');
	for(var j = 0; j < controlArray.length; j++) {
		var controlId = controlArray[j];	

		element.addEventListener('click', function (event) {
			var controlElement = document.getElementById(controlId);
			if(hasClass(this, 'fa-minus-square')) {				
				controlElement.addEventListener('click', clickEventFunction, false);
			}
			else {
				controlElement.removeEventListener('click', clickEventFunction);
			}
		});
	}	
}

function expandAdditionalLists(element) {
	var additionalLists = element.getAttribute('additionallists');

	if(!additionalLists) {
		return;
	}

	element.addEventListener('click', function (event) {
		var controlArray = additionalLists.split(',');
		for(var j = 0; j < controlArray.length; j++) {
			var controlId = controlArray[j];
			var controlElement = document.getElementById(controlId);
			updateClass(controlElement, 'listitem-hidden', '');
		}
	});		
}