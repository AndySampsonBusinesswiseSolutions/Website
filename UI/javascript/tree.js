var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, groupByOption, divId, commodity, checkboxFunction) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, groupByOption, ul, commodity, checkboxFunction);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, groupByOption, baseElement, commodity, checkboxFunction) {
    for(var i = 0; i < baseData.length; i++){
        if(!commoditySiteMatch(baseData[i], commodity)) {
            continue;
        }

        var site = baseData[i];
        var siteName = getAttribute(site.Attributes, 'SiteName');
        var li = document.createElement('li');
        var ul = createUL();
        
        if(groupByOption == 'Hierarchy') {
            buildIdentifierHierarchy(site.Meters, ul, commodity, checkboxFunction, siteName);
        }
        else {
            buildBranch(site.Meters, groupByOption, ul, commodity, checkboxFunction, siteName);
        }
        appendListItemChildren(li, commodity.concat('Site').concat(site.GUID), checkboxFunction, 'Site', siteName, commodity, ul, siteName, site.GUID);

        baseElement.appendChild(li);        
    }
}

function buildBranch(meters, groupByOption, baseElement, commodity, checkboxFunction, linkedSite) {
    var branchOptions = getBranchOptions(meters, groupByOption, commodity);
    var branchId;
    var groupBySubOption;

    switch (groupByOption) {
        case 'DeviceType':
            branchId = commodity.concat('DeviceType');
            groupBySubOption = 'DeviceSubType';
            break;
        case 'Zone':
            branchId = commodity.concat('Zone');
            groupBySubOption = 'Panel';
            break;
    }

    for(var i = 0; i < branchOptions.length; i++) {
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupByOption, branchOptions[i], commodity);
        var ul = createUL();
        buildSubBranch(matchedMeters, ul, groupBySubOption, commodity, checkboxFunction, linkedSite);
        appendListItemChildren(li, branchId.concat(branchCount), checkboxFunction, 'GroupByOption|'.concat(groupByOption), branchOptions[i], commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildSubBranch(meters, baseElement, groupBySubOption, commodity, checkboxFunction, linkedSite) {
    var branchOptions = getBranchOptions(meters, groupBySubOption, commodity);
    var branchId; 

    switch (groupBySubOption) {
        case 'DeviceSubType':
            branchId = commodity.concat('DeviceSubType');
            break;
        case 'Panel':
            branchId = commodity.concat('Panel');
            break;
    }

    for(var i = 0; i < branchOptions.length; i++) {
        var li = document.createElement('li');

        var matchedMeters = getMatchedMeters(meters, groupBySubOption, branchOptions[i], commodity);
        var ul = createUL();
        buildIdentifierHierarchy(matchedMeters, ul, commodity, checkboxFunction, linkedSite);
        appendListItemChildren(li, branchId.concat(subBranchCount), checkboxFunction, 'GroupBySubOption|'.concat(groupBySubOption), branchOptions[i], commodity, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid));
    li.appendChild(createIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite) {
    for(var i = 0; i < meters.length; i++){
        if(!commodityMeterMatch(meters[i], commodity)) {
            continue;
        }

        var identifier = getAttribute(meters[i].Attributes, 'Identifier')
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meters[i].GUID);

        var branchDiv = createBranchDiv(branchId);
        
        if(!meters[i].hasOwnProperty('SubMeters')) {
            branchDiv.removeAttribute('class', 'far fa-plus-square');
            branchDiv.setAttribute('class', 'far fa-times-circle');
        }

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meters[i].GUID));
        li.appendChild(createIcon(getAttribute(meters[i].Attributes, 'DeviceType'), getAttribute(meters[i].Attributes, 'Commodity')));
        li.appendChild(createSpan(branchId, identifier));

        if(meters[i].hasOwnProperty('SubMeters')) {
            var ul = createUL();
            buildSubMeterHierarchy(meters[i]['SubMeters'], ul, getAttribute(meters[i].Attributes, 'DeviceType'), getAttribute(meters[i].Attributes, 'Commodity'), checkboxFunction, linkedSite);

            li.appendChild(createBranchListDiv(branchId.concat('List'), ul));
        }        

        baseElement.appendChild(li); 
    }
}

function buildSubMeterHierarchy(subMeters, baseElement, deviceType, commodity, checkboxFunction, linkedSite) {
    for(var i = 0; i < subMeters.length; i++){
        var li = document.createElement('li');

        var identifier = getAttribute(subMeters[i].Attributes, 'Identifier');
        var branchDiv = createBranchDiv(subMeters[i].GUID);
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox('SubMeter'.concat(subMeters[i].GUID), checkboxFunction, 'SubMeter', linkedSite, subMeters[i].GUID));
        li.appendChild(createIcon(deviceType, commodity));
        li.appendChild(createSpan('SubMeter'.concat(subMeters[i].GUID), identifier));   

        baseElement.appendChild(li); 
    }
}

function getBranchOptions(meters, property, commodity) {
    var branchOptions = [];

    for(var i = 0; i < meters.length; i++) {
        var attribute = getAttribute(meters[i].Attributes, property);
        if(!branchOptions.includes(attribute)
            && commodityMeterMatch(meters[i], commodity)) {
            branchOptions.push(attribute);
        }        
    }

    return branchOptions;
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

function createIcon(branch, commodity) {
    var icon = document.createElement('i');
    icon.setAttribute('class', getIconByBranch(branch, commodity));
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
    var functionName = functionArray[0];
    var functionArguments = [];

    var checkBox = document.createElement('input');
    checkBox.type = 'checkbox';  
    checkBox.id = checkboxId.concat('checkbox');
    checkBox.setAttribute('Branch', branch);
    checkBox.setAttribute('LinkedSite', linkedSite);
    checkBox.setAttribute('GUID', guid);

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
    if(!site.hasOwnProperty('Meters')) {
        return false;
    }

    for(var i = 0; i < site.Meters.length; i++) {
        if(commodityMeterMatch(site.Meters[i], commodity)) {
            return true;
        }
    }

    return false;
}

function commodityMeterMatch(meter, commodity) {
    if(commodity == '') {
        return true;
    }

    var meterCommodity = getAttribute(meter.Attributes, 'Commodity');
    return meterCommodity.toLowerCase() == commodity.toLowerCase();
}

function getIconByBranch(branch, commodity) {
    switch (branch) {
        case 'Site':
            return 'fas fa-map-marker-alt';
        case 'Mains':
            if(commodity == 'Gas') {
                return 'fas fa-burn';
            }
            else {
                return 'fas fa-plug';
            }
        case 'Lighting':
            return 'fas fa-lightbulb';
        case 'Unknown':
            return 'fas fa-question-circle';
    }
}

function getMatchedMeters(meters, attribute, branchOption, commodity) {
    var matchedMeters = [];

    for(j = 0; j < meters.length; j++){
        if(getAttribute(meters[j].Attributes, attribute) == branchOption
            && commodityMeterMatch(meters[j], commodity)) {
            matchedMeters.push(meters[j]);
        }
    }

    return matchedMeters;
}