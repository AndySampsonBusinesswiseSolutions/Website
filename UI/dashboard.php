<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
  <style>
  .draggable { width: 90px; height: 80px; padding: 5px; float: left; margin: 0 10px 10px 0; font-size: .9em; }
  .ui-widget-header p, .ui-widget-content p { margin: 0; }
  #snaptarget { height: 140px; }
  </style>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div id="snaptarget" class="ui-widget-header">
  <p>I'm a snap target</p>
</div>
 
<br style="clear:both">
 
<div id="draggable" class="draggable ui-widget-content">
  <p>Default (snap: true), snaps to all other draggable elements</p>
</div>
 
<div id="draggable2" class="draggable ui-widget-content">
  <p>I only snap to the big box</p>
</div>
 
<div id="draggable3" class="draggable ui-widget-content">
  <p>I only snap to the outer edges of the big box</p>
</div>
 
<div id="draggable4" class="draggable ui-widget-content">
  <p>I snap to a 20 x 20 grid</p>
</div>
 
<div id="draggable5" class="draggable ui-widget-content">
  <p>I snap to a 80 x 80 grid</p>
</div>

	<!-- <div">
		<div class="fusion-row"  style="padding-left: 15px;>
			<div class="fusion-columns fusion-columns-3 fusion-widget-area">
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					<a class="fusion-logo-link" href="https://www.businesswisesolutions.co.uk/">
                        <img src="https://www.dfs.co.uk/wcsstore/DFSStorefrontAssetStore/images/dfs_logo.svg" srcset="https://www.dfs.co.uk/wcsstore/DFSStorefrontAssetStore/images/dfs_logo.svg 1x" width="204" height="52" alt="Businesswise Solutions Logo" retina_logo_url="" class="fusion-standard-logo">
                    </a>
				</div>
				<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
					Meter & SubMeter Interactive Image
				</div>
			</div>
		</div>
		<div class="fusion-row" style="padding-left: 15px;>
			<div class="fusion-columns fusion-columns-3 fusion-widget-area">
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Summaries
					<div class="fusion-row">
						<div class="fusion-columns fusion-columns-3 fusion-widget-area" style="background-color: crimson; color: white; width: 50%; display: block-inline;">
							<div class="fusion-row" align="center" style="font-size: 40px">
								Site Count
							</div>
							<div class="fusion-row" align="center">
								Electricity: 10 Half Hourly / 253 Non-Half Hourly
							</div>
							<div class="fusion-row" align="center">
								Gas: 0 Daily Metered / 3 Non-Daily Metered
							</div>
						</div>
						<div class="fusion-columns fusion-columns-3 fusion-widget-area" style="background-color: green; color: white; width: 50%; display: block-inline;">
							<div class="fusion-row" align="center">
								<i class="fas fa-pound-sign"></i>
							</div>
							<div class="fusion-row" align="center">
								Previous 12 Months Spend
							</div>
							<div class="fusion-row" align="center">
								£3,100,102
							</div>
						</div>
					</div>
					<div class="fusion-row">
						<div class="fusion-columns fusion-columns-3 fusion-widget-area">
							Previous 12 Months Consumption Summary
						</div>
						<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
							Forecast 12 Months Consumption Summary
						</div>
					</div>
				</div>
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Actual Usage & Cost
				</div>
				<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
					Map
				</div>
			</div>
		</div>
		<div class="fusion-row" style="padding-left: 15px;>
			<div class="fusion-columns fusion-columns-3 fusion-widget-area">
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Invoices
				</div>
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Project Performance
				</div>
				<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
					Half Hourly Usage
				</div>
			</div>
		</div>
		<div class="fusion-row" style="padding-left: 15px;>
			<div class="fusion-columns fusion-columns-3 fusion-widget-area">
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Latest Bill
				</div>
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Projected Next Bill
				</div>
				<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
					Last 5 Trades
				</div>
			</div>
		</div>
		<div class="fusion-row" style="padding-left: 15px;>
			<div class="fusion-columns fusion-columns-3 fusion-widget-area">
				<div class="fusion-column col-lg-4 col-md-4 col-sm-4">
					Tradeable Positions
				</div>
				<div class="fusion-column fusion-column-last col-lg-4 col-md-4 col-sm-4">
					Market Graphs
				</div>
			</div>
		</div>
	<div> -->
</body>

<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
  <script>
  $( function() {
    $( "#draggable" ).draggable({ snap: true });
    $( "#draggable2" ).draggable({ snap: ".ui-widget-header" });
    $( "#draggable3" ).draggable({ snap: ".ui-widget-header", snapMode: "outer" });
    $( "#draggable4" ).draggable({ grid: [ 20, 20 ] });
    $( "#draggable5" ).draggable({ grid: [ 80, 80 ] });
  } );
  </script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>