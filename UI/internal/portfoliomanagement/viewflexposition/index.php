<?php 
	$PAGE_TITLE = "View Flex Position";
	 include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

  <link rel="stylesheet" href="viewflexposition.css">
  <link rel="stylesheet" href="https://bossanova.uk/jexcel/v3/jexcel.css" type="text/css" />
  <link rel="stylesheet" href="https://bossanova.uk/jsuites/v2/jsuites.css" type="text/css" />
</head>

<body>
	<div id="mySidenav" class="sidenav">
		<i class="fas fa-angle-double-left sidenav-icon closebtn" onclick="closeNav()"></i>
		<div class="tree-column">
			<div id="treeDiv" class="tree-div roundborder">
        <span style="padding-left: 5px;">Select Items To Display <i class="far fa-plus-square show-pointer" id="itemsToDisplaySelector"></i></span>
				<ul class="format-listitem" id="itemsToDisplaySelectorList">
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

	<div class="section-header">
		<i id="openNav" class="fas fa-angle-double-right sidenav-icon" onclick="openNav()"></i>
		<div class="section-header-text"><?php echo $PAGE_TITLE ?></div>
	</div>
  <br>
  <div class="final-column">
    <div id="electricityVolumeContainer" class="roundborder tree-div">
        <div style="text-align: center; border-bottom: solid black 1px;">
          <span>Electricity Volume</span>
          <div id="electricityVolume" class="far fa-plus-square show-pointer"></div>
				  <div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Electricity Flex Volume To Download Basket"></div>
				  <div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Electricity Flex Volume"></div>
        </div>
        <div id="electricityVolumeList" class="roundborder chart">
          <div id="electricityVolumeChart"></div>
        </div>
    </div>
    <div id="electricityPriceContainer" class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center; border-bottom: solid black 1px;">
          <span>Electricity Price</span>
          <div id="electricityPrice" class="far fa-plus-square show-pointer"></div>
				  <div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Electricity Flex Prices To Download Basket"></div>
				  <div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Electricity Flex Prices"></div>
        </div>
        <div id="electricityPriceList" class="roundborder chart listitem-hidden">
            <div id="electricityPriceChart"></div>
        </div>
    </div>
    <div id="gasVolumeContainer" class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center; border-bottom: solid black 1px;">
          <span>Gas Volume</span>
          <div id="gasVolume" class="far fa-plus-square show-pointer"></div>
				  <div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Gas Flex Volume To Download Basket"></div>
				  <div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Gas Flex Volume"></div>
        </div>
        <div id="gasVolumeList" class="roundborder chart listitem-hidden">
            <div id="gasVolumeChart"></div>
        </div>
    </div>
    <div id="gasPriceContainer" class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center; border-bottom: solid black 1px;">
            <span>Gas Price</span>
            <div id="gasPrice" class="far fa-plus-square show-pointer"></div>
				  <div class="fas fa-cart-arrow-down show-pointer" style="float: right;" title="Add Gas Flex Prices To Download Basket"></div>
				  <div class="fas fa-download show-pointer" style="margin-right: 5px; float: right;" title="Download Gas Flex Prices"></div>
        </div>
        <div id="gasPriceList" class="roundborder chart listitem-hidden">
            <div id="gasPriceChart"></div>
        </div>
    </div>
    <div id="electricityDatagridsContainer" class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center; border-bottom: solid black 1px;">
            <span>Electricity Datagrids</span>
            <div id="electricityDatagrids" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="electricityDatagridsList" class="roundborder chart listitem-hidden">
          <div class="first"></div>
          <div class="left">
            <div id="spreadsheet3" style="margin-top: 5px;"></div>
          </div>
          <div class="middle"></div>
          <div class="right">
            <div id="spreadsheet4" style="margin-top: 5px;"></div>
          </div>
          <div class="last"></div>
        </div>
    </div>
    <div id="gasDatagridsContainer" class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center; border-bottom: solid black 1px;">
            <span>Gas Datagrids</span>
            <div id="gasDatagrids" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="gasDatagridsList" class="roundborder chart listitem-hidden">
          <div class="first"></div>
          <div class="left">
            <div id="spreadsheet5" style="margin-top: 5px;"></div>
          </div>
          <div class="middle"></div>
          <div class="right">
            <div id="spreadsheet6" style="margin-top: 5px;"></div>
          </div>
          <div class="last"></div>
        </div>
    </div>
  </div> 
  <br>
</body>

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