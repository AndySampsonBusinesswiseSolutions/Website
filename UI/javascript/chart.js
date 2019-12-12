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

function getNewChartSeries(checkBoxes, showBySpan, newCategories, commodity, dateFormat) {
  var meters = [];
  var newSeries = [];

  for(var checkboxCount = 0; checkboxCount < checkBoxes.length; checkboxCount++) {
    var checkboxBranch = checkBoxes[checkboxCount].attributes['Branch'].nodeValue;
    var linkedSite = checkBoxes[checkboxCount].attributes['LinkedSite'].nodeValue;
    var span = document.getElementById(checkBoxes[checkboxCount].id.replace('checkbox', 'span'));
    var seriesName = span.innerHTML;

    if(checkboxBranch == 'Site') {				
      meters = getSitesByAttribute('SiteName', linkedSite)[0].Meters;
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

    newSeries.push(summedMeterSeries(meters, seriesName, showBySpan.children[0].value, newCategories, commodity, dateFormat));
  }

  return newSeries;
}

function getSitesByAttribute(attribute, value) {
  var sites = []
  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    if(getAttribute(data[siteCount].Attributes, attribute) == value) {
      sites.push(data[siteCount]);
    }
  }

  return sites;
}

function getMetersByAttribute(attribute, value, linkedSite) {
  var meters = [];

  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
      if(getAttribute(data[siteCount].Meters[meterCount].Attributes, attribute) == value) {
        if(linkedSiteMatch(data[siteCount].Meters[meterCount].GUID, 'Meter', linkedSite)) {
          meters.push(data[siteCount].Meters[meterCount]);
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

  for(var siteCount = 0; siteCount < data.length; siteCount++) {
    for(var meterCount = 0; meterCount < data[siteCount].Meters.length; meterCount++) {
      if(data[siteCount].Meters[meterCount]['SubMeters']){
        for(var subMeterCount = 0; subMeterCount < data[siteCount].Meters[meterCount]['SubMeters'].length; subMeterCount++){
          if(getAttribute(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount].Attributes, attribute) == value) {
            if(linkedSiteMatch(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount].GUID, 'SubMeter', linkedSite)) {
              subMeters.push(data[siteCount].Meters[meterCount]['SubMeters'][subMeterCount]);
            }            
          }
        }
      }              
    }			
  }

  return subMeters;
}

function summedMeterSeries(meters, seriesName, showBy, newCategories, commodity, dateFormat) {
  var summedMeterSeries = {
    name: seriesName,
    data: [0]
  };

  for(var meterCount = 0; meterCount < meters.length; meterCount++) {
    if(commodityMeterMatch(meters[meterCount], commodity)) {
      var meterData = meters[meterCount][showBy];
      
      if(!meterData) {
        continue;
      }

      for(var j = 0; j < meterData.length; j++) {
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
      return 'yyyy-MM-dd hh:mm:ss';
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