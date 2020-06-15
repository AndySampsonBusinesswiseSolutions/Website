function pageLoad() {
  updatePage();
}

function resetPage() {
  electricityCommoditycheckbox.checked = true;
  gasCommoditycheckbox.checked = true;

  showHideContainer(electricityCommoditycheckbox);
  showHideContainer(gasCommoditycheckbox);

  updatePage();
}

function updatePage() {
  addExpanderOnClickEvents();
  setOpenExpanders();
  setupCharts();
  setupDatagrids();
}

function setupDatagrids() {
  jexcel(document.getElementById('spreadsheet3'), {
    pagination: 12,
        data:[
            {month:'OCT-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'NOV-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'DEC-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'JAN-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'FEB-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'MAR-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'APR-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'MAY-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'JUN-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'JUL-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'AUG-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'SEP-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
        ],
        columns: [
            {type:'text', width:'115px', name:'month', title:'Month'},
            {type:'text', width:'115px', name:'openvol', title:'Open Volume'},
            {type:'text', width:'115px', name:'hedgevol', title:'Hedge Volume'},
            {type:'text', width:'115px', name:'capprice', title:'Cap Price'},
            {type:'text', width:'115px', name:'ecp', title:'ECP'},
            {type:'text', width:'115px', name:'marketprice', title:'Market Price'},
            {type:'text', width:'115px', name:'day1price', title:'Day 1 Price'},
          ]
  });
    
  jexcel(document.getElementById('spreadsheet4'), {
    pagination: 12,
        data:[
            {date:'15/02/2018',	tradereference:'00001',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00003',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00005',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00007',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00009',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00011',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00013',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00015',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00017',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00019',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00021',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00023',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
        ],
        columns: [
            {type:'text', width:'115px', name:'date', title:'Date'},
            {type:'text', width:'135px', name:'tradereference', title:'Trade Reference'},
            {type:'text', width:'115px', name:'period', title:'Period'},
            {type:'text', width:'115px', name:'volume', title:'Volume'},
            {type:'text', width:'115px', name:'price', title:'Price'},
          ]
  });

  jexcel(document.getElementById('spreadsheet5'), {
    pagination: 12,
        data:[
            {month:'OCT-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'NOV-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'DEC-20',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'JAN-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'FEB-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'MAR-21',	openvol:'1.324',	hedgevol:'1.426',	capprice:'5.718',	ecp:'4.99',	marketprice:'4.886',	day1price:'5.918'},
            {month:'APR-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'MAY-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'JUN-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'JUL-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'AUG-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
            {month:'SEP-21',	openvol:'0.713',	hedgevol:'1.657',	capprice:'4.888',	ecp:'5.03',	marketprice:'4.99',	day1price:'4.444'},
        ],
        columns: [
            {type:'text', width:'115px', name:'month', title:'Month'},
            {type:'text', width:'115px', name:'openvol', title:'Open Volume'},
            {type:'text', width:'115px', name:'hedgevol', title:'Hedge Volume'},
            {type:'text', width:'115px', name:'capprice', title:'Cap Price'},
            {type:'text', width:'115px', name:'ecp', title:'ECP'},
            {type:'text', width:'115px', name:'marketprice', title:'Market Price'},
            {type:'text', width:'115px', name:'day1price', title:'Day 1 Price'},
          ]
  });
  
  jexcel(document.getElementById('spreadsheet6'), {
    pagination: 12,
        data:[
            {date:'15/02/2018',	tradereference:'00002',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00004',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00006',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00008',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00010',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00012',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00014',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00016',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00018',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00020',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00022',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
            {date:'15/02/2018',	tradereference:'00024',	period:'Winter 2020',	volume:'1MW',	price:'£40/MWh'},
        ],
        columns: [
            {type:'text', width:'115px', name:'date', title:'Date'},
            {type:'text', width:'135px', name:'tradereference', title:'Trade Reference'},
            {type:'text', width:'115px', name:'period', title:'Period'},
            {type:'text', width:'115px', name:'volume', title:'Volume'},
            {type:'text', width:'115px', name:'price', title:'Price'},
          ]
  });

  var textAreas = document.getElementsByClassName('jexcel_textarea');
  for(var i = 0; i < textAreas.length; i++) {
    textAreas[i].style.display = "none";
  }
}

function setupCharts() {
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
    'OCT-20', '', '',
    'JAN-21', '', '', 'APR-21', '', '', 'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
  ];
    
  var electricityVolumeOptions = {
    chart: {
      type: 'bar',
      stacked: true
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
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
        format: getChartXAxisLabelFormat('Weekly')
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
          return val.toLocaleString();
        }
      },
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
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
        format: getChartXAxisLabelFormat('Weekly')
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
          return val.toLocaleString();
        }
      },
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
    'JUL-21', '', '', 'OCT-21', '', '',
    'JAN-22', '', '', 'APR-22', '', '', 'JUL-22', '', '', 'OCT-22', '', '',
    'JAN-23', '', '', 'APR-23', '', '', 'JUL-23', '', ''
  ];
      
  var gasVolumeOptions = {
    chart: {
        type: 'bar',
      stacked: true
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
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
        format: getChartXAxisLabelFormat('Weekly')
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
      },
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
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
              },
        format: getChartXAxisLabelFormat('Weekly')
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
          return val.toLocaleString();
        }
      },
        decimalsInFloat: 3
    }]
  };
        
  refreshChart(electricityVolumeSeries, "#electricityVolumeChart", electricityVolumeOptions);
  refreshChart(electricityPriceSeries, "#electricityPriceChart", electricityPriceOptions);
  refreshChart(gasVolumeSeries, "#gasVolumeChart", gasVolumeOptions);
  refreshChart(gasPriceSeries, "#gasPriceChart", gasPriceOptions);
}

function showHideContainer(checkbox) {
  var commodity = checkbox.id.replace('Commoditycheckbox', '');

  if(commodity == "electricity") {
    electricityVolumeContainer.style.display = checkbox.checked ? "" : "none"
  }
  else {
    gasVolumeContainer.style.display = checkbox.checked ? "" : "none"
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
      colors: ['#61B82E', '#1CB89D', '#3C6B20', '#851B1E', '#C36265', '#104A6B', '#B8B537', '#B8252A', '#0B6B5B'],
      series: newSeries,
      yaxis: chartOptions.yaxis,
      xaxis: chartOptions.xaxis
    };  
  
    renderChart(chartId, options);
}