var categories;
var chartSeries;

async function pageLoad() {
  await createTrees(true);
  await updateChart(true);
  showElement(timePeriodOptionsFilteredCreated, false);
  showElement(timePeriodOptionsFilteredCreatedBreak, false);
  showElement(mySidenav, false);
  await showOverlay(false);
}

async function doneOnClick() {
  await showOverlay(true, false);
  await updateChart(false);
  closeNav();
  await showOverlay(false);
}

async function showOverlay(show, isPageLoading = true) {
  loader.style.opacity = isPageLoading ? 1 : 0.5;
  setElementDisplayStyle(loader, show ? '' : 'none');
}

function getCommodityOption() {
  if(electricityCommoditycheckbox.checked && gasCommoditycheckbox.checked) {
    return '';
  }
  else if(electricityCommoditycheckbox.checked) {
    return 'Electricity';
  }
  else if(gasCommoditycheckbox.checked) {
    return 'Gas';
  }

  return 'None';
}

function resetSlider() {
  var scope = angular.element(timePeriodOptionsDisplayTimeSpan).scope();
  scope.$apply(function () {
    scope.resetSliders();
  });
}

function resetSwitchButtons() {
  siteLocationcheckbox.checked = true;
  areaLocationcheckbox.checked = true;
  commodityLocationcheckbox.checked = true;
  meterLocationcheckbox.checked = true;
  subareaLocationcheckbox.checked = true;
  assetLocationcheckbox.checked = true;
  submeterLocationcheckbox.checked = true;
  electricityCommoditycheckbox.checked = true;
  gasCommoditycheckbox.checked = true;
}

async function createTrees(isPageLoad) {
  createEnergyUnitListItem();
  createEnergyUnitInstanceListItem();
  
  if(isPageLoad) {
    createCommodityListItem();
    createDateRangeDisplayListItem();
    createGranularityDisplayListItem();
    createGroupingOptionTree();
    createTimePeriodTree();
  }
  
  await createSiteTree(isPageLoad);
  
  addExpanderOnClickEvents();
  setOpenExpanders();
}

function createEnergyUnitListItem() {
  var div = document.getElementById('displayListItemTitlespan');
  clearElement(div);

  var headerDiv = createHeaderDiv("displayListItemTitleHeader", 'Energy Unit ', true);
  var ul = createBranchUl("displayListItemTitleSelector", false, true);

  var costDisplayListItem = appendListItemChildren('costDisplaySelector', true, 'updatePage()', [{"Name" : "Cost"}], 'displaySelector', false, 'radio', 'displayGroup');
  var usageDisplayListItem = appendListItemChildren('usageDisplaySelector', true, 'updatePage()', [{"Name" : "Usage"}], 'displaySelector', true, 'radio', 'displayGroup');
  var rateDisplayListItem = appendListItemChildren('rateDisplaySelector', true, 'updatePage()', [{"Name" : "Rate"}], 'displaySelector', false, 'radio', 'displayGroup');

  ul.appendChild(costDisplayListItem);
  ul.appendChild(usageDisplayListItem);
  ul.appendChild(rateDisplayListItem);
  div.appendChild(headerDiv);
  div.appendChild(ul);

  createCostRateDisplayListItems(costDisplayListItem, 'Cost');
  createUsageDisplayListItems(usageDisplayListItem);
  createCostRateDisplayListItems(rateDisplayListItem, 'Rate');
}

