function initialiseChart(chartId, noDataText) {
    var options = {
        chart: {
            height: '100%',
            width: '100%',
          type: 'line'
        },
        xaxis: {
          categories: [1991,1992,1993,1994,1995,1996,1997, 1998,1999]
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