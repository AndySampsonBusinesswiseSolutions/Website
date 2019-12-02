function createBlankChart(chartId, noDataText) {
    var options = {
        chart: {
            height: '100%',
            width: '100%',
          type: 'line'
        },
        series: [],
        yaxis: {
            show: false
        },
      noData: {
            text: noDataText,
            align: 'center',
            verticalAlign: 'middle',
            offsetX: 0,
            offsetY: 0
        }
      }
      
      renderChart(chartId, options);
}

function resizeCharts(windowWidthReduction){
  var finalColumns = document.getElementsByClassName('final-column');
  var chartWidth = window.innerWidth - windowWidthReduction;

  for(var i=0; i<finalColumns.length; i++){
    finalColumns[i].setAttribute('style', 'width: '+chartWidth+'px;');
  }
}

function renderChart(chartId, options) {
  var chart = new ApexCharts(document.querySelector(chartId), options);
  chart.render();
}

function refreshChart(newSeries, newCategories, chartId, chartOptions) {
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
        format: 'dd/MM/yyyy HH:mm'
      }
    },
    legend: {
      show: true,
      position: 'right'
    },
    series: newSeries,
    yaxis: {
      title: {
        text: chartOptions.yaxis.title.text
      },
      show: true
    },
    xaxis: {
      type: 'datetime',
      title: {
        text: chartOptions.xaxis.title.text
      },
      labels: {
          format: 'HH:mm'
      },
      min: chartOptions.xaxis.min,
      max: chartOptions.xaxis.max,
      categories: newCategories
    }
  };  

  renderChart(chartId, options);
}