function createEnergyUnitInstanceListItem() {
  var div = document.getElementById('displayListItemAdditionalTitlespan');
  clearElement(div);

  var headerDiv = createHeaderDiv("displayListItemAdditionalTitleHeader", 'Energy Unit Instance', true);
  var ul = createBranchUl("displayListItemAdditionalTitleSelector", false, true);

  ul.appendChild(createForecastTree());
  ul.appendChild(createBudgetTree());
  ul.appendChild(createInvoiceTree());
  ul.appendChild(createVarianceTree());
  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createBreakDisplayListItem() {
  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';

  return breakDisplayListItem;
}

function createCommodityListItem() {
  var div = document.getElementById('commodityTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("commodityTreeHeader", 'Commodity', true);
  var ul = createBranchUl("commodityTreeSelector", false, true);

  var commodityListItem = document.createElement('li');
  commodityListItem.innerHTML = '<div class="scrolling-wrapper">'
    +'<div id="configureCommoditySelectorList" class="expander-container">'
    +'<div style="width: 45%; text-align: center; float: left;">'
    +'<span>Electricity</span>'
    +'<label class="switch"><input type="checkbox" id="electricityCommoditycheckbox" checked onclick="updatePage(this)" branch="commoditySelector"></input><div class="switch-btn"></div></label>'
    +'</div>'
    +'<div style="width: 10%; text-align: center; float: left; height: 1px;">'
    +'</div>'
    +'<div style="width: 45%; text-align: center; float: left;">'
    +'<span>Gas</span>'
    +'<label class="switch"><input type="checkbox" id="gasCommoditycheckbox" checked onclick="updatePage(this)" branch="commoditySelector"></input><div class="switch-btn"></div></label>'
    +'</div>'
    +'</div>'
    +'</div>'

  ul.appendChild(commodityListItem);
  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createDateRangeDisplayListItem() {
  var div = document.getElementById('dateRangeDisplay');
  clearElement(div);

  var headerDiv = createHeaderDiv("dateRangeDisplayHeader", 'Date Range', true);
  var ul = createBranchUl("dateRangeDisplaySelector", false, true);
  
  var dateRangeDisplayListItem = document.createElement('li');
  var dateRangeDisplayRZSlider = document.createElement('rzslider');
  dateRangeDisplayRZSlider.id = 'timePeriodOptionsDisplayDateRange';
  dateRangeDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsDisplayDateRange.minValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-high', 'timePeriodOptionsDisplayDateRange.maxValue');
  dateRangeDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsDisplayDateRange.options');

  dateRangeDisplayListItem.appendChild(dateRangeDisplayRZSlider);

  ul.appendChild(dateRangeDisplayListItem);
  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createGranularityDisplayListItem() {
  var div = document.getElementById('granularityTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("granularityTreeHeader", 'Granularity', true);
  var ul = createBranchUl("granularityTreeSelector", false, true);

  var granularityDisplayListItem = document.createElement('li');
  var granularityDisplayRZSlider = document.createElement('rzslider');
  granularityDisplayRZSlider.id = 'timePeriodOptionsDisplayTimeSpan';
  granularityDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsDisplayTimeSpan.value');
  granularityDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsDisplayTimeSpan.options');

  granularityDisplayListItem.appendChild(granularityDisplayRZSlider);

  ul.appendChild(granularityDisplayListItem);
  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createCostRateDisplayListItems(displayListItem, type) {
  var displaySelectorListUl = displayListItem.getElementsByTagName('ul')[0];
  var allItemsListItem = appendListItemChildren('all' + type + 'ItemsDisplaySelector', false, 'updatePage()', [{"Name" : "All " + type + " Items"}], type.toLowerCase() + 'DisplaySelector', true, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var networkItemsListItem = appendListItemChildren('network' + type + 'ItemsDisplaySelector', true, 'updatePage()', [{"Name" : "Network"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var renewablesItemsListItem = appendListItemChildren('renewables' + type + 'ItemsDisplaySelector', true, 'updatePage()', [{"Name" : "Renewables"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var balancingItemsListItem = appendListItemChildren('balancing' + type + 'ItemsDisplaySelector', true, 'updatePage()', [{"Name" : "Balancing"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var otherItemsListItem = appendListItemChildren('other' + type + 'ItemsDisplaySelector', true, 'updatePage()', [{"Name" : "Other"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  
  var resetButton = document.createElement('button');
  resetButton.id = 'reset' + type + 'Button';
  resetButton.innerText = 'Reset ' + type + ' Items';
  resetButton.style.float = 'right';

  var clearDiv = document.createElement('div');
  clearDiv.style.clear = 'both';

  displaySelectorListUl.appendChild(allItemsListItem);
  displaySelectorListUl.appendChild(networkItemsListItem);
  displaySelectorListUl.appendChild(renewablesItemsListItem);
  displaySelectorListUl.appendChild(balancingItemsListItem);
  displaySelectorListUl.appendChild(otherItemsListItem);
  displaySelectorListUl.appendChild(resetButton);
  displaySelectorListUl.appendChild(clearDiv);

  createNetworkDisplayListItems(networkItemsListItem, type);
  createRenewablesDisplayListItems(renewablesItemsListItem, type);
  createBalancingDisplayListItems(balancingItemsListItem, type);
  createOtherDisplayListItems(otherItemsListItem, type);
}

function createUsageDisplayListItems(usageDisplayListItem) {
  var usageDisplaySelectorListUl = usageDisplayListItem.getElementsByTagName('ul')[0];
  var consumptionUsageItemsListItem = appendListItemChildren('consumptionUsageItemsUsageDisplaySelector', false, 'updatePage()', [{"Name" : "Consumption"}], 'usageDisplaySelector', true, 'radio', 'usageDisplayGroup');
  var capacityUsageItemsListItem = appendListItemChildren('capacityUsageItemsUsageDisplaySelector', false, 'updatePage()', [{"Name" : "Capacity"}], 'usageDisplaySelector', false, 'radio', 'usageDisplayGroup');

  usageDisplaySelectorListUl.appendChild(consumptionUsageItemsListItem);
  usageDisplaySelectorListUl.appendChild(capacityUsageItemsListItem);
}

function createNetworkDisplayListItems(networkItemsListItem, type) {
  var networkDisplaySelectorListUl = networkItemsListItem.getElementsByTagName('ul')[0];
  var wholesaleNetworkItemsListItem = appendListItemChildren('wholesale' + type + 'NetworkItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Wholesale"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var distributionNetworkItemsListItem = appendListItemChildren('distribution' + type + 'NetworkItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Distribution"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var transmissionNetworkItemsListItem = appendListItemChildren('transmission' + type + 'NetworkItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Transmission"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');

  networkDisplaySelectorListUl.appendChild(wholesaleNetworkItemsListItem);
  networkDisplaySelectorListUl.appendChild(distributionNetworkItemsListItem);
  networkDisplaySelectorListUl.appendChild(transmissionNetworkItemsListItem);
}

function createRenewablesDisplayListItems(renewablesItemsListItem, type) {
  var renewablesDisplaySelectorListUl = renewablesItemsListItem.getElementsByTagName('ul')[0];
  var renewablesObligationRenewablesItemsListItem = appendListItemChildren('renewablesObligation' + type + 'RenewablesItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Renewables Obligation"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var feedInTariffRenewablesItemsListItem = appendListItemChildren('feedInTariff' + type + 'RenewablesItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Feed In Tariff"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var contractsForDifferenceRenewablesItemsListItem = appendListItemChildren('contractsForDifference' + type + 'RenewablesItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Contracts For Difference"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var energyIntensiveIndustryRenewablesItemsListItem = appendListItemChildren('energyIntensiveIndustry' + type + 'RenewablesItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Energy Intensive Industry"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var capacityMarketRenewablesItemsListItem = appendListItemChildren('capacity' + type + 'MarketRenewablesItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Capacity Markets"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');

  renewablesDisplaySelectorListUl.appendChild(renewablesObligationRenewablesItemsListItem);
  renewablesDisplaySelectorListUl.appendChild(feedInTariffRenewablesItemsListItem);
  renewablesDisplaySelectorListUl.appendChild(contractsForDifferenceRenewablesItemsListItem);
  renewablesDisplaySelectorListUl.appendChild(energyIntensiveIndustryRenewablesItemsListItem);
  renewablesDisplaySelectorListUl.appendChild(capacityMarketRenewablesItemsListItem);
}

function createBalancingDisplayListItems(balancingItemsListItem, type) {
  var balancingDisplaySelectorListUl = balancingItemsListItem.getElementsByTagName('ul')[0];
  var bsuosBalancingItemsListItem = appendListItemChildren('bsuos' + type + 'BalancingItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Balancing System Use Of System"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');
  var rcrcBalancingItemsListItem = appendListItemChildren('rcrc' + type + 'BalancingItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Residual Cashflow Reallocation Cashflow"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');

  balancingDisplaySelectorListUl.appendChild(bsuosBalancingItemsListItem);
  balancingDisplaySelectorListUl.appendChild(rcrcBalancingItemsListItem);
}

function createOtherDisplayListItems(otherItemsListItem, type) {
  var otherDisplaySelectorListUl = otherItemsListItem.getElementsByTagName('ul')[0];
  var sundryOtherItemsListItem = appendListItemChildren('sundry' + type + 'OtherItemsDisplaySelector', false, 'updatePage()', [{"Name" : "Sundry"}], type.toLowerCase() + 'DisplaySelector', false, 'checkbox', type.toLowerCase() + 'DisplayGroup');

  otherDisplaySelectorListUl.appendChild(sundryOtherItemsListItem);
}

function createBudgetTree() {
  var budgetListItem = appendListItemChildren('budgetSelector', true, 'updatePage()', [{"Name" : "Budget"}], 'budgetSelector', false, 'checkbox', 'budgetGroup');
  budgetListItem.id = "budgetTree";

  var budgetSelectorListUl = budgetListItem.getElementsByTagName('ul')[0];

  var budget2BudgetListItem = appendListItemChildren('budget2BudgetSelector', true, 'updatePage()', [{"Name" : "Budget 2"}], 'budgetSelector', false, 'checkbox', 'budgetGroup');
  var budget1BudgetListItem = appendListItemChildren('budget1BudgetSelector', false, 'updatePage()', [{"Name" : "Budget 1"}], 'budgetSelector', false, 'checkbox', 'budgetGroup');

  budgetSelectorListUl.appendChild(budget2BudgetListItem);
  budgetSelectorListUl.appendChild(budget1BudgetListItem);

  var budget2BudgetSelectorListUl = budget2BudgetListItem.getElementsByTagName('ul')[0];
  var version2Budget2ListItem = appendListItemChildren('version2Budget2CostBudgetSelector', false, 'updatePage()', [{"Name" : "Version 2"}], 'budget2BudgetSelector', false, 'checkbox', 'budget2BudgetGroup');
  var version1Budget2ListItem = appendListItemChildren('version1Budget2CostBudgetSelector', false, 'updatePage()', [{"Name" : "Version 1"}], 'budget2BudgetSelector', false, 'checkbox', 'budget2BudgetGroup');

  budget2BudgetSelectorListUl.appendChild(version2Budget2ListItem);
  budget2BudgetSelectorListUl.appendChild(version1Budget2ListItem);

  return budgetListItem;
}

async function createSiteTree(isPageLoad) {
  var div = document.getElementById('siteTree');
  var inputs = div.getElementsByTagName('input');
  var checkboxes = !isPageLoad ? [] : getCheckedElements(inputs);

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

  var headerDiv = createHeaderDiv("siteHeader", 'Location', true);
  var ul = createBranchUl("siteSelector", false, true);

  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';
  breakDisplayListItem.classList.add('format-listitem');

  var recurseSelectionListItem = document.createElement('li');
  recurseSelectionListItem.classList.add('format-listitem');
  recurseSelectionListItem.classList.add('listItemWithoutPadding');

  var recurseSelectionCheckbox = createBranchCheckbox('recurseSelectionCheckbox', '', 'recurseSelection', 'checkbox', 'recurseSelection', false);
  var recurseSelectionSpan = createBranchSpan('recurseSelectionSpan', 'Recurse Selection?');
  recurseSelectionListItem.appendChild(recurseSelectionCheckbox);
  recurseSelectionListItem.appendChild(recurseSelectionSpan);

  await buildSiteBranch(ul);
  clearElement(div);
  div.appendChild(headerDiv);
  ul.appendChild(breakDisplayListItem);
  ul.appendChild(recurseSelectionListItem);
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
  var invoiceListItem = appendListItemChildren('invoiceSelector', true, 'updatePage()', [{"Name" : "Invoice"}], 'invoiceSelector', false, 'checkbox', 'invoiceGroup');
  invoiceListItem.id = "invoiceTree";

  var invoiceSelectorListUl = invoiceListItem.getElementsByTagName('ul')[0];
  var invoice3InvoiceListItem = appendListItemChildren('invoice3InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0003"}], 'invoiceSelector', false, 'checkbox', 'selectSpecificInvoiceGroup');
  var invoice2InvoiceListItem = appendListItemChildren('invoice2InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0002"}], 'invoiceSelector', false, 'checkbox', 'selectSpecificInvoiceGroup');
  var invoice1InvoiceListItem = appendListItemChildren('invoice1InvoiceSelector', false, 'updatePage()', [{"Name" : "Invoice 0001"}], 'invoiceSelector', false, 'checkbox', 'selectSpecificInvoiceGroup');

  invoiceSelectorListUl.appendChild(invoice3InvoiceListItem);
  invoiceSelectorListUl.appendChild(invoice2InvoiceListItem);
  invoiceSelectorListUl.appendChild(invoice1InvoiceListItem);

  var breakDisplayListItem = document.createElement('li');
  breakDisplayListItem.innerHTML = '<br>';

  invoiceSelectorListUl.appendChild(breakDisplayListItem);

  return invoiceListItem;
}

function createVarianceTree() {
  return appendListItemChildren('varianceSelector', false, 'updatePage()', [{"Name" : "Show Variances"}], 'varianceSelector', false, 'checkbox', 'varianceGroup');
}

function createForecastTree() {
  var forecastListItem = appendListItemChildren('forecastSelector', false, 'updatePage()', [{"Name" : "BWS Forecast"}], 'forecastSelector', true, 'checkbox', 'forecastGroup');

  return forecastListItem;
}

function createGroupingOptionTree() {
  var div = document.getElementById('groupingOptionTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("groupingOptionHeader", "Grouping Option", true);
  var ul = createBranchUl("groupingOptionSelector", false, true);

  var groupingOption2GroupingOptionListItem = appendListItemChildren('groupingOption2GroupingOptionSelector', false, 'updatePage()', [{"Name" : "No Grouping"}], 'groupingOptionSelector', true, 'radio', 'groupingOptionGroup');
  var groupingOption1GroupingOptionListItem = appendListItemChildren('groupingOption1GroupingOptionSelector', true, 'updatePage()', [{"Name" : "Group"}], 'groupingOptionSelector', false, 'radio', 'groupingOptionGroup');

  ul.appendChild(groupingOption2GroupingOptionListItem);
  ul.appendChild(groupingOption1GroupingOptionListItem);

  var groupingOption1GroupingOptionSelectorListUl = groupingOption1GroupingOptionListItem.getElementsByTagName('ul')[0];
  var sumGroupingOption1ListItem = appendListItemChildren('sumGroupingOption1CostGroupingOptionSelector', false, 'updatePage()', [{"Name" : "Sum"}], 'groupingOption1GroupingOptionSelector', true, 'checkbox', 'groupingOption1GroupingOptionGroup');
  var averageGroupingOption1ListItem = appendListItemChildren('averageGroupingOption1CostGroupingOptionSelector', false, 'updatePage()', [{"Name" : "Average"}], 'groupingOption1GroupingOptionSelector', false, 'checkbox', 'groupingOption1GroupingOptionGroup');

  groupingOption1GroupingOptionSelectorListUl.appendChild(sumGroupingOption1ListItem);
  groupingOption1GroupingOptionSelectorListUl.appendChild(averageGroupingOption1ListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

function createTimePeriodTree() {
  var div = document.getElementById('timePeriodTree');
  clearElement(div);

  var headerDiv = createHeaderDiv("timePeriodHeader", "Time Period", true);
  var ul = createBranchUl("timePeriodSelector", false, true);
  ul.classList.add("slider-list");

  var createdDisplayListItem = document.createElement('li');
  var createdDisplayCheckbox = createBranchCheckbox('createdDisplayCheckbox', '', 'createdDisplay', 'checkbox', 'createdDisplay', true);
  createdDisplayCheckbox.setAttribute('onclick', 'showElement(timePeriodOptionsFilteredCreatedBreak, !this.checked); showElement(timePeriodOptionsFilteredCreated, !this.checked);');

  var createdDisplaySpan = createBranchSpan('createdDisplaySpan', "Latest Created");
  var createdDisplayRZSlider = document.createElement('rzslider');
  createdDisplayRZSlider.id = 'timePeriodOptionsFilteredCreated';
  createdDisplayRZSlider.setAttribute('rz-slider-model', 'timePeriodOptionsFilteredCreated.value');
  createdDisplayRZSlider.setAttribute('rz-slider-options', 'timePeriodOptionsFilteredCreated.options');

  var breakDisplayListItem = createBreakDisplayListItem();
  breakDisplayListItem.id = 'timePeriodOptionsFilteredCreatedBreak';

  createdDisplayListItem.appendChild(createdDisplayCheckbox);
  createdDisplayListItem.appendChild(createdDisplaySpan);
  createdDisplayListItem.appendChild(breakDisplayListItem);
  createdDisplayListItem.appendChild(createdDisplayRZSlider);

  ul.appendChild(createdDisplayListItem);

  div.appendChild(headerDiv);
  div.appendChild(ul);
}

async function getDataFromAPI(data, processQueueGUID) {
	var postSuccessful = postData(data);

	if(postSuccessful) {
		var response = await getProcessResponse(processQueueGUID);
		return await processResponse(response, processQueueGUID);;
	}
}

//build site
async function buildSiteBranch(elementToAppendTo) {
  var processQueueGUID = CreateGUID();
  var commodities = [];
  if(electricityCommoditycheckbox.checked){commodities.push('Electricity')};
  if(gasCommoditycheckbox.checked){commodities.push('Gas')};

  var filterData = {
    SiteChecked: siteLocationcheckbox.checked,
    AreaChecked: areaLocationcheckbox.checked,
    CommodityChecked: commodityLocationcheckbox.checked,
    MeterChecked: meterLocationcheckbox.checked,
    SubAreaChecked: subareaLocationcheckbox.checked,
    AssetChecked: assetLocationcheckbox.checked,
    SubMeterChecked: submeterLocationcheckbox.checked,
    Commodities: commodities
  };
	var data = {ProcessQueueGUID: processQueueGUID, FilterData: filterData, ProcessGUID: '7A8E05B5-34EF-4A14-9076-34F53BD7C5F6'};
  var treeResponse = await getDataFromAPI(data, processQueueGUID);
  elementToAppendTo.innerHTML = treeResponse.message;
}

async function updatePage(callingElement) {
  var branch = callingElement.getAttribute('branch');
  
  switch(branch) {
    case 'reset':
      showElement(miniLoader, true);
      resetSlider();
      resetSwitchButtons();
      await createTrees(false);
      showElement(miniLoader, false);
      break;
    case 'displaySelector':
    case 'usageDisplaySelector':
      updateChartHeader(callingElement);
      break;
    case 'invoiceSelector':
      updatePageFromInvoice(callingElement);
      break;
    case 'commoditySelector':
    case 'locationSelector':
      showElement(miniLoader, true);
      await createTrees(false);
      showElement(miniLoader, false);
      break;
    case 'groupingOptionSelector':
    case 'groupingOption1GroupingOptionSelector':
    case 'budgetSelector':
    case 'varianceSelector':
    case 'forecastSelector':
    case 'costDisplaySelector':
    case 'rateDisplaySelector':
      break;
    case 'Site':
    case 'Area':
    case 'Meter':
    case 'SubArea':
    case 'Asset':
    case 'SubMeter':
    case 'Commodity':
      recurseSelection(callingElement, 'recurseSelectionCheckboxcheckbox');
      break;
    default:
      alert(branch);
      break;
  }
}

function updateChartHeader(callingElement) {
  var span = document.getElementById(callingElement.id.replace('radio', 'span'));
  var chartHeader = document.getElementById("chartHeaderSpan");
  var chartHeaderText = '';

  if(span.innerText == "Usage") {
    chartHeaderText = document.getElementById('consumptionUsageItemsUsageDisplaySelectorradio').checked 
      ? 'Usage' : 'Capacity';
  }
  else {
    chartHeaderText = span.innerText;
  }

  chartHeader.innerText = chartHeaderText;
}

function updatePageFromInvoice(callingElement) {
  if(callingElement.checked) {
    var timePeriodOptionsDisplayTimeSpan = document.getElementById('timePeriodOptionsDisplayTimeSpan');
    var granularity = timePeriodOptionsDisplayTimeSpan.children[6].innerHTML;

    if(granularity == 'Half Hourly' || granularity == 'Daily') {
      makeTimePeriodOptionsTimeSpanMonthly();
    }
  }

  switch(callingElement.id) {
    case 'invoice1InvoiceSelectorcheckbox':
    case 'invoice2InvoiceSelectorcheckbox':
    case 'invoice3InvoiceSelectorcheckbox':
      if(!callingElement.checked) {
        selectAllInvoiceSelectorcheckbox.checked = false;
      }
      else {
        if(invoice1InvoiceSelectorcheckbox.checked
          && invoice2InvoiceSelectorcheckbox.checked
          && invoice3InvoiceSelectorcheckbox.checked) {
            selectAllInvoiceSelectorcheckbox.checked = true;
          }
      }
      break;
    case 'selectAllInvoiceSelectorcheckbox':
      invoice3InvoiceSelectorcheckbox.checked = callingElement.checked;
      invoice2InvoiceSelectorcheckbox.checked = callingElement.checked;
      invoice1InvoiceSelectorcheckbox.checked = callingElement.checked;
      break;
  }
}

function makeTimePeriodOptionsTimeSpanMonthly() {
  var scope = angular.element(document.getElementById("timePeriodOptionsDisplayTimeSpan")).scope();
  scope.$apply(function () {
    scope.makeTimePeriodOptionsTimeSpanMonthly();
  });
}

async function updateChart(isPageLoading) {
  checkTimePeriodOptionsTimeSpan();

  var commodityOption = getCommodityOption();
  var showByArray = determineShowByArray();
  var startDate = getStartDate();
  var endDate = getEndDate();
  var dateFormat = getPeriodDateFormat()
  categories = getCategoryTexts(startDate, endDate, dateFormat);
  var displayType = getDisplayType();

  var treeDiv = document.getElementById('siteTree');
  var inputs = treeDiv.getElementsByTagName('input');
  var checkboxes = getCheckedElements(inputs);
  
  if(isPageLoading) {
    checkboxes = getBranchCheckboxes(inputs, 'Site');
  }

  var meters = [];// getMeters(showByArray, checkboxes, commodityOption);
  chartSeries = await getChartSeries(showByArray, meters, categories, dateFormat, startDate, endDate);
  var chartOptions = getChartOptions(categories, displayType, getXAxisTypeFromTimeSpan(), dateFormat);

  clearElement(chart);
  refreshChart(chartSeries, displayType, chartOptions);
  updateDataGrid(chartSeries, categories);
}

function checkTimePeriodOptionsTimeSpan() {
  var invoices = document.getElementById('invoiceTree');
  var inputs = invoices.getElementsByTagName('input');
  var checkboxes = getCheckedElements(inputs);

  var timePeriodOptionsDisplayTimeSpan = document.getElementById('timePeriodOptionsDisplayTimeSpan');
  var granularity = timePeriodOptionsDisplayTimeSpan.children[6].innerHTML;

  if(granularity == 'Half Hourly' || granularity == 'Daily') {
    for(var i = 0; i < checkboxes.length; i++) {
      checkboxes[i].checked = false;
    }
  }
}

function getMeters(showByArray, checkboxes, commodityOption) {
  var noGroupradio = document.getElementById('groupingOption2GroupingOptionSelectorradio');
  var checkboxLength = checkboxes.length;
  var tempMeters = [];
  var commodities = [];
  var branches = [];

  if(commodityOption == '') {
    commodities.push('Electricity');
    commodities.push('Gas');
  }
  else {
    commodities.push(commodityOption);
  }

  for(var s = 0; s < showByArray.length; s++) {
    var data = getData(showByArray[s]);
  
    for(var i = 0; i < checkboxLength; i++) {
      var hierarchy = checkboxes[i].id.replace('checkbox', '').split('_');
      var lastRecord = hierarchy[hierarchy.length - 1];

      for(var j = 0; j < commodities.length; j++) {
        var meters = [];
        var branch = '';
        var seriesName = '';

        if(lastRecord.includes('Site')) {
          var site = data[parseInt(lastRecord.replace('Site', ''))];
          meters = getMetersBySite(site, commodities[j]);
          branch = 'Site';
          seriesName = getAttribute(site.Attributes, 'Name') + ' - ' + commodities[j];
        }
        else if(lastRecord.includes('SubArea')) {
          var site = data[parseInt(hierarchy[hierarchy.length - 5].replace('Site', ''))];
          var area = site.Areas[parseInt(hierarchy[hierarchy.length - 4].replace('Area', ''))];
          var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 3].replace('Commodity', ''))];
          var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 2].replace('Meter', ''))];
          var subArea = meter.SubAreas[parseInt(lastRecord.replace('SubArea', ''))];
          meters = getSubMetersBySubArea(subArea, commodities[j]);
          branch = 'SubArea';
          seriesName = getAttribute(site.Attributes, 'Name') + ' - ' + getAttribute(meter.Attributes, 'Name') + ' - ' + getAttribute(subArea.Attributes, 'Name');
        }
        else if(lastRecord.includes('Area')) {
          var site = data[parseInt(hierarchy[hierarchy.length - 2].replace('Site', ''))];
          var area = site.Areas[parseInt(lastRecord.replace('Area', ''))];
          meters = getMetersByArea(area, commodities[j]);
          branch = 'Area';
          seriesName = getAttribute(site.Attributes, 'Name') + ' - ' + getAttribute(area.Attributes, 'Name') + ' - ' + commodities[j];
        }
        else if(lastRecord.includes('SubMeter')) {
          var site = data[parseInt(hierarchy[hierarchy.length - 7].replace('Site', ''))];
          var area = site.Areas[parseInt(hierarchy[hierarchy.length - 6].replace('Area', ''))];
          var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 5].replace('Commodity', ''))];
          var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 4].replace('Meter', ''))];
          var subArea = meter.SubAreas[parseInt(hierarchy[hierarchy.length - 3].replace('SubArea', ''))];
          var asset = subArea.Assets[parseInt(hierarchy[hierarchy.length - 2].replace('Asset', ''))];
          var subMeter = asset.SubMeters[parseInt(lastRecord.replace('SubMeter', ''))];
          branch = 'SubMeter';
          seriesName = getAttribute(site.Attributes, 'Name') 
            + ' - ' + getAttribute(meter.Attributes, 'Name')
            + ' - ' + getAttribute(subArea.Attributes, 'Name')
            + ' - ' + getAttribute(asset.Attributes, 'Name')
            + ' - ' + getAttribute(subMeter.Attributes, 'Name');

          if(getAttribute(subMeter.Attributes, 'Commodities').includes(commodities[j])) {
            meters.push(subMeter);
          }
        }
        else if(lastRecord.includes('Meter')) {
          var site = data[parseInt(hierarchy[hierarchy.length - 4].replace('Site', ''))];
          var area = site.Areas[parseInt(hierarchy[hierarchy.length - 3].replace('Area', ''))];
          var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 2].replace('Commodity', ''))];
          var meter = commodity.Meters[parseInt(lastRecord.replace('Meter', ''))];
          branch = 'Meter';
          seriesName = getAttribute(site.Attributes, 'Name') + ' - ' + getAttribute(meter.Attributes, 'Name');

          if(getAttribute(meter.Attributes, 'Commodities').includes(commodities[j])) {
            meters.push(meter);
          }
        }
        else if(lastRecord.includes('Asset')) {
          var site = data[parseInt(hierarchy[hierarchy.length - 6].replace('Site', ''))];
          var area = site.Areas[parseInt(hierarchy[hierarchy.length - 5].replace('Area', ''))];
          var commodity = area.Commodities[parseInt(hierarchy[hierarchy.length - 4].replace('Commodity', ''))];
          var meter = commodity.Meters[parseInt(hierarchy[hierarchy.length - 3].replace('Meter', ''))];
          var subArea = meter.SubAreas[parseInt(hierarchy[hierarchy.length - 2].replace('SubArea', ''))];
          var asset = subArea.Assets[parseInt(lastRecord.replace('Asset', ''))];
          meters = getSubMetersByAsset(asset, commodities[j]);
          branch = 'Asset';
          seriesName = getAttribute(site.Attributes, 'Name') 
            + ' - ' + getAttribute(meter.Attributes, 'Name')
            + ' - ' + getAttribute(subArea.Attributes, 'Name')
            + ' - ' + getAttribute(asset.Attributes, 'Name');
        }

        var tempMeterMeters = [];

        for(var m = 0; m < meters.length; m++) {
          var meterData = meters[m][showByArray[s]];

          if(showByArray[s] == "MaxDemand") {
            meterData = meters[m]["Capacity"];
          }
  
          if(meterData) {
            tempMeterMeters.push(meters[m]);
          }
        }
    
        if(tempMeterMeters.length > 0) {
          var tempMeter = {
            SeriesName: seriesName,
            Commodity: commodities[j],
            Branch: branch,
            Meters: tempMeterMeters,
            ShowBy: showByArray[s]
          }
      
          tempMeters.push(tempMeter);

          if(!branches.includes(branch)) {
            branches.push(branch);
          }
        }
      }    
    }
  }

  if(noGroupradio.checked) {
    return tempMeters;
  }

  var series = [];
  for(var i = 0; i < branches.length; i++) {
    for(var j = 0; j < commodities.length; j++) {
      for(var s = 0; s < showByArray.length; s++) {
        var tempMeter = {
          SeriesName: branches[i] + ' - ' + commodities[j],
          Meters: [],
          ShowBy: showByArray[s]
        }
    
        for(var k = 0; k < tempMeters.length; k++) {
          if(tempMeters[k].Commodity == commodities[j] 
            && tempMeters[k].Branch == branches[i]
            && tempMeters[k].ShowBy == showByArray[s]) {
            tempMeter.Meters.push(...tempMeters[k].Meters);
          }      
        }
    
        if(tempMeter.Meters.length > 0) {
          series.push(tempMeter);
        }
      }
    }
  }  

  return series;
}

function getData(showBy) {
  switch(showBy) {
    case "Cost":
      return costSites;
    case "Capacity":
    case "MaxDemand":
      return capacitySites;
    case 'Cost - Wholesale':
      return wholesaleCostSites;
    case 'Cost - Distribution':
      return distributionCostSites;
    case 'Cost - Transmission':
      return transmissionCostSites;
    case 'Cost - Renewables Obligation':
      return renewablesobligationCostSites;
    case 'Cost - Feed In Tariff':
      return feedintariffCostSites;
    case 'Cost - Contracts For Difference':
      return contractsfordifferenceCostSites;
    case 'Cost - Energy Intensive Industry':
      return energyintensiveindustryCostSites;
    case 'Cost - Capacity Markets':
      return capacitymarketCostSites;
    case 'Cost - Balancing System Use Of System':
      return balancingsystemuseofsystemCostSites;
    case 'Cost - Residual Cashflow Reallocation Cashflow':
      return residualcashflowreallocationcashflowCostSites;
    case 'Cost - Sundry':
      return sundryCostSites;
    case 'Usage - Budget 1':
      return budget1UsageSites
    case 'Cost - Budget 1':
      return budget1CostSites
    case 'Usage - Budget 2 V1':
      return budget2v1UsageSites
    case 'Cost - Budget 2 V1':
      return budget2v1CostSites
    case 'Usage - Budget 2 V2':
      return budget2v2UsageSites
    case 'Cost - Budget 2 V2':
      return budget2v2CostSites
    case 'Usage - Invoice 0001':
      return invoice0001UsageSites;
    case 'Cost - Invoice 0001':
      return invoice0001CostSites;
    case 'Usage - Invoice 0002':
      return invoice0002UsageSites;
    case 'Cost - Invoice 0002':
      return invoice0002CostSites;
    case 'Usage - Invoice 0003':
      return invoice0003UsageSites;
    case 'Cost - Invoice 0003':
      return invoice0003CostSites;
    case 'Usage - Invoice 0001 - Budget 1 Variance':
      return invoice0001budget1varianceUsageSites;
    case 'Usage - Invoice 0001 - Forecast Variance':
      return invoice0001forecastvarianceUsageSites;
    case 'Cost - Invoice 0001 - Budget 1 Variance':
      return invoice0001budget1varianceCostSites;
    case 'Cost - Invoice 0001 - Forecast Variance':
      return invoice0001forecastvarianceCostSites;
    case 'Usage - Invoice 0002 - Budget 1 Variance':
      return invoice0002budget1varianceUsageSites;
    case 'Usage - Invoice 0002 - Forecast Variance':
      return invoice0002forecastvarianceUsageSites;
    case 'Cost - Invoice 0002 - Budget 1 Variance':
      return invoice0002budget1varianceCostSites;
    case 'Cost - Invoice 0002 - Forecast Variance':
      return invoice0002forecastvarianceCostSites;
    case 'Usage - Invoice 0003 - Budget 1 Variance':
      return invoice0003budget1varianceUsageSites;
    case 'Usage - Invoice 0003 - Forecast Variance':
      return invoice0003forecastvarianceUsageSites;
    case 'Cost - Invoice 0003 - Budget 1 Variance':
      return invoice0003budget1varianceCostSites;
    case 'Cost - Invoice 0003 - Forecast Variance':
      return invoice0003forecastvarianceCostSites;
    default:
      return usageSites;
  }
}

function getMetersBySite(site, commodityOption) {
  var meters = [];

  var areaLength = site.Areas.length;
  for(var areaCount = 0; areaCount < areaLength; areaCount++) {
    var area = site.Areas[areaCount];

    if(getAttribute(area.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getMetersByArea(area, commodityOption));
    }
  }

  return [...meters];
}

function getMetersByArea(area, commodityOption) {
  var meters = [];

  var commodityLength = area.Commodities.length;
  for(var commodityCount = 0; commodityCount < commodityLength; commodityCount++) {
    var commodity = area.Commodities[commodityCount];

    if(getAttribute(commodity.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getMetersByCommodity(commodity, commodityOption));
    }
  }

  return [...meters];
}

function getMetersByCommodity(commodity, commodityOption) {
  var meters = [];

  var meterLength = commodity.Meters.length;
  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = commodity.Meters[meterCount];

    if(getAttribute(meter.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(meter);
    }
  }

  return [...meters];
}

function getSubMetersBySubArea(subArea, commodityOption) {
  var meters = [];

  var assetLength = subArea.Assets.length;
  for(var assetCount = 0; assetCount < assetLength; assetCount++) {
    var asset = subArea.Assets[assetCount];

    if(getAttribute(asset.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(...getSubMetersByAsset(asset, commodityOption));
    }
  }

  return [...meters];
}

function getSubMetersByAsset(asset, commodityOption) {
  var meters = [];

  var subMeterLength = asset.SubMeters.length;
  for(var subMeterCount = 0; subMeterCount < subMeterLength; subMeterCount++) {
    var subMeter = asset.SubMeters[subMeterCount];

    if(getAttribute(subMeter.Attributes, 'Commodities').includes(commodityOption)) {
      meters.push(subMeter);
    }
  }

  return [...meters];
}

function determineShowByArray() {
  var div = document.getElementById('displayListItemTitlespan');
  var inputs = div.getElementsByTagName('input');
  var checkedElements = getCheckedElements(inputs);
  var baseDisplayRadio = null;

  for(var i = 0; i < checkedElements.length; i++) {
    if(checkedElements[i].getAttribute('branch') == 'displaySelector') {
      baseDisplayRadio = checkedElements[i];
      break;
    }
  }

  var showByArray = [];
  if(forecastSelectorcheckbox.checked) {
    for(var i = 0; i < checkedElements.length; i++) {
      if(baseDisplayRadio.id == 'usageDisplaySelectorradio') {
        if(checkedElements[i].getAttribute('branch') == 'usageDisplaySelector') {
          if(checkedElements[i].id == 'consumptionUsageItemsUsageDisplaySelectorradio') {
            showByArray = ['Usage']
          }
          else {
            showByArray = ['Capacity', 'MaxDemand']
          }
        }
      }
      else {
        if(checkedElements[i].getAttribute('branch').concat('radio') == baseDisplayRadio.id) {
          var branch = '';
  
          if(baseDisplayRadio.id == 'rateDisplaySelectorradio') {
            branch = 'Rate';
            pushToArray('Usage', showByArray);
          }
          else {
            branch = 'Cost';
          }
  
          switch(checkedElements[i].id.replace(branch, '')) {
            case 'allItemsDisplaySelectorcheckbox':
              pushToArray('Cost', showByArray);
              break;
            case 'networkItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Wholesale', showByArray);
              pushToArray('Cost - Distribution', showByArray);
              pushToArray('Cost - Transmission', showByArray);
              break;
            case 'wholesaleNetworkItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Wholesale', showByArray);
              break;
            case 'distributionNetworkItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Distribution', showByArray);
              break;
            case 'transmissionNetworkItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Transmission', showByArray);
              break;
            case 'renewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Renewables Obligation', showByArray);
              pushToArray('Cost - Feed In Tariff', showByArray);
              pushToArray('Cost - Contracts For Difference', showByArray);
              pushToArray('Cost - Energy Intensive Industry', showByArray);
              pushToArray('Cost - Capacity Markets', showByArray);
              break;
            case 'renewablesObligationRenewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Renewables Obligation', showByArray);
              break;
            case 'feedInTariffRenewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Feed In Tariff', showByArray);
              break;
            case 'contractsForDifferenceRenewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Contracts For Difference', showByArray);
              break;
            case 'energyIntensiveIndustryRenewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Energy Intensive Industry', showByArray);
              break;
            case 'capacityMarketRenewablesItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Capacity Markets', showByArray);
              break;
            case 'balancingItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Balancing System Use Of System', showByArray);
              pushToArray('Cost - Residual Cashflow Reallocation Cashflow', showByArray);
              break;
            case 'bsuosBalancingItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Balancing System Use Of System', showByArray);
              break;
            case 'rcrcBalancingItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Residual Cashflow Reallocation Cashflow', showByArray);
              break;
            case 'otherItemsDisplaySelectorcheckbox':
            case 'sundryOtherItemsDisplaySelectorcheckbox':
              pushToArray('Cost - Sundry', showByArray);
              break;
          }
        }
      }
    }
  }

  div = document.getElementById('budgetTree');
  var inputs = div.getElementsByTagName('input');
  var checkedElements = getCheckedElements(inputs);

  var budgetSelected = checkedElements.length > 0;
  for(var i = 0; i < checkedElements.length; i++) {
    var budget = document.getElementById(checkedElements[i].id.replace('checkbox', 'span')).innerText;

    switch(baseDisplayRadio.id) {
      case 'usageDisplaySelectorradio':
        if(budget == 'Budget 2') {
          pushToArray('Usage - Budget 2 V1', showByArray);
          pushToArray('Usage - Budget 2 V2', showByArray);
        }
        else if(budget == 'Budget') {
          pushToArray('Usage - Budget 2 V1', showByArray);
          pushToArray('Usage - Budget 2 V2', showByArray);
          pushToArray('Usage - Budget 1', showByArray);
        }
        else {
          pushToArray('Usage - ' + budget, showByArray);
        }
        break;
      case 'costDisplaySelectorradio':
        if(budget == 'Budget 2') {
          pushToArray('Cost - Budget 2 V1', showByArray);
          pushToArray('Cost - Budget 2 V2', showByArray);
        }
        else if(budget == 'Budget') {
          pushToArray('Cost - Budget 2 V1', showByArray);
          pushToArray('Cost - Budget 2 V2', showByArray);
          pushToArray('Cost - Budget 1', showByArray);
        }
        else {
          pushToArray('Cost - ' + budget, showByArray);
        }
        break;
      case 'rateDisplaySelectorradio':
        if(budget == 'Budget 2') {
          pushToArray('Usage - Budget 2 V1', showByArray);
          pushToArray('Usage - Budget 2 V2', showByArray);
          pushToArray('Cost - Budget 2 V1', showByArray);
          pushToArray('Cost - Budget 2 V2', showByArray);
        }
        else if(budget == 'Budget') {
          pushToArray('Usage - Budget 2 V1', showByArray);
          pushToArray('Usage - Budget 2 V2', showByArray);
          pushToArray('Usage - Budget 1', showByArray);
          pushToArray('Cost - Budget 2 V1', showByArray);
          pushToArray('Cost - Budget 2 V2', showByArray);
          pushToArray('Cost - Budget 1', showByArray);
        }
        else {
          pushToArray('Usage - ' + budget, showByArray);
          pushToArray('Cost - ' + budget, showByArray);
        }
        break;
    }
  }

  div = document.getElementById('invoiceTree');
  var inputs = div.getElementsByTagName('input');
  var checkedElements = getCheckedElements(inputs);

  for(var i = 0; i < checkedElements.length; i++) {
    if(checkedElements[i].type == 'checkbox') {
      var invoice = document.getElementById(checkedElements[i].id.replace('checkbox', 'span')).innerText;

      if(varianceSelectorcheckbox.checked && forecastSelectorcheckbox.checked) {
        for(var j = 0; j < checkedElements.length; j++) {
          invoice = document.getElementById(checkedElements[j].id.replace('checkbox', 'span')).innerText;

          if(invoice.startsWith('Invoice ')) {
            switch(baseDisplayRadio.id) {
              case 'usageDisplaySelectorradio':
                pushToArray('Usage - ' + invoice + ' - Budget 1 Variance', showByArray);
                break;
              case 'costDisplaySelectorradio':
                pushToArray('Cost - ' + invoice + ' - Budget 1 Variance', showByArray);
                break;
              case 'rateDisplaySelectorradio':
                pushToArray('Usage - ' + invoice + ' - Budget 1 Variance', showByArray);
                pushToArray('Cost - ' + invoice + ' - Budget 1 Variance', showByArray);
                break;
            }
          }
          else {
            switch(baseDisplayRadio.id) {
              case 'usageDisplaySelectorradio':
                pushToArray('Usage - Invoice 0001 - Budget 1 Variance', showByArray);
                pushToArray('Usage - Invoice 0002 - Budget 1 Variance', showByArray);
                pushToArray('Usage - Invoice 0003 - Budget 1 Variance', showByArray);
                break;
              case 'costDisplaySelectorradio':
                pushToArray('Cost - Invoice 0001 - Budget 1 Variance', showByArray);
                pushToArray('Cost - Invoice 0002 - Budget 1 Variance', showByArray);
                pushToArray('Cost - Invoice 0003 - Budget 1 Variance', showByArray);
                break;
              case 'rateDisplaySelectorradio':
                pushToArray('Usage - Invoice 0001 - Budget 1 Variance', showByArray);
                pushToArray('Usage - Invoice 0002 - Budget 1 Variance', showByArray);
                pushToArray('Usage - Invoice 0003 - Budget 1 Variance', showByArray);
                pushToArray('Cost - Invoice 0001 - Budget 1 Variance', showByArray);
                pushToArray('Cost - Invoice 0002 - Budget 1 Variance', showByArray);
                pushToArray('Cost - Invoice 0003 - Budget 1 Variance', showByArray);
                break;
            }
          }
        }
      }
      
      if(varianceSelectorcheckbox.checked && budgetSelected) {
        for(var j = 0; j < checkedElements.length; j++) {
          invoice = document.getElementById(checkedElements[j].id.replace('checkbox', 'span')).innerText;

          if(invoice.startsWith('Invoice ')) {
            switch(baseDisplayRadio.id) {
              case 'usageDisplaySelectorradio':
                pushToArray('Usage - ' + invoice + ' - Forecast Variance', showByArray);
                break;
              case 'costDisplaySelectorradio':
                pushToArray('Cost - ' + invoice + ' - Forecast Variance', showByArray);
                break;
              case 'rateDisplaySelectorradio':
                pushToArray('Usage - ' + invoice + ' - Forecast Variance', showByArray);
                pushToArray('Cost - ' + invoice + ' - Forecast Variance', showByArray);
                break;
            }
          }
          else {
            switch(baseDisplayRadio.id) {
              case 'usageDisplaySelectorradio':
                pushToArray('Usage - Invoice 0001 - Forecast Variance', showByArray);
                pushToArray('Usage - Invoice 0002 - Forecast Variance', showByArray);
                pushToArray('Usage - Invoice 0003 - Forecast Variance', showByArray);
                break;
              case 'costDisplaySelectorradio':
                pushToArray('Cost - Invoice 0001 - Forecast Variance', showByArray);
                pushToArray('Cost - Invoice 0002 - Forecast Variance', showByArray);
                pushToArray('Cost - Invoice 0003 - Forecast Variance', showByArray);
                break;
              case 'rateDisplaySelectorradio':
                pushToArray('Usage - Invoice 0001 - Forecast Variance', showByArray);
                pushToArray('Usage - Invoice 0002 - Forecast Variance', showByArray);
                pushToArray('Usage - Invoice 0003 - Forecast Variance', showByArray);
                pushToArray('Cost - Invoice 0001 - Forecast Variance', showByArray);
                pushToArray('Cost - Invoice 0002 - Forecast Variance', showByArray);
                pushToArray('Cost - Invoice 0003 - Forecast Variance', showByArray);
                break;
            }
          }
        }
      }

      if(invoice == 'Invoice') {
        switch(baseDisplayRadio.id) {
          case 'usageDisplaySelectorradio':
            pushToArray('Usage - Invoice 0001', showByArray);
            pushToArray('Usage - Invoice 0002', showByArray);
            pushToArray('Usage - Invoice 0003', showByArray);
            break;
          case 'costDisplaySelectorradio':
            pushToArray('Cost - Invoice 0001', showByArray);
            pushToArray('Cost - Invoice 0002', showByArray);
            pushToArray('Cost - Invoice 0003', showByArray);
            break;
          case 'rateDisplaySelectorradio':
            pushToArray('Usage - Invoice 0001', showByArray);
            pushToArray('Usage - Invoice 0002', showByArray);
            pushToArray('Usage - Invoice 0003', showByArray);
            pushToArray('Cost - Invoice 0001', showByArray);
            pushToArray('Cost - Invoice 0002', showByArray);
            pushToArray('Cost - Invoice 0003', showByArray);
            break;
        }
      }
      else {
        switch(baseDisplayRadio.id) {
          case 'usageDisplaySelectorradio':
            pushToArray('Usage - ' + invoice, showByArray);
            break;
          case 'costDisplaySelectorradio':
            pushToArray('Cost - ' + invoice, showByArray);
            break;
          case 'rateDisplaySelectorradio':
            pushToArray('Usage - ' + invoice, showByArray);
            pushToArray('Cost - ' + invoice, showByArray);
            break;
        }
      }
    }
  }
  
  return showByArray;
}

