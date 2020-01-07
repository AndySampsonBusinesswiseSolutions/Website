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

        buildCommodity(base.Commodities, ul, checkboxFunction, baseName);
        appendListItemChildren(li, 'Site'.concat(base.GUID), checkboxFunction, 'Site', baseName, ul, baseName, base.GUID);

        baseElement.appendChild(li);        
    }
}

function buildCommodity(commodities, baseElement, checkboxFunction, linkedSite) {
    var commoditiesLength = commodities.length;
    for(var i = 0; i < commoditiesLength; i++) {
        var commodity = commodities[i];
        var li = document.createElement('li');

        var ul = createUL();
        buildProfileClass(commodity.ProfileClasses, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'Commodity'.concat(branchCount), checkboxFunction, 'Commodity', commodity.CommodityName, ul, linkedSite, '');

        baseElement.appendChild(li);
        branchCount++;
    }
}

function buildProfileClass(profileClasses, baseElement, checkboxFunction, linkedSite) {
    var profileClassesLength = profileClasses.length;
    for(var i = 0; i < profileClassesLength; i++) {
        var profileClass = profileClasses[i];
        var li = document.createElement('li');
        var ul = createUL();
        buildProduct(profileClass.Products, ul, checkboxFunction, linkedSite);
        appendListItemChildren(li, 'ProfileClass'.concat(subBranchCount), checkboxFunction, 'ProfileClass', profileClass.MeterType, ul, linkedSite, '');

        baseElement.appendChild(li);
        subBranchCount++;
    }
}

function appendListItemChildren(li, id, checkboxFunction, checkboxBranch, branchOption, ul, linkedSite, guid) {
    li.appendChild(createBranchDiv(id));
    li.appendChild(createTreeIcon(branchOption));
    li.appendChild(createSpan(id, branchOption));
    li.appendChild(createBranchListDiv(id.concat('List'), ul));
}

function buildProduct(products, baseElement, checkboxFunction, linkedSite) {
    var productsLength = products.length;
    for(var i = 0; i < productsLength; i++){
        var product = products[i];
        var productAttributes = product.Attributes;
        var identifier = getAttribute(productAttributes, 'ProductName');
        var li = document.createElement('li');
        var branchId = 'Meter'.concat(product.GUID);
        var branchDiv = createBranchDiv(branchId);
        
        branchDiv.removeAttribute('class', 'far fa-plus-square');
        branchDiv.setAttribute('class', 'far fa-times-circle');

        li.appendChild(branchDiv);
        li.appendChild(createCheckbox(branchId, checkboxFunction, 'Product', linkedSite, product.GUID));
        li.appendChild(createTreeIcon('Product'));
        li.appendChild(createSpan(branchId, identifier));

        baseElement.appendChild(li); 
    }
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
    switch (branch) {
        case 'Unknown':
            return 'fas fa-question-circle';
        default:
            return 'fas fa-map-marker-alt';
    }
}