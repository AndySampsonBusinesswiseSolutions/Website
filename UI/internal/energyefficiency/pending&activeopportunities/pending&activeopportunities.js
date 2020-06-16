function pageLoad() {
    createTree(activeopportunity, "siteTree", "updateGanttChartAndDataGrid()", true);
    updateGanttChartAndDataGrid();
    setOpenExpanders();
}

function createTree(baseData, divId, checkboxFunction, isPageLoading = false) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');

    var order = $("input[type='radio'][name='group1']:checked").val();
    var headerDiv = createHeaderDiv("siteHeader", order == "Project" ? 'Project' : 'Location', true);
    var ul = createBranchUl("siteSelector", false, true);

    tree.appendChild(ul);

    if(order == "Project") {
        buildTree(baseData, ul, checkboxFunction);
    }
    else {
        buildSiteProjectTree(baseData, ul, checkboxFunction);
    }

    document.getElementById("locationSelectorSpan").innerText = order == "Project" ? 'Project Visibility' : 'Location Visibility';
    var div = document.getElementById(divId);
    clearElement(div);

    div.appendChild(headerDiv);
    div.appendChild(tree);

    addExpanderOnClickEvents();

    if(!isPageLoading) {
        updateClassOnClick('siteTreeSelector', 'fa-plus-square', 'fa-minus-square');
    }    
}

function buildTree(baseData, baseElement, checkboxFunction) {
    var dataLength = baseData.length;

    if(projectsLocationcheckbox.checked) {
        for(var i = 0; i < dataLength; i++){
            var base = baseData[i];
            var li = document.createElement('li');
            var ul = createUL();
            var baseName = getAttribute(base.Attributes, 'ProjectName');
            buildSite(base.Sites, ul, checkboxFunction, baseName);
            appendListItemChildren(li, 'ProjectName'.concat(base.GUID), checkboxFunction, 'ProjectName', baseName, ul, baseName, base.GUID);
    
            baseElement.appendChild(li);        
        }
    }
    else {
        var sites = [];
        for(var i = 0; i < dataLength; i++){
            var project = baseData[i];
            sites.push(...project.Sites);
        }

        buildSite(sites, baseElement, checkboxFunction, baseName);
    }
}