function pushToArray(value, array) {
  if(!array.includes(value)) {
    array.push(value);
  }
}

function getStartDate() {
  var timePeriodOptionsDisplayDateRange = document.getElementById('timePeriodOptionsDisplayDateRange');
  var startDateMilliseconds = parseInt(timePeriodOptionsDisplayDateRange.getElementsByClassName('rz-pointer-min')[0].getAttribute('aria-valuenow'));
  return new Date(startDateMilliseconds);
}

function getEndDate() {
  var timePeriodOptionsDisplayDateRange = document.getElementById('timePeriodOptionsDisplayDateRange');
  var endDateMilliseconds = parseInt(timePeriodOptionsDisplayDateRange.getElementsByClassName('rz-pointer-max')[0].getAttribute('aria-valuenow'));
  return new Date(endDateMilliseconds + (24*60*60*1000));
}

function getStartDateText() {
  var timePeriodOptionsDisplayDateRange = document.getElementById('timePeriodOptionsDisplayDateRange');
  return timePeriodOptionsDisplayDateRange.getElementsByClassName('rz-pointer-min')[0].getAttribute('aria-valuetext');
}

function getEndDateText() {
  var timePeriodOptionsDisplayDateRange = document.getElementById('timePeriodOptionsDisplayDateRange');
  return timePeriodOptionsDisplayDateRange.getElementsByClassName('rz-pointer-max')[0].getAttribute('aria-valuetext');
}

