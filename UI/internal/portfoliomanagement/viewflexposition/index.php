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
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
  <br>
  <div class="final-column">
    <div class="roundborder tree-div">
        <div style="text-align: center;">
            <span>Electricity Volume</span>
            <div id="electricityVolume" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="electricityVolumeList" class="roundborder chart">
            <div id="electricityVolumeChart"></div>
        </div>
    </div>
    <div class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center;">
            <span>Electricity Price</span>
            <div id="electricityPrice" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="electricityPriceList" class="roundborder chart listitem-hidden">
            <div id="electricityPriceChart"></div>
        </div>
    </div>
    <div class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center;">
            <span>Gas Volume</span>
            <div id="gasVolume" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="gasVolumeList" class="roundborder chart listitem-hidden">
            <div id="gasVolumeChart"></div>
        </div>
    </div>
    <div class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center;">
            <span>Gas Price</span>
            <div id="gasPrice" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="gasPriceList" class="roundborder chart listitem-hidden">
            <div id="gasPriceChart"></div>
        </div>
    </div>
    <div class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center;">
            <span>Electricity Datagrids</span>
            <div id="electricityDatagrids" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="electricityDatagridsList" class="roundborder chart listitem-hidden">
          <div class="first"></div>
          <div class="left">
            <div id="spreadsheet3"></div>
          </div>
          <div class="middle"></div>
          <div class="right">
            <div id="spreadsheet4"></div>
          </div>
          <div class="last"></div>
        </div>
    </div>
    <div class="roundborder tree-div" style="margin-top: 5px;">
        <div style="text-align: center;">
            <span>Gas Datagrids</span>
            <div id="gasDatagrids" class="far fa-plus-square show-pointer"></div>
        </div>
        <div id="gasDatagridsList" class="roundborder chart listitem-hidden">
          <div class="first"></div>
          <div class="left">
            <div id="spreadsheet5"></div>
          </div>
          <div class="middle"></div>
          <div class="right">
            <div id="spreadsheet6"></div>
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