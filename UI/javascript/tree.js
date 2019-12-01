var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity, checkboxFunction) {
    var tree = document.createElement("div");
    tree.setAttribute("class", "scrolling-wrapper");
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity, checkboxFunction);

    var div = document.getElementById(divId);

    while (div.firstChild) {
        div.removeChild(div.firstChild);
    }

    div.appendChild(tree);
}

function buildTree(baseData, groupByOption, baseElement, commodity, checkboxFunction) {
    for(var i = 0; i < baseData.length; i++){
        if(!commoditySiteMatch(baseData[i], commodity)) {
            continue;
        }

        var site = baseData[i];
        var siteName = site.SiteName;

        var li = document.createElement("li");

        var ul = createUL();
        if(groupByOption == "Hierarchy") {
            buildIdentifierHierarchy(site.Meters, ul, commodity, checkboxFunction);
        }
        else {
            buildBranch(site.Meters, groupByOption, ul, commodity, checkboxFunction);
        }        

        li.appendChild(createBranchDiv(commodity.concat("Site").concat(i)));
        li.appendChild(createCheckbox(commodity.concat("Site").concat(i), checkboxFunction, "Site"));
        li.appendChild(createIcon("Site", commodity));
        li.appendChild(createSpan(commodity.concat("Site").concat(i), siteName));
        li.appendChild(createBranchListDiv(commodity.concat("Site").concat(i).concat("List"), ul));

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity, checkboxFunction) {
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
        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity, checkboxFunction);

        li.appendChild(createBranchDiv(branchId.concat(branchCount)));
        li.appendChild(createCheckbox(branchId.concat(branchCount), checkboxFunction, groupByOption));
        li.appendChild(createIcon(branchOptions[i], commodity));
        li.appendChild(createSpan(branchId.concat(branchCount), branchOptions[i]));
        li.appendChild(createBranchListDiv(branchId.concat(branchCount).concat("List"), ul));

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity, checkboxFunction) {
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
        buildIdentifierHierarchy(matchedMeters, ul, commodity, checkboxFunction);

        li.appendChild(createBranchDiv(branchId.concat(subBranchCount)));
        li.appendChild(createCheckbox(branchId.concat(subBranchCount), checkboxFunction, groupBySubOption));
        li.appendChild(createIcon(branchOptions[i], commodity));
        li.appendChild(createSpan(branchId.concat(subBranchCount), branchOptions[i]));
        li.appendChild(createBranchListDiv(branchId.concat(subBranchCount).concat("List"), ul));

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction) {
    for(var i = 0; i < meters.length; i++){
        if(!commodityMeterMatch(meters[i], commodity)) {
            continue;
        }

        var li = document.createElement("li");
        var branchId = "Meter".concat(meters[i].Identifier);

        var branchDiv = createBranchDiv(branchId);
        
        if(!meters[i].hasOwnProperty("Sub Meters")) {
            branchDiv.removeAttribute("class", "far fa-plus-square");
            branchDiv.setAttribute("class", "far fa-times-circle");
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, "Meter"));
        li.appendChild(createIcon(meters[i]["Device Type"], commodity));
        li.appendChild(createSpan(branchId, meters[i].Identifier));

        if(meters[i].hasOwnProperty("Sub Meters")) {
            var ul = createUL();
            buildSubMeterHierarchy(meters[i]["Sub Meters"], ul, meters[i]["Device Type"], commodity, checkboxFunction);

            li.appendChild(createBranchListDiv(branchId.concat("List"), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity, checkboxFunction) {
    for(var i = 0; i < subMeters.length; i++){
        var li = document.createElement("li");

        var branchDiv = createBranchDiv(subMeters[i].Identifier);
        branchDiv.removeAttribute("class", "far fa-plus-square");
        branchDiv.setAttribute("class", "far fa-times-circle");

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox("SubMeter".concat(subMeters[i].Identifier), checkboxFunction, "SubMeter"));
        li.appendChild(createIcon(deviceType, commodity));
        li.appendChild(createSpan(subMeters[i].Identifier, subMeters[i].Identifier));   

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
    branchDiv.id = branchDivId.replace(/ /g, '');
    branchDiv.setAttribute("class", "far fa-plus-square");
    branchDiv.setAttribute("style", "padding-right: 4px;");
    return branchDiv;
}

function createBranchListDiv(branchListDivId, ul) {
    var branchListDiv = document.createElement("div");
    branchListDiv.id = branchListDivId.replace(/ /g, '');
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

function createSpan(spanId, innerHTML) {
    var span = document.createElement("span");
    span.id = spanId.concat("span").replace(/ /g, '');
    span.innerHTML = innerHTML;
    return span;
}

function createCheckbox(checkboxId, checkboxFunction, branch) {
    var functionArray = checkboxFunction.replace(')', '').split('(');
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement("input");
    checkBox.type = "checkbox";  
    checkBox.id = checkboxId.concat("checkbox").replace(/ /g, '');
    checkBox.setAttribute('Branch', branch.replace(/ /g, ''));

    functionArguments.push(checkBox.id);
    if(functionArray.length > 1) {
        for(var i = 0; i < functionArray[1].split(',').length; i++) {
            functionArguments.push(functionArray[1].split(',')[i]);
        }
    }
    functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
    
    checkBox.setAttribute('onclick', functionName);
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