function getPeriodDateFormat() {
  var timePeriodOptionsDisplayTimeSpan = document.getElementById('timePeriodOptionsDisplayTimeSpan');
  switch(timePeriodOptionsDisplayTimeSpan.children[6].innerHTML) {
    case 'Half Hourly':
      return 'dd MMM yy hh:mm:ss';
    case 'Daily':
      return 'dd MMM yy';
    case "Monthly":
      return 'MMM yyyy';
    case "Quarterly":
      return 'yyyy QQ';
    case "Yearly":
      return 'yyyy';
  }
}

function getCategoryTexts(startDate, endDate, dateFormat) {
  var categories = [];

  // for(var newDate = new Date(startDate); newDate < new Date(endDate); newDate.setDate(newDate.getDate() + 1)) {
  //   for(var hh = 0; hh < 48; hh++) {
  //     var newCategoryText = formatDate(new Date(newDate.getTime() + hh*30*60000), dateFormat).toString();

  //     if(!categories.includes(newCategoryText)) {
  //       categories.push(newCategoryText);
  //     }      
  //   }
  // }

  return categories;
}

function formatDate(dateToBeFormatted, format) {
	var baseDate = new Date(dateToBeFormatted);

	switch(format) {
		case 'dd MMM yy':
			var aaaa = baseDate.getFullYear();
			var gg = baseDate.getDate();
			var mm = (baseDate.getMonth() + 1);
		
			if (gg < 10) {
				gg = '0' + gg;
			}				
		
			if (mm < 10) {
				mm = '0' + mm;
			}
		
			return gg + ' ' + convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'dd MMM yy hh:mm:ss':
			var hours = baseDate.getHours()
			var minutes = baseDate.getMinutes()
			var seconds = baseDate.getSeconds();
		
			if (hours < 10) {
				hours = '0' + hours;
			}				
		
			if (minutes < 10) {
				minutes = '0' + minutes;
			}				
		
			if (seconds < 10) {
				seconds = '0' + seconds;
			}			
		
			return formatDate(baseDate, 'dd MMM yy') + ' ' + hours + ':' + minutes + ':' + seconds;
		case 'MMM yyyy':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

			return convertMonthIdToShortCode(mm) + ' ' + aaaa;
		case 'yyyy':
			return baseDate.getFullYear();
		case 'yyyy QQ':
			var aaaa = baseDate.getFullYear();
			var mm = (baseDate.getMonth() + 1);

      return aaaa + ' ' + convertMonthIdToQuarter(mm);
	}
}