function buildSiteProjectTree(baseData, baseElement, checkboxFunction) {
    var siteNames = [];
    var sites = [];

    var dataLength = baseData.length;
    if(sitesLocationcheckbox.checked) {
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
    else {
        buildTree(baseData, baseElement, checkboxFunction);
    }
}

function buildProject(projects, baseElement, checkboxFunction, linkedSite) {
    var projectsLength = projects.length;
    if(projectsLocationcheckbox.checked) {
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
    else {
        var meters = [];
        for(var j = 0; j < projectsLength; j++){
            var project = projects[j];
            meters.push(...project.meters);
        }

        buildMeter(meters, baseElement, checkboxFunction, linkedSite);
    }
}

function buildSite(sites, baseElement, checkboxFunction, linkedSite) {
    var sitesLength = sites.length;

    if(sitesLocationcheckbox.checked) {
        for(var i = 0; i < sitesLength; i++) {
            var site = sites[i];
            var li = document.createElement('li');
            var ul = createUL();
            buildMeter(site.Meters, ul, checkboxFunction, linkedSite);
            appendListItemChildren(li, 'Site'.concat(i), checkboxFunction, 'Site', site.SiteName, ul, linkedSite, '');
    
            baseElement.appendChild(li);
        }
    }
    else {
        var meters = [];
        for(var i = 0; i < sitesLength; i++){
            var site = sites[i];
            meters.push(...site.Meters);
        }

        buildMeter(meters, baseElement, checkboxFunction, linkedSite);
    }
}

function buildMeter(meters, baseElement, checkboxFunction, linkedSite) {
    if(!metersLocationcheckbox.checked) {
        return;
    }

    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        var li = document.createElement('li');
        var ul = createUL();
        var branchId = 'Meter'.concat(meter.GUID);
        appendListItemChildren(li, branchId, checkboxFunction, 'Meter', meter.Identifier, ul, linkedSite, '');

        var branchDiv = li.children[branchId];
        branchDiv.removeAttribute('class', 'far fa-plus-square show-pointer expander');
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

function updateGanttChartAndDataGrid() {
    buildGanttChart();
    buildDataGrid();
}

function filerGanttData () {
    var projectInputs = $("input[type='checkbox'][branch='ProjectName']:checked");
    var siteInputs = $("input[type='checkbox'][branch='Site']:checked");
    var meterInputs = $("input[type='checkbox'][branch='Meter']:checked");
    
    var status = $("input[type='radio'][name='group2']:checked").val();
    var ganttDataLength = ganttData.length;
    var newGanttData = [];
    var idsToPush = [];

    if(status == "All" 
        && projectInputs.length == 0
        && siteInputs.length == 0
        && meterInputs.length == 0) {
        return ganttData;
    }

    for(var i = 0; i < ganttDataLength; i++) {
        var project = ganttData[i];

        if(status == 'All' || project.status == status) {
            if(projectInputs.length == 0
                && siteInputs.length == 0
                && meterInputs.length == 0) {
                pushProjectId(idsToPush, project.id);
            }
            else {
                for(var j = 0; j < projectInputs.length; j++) {
                    var projectSpan = document.getElementById(projectInputs[j].id.replace('checkbox', 'span'));
    
                    if(projectSpan.innerText == project.name) {
                        pushProjectId(idsToPush, project.id);
                    }
                }

                for(var j = 0; j < siteInputs.length; j++) {
                    var siteSpan = document.getElementById(siteInputs[j].id.replace('checkbox', 'span'));

                    for(var k = 0; k < project.sites.length; k++) {
                        if(project.sites[k].name == siteSpan.innerText) {
                            pushProjectId(idsToPush, project.id);
                        }
                    }
                }

                for(var j = 0; j < meterInputs.length; j++) {
                    var meterSpan = document.getElementById(meterInputs[j].id.replace('checkbox', 'span'));

                    for(var k = 0; k < project.sites.length; k++) {
                        for(var l = 0; l < project.sites[k].meters.length; l++) {
                            if(project.sites[k].meters[l].identifier == meterSpan.innerText) {
                                pushProjectId(idsToPush, project.id);
                            }
                        }
                    }
                }
            }
        }
    }

    idsToPush.forEach(element => {
        for(var i = 0; i < ganttDataLength; i++) {
            if(ganttData[i].id == element) {
                newGanttData.push(ganttData[i]);
            }
        }
    });

    return newGanttData;
}

function pushProjectId(idsToPush, id) {
    if(!idsToPush.includes(id)) {
        idsToPush.push(id);
    }
}

function buildDataGrid() {
    var newGanttData = filerGanttData();
    var spreadsheet = document.getElementById('spreadsheet');
    clearElement(spreadsheet);

    var columnsToMerge = ["A", "D", "E", "F", "G", "H", "I", "J", "K", "L"];
    var displayData = [];
    var cellsToMerge = [];
    var ganttDataLength = newGanttData.length;
    var startRow = 1;
    var endRow = 1;
    for(var i = 0; i < ganttDataLength; i++) {
        var project = newGanttData[i];
        var siteLength = project.sites.length;
        var mergeProject = siteLength > 1;
        var rowCount = 0;

        for(var j = 0; j < siteLength; j++) {
            var site = project.sites[j];
            var meterLength = site.meters.length;
            var mergeSite = meterLength > 1;

            if(!mergeProject) {
                mergeProject = mergeSite;
            }

            if(mergeSite) {
                cellsToMerge.push({cell: "B" + endRow, rowSpan: meterLength});
            }

            for(var k = 0; k < meterLength; k++) {
                var meter = site.meters[k];
                displayData.push({
                    projectName:project.name,
                    site:site.name,
                    meter:meter.identifier,
                    engineer:'En Gineer',
                    projectStatus:project.status,
                    estimatedStartDate:'01/04/2020',
                    estimatedFinishDate:'09/05/2020',
                    estimatedCost:'£100,000',
                    estimatedkWhSavings:'10,000',
                    estimatedCostSavings:'£15,000',
                    totalROIMonths:'84',
                    roiMonthsRemaining:'84'},);
                endRow++;
                rowCount++;
            }
        }

        if(mergeProject) {
            for(var j = 0; j < columnsToMerge.length; j++)
            {
                cellsToMerge.push({cell: columnsToMerge[j] + startRow, rowSpan: rowCount});
            }
        }

        startRow = endRow + 1;
    }

    var table = jexcel(spreadsheet, {
        pagination:10,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
        data: displayData,
        columns: [
            {type:'text', width:'175px', name:'projectName', title:'Project Name', readOnly: true},
            {type:'text', width:'150px', name:'site', title:'Site', readOnly: true},
            {type:'text', width:'150px', name:'meter', title:'Meter', readOnly: true},
            {type:'text', width:'140px', name:'engineer', title:'Engineer', readOnly: true},
            {type:'text', width:'140px', name:'projectStatus', title:'Project<br>Status', readOnly: true},
            {type:'text', width:'150px', name:'estimatedStartDate', title:'Estimated<br>Start Date', readOnly: true},
            {type:'text', width:'150px', name:'estimatedFinishDate', title:'Estimated<br>End Date', readOnly: true},
            {type:'text', width:'140px', name:'estimatedCost', title:'Estimated<br>Cost', readOnly: true},
            {type:'text', width:'150px', name:'estimatedkWhSavings', title:'Estimated kWh<br>Savings (pa)', readOnly: true},
            {type:'text', width:'150px', name:'estimatedCostSavings', title:'Estimated £<br>Savings (pa)', readOnly: true},
            {type:'text', width:'150px', name:'totalROIMonths', title:'Total<br>ROI Months', readOnly: true},
            {type:'text', width:'150px', name:'roiMonthsRemaining', title:'ROI Months<br>Remaining', readOnly: true},
         ]
    });

    cellsToMerge.forEach(element => {
        table.setMerge(element.cell, 1, element.rowSpan);
    });
}

function buildGanttChart() {
    var newGanttData = filerGanttData();
    var ganttChart = document.getElementById("ganttChart");
    clearElement(ganttChart);

    $(function () {
        var tableContainer = document.getElementById('spreadsheet');

        $("#ganttChart").ganttView({ 
            data: newGanttData,
            slideWidth: tableContainer.clientWidth
        });
    });
}

(function (jQuery) {
	
    jQuery.fn.ganttView = function () {
    	
    	var args = Array.prototype.slice.call(arguments);
    	
    	if (args.length == 1 && typeof(args[0]) == "object") {
        	build.call(this, args[0]);
    	}
    };
    
    function build(options) {
    	
    	var els = this;
        var defaults = {
            showWeekends: true,
            cellWidth: 20,
            cellHeight: 50,
            slideWidth: 250,
            vHeaderWidth: 100,
            behavior: {
            	clickable: true//,
            	//draggable: true,
            	//resizable: true
            }
        };
        
        var opts = jQuery.extend(true, defaults, options);

		if (opts.data) {
			build();
		} else if (opts.dataUrl) {
			jQuery.getJSON(opts.dataUrl, function (data) { opts.data = data; build(); });
		}

		function build() {
			var startEnd = DateUtils.getBoundaryDatesFromData(opts.data);
			opts.start = startEnd[0];
			opts.end = startEnd[1];
			
	        els.each(function () {

	            var container = jQuery(this);
                var div = jQuery("<div>", { "class": "ganttview" });
                container.append(div);
	            container.css("width", "100%");
	            new Chart(div, opts).render();				
	        });
		}
    }

	var Chart = function(div, opts) {
		
		function render() {
            addVtHeader(div, opts.data, opts.cellHeight);
            
            var slideDiv = jQuery("<div>", {
                "class": "ganttview-slide-container",
                // "css": { "width": "calc(100% - 282px)" }
            });
            div.append(slideDiv);
			
            dates = getDates(opts.start, opts.end);
            addHzHeader(slideDiv, dates, opts.cellWidth);
            addGrid(slideDiv, opts.data, dates, opts.cellWidth, opts.showWeekends);
            addBlockContainers(slideDiv, opts.data);
            addBlocks(slideDiv, opts.data, opts.cellWidth, opts.start);
            applyLastClass(div.parent());
		}
		
		var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

		// Creates a 3 dimensional array [year][month][day] of every day 
		// between the given start and end dates
        function getDates(start, end) {
            var dates = [];
			dates[start.getFullYear()] = [];
			dates[start.getFullYear()][start.getMonth()] = [start]
			var last = start;
			while (last.compareTo(end) == -1) {
				var next = last.clone().addDays(1);
				if (!dates[next.getFullYear()]) { dates[next.getFullYear()] = []; }
				if (!dates[next.getFullYear()][next.getMonth()]) { 
					dates[next.getFullYear()][next.getMonth()] = []; 
				}
				dates[next.getFullYear()][next.getMonth()].push(next);
				last = next;
			}
			return dates;
        }

        function addVtHeader(div, data, cellHeight) {
            var headerDiv = jQuery("<div>", { "class": "ganttview-vtheader" });
            for (var i = 0; i < data.length; i++) {
                var vtHeaderDiv = jQuery("<div>");
                vtHeaderDiv.append(data[i].name);
                vtHeaderDiv.append(jQuery("<br>"));

                for(var j = 0; j < data[i].sites.length; j++) {
                    var siteDiv = jQuery("<div>", { "css": { "padding-left" : "5px"}});
                    siteDiv.append(data[i].sites[j].name);
                    siteDiv.append(jQuery("<br>"));

                    for(var k = 0; k < data[i].sites[j].meters.length; k++) {
                        var meterDiv = jQuery("<div>", { "css": { "padding-left" : "10px"}});
                        meterDiv.append(data[i].sites[j].meters[k].identifier);
                        meterDiv.append(jQuery("<br>"));

                        siteDiv.append(meterDiv);
                    }

                    vtHeaderDiv.append(siteDiv);
                }

                var itemDiv = jQuery("<div>", { "class": "ganttview-vtheader-item" });
                itemDiv.append(jQuery("<div>", {
                    "class": "ganttview-vtheader-item-name",
                    "css": { "height": (data[i].series.length * cellHeight) + "px" }
                }).append(vtHeaderDiv));
                var seriesDiv = jQuery("<div>", { "class": "ganttview-vtheader-series" });
                for (var j = 0; j < data[i].series.length; j++) {
                    seriesDiv.append(jQuery("<div>", { "class": "ganttview-vtheader-series-name" })
						.append(data[i].series[j].name));
                }
                itemDiv.append(seriesDiv);
                headerDiv.append(itemDiv);
            }
            div.append(headerDiv);
        }

        function addHzHeader(div, dates, cellWidth) {
            var headerDiv = jQuery("<div>", { "class": "ganttview-hzheader" });
            var monthsDiv = jQuery("<div>", { "class": "ganttview-hzheader-months" });
            var daysDiv = jQuery("<div>", { "class": "ganttview-hzheader-days" });
            var totalW = 0;
            var monthCount = 0;
			for (var y in dates) {
				for (var m in dates[y]) {
                    monthCount++;
					var w = (dates[y][m].length * (cellWidth + 1));
					totalW = totalW + w;
					monthsDiv.append(jQuery("<div>", {
						"class": "ganttview-hzheader-month",
						"css": { "width": (w - 1) + "px" }
					}).append(monthNames[m] + " " + y));
					for (var d in dates[y][m]) {
						daysDiv.append(jQuery("<div>", { "class": "ganttview-hzheader-day" })
							.append(dates[y][m][d].getDate()));
					}
				}
            }
            var groupingWidth = div[0].clientWidth < totalW ? totalW : div[0].clientWidth;
            daysDiv.css("width", totalW + "px");
            monthsDiv.css("width", (groupingWidth + monthCount) + "px");
            headerDiv.append(monthsDiv).append(daysDiv);
            div.append(headerDiv);
        }

        function addGrid(div, data, dates, cellWidth, showWeekends) {
            var gridDiv = jQuery("<div>", { "class": "ganttview-grid" });
            var rowDiv = jQuery("<div>", { "class": "ganttview-grid-row" });
			for (var y in dates) {
				for (var m in dates[y]) {
					for (var d in dates[y][m]) {
						var cellDiv = jQuery("<div>", { "class": "ganttview-grid-row-cell" });
						if (DateUtils.isWeekend(dates[y][m][d]) && showWeekends) { 
							cellDiv.addClass("ganttview-weekend"); 
                        }
                        if (DateUtils.isToday(dates[y][m][d])) { 
							cellDiv.addClass("ganttview-today"); 
						}
						rowDiv.append(cellDiv);
					}
				}
			}
            var w = jQuery("div.ganttview-grid-row-cell", rowDiv).length * (cellWidth + 1);
            rowDiv.css("width", w + "px");
            gridDiv.css("width", w + "px");
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    gridDiv.append(rowDiv.clone());
                }
            }
            div.append(gridDiv);
        }

        function addBlockContainers(div, data) {
            var blocksDiv = jQuery("<div>", { "class": "ganttview-blocks" });
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    blocksDiv.append(jQuery("<div>", { "class": "ganttview-block-container" }));
                }
            }
            div.append(blocksDiv);
        }

        function addBlocks(div, data, cellWidth, start) {
            var rows = jQuery("div.ganttview-blocks div.ganttview-block-container", div);
            var rowIdx = 0;
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].series.length; j++) {
                    var series = data[i].series[j];
                    var size = DateUtils.daysBetween(series.start, series.end) + 1;
					var offset = DateUtils.daysBetween(start, series.start);
					var block = jQuery("<div>", {
                        "class": "ganttview-block",
                        "title": series.name + ": " + size + " days",
                        "css": {
                            "width": ((size * (cellWidth + 1)) - 9) + "px",
                            "margin-left": ((offset * (cellWidth + 1)) + 3) + "px"
                        }
                    });
                    addBlockData(block, data[i], series);
                    if (data[i].series[j].color) {
                        block.css("background-color", data[i].series[j].color);
                    }
                    block.append(jQuery("<div>", { "class": "ganttview-block-text" }));
                    jQuery(rows[rowIdx]).append(block);
                    rowIdx = rowIdx + 1;
                }
            }
        }
        
        function addBlockData(block, data, series) {
        	// This allows custom attributes to be added to the series data objects
        	// and makes them available to the 'data' argument of click, resize, and drag handlers
        	var blockData = { id: data.id, name: data.name };
        	jQuery.extend(blockData, series);
        	block.data("block-data", blockData);
        }

        function applyLastClass(div) {
            jQuery("div.ganttview-grid-row div.ganttview-grid-row-cell:last-child", div).addClass("last");
            jQuery("div.ganttview-hzheader-days div.ganttview-hzheader-day:last-child", div).addClass("last");
            jQuery("div.ganttview-hzheader-months div.ganttview-hzheader-month:last-child", div).addClass("last");
        }
		
		return {
			render: render
		};
	}

    var DateUtils = {
    	
        daysBetween: function (start, end) {
            if (!start || !end) { return 0; }
            start = Date.parse(start); end = Date.parse(end);
            if (start.getYear() == 1901 || end.getYear() == 8099) { return 0; }
            var count = 0, date = start.clone();
            while (date.compareTo(end) == -1) { count = count + 1; date.addDays(1); }
            return count;
        },
        
        isWeekend: function (date) {
            return date.getDay() % 6 == 0;
        },

        isToday: function (date) {
            const today = new Date()
            return date.getDate() == today.getDate() &&
                date.getMonth() == today.getMonth() &&
                date.getFullYear() == today.getFullYear();
        },

		getBoundaryDatesFromData: function (data) {
			var minStart = new Date(); maxEnd = new Date(); today = new Date();
			for (var i = 0; i < data.length; i++) {
				for (var j = 0; j < data[i].series.length; j++) {
					var start = Date.parse(data[i].series[j].start);
					var end = Date.parse(data[i].series[j].end)
					if (i == 0 && j == 0) { minStart = start; maxEnd = end; }
					if (minStart.compareTo(start) == 1) { minStart = start; }
					if (maxEnd.compareTo(end) == -1) { maxEnd = end; }
				}
            }
            
            if(maxEnd < today) {
                maxEnd = new Date(today.getFullYear(), today.getMonth(), today.getDate());
            }

			var newMaxEnd = maxEnd.clone().moveToLastDayOfMonth();
			
			return [minStart, newMaxEnd];
		}
    };

})(jQuery);