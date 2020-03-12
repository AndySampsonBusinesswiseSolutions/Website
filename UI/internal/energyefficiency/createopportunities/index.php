<?php 
	$PAGE_TITLE = "Create Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation/navigation.php");
?>

<!DOCTYPE html>
<html>
<head>
	<title><?php echo $PAGE_TITLE ?></title>

	<link rel="stylesheet" href="createopportunities.css">
</head>

<body>
    <div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
    <br>
    <div class="row" style="padding-left: 30px; padding-right:30px;">
        <div class="roundborder" style="padding-left: 5px; padding-right:5px;">
            <div style="text-align: center;">
                <span style="border-bottom: solid black 1px;">Identified Opportunities<br></span>
                <br>
                <table style="width: 100%;">
                    <thead>
                        <tr>
                            <td style="border: solid black 1px;">Opportunity Type</td>
                            <td style="border: solid black 1px;">Opportunity Name</td>
                            <td style="border: solid black 1px;">Status</td>
                            <td style="border: solid black 1px;">Site</td>
                            <td style="border: solid black 1px;">Meter</td>
                            <td style="border: solid black 1px;">Sub Meter</td>
                            <td style="border: solid black 1px;">Month</td>
                            <td style="border: solid black 1px;">Day Of Week</td>
                            <td style="border: solid black 1px;">Time Period</td>
                            <td style="border: solid black 1px;">Estimated Cost</td>
                            <td style="border: solid black 1px;">Percentage Saving</td>
                            <td style="border: solid black 1px;">Estimated Annual kWh Savings</td>
                            <td style="border: solid black 1px;">Estimated Annual £ Savings</td>
                            <td style="border: solid black 1px;"><input type="checkbox">&nbsp<i class="fas fa-trash-alt"></i></input</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="border: solid black 1px;">Custom</td>
                            <td style="border: solid black 1px;">LED Lighting</td>
                            <td style="border: solid black 1px;">Recommend</td>
                            <td style="border: solid black 1px;">Site X</td>
                            <td style="border: solid black 1px;">12345678910125</td>
                            <td style="border: solid black 1px;">Sub Meter 2</td>
                            <td style="border: solid black 1px;">All Months</td>
                            <td style="border: solid black 1px;">All Days</td>
                            <td style="border: solid black 1px;">All Periods</td>
                            <td style="border: solid black 1px;">£5,000</td>
                            <td style="border: solid black 1px;">10%</td>
                            <td style="border: solid black 1px;">5,000</td>
                            <td style="border: solid black 1px;">£1,000</td>
                            <td style="border: solid black 1px;"><input type="checkbox">&nbsp<i class="fas fa-trash-alt"></i></input</td>
                        </tr>
                        <tr>
                            <td style="border: solid black 1px;">Custom</td>
                            <td style="border: solid black 1px;">LED Lighting</td>
                            <td style="border: solid black 1px;">Approved</td>
                            <td style="border: solid black 1px;">Site X</td>
                            <td style="border: solid black 1px;">12345678910126</td>
                            <td style="border: solid black 1px;">New Sub Meter Required</td>
                            <td style="border: solid black 1px;">All Months</td>
                            <td style="border: solid black 1px;">All Days</td>
                            <td style="border: solid black 1px;">All Periods</td>
                            <td style="border: solid black 1px;">£5,000</td>
                            <td style="border: solid black 1px;">N/A</td>
                            <td style="border: solid black 1px;">N/A</td>
                            <td style="border: solid black 1px;">N/A</td>
                            <td style="border: solid black 1px;"><input type="checkbox">&nbsp<i class="fas fa-trash-alt"></i></input</td>
                        </tr>
                    </tbody>
                </table>
                <br>
                <div style="text-align: right;">
                    <button style="background-color: green;">Submit Selected Opportunities</button>&nbsp;
                    <button style="background-color: red;">Reject Selected Opportunities</button>
                </div>
            </div>
        </div>
    </div>
    <br>
    <div class="row" style="padding-left: 30px; padding-right:30px;">
        <div class="divcolumn roundborder" style="width: 100%;">
            <div style="text-align: center;">
                <span style="border-bottom: solid black 1px;">Create New Opportunity</span>
            </div>
            <br>
            <div style="float: left;">
                <div style="float: left; width: 300px;">
                    <div class="tree-div" id="treeDiv"></div>
                    <br>
                    <div class="roundborder" style="padding: 5px;">
                        <div class="roundborder" style="width: 100%;">
                            <div id="monthOfYear" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
                            <span id="monthOfYearspan">Months</span>
                            <div id="monthOfYearList"  class="listitem-hidden" style="margin: 5px;">
                                <div class="roundborder">
                                    <input type="checkbox" checked>All Year</input><br><br>
                                    <input type="checkbox">January</input>&nbsp&nbsp&nbsp<input type="checkbox">February</input>&nbsp&nbsp&nbsp<input type="checkbox">March</input><br>
                                    <input type="checkbox">April</input>&nbsp&nbsp&nbsp<input type="checkbox">May</input>&nbsp&nbsp&nbsp<input type="checkbox">June</input><br>
                                    <input type="checkbox">July</input>&nbsp&nbsp&nbsp<input type="checkbox">August</input>&nbsp&nbsp&nbsp<input type="checkbox">September</input><br>
                                    <input type="checkbox">October</input>&nbsp&nbsp&nbsp<input type="checkbox">November</input>&nbsp&nbsp&nbsp<input type="checkbox">December</input>
                                </div>
                            </div>
                        </div>
                        <div class="roundborder" style="width: 100%;">
                            <div id="dayOfWeek" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
                            <span id="dayOfWeekspan">Days Of The Week</span>
                            <div id="dayOfWeekList"  class="listitem-hidden" style="margin: 5px;">
                                <div class="roundborder">
                                    <input type="checkbox" checked>All Week</input><br><br>
                                    <input type="checkbox">Weekdays</input>&nbsp&nbsp&nbsp<input type="checkbox">Weekends</input><br><br>
                                    <input type="checkbox">Monday</input>&nbsp&nbsp&nbsp<input type="checkbox">Tuesday</input>&nbsp&nbsp&nbsp<input type="checkbox">Wednesday</input><br>
                                    <input type="checkbox">Thursday</input>&nbsp&nbsp&nbsp<input type="checkbox">Friday</input><br>
                                    <input type="checkbox">Saturday</input>&nbsp&nbsp&nbsp<input type="checkbox">Sunday</input>
                                </div>
                            </div>
                        </div>
                        <div class="roundborder" style="width: 100%;">
                            <div id="timePeriod" class="far fa-plus-square show-pointer" style="margin-left: 5px;"></div>
                            <span id="timePeriodspan">Time Periods</span>
                            <div id="timePeriodList"  class="listitem-hidden" style="margin: 5px;">
                                <div class="roundborder">
                                    <input type="checkbox" checked>All Day</input><br><br>
                                    <input type="checkbox">Day (07:00 - 19:00)</input>&nbsp&nbsp&nbsp<input type="checkbox">Night (19:00 - 07:00)</input><br><br>
                                    <input type="checkbox">00:00 - 00:30</input>&nbsp&nbsp&nbsp<input type="checkbox">00:30 - 01:00</input>&nbsp&nbsp&nbsp<input type="checkbox">01:00 - 01:30</input>&nbsp&nbsp&nbsp<input type="checkbox">01:30 - 02:00</input><br>
                                    <input type="checkbox">02:00 - 02:30</input>&nbsp&nbsp&nbsp<input type="checkbox">02:30 - 03:00</input>&nbsp&nbsp&nbsp<input type="checkbox">03:00 - 03:30</input>&nbsp&nbsp&nbsp<input type="checkbox">03:30 - 04:00</input><br>
                                    <input type="checkbox">04:00 - 04:30</input>&nbsp&nbsp&nbsp<input type="checkbox">04:30 - 05:00</input>&nbsp&nbsp&nbsp<input type="checkbox">05:00 - 05:30</input>&nbsp&nbsp&nbsp<input type="checkbox">05:30 - 06:00</input><br>
                                    <input type="checkbox">06:00 - 06:30</input>&nbsp&nbsp&nbsp<input type="checkbox">06:30 - 07:00</input>&nbsp&nbsp&nbsp<input type="checkbox">07:00 - 07:30</input>&nbsp&nbsp&nbsp<input type="checkbox">07:30 - 08:00</input><br>
                                    <input type="checkbox">08:00 - 08:30</input>&nbsp&nbsp&nbsp<input type="checkbox">08:30 - 09:00</input>&nbsp&nbsp&nbsp<input type="checkbox">09:00 - 09:30</input>&nbsp&nbsp&nbsp<input type="checkbox">09:30 - 10:00</input><br>
                                    <input type="checkbox">10:00 - 10:30</input>&nbsp&nbsp&nbsp<input type="checkbox">10:30 - 11:00</input>&nbsp&nbsp&nbsp<input type="checkbox">11:00 - 11:30</input>&nbsp&nbsp&nbsp<input type="checkbox">11:30 - 12:00</input><br>
                                    <input type="checkbox">12:00 - 12:30</input>&nbsp&nbsp&nbsp<input type="checkbox">12:30 - 13:00</input>&nbsp&nbsp&nbsp<input type="checkbox">13:00 - 13:30</input>&nbsp&nbsp&nbsp<input type="checkbox">13:30 - 14:00</input><br>
                                    <input type="checkbox">14:00 - 14:30</input>&nbsp&nbsp&nbsp<input type="checkbox">14:30 - 15:00</input>&nbsp&nbsp&nbsp<input type="checkbox">15:00 - 15:30</input>&nbsp&nbsp&nbsp<input type="checkbox">15:30 - 16:00</input><br>
                                    <input type="checkbox">16:00 - 16:30</input>&nbsp&nbsp&nbsp<input type="checkbox">16:30 - 17:00</input>&nbsp&nbsp&nbsp<input type="checkbox">17:00 - 17:30</input>&nbsp&nbsp&nbsp<input type="checkbox">17:30 - 18:00</input><br>
                                    <input type="checkbox">18:00 - 18:30</input>&nbsp&nbsp&nbsp<input type="checkbox">18:30 - 19:00</input>&nbsp&nbsp&nbsp<input type="checkbox">19:00 - 19:30</input>&nbsp&nbsp&nbsp<input type="checkbox">19:30 - 20:00</input><br>
                                    <input type="checkbox">20:00 - 20:30</input>&nbsp&nbsp&nbsp<input type="checkbox">20:30 - 21:00</input>&nbsp&nbsp&nbsp<input type="checkbox">21:00 - 21:30</input>&nbsp&nbsp&nbsp<input type="checkbox">21:30 - 22:00</input><br>
                                    <input type="checkbox">22:00 - 22:30</input>&nbsp&nbsp&nbsp<input type="checkbox">22:30 - 23:00</input>&nbsp&nbsp&nbsp<input type="checkbox">23:00 - 23:30</input>&nbsp&nbsp&nbsp<input type="checkbox">23:30 - 00:00</input>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="middle">&nbsp</div>
            <div style="float: left;">
                <div>
                    <div class="roundborder" style="padding: 5px;">
                        <label for="opportunityType" style="width:50%;">Select Opportunity Type:</label>
                        <select id="opportunityType" style="width:48%;">
                            <option value="Custom">Custom</option>
                            <option value="DUoSReduction">DUoS Reduction</option>
                            <option value="TriadReduction">Triad Reduction</option>
                        </select>
                        <br>
                        <label for="opportunityStatus" style="width:50%;">Select Opportunity Status:</label>
                        <select id="opportunityStatus" style="width:48%;">
                            <option value="Recommend">Recommend</option>
                            <option value="Approved">Approved</option>
                        </select>
                        <br>
                        <label for="opportunityName" style="width:50%;">Enter Opportunity Name:</label>
                        <input id="opportunityName" style="width:48%;"></input>
                        <br>
                        <label for="opportunityStartDate" style="width:50%;">Select Estimated Start Date:</label>
                        <input id="opportunityStartDate" style="width:48%;"></input>
                        <br>
                        <label for="opportunityCost" style="width:50%;">Enter Estimated Cost:</label>
                        <input id="opportunityCost" style="width:48%;"></input>
                    </div>
                    <div>&nbsp</div>
                    <div class="roundborder" style="padding: 5px;">
                        <label for="opportunityNotes">Notes:</label>
                        <input id="opportunityNotes"></input>
                    </div>
                </div>
                <br>
                <div class="roundborder" style="text-align: center;">
                    <label for="applyPercentage">Percentage Saving</label>
                    <input id="applyPercentage"></input>
                    <button class="show-pointer">Apply Percentage Saving</button>
                    <br>
                    <table style="margin: 5px;">
                        <thead>
                            <tr>
                                <td style="border: solid black 1px;"><input type="checkbox" checked></input></td>
                                <td style="border: solid black 1px;">Meter</td>
                                <td style="border: solid black 1px;">Sub Meter</td>
                                <td style="border: solid black 1px;">Month</td>
                                <td style="border: solid black 1px;">Day Of The Week</td>
                                <td style="border: solid black 1px;">Time Period</td>
                                <td style="border: solid black 1px;">Percentage Saving</td>
                                <td style="border: solid black 1px;">Estimated Annual<br>kWh Savings</td>
                                <td style="border: solid black 1px;">Estimated Annual<br>£ Savings</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="border: solid black 1px;"><input type="checkbox" checked></input></td>
                                <td style="border: solid black 1px;">1234567890123</td>
                                <td style="border: solid black 1px;">Sub Meter 1</td>
                                <td style="border: solid black 1px;">All Months</td>
                                <td style="border: solid black 1px;">All Days</td>
                                <td style="border: solid black 1px;">All Periods</td>
                                <td style="border: solid black 1px;"><input style="width: 100%;"></input></td>
                                <td style="border: solid black 1px;">5,000</td>
                                <td style="border: solid black 1px;">£10,000</td>
                            </tr>
                            <tr>
                                <td style="border: solid black 1px;"><input type="checkbox" checked></input></td>
                                <td style="border: solid black 1px;">1234567890124</td>
                                <td style="border: solid black 1px;">New Sub Meter Required</td>
                                <td style="border: solid black 1px;">All Months</td>
                                <td style="border: solid black 1px;">All Days</td>
                                <td style="border: solid black 1px;">All Periods</td>
                                <td style="border: solid black 1px;"><input style="width: 100%;"></input></td>
                                <td style="border: solid black 1px;">N/A</td>
                                <td style="border: solid black 1px;">N/A</td>
                            </tr>
                        </tbody>
                    </table>
                    <span style="border-bottom: solid black 1px;">Estimated Savings<br></span>
                    <br>
                    <div>
                        <span>Number of new sub meters required: 1</span><br>
                        <span>Estimated Annual kWh Savings on Existing Sub Meters: 10,000</span><br>
                        <span>Estimated Annual £ Savings on Existing Sub Meters: £1,000</span>
                    </div>
                </div>
            </div>
        </div>
        <br>
        <div style="float: right;">
            <button class="show-pointer">Add Opportunity</button>
            &nbsp&nbsp&nbsp&nbsp&nbsp
            <button class="show-pointer">Reset Opportunity</button>
        </div>
    </div>
    <br>
</body>

<script type="text/javascript" src="createopportunities.js"></script>
<script type="text/javascript" src="createopportunities.json"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>