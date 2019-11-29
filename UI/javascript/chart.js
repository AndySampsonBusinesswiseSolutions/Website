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
  var finalColumns = document.getElementsByClassName("final-column");
  var chartWidth = window.innerWidth - windowWidthReduction;

  for(var i=0; i<finalColumns.length; i++){
    finalColumns[i].setAttribute("style", "width: "+chartWidth+"px;");
  }
}

function renderChart(chartId, options) {
  var chart = new ApexCharts(document.querySelector(chartId), options);
  chart.render();
}

function refreshChart(newSeries, newCategories, chartId) {
  var options = {
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
      },
      animations: {
        enabled: false
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
      min: new Date('2019-11-01').getTime(),
      max: new Date('2019-11-02').getTime(),
      categories: newCategories
    }
  };  

  renderChart(chartId, options);
}

function testChart(chart) {
    var checkBoxes = [];
		var treeDiv = document.getElementById(chart.id.replace('Chart', 'TreeDiv'));
    var inputs = treeDiv.getElementsByTagName('input');

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
			createBlankChart("#electricityChart", "There's no electricity data to display. Select from the tree to the left to display");
			return;
		}

    var showBySpan = document.getElementById('electricityChartHeaderShowBy');
    var showBy = showBySpan.children[0].value;
		var newSeries = [];
		var newCategories = [
			"2019-11-01 00:00:00", "2019-11-01 00:30:00", "2019-11-01 01:00:00", "2019-11-01 01:30:00",
			"2019-11-01 02:00:00", "2019-11-01 02:30:00", "2019-11-01 03:00:00", "2019-11-01 03:30:00",
			"2019-11-01 04:00:00", "2019-11-01 04:30:00", "2019-11-01 05:00:00", "2019-11-01 05:30:00",
			"2019-11-01 06:00:00", "2019-11-01 06:30:00", "2019-11-01 07:00:00", "2019-11-01 07:30:00",
			"2019-11-01 08:00:00", "2019-11-01 08:30:00", "2019-11-01 09:00:00", "2019-11-01 09:30:00",
			"2019-11-01 10:00:00", "2019-11-01 10:30:00", "2019-11-01 11:00:00", "2019-11-01 11:30:00",
			"2019-11-01 12:00:00", "2019-11-01 12:30:00", "2019-11-01 13:00:00", "2019-11-01 13:30:00",
			"2019-11-01 14:00:00", "2019-11-01 14:30:00", "2019-11-01 15:00:00", "2019-11-01 15:30:00",
			"2019-11-01 16:00:00", "2019-11-01 16:30:00", "2019-11-01 17:00:00", "2019-11-01 17:30:00",
			"2019-11-01 18:00:00", "2019-11-01 18:30:00", "2019-11-01 19:00:00", "2019-11-01 19:30:00",
			"2019-11-01 20:00:00", "2019-11-01 20:30:00", "2019-11-01 21:00:00", "2019-11-01 21:30:00",
			"2019-11-01 22:00:00", "2019-11-01 22:30:00", "2019-11-01 23:00:00", "2019-11-01 23:30:00"
		];

		for(var i = 0; i < checkBoxes.length; i++) {
			if(checkBoxes[i].id.includes('Site')) {
				var span = document.getElementById(checkBoxes[i].id.replace('checkbox', 'span'));
				
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
					if(meters[meterCount].Commodity == "Electricity") {
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
		}		

		refreshChart(newSeries, newCategories, "#electricityChart");
}