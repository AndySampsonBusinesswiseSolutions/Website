function pageLoad() {
  createTree(sites, "");

  document.onmousemove = function(e) {
    setupSidebarHeight();
    setupSidebar(e);
  };

  window.onscroll = function() {
    setupSidebarHeight();
  };

  window.onload = function() {
    hideSliders();
  };
}

function setupSidebarHeight() {
  var navBar = document.getElementsByClassName('fusion-header-wrapper')[0];
  var footer = document.getElementsByClassName('footer')[0];
  var sidebar = document.getElementById("mySidenav");
  var navBarHeight = navBar.clientHeight;
  var footerHeight = footer.clientHeight;
  var reduction = (window.innerHeight + window.scrollY) - (document.body.offsetHeight - footerHeight);
    
  sidebar.style.height = "calc(100% - " + (reduction > 0 ? reduction : 0) + "px)";
  sidebar.style.marginTop = window.pageYOffset >= navBarHeight ? '0px' : (navBarHeight - window.pageYOffset) + 'px';
}

function getMousePos(e) {
	return {x:e.clientX,y:e.clientY};
}

function setupSidebar(e) {
  var sidebar = document.getElementById("mySidenav");
  var currentSidebarWidth = sidebar.style.width == '' ? 0 : parseInt(sidebar.style.width.replace('px', ''));
  var newSidebarWidth = (sidebar.scrollHeight > sidebar.clientHeight ? 415 : 400);

  if(currentSidebarWidth > 0 && newSidebarWidth != currentSidebarWidth) {
    sidebar.style.width = newSidebarWidth + "px";
  }

  var mousecoords = getMousePos(e);
  if(currentSidebarWidth == 0 && mousecoords.x <= 15) {
    openNav(sidebar, newSidebarWidth);
  }  
  else if(currentSidebarWidth > 0 && mousecoords.x >= newSidebarWidth) {
    closeNav(sidebar);
  }  
}

function lockSidebar() {
  updateClassOnClick('lock', 'fa-unlock', 'fa-lock'); 
  var lock = document.getElementsByClassName('lock')[0];
  var documentBody = document.getElementsByClassName('final-column')[0];
  var sidebar = document.getElementById("mySidenav"); 

  if(lock.classList.contains('fa-lock')) {
    documentBody.style.marginLeft = sidebar.style.width;
    lock.title = "Click To Unlock Sidebar";
  }
  else {
    documentBody.style.marginLeft = "0px";
    lock.title = "Click To Lock Sidebar";
  }
}

function openNav(sidebar, newSidebarWidth) {
	sidebar.style.width = newSidebarWidth + "px";
  document.getElementById("openNav").style.color = "#b62a51";
}

function closeNav(sidebar) {
  var lock = document.getElementsByClassName('lock')[0];

  if(lock.classList.contains('fa-unlock')) {
	  document.getElementById("openNav").style.color = "white";
    sidebar.style.width = "0px";
  }
}

