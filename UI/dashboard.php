<?php 
	if(!session_id()) session_start();
	$PAGE_TITLE = "Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
<style>
  .dragable { padding: 0.5em; float: left; margin: 0 10px 10px 0; }
  #containment-wrapper { width: 100%; height:800px; border:2px solid #ccc; padding: 10px; }
</style>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div id="containment-wrapper">
		<div id="dragable1" class="dragable ui-widget-content" style="width: 90px; height: 90px;">
			<p>I'm contained within the box</p>
		</div>
		<div id="dragable2" class="dragable ui-widget-content" style="width: 90px; height: 90px;">
			<p>I'm contained within the box</p>
		</div>
		<div id="dragable3" class="dragable ui-widget-content" style="width: 90px; height: 90px;">
			<p>I'm contained within the box</p>
		</div>
		<div id="dragable4" class="dragable ui-widget-content" style="width: 180px; height: 180px;">
			<p>I'm contained within the box</p>
		</div>
	</div>
</body>

<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
  <script>
  $( function() {
	  var dragables = document.getElementsByClassName('ui-widget-content');

	  for(var i = 0; i < dragables.length; i++) {
		$(dragables[i]).draggable({ containment: "#containment-wrapper", scroll: false, snap: true, cursor: "move" });
	  }	
  } );
  </script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>