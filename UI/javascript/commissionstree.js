var branchCount = 0;
var subBranchCount = 0;

function createTree(baseData, divId, commodity, checkboxFunction, showSubMeters) {
    var tree = document.createElement('div');
    tree.setAttribute('class', 'scrolling-wrapper');
    
    var ul = createUL();
    tree.appendChild(ul);

    branchCount = 0;
    subBranchCount = 0; 

    buildTree(baseData, ul, commodity, checkboxFunction, showSubMeters);

    var div = document.getElementById(divId);
    clearElement(div);
    div.appendChild(tree);
}

function buildTree(baseData, baseElement, commodity, checkboxFunction) {
    var dataLength = baseData.length;
    for(var i = 0; i < dataLength; i++){
        var base = baseData[i];
        
        var baseName = getAttribute(base.Attributes, 'BaseName');
        var li = document.createElement('li');
        var ul = createUL();

        buildCustomerHierarchy(base.Customers, ul, commodity, checkboxFunction, baseName);
        appendListItemChildren(li, commodity.concat('Site').concat(base.GUID), checkboxFunction, 'Site', baseName, commodity, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, commodity, ul, linkedSite, guid, linkedCustomer) {
    li.appendChild(createBranchDiv(id, true));
    li.appendChild(createCheckbox(id, checkboxFunction, checkboxBranch, linkedSite, guid, linkedCustomer));
    li.appendChild(createTreeIcon(branchOption, commodity));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildCustomerHierarchy(customers, baseElement, commodity, checkboxFunction, linkedSite) {
    var customersLength = customers.length;
    for(var i = 0; i < customersLength; i++){
        var customer = customers[i];
        if(!commoditySiteMatch(customer, commodity)) {
            continue;
        }

        var customerAttributes = customer.Attributes;
        var identifier = getAttribute(customerAttributes, 'CustomerName');
        var li = document.createElement('li');
        var ul = createUL();

        buildIdentifierHierarchy(customer.Meters, ul, commodity, checkboxFunction, linkedSite, identifier);
        appendListItemChildren(li, commodity.concat('Customer').concat(customer.GUID), checkboxFunction, 'Customer', identifier, commodity, ul, linkedSite, customer.GUID, identifier);

        baseElement.appendChild(li); 
    }
}

function buildIdentifierHierarchy(meters, baseElement, commodity, checkboxFunction, linkedSite, linkedCustomer) {
    var metersLength = meters.length;
    for(var i = 0; i < metersLength; i++){
        var meter = meters[i];
        if(!commodityMeterMatch(meter, commodity)) {
            continue;
        }

        var meterAttributes = meter.Attributes;
        var identifier = getAttribute(meterAttributes, 'Identifier');
        var meterCommodity = getAttribute(meterAttributes, 'Commodity');
        var deviceType = getAttribute(meterAttributes, 'DeviceType');
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(meter.GUID);
        var branchDiv = createBranchDiv(branchId, false);

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Meter', linkedSite, meter.GUID, linkedCustomer));
        li.appendChild(createTreeIcon(deviceType, meterCommodity));
        li.appendChild(createSpan(branchId, identifier));

        baseElement.appendChild(li); 
    }
}

function createBranchDiv(branchDivId, childrenCreated = true) {
    var branchDiv = document.createElement('div');
    branchDiv.id = branchDivId;

    if(childrenCreated) {
        branchDiv.setAttribute('class', 'far fa-plus-square');
    }
    else {
        branchDiv.setAttribute('class', 'far fa-times-circle');
    }

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

function createTreeIcon(branch, commodity) {
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

function createCheckbox(checkboxId, checkboxFunction, branch, linkedSite, guid, linkedCustomer) {
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

    if(linkedCustomer) {
        checkBox.setAttribute('LinkedCustomer', linkedCustomer);
    }

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

function commoditySiteMatch(site, commodity) {
    if(commodity == '') {
        return true;
    }

    if(!site.hasOwnProperty('Meters')) {
        return false;
    }

    var metersLength = site.Meters.length;
    for(var i = 0; i < metersLength; i++) {
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
        default:
            return 'fas fa-map-marker-alt';
    }
}