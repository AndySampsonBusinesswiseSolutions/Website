<?php 
	$PAGE_TITLE = "Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/navigation.php");
?>

<body>

<div class="container" id="main-content">
	<h2><?php echo $PAGE_TITLE ?></h2>
	<table border="1">
		<tr>
			<td>
				<p>Some information about Latest Bill</p>
			</td>
			<td>
				<p>Some information about Next Bill</p>
			</td>
		</tr>
	</table>
	<table border="1">
		<tr>
			<td>
				Where user has multiple customer entites, allow portfolio and individual customer view
				Show logo for parent if multiple customer entites other individual logo
				Show name(s) of customer entities being viewed - with parent if exists
				<p>Interactive image that allows user to view information about meters & sub-meters/assets</p>
				<p>Geographical Map of sites</p>
				Summary of portfolio - no of sites (split by commodity/HH_NHH)
				Summary of last 12 months electricity spend £
				Summary of last 12 months consumption
				Summary of best forecast for next 12 months
				Tradable position
				Last 5 trades
				Latest market ICE closed prices for next 4 seasons
			</td>
		</tr>
	</table>
</div>

</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/Website/UI/includes/footer.php");?>