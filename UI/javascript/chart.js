function initialiseChart(chartId, noDataText) {
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
      
      var chart = new ApexCharts(document.querySelector(chartId), options);
      
      chart.render();
}

function resizeCharts(windowWidthReduction){
  var finalColumns = document.getElementsByClassName("final-column");
  var chartWidth = window.innerWidth - windowWidthReduction;

  for(var i=0; i<finalColumns.length; i++){
    finalColumns[i].setAttribute("style", "width: "+chartWidth+"px;");
  }
}

function addEnergyToChart() {
  var energy = data[0]["Meters"][0]["Energy"];
  var newSeries = [
    {
      name: "Meter",
      data: []
    }
  ];
  var newCategories = [
    "2019-11-26 00:00:00", "2019-11-26 00:30:00", "2019-11-26 01:00:00", "2019-11-26 01:30:00",
    "2019-11-26 02:00:00", "2019-11-26 02:30:00", "2019-11-26 03:00:00", "2019-11-26 03:30:00",
    "2019-11-26 04:00:00", "2019-11-26 04:30:00", "2019-11-26 05:00:00", "2019-11-26 05:30:00",
    "2019-11-26 06:00:00", "2019-11-26 06:30:00", "2019-11-26 07:00:00", "2019-11-26 07:30:00",
    "2019-11-26 08:00:00", "2019-11-26 08:30:00", "2019-11-26 09:00:00", "2019-11-26 09:30:00",
    "2019-11-26 10:00:00", "2019-11-26 10:30:00", "2019-11-26 11:00:00", "2019-11-26 11:30:00",
    "2019-11-26 12:00:00", "2019-11-26 12:30:00", "2019-11-26 13:00:00", "2019-11-26 13:30:00",
    "2019-11-26 14:00:00", "2019-11-26 14:30:00", "2019-11-26 15:00:00", "2019-11-26 15:30:00",
    "2019-11-26 16:00:00", "2019-11-26 16:30:00", "2019-11-26 17:00:00", "2019-11-26 17:30:00",
    "2019-11-26 18:00:00", "2019-11-26 18:30:00", "2019-11-26 19:00:00", "2019-11-26 19:30:00",
    "2019-11-26 20:00:00", "2019-11-26 20:30:00", "2019-11-26 21:00:00", "2019-11-26 21:30:00",
    "2019-11-26 22:00:00", "2019-11-26 22:30:00", "2019-11-26 23:00:00", "2019-11-26 23:30:00"
  ];

  for(var i = 0; i < newCategories.length; i++) {
    var value = null;

    for(var j = 0; j < energy.length; j++) {
      if(energy[j].Date == newCategories[i]) {
        value = energy[j].MWh;
        break;
      }
    }

    newSeries[0].data.push(value);
  }

  var newOptions = {
    chart: {
        height: '100%',
        width: '100%',
      type: 'line',
      zoom: {
        type: 'x',
        enabled: true,
        autoScaleYaxis: true
      },
      dataLabels: {
        enabled: false
      },
      toolbar: {
        autoSelected: 'zoom',
        tools: {
          download: false
        }        
      }
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
        text: 'Energy (MWh)'
      },
      show: true
    },
    xaxis: {
      type: 'datetime',
      title: {
        text: 'Date'
      },
      labels: {
          format: 'HH:mm'
      },
      min: new Date('2019-11-26').getTime(),
      max: new Date('2019-11-27').getTime(),
      categories: newCategories
    }
  };
  
  var div = document.getElementById("electricityChart");

  while (div.firstChild) {
      div.removeChild(div.firstChild);
  }

  var newChart = new ApexCharts(document.querySelector("#electricityChart"), newOptions);
  
  newChart.render();
}