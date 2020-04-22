function pageLoad() {
  createTrees();  

  document.onmousemove = function(e) {
    setupSidebarHeight();
    setupSidebar(e);
  };

  window.onscroll = function() {
    setupSidebarHeight();
  };

  window.onload = function() {
    hideSliders(); 
  }
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
  var electricityCommodityradio = document.getElementById('electricityCommoditySelectorradio');
  if(electricityCommodityradio.checked) {
    commodity = 'Electricity';
  }
  else {
    var gasCommodityradio = document.getElementById('gasCommoditySelectorradio');
    if(gasCommodityradio.checked) {
      commodity = 'Gas';
    }
  }
  return commodity;
}

function createTrees() {
  createCommodityTree();
  createDisplayTree();
  createBudgetTree();
  createSiteTree(sites, "updatePage()");
  createInvoiceTree();
  createGroupingOptionTree();
  createTimePeriodTree();

  addExpanderOnClickEvents();
}

function createDisplayTree() {
  var div = document.getElementById('displayTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("displayHeader", "Select Display");
  var ul = createBranchUl("displaySelector", false, true);

  div.appendChild(headerDiv);
  div.appendChild(ul);

  createDisplayListItems(ul);
}

function createDisplayListItems(ul) {
  var costDisplayListItem = appendListItemChildren('costDisplaySelector', true, 'updatePage()', [{"Name" : "Cost"}], 'displaySelector', true, 'radio', 'displayGroup');
  var usageDisplayListItem = appendListItemChildren('usageDisplaySelector', true, 'updatePage()', [{"Name" : "Usage"}], 'displaySelector', false, 'radio', 'displayGroup');
  var rateDisplayListItem = appendListItemChildren('rateDisplaySelector', false, 'updatePage()', [{"Name" : "Rate"}], 'displaySelector', false, 'radio', 'displayGroup');
  
  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';

  var granularityDisplayListItem = document.createElement('li');
  var granularityDisplaySpan = createBranchSpan('granularityDisplaySpan', "Granularity<br>");
  var granularityDisplayRZSlider = document.createElement('rzslider');
  granularityDisplayRZSlider.id = 'timePeriodOptionsDisplayTimeSpan';
  granularityDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsDisplayTimeSpan.value');
  granularityDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsDisplayTimeSpan.options');

  granularityDisplayListItem.appendChild(granularityDisplaySpan);
  granularityDisplayListItem.appendChild(granularityDisplayRZSlider);

  var dateRangeDisplayListItem = document.createElement('li');
  var dateRangeDisplaySpan = createBranchSpan('dateRangeDisplaySpan', "Date Range<br>");
  var dateRangeDisplayRZSlider = document.createElement('rzslider');
  dateRangeDisplayRZSlider.id = 'timePeriodOptionsDisplayDateRange';
  dateRangeDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsDisplayDateRange.minValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-high', 'timePeriodOptionsDisplayDateRange.maxValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsDisplayDateRange.options');

  dateRangeDisplayListItem.appendChild(dateRangeDisplaySpan);
  dateRangeDisplayListItem.appendChild(dateRangeDisplayRZSlider);

  ul.appendChild(costDisplayListItem);
  ul.appendChild(usageDisplayListItem);
  ul.appendChild(rateDisplayListItem);
  ul.appendChild(breakDisplayListItem);
  ul.appendChild(granularityDisplayListItem);
  ul.appendChild(breakDisplayListItem);
  ul.appendChild(dateRangeDisplayListItem);

  createCostDisplayListItems(costDisplayListItem);
  createUsageDisplayListItems(usageDisplayListItem);
}

function createCostDisplayListItems(costDisplayListItem) {
  var costDisplaySelectorListUl = costDisplayListItem.getElementsByTagName('ul')[0];
  var allCostItemsListItem = appendListItemChildren('allCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "All Cost Items"}], 'costDisplaySelector', true, 'radio', 'costDisplayGroup');
  var networkCostItemsListItem = appendListItemChildren('networkCostItemsCostDisplaySelector', true, 'updatePage()', [{"Name" : "Network"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var renewablesCostItemsListItem = appendListItemChildren('renewablesCostItemsCostDisplaySelector', true, 'updatePage()', [{"Name" : "Renewables"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var balancingCostItemsListItem = appendListItemChildren('balancingCostItemsCostDisplaySelector', true, 'updatePage()', [{"Name" : "Balancing"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var otherCostItemsListItem = appendListItemChildren('otherCostItemsCostDisplaySelector', true, 'updatePage()', [{"Name" : "Other"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');

  costDisplaySelectorListUl.appendChild(allCostItemsListItem);
  costDisplaySelectorListUl.appendChild(networkCostItemsListItem);
  costDisplaySelectorListUl.appendChild(renewablesCostItemsListItem);
  costDisplaySelectorListUl.appendChild(balancingCostItemsListItem);
  costDisplaySelectorListUl.appendChild(otherCostItemsListItem);

  createNetworkCostDisplayListItems(networkCostItemsListItem);
  createRenewablesCostDisplayListItems(renewablesCostItemsListItem);
  createBalancingCostDisplayListItems(balancingCostItemsListItem);
  createOtherCostDisplayListItems(otherCostItemsListItem);
}

function createUsageDisplayListItems(usageDisplayListItem) {
  var usageDisplaySelectorListUl = usageDisplayListItem.getElementsByTagName('ul')[0];
  var consumptionUsageItemsListItem = appendListItemChildren('consumptionUsageItemsUsageDisplaySelector', false, 'updatePage()', [{"Name" : "Consumption"}], 'usageDisplaySelector', false, 'checkbox', 'usageDisplayGroup');
  var capacityUsageItemsListItem = appendListItemChildren('capacityUsageItemsUsageDisplaySelector', false, 'updatePage()', [{"Name" : "Capacity"}], 'usageDisplaySelector', false, 'checkbox', 'usageDisplayGroup');

  usageDisplaySelectorListUl.appendChild(consumptionUsageItemsListItem);
  usageDisplaySelectorListUl.appendChild(capacityUsageItemsListItem);
}

function createNetworkCostDisplayListItems(networkCostItemsListItem) {
  var networkCostDisplaySelectorListUl = networkCostItemsListItem.getElementsByTagName('ul')[0];
  var wholesaleNetworkCostItemsListItem = appendListItemChildren('wholesaleNetworkCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Wholesale"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var distributionNetworkCostItemsListItem = appendListItemChildren('distributionNetworkCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Distribution"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var transmissionNetworkCostItemsListItem = appendListItemChildren('transmissionNetworkCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Transmission"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');

  networkCostDisplaySelectorListUl.appendChild(wholesaleNetworkCostItemsListItem);
  networkCostDisplaySelectorListUl.appendChild(distributionNetworkCostItemsListItem);
  networkCostDisplaySelectorListUl.appendChild(transmissionNetworkCostItemsListItem);
}

function createRenewablesCostDisplayListItems(renewablesCostItemsListItem) {
  var renewablesCostDisplaySelectorListUl = renewablesCostItemsListItem.getElementsByTagName('ul')[0];
  var renewablesObligationRenewablesCostItemsListItem = appendListItemChildren('renewablesObligationRenewablesCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Renewables Obligation"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var feedInTariffRenewablesCostItemsListItem = appendListItemChildren('feedInTariffRenewablesCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Feed In Tariff"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var contractsForDifferenceRenewablesCostItemsListItem = appendListItemChildren('contractsForDifferenceRenewablesCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Contracts For Difference"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var energyIntensiveIndustryRenewablesCostItemsListItem = appendListItemChildren('energyIntensiveIndustryRenewablesCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Energy Intensive Industry"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var capacityMarketRenewablesCostItemsListItem = appendListItemChildren('capacityMarketRenewablesCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Capacity Markets"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');

  renewablesCostDisplaySelectorListUl.appendChild(renewablesObligationRenewablesCostItemsListItem);
  renewablesCostDisplaySelectorListUl.appendChild(feedInTariffRenewablesCostItemsListItem);
  renewablesCostDisplaySelectorListUl.appendChild(contractsForDifferenceRenewablesCostItemsListItem);
  renewablesCostDisplaySelectorListUl.appendChild(energyIntensiveIndustryRenewablesCostItemsListItem);
  renewablesCostDisplaySelectorListUl.appendChild(capacityMarketRenewablesCostItemsListItem);
}

function createBalancingCostDisplayListItems(balancingCostItemsListItem) {
  var balancingCostDisplaySelectorListUl = balancingCostItemsListItem.getElementsByTagName('ul')[0];
  var bsuosBalancingCostItemsListItem = appendListItemChildren('bsuosBalancingCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Balancing System Use Of System"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');
  var rcrcBalancingCostItemsListItem = appendListItemChildren('rcrcBalancingCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Residual Cashflow Reallocation Cashflow"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');

  balancingCostDisplaySelectorListUl.appendChild(bsuosBalancingCostItemsListItem);
  balancingCostDisplaySelectorListUl.appendChild(rcrcBalancingCostItemsListItem);
}

function createOtherCostDisplayListItems(otherCostItemsListItem) {
  var otherCostDisplaySelectorListUl = otherCostItemsListItem.getElementsByTagName('ul')[0];
  var otherNetworkCostItemsListItem = appendListItemChildren('otherNetworkCostItemsCostDisplaySelector', false, 'updatePage()', [{"Name" : "Other"}], 'costDisplaySelector', false, 'checkbox', 'costDisplayGroup');

  otherCostDisplaySelectorListUl.appendChild(otherNetworkCostItemsListItem);
}

function createBudgetTree() {
  var div = document.getElementById('budgetTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("budgetHeader", "Select Budget");
  var ul = createBranchUl("budgetSelector", false, true);

  var budget2BudgetListItem = appendListItemChildren('budget2BudgetSelector', true, 'updatePage()', [{"Name" : "Budget 2"}], 'budgetSelector', false, 'radio', 'budgetGroup');
  var budget1BudgetListItem = appendListItemChildren('budget1BudgetSelector', false, 'updatePage()', [{"Name" : "Budget 1"}], 'budgetSelector', false, 'checkbox', 'budgetGroup');

  ul.appendChild(budget2BudgetListItem);
  ul.appendChild(budget1BudgetListItem);


  var budget2BudgetSelectorListUl = budget2BudgetListItem.getElementsByTagName('ul')[0];
  var version2Budget2ListItem = appendListItemChildren('version2Budget2CostBudgetSelector', false, 'updatePage()', [{"Name" : "Version 2"}], 'budget2BudgetSelector', false, 'checkbox', 'budget2BudgetGroup');
  var version1Budget2ListItem = appendListItemChildren('version1Budget2CostBudgetSelector', false, 'updatePage()', [{"Name" : "Version 1"}], 'budget2BudgetSelector', false, 'checkbox', 'budget2BudgetGroup');

  budget2BudgetSelectorListUl.appendChild(version2Budget2ListItem);
  budget2BudgetSelectorListUl.appendChild(version1Budget2ListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createSiteTree(sites, functions) {
  var div = document.getElementById('siteTree');
  var inputs = div.getElementsByTagName('input');
  var checkboxes = getCheckedCheckBoxes(inputs);

  if(checkboxes.length == 0) {
    checkboxes = getBranchCheckboxes(inputs, 'Site');
  }

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

  var headerDiv = createHeaderDiv("siteHeader", 'Select Sites/Meters');
  var ul = createBranchUl("siteSelector", false, true);

  buildSiteBranch(sites, getCommodityOption(), ul, functions);  

  div.appendChild(headerDiv);
  div.appendChild(ul);

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

function createInvoiceTree() {
  var div = document.getElementById('invoiceTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("invoiceHeader", "Select Invoice");
  var ul = createBranchUl("invoiceSelector", true, true);

  var invoice4InvoiceListItem = appendListItemChildren('invoice4InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0004"}], 'invoiceSelector', false, 'checkbox', 'invoiceGroup');
  var invoice3InvoiceListItem = appendListItemChildren('invoice3InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0003"}], 'invoiceSelector', false, 'checkbox', 'invoiceGroup');
  var invoice2InvoiceListItem = appendListItemChildren('invoice2InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0002"}], 'invoiceSelector', false, 'checkbox', 'invoiceGroup');
  var invoice1InvoiceListItem = appendListItemChildren('invoice1InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0001"}], 'invoiceSelector', false, 'checkbox', 'invoiceGroup');

  ul.appendChild(invoice4InvoiceListItem);
  ul.appendChild(invoice3InvoiceListItem);
  ul.appendChild(invoice2InvoiceListItem);
  ul.appendChild(invoice1InvoiceListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createGroupingOptionTree() {
  var div = document.getElementById('groupingOptionTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("groupingOptionHeader", "Select Grouping Option");
  var ul = createBranchUl("groupingOptionSelector", true, true);

  var groupingOption2GroupingOptionListItem = appendListItemChildren('groupingOption2GroupingOptionSelector', false, 'updatePage()', [{"Name" : "No Grouping"}], 'groupingOptionSelector', true, 'radio', 'groupingOptionGroup');
  var groupingOption1GroupingOptionListItem = appendListItemChildren('groupingOption1GroupingOptionSelector', true, 'updatePage()', [{"Name" : "Group"}], 'groupingOptionSelector', false, 'radio', 'groupingOptionGroup');

  ul.appendChild(groupingOption2GroupingOptionListItem);
  ul.appendChild(groupingOption1GroupingOptionListItem);


  var groupingOption1GroupingOptionSelectorListUl = groupingOption1GroupingOptionListItem.getElementsByTagName('ul')[0];
  var sumGroupingOption1ListItem = appendListItemChildren('sumGroupingOption1CostGroupingOptionSelector', false, 'updatePage()', [{"Name" : "Sum"}], 'groupingOption1GroupingOptionSelector', false, 'checkbox', 'groupingOption1GroupingOptionGroup');
  var averageGroupingOption1ListItem = appendListItemChildren('averageGroupingOption1CostGroupingOptionSelector', false, 'updatePage()', [{"Name" : "Average"}], 'groupingOption1GroupingOptionSelector', false, 'checkbox', 'groupingOption1GroupingOptionGroup');

  groupingOption1GroupingOptionSelectorListUl.appendChild(sumGroupingOption1ListItem);
  groupingOption1GroupingOptionSelectorListUl.appendChild(averageGroupingOption1ListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createCommodityTree() {
  var div = document.getElementById('commodityTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("commodityHeader", "Filter By Commodity");
  var ul = createBranchUl("commoditySelector", true, true);

  var allCommodityListItem = appendListItemChildren('allCommoditySelector', false, 'updatePage()', [{"Name" : "All"}], 'commoditySelector', true, 'radio', 'commodityGroup');
  var electricityCommodityListItem = appendListItemChildren('electricityCommoditySelector', false, 'updatePage()', [{"Name" : "Electricity"}], 'commoditySelector', false, 'radio', 'commodityGroup');
  var gasCommodityListItem = appendListItemChildren('gasCommoditySelector', false, 'updatePage()', [{"Name" : "Gas"}], 'commoditySelector', false, 'radio', 'commodityGroup');

  ul.appendChild(allCommodityListItem);
  ul.appendChild(electricityCommodityListItem);
  ul.appendChild(gasCommodityListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createTimePeriodTree() {
  var div = document.getElementById('timePeriodTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("timePeriodHeader", "Filter By Time Period");
  var ul = createBranchUl("timePeriodSelector", false, true);
  ul.classList.add("slider-list");

  var dateRangeDisplayListItem = document.createElement('li');
  var dateRangeDisplaySpan = createBranchSpan('dateRangeDisplaySpan', "Date Range");
  var dateRangeDisplayRZSlider = document.createElement('rzslider');
  dateRangeDisplayRZSlider.id = 'timePeriodOptionsFilterDateRange';
  dateRangeDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsFilterDateRange.minValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-high', 'timePeriodOptionsFilterDateRange.maxValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsFilterDateRange.options');

  dateRangeDisplayListItem.appendChild(dateRangeDisplaySpan);
  dateRangeDisplayListItem.appendChild(dateRangeDisplayRZSlider);

  var createdDisplayListItem = document.createElement('li');
  var createdDisplaySpan = createBranchSpan('createdDisplaySpan', "Created");
  var createdDisplayRZSlider = document.createElement('rzslider');
  createdDisplayRZSlider.id = 'timePeriodOptionsFilteredCreated';
  createdDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsFilteredCreated.value');
  createdDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsFilteredCreated.options');

  createdDisplayListItem.appendChild(createdDisplaySpan);
  createdDisplayListItem.appendChild(createdDisplayRZSlider);

  ul.appendChild(dateRangeDisplayListItem);
  ul.appendChild(createdDisplayListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createHeaderDiv(id, headerText) {
  var headerDiv = document.createElement('div');
  headerDiv.id = id;
  headerDiv.setAttribute('class', 'expander-header');

  var header = document.createElement('span');
  header.innerText = headerText;

  var headerExpander = createBranchDiv(id.replace('Header', 'Selector'), true);

  headerDiv.appendChild(header);
  headerDiv.appendChild(headerExpander);

  return headerDiv;
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

function appendListItemChildren(id, hasChildren, functions, attributes, branch, isChecked = false, elementType = 'checkbox', groupName = '') {
  var li = document.createElement('li');
  li.appendChild(createBranchDiv(id, hasChildren));
  li.appendChild(createBranchCheckbox(id, functions, branch, elementType, groupName, isChecked));

  if(getAttribute(attributes, 'Icon')) {
    li.appendChild(createBranchIcon(getAttribute(attributes, 'Icon')));
  }
  
  li.appendChild(createBranchSpan(id, getAttribute(attributes, 'Name')));

  if(hasChildren) {
    li.appendChild(createBranchUl(id));
  }

  return li;
}

function createBranchUl(id, hideUl = true, isTopUl = false) {
  var ul = document.createElement('ul');
  ul.id = id + 'List';
  ul.setAttribute('class', 'format-listitem'
   + (hideUl ? ' listitem-hidden' : '')
   + (isTopUl ? ' topListItem' : ''));
  return ul;
}

function createBranchDiv(id, hasChildren = true) {
    var branchDiv = document.createElement('i');
    branchDiv.id = id;
    branchDiv.setAttribute('class', (hasChildren ? 'far fa-plus-square show-pointer' : 'far fa-times-circle') + ' expander');
    return branchDiv;
}

function createBranchCheckbox(id, functions, branch, elementType, groupName, isChecked) {
  var checkbox = document.createElement('input');
  checkbox.type = elementType;  
  checkbox.id = id.concat(elementType);
  checkbox.setAttribute('branch', branch);
  checkbox.setAttribute('name', groupName);
  checkbox.checked = isChecked;

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
  icon.setAttribute('class', iconClass + ' listitem-icon');
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

  return checkBoxes;
}

function getBranchCheckboxes(inputs, branch) {
  var checkBoxes = [];
  var inputLength = inputs.length;

  for(var i = 0; i < inputLength; i++) {
    if(inputs[i].type.toLowerCase() == 'checkbox') {
      if(inputs[i].getAttribute('branch') == branch) {
        inputs[i].checked = true;
        checkBoxes.push(inputs[i]);
      }
    }
  }

  return checkBoxes;
}

function updatePage(callingElement) {
  var branch = callingElement.getAttribute('branch');
  
  switch(branch) {
    case 'invoiceSelector':
      updatePageFromInvoice(callingElement);
      break;
    default:
      alert(callingElement.id);
      break;
  }
}

function updatePageFromInvoice(callingElement) {
  if(callingElement.checked) {
    var timePeriodOptionsTimeSpan = document.getElementById('timePeriodOptionsTimeSpan');
    var granularity = timePeriodOptionsTimeSpan.children[6].innerHTML;

    if(granularity == 'Half Hourly' || granularity == 'Daily') {
      makeTimePeriodOptionsTimeSpanMonthly();
    }
  }

  updateCharts();
}

function makeTimePeriodOptionsTimeSpanMonthly() {
  var scope = angular.element(document.getElementById("timePeriodOptionsTimeSpan")).scope();
  scope.$apply(function () {
    scope.makeTimePeriodOptionsTimeSpanMonthly();
  });
}

function updateCharts() {
  checkTimePeriodOptionsTimeSpan();
}

function checkTimePeriodOptionsTimeSpan() {
  var invoices = document.getElementById('invoiceTree');
  var inputs = invoices.getElementsByTagName('input');
  var checkboxes = getCheckedCheckBoxes(inputs);

  var timePeriodOptionsTimeSpan = document.getElementById('timePeriodOptionsTimeSpan');
  var granularity = timePeriodOptionsTimeSpan.children[6].innerHTML;

  if(granularity == 'Half Hourly' || granularity == 'Daily') {
    for(var i = 0; i < checkboxes.length; i++) {
      checkboxes[i].checked = false;
    }
  }
}