function convertMonthIdToFullText(monthId) {
	switch(monthId) {
		case 1:
			return 'January';
		case 2:
			return 'February';
		case 3:
			return 'March';
		case 4:
			return 'April';
		case 5:
			return 'May';
		case 6:
			return 'June';
		case 7:
			return 'July';
		case 8:
			return 'August';
		case 9:
			return 'September';
		case 10:
			return 'October';
		case 11:
			return 'November';
		case 12:
			return 'December';
	}
}

function convertMonthIdToShortCode(monthId) {
	return convertMonthIdToFullText(parseInt(monthId)).slice(0, 3).toUpperCase();
}

function convertMonthIdToQuarter(monthId) {
  switch(monthId) {
		case 1:
		case 2:
		case 3:
			return 'Q1';
		case 4:
		case 5:
		case 6:
      return 'Q2';
		case 7:
		case 8:
		case 9:
      return 'Q3';
		case 10:
		case 11:
		case 12:
			return 'Q4';
	}
}

function getCheckedLocations()
{
  var div = document.getElementById('siteTree');
  var inputs = div.getElementsByTagName('input');
  var checkedElements = getCheckedElements(inputs);
  var checkedGUIDS = [];

  for(var i = 0; i < checkedElements.length; i++)
  {
    var checkedElement = checkedElements[i];
    if(checkedElement.hasAttribute("guid"))
    {
      checkedGUIDS.push(checkedElement.getAttribute("guid"));
    }
  }

  return checkedGUIDS;
}

