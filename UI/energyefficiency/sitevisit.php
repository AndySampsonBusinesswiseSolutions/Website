<?php 
	$PAGE_TITLE = "Site Visit";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<body>
    <div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
    <br>
    <div class="row" style="padding-left: 15px; padding-right: 15px;">
        <div class="divcolumn first"></div>
        <div style="border: solid black 1px;" class="divcolumn left"></div>
        <div class="divcolumn middle"></div>
        <div style="border: solid black 1px;" class="divcolumn right"></div>
        <div class="divcolumn last"></div>
    </div>
    <br>
    <div class="row" style="padding-left: 15px; padding-right: 15px;">
        <div class="divcolumn first"></div>
        <div style="border: solid black 1px; width: 96%;"></div>
        <div class="divcolumn last"></div>
    </div>
    <br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>