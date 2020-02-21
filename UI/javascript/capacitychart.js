function updateChart(callingElement, chart) {
  var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
  var inputs = treeDiv.getElementsByTagName('input');
  var commodity = chart.id.replace('Chart', '').toLowerCase();
  var checkBoxes = getCheckedCheckBoxes(inputs);		
  
  clearElement(chart);

  if(checkBoxes.length == 0) {
    createBlankChart('#' + commodity + 'Chart', 'There is no ' + commodity + ' data to display. Select from the tree to the left to display');
    return;
  }

  var newCategories = getNewCategories('Yearly', new Date(2019, 1, 1));   
  var newSeries = getNewChartSeries(checkBoxes, '', newCategories, commodity, getPeriodDateFormat('Yearly'));

  var maximumDemandSeries = {
    name: 'Maximum Demand',
    data: [0]
  };

  var categoryLength = newCategories.length;
  for(var i = 0; i < categoryLength; i++) {
    maximumDemandSeries.data[i] = 250;
  }

  newSeries.push(maximumDemandSeries);

  var chartOptions = {
  chart: {
    type: getChartType('Line'),
    stacked: false
  },
  tooltip: {
    x: {
    format: getChartTooltipXFormat('Yearly')
    }
  },
  yaxis: [{
    title: {
      text: getChartYAxisTitle('Energy', commodity)
    },
        show: true
  }],
  xaxis: {
    type: 'datetime',
    title: {
    text: formatDate(new Date(2019, 1, 1), getChartXAxisTitleFormat('Yearly'))
    },
    labels: {
    format: getChartXAxisLabelFormat('Yearly')
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

function getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity, dateFormat) {
var meters = [];
var newSeries = [];
var checkBoxesLength = checkBoxes.length;

for(var checkboxCount = 0; checkboxCount < checkBoxesLength; checkboxCount++) {
  var checkboxBranch = checkBoxes[checkboxCount].attributes['Branch'].nodeValue;
  var linkedSite = checkBoxes[checkboxCount].attributes['LinkedSite'].nodeValue;
  var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
  var seriesName = span.innerHTML;

  if(checkboxBranch == 'Site') {				
    meters = getSitesByAttribute('BaseName', linkedSite)[0].Meters;
  }
  else if(checkboxBranch.includes('GroupByOption')) {
    meters = getMetersByAttribute(checkboxBranch.replace('GroupByOption|', ''), span.innerHTML, linkedSite);
    seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
  }
  else if(checkboxBranch.includes('GroupBySubOption')) {
    meters = getMetersByAttribute(checkboxBranch.replace('GroupBySubOption|', ''), span.innerHTML, linkedSite);
    seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
  }
  else if(checkboxBranch == 'Meter') {
    meters = getMetersByAttribute('Identifier', span.innerHTML, linkedSite);
  }
  else if(checkboxBranch == 'SubMeter') {
    meters = getSubMetersByAttribute('Identifier', span.innerHTML, linkedSite);
    seriesName = linkedSite.concat(' - ').concat(span.innerHTML);
  }

  newSeries.push(summedMeterSeries(meters, seriesName, 'Energy', newCategories, commodity, dateFormat));
}

return newSeries;
}

function getSitesByAttribute(attribute, value) {
var sites = []
var dataLength = data.length;

for(var siteCount = 0; siteCount < dataLength; siteCount++) {
  var site = data[siteCount];

  if(getAttribute(site.Attributes, attribute) == value) {
    sites.push(site);
  }
}

return sites;
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

return meters;
}

function linkedSiteMatch(identifier, meterType, linkedSite) {
var identifierCheckbox = document.getElementById(meterType.concat(identifier).concat('checkbox'));
var identifierLinkedSite = identifierCheckbox.attributes['LinkedSite'].nodeValue;

return identifierLinkedSite == linkedSite;
}

function getSubMetersByAttribute(attribute, value, linkedSite) {
var subMeters = [];
var dataLength = data.length;

for(var siteCount = 0; siteCount < dataLength; siteCount++) {
  var site = data[siteCount];
  var meterLength = site.Meters.length;

  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = site.Meters[meterCount];

    if(meter['SubMeters']){
      var meterSubMeters = meter['SubMeters'];
      var subMeterLength = meterSubMeters.length;

      for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++){
        var subMeter = meterSubMeters[subMeterCount];

        if(getAttribute(subMeter.Attributes, attribute) == value) {
          if(linkedSiteMatch(subMeter.GUID, 'SubMeter', linkedSite)) {
            subMeters.push(subMeter);
          }            
        }
      }
    }              
  }			
}

return subMeters;
}

function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity, dateFormat) {
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
      var i = newCategories.findIndex(n => n == formatDate(meterData[j].Date, dateFormat));
      var value = meterData[j].Value;

      if(!value && !summedMeterSeries.data[i]){
        summedMeterSeries.data[i] = null;
      }
      else if(value && !summedMeterSeries.data[i]) {
        summedMeterSeries.data[i] = value;
      }
      else if(value && summedMeterSeries.data[i]) {
        summedMeterSeries.data[i] += value;
      }							     
    }
  }
}

return summedMeterSeries;
}

function getNewCategories(period, chartDate) {
var dateFormat = getPeriodDateFormat(period);
switch(period) {
  case 'Daily':
    return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate()), new Date(chartDate.getFullYear(), chartDate.getMonth(), chartDate.getDate() + 1), dateFormat);
  case "Weekly":
    return getCategoryTexts(getMonday(chartDate), new Date(getMonday(chartDate).getFullYear(), getMonday(chartDate).getMonth(), getMonday(chartDate).getDate() + 7), dateFormat);
  case "Monthly":
    return getCategoryTexts(new Date(chartDate.getFullYear(), chartDate.getMonth(), 1), new Date(chartDate.getFullYear(), chartDate.getMonth() + 1, 1), dateFormat);
  case "Yearly":
    return getCategoryTexts(new Date(chartDate.getFullYear(), 0, 1), endDate = new Date(chartDate.getFullYear() + 1, 0, 1), dateFormat);
}
}

function getPeriodDateFormat(period) {
switch(period) {
  case 'Daily':
  case "Weekly":
  case "Monthly":
  case "Yearly":
    return 'yyyy-MM-dd';
}
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

function getChartYAxisTitle(showBy, commodity) {
return 'Capacity (kVa)';
}