function hideSliders() {
  var sliders = document.getElementsByClassName('slider-list');
  [...sliders].forEach(slider => {
    slider.classList.add('listitem-hidden');
  });
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
  
  updateClassOnClick('chartHeader', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('budgetSelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('siteSelector', 'fa-plus-square', 'fa-minus-square');
  updateClassOnClick('displaySelector', 'fa-plus-square', 'fa-minus-square');
}

function addExpanderOnClickEventsByElement(element) {
	element.addEventListener('click', function (event) {
		updateClassOnClick(this.id, 'fa-plus-square', 'fa-minus-square');
    updateClassOnClick(this.id.concat('List'), 'listitem-hidden', '');
    setupSidebar(event);
	});
}

function getCommodityOption() {
  var commodity = '';
  var electricityCommodityradio = document.getElementById('electricityCommodityradio');
  if(electricityCommodityradio.checked) {
    commodity = 'Electricity';
  }
  else {
    var gasCommodityradio = document.getElementById('gasCommodityradio');
    if(gasCommodityradio.checked) {
      commodity = 'Gas';
    }
  }
  return commodity;
}

function createTree(sites, functions) {
  var div = document.getElementById('siteDiv');
  var inputs = div.getElementsByTagName('input');
  var checkboxes = getCheckedCheckBoxes(inputs);
  var elements = div.getElementsByTagName("*");

  var checkboxIds = [];
  for(var i = 0; i < checkboxes.length; i++) {
    checkboxIds.push(checkboxes[i].id);
  }

  var elementClasses = [];
  for(var i = 0; i < elements.length; i++) {
    if(elements[i].id != '') {
      var element = {
        id: elements[i].id,
        classList: elements[i].classList
      }
  
      elementClasses.push(element);
    }    
  }

  clearElement(div);

  var headerDiv = document.createElement('div');
  headerDiv.id = "siteHeader";
  headerDiv.setAttribute('class', 'expander-header');

  var header = document.createElement('span');
  header.style = "padding-left: 5px;";
  header.innerHTML = 'Select Sites/Meters';

  var headerExpander = createBranchDiv("siteSelector", true);

  headerDiv.appendChild(header);
  headerDiv.appendChild(headerExpander);
  
  var tree = document.createElement('div');
  tree.setAttribute('style', 'margin-top: 5px;');
  
  var ul = createBranchUl('siteSelector', false);
  tree.appendChild(ul);

  buildSiteBranch(sites, getCommodityOption(), ul, functions);  

  div.appendChild(headerDiv);
  div.appendChild(tree);

  addExpanderOnClickEvents();

  for(var i = 0; i < checkboxIds.length; i++) {
    var checkbox = document.getElementById(checkboxIds[i]);
    if(checkbox) {
      checkbox.checked = true;
    }
  }

  for(var i = 0; i < elementClasses.length; i++) {
    var element = document.getElementById(elementClasses[i].id);
    if(element) {
      element.classList = elementClasses[i].classList;
    }
  }  
}

//build site
function buildSiteBranch(sites, commodityOption, elementToAppendTo, functions) {
  var siteLength = sites.length;

  for(var siteCount = 0; siteCount < siteLength; siteCount++) {
    var site = sites[siteCount];

    if(!commodityMatch(site, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren('Site' + siteCount, site.hasOwnProperty('Areas'), functions, site.Attributes, 'Site');
    elementToAppendTo.appendChild(listItem);

    if(site.hasOwnProperty('Areas')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildAreaBranch(site.Areas, commodityOption, ul, functions, 'Site' + siteCount);
    }
  }
}

//build area
function buildAreaBranch(areas, commodityOption, elementToAppendTo, functions, previousId) {
  var areaLength = areas.length;

  for(var areaCount = 0; areaCount < areaLength; areaCount++) {
    var area = areas[areaCount];

    if(!commodityMatch(area, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Area' + areaCount, area.hasOwnProperty('Commodities'), functions, area.Attributes, 'Area');
    elementToAppendTo.appendChild(listItem);

    if(area.hasOwnProperty('Commodities')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildCommodityBranch(area.Commodities, commodityOption, ul, functions, previousId + '_Area' + areaCount);
    }
  }
}

//build commodity
function buildCommodityBranch(commodities, commodityOption, elementToAppendTo, functions, previousId) {
  var commodityLength = commodities.length;

  for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
    var commodity = commodities[commodityCount];

    if(!commodityMatch(commodity, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Commodity' + commodityCount, commodity.hasOwnProperty('Meters'), functions, commodity.Attributes, 'Commodity');
    elementToAppendTo.appendChild(listItem);

    if(commodity.hasOwnProperty('Meters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildMeterBranch(commodity.Meters, commodityOption, ul, functions, previousId + '_Commodity' + commodityCount);
    }
  }
}

//build meter
function buildMeterBranch(meters, commodityOption, elementToAppendTo, functions, previousId) {
  var meterLength = meters.length;

  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = meters[meterCount];

    if(!commodityMatch(meter, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Meter' + meterCount, meter.hasOwnProperty('SubAreas'), functions, meter.Attributes, 'Meter');
    elementToAppendTo.appendChild(listItem);

    if(meter.hasOwnProperty('SubAreas')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubAreaBranch(meter.SubAreas, commodityOption, ul, functions, previousId + '_Meter' + meterCount);
    }
  }
}

//build sub area
function buildSubAreaBranch(subAreas, commodityOption, elementToAppendTo, functions, previousId) {
  var subAreaLength = subAreas.length;

  for(var subAreaCount = 0; subAreaCount < subAreaLength; subAreaCount++) {
    var subArea = subAreas[subAreaCount];

    if(!commodityMatch(subArea, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_SubArea' + subAreaCount, subArea.hasOwnProperty('Assets'), functions, subArea.Attributes, 'SubArea');
    elementToAppendTo.appendChild(listItem);

    if(subArea.hasOwnProperty('Assets')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildAssetBranch(subArea.Assets, commodityOption, ul, functions, previousId + '_SubArea' + subAreaCount);
    }
  }
}

//build asset
function buildAssetBranch(assets, commodityOption, elementToAppendTo, functions, previousId) {
  var assetLength = assets.length;

  for(var assetCount = 0; assetCount < assetLength; assetCount++) {
    var asset = assets[assetCount];

    if(!commodityMatch(asset, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_Asset' + assetCount, asset.hasOwnProperty('SubMeters'), functions, asset.Attributes, 'Asset');
    elementToAppendTo.appendChild(listItem);

    if(asset.hasOwnProperty('SubMeters')) {
      var ul = listItem.getElementsByTagName('ul')[0];
      buildSubMeterBranch(asset.SubMeters, commodityOption, ul, functions, previousId + '_Asset' + assetCount);
    }
  }
}

//build sub meter
function buildSubMeterBranch(subMeters, commodityOption, elementToAppendTo, functions, previousId) {
  var subMeterLength = subMeters.length;

  for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++) {
    var subMeter = subMeters[subMeterCount];

    if(!commodityMatch(subMeter, commodityOption)) {
      continue;
    }

    var listItem = appendListItemChildren(previousId + '_SubMeter' + subMeterCount, false, functions, subMeter.Attributes, 'SubMeter');
    elementToAppendTo.appendChild(listItem);
  }
}

function commodityMatch(entity, commodity) {
  if(commodity == '') {
      return true;
  }

  var entityCommodities = getAttribute(entity.Attributes, 'Commodities');
  return entityCommodities && entityCommodities.includes(commodity);
}

function appendListItemChildren(id, hasChildren, functions, attributes, branch) {
  var li = document.createElement('li');
  li.appendChild(createBranchDiv(id, hasChildren));
  li.appendChild(createBranchCheckbox(id, functions, branch));
  li.appendChild(createBranchIcon(getAttribute(attributes, 'Icon')));
  li.appendChild(createBranchSpan(id, getAttribute(attributes, 'Name')));

  if(hasChildren) {
    li.appendChild(createBranchUl(id));
  }

  return li;
}

function createBranchUl(id, hideUl = true) {
  var ul = document.createElement('ul');
  ul.id = id.concat('List');
  ul.setAttribute('class', 'format-listitem' + (hideUl ? ' listitem-hidden' : ''));
  return ul;
}

function createBranchDiv(branchDivId, hasChildren = true) {
    var branchDiv = document.createElement('i');
    branchDiv.id = branchDivId;
    branchDiv.setAttribute('class', (hasChildren ? 'far fa-plus-square show-pointer' : 'far fa-times-circle') + ' expander');
    return branchDiv;
}

function createBranchCheckbox(id, functions, branch) {
  var checkbox = document.createElement('input');
  checkbox.type = 'checkbox';  
  checkbox.id = id.concat('checkbox');
  checkbox.setAttribute('branch', branch);

  var functionArray = functions.replace(')', '').split('(');
  var functionArrayLength = functionArray.length;
  var functionName = functionArray[0];
  var functionArguments = [];

  functionArguments.push(checkbox.id);
  if(functionArrayLength > 1) {
      var functionArgumentLength = functionArray[1].split(',').length;
      for(var i = 0; i < functionArgumentLength; i++) {
          functionArguments.push(functionArray[1].split(',')[i]);
      }
  }
  functionName = functionName.concat('(').concat(functionArguments.join(',').concat(')'));
  
  checkbox.setAttribute('onclick', functionName);
  return checkbox;
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

function createBranchIcon(iconClass) {
  var icon = document.createElement('i');
  icon.setAttribute('class', iconClass);
  icon.setAttribute('style', 'padding-left: 3px; padding-right: 3px;');
  return icon;
}

function createBranchSpan(id, innerHTML) {
  var span = document.createElement('span');
  span.id = id.concat('span');
  span.innerHTML = innerHTML;
  return span;
}

function clearElement(element) {
	while (element.firstChild) {
		element.removeChild(element.firstChild);
	}
}

function getCheckedCheckBoxes(inputs) {
  var checkBoxes = [];
  var inputLength = inputs.length;

  for(var i = 0; i < inputLength; i++) {
    if(inputs[i].type.toLowerCase() == 'checkbox') {
      if(inputs[i].checked) {
        checkBoxes.push(inputs[i]);
      }
    }
  }

  if(checkBoxes.length == 0) {
    for(var i = 0; i < inputLength; i++) {
      if(inputs[i].type.toLowerCase() == 'checkbox') {
        if(inputs[i].getAttribute('branch') == 'Site') {
          inputs[i].checked = true;
          checkBoxes.push(inputs[i]);
        }
      }
    }
  }

  return checkBoxes;
}