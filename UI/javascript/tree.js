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

function buildTree(baseData, groupByOption, baseElement, commodity) {
    for(var i = 0; i < baseData.length; i++){
        if(!commoditySiteMatch(baseData[i], commodity)) {
            continue;
        }

        var site = baseData[i];
        var siteName = site.SiteName;

        var li = document.createElement("li");

        var ul = createUL();
        if(groupByOption == "Hierarchy") {
            buildIdentifierHierarchy(site.Meters, ul, commodity);
        }
        else {
            buildBranch(site.Meters, groupByOption, ul, commodity);
        }        

        li.appendChild(createBranchDiv(commodity.concat("Site").concat(i)));
        li.appendChild(createCheckbox());
        li.appendChild(createIcon("Site", commodity));
        li.appendChild(createSpan(siteName));
        li.appendChild(createBranchListDiv(commodity.concat("Site").concat(i).concat("List"), ul));

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity) {
    var branchOptions = getBranchOptions(meters, groupByOption, commodity);
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

        var matchedMeters = [];
        for(j = 0; j < meters.length; j++){
            if(meters[j].hasOwnProperty(groupByOption)
                && commodityMeterMatch(meters[j], commodity)) {
                if(meters[j][groupByOption] == branchOptions[i]) {
                    matchedMeters.push(meters[j]);
                }
            }
        }

        var ul = createUL();
        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity);

        li.appendChild(createBranchDiv(branchId.concat(branchCount)));
        li.appendChild(createCheckbox());
        li.appendChild(createIcon(branchOptions[i], commodity));
        li.appendChild(createSpan(branchOptions[i]));
        li.appendChild(createBranchListDiv(branchId.concat(branchCount).concat("List"), ul));

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity) {
    var branchOptions = getBranchOptions(meters, groupBySubOption, commodity);
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

        var matchedMeters = [];
        for(j = 0; j < meters.length; j++){
            if(meters[j].hasOwnProperty(groupBySubOption)
                && commodityMeterMatch(meters[j], commodity)) {
                if(meters[j][groupBySubOption] == branchOptions[i]) {
                    matchedMeters.push(meters[j]);
                }
            }
        }

        var ul = createUL();
        buildIdentifierHierarchy(matchedMeters, ul, commodity);

        li.appendChild(createBranchDiv(branchId.concat(subBranchCount)));
        li.appendChild(createCheckbox());
        li.appendChild(createIcon(branchOptions[i], commodity));
        li.appendChild(createSpan(branchOptions[i]));
        li.appendChild(createBranchListDiv(branchId.concat(subBranchCount).concat("List"), ul));

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

        var branchDiv = createBranchDiv(meters[i].Identifier);
        
        if(!meters[i].hasOwnProperty("Sub Meters")) {
            branchDiv.removeAttribute("class", "far fa-plus-square");
            branchDiv.setAttribute("class", "far fa-times-circle");
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox());
        li.appendChild(createIcon(meters[i]["Device Type"], commodity));
        li.appendChild(createSpan(meters[i].Identifier));

        if(meters[i].hasOwnProperty("Sub Meters")) {
            var ul = createUL();
            buildSubMeterHierarchy(meters[i]["Sub Meters"], ul, meters[i]["Device Type"], commodity);

            li.appendChild(createBranchListDiv(meters[i].Identifier.concat("List"), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity) {
    for(var i = 0; i < subMeters.length; i++){
        var li = document.createElement("li");

        var branchDiv = createBranchDiv(subMeters[i].Identifier);
        branchDiv.removeAttribute("class", "far fa-plus-square");
        branchDiv.setAttribute("class", "far fa-times-circle");

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox());
        li.appendChild(createIcon(deviceType, commodity));
        li.appendChild(createSpan(subMeters[i].Identifier));   

        baseElement.appendChild(li); 
    }
}

function getBranchOptions(meters, property, commodity) {
    var branchOptions = [];

    for(var i = 0; i < meters.length; i++) {
        if(meters[i].hasOwnProperty(property)
            && commodityMeterMatch(meters[i], commodity)) {
            if(!branchOptions.includes(meters[i][property])) {
                branchOptions.push(meters[i][property]);
            }
        }        
    }

    return branchOptions;
}

function createBranchDiv(branchDivId) {
    var branchDiv = document.createElement("div");
    branchDiv.id = branchDivId;
    branchDiv.setAttribute("class", "far fa-plus-square");
    branchDiv.setAttribute("style", "padding-right: 4px;");
    return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
    var branchListDiv = document.createElement("div");
    branchListDiv.id = branchListDivId;
    branchListDiv.setAttribute("class", "listitem-hidden");
    branchListDiv.appendChild(ul);
    return branchListDiv;
}

function createUL() {
    var ul = document.createElement("ul");
    ul.setAttribute("class", "format-listitem");
    return ul;
}

function createIcon(branch, commodity) {
    var icon = document.createElement("i");
    icon.setAttribute("class", getIconByBranch(branch, commodity));
    icon.setAttribute("style", "padding-left: 3px; padding-right: 3px;");
    return icon;
}

function createSpan(innerHTML) {
    var span = document.createElement("span");
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox() {
    var checkBox = document.createElement("input");
    checkBox.type = "checkbox";  
    return checkBox;
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
        case "Site":
            return "fas fa-map-marker-alt";
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