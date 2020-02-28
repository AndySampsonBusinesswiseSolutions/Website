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
  <div class="panel-group" id="accordion1" style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordion1" href="#collapse1" class="expandicon" style="padding-left: 15px;">Electricity Volume</a>
        </div>
        <div id="collapse1" class="panel-collapse collapse">
            <div class="panel-body">
              <div class="row" style="margin-left: 15px; margin-right: 15px; margin-bottom: 15px;">
                <div class="final-column">
                  <div class="tree-div" style="height: 1000px;">
                    <div id="electricityVolumeChart"></div>
                  </div>
                </div>	
              </div>
            </div>
        </div>
    </div>
  </div>
  <div class="panel-group" id="accordion2" style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">

            <a data-toggle="collapse" data-parent="#accordion2" href="#collapse2" class="expandicon" style="padding-left: 15px;">Electricity Price</a>

        </div>
        <div id="collapse2" class="panel-collapse collapse">
            <div class="panel-body">
              <div class="row" style="margin-left: 15px; margin-right: 15px; margin-bottom: 15px;">
                <div class="final-column">
                  <div class="tree-div" style="height: 1000px;">
                    <div id="electricityPriceChart"></div>
                  </div>
                </div>	
              </div>
            </div>
        </div>
    </div>
  </div>
  <div class="panel-group" id="accordion3" style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">

            <a data-toggle="collapse" data-parent="#accordion3" href="#collapse3" class="expandicon" style="padding-left: 15px;">Gas Volume</a>

        </div>
        <div id="collapse3" class="panel-collapse collapse">
            <div class="panel-body">
              <div class="row" style="margin-left: 15px; margin-right: 15px; margin-bottom: 15px;">
                <div class="final-column">
                  <div class="tree-div" style="height: 1000px;">
                    <div id="gasVolumeChart"></div>
                  </div>
                </div>	
              </div>
            </div>
        </div>
    </div>
  </div>
  <div class="panel-group" id="accordion4" style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">

            <a data-toggle="collapse" data-parent="#accordion4" href="#collapse4" class="expandicon" style="padding-left: 15px;">Gas Price</a>

        </div>
        <div id="collapse4" class="panel-collapse collapse">
            <div class="panel-body">
              <div class="row" style="margin-left: 15px; margin-right: 15px; margin-bottom: 15px;">
                <div class="final-column">
                  <div class="tree-div" style="height: 1000px;">
                    <div id="gasPriceChart"></div>
                  </div>
                </div>	
              </div>
            </div>
        </div>
    </div>
  </div>
  <div class="panel-group" id="accordion5" style="padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">

            <a data-toggle="collapse" data-parent="#accordion5" href="#collapse5" class="expandicon" style="padding-left: 15px;">Electricity Datagrids</a>

        </div>
        <div id="collapse5" class="panel-collapse collapse">
            <div class="panel-body">
              <div class="row roundborder" style="margin-left: 15px; margin-right: 15px; margin-bottom: 15px;">
                <div class="divcolumn first"></div>
                <div class="divcolumn left">
                  <div id="spreadsheet3"></div>
                </div>
                <div class="divcolumn middle"></div>
                <div class="divcolumn right">
                  <div id="spreadsheet4"></div>
                </div>
                <div class="divcolumn last"></div>
              </div>
            </div>
        </div>
    </div>
  </div>
  <div class="panel-group" id="accordion6" style="padding-left: 15px; padding-right: 15px;">
    <div class="panel panel-default" style="border: solid black 1px;">
        <div class="panel-heading">

            <a data-toggle="collapse" data-parent="#accordion6" href="#collapse6" class="expandicon" style="padding-left: 15px;">Gas Datagrids</a>

        </div>
        <div id="collapse6" class="panel-collapse collapse">
            <div class="panel-body">
            </div>
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