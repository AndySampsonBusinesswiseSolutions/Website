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
  <div id="outerContainer">
		<div id="mainContainer">
    <div class="section-header">
				<div id="mySidenav" class="sidenav" style="display: none;">
					<div class="header">
						<button class="closebtn" onclick="closeNav()">Close</button>
						<i class="fas fa-filter sidenav-icon-close"></i>
					</div>
					<div class="tree-column">
            <div class="expander-header">
              <span id="commoditySelectorSpan">Commodity</span>
              <i id="commoditySelector" class="far fa-plus-square expander openExpander show-pointer"></i>
            </div>
            <div id="commoditySelectorList" class="expander-container scrolling-wrapper">
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
					<div style="clear: both;"></div>
					<div class="header">
						<button class="resetbtn" onclick="pageLoad(true)">Reset To Default</button>
						<button class="applybtn" onclick="closeNav()">Done</button>
					</div>
				</div>
				<i id="openNav" class="fas fa-filter sidenav-icon" onclick="openNav()"></i>
				<div class="section-header-text"><?php echo $PAGE_TITLE ?><i class="far fa-question-circle show-pointer" title="Items can be added to the Dashboard using the 'Filter' icon on the left-hand side"></i></div>
			</div>
      <div class="final-column">
				<div id="overlay" style="display: none;">
				</div>
        <div id="electricityVolumeContainer" class="dashboard expander-container outer-container">
          <div class="expander-header">
            <span>Electricity</span>
            <i id="electricityVolume" class="far fa-plus-square show-pointer expander openExpander"></i>
            <i class="fas fa-cart-arrow-down show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Add Electricity Flex To Download Basket"></i>
            <i class="fas fa-download show-pointer" style="margin-top: 3px; margin-right: 5px; float: right;" title="Download Electricity Flex"></i>
          </div>
          <div id="electricityVolumeList" class="expander-container">
            <div id="leftHandChartDiv" class="chart" style="float: left;">
              <div id="electricityVolumeChart"></div>
            </div>
            <div id="rightHandChartDiv" class="chart" style="float: right;">
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
                <div id="spreadsheet4" class="expander-container"></div>
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
            <div id="leftHandChartDiv" class="chart" style="float: left;">
              <div id="gasVolumeChart"></div>
            </div>
            <div id="rightHandChartDiv" class="chart" style="float: right;">
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
                <div id="spreadsheet6" class="expander-container"></div>
              </div>
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