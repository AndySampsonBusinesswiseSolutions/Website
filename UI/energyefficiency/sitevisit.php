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
                                    <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipisicing elit,
                                    sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
                                    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Days Of The Week</a>
                                    </h4>
                                </div>
                                <div id="collapse2" class="panel-collapse collapse">
                                    <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipisicing elit,
                                    sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
                                    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Time Periods</a>
                                    </h4>
                                </div>
                                <div id="collapse3" class="panel-collapse collapse">
                                    <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipisicing elit,
                                    sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
                                    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</div>
                                </div>
                            </div>
                        </div> 
                    </div>
                </div>
                <div class="middle">&nbsp</div>
                <div class="right tree-div"></div>
            </div>
            <div class="divcolumn middle"></div>
            <div class="divcolumn right" style="border: solid black 1px;"></div>
            <div class="divcolumn last"></div>
            <br>
            &nbsp
            <br>
            &nbsp
        </div>
        <div class="divcolumn last"></div>
    </div>
    <br>
</body>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>