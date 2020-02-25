<?php 
	$PAGE_TITLE = "Site Visit";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<link rel="stylesheet" href="/css/tree.css">

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<body>
    <div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
    <br>
    <div class="row" style="padding-left: 15px; padding-right: 15px;">
        <div class="divcolumn first"></div>
        <div style="border: solid black 1px; width: 96%;">
            <div style="text-align: center;">
                <span style="border-bottom: solid black 1px;">Identified Opportunities</span>
            </div>
        </div>
        <div class="divcolumn last"></div>
    </div>
    <br>
    <div class="row" style="padding-left: 15px; padding-right: 15px;">
        <div class="divcolumn first"></div>
        <div style="border: solid black 1px; width: 96%;">
            <div style="text-align: center;">
                <span style="border-bottom: solid black 1px;">Create New Opportunity</span>
            </div>
            <br>
            <div class="divcolumn first"></div>
            <div class="divcolumn left" style="border: solid black 1px;">
                <div class="first">&nbsp</div>
                <div class="left">
                    <div style="border: solid black 1px; padding: 5px;">
                        <label for="opportunityType" style="width:45%;">Select Opportunity Type:</label>
                        <select id="opportunityType" style="width:52%;">
                            <option value="Custom">Custom</option>
                            <option value="DUoSReduction">DUoS Reduction</option>
                            <option value="TriadReduction">Triad Reduction</option>
                        </select>
                        <br>
                        <label for="opportunityName" style="width:45%;">Enter Opportunity Name:</label>
                        <input id="opportunityName" style="width:52%;"></input>
                    </div>
                    <br>
                    <div style="border: solid black 1px; padding: 5px;">
                        <div class="panel-group" id="accordion">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1" class="expandicon">Months</a>
                                    </h4>
                                </div>
                                <div id="collapse1" class="panel-collapse collapse">
                                    <div class="panel-body">
                                        <input type="checkbox" checked>All Year</input><br><br>
                                        <input type="checkbox">January</input><br>
                                        <input type="checkbox">February</input><br>
                                        <input type="checkbox">March</input><br>
                                        <input type="checkbox">April</input><br>
                                        <input type="checkbox">May</input><br>
                                        <input type="checkbox">June</input><br>
                                        <input type="checkbox">July</input><br>
                                        <input type="checkbox">August</input><br>
                                        <input type="checkbox">September</input><br>
                                        <input type="checkbox">October</input><br>
                                        <input type="checkbox">November</input><br>
                                        <input type="checkbox">December</input><br>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Days Of The Week</a>
                                    </h4>
                                </div>
                                <div id="collapse2" class="panel-collapse collapse">
                                    <div class="panel-body">
                                        <input type="checkbox" checked>All Week</input><br><br>
                                        <input type="checkbox">Weekdays</input><br>
                                        <input type="checkbox">Weekends</input><br><br>
                                        <input type="checkbox">Monday</input><br>
                                        <input type="checkbox">Tuesday</input><br>
                                        <input type="checkbox">Wednesday</input><br>
                                        <input type="checkbox">Thursday</input><br>
                                        <input type="checkbox">Friday</input><br>
                                        <input type="checkbox">Saturday</input><br>
                                        <input type="checkbox">Sunday</input><br>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Time Periods</a>
                                    </h4>
                                </div>
                                <div id="collapse3" class="panel-collapse collapse">
                                    <div class="panel-body">
                                        <input type="checkbox" checked>All Day</input><br><br>
                                        <input type="checkbox">Day (07:00 - 19:00)</input><br>
                                        <input type="checkbox">Night (19:00 - 07:00)</input><br><br>
                                        <input type="checkbox">00:00 - 00:30</input><input type="checkbox">00:30 - 01:00</input><input type="checkbox">01:00 - 01:30</input><input type="checkbox">01:30 - 02:00</input>
                                        <input type="checkbox">02:00 - 02:30</input><input type="checkbox">02:30 - 03:00</input><input type="checkbox">03:00 - 03:30</input><input type="checkbox">03:30 - 04:00</input>
                                        <input type="checkbox">04:00 - 04:30</input><input type="checkbox">04:30 - 05:00</input><input type="checkbox">05:00 - 05:30</input><input type="checkbox">05:30 - 06:00</input>
                                        <input type="checkbox">06:00 - 06:30</input><input type="checkbox">06:30 - 07:00</input><input type="checkbox">07:00 - 07:30</input><input type="checkbox">07:30 - 08:00</input>
                                        <input type="checkbox">08:00 - 08:30</input><input type="checkbox">08:30 - 09:00</input><input type="checkbox">09:00 - 09:30</input><input type="checkbox">09:30 - 10:00</input>
                                        <input type="checkbox">10:00 - 10:30</input><input type="checkbox">10:30 - 11:00</input><input type="checkbox">11:00 - 11:30</input><input type="checkbox">11:30 - 12:00</input>
                                        <input type="checkbox">12:00 - 12:30</input><input type="checkbox">12:30 - 13:00</input><input type="checkbox">13:00 - 13:30</input><input type="checkbox">13:30 - 14:00</input>
                                        <input type="checkbox">14:00 - 14:30</input><input type="checkbox">14:30 - 15:00</input><input type="checkbox">15:00 - 15:30</input><input type="checkbox">15:30 - 16:00</input>
                                        <input type="checkbox">16:00 - 16:30</input><input type="checkbox">16:30 - 17:00</input><input type="checkbox">17:00 - 17:30</input><input type="checkbox">17:30 - 18:00</input>
                                        <input type="checkbox">18:00 - 18:30</input><input type="checkbox">18:30 - 19:00</input><input type="checkbox">19:00 - 19:30</input><input type="checkbox">19:30 - 20:00</input>
                                        <input type="checkbox">20:00 - 20:30</input><input type="checkbox">20:30 - 21:00</input><input type="checkbox">21:00 - 21:30</input><input type="checkbox">21:30 - 22:00</input>
                                        <input type="checkbox">22:00 - 22:30</input><input type="checkbox">22:30 - 23:00</input><input type="checkbox">23:00 - 23:30</input><input type="checkbox">23:30 - 00:00</input>
                                    </div>
                                </div>
                            </div>
                        </div> 
                    </div>
                </div>
                <div class="middle">&nbsp</div>
                <div class="right tree-div"></div>
            </div>
            <div class="divcolumn middle"></div>
            <div class="divcolumn right" style="border: solid black 1px;">
                <div style="border: solid black 1px; text-align: center">
                    <span style="border-bottom: solid black 1px;">Asset Percentages</span>
                    <br>
                    <table style="width: 100%;">
                        <thead>
                            <tr>
                                <td style="border: solid black 1px;">Meter</td>
                                <td style="border: solid black 1px;">Sub Meter</td>
                                <td style="border: solid black 1px;">Month</td>
                                <td style="border: solid black 1px;">Day Of The Week</td>
                                <td style="border: solid black 1px;">Time Period</td>
                                <td style="border: solid black 1px;">Percentage</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="border: solid black 1px;">1234567890123</td>
                                <td style="border: solid black 1px;">Sub Meter 1</td>
                                <td style="border: solid black 1px;">All Months</td>
                                <td style="border: solid black 1px;">All Days</td>
                                <td style="border: solid black 1px;">All Periods</td>
                                <td style="border: solid black 1px;"><input style="width: 100%;"></input></td>
                            </tr>
                            <tr>
                                <td style="border: solid black 1px;">1234567890124</td>
                                <td style="border: solid black 1px;">New Sub Meter Required</td>
                                <td style="border: solid black 1px;">All Months</td>
                                <td style="border: solid black 1px;">All Days</td>
                                <td style="border: solid black 1px;">All Periods</td>
                                <td style="border: solid black 1px;"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div style="border: solid black 1px; text-align: center">
                    <div>
                        <span style="border-bottom: solid black 1px;">Estimated Savings<br></span>
                        <br>
                        <div>
                            <div style="border: solid black 1px; width: 50%; float: left; height: 100px;">
                                <br>
                                <span>Number of new sub meters required: 1</span><br>
                                <span>Estimated Annual kWh Savings on Existing Sub Meters: 10,000</span><br>
                                <span>Estimated £ kWh Savings on Existing Sub Meters: £1,000</span><br>
                                &nbsp
                            </div>
                            <div style="border: solid black 1px; width: 50%; float: left; height: 100px;">
                                <br><br>
                                <button class="show-pointer">Add Opportunity</button>
                                &nbsp&nbsp&nbsp&nbsp&nbsp
                                <button class="show-pointer">Reset Opportunity</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divcolumn last"></div>
        </div>
        <div class="divcolumn last"></div>
    </div>
    <br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>