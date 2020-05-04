<?php 
	$PAGE_TITLE = "Flexible Procurement";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

  <link rel="stylesheet" href="viewflexposition.css">
</head>

<body>
  <div id="mySidenav" class="sidenav">
    <i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar()" title="Click To Lock Sidebar"></i>
    <i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
    <div class="tree-column">
      <div class="dashboard roundborder outer-container">
        <div class="expander-header">
          <span id="selectOptionsSpan">Select Options</span>
          <i id="selectOptions" class="far fa-plus-square expander show-pointer openExpander"></i>
        </div>
        <div id="selectOptionsList" class="expander-container">
          <div class="sidebar-tree-div roundborder scrolling-wrapper">
            <div class="expander-header">
              <span id="itemsToDisplaySelectorSpan">Items To Display</span>
              <i id="itemsToDisplaySelector" class="far fa-plus-square expander show-pointer openExpander"></i>
            </div>
            <div id="itemsToDisplaySelectorList" class="expander-container">
              <ul class="format-listitem toplistitem">
                <li>
                  <input type="checkbox" id="electricityVolumeCheckbox" checked onclick='showHideContainer(this);'><span id="allCommodityspan" style="padding-left: 1px;">Electricity Volume</span>
                </li>
                <li>
                  <input type="checkbox" id="electricityPriceCheckbox" checked onclick='showHideContainer(this);'><span id="electricityCommodityspan" style="padding-left: 1px;">Electricity Price</span>
                </li>
                <li>
                  <input type="checkbox" id="gasVolumeCheckbox" checked onclick='showHideContainer(this);'><span id="allCommodityspan" style="padding-left: 1px;">Gas Volume</span>
                </li>
                <li>
                  <input type="checkbox" id="gasPriceCheckbox" checked onclick='showHideContainer(this);'><span id="electricityCommodityspan" style="padding-left: 1px;">Gas Price</span>
                </li>
                <li>
                  <input type="checkbox" id="electricityDatagridsCheckbox" checked onclick='showHideContainer(this);'><span id="allCommodityspan" style="padding-left: 1px;">Electricity Datagrids</span>
                </li>
                <li>
                  <input type="checkbox" id="gasDatagridsCheckbox" checked onclick='showHideContainer(this);'><span id="electricityCommodityspan" style="padding-left: 1px;">Gas Datagrids</span>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div id="outerContainer">
		<div id="mainContainer">
      <div class="section-header">
        <i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
        <div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
      </div>
      <br>
      <div class="final-column">
        <div id="electricityVolumeContainer" class="dashboard roundborder outer-container">
          <div class="expander-header">
            <span>Electricity Volume</span>
              <i id="electricityVolume" class="far fa-plus-square show-pointer expander openExpander"></i>
              <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Electricity Flex Volume To Download Basket"></i>
              <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Electricity Flex Volume"></i>
          </div>
          <div id="electricityVolumeList" class="roundborder chart expander-container">
            <div id="electricityVolumeChart"></div>
          </div>
        </div>
        <div id="electricityPriceContainer" class="dashboard roundborder outer-container expander-container">
            <div class="expander-header">
              <span>Electricity Price</span>
              <i id="electricityPrice" class="far fa-plus-square show-pointer expander"></i>
              <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Electricity Flex Prices To Download Basket"></i>
              <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Electricity Flex Prices"></i>
            </div>
            <div id="electricityPriceList" class="roundborder chart expander-container listitem-hidden">
                <div id="electricityPriceChart"></div>
            </div>
        </div>
        <div id="gasVolumeContainer" class="dashboard roundborder outer-container expander-container">
            <div class="expander-header">
              <span>Gas Volume</span>
              <i id="gasVolume" class="far fa-plus-square show-pointer expander"></i>
              <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Gas Flex Volume To Download Basket"></i>
              <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Gas Flex Volume"></i>
            </div>
            <div id="gasVolumeList" class="roundborder chart expander-container listitem-hidden">
                <div id="gasVolumeChart"></div>
            </div>
        </div>
        <div id="gasPriceContainer" class="dashboard roundborder outer-container expander-container">
            <div class="expander-header">
                <span>Gas Price</span>
                <i id="gasPrice" class="far fa-plus-square show-pointer expander"></i>
              <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Gas Flex Prices To Download Basket"></i>
              <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Gas Flex Prices"></i>
            </div>
            <div id="gasPriceList" class="roundborder chart expander-container listitem-hidden">
                <div id="gasPriceChart"></div>
            </div>
        </div>
        <div id="electricityDatagridsContainer" class="dashboard roundborder outer-container expander-container">
            <div class="expander-header">
                <span>Electricity Datagrids</span>
                <i id="electricityDatagrids" class="far fa-plus-square show-pointer expander"></i>
            </div>
            <div id="electricityDatagridsList" class="roundborder chart expander-container listitem-hidden">
              <div class="first"></div>
              <div class="left">
                <div id="spreadsheet3" class="expander-container"></div>
              </div>
              <div class="middle"></div>
              <div class="right">
                <div id="spreadsheet4" class="expander-container"></div>
              </div>
              <div class="last"></div>
            </div>
        </div>
        <div id="gasDatagridsContainer" class="dashboard roundborder outer-container expander-container">
            <div class="expander-header">
                <span>Gas Datagrids</span>
                <i id="gasDatagrids" class="far fa-plus-square show-pointer expander"></i>
            </div>
            <div id="gasDatagridsList" class="roundborder chart expander-container listitem-hidden">
              <div class="first"></div>
              <div class="left">
                <div id="spreadsheet5" class="expander-container"></div>
              </div>
              <div class="middle"></div>
              <div class="right">
                <div id="spreadsheet6" class="expander-container"></div>
              </div>
              <div class="last"></div>
            </div>
        </div>
      </div> 
      <br>
    </div>
  </div>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="viewflexposition.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<script type="text/javascript"> 
  pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>