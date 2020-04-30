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
    <div id="popup" class="popup">
		<div class="modal-content">
			<span class="close" title="Close">&times;</span>
			<div class="title">
				<span id="title"></span>
			</div>
			<br>
			<span id="popupText"></span><br>
		</div>
	</div>
    <div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>
    <br>
    <div class="final-column">
        <div class="roundborder divcolumn">
            <div class="expander-header">
                <span>Identified Opportunities</span>
                <div id="identifiedOpportunities" class="far fa-plus-square show-pointer expander openExpander"></div>
            </div>
            <div id="identifiedOpportunitiesList">
                <div style="text-align: right;">
                    <button class="approve show-pointer">Submit Selected Opportunities</button>&nbsp;
                    <button class="reject show-pointer">Reject Selected Opportunities</button>
                </div>
                <br>
                <div id="identifiedOpportunitiesSpreadsheet"></div>
            </div>
        </div>
        <div class="roundborder divcolumn expander-container">
            <div class="expander-header">
                <span>Create New Opportunity</span>
                <div id="createNewOpportunity" class="far fa-plus-square show-pointer expander openExpander"></div>
            </div>
            <div id="createNewOpportunityList">
            <div>
                <div class="expander-container">
                    <div style="float: left;">
                        <div class="tree-div roundborder" style="padding: 5px; text-align: right;">
                            <label for="opportunityType">Select Opportunity Type:</label>
                            <select id="opportunityType" style="width:46%;">
                                <option value="Custom">Custom</option>
                                <option value="DUoSReduction">DUoS Reduction</option>
                                <option value="TriadReduction">Triad Reduction</option>
                            </select>
                            <br>
                            <label for="opportunityStatus">Select Opportunity Status:</label>
                            <select id="opportunityStatus" style="width:46%;">
                                <option value="Recommend">Recommend</option>
                                <option value="Approved">Approved</option>
                            </select>
                            <br>
                            <label for="opportunityName">Enter Opportunity Name:</label>
                            <input id="opportunityName" style="width:46%;"></input>
                            <br>
                            <label for="opportunityStartDate">Enter Estimated Start Date:</label>
                            <input id="opportunityStartDate" type="date" style="width:46%;"></input>
                            <br>
                            <label for="opportunityCAPEXCost">Enter Estimated CAPEX Cost:</label>
                            <input id="opportunityCAPEXCost" style="width:46%;"></input>
                            <br>
                            <label for="opportunityOPEXCost">Enter Estimated OPEX Cost:</label>
                            <input id="opportunityOPEXCost" style="width:46%;"></input><br>
                            <input type="checkbox" class="show-pointer">Include BWS Fee In OPEX Cost?</input>
                        </div>
                    </div>
                    <div class="middle"></div>
                    <div style="float: left;">
                        <div id="siteTree" class="tree-div roundborder scrolling-wrapper" style="padding: 5px;">
                        </div>
                    </div>
                    <div class="middle"></div>
                    <div style="float: left;">
                        <div class="tree-div roundborder scrolling-wrapper" style="padding: 5px;">
                            <span id="timePeriodsDiv">Select Time Periods</span>
                            <ul class="format-listitem toplistitem" id="timePeriodsDivSelectorList">
                                <li>
                                    <div id="monthOfYear" class="far fa-plus-square show-pointer expander"></div>
                                    <span id="monthOfYearspan">Months</span>
                                    <div id="monthOfYearList"  class="listitem-hidden">
                                        <ul class="format-listitem">
                                            <li>
                                                <div class="far fa-times-circle"></div>
                                                <input type="checkbox" class="show-pointer" checked><span>All Year</span>
                                            </li>
                                            <li>
                                                <div id="monthOfYearDetail" class="far fa-plus-square show-pointer expander"></div>
                                                <span id="monthOfYearDetailspan">Select Specific Month(s)</span>
                                                <div id="monthOfYearDetailList"  class="listitem-hidden">
                                                    <ul class="format-listitem">
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>January</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>February</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>March</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>April</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>May</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>June</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>July</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>August</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>September</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>October</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>November</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>December</span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li>
                                    <div id="dayOfWeek" class="far fa-plus-square show-pointer expander"></div>
                                    <span id="dayOfWeekspan">Days Of The Week</span>
                                    <div id="dayOfWeekList"  class="listitem-hidden">
                                        <ul class="format-listitem">
                                            <li>
                                                <div class="far fa-times-circle"></div>
                                                <input type="checkbox" class="show-pointer" checked><span>All Week</span>
                                            </li>
                                            <li>
                                                <div id="dayOfWeekDetail" class="far fa-plus-square show-pointer expander"></div>
                                                <span id="dayOfWeekDetailspan">Select Specific Day(s)</span>
                                                <div id="dayOfWeekDetailList"  class="listitem-hidden">
                                                    <ul class="format-listitem">
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Monday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Tuesday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Wednesday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Thursday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Friday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Saturday</span>
                                                        </li>
                                                        <li>
                                                            <div style="padding-right: 4px;" class="far fa-times-circle"></div>
                                                            <input type="checkbox" class="show-pointer"><span>Sunday</span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li>
                                    <div id="timePeriod" class="far fa-plus-square show-pointer expander"></div>
                                    <span id="timePeriodspan">Time Periods</span>
                                    <div id="timePeriodList"  class="listitem-hidden">
                                        <ul class="format-listitem">
                                            <li>
                                                <div class="far fa-times-circle"></div>
                                                <input type="checkbox" class="show-pointer" checked><span>All Day</span>
                                            </li>
                                            <li>
                                                <div id="timePeriodDetail" class="far fa-plus-square show-pointer expander"></div>
                                                <span id="timePeriodDetailspan">Select Specific Time Period(s)</span>
                                                <div id="timePeriodDetailList"  class="listitem-hidden">
                                                    <ul class="format-listitem">
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>Day (07:00 - 19:00)</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>Night (19:00 - 07:00)</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>00:00 - 00:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>00:00 - 00:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>00:30 - 01:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>01:00 - 01:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>01:30 - 02:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>02:00 - 02:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>02:30 - 03:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>03:00 - 03:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>03:30 - 04:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>04:00 - 04:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>04:30 - 05:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>05:00 - 05:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>05:30 - 06:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>06:00 - 06:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>06:30 - 07:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>07:00 - 07:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>07:30 - 08:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>08:00 - 08:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>08:30 - 09:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>09:00 - 09:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>09:30 - 10:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>10:00 - 10:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>10:30 - 11:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>11:00 - 11:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>11:30 - 12:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>12:00 - 12:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>12:30 - 13:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>13:00 - 13:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>13:30 - 14:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>14:00 - 14:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>14:30 - 15:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>15:00 - 15:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>15:30 - 16:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>16:00 - 16:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>16:30 - 17:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>17:00 - 17:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>17:30 - 18:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>18:00 - 18:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>18:30 - 19:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>19:00 - 19:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>19:30 - 20:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>20:00 - 20:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>20:30 - 21:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>21:00 - 21:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>21:30 - 22:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>22:00 - 22:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>22:30 - 23:00</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>23:00 - 23:30</span></li>
                                                        <li><div style="padding-right: 4px;" class="far fa-times-circle"></div><input type="checkbox" class="show-pointer"><span>23:30 - 00:00</span></li>
                                                    </ul>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="middle"></div>
                    <div style="float: left;">
                        <div class="tree-div roundborder" style="padding: 5px; width: 918px;">
                            <label for="opportunityNotes">Notes:</label>
                            <textarea id="opportunityNotes" style="height: 15%; resize: none;"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both;"></div>
            <div class="expander-container">
                <div class="roundborder" style="text-align: center; float: left; width: 1375px;">
                    <div style="padding: 5px;">
                        <label for="applyPercentage">Percentage Saving</label>
                        <input id="applyPercentage"></input>
                        <button class="show-pointer">Apply Percentage Saving</button>
                    </div>
                    <div id="createNewOpportunitySpreadsheet"></div>
                </div>
                <div class="middle"></div>
                <div class="roundborder" style="text-align: center; float: left;">
                    <span style="border-bottom: solid black 1px;">Estimated Savings<br></span>
                    <br>
                    <div style="padding: 5px;">
                        <span>Number of new sub meters required: 1</span><br>
                        <span>Estimated Annual kWh Savings on Existing Devices: 10,000</span><br>
                        <span>Estimated Annual £ Savings on Existing Devices: £1,000</span>
                    </div>
                </div>
                <div class="middle"></div>
                <div style="float: right;">
                    <br><br>
                    <button class="show-pointer approve" style="width: 100%;">Add Opportunity</button>
                    <button class="show-pointer reject" style="width: 100%;">Reset Opportunity</button>
                </div>
            </div>
            <div style="clear: both;"></div>
            </div>
        </div>
    </div>
    <br>
</body>

<script src="/includes/base.js"></script>

<script type="text/javascript" src="createopportunities.js"></script>
<script type="text/javascript" src="createopportunities.json"></script>
<script type="text/javascript" src="https://bossanova.uk/jexcel/v3/jexcel.js"></script>
<script type="text/javascript" src="https://bossanova.uk/jsuites/v2/jsuites.js"></script>

<script type="text/javascript"> 
	pageLoad();
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer/footer.php");?>