async function getChartSeries(showByArray, meters, categories, dateFormat, startDate, endDate) {
  var processQueueGUID = CreateGUID();
  var grouping = [];
  if(groupingOption2GroupingOptionSelectorradio.checked){grouping.push('No Grouping')}
  if(groupingOption1GroupingOptionSelectorradio.checked && sumGroupingOption1CostGroupingOptionSelectorcheckbox.checked){grouping.push('Sum')}
  if(groupingOption1GroupingOptionSelectorradio.checked && averageGroupingOption1CostGroupingOptionSelectorcheckbox.checked){grouping.push('Average')}

  var commodities = [];
  if(electricityCommoditycheckbox.checked){commodities.push('Electricity')};
  if(gasCommoditycheckbox.checked){commodities.push('Gas')};

  var timePeriodOptionsDisplayTimeSpan = document.getElementById('timePeriodOptionsDisplayTimeSpan');
  var granularity = timePeriodOptionsDisplayTimeSpan.children[6].innerHTML;

  var filterData = {
    EnergyUnit: 'Usage',
    EnergyUnitInstance: 'BWS Forecast',
    Locations: getCheckedLocations(),
    StartDate: getStartDateText(),
    EndDate: getEndDateText(),
    Granularity: granularity,
    LatestCreated: true, //createdDisplayCheckboxcheckbox.checked,
    CreatedDate: '01-JAN-2020',
    Grouping: grouping,
    Commodities: commodities,
    CustomerGUID: '33669AEC-6C36-4DE9-98F0-A604F3877BB8'
  };
  var data = {ProcessQueueGUID: processQueueGUID, FilterData: filterData, ProcessGUID: '7626BEDC-AB23-4F6E-B87B-2D4976DA1608'};
  
  var dailyForecast = await getDataFromAPI(data, processQueueGUID);
  var tempMeters = JSON.parse(dailyForecast.message);

  for(var i = 0; i < tempMeters.Meters.length; i++) {
    var tempMeter = {
      SeriesName: tempMeters.Meters[i].SeriesName,
      Meters: []
    };

    tempMeter.Meters.push(tempMeters.Meters[i]);

    meters.push(tempMeter);
  }

  var newSeries = [];
  var showByLength = showByArray.length;
  var costDataUsed = false;
  var usageDataUsed = false;

  for(var i = 0; i < showByLength; i++) {
    if(showByArray[i].includes('Cost')) {
      costDataUsed = true;
    }

    if(showByArray[i].includes('Usage')) {
      usageDataUsed = true;
    }

    for(var j = 0; j < meters.length; j++) {
      var usageData = [];
      var usageLength = meters[j].Meters[0].Usage.length;

      for(var k = 0; k < usageLength; k++) {
        var usage = meters[j].Meters[0].Usage[k];

        if(!categories.includes(usage["Date"])) {
          categories.push(usage["Date"]);
        }  
        usageData.push(usage["Value"]);
      }

      var series = {
        name: meters[j].SeriesName,
        data: usageData,
        type: 'line'
      };

      // if(meters[j].ShowBy == showByArray[i]) {
        //var series = getNewChartSeries(meters[j].Meters, showByArray[i], categories, dateFormat, startDate, endDate, meters[j].SeriesName, showByLength > 1);

        if(series.length) {
          newSeries.push(...series);
        }
        else {
          newSeries.push(series);
        }
      // }
    }    
  }

  // if(costDataUsed && usageDataUsed) {
  //   var rateSeries = [];

  //   for(var i = 0; i < newSeries.length; i++) {
  //     if(newSeries[i].name.includes('Cost')) {
  //       for(var j = 0; j < newSeries.length; j++) {
  //         if(newSeries[j].name.includes('Usage')
  //         && compareUsageNameToCostName(newSeries[j].name, newSeries[i].name)) {
  //           for(var k = 0; k < newSeries[j].data.length; k++) {
  //             if(newSeries[i].data[k]) {
  //               newSeries[i].data[k] = JSON.parse(JSON.stringify(preciseRound(newSeries[i].data[k] * 100 / newSeries[j].data[k], 3)));
  //             }
  //             else {
  //               newSeries[i].data[k] = null;
  //             }
  //           }

  //           var series = {
  //             name: newSeries[i].name.replace(' - Cost', ''),
  //             data: newSeries[i].data
  //           }

  //           rateSeries.push(series);
  //           break;
  //         }
  //       }
  //     }
  //   }

  //   return rateSeries;
  // }

  return newSeries;
}