function testChart(chart) {
    var checkBoxes = [];
		var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
    var inputs = treeDiv.getElementsByTagName('input');
    var commodity = chart.id.replace('Chart', '').toLowerCase();

		for(var i = 0; i < inputs.length; i++) {
			if(inputs[i].type.toLowerCase() == 'checkbox') {
				if(inputs[i].checked) {
					checkBoxes.push(inputs[i]);
				}
			}
		}

		while (chart.firstChild) {
			chart.removeChild(chart.firstChild);
		}

		if(checkBoxes.length == 0) {
			createBlankChart('#' + commodity + 'Chart', 'There is no ' + commodity + ' data to display. Select from the tree to the left to display');
			return;
		}
    
    var showBySpan = document.getElementById(commodity.concat('ChartHeaderShowBy'));
    var typeSpan = document.getElementById(commodity.concat('ChartHeaderType'));
    var periodSpan = document.getElementById(commodity.concat('ChartHeaderPeriod'));
    var chartDate = new Date(document.getElementById(commodity.concat('Calendar')).value);

    var showBy = showBySpan.children[0].value;
		var newSeries = [];
    var newCategories = [];
    
    switch(periodSpan.children[0].value) {
      case 'Daily':
        for(var hh = 1; hh < 49; hh++) {
          var newCategoryText = formatDate(new Date(chartDate.getTime() + hh*30*60000), 'yyyy-MM-dd hh:mm:ss');
          newCategories.push(newCategoryText);
        }
        break;
    }

		for(var checkboxCount = 0; checkboxCount < checkBoxes.length; checkboxCount++) {
      var checkboxBranch = checkBoxes[checkboxCount].attributes['Branch'].nodeValue;

			if(checkboxBranch == 'Site') {
				var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
				
				var siteCount = 0;
				for(siteCount = 0; siteCount < data.length; siteCount++) {
					if(data[siteCount].SiteName == span.innerHTML) {
						break;
					}
				}

				var meters = data[siteCount].Meters;
				var siteSeries = {
						name: span.innerHTML,
						data: []
					};

				for(var meterCount = 0; meterCount < meters.length; meterCount++) {
					if(meters[meterCount].Commodity.toLowerCase() == commodity) {
            var meterData = meters[meterCount][showBy];
            
            if(!meterData) {
              continue;
            }

						for(var i = 0; i < newCategories.length; i++) {
							var value = null;

							for(var j = 0; j < meterData.length; j++) {
								if(meterData[j].Date == newCategories[i]) {
									value = meterData[j].Value;
									break;
								}
							}

							if(!Array.isArray(siteSeries.data) || siteSeries.data.length == i) {
								siteSeries.data.push(value);
              }
							else {
                if(value === null && siteSeries.data[i] === null){
                  siteSeries.data[i] = null;
                }
                else if(value !== null) {
                  siteSeries.data[i] += value;
                }								
							}
						}
					}
				}

				newSeries.push(siteSeries);
      }
      else if(checkboxBranch.includes('GroupByOption')) {
        var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
        var groupByOption = span.innerHTML;
        var groupBy = checkboxBranch.replace('GroupByOption|', '');

        var meters = [];
        for(var siteCount = 0; siteCount < data.length; siteCount++) {
          if(data[siteCount].Meters) {
            for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
              if(data[siteCount].Meters[meterCount][groupBy] == groupByOption) {
                meters.push(data[siteCount].Meters[meterCount]);
              }
            }
          }
        }

        var groupByOptionSeries = {
          name: groupByOption,
          data: []
        };

        for(var meterCount = 0; meterCount < meters.length; meterCount++) {
          if(meters[meterCount].Commodity.toLowerCase() == commodity) {
            var meterData = meters[meterCount][showBy];
            
            if(!meterData) {
              continue;
            }

            for(var i = 0; i < newCategories.length; i++) {
              var value = null;

              for(var j = 0; j < meterData.length; j++) {
                if(meterData[j].Date == newCategories[i]) {
                  value = meterData[j].Value;
                  break;
                }
              }

              if(!Array.isArray(groupByOptionSeries.data) || groupByOptionSeries.data.length == i) {
                groupByOptionSeries.data.push(value);
              }
              else {
                if(value === null && groupByOptionSeries.data[i] === null){
                  groupByOptionSeries.data[i] = null;
                }
                else if(value !== null) {
                  groupByOptionSeries.data[i] += value;
                }								
              }
            }
          }
        }

        newSeries.push(groupByOptionSeries);
      }
      else if(checkboxBranch.includes('GroupBySubOption')) {
        var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
        var groupBySubOption = span.innerHTML;
        var groupBy = checkboxBranch.replace('GroupBySubOption|', '');

        var meters = [];
        for(var siteCount = 0; siteCount < data.length; siteCount++) {
          if(data[siteCount].Meters) {
            for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
              if(data[siteCount].Meters[meterCount][groupBy] == groupBySubOption) {
                meters.push(data[siteCount].Meters[meterCount]);
              }
            }
          }
        }

        var groupBySubOptionSeries = {
          name: groupBySubOption,
          data: []
        };

        for(var meterCount = 0; meterCount < meters.length; meterCount++) {
          if(meters[meterCount].Commodity.toLowerCase() == commodity) {
            var meterData = meters[meterCount][showBy];
            
            if(!meterData) {
              continue;
            }

            for(var i = 0; i < newCategories.length; i++) {
              var value = null;

              for(var j = 0; j < meterData.length; j++) {
                if(meterData[j].Date == newCategories[i]) {
                  value = meterData[j].Value;
                  break;
                }
              }

              if(!Array.isArray(groupBySubOptionSeries.data) || groupBySubOptionSeries.data.length == i) {
                groupBySubOptionSeries.data.push(value);
              }
              else {
                if(value === null && groupBySubOptionSeries.data[i] === null){
                  groupBySubOptionSeries.data[i] = null;
                }
                else if(value !== null) {
                  groupBySubOptionSeries.data[i] += value;
                }								
              }
            }
          }
        }

        newSeries.push(groupBySubOptionSeries);
      }
      else if(checkboxBranch == 'Meter') {
        var meter;
        for(var siteCount = 0; siteCount < data.length; siteCount++) {
          if(data[siteCount].Meters) {
            for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
              if(data[siteCount].Meters[meterCount].Identifier == checkBoxes[checkboxCount].id.replace('Meter', '').replace('checkbox', '')) {
                meter = data[siteCount].Meters[meterCount];
                break;
              }
            }
          }

          if(meter) {
            break;
          }
        }
        
        var meterSeries = {
          name: meter.Identifier,
          data: []
        };

        var meterData = meter[showBy];
            
        if(meterData) {
          for(var i = 0; i < newCategories.length; i++) {
            var value = null;

            for(var j = 0; j < meterData.length; j++) {
              if(meterData[j].Date == newCategories[i]) {
                value = meterData[j].Value;
                break;
              }
            }

            meterSeries.data.push(value);
          }
        }

        newSeries.push(meterSeries);
      }
      else if(checkboxBranch == 'SubMeter') {
        var subMeter;
        for(var siteCount = 0; siteCount < data.length; siteCount++) {
          if(data[siteCount].Meters) {
            for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
              if(data[siteCount].Meters[meterCount]['SubMeters']){
                for(var subMeterCount = 0; subMeterCount < data[siteCount].Meters[meterCount]['SubMeters'].length; subMeterCount++){
                  if(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount].Identifier.replace(/ /g, '') == checkBoxes[checkboxCount].id.replace('SubMeter', '').replace('checkbox', '')) {
                    subMeter = data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount];
                    break;
                  }
                }
              }              
              
              if(subMeter) {
                break;
              }
            }
          }					

          if(subMeter) {
            break;
          }
        }
        
        var meterSeries = {
          name: subMeter.Identifier,
          data: []
        };

        var meterData = subMeter[showBy];
            
        if(meterData) {
          for(var i = 0; i < newCategories.length; i++) {
            var value = null;

            for(var j = 0; j < meterData.length; j++) {
              if(meterData[j].Date == newCategories[i]) {
                value = meterData[j].Value;
                break;
              }
            }

            meterSeries.data.push(value);
          }
        }

        newSeries.push(meterSeries);
      }
		}		

    var chartType;
    var chartStacked = false;
    var chartYAxisTitle;

    switch(showBy) {
      case 'Energy':
        chartYAxisTitle = 'Energy (MWh)';
        break;
      case 'Power':
        chartYAxisTitle = 'Power (MW)';
        break;
      case 'Current':
        chartYAxisTitle = 'Current (A)';
        break;
      case 'Cost':
        chartYAxisTitle = 'Cost (£)';
        break;
    }

    switch(typeSpan.children[0].value){
      case 'Line':
      case 'Bar':
      case 'Area':
        chartType = typeSpan.children[0].value.toLowerCase();
        break;
      case 'Stacked Line':
      case 'Stacked Bar':
        chartType = typeSpan.children[0].value.replace('Stacked ', '').toLowerCase();
        chartStacked = true;
        break;
    }

    var chartOptions = {
      chart: {
        type: chartType,
        stacked: chartStacked
      },
      yaxis: {
        title: {
          text: chartYAxisTitle
        }
      },
      xaxis: {
        title: {
          text: formatDate(chartDate, 'yyyy-MM-dd')
        },
        min: new Date(newCategories[0]).getTime(),
        max: new Date(newCategories[newCategories.length - 1]).getTime()
      }
    };
		refreshChart(newSeries, newCategories, '#'.concat(commodity).concat('Chart'), chartOptions);
}