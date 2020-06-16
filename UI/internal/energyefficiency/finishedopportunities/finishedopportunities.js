function pageLoad() {
  createGroupingOptionTree();
  createTree("updateGraphs()");
  updateGraphs();
}

function resetPage() {
  projectLocationcheckbox.checked = true;
  siteLocationcheckbox.checked = true;
  meterLocationcheckbox.checked = true;
  pageLoad();
}

function createGroupingOptionTree() {
  var div = document.getElementById('groupingOptionTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("groupingOptionHeader", "Grouping Option", true);
  var ul = createBranchUl("groupingOptionSelector", false, true);

  var groupingOption2GroupingOptionListItem = appendListItemChildren('groupingOption2GroupingOptionSelector', false, 'updateGraphs()', [{"Name" : "No Grouping"}], 'groupingOptionSelector', false, 'radio', 'groupingOptionGroup');
  var groupingOption1GroupingOptionListItem = appendListItemChildren('groupingOption1GroupingOptionSelector', false, 'updateGraphs()', [{"Name" : "Group"}], 'groupingOptionSelector', true, 'radio', 'groupingOptionGroup');

  ul.appendChild(groupingOption2GroupingOptionListItem);
  ul.appendChild(groupingOption1GroupingOptionListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function loadDataGrid() {
  clearElement(document.getElementById('spreadsheet'));
	var opportunities = [];

  var row = null;
  
  if(Project13checkbox.checked 
    || Site114checkbox.checked
    || Meter14checkbox.checked) {
    row = {
      projectName:'LED Lighting',
      site:'Site X',
      meter:'1234567890123',
      engineer:'En Gineer',
      startDate:'01/01/2017',
      finishDate:'28/02/2017',
      cost:'£55,000',
      actualVolumeSavings:'10,000',
      actualCostSavings:'£65,000',
      estimatedVolumeSavings: '9,000',
      estimatedCostSavings:'£60,000',
      netVolumeSavings:'45,000',
      netCostSavings:'£140,000',
      totalROIMonths:'9',
      remainingROIMonths:'0'
    }
    opportunities.push(row);
  }
  
  if(Project15checkbox.checked 
    || Site116checkbox.checked
    || Meter16checkbox.checked) {
    row = {
      projectName:'LED Lighting 2',
      site:'Site A',
      meter:'1234567890124',
      engineer:'En Gineer',
      startDate:'01/08/2019',
      finishDate:'15/08/2019',
      cost:'£10,000',
      actualVolumeSavings:'5,000',
      actualCostSavings:'£60,000',
      estimatedVolumeSavings: '9,000',
      estimatedCostSavings:'£160,000',
      netVolumeSavings:'45,000',
      netCostSavings:'£120,000',
      totalROIMonths:'2',
      remainingROIMonths:'0'
    }
    opportunities.push(row);
  }  

	jexcel(document.getElementById('spreadsheet'), {
		pagination:10,
    data: opportunities,
    allowInsertRow: false,
    allowManualInsertRow: false,
    allowInsertColumn: false,
    allowManualInsertColumn: false,
    allowDeleteRow: false,
    allowDeleteColumn: false,
    allowRenameColumn: false,
    wordWrap: true,
		columns: [
      {type:'text', width:'150px', name:'projectName', title:'Project Name', readOnly: true},
      {type:'text', width:'118px', name:'site', title:'Site', readOnly: true},
      {type:'text', width:'128px', name:'meter', title:'Meter', readOnly: true},
      {type:'text', width:'118px', name:'engineer', title:'Engineer', readOnly: true},
      {type:'text', width:'118px', name:'startDate', title:'Start Date', readOnly: true},
      {type:'text', width:'118px', name:'finishDate', title:'Finish Date', readOnly: true},
      {type:'text', width:'118px', name:'cost', title:'Cost', readOnly: true},
      {type:'text', width:'118px', name:'actualVolumeSavings', title:'Actual kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'actualCostSavings', title:'Actual £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'estimatedVolumeSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'netVolumeSavings', title:'Net kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'netCostSavings', title:'Net £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'118px', name:'totalROIMonths', title:'Total<br>ROI Months', readOnly: true},
      {type:'text', width:'118px', name:'remainingROIMonths', title:'Remaining<br>ROI Months', readOnly: true},
		 ]
    });													
}

function updateGraphs() {
  var cumulativeSavingSeries = [];

  if(capexFinancialsSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('CAPEX', 'bar', 'CAPEX'));
  }

  if(opexFinancialsSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('OPEX', 'bar', 'OPEX'));
  }

  if(savingsFinancialsSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('Savings', 'line', 'Savings'));
  }

  if(cumulativeSavingsFinancialsSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('Cumulative Savings', 'line', 'CumulativeSavings'));
  }

  if(includeCAPEXCumulativeSavingSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('Cumulative Savings Inc CAPEX', 'line', 'CumulativeSavingsIncludingCAPEX'));
  }

  if(includeOPEXCumulativeSavingSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('Cumulative Savings Inc OPEX', 'line', 'CumulativeSavingsIncludingOPEX'));
  }

  if(includeCAPEXCumulativeSavingSelectorcheckbox.checked && includeOPEXCumulativeSavingSelectorcheckbox.checked) {
    cumulativeSavingSeries.push(...createDisplayData('Cumulative Savings Inc CAPEX & OPEX', 'line', 'CumulativeSavingsIncludingCAPEXAndOPEX'));
  }

  var costSavingSeries = [{
      name: 'Estimated Cost Saving',
      data: [
                4718,4718,4718,4718,4718,4718,4718,4718,
                3888,3888,3888,3888,3888,3888,3888,3888,
                4536,4536,4536,4536,4536,4536,4536,4536,
                3905,3905,3905,3905,3905,3905,3905,3905,
                4558,4558,4558,4558,4558,4558,4558,4558,
                3907,3907,3907,3907,3907,3907,3907,3907,
              ]
    },{
      name: 'Actual Cost Saving',
      data: [
                5718,5718,5718,5718,5718,5718,
                4888,4888,4888,4888,4888,4888,
                5536,5536,5536,5536,5536,5536,
                4905,4905,4905,4905,4905,4905,
                5558,5558,5558,5558,5558,5558,
                4907,4907,4907,4907,4907,4907
              ]
    }];

  var volumeSavingSeries = [
    {
      name: 'Estimated kWh Saving',
      data: [
                3990,3990,3990,3990,3990,3990,3990,3990,
                3310,3310,3310,3310,3310,3310,3310,3310,
                4030,4030,4030,4030,4030,4030,4030,4030,
                3340,3340,3340,3340,3340,3340,3340,3340,
                4010,4010,4010,4010,4010,4010,4010,4010,
                3350,3350,3350,3350,3350,3350,3350,3350,
            ]
    }, {
      name: 'Actual kWh Saving',
      data: [
                4990,4990,4990,4990,4990,4990,
                4310,4310,4310,4310,4310,4310,
                5030,5030,5030,5030,5030,5030,
                4340,4340,4340,4340,4340,4340,
                5010,5010,5010,5010,5010,5010,
                4350,4350,4350,4350,4350,4350,
            ]
    }];

  var electricityCategories = [
    'FEB-17', 'MAR-17', 'APR-17', 'MAY-17', 'JUN-17', 'JUL-17', 
    'AUG-17', 'SEP-17', 'OCT-17', 'NOV-17', 'DEC-17', 'JAN-18', 
    'FEB-18', 'MAR-18', 'APR-18', 'MAY-18', 'JUN-18', 'JUL-18', 
    'AUG-18', 'SEP-18', 'OCT-18', 'NOV-18', 'DEC-18', 'JAN-19', 
    'FEB-19', 'MAR-19', 'APR-19', 'MAY-19', 'JUN-19', 'JUL-19', 
    'AUG-19', 'SEP-19', 'OCT-19', 'NOV-19', 'DEC-19', 'JAN-20', 
    'FEB-20', 'MAR-20', 'APR-20', 'MAY-20', 'JUN-20', 'JUL-20', 
    'AUG-20', 'SEP-20', 'OCT-20', 'NOV-20', 'DEC-20', 'JAN-21'
    ];
    
  var cumulativeSavingOptions = {
    chart: {
      type: 'line'
    },
    title: {
      text: 'Project Financials'
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
        text: '£'
      },
      forceNiceScale: true,
    labels: {
      formatter: function(val) {
        return '£' + val.toLocaleString();
      }
    },
    decimalsInFloat: 0
    }]
  };

  var costSavingOptions = {
    chart: {
      type: 'bar',
      stacked: false
    },
    title: {
      text: 'Cost Savings'
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
    yaxis: [
    {
    show: true,
    axisTicks: {
      show: false,
    },
    labels: {
      style: {
      color: '#00E396',
      }
    },
    title: {
      style: {
        fontSize: '10px',
        fontFamily: 'Helvetica, Arial, sans-serif',
        fontWeight: 400,
      },
      text: "£"
    },
    forceNiceScale: true,
    labels: {
      formatter: function(val) {
        return '£' + val.toLocaleString();
      }
    },
    }]
  };

  var volumeSavingOptions = {
    chart: {
      type: 'bar',
      stacked: false
    },
    title: {
      text: 'Volume Savings'
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
    yaxis: [
    {
    show: true,
    axisTicks: {
      show: false,
    },
    labels: {
      style: {
      color: '#00E396',
      }
    },
    title: {
      style: {
        fontSize: '10px',
        fontFamily: 'Helvetica, Arial, sans-serif',
        fontWeight: 400,
      },
      text: "kWh"
    },
    forceNiceScale: true,
    labels: {
      formatter: function(val) {
        return val.toLocaleString();
      }
    },
    }]
  };
  
  refreshChart(cumulativeSavingSeries, "#cumulativeSavingChart", cumulativeSavingOptions);
  refreshChart(costSavingSeries, "#costSavingChart", costSavingOptions);
  refreshChart(volumeSavingSeries, "#volumeSavingChart", volumeSavingOptions);

  loadDataGrid();
}

function createDisplayData(name, type, attribute) {
  var displayData = [];

  var site1Selected = Project13checkbox.checked 
  || Site114checkbox.checked
  || Meter14checkbox.checked;
  var site2Selected = Project15checkbox.checked 
  || Site116checkbox.checked
  || Meter16checkbox.checked;

  if(groupingOption1GroupingOptionSelectorradio.checked) {
    var data = [];

    if(site1Selected && !site2Selected) {
      data = JSON.parse(JSON.stringify(finishedopportunitySite[0].Projects[0].Meters[0][attribute]));
    }
    else if(!site1Selected && site2Selected) {
      data = JSON.parse(JSON.stringify(finishedopportunitySite[1].Projects[0].Meters[0][attribute]));
    }
    else if(site1Selected && site2Selected) {
      data = JSON.parse(JSON.stringify(finishedopportunitySite[0].Projects[0].Meters[0][attribute]));

      for(var i = 0; i < finishedopportunitySite[1].Projects[0].Meters[0][attribute].length; i++) {
        if(finishedopportunitySite[1].Projects[0].Meters[0][attribute][i] != null) {
          if(data[i] == null) {
            data[i] = JSON.parse(JSON.stringify(finishedopportunitySite[1].Projects[0].Meters[0][attribute][i]));
          }
          else {
            data[i] += JSON.parse(JSON.stringify(finishedopportunitySite[1].Projects[0].Meters[0][attribute][i]));
          }
        }
      }
    }

    displayData.push({
      name: name,
      type: type,
      data: data
    });
  }
  else {
    if(site1Selected) {
      displayData.push({
        name: name + ' - LED Lighting - Site X',
        type: type,
        data: JSON.parse(JSON.stringify(finishedopportunitySite[0].Projects[0].Meters[0][attribute]))
      });
    }

    if(site2Selected) {
      displayData.push({
        name: name + ' - LED Lighting 2 - Site A',
        type: type,
        data: JSON.parse(JSON.stringify(finishedopportunitySite[1].Projects[0].Meters[0][attribute]))
      });
    }
  }

  return displayData;
}

function createTree(functions) {
  var div = document.getElementById("siteTree");
  clearElement(div);
  
  var tree = document.createElement('div');
  tree.setAttribute('class', 'scrolling-wrapper');

  var headerDiv = createHeaderDiv("siteHeader", 'Location', true);
  var ul = createBranchUl("siteSelector", false, true);

  tree.appendChild(ul);

  var order = $("input[type='radio'][name='group1']:checked").val();
  if(order == "Project") {
    buildProjectSiteProjectBranch(finishedopportunityProject, ul, functions);
  }
  else {
    buildSiteProjectSiteBranch(finishedopportunitySite, ul, functions);
  }

  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';

  ul.appendChild(breakDisplayListItem);
  ul.appendChild(buildFinancialsBranch());  

  div.appendChild(headerDiv);
  div.appendChild(tree);

  addExpanderOnClickEvents();
  setOpenExpanders();
}

function buildFinancialsBranch() {
  var financialsListItem = appendListItemChildren('financialsSelector', true, '', [{"Name" : "Project Financials"}], 'financialsSelector', true, 'checkbox', 'financialsGroup');
  financialsListItem.id = "financialsTree";

  var financialsSelectorListUl = financialsListItem.getElementsByTagName('ul')[0];
  var capexFinancialsListItem = appendListItemChildren('capexFinancialsSelector', false, 'updateGraphs()', [{"Name" : "CAPEX"}], 'financialsSelector', true, 'checkbox', 'selectSpecificFinancialsGroup');
  var opexFinancialsListItem = appendListItemChildren('opexFinancialsSelector', false, 'updateGraphs()', [{"Name" : "OPEX"}], 'financialsSelector', true, 'checkbox', 'selectSpecificFinancialsGroup');
  var savingsFinancialsListItem = appendListItemChildren('savingsFinancialsSelector', false, 'updateGraphs()', [{"Name" : "Savings"}], 'financialsSelector', true, 'checkbox', 'selectSpecificFinancialsGroup');
  var cumulativeSavingsFinancialsListItem = appendListItemChildren('cumulativeSavingsFinancialsSelector', true, 'updateGraphs()', [{"Name" : "Cumulative Savings"}], 'financialsSelector', true, 'checkbox', 'selectSpecificFinancialsGroup');
  
  financialsSelectorListUl.appendChild(capexFinancialsListItem);
  financialsSelectorListUl.appendChild(opexFinancialsListItem);
  financialsSelectorListUl.appendChild(savingsFinancialsListItem);
  financialsSelectorListUl.appendChild(cumulativeSavingsFinancialsListItem);

  var cumulativeSavingsFinancialsSelectorListUl = cumulativeSavingsFinancialsListItem.getElementsByTagName('ul')[0];
  var includeCAPEXCumulativeSavingListItem = appendListItemChildren('includeCAPEXCumulativeSavingSelector', false, 'updateGraphs()', [{"Name" : "Include CAPEX"}], 'invoiceSelector', true, 'checkbox', 'selectSpecificInvoiceGroup');
  var includeOPEXCumulativeSavingListItem = appendListItemChildren('includeOPEXCumulativeSavingSelector', false, 'updateGraphs()', [{"Name" : "Include OPEX"}], 'invoiceSelector', true, 'checkbox', 'selectSpecificInvoiceGroup');

  cumulativeSavingsFinancialsSelectorListUl.appendChild(includeCAPEXCumulativeSavingListItem);
  cumulativeSavingsFinancialsSelectorListUl.appendChild(includeOPEXCumulativeSavingListItem);

  return financialsListItem;
}

function buildProjectSiteProjectBranch(projects, elementToAppendTo, functions) {
  var projectLength = projects.length;

  if(projectLocationcheckbox.checked) {
    for(var projectCount = 0; projectCount < projectLength; projectCount++) {
      var project = projects[projectCount];
      var listItem = appendListItemChildren('Project' + getAttribute(project.Attributes, "GUID"), project.hasOwnProperty('Sites'), functions, project.Attributes, 'Project', true);
      elementToAppendTo.appendChild(listItem);

      if(project.hasOwnProperty('Sites')) {
        var ul = listItem.getElementsByTagName('ul')[0];
        buildProjectSiteSiteBranch(project.Sites, ul, functions);
      }
    }
  }
  else {
    var sites = [];
    for(var projectCount = 0; projectCount < projectLength; projectCount++) {
      var project = projects[projectCount];
      sites.push(...project.Sites);
    }

    buildProjectSiteSiteBranch(sites, elementToAppendTo, functions);
  }
}

function buildProjectSiteSiteBranch(sites, elementToAppendTo, functions) {
  var siteLength = sites.length;

  if(siteLocationcheckbox.checked) {
    for(var siteCount = 0; siteCount < siteLength; siteCount++) {
      var site = sites[siteCount];
      var listItem = appendListItemChildren('Site' + getAttribute(site.Attributes, "GUID"), site.hasOwnProperty('Meters'), functions, site.Attributes, 'Site', true);
      elementToAppendTo.appendChild(listItem);

      if(site.hasOwnProperty('Meters')) {
        var ul = listItem.getElementsByTagName('ul')[0];
        buildMeterBranch(site.Meters, ul, functions);
      }
    }
  }
  else {
    var meters = [];
    for(var siteCount = 0; siteCount < siteLength; siteCount++) {
      var site = sites[siteCount];
      meters.push(...site.Meters);
    }

    buildMeterBranch(meters, elementToAppendTo, functions);
  }
}

function buildSiteProjectSiteBranch(sites, elementToAppendTo, functions) {
  var siteLength = sites.length;

  if(siteLocationcheckbox.checked) {
    for(var siteCount = 0; siteCount < siteLength; siteCount++) {
      var site = sites[siteCount];
      var listItem = appendListItemChildren('Site' + getAttribute(site.Attributes, "GUID"), site.hasOwnProperty('Projects'), functions, site.Attributes, 'Site', true);
      elementToAppendTo.appendChild(listItem);

      if(site.hasOwnProperty('Projects')) {
        var ul = listItem.getElementsByTagName('ul')[0];
        buildSiteProjectProjectBranch(site.Projects, ul, functions);
      }
    }
  }
  else {
    var projects = [];
    for(var siteCount = 0; siteCount < siteLength; siteCount++) {
      var site = sites[siteCount];
      projects.push(...site.Projects);
    }

    buildSiteProjectProjectBranch(projects, elementToAppendTo, functions);
  }
}

function buildSiteProjectProjectBranch(projects, elementToAppendTo, functions) {
  var projectLength = projects.length;

  if(projectLocationcheckbox.checked) {
    for(var projectCount = 0; projectCount < projectLength; projectCount++) {
      var project = projects[projectCount];
      var listItem = appendListItemChildren('Project' + getAttribute(project.Attributes, "GUID"), project.hasOwnProperty('Meters'), functions, project.Attributes, 'Project', true);
      elementToAppendTo.appendChild(listItem);

      if(project.hasOwnProperty('Meters')) {
        var ul = listItem.getElementsByTagName('ul')[0];
        buildMeterBranch(project.Meters, ul, functions);
      }
    }
  }
  else {
    var meters = [];
    for(var projectCount = 0; projectCount < projectLength; projectCount++) {
      var project = projects[projectCount];
      meters.push(...project.Meters);
    }

    buildMeterBranch(meters, elementToAppendTo, functions);
  }
}

function buildMeterBranch(meters, elementToAppendTo, functions) {
  if(!meterLocationcheckbox.checked) {
    return;
  }

  var meterLength = meters.length;
  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = meters[meterCount];
    var listItem = appendListItemChildren('Meter' + getAttribute(meter.Attributes, "GUID"), false, functions, meter.Attributes, 'Meter', true)
    elementToAppendTo.appendChild(listItem);
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
      title: {
        text: chartOptions.title.text,
        align: 'center',
        style: {
          fontSize: '25px',
          fontFamily: 'Arial, Helvetica, sans-serif',
          fontWeight: 'normal',
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
      colors: ['#69566c', '#61B82E', '#1CB89D', '#3C6B20', '#851B1E', '#C36265', '#104A6B', '#B8B537', '#B8252A', '#0B6B5B'],
      series: newSeries,
      yaxis: chartOptions.yaxis,
      xaxis: chartOptions.xaxis
    };  
  
    renderChart(chartId, options);
}