function compareUsageNameToCostName(usageName, costName) {
  if(costName.includes('Budget') && costName.includes('Invoice')) {
    if(!usageName.includes('Budget') && !usageName.includes('Invoice')) {
      return false;
    }

    var budgetUsageName = usageName.replace(' - Usage', '');
    var budgetCostName = costName.replace(' - Cost', '');

    return budgetCostName.includes(budgetUsageName);
  }
  else if(costName.includes('Budget')) {
    if(!usageName.includes('Budget')) {
      return false;
    }

    var budgetUsageName = usageName.replace(' - Usage', '');
    var budgetCostName = costName.replace(' - Cost', '');

    return budgetCostName.includes(budgetUsageName);
  }
  else if(costName.includes('Invoice')) {
    if(!usageName.includes('Invoice')) {
      return false;
    }

    var invoiceUsageName = usageName.replace(' - Usage', '');
    var invoiceCostName = costName.replace(' - Cost', '');

    return invoiceCostName.includes(invoiceUsageName);
  }
  else {
    var index = usageName.indexOf(' - Usage');
    usageName = usageName.substr(0, index);
  
    return costName.includes(usageName);
  }  
}

function getNewChartSeries(meters, showBy, categories, dateFormat, startDate, endDate, seriesName, appendShowByToSeriesName) {
  if(showBy == "MaxDemand") {
    var meterLength = meters.length;

    for(var meterCount = 0; meterCount < meterLength; meterCount++) {
      var meter = meters[meterCount];
      var meterData = meter['Capacity'];
      
      if(!meterData) {
        continue;
      }

      var maxDemand = getAttribute(meter.Attributes, 'MaxDemand');      
      meter['MaxDemand'] = JSON.parse(JSON.stringify(meterData));

      var meterDataLength = meterData.length;
      for(var j = 0; j < meterDataLength; j++) {
        meter['MaxDemand'][j].Value = maxDemand;
      }
    }
  }

  var summedMeterSeries = getSummedMeterSeries(meters, showBy, categories, dateFormat, startDate, endDate);
  return finaliseData(summedMeterSeries, seriesName + (appendShowByToSeriesName ? ' - ' + showBy : ''));
}

function getSummedMeterSeries(meters, showBy, categories, dateFormat, startDate, endDate) {
  var meterLength = meters.length;
  var summedMeterSeries = {
    value: [0],
    count: [0]
  }
  
  for(var meterCount = 0; meterCount < meterLength; meterCount++) {
    var meter = meters[meterCount];
    var meterData = meter[showBy];
    
    if(!meterData) {
      continue;
    }

    var meterDatesApplied = [];
    var meterDataLength = meterData.length;
    for(var j = 0; j < meterDataLength; j++) {
      var meterDate = new Date(meterData[j].Date);

      if(meterDate >= startDate && meterDate <= endDate) {
        var formattedDate = formatDate(meterDate, dateFormat);
        var i = categories.findIndex(n => n == formattedDate);
        var value = meterData[j].Value;

        if(!value && !summedMeterSeries.value[i]){
          summedMeterSeries.value[i] = null;
        }
        else if(value && !summedMeterSeries.value[i]) {
          summedMeterSeries.value[i] = value;
        }
        else if(value && summedMeterSeries.value[i]) {
          summedMeterSeries.value[i] += value;
        }  
        
        if(!meterDatesApplied.includes(formattedDate)) {
          meterDatesApplied.push(formattedDate);

          if(summedMeterSeries.count[i]) {
            summedMeterSeries.count[i] += 1;
          }
          else {
            summedMeterSeries.count[i] = 1;
          }
        }
      }          					     
    }
  } 

  var cleanValue = [];
  var cleanCount = [];

  for(var i = 0; i < summedMeterSeries.count.length; i++) {
    if(summedMeterSeries.count[i] > 0) {
      cleanValue.push(summedMeterSeries.value[i]);
      cleanCount.push(summedMeterSeries.count[i]);
    }
    else {
      cleanValue.push(null);
      cleanCount.push(null);
    }
  }

  return {
    value: cleanValue, 
    count: cleanCount
  };
}

