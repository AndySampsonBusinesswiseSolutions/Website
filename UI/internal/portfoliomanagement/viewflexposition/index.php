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
    <div style="text-align: center;">
			<span id="selectOptionsSpan" style="font-size: 25px;">Options</span>
      <i class="fas fa-unlock fa-w-14 sidenav-icon lock" onclick="lockSidebar(); updatePage()" title="Click To Lock Sidebar"></i>
      <i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
    </div>
    <div class="tree-column">
			<div id="configureContainer" class="dashboard roundborder outer-container">
				<div class="expander-header">
					<span id="configureOptionsSpan">Configure</span>
					<i id="configureOptions" class="far fa-plus-square expander show-pointer"></i>
				</div>
				<div id="configureOptionsList" class="slider-list expander-container listitem-hidden">
					<div class="tree-div dashboard roundborder outer-container scrolling-wrapper">
						<div class="expander-header">
							<span id="commoditySelectorSpan">Commodity</span>
							<i id="commoditySelector" class="far fa-plus-square expander show-pointer"></i>
						</div>
						<div id="commoditySelectorList" class="expander-container listitem-hidden">
							<div style="width: 45%; text-align: center; float: left;">
								<span>Electricity</span>
								<label class="switch"><input type="checkbox" id="electricityCommoditycheckbox" checked onclick='showHideContainer(this);'></input><div class="switch-btn"></div></label>
							</div>
							<div style="width: 45%; text-align: center; float: right;">
								<span>Gas</span>
								<label class="switch"><input type="checkbox" id="gasCommoditycheckbox" checked onclick='showHideContainer(this);'></input><div class="switch-btn"></div></label>
              </div>
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
      <div class="final-column">
        <div id="electricityVolumeContainer" class="dashboard expander-container outer-container">
          <div class="expander-header">
            <span>Electricity</span>
            <i id="electricityVolume" class="far fa-plus-square show-pointer expander openExpander"></i>
            <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Electricity Flex To Download Basket"></i>
            <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Electricity Flex"></i>
          </div>
          <div id="electricityVolumeList" class="expander-container">
            <div id="leftHandChartDiv" class="roundborder chart" style="float: left;">
              <div id="electricityVolumeChart"></div>
            </div>
            <div id="rightHandChartDiv" class="roundborder chart" style="float: right;">
              <div id="electricityPriceChart"></div>
            </div>
            <div style="clear: both;"></div>
            <div id="electricityDataContainer" class="dashboard expander-container outer-container">
              <div class="expander-header">
                <span>Electricity Data</span>
                <i id="electricityData" class="far fa-plus-square show-pointer expander"></i>
              </div>
              <div id="electricityDataList" class="expander-container listitem-hidden" style="text-align: center;">
                <div id="spreadsheet3" class="expander-container"></div>
              </div>
            </div>
          </div>			
        </div>
        <div id="gasVolumeContainer" class="dashboard expander-container outer-container">
          <div class="expander-header">
            <span>Gas</span>
            <i id="gasVolume" class="far fa-plus-square show-pointer expander"></i>
            <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Gas Flex To Download Basket"></i>
            <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Gas Flex"></i>
          </div>
          <div id="gasVolumeList" class="expander-container listitem-hidden">
            <div id="leftHandChartDiv" class="roundborder chart" style="float: left;">
              <div id="gasVolumeChart"></div>
            </div>
            <div id="rightHandChartDiv" class="roundborder chart" style="float: right;">
              <div id="gasPriceChart"></div>
            </div>
            <div style="clear: both;"></div>
            <div id="gasDataContainer" class="dashboard expander-container outer-container">
              <div class="expander-header">
                <span>Gas Data</span>
                <i id="gasData" class="far fa-plus-square show-pointer expander"></i>
              </div>
              <div id="gasDataList" class="expander-container listitem-hidden" style="text-align: center;">
                <div id="spreadsheet5" class="expander-container"></div>
              </div>
            </div>
          </div>			
        </div>
        <div id="leftHandTradeDiv" style="float: left; width: 49.5%;">
          <div id="electricityDatagridContainer" class="dashboard outer-container expander-container">
            <div class="expander-header">
                <span>Electricity Trade Information</span>
                <i id="electricityDatagrid" class="far fa-plus-square show-pointer expander"></i>
            </div>
            <div id="electricityDatagridList" class="roundborder expander-container listitem-hidden" style="text-align: center;">
              <div id="spreadsheet4" class="expander-container"></div>
            </div>
          </div>
        </div>
        <div id="rightHandTradeDiv" style="float: right; width: 49.5%;">
          <div id="gasDatagridContainer" class="dashboard outer-container expander-container">
            <div class="expander-header">
                <span>Gas Trade Information</span>
                <i id="gasDatagrid" class="far fa-plus-square show-pointer expander"></i>
            </div>
            <div id="gasDatagridList" class="roundborder expander-container listitem-hidden" style="text-align: center;">
              <div id="spreadsheet6" class="expander-container"></div>
            </div>
          </div>
        </div>
      </div> 
    </div>
  </div>
</body>

<script type="text/javascript" src="/includes/base/base.js"></script>
<script type="text/javascript" src="viewflexposition.js"></script>
<script type="text/javascript" src="/includes/apexcharts/apexcharts.js"></script>
<script type="text/javascript" src="/includes/jexcel/jexcel.js"></script>
<script type="text/javascript" src="/includes/jsuites/jsuites.js"></script>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<script type="text/javascript"> 
  pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>