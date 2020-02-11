<?php 
	$PAGE_TITLE = "Opportunities Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="divcolumn left" style="border: solid black 1px; background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Visits</span>
			</div>
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Historical Site Visits</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer2">
						<table style="width: 100%;">
							<thead class="fixedHeader">
								<tr>
									<th style="border: solid black 1px;">Date Of Visit</th>
									<th style="border: solid black 1px;">Engineer</th>
									<th style="border: solid black 1px;">Notes</th>
								</tr>
							</thead>
							<tbody class="scrollContent2">
								<tr>
									<td style="width: 164px; border: solid black 1px;">01/02/2020</td>
									<td style="width: 123px; border: solid black 1px;">En Gineer</td>
									<td style="width: 70px; border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/01/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/12/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/11/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/10/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/09/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/08/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/07/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/06/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/05/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/04/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/03/2019</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn right">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Future Site Visits</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer">
						<table style="width: 100%;">
							<thead class="fixedHeader">
								<tr>
									<th style="border: solid black 1px;">Date Of Visit</th>
									<th style="border: solid black 1px;">Engineer</th>
									<th style="border: solid black 1px;">Notes</th>
								</tr>
							</thead>
							<tbody class="scrollContent">
								<tr>
									<td style="width: 164px; border: solid black 1px;">???</td>
									<td style="width: 123px; border: solid black 1px;">En Gineer</td>
									<td style="width: 70px; border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/04/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/05/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/06/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/07/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/08/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/09/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/10/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/11/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/12/2020</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/01/2021</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td style="border: solid black 1px;">01/02/2021</td>
									<td style="border: solid black 1px;">En Gineer</td>
									<td style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
							</tbody>
						</table>
					</div>
					<br>
					<button style="width: 100%;">Arrange Site Visit</button>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn middle"></div>
		<div class="divcolumn right" style="border: solid black 1px; background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Opportunity Summary</span>
				<br>
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<span>Number of Active Opportunities</span><br>
					<span>Number of Pending Opportunities</span><br>
					<span>Number of Finished Opportunities</span><br>
					<br>
					<span>Total kWh Savings of Finished Opportunities</span><br>
					<span>Total £ Savings of Finished Opportunities</span><br>
					<span>kWh Savings of Finished Opportunities over past 12 months</span><br>
					<span>£ Savings of Finished Opportunities over past 12 months</span><br>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn right">
					<span>2</span><br>
					<span>10</span><br>
					<span>5</span><br>
					<br>
					<span>300,000</span><br>
					<span>£10,000</span><br>
					<span>300,000</span><br>
					<span>£10,000</span><br>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="divcolumn left" style="border: solid black 1px; background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Recommended Opportunities</span>
				<br>
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Opportunity</span>
					</div><br>
					<span>LED Lighting Installation</span><br>
				</div>
				<div class="divcolumn middle">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Notes</span>
					</div><br>
					<i class="fas fa-search show-pointer"></i><br>
				</div>
				<div class="divcolumn right">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Approve/Reject</span>
					</div><br>
					<button style="width: 25%; background-color: green">Approve</button>
					&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
					<button style="width: 25%; background-color: red">Reject</button><br>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn middle"></div>
		<div class="divcolumn right" style="border: solid black 1px; background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Ranking</span>
				<br>
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Site</span>
					</div><br>
					<span>Site X</span><br>
					<span>Site Y</span><br>
					<span>Site Z</span><br>
				</div>
				<div class="divcolumn middle">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Rank</span>
					</div><br>
					<span>1</span><br>
					<span>2</span><br>
					<span>3</span><br>
				</div>
				<div class="divcolumn right">
					<div style="text-align: center; border-bottom: solid black 1px;">
						<span>Notes</span>
					</div><br>
					<i class="fas fa-search show-pointer"></i><br>
					<i class="fas fa-search show-pointer"></i><br>
					<i class="fas fa-search show-pointer"></i><br>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>