function finaliseData(summedMeterSeries, seriesName) {
  // var finalSeries = [];
  // var noGroupradio = document.getElementById('groupingOption2GroupingOptionSelectorradio');

  // if(noGroupradio.checked) {
    var series = {
      name: seriesName,
      data: summedMeterSeries.value,
      type: seriesName.includes('Invoice') ? 'bar' : 'line'
    };

    return series;
  // }
  // else {
  //   var sumcheckbox = document.getElementById('sumGroupingOption1CostGroupingOptionSelectorcheckbox');
  //   if(sumcheckbox.checked) {
  //     var series = {
  //       name: seriesName + ' - Sum',
  //       data: summedMeterSeries.value,
  //       type: seriesName.includes('Invoice') ? 'bar' : 'line'
  //     };
  
  //     finalSeries.push(series);
  //   }

  //   var averagecheckbox = document.getElementById('averageGroupingOption1CostGroupingOptionSelectorcheckbox');
  //   if(averagecheckbox.checked) {
  //     var series = {
  //       name: seriesName + ' - Average',
  //       data: [],
  //       type: seriesName.includes('Invoice') ? 'bar' : 'line'
  //     };
  
  //     for(var i = 0; i < summedMeterSeries.value.length; i++) {
  //       series.data.push(summedMeterSeries.value[i]/summedMeterSeries.count[i]);
  //     }
  
  //     finalSeries.push(series);
  //   }
  // }

  // var temp = [...finalSeries];
  // return temp;
}

function getDisplayType() {
  var div = document.getElementById('displayListItemTitlespan');
  var inputs = div.getElementsByTagName('input');
  var checkedElements = getCheckedElements(inputs);
  var baseDisplayRadio = null;

  for(var i = 0; i < checkedElements.length; i++) {
    if(checkedElements[i].getAttribute('branch') == 'displaySelector') {
      baseDisplayRadio = checkedElements[i];
      break;
    }
  }

  switch(baseDisplayRadio.id) {
    case 'rateDisplaySelectorradio':
      return 'Rate';
    case 'costDisplaySelectorradio':
      return 'Cost';
    default:
      for(var i = 0; i < checkedElements.length; i++) {
        if(checkedElements[i].getAttribute('branch') == 'usageDisplaySelector') {
          if(checkedElements[i].id == 'consumptionUsageItemsUsageDisplaySelectorradio') {
            return 'Usage';
          }
          else {
            return 'Capacity';
          }
        }
      }
  }
}

function getChartOptions(categories, displayType, xAxisType, dateFormat) {
  return {
    chart: {
        type: getChartTypeFromCategoryCount(categories.length),
    },
    title: {
      text: chartHeaderSpan.innerText,
    },
    yaxis: [{
      title: {
        style: {
          fontSize: '15px',
          fontFamily: 'Helvetica, Arial, sans-serif',
          fontWeight: 400,
        },
        text: getChartYAxisTitle(displayType)
      },
      forceNiceScale: true,
      labels: {
        style: {
          fontSize: '15px',
          fontFamily: 'Helvetica, Arial, sans-serif',
          fontWeight: 400,
        },
        formatter: function(val) {
          return getYAxisLabelFormat(displayType, val);
        }
      }
    }],
    xaxis: {
        type: xAxisType,
        min: categories[0],
        max: categories[categories.length - 1],
        categories: categories,
        axisTicks: {
          show: true,
          color: '#993333',
        },
        labels: {
          rotate: -45,
          rotateAlways: true,
          hideOverlappingLabels: true,
          style: {
            fontSize: '10px',
            fontFamily: 'Helvetica, Arial, sans-serif',
            fontWeight: 400,
          },
        },
        tickPlacement: dateFormat == 'dd MMM yy' ? 'on' : 'between'
    }
  };
}

function getChartTypeFromCategoryCount(categoryCount) {
  return categoryCount == 1 ? 'bar' : 'line';
}

function getChartYAxisTitle(displayType) {
  switch(displayType) {
    case 'Usage':
      return 'kWh';
    case 'Capacity':
      return 'kVa';
    case 'Rate':
      return 'p/kWh';
    default:
      return '£';
  }
}

function getYAxisLabelFormat(displayType, val) {
  switch(displayType) {
    case 'Usage':
    case 'Capacity':
    case 'Rate':
      return val.toLocaleString(); 
    default:
      return '£' + val.toLocaleString();
  }
}

function getXAxisTypeFromTimeSpan() {
  var timePeriodOptionsDisplayTimeSpan = document.getElementById('timePeriodOptionsDisplayTimeSpan');
  switch(timePeriodOptionsDisplayTimeSpan.children[6].innerHTML) {
    case 'Half Hourly':
      return 'datetime';
    default:
      return 'category';
  }
}

function refreshChart(newSeries, displayType, chartOptions) {
  for(var i = 0; i < newSeries.length; i++) {
    newSeries[i].name = newSeries[i].name.replace(' - Usage', '').replace(' - Rate', '').replace(' - Cost', '');
  }

  var options = {
    chart: {
        height: '100%',
        width: '100%',
      type: chartOptions.chart.type,
      zoom: {
        type: 'x',
        enabled: true,
        autoScaleYaxis: true
      },
      animations: {
        enabled: false,
      },
      toolbar: {
        autoSelected: 'zoom',
        tools: {
          download: false
        }
      }
    },
    title: {
      text: chartOptions.title.text,
      align: 'center',
      style: {
        fontSize: '25px',
        fontFamily: 'Arial, Helvetica, sans-serif',
        fontWeight: 'normal',
      }
    },
    dataLabels: {
      enabled: false
    },
    legend: {
      show: true,
      showForSingleSeries: false,
      showForNullSeries: true,
      showForZeroSeries: true,
      position: 'top',
      horizontalAlign: 'center', 
      onItemClick: {
        toggleDataSeries: true
      },
      formatter: function(seriesName) {
        return getLegendFormat(displayType, seriesName);
      },
      fontSize: '20px',
      fontFamily: 'Arial, Helvetica, sans-serif',
      fontWeight: 'normal',
    },
    stroke: {
      width: 2
    },
    colors: ['#69566c', '#61B82E', '#1CB89D', '#3C6B20', '#851B1E', '#C36265', '#104A6B', '#B8B537', '#B8252A', '#0B6B5B'],
    series: newSeries,
    yaxis: chartOptions.yaxis,
    xaxis: chartOptions.xaxis
  };  

  renderChart('#chart', options);
}

function getLegendFormat(displayType, seriesName) {
  return seriesName;
}

function updateDataGrid(chartSeries, categories) {
  var datagrid = document.getElementById('datagrid');
  var data = getDisplayData(chartSeries, categories);
  var columns = getColumns(chartSeries, datagrid);

  clearElement(datagrid);
  
  var options = {
		pagination:50,
		allowInsertRow: false,
		allowManualInsertRow: false,
		allowInsertColumn: false,
		allowManualInsertColumn: false,
		allowDeleteRow: false,
		allowDeleteColumn: false,
		allowRenameColumn: false,
		wordWrap: true,
		data: data,
		columns: columns,
  };

	jexcel(datagrid, options); 
}

function getDisplayData(chartSeries, categories) {
	var data = [];
  var categoryLength = categories.length;

  for(var i = 0; i < categoryLength; i++) {
    var row = {
      period: categories[i]
    };

    for(var j = 0; j < chartSeries.length; j++) {
      row[convertChartSeriesNameToColumnName(chartSeries[j]["name"])] = (chartSeries[j]["data"][i] ?? 0).toLocaleString();
    }
    data.push(row);
  }

  return data;
}

function getColumns(chartSeries, datagrid) {
  var divWidth = document.getElementById('dataHeaderList').clientWidth;
  var periodWidth = Math.floor(document.getElementById('dataHeaderList').clientWidth/15);
  var divWidthMinusPeriodColumnWidth = divWidth - periodWidth - 58;
  var totalColumnWidth = 0;
  var columns = [{type:'text', 
    width:periodWidth, 
    name:'period', 
    title:'Period', 
    readOnly: true},
  ];

  for(var i = 0; i < chartSeries.length; i++) {
    var columnTitle = convertChartSeriesNameToColumnTitle(chartSeries[i]["name"]);
    var columnWidth = calculateColumnTitleWidth(columnTitle, datagrid);
    totalColumnWidth += columnWidth;

    var column = {type:'text', 
      width:columnWidth, 
      name:convertChartSeriesNameToColumnName(chartSeries[i]["name"]), 
      title:columnTitle, 
      readOnly:true,
    };
    columns.push(column);
  }

  if(totalColumnWidth < divWidthMinusPeriodColumnWidth) {
    var multiplier = (Math.floor((divWidthMinusPeriodColumnWidth / totalColumnWidth)*10)/10);

    for(var i = 1; i < columns.length; i++) {
      var currentColumnWidth = columns[i].width;
      var newColumnWidth = currentColumnWidth * multiplier;
      columns[i].width = newColumnWidth;
    }
  }

  return columns;
}

function calculateColumnTitleWidth(columnTitle, datagrid) {
  var div = document.createElement('span');
  div.innerHTML = columnTitle;
  div.setAttribute('style', 'position: absolute; visibility: hidden; height: auto; width: auto; white-space: nowrap;');
  datagrid.appendChild(div);
  return div.clientWidth*2;
}

function convertChartSeriesNameToColumnName(chartSeriesName) {
  return chartSeriesName.split(' ').join('');
}

function convertChartSeriesNameToColumnTitle(chartSeriesName) {
  return chartSeriesName.split(' - ').join('<br>');
}