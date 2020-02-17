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
        var baseName = getAttribute(base.Attributes, 'Period');
        var li = document.createElement('li');
        var ul = createUL();

        buildSite(base.Sites, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Period'.concat(base.GUID), checkboxFunction, 'Period', baseName, ul, baseName, base.GUID);

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
        buildBill(meter.Bills, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Meter'.concat(meter.GUID), checkboxFunction, 'Meter', meter.Identifier, ul, linkedSite, '');

        baseElement.appendChild(li); 
    }
}

function buildBill(bills, baseElement, checkboxFunction, linkedSite) {
    var billsLength = bills.length;
    for(var i = 0; i < billsLength; i++){
        var bill = bills[i];

        var li = document.createElement('li');
        var ul = createUL();
        var branchId = 'Bill'.concat(bill.GUID);
        appendListItemChildren(li, branchId, checkboxFunction, 'Bill'.concat(bill.Status), bill.BillNumber, ul, linkedSite, bill.GUID);

        var branchDiv = li.children[branchId];
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        var branchIcon = li.children['Bill'.concat(bill.GUID).concat('span')];
        branchIcon.style.color = getBillStatusColour(bill.Status);

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

function getBillStatusColour(status) {
    switch(status) {
        case "Valid":
            return "green";
        case "Investigation":
            return "orange";
        case "Invalid":
            return "red";
    }
}