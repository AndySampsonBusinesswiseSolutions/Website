var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity) {
    var tree = document.createElement("div");
    tree.setAttribute("class", "scrolling-wrapper");
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity);

    var div = document.getElementById(divId);

    while (div.firstChild) {
        div.removeChild(div.firstChild);
    }

    div.appendChild(tree);

    var expanders = document.getElementsByClassName("fa-plus-square");
	for(var i=0; i< expanders.length; i++){
		expanders[i].addEventListener('click', function (event) {
			updateClassOnClick(this.id, "fa-plus-square", "fa-minus-square")
			updateClassOnClick(this.id.concat('List'), "listitem-hidden", "")
		});
	}
}

function createUL() {
    var ul = document.createElement("ul");
    ul.setAttribute("class", "format-listitem");
    return ul;
}

function buildTree(baseData, groupByOption, baseElement, commodity) {
    for(var i = 0; i < baseData.length; i++){
        if(!commoditySiteMatch(baseData[i], commodity)) {
            continue;
        }

        var site = baseData[i];
        var siteName = site.SiteName;

        var li = document.createElement("li");

        var siteDiv = document.createElement("div");
        siteDiv.id = commodity.concat("Site").concat(i);
        siteDiv.setAttribute("class", "far fa-plus-square");
        siteDiv.setAttribute("style", "padding-right: 4px;");

        var checkBox = document.createElement("input");
        checkBox.type = "checkbox";  

        var icon = document.createElement("i");
        icon.setAttribute("class", "fas fa-map-marker-alt");
        icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");

        var span = document.createElement("span");
        span.innerHTML = siteName;

        var siteListDiv = document.createElement("div");
        siteListDiv.id = commodity.concat("Site").concat(i).concat("List");
        siteListDiv.setAttribute("class", "listitem-hidden");

        var ul = createUL();
        if(groupByOption == "Hierarchy") {
            buildIdentifierHierarchy(site.Meters, ul, commodity);
        }
        else {
            buildBranch(site.Meters, groupByOption, ul, commodity);
        }        

        siteListDiv.appendChild(ul);
        li.appendChild(siteDiv);
        li.appendChild(checkBox);
        li.appendChild(icon);
        li.appendChild(span);
        li.appendChild(siteListDiv);

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity) {
    //loop through meters
    //get group by option data
    //add into array if not already present
    //loop through array
    var branchOptions = [];

    for(var i = 0; i < meters.length; i++) {
        if(meters[i].hasOwnProperty(groupByOption)
            && commodityMeterMatch(meters[i], commodity)) {
            if(!branchOptions.includes(meters[i][groupByOption])) {
                branchOptions.push(meters[i][groupByOption]);
            }
        }        
    }

    var branchId;
    var groupBySubOption;

    switch (groupByOption) {
        case "Device Type":
            branchId = commodity.concat("DeviceType");
            groupBySubOption = "Device Sub Type";
            break;
        case "Zone":
            branchId = commodity.concat("Zone");
            groupBySubOption = "Panel";
            break;
    }

    for(var i = 0; i < branchOptions.length; i++) {
        var li = document.createElement("li");

        var branchDiv = document.createElement("div");
        branchDiv.id = branchId.concat(branchCount);
        branchDiv.setAttribute("class", "far fa-plus-square");
        branchDiv.setAttribute("style", "padding-right: 4px;");

        var checkBox = document.createElement("input");
        checkBox.type = "checkbox";  

        var icon = document.createElement("i");
        icon.setAttribute("class", getIconByBranch(branchOptions[i], commodity));
        icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");

        var span = document.createElement("span");
        span.innerHTML = branchOptions[i];

        var branchOptionListDiv = document.createElement("div");
        branchOptionListDiv.id = branchDiv.id.concat("List");
        branchOptionListDiv.setAttribute("class", "listitem-hidden");

        var ul = createUL();

        var matchedMeters = [];
        for(j = 0; j < meters.length; j++){
            if(meters[j].hasOwnProperty(groupByOption)
                && commodityMeterMatch(meters[j], commodity)) {
                if(meters[j][groupByOption] == branchOptions[i]) {
                    matchedMeters.push(meters[j]);
                }
            }
        }

        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity);

        branchOptionListDiv.appendChild(ul);
        li.appendChild(branchDiv);
        li.appendChild(checkBox);
        li.appendChild(icon);
        li.appendChild(span);
        li.appendChild(branchOptionListDiv);

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity) {
    var branchOptions = [];

    for(var i = 0; i < meters.length; i++) {
        if(meters[i].hasOwnProperty(groupBySubOption)) {
            if(!branchOptions.includes(meters[i][groupBySubOption])) {
                branchOptions.push(meters[i][groupBySubOption]);
            }
        }        
    }

    var branchId;

    switch (groupBySubOption) {
        case "Device Sub Type":
            branchId = commodity.concat("DeviceSubType");
            break;
        case "Panel":
            branchId = commodity.concat("Panel");
            break;
    }

    for(var i = 0; i < branchOptions.length; i++) {
        var li = document.createElement("li");

        var branchDiv = document.createElement("div");
        branchDiv.id = branchId.concat(subBranchCount);
        branchDiv.setAttribute("class", "far fa-plus-square");
        branchDiv.setAttribute("style", "padding-right: 4px;");

        var checkBox = document.createElement("input");
        checkBox.type = "checkbox";  

        var icon = document.createElement("i");
        icon.setAttribute("class", getIconByBranch(branchOptions[i], commodity));
        icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");

        var span = document.createElement("span");
        span.innerHTML = branchOptions[i];

        var branchOptionListDiv = document.createElement("div");
        branchOptionListDiv.id = branchDiv.id.concat("List");
        branchOptionListDiv.setAttribute("class", "listitem-hidden");

        var ul = createUL();

        var matchedMeters = [];
        for(j = 0; j < meters.length; j++){
            if(meters[j].hasOwnProperty(groupBySubOption)
                && commodityMeterMatch(meters[j], commodity)) {
                if(meters[j][groupBySubOption] == branchOptions[i]) {
                    matchedMeters.push(meters[j]);
                }
            }
        }

        buildIdentifierHierarchy(matchedMeters, ul, commodity);

        branchOptionListDiv.appendChild(ul);
        li.appendChild(branchDiv);
        li.appendChild(checkBox);
        li.appendChild(icon);
        li.appendChild(span);
        li.appendChild(branchOptionListDiv);

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function buildIdentifierHierarchy(meters, baseElement, commodity) {
    for(var i = 0; i < meters.length; i++){
        if(!commodityMeterMatch(meters[i], commodity)) {
            continue;
        }

        var li = document.createElement("li");

        var branchDiv = document.createElement("div");
        branchDiv.id = meters[i].Identifier;
        
        if(meters[i].hasOwnProperty("Sub Meters")) {
            branchDiv.setAttribute("class", "far fa-plus-square");
        }
        else {
            branchDiv.setAttribute("class", "far fa-times-circle");
        }

        branchDiv.setAttribute("style", "padding-right: 4px;");

        var checkBox = document.createElement("input");
        checkBox.type = "checkbox";  

        var icon = document.createElement("i");
        icon.setAttribute("class", getIconByBranch(meters[i]["Device Type"], commodity));
        icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");

        var span = document.createElement("span");
        span.innerHTML = meters[i].Identifier;

        li.appendChild(branchDiv);
        li.appendChild(checkBox);
        li.appendChild(icon);
        li.appendChild(span);

        if(meters[i].hasOwnProperty("Sub Meters")) {
            var branchOptionListDiv = document.createElement("div");
            branchOptionListDiv.id = meters[i].Identifier.concat("List");
            branchOptionListDiv.setAttribute("class", "listitem-hidden");

            var ul = createUL();
            buildSubMeterHierarchy(meters[i]["Sub Meters"], ul);

            li.appendChild(branchOptionListDiv);
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType) {
    for(var i = 0; i < subMeters.length; i++){
        var li = document.createElement("li");

        var checkBox = document.createElement("input");
        checkBox.type = "checkbox";  

        var icon = document.createElement("i");
        icon.setAttribute("class", getIconByBranch(deviceType, commodity));
        icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");

        var span = document.createElement("span");
        span.innerHTML = subMeters[i].Identifier;

        li.appendChild(branchDiv);
        li.appendChild(checkBox);
        li.appendChild(icon);
        li.appendChild(span);   

        baseElement.appendChild(li); 
    }
}

function commoditySiteMatch(site, commodity) {
    if(!site.hasOwnProperty("Meters")) {
        return false;
    }

    for(var i = 0; i < site.Meters.length; i++) {
        if(site.Meters[i].Commodity.toLowerCase() == commodity.toLowerCase()) {
            return true;
        }
    }

    return false;
}

function commodityMeterMatch(meter, commodity) {
    return meter.Commodity.toLowerCase() == commodity.toLowerCase();
}

function getIconByBranch(branch, commodity) {
    switch (branch) {
        case "Mains":
            if(commodity == "Gas") {
                return "fas fa-burn";
            }
            else {
                return "fas fa-plug";
            }
        case "Lighting":
            return "fas fa-lightbulb";
        case "Unknown":
            return "fas fa-question-circle";
    }
}