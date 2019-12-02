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
        format: chartOptions.tooltip.x.format
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
          format: chartOptions.xaxis.labels.format
      },
      min: chartOptions.xaxis.min,
      max: chartOptions.xaxis.max,
      categories: newCategories
    }
  };  

  renderChart(chartId, options);
}

function testChart(chart) {
    var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
    var inputs = treeDiv.getElementsByTagName('input');
    var commodity = chart.id.replace('Chart', '').toLowerCase();
    var checkBoxes = getCheckedCheckBoxes(inputs);		
    
    clearElement(chart);

		if(checkBoxes.length == 0) {
			createBlankChart('#' + commodity + 'Chart', 'There is no ' + commodity + ' data to display. Select from the tree to the left to display');
			return;
		}
    
    var showBySpan = document.getElementById(commodity.concat('ChartHeaderShowBy'));
    var periodSpan = document.getElementById(commodity.concat('ChartHeaderPeriod'));
    var chartDate = new Date(document.getElementById(commodity.concat('Calendar')).value);
    var newCategories = getNewCategories(periodSpan.children[0].value, chartDate);   
    var newSeries = getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity);
    var typeSpan = document.getElementById(commodity.concat('ChartHeaderType'));

    var chartOptions = {
      chart: {
        type: getChartType(typeSpan.children[0].value),
        stacked: typeSpan.children[0].value.includes('Stacked')
      },
      tooltip: {
        x: {
          format: getChartTooltipXFormat(periodSpan.children[0].value)
        }
      },
      yaxis: {
        title: {
          text: getChartYAxisTitle(showBySpan.children[0].value)
        }
      },
      xaxis: {
        title: {
          text: formatDate(chartDate, getChartXAxisTitleFormat(periodSpan.children[0].value))
        },
        labels: {
          format: getChartXAxisLabelFormat(periodSpan.children[0].value)
        },
        min: new Date(newCategories[0]).getTime(),
        max: new Date(newCategories[newCategories.length - 1]).getTime()
      }
    };

		refreshChart(newSeries, newCategories, '#'.concat(commodity).concat('Chart'), chartOptions);
}

function getCheckedCheckBoxes(inputs) {
  var checkBoxes = [];

  for(var i = 0; i < inputs.length; i++) {
    if(inputs[i].type.toLowerCase() == 'checkbox') {
      if(inputs[i].checked) {
        checkBoxes.push(inputs[i]);
      }
    }
  }

  return checkBoxes;
}

function getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity) {
  var newSeries = [];

  for(var checkboxCount = 0; checkboxCount < checkBoxes.length; checkboxCount++) {
    var checkboxBranch = checkBoxes[checkboxCount].attributes['Branch'].nodeValue;
    var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));

    if(checkboxBranch == 'Site') {				
      var meters = getSitesByAttribute('SiteName', span.innerHTML)[0].Meters;

      newSeries.push(summedMeterSeries(meters, span.innerHTML, showBySpan.children[0].value, newCategories, commodity));
    }
    else if(checkboxBranch.includes('GroupByOption')) {
      newSeries.push(summedMeterSeries(
        getMetersByAttribute(checkboxBranch.replace('GroupByOption|', ''), span.innerHTML), 
        span.innerHTML, showBySpan.children[0].value, newCategories, commodity));
    }
    else if(checkboxBranch.includes('GroupBySubOption')) {
      newSeries.push(summedMeterSeries(
        getMetersByAttribute(checkboxBranch.replace('GroupBySubOption|', ''), span.innerHTML), 
        span.innerHTML, showBySpan.children[0].value, newCategories, commodity));
    }
    else if(checkboxBranch == 'Meter') {
      var meter = getMetersByAttribute('Identifier', span.innerHTML)[0];
      
      newSeries.push(newMeterSeries(meter.Identifier, meter[showBySpan.children[0].value], newCategories));
    }
    else if(checkboxBranch == 'SubMeter') {
      var subMeter = getSubMetersByAttribute('Identifier', span.innerHTML)[0];
      
      newSeries.push(newMeterSeries(subMeter.Identifier, subMeter[showBySpan.children[0].value], newCategories));
    }
  }

  return newSeries;
}

function getSitesByAttribute(attribute, value) {
  var sites = []
  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    if(data[siteCount][attribute] == value) {
      sites.push(data[siteCount]);
    }
  }

  return sites;
}

function getMetersByAttribute(attribute, value) {
  var meters = [];

  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
      if(data[siteCount].Meters[meterCount][attribute] == value) {
        meters.push(data[siteCount].Meters[meterCount]);
      }
    }
  }

  return meters;
}

function getSubMetersByAttribute(attribute, value) {
  var subMeters = [];

  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
      if(data[siteCount].Meters[meterCount]['SubMeters']){
        for(var subMeterCount = 0; subMeterCount < data[siteCount].Meters[meterCount]['SubMeters'].length; subMeterCount++){
          if(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount][attribute] == value) {
            subMeters.push(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount]);
          }
        }
      }              
    }			
  }

  return subMeters;
}

function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity) {
  var summedMeterSeries = {
    name: seriesName,
    data: []
  };

  for(var meterCount = 0; meterCount < meters.length; meterCount++) {
    if(commodityMeterMatch(meters[meterCount], commodity)) {
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

        if(!Array.isArray(summedMeterSeries.data) || summedMeterSeries.data.length == i) {
          summedMeterSeries.data.push(value);
        }
        else {
          if(value === null && summedMeterSeries.data[i] === null){
            summedMeterSeries.data[i] = null;
          }
          else if(value !== null) {
            summedMeterSeries.data[i] += value;
          }								
        }
      }
    }
  }

  return summedMeterSeries;
}

function newMeterSeries(meterIdentifier, meterData, newCategories) {
  var meterSeries = {
    name: meterIdentifier,
    data: []
  };
      
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

  return meterSeries;
}

function getNewCategories(period, chartDate) {
  switch(period) {
    case 'Daily':
      return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate()), new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate() + 1));
    case "Weekly":
      return getCategoryTexts(getMonday(chartDate), new Date(getMonday(chartDate).getFullYear(), getMonday(chartDate).getMonth(), getMonday(chartDate).getDate() + 7));
    case "Monthly":
      return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), 1), new Date(chartDate.getFullYear(), chartDate.getMonth() + 1, 1));
    case "Yearly":
      return getCategoryTexts(new Date(chartDate.getFullYear(), 1, 1), endDate = new Date(chartDate.getFullYear() + 1, 1, 1));
  }
}

function getCategoryTexts(startDate, endDate) {
  var newCategories = [];

  for(var newDate = startDate; newDate < endDate; newDate.setDate(newDate.getDate() + 1)) {
    for(var hh = 1; hh < 49; hh++) {
      var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), 'yyyy-MM-dd hh:mm:ss');
      newCategories.push(newCategoryText);
    }
  }

  return newCategories;
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

function getChartXAxisTitleFormat(period) {
  switch(period) {
    case 'Daily':
      return 'yyyy-MM-dd';
    case "Weekly":
      return 'yyyy-MM-dd to yyyy-MM-dd';
    case "Monthly":
      return 'MMM yyyy';
    case "Yearly":
      return 'yyyy';
  }
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

function getChartYAxisTitle(showBy) {
  switch(showBy) {
    case 'Energy':
      return 'Energy (MWh)';
    case 'Power':
      return 'Power (MW)';
    case 'Current':
      return 'Current (A)';
    case 'Cost':
      return 'Cost (£)';
  }
}