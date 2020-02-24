<?php 
	$PAGE_TITLE = "Opportunities Dashboard";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<link rel="stylesheet" type="text/css" href="/css/jquery-ui-1.8.4.css" />
<link rel="stylesheet" type="text/css" href="/css/jquery.ganttView.css" />

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="roundborder divcolumn left" style="background-color: #e9eaee;">
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
		<div class="divcolumn middle"></div>
		<div class="roundborder divcolumn right" style="background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Recommended Opportunities</span>
				<br>
				<div class="divcolumn first"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
								<span style="border-bottom: solid black 1px;">Opportunity</span>
							</div>
						</li>
						<br>
						<li>
							<div id="Project1" class="far fa-plus-square" additionallists="Project1ListButtons,Project1ListSavings"></div>
							<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
							<span id="Project1span">LED Lighting Installation</span><i class="fas fa-search show-pointer"></i>
							<div id="Project1List" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<div id="Site1" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site1span">Site X</span>
									</li>
									<li>
										<div id="Site2" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site2span">Site Y</span>
									</li>
									<li>
										<div id="Site3" class="far fa-times-circle"></div>
										<i class="far fa-calender-alt" style="padding-left: 3px; padding-right: 3px;"></i>
										<span id="Site2span">Site Z</span>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
							<span style="border-bottom: solid black 1px;">Approve/Reject</span>
							</div>
						</li>
						<br>
						<li>
							<button class="show-pointer"style="width: 45%; background-color: green;">Approve Opportunity</button>
							&nbsp&nbsp&nbsp&nbsp
							<button class="show-pointer"style="width: 45%; background-color: red;">Reject Opportunity</button>
							<div id="Project1ListButtons" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site X</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site X</button>
									</li>
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site Y</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site Y</button>
									</li>
									<li>
										<button class="show-pointer"style="width: 45%; background-color: green;">Approve Site Z</button>
										&nbsp&nbsp&nbsp
										<button class="show-pointer"style="width: 45%; background-color: red;">Reject Site Z</button>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn" style="width: 30%;">
					<ul class="format-listitem">
						<li>
							<div>
							<span style="border-bottom: solid black 1px;">Estimated Annual Savings</span>
							</div>
						</li>
						<br>
						<li>
							<span>kWh Savings: 10,000</span>
							<br>
							<span>£ Savings: £10,000</span>
							<div id="Project1ListSavings" class="listitem-hidden">
								<ul class="format-listitem">
									<li>
										<span>kWh Savings: 5,000</span>
										<br>
										<span>£ Savings: £5,000</span>
									</li>
									<li>
										<span>kWh Savings: 3,000</span>
										<br>
										<span>£ Savings: £3,000</span>
									</li>
									<li>
										<span>kWh Savings: 2,000</span>
										<br>
										<span>£ Savings: £2,000</span>
									</li>
								</ul>
							</div>
						</li>
					</ul>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
	<div class="row">
		<div class="divcolumn first"></div>
		<div class="roundborder divcolumn left" style="background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Visits</span>
			</div>
			<div class="row">
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div style="text-align: center;">
						<span style="border-bottom: solid black 1px;">Future Site Visits</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer2">
						<table style="width: 100%;">
							<thead>
								<tr>
									<th id="futureSiteVisitsth0" style="border: solid black 1px;">Date Of Visit</th>
									<th id="futureSiteVisitsth1" style="border: solid black 1px;">Engineer</th>
									<th id="futureSiteVisitsth2" style="border: solid black 1px;">Notes</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">???</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/04/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/05/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/06/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/07/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/08/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/09/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/10/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/11/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/12/2020</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/01/2021</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="futureSiteVisitsth0" style="border: solid black 1px;">01/02/2021</td>
									<td headers="futureSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="futureSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
							</tbody>
						</table>
					</div>
					<br>
					<button class="show-pointer"style="width: 100%;">Arrange Visit</button>
				</div>
				<div class="divcolumn middle"></div>
				<div class="divcolumn right">
					<div style="text-align: center;">
						<span style="border-bottom: solid black 1px;">Historical Site Visits</span>
					</div>
					<br>
					<div id="tableContainer" class="tableContainer2">
						<table style="width: 100%;">
							<thead>
								<tr>
									<th id="historicSiteVisitsth0" style="border: solid black 1px;">Date Of Visit</th>
									<th id="historicSiteVisitsth1" style="border: solid black 1px;">Engineer</th>
									<th id="historicSiteVisitsth2" style="border: solid black 1px;">Notes</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/02/2020</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/01/2020</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/12/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/11/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/10/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/09/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/08/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/07/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/06/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/05/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/04/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/03/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/02/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
								<tr>
									<td headers="historicSiteVisitsth0" style="border: solid black 1px;">01/01/2019</td>
									<td headers="historicSiteVisitsth1" style="border: solid black 1px;">En Gineer</td>
									<td headers="historicSiteVisitsth2" style="border: solid black 1px;"><i class="fas fa-search show-pointer"></i></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="divcolumn last"></div>
			</div>
		</div>
		<div class="divcolumn middle"></div>
		<div class="roundborder divcolumn right" style="background-color: #e9eaee;">
			<div style="text-align: center; border-bottom: solid black 1px;">
				<span>Site Ranking</span>
			</div>
			<div class="row" style="text-align: center;">
				<div class="divcolumn first"></div>
				<div class="divcolumn left">
					<div>
						<span style="border-bottom: solid black 1px;">Site</span>
					</div><br>
					<span>Site X</span><br>
					<span>Site Y</span><br>
					<span>Site Z</span><br>
				</div>
				<div class="divcolumn middle">
					<div>
						<span style="border-bottom: solid black 1px;">Rank</span>
					</div><br>
					<span>1</span><br>
					<span>2</span><br>
					<span>3</span><br>
				</div>
				<div class="divcolumn right">
					<div>
						<span style="border-bottom: solid black 1px;">Notes</span>
					</div><br>
					<i class="fas fa-search show-pointer"></i><br>
					<i class="fas fa-search show-pointer"></i><br>
					<i class="fas fa-search show-pointer"></i><br>
				</div>
				<div class="last"></div>
			</div>			
		</div>
		<div class="divcolumn last"></div>
	</div>
	<br>
</body>

<script src="/javascript/utils.js"></script>

<script type="text/javascript">
	addExpanderOnClickEvents();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>