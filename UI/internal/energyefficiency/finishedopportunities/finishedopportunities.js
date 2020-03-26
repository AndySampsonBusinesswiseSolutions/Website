function pageLoad() {
  createTree(activeopportunity, "treeDiv", "");
  updateGraphs();
  showCumulativeSavingChart();  
  loadDataGrid();

  document.onmousemove=function(e) {
    var mousecoords = getMousePos(e);
    if(mousecoords.x <= 25) {
      openNav();
    }  
    else if(mousecoords.x >= 400) {
      closeNav();
    }  
};
}

function getMousePos(e) {
return {x:e.clientX,y:e.clientY};
}

function openNav() {
document.getElementById("mySidenav").style.width = "400px";
document.getElementById("openNav").style.color = "#b62a51";
}

function closeNav() {
document.getElementById("openNav").style.color = "white";
document.getElementById("mySidenav").style.width = "0px";
}

function loadDataGrid() {
	var opportunities = [];

	var row = {
		projectName:'LED Lighting <i class="fas fa-search show-pointer"></i>',
    site:'Site X',
    meter:'N/A',
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
  
  row = {
		projectName:'LED Lighting 2 <i class="fas fa-search show-pointer"></i>',
    site:'Site Y',
    meter:'N/A',
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
      {type:'text', width:'119px', name:'site', title:'Site', readOnly: true},
      {type:'text', width:'119px', name:'meter', title:'Meter', readOnly: true},
      {type:'text', width:'119px', name:'engineer', title:'Engineer', readOnly: true},
      {type:'text', width:'119px', name:'startDate', title:'Start Date', readOnly: true},
      {type:'text', width:'119px', name:'finishDate', title:'Finish Date', readOnly: true},
      {type:'text', width:'119px', name:'cost', title:'Cost', readOnly: true},
      {type:'text', width:'119px', name:'actualVolumeSavings', title:'Actual kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'actualCostSavings', title:'Actual £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'estimatedVolumeSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'netVolumeSavings', title:'Net kWh<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'netCostSavings', title:'Net £<br>Savings (pa)', readOnly: true},
      {type:'text', width:'119px', name:'totalROIMonths', title:'Total<br>ROI Months', readOnly: true},
      {type:'text', width:'119px', name:'remainingROIMonths', title:'Remaining<br>ROI Months', readOnly: true},
		 ]
    });													
}

function showCumulativeSavingChart() {
  updateClassOnClick('cumulativeSaving', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('cumulativeSaving'.concat('List'), 'listitem-hidden', '');
}

function updateGraphs() {
  var cumulativeSavingSeries = [{
    name: 'CAPEX/OPEX',
    type: 'bar',
      data: [
              -55000, 0, 0, 0, 0, 0, 
              0, 0, 0, 0, 0, 0, 
              0, 0, 0, 0, 0, 0,
              0, 0, 0, 0, 0, 0,
              0, 0, 0, 0, 0, 0,
              -10000, 0, 0, 0, 0, 0
            ]
    }, {
    name: 'Cumulative £ Saving',
    type: 'line',
      data: [
      -49583.3333333333,-44166.6666666667,-38750,-33333.3333333333,-27916.6666666667,-22500,
  -17083.3333333333,-11666.6666666667,-6250,-833.333333333338,4583.33333333333,10000,
  15416.6666666667,20833.3333333333,26250,31666.6666666667,37083.3333333333,42500,
  47916.6666666667,53333.3333333333,58750,64166.6666666667,69583.3333333333,75000,
  80416.6666666667,85833.3333333333,91250,96666.6666666667,102083.333333333,107500,
  107916.666666667,118333.333333333,128750,139166.666666667,149583.333333333,160000
  
            ]
    }, {
    name: 'LED Lighting - Site X',
    type: 'line',
      data: [
      -49583.33,-44166.66,-38749.99,-33333.32,-27916.65,-22499.98,
  -17083.31,-11666.64,-6249.97,-833.300000000003,4583.37,10000.04,
  15416.71,20833.38,26250.05,31666.72,37083.39,42500.06,
  47916.73,53333.4,58750.07,64166.74,69583.41,75000.08,
  80416.75,85833.42,91250.09,96666.76,102083.43,107500.1,
  112916.77,118333.44,123750.11,129166.78,134583.45,140000.12
            ]
    }, {
    name: 'LED Lighting - Site Y',
    type: 'line',
      data: [
      null,null,null,null,null,null,
      null,null,null,null,null,null,
      null,null,null,null,null,null,
      null,null,null,null,null,null,
      null,null,null,null,null,null,
      -5000,0,5000,10000,15000,20000
            ]
    }];
    
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
      '02 2017', '03 2017', '04 2017',
      '05 2017', '06 2017', '07 2017', '08 2017', '09 2017', '10 2017', '11 2017', '12 2017', '01 2018', '02 2018', '03 2018', '04 2018',
      '05 2018', '06 2018', '07 2018', '08 2018', '09 2018', '10 2018', '11 2018', '12 2018', '01 2019', '02 2019', '03 2019', '04 2019',
      '05 2019', '06 2019', '07 2019', '08 2019', '09 2019', '10 2019', '11 2019', '12 2019', '01 2020', '02 2020', '03 2020', '04 2020',
      '05 2020', '06 2020', '07 2020', '08 2020', '09 2020', '10 2020', '11 2020', '12 2020', '01 2021'
      ];
      
    var cumulativeSavingOptions = {
      chart: {
        type: 'line'
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
          format: getChartXAxisLabelFormat('Weekly')
          },
          categories: electricityCategories
      },
      yaxis: [{
        title: {
          text: '£'
        },
      decimalsInFloat: 0
      }]
    };
  
    var costSavingOptions = {
      chart: {
        type: 'bar',
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
        text: "£"
      },
          min: 3000,
          max: 6000,
          decimalsInFloat: 0
      }]
    };
  
    var volumeSavingOptions = {
      chart: {
        type: 'bar',
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
        text: "kWh"
      },
          min: 3000,
          max: 6000,
          decimalsInFloat: 0
      }]
    };
    
    refreshChart(cumulativeSavingSeries, "#cumulativeSavingChart", cumulativeSavingOptions);
    refreshChart(costSavingSeries, "#costSavingChart", costSavingOptions);
    refreshChart(volumeSavingSeries, "#volumeSavingChart", volumeSavingOptions);
}

var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    var order = $("input[type='radio'][name='group1']:checked").val();
    if(order == "Project") {
        buildTree(baseData, ul, checkboxFunction);
    }
    else {
        buildSiteProjectTree(baseData, ul, checkboxFunction);
    }

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);

    addExpanderOnClickEvents();
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'ProjectName');
        var li = document.createElement('li');
        var ul = createUL();

        buildSite(base.Sites, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'ProjectName'.concat(base.GUID), checkboxFunction, 'ProjectName', baseName, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function buildSiteProjectTree(baseData, baseElement, checkboxFunction) {
  var siteNames = [];
  var sites = [];

  var dataLength = baseData.length;
  for(var i = 0; i < dataLength; i++){
      var project = baseData[i];
      var siteLength = project.Sites.length;

      for(var j = 0; j < siteLength; j++) {
          if(!siteNames.includes(project.Sites[j].SiteName)) {
              siteNames.push(project.Sites[j].SiteName);
              sites.push(project.Sites[j]);
          }
      }
  }

  dataLength = sites.length;
  for(var i = 0; i < dataLength; i++) {
      var base = sites[i];
      var li = document.createElement('li');
      var ul = createUL();
      
      var projectNames = [];
      var projects = [];

      for(var j = 0; j < baseData.length; j++){
          var project = baseData[j];
          var projectName = getAttribute(project.Attributes, 'ProjectName');
          var siteLength = project.Sites.length;

          for(var k = 0; k < siteLength; k++) {
              if(project.Sites[k].SiteName == base.SiteName) {
                  if(!projectNames.includes(projectName)) {
                      projectNames.push(projectName);
                      projects.push({project: project, meters: project.Sites[k].Meters});
                  }
              }
          }
      }

      buildProject(projects, ul, checkboxFunction, base.SiteName);        
      appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', siteNames[i], ul, siteNames[i], base.GUID);

      baseElement.appendChild(li); 
  }
}

function buildProject(projects, baseElement, checkboxFunction, linkedSite) {
  var projectsLength = projects.length;
  for(var j = 0; j < projectsLength; j++){
      var project = projects[j].project;
      var projectName = getAttribute(project.Attributes, 'ProjectName');
      var li = document.createElement('li');
      var ul = createUL();
      var branchId = 'Project'.concat(project.GUID).concat(linkedSite.replace(' ', ''));

      buildMeter(projects[j].meters, ul, checkboxFunction, linkedSite);
      appendListItemChildren(li, branchId, checkboxFunction, 'Project', projectName, ul, linkedSite, '');

      baseElement.appendChild(li); 
  }
}

function buildSite(sites, baseElement, checkboxFunction, linkedSite) {
    var sitesLength = sites.length;
    for(var i = 0; i < sitesLength; i++) {
        var site = sites[i];
        var li = document.createElement('li');
        var ul = createUL();
        buildMeter(site.Meters, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Site'.concat(subBranchCount), checkboxFunction, 'Site', site.SiteName, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function buildMeter(meters, baseElement, checkboxFunction, linkedSite) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        var li = document.createElement('li');
        var ul = createUL();
        var branchId = 'Meter'.concat(meter.GUID);
        appendListItemChildren(li, branchId, checkboxFunction, 'Meter', meter.Identifier, ul, linkedSite, '');

        var branchDiv = li.children[branchId];
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        baseElement.appendChild(li); 
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createTreeIcon(checkboxBranch));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    branchDiv.setAttribute('class', 'far fa-plus-square');
    branchDiv.setAttribute('style', 'padding-right: 4px;');
    return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
    var branchListDiv = document.createElement('div');
    branchListDiv.id = branchListDivId;
    branchListDiv.setAttribute('class', 'listitem-hidden');
    branchListDiv.appendChild(ul);
    return branchListDiv;
}

function createUL() {
    var ul = document.createElement('ul');
    ul.setAttribute('class', 'format-listitem');
    return ul;
}

function createTreeIcon(branch) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch));
    icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
    return icon;
}

