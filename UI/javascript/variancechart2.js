function updateChart(callingElement, chart) {
  var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
  var inputs = treeDiv.getElementsByTagName('input');
  var commodity = chart.id.replace('Chart', '').toLowerCase();
  var checkBoxes = getCheckedCheckBoxes(inputs);

  var showBy = 'Forecast';
  var tabDiv = document.getElementById('tabDiv');
  var buttons = tabDiv.getElementsByClassName('active');

  if(buttons[0].id == 'CostElements')
  {
    var costElementDetailDataDiv = document.getElementById('costElementDetailDataTabDiv');
    buttons = costElementDetailDataDiv.getElementsByClassName('active');
    showBy = buttons[0].id;
  }
  else {
    createBlankChart("#electricityChart", "There's no electricity data to display. Select from the tree to the left to display");
    return;
  }
  
  clearElement(chart);
  
  var newCategories = getNewCategories();   
  var newSeries = getNewChartSeries(checkBoxes, showBy, newCategories, commodity);

  var chartOptions = {
  chart: {
    type: 'bar',
    stacked: false
  },
  tooltip: {
    x: {
    format: getChartTooltipXFormat()
    }
  },
  yaxis: [{
    axisTicks: {
      show: true
    },
    axisBorder: {
      show: true,
    },
    title: {
      text: ''
    },
        show: true,
        decimalsInFloat: 2
  }],
  xaxis: {
    type: 'category',
    title: {
      text: ''
    },
    min: new Date(newCategories[0]).getTime(),
    max: new Date(newCategories[newCategories.length - 1]).getTime(),
        categories: newCategories
  }
  };

  refreshChart(newSeries, newCategories, '#'.concat(commodity).concat('Chart'), chartOptions);
}

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
    var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));

    meters.push(getMetersByAttribute('Identifier', span.innerHTML, linkedSite));
  }

  var series = getSeries(showBy);

  for(var i = 0; i < series.length; i++) {
    newSeries.push(summedMeterSeries(meters, series[i], showBy, newCategories, commodity));
  }

  return newSeries;
}

function getSeries(showBy) {
  switch(showBy) {
    case "WholesaleUsage":
      return ["Latest Forecast Usage","Invoiced Usage","Usage Difference","DUoS Reduction Project","Waste Reduction","Unknown"];
    case "WholesaleCost":
      return ["Latest Forecast Cost","Invoiced Cost","Cost Difference","DUoS Reduction Project","Waste Reduction","Unknown"];
    case "WholesaleRate":
        return ["Latest Forecast Rate","Invoiced Rate","Rate Difference","Usage Change","DUoS Reduction Project","Waste Reduction","Unknown"];
  }
}

function getMetersByAttribute(attribute, value, linkedSite) {
  var meters = [];
  var dataLength = data.length;

  for(var siteCount = 0; siteCount < dataLength; siteCount++) {
    var site = data[siteCount];
    var meterLength = site.Meters.length;

    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = site.Meters[meterCount];

      if(getAttribute(meter.Attributes, attribute) == value) {
        if(linkedSiteMatch(meter.GUID, 'Meter', linkedSite)) {
          meters.push(meter);
        }
      }
    }
  }

  return meters[0];
}

function linkedSiteMatch(identifier, meterType, linkedSite) {
  var identifierCheckbox = document.getElementById(meterType.concat(identifier).concat('checkbox'));
  var identifierLinkedSite = identifierCheckbox.attributes['LinkedSite'].nodeValue;

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
        var i = newCategories.findIndex(n => n == meterData[j][0].Month);
        var value = getAttribute(meterData[j], seriesName);

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

function getChartTooltipXFormat() {
  return 'MMM yyyy';
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