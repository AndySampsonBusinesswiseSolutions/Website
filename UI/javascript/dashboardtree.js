var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();

        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createSpan(id, branchOption));
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;
    return branchDiv;
}

function createUL() {
    var ul = document.createElement('ul');
    ul.setAttribute('class', 'format-listitem');
    return ul;
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

function addDashboardItem(checkbox) {
    var dashboard = document.getElementById('containment-wrapper');
    var guid = checkbox.getAttribute('guid');
    var dashboardItemId = 'dashboardItem'.concat(guid);

    if(checkbox.checked) {
        var newDashboardItem = document.createElement('div');
        newDashboardItem.id = dashboardItemId;
        newDashboardItem.setAttribute('class', 'dragable ui-widget-content');
        $(newDashboardItem).draggable({ containment: "#containment-wrapper", scroll: false, snap: true, cursor: "move" });

        var width;
        var height;
        var dataLength = data.length;
        for(var i = 0; i < dataLength; i++) {
            var item = data[i];

            if(item.GUID == guid) {
                width = getAttribute(item.Attributes, "Width");
                height = getAttribute(item.Attributes, "Height");
                break;
            }
        }

        newDashboardItem.setAttribute('style', 'width: ' + width + '; height: ' + height + ';');
        newDashboardItem.innerText = dashboardItemId;
        dashboard.appendChild(newDashboardItem);

        if(guid == 1) {
            var flexElectricityPositionItem = document.createElement('div');
            flexElectricityPositionItem.id = 'electricityVolumeChart';

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

              var electricityCategories = [
                '10 2020', '11 2020', '12 2020',
                '01 2021', '02 2021', '03 2021', '04 2021', '05 2021', '06 2021', '07 2021', '08 2021', '09 2021', '10 2021', '11 2021', '12 2021',
                '01 2022', '02 2022', '03 2022', '04 2022', '05 2022', '06 2022', '07 2022', '08 2022', '09 2022', '10 2022', '11 2022', '12 2022',
                '01 2023', '02 2023', '03 2023', '04 2023', '05 2023', '06 2023', '07 2023', '08 2023', '09 2023'
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
                    format: getChartXAxisLabelFormat('Weekly')
                    },
                    categories: electricityCategories
                },
                yaxis: [{
                  title: {
                    text: 'MW'
                  },
                    min: 0,
                    max: 3,
                    decimalsInFloat: 3
                }]
              };

            newDashboardItem.appendChild(flexElectricityPositionItem);
            refreshChart(electricityVolumeSeries, electricityCategories, "#electricityVolumeChart", electricityVolumeOptions);
        }
    }
    else {
        var dashboardItem = document.getElementById(dashboardItemId);
        dashboard.removeChild(dashboardItem);
    }
}