function createSpan(spanId, innerHTML) {
    var span = document.createElement('span');
    span.id = spanId.concat('span');
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid) {
    var functionArray = checkboxFunction.replace(')', '').split('(');
    var functionArrayLength = functionArray.length;
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement('input');
    checkBox.type = 'checkbox';  
    checkBox.id = checkboxId.concat('checkbox');
    checkBox.setAttribute('Branch', branch);
    checkBox.setAttribute('LinkedSite', linkedSite);
    checkBox.setAttribute('GUID', guid);

    functionArguments.push(checkBox.id);
    if(functionArrayLength > 1) {
        var functionArgumentLength = functionArray[1].split(',').length;
        for(var i = 0; i < functionArgumentLength; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
    return checkBox;
}

function getIconByBranch(branch) {
    switch(branch) {
        case 'Period':
            return "far fa-calendar-alt";
        case "BillValid":
            return "fas fa-check-circle";
        case "BillInvestigation":
            return "fas fa-question-circle";
        case "BillInvalid":
            return "fas fa-times-circle";
    }    
}

function getAttribute(attributes, attributeRequired) {
	for (var attribute in attributes) {
		var array = attributes[attribute];

		for(var key in array) {
			if(key == attributeRequired) {
				return array[key];
			}
		}
	}

	return null;
}

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

function updateClassOnClick(elementId, firstClass, secondClass){
	var elements = document.getElementsByClassName(elementId);

	if(elements.length == 0) {
		var element = document.getElementById(elementId);
		updateClass(element, firstClass, secondClass);
	}
	else {
		for(var i = 0; i< elements.length; i++) {
			updateClass(elements[i], firstClass, secondClass)
		}
	}
}

function updateClass(element, firstClass, secondClass)
{
	if(hasClass(element, firstClass)){
		element.classList.remove(firstClass);

		if(secondClass != ''){
			element.classList.add(secondClass);
		}
	}
	else {
		if(secondClass != ''){
			element.classList.remove(secondClass);
		}
		
		element.classList.add(firstClass);
	}
}
  
function hasClass(elem, className) {
	return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}

function addExpanderOnClickEvents() {
	var expanders = document.getElementsByClassName('fa-plus-square');
	var expandersLength = expanders.length;
	for(var i = 0; i < expandersLength; i++){
		addExpanderOnClickEventsByElement(expanders[i]);
	}
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square')
		updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '')
	});
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

  function renderChart(chartId, options) {
    var chart = new ApexCharts(document.querySelector(chartId), options);
    chart.render();
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