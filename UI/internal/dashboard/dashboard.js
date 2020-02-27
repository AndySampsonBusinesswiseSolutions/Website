function loadPage(){  
	createTree(dashboard, "treeDiv", "addDashboardItem");
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
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        createUL();

        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createSpan(id, branchOption));
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    return branchDiv;
}

function createUL() {
    var ul = document.createElement('ul');
    ul.setAttribute('class', 'format-listitem');
    return ul;
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