<HTML>

<!--------------------------------------------------------------->
<!-- Copyright (c) 2006 by Conor O'Mahony.                     -->
<!-- For enquiries, please email GubuSoft@GubuSoft.com.        -->
<!-- Please keep all copyright notices below.                  -->
<!-- Original author of TreeView script is Marcelino Martins.  -->
<!--------------------------------------------------------------->
<!-- This document includes the TreeView script.  The TreeView -->
<!-- script can be found at http://www.TreeView.net.  The      -->
<!-- script is Copyright (c) 2006 by Conor O'Mahony.           -->
<!--------------------------------------------------------------->

 <HEAD>
  <!-- Code for browser detection. DO NOT REMOVE.             -->
  <SCRIPT src="/javascript/ua.js"></SCRIPT>

  <!-- Infrastructure code for the TreeView. DO NOT REMOVE.   -->
  <SCRIPT src="/javascript/ftiens4.js"></SCRIPT>

  <!-- Scripts that define the tree. DO NOT REMOVE.           -->
  <SCRIPT SRC="/javascript/demoCheckboxNodes.js"></SCRIPT>

  <SCRIPT SRC="/javascript/utils.js"></SCRIPT>
 </HEAD>

 <DIV style="position:absolute; top:0; left:0;"><TABLE><TR><TD><A style="font-size:7pt;text-decoration:none;color:silver" href="http://www.treemenu.net/" target=_blank>Javascript Tree Menu</A></TD></TR></TABLE></DIV>
 <div class="row">
  <div class="column">
      <SCRIPT>initializeDocument()</SCRIPT>
  </div>
  <div class="finalcolumn" id="rightFrame">
	<div><div class="chartjs-size-monitor"><div class="chartjs-size-monitor-expand"><div class=""></div></div><div class="chartjs-size-monitor-shrink"><div class=""></div></div></div>
		<canvas id="canvas" class="chartjs-render-monitor"></canvas>
	</div>
	<br>
	<br>

	<button id="addData">Add Data</button>
	<button id="removeData">Remove Data</button>
	<script>        
		var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
		var config = {
			type: 'line',
			data: {
				labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July']
			},
			options: {
				responsive: true,
				title: {
					display: true,
					text: 'Electricity Consumption Summary'
				},
				tooltips: {
					mode: 'index',
					intersect: false,
				},
				hover: {
					mode: 'nearest',
					intersect: true
				},
				scales: {
					xAxes: [{
						display: true,
						scaleLabel: {
							display: true,
							labelString: 'Month'
						}
					}],
					yAxes: [{
						display: true,
						scaleLabel: {
							display: true,
							labelString: 'Value'
						}
					}]
				}
			}
		};

		window.onload = function() {
			var ctx = document.getElementById('canvas').getContext('2d');
			window.myLine = new Chart(ctx, config);
		};

        var colorNames = Object.keys(window.chartColors);
        var inputs = document.getElementsByTagName('input');

        for(var i = 0; i < inputs.length; i++) {
            if(inputs[i].type.toLowerCase() == 'checkbox') {
                inputs[i].addEventListener('click', function() {
                    var outerHTML = this.outerHTML.split("desc=");
                    var cleanText = outerHTML[1].replace(/["']/g, "").replace("=", "").replace(">", "");

                    if(this.checked) {
                        var colorName = colorNames[this.id % colorNames.length];
                        var newColor = window.chartColors[colorName];
                        var newDataset = {
                            label: cleanText,
                            backgroundColor: newColor,
                            borderColor: newColor,
                            data: [],
                            fill: false
                        } 

                        for (var index = 0; index < config.data.labels.length; ++index) {
                            newDataset.data.push(randomScalingFactor());
                        }

                        config.data.datasets.push(newDataset);
                    }
                    else {
                        for(var j = 0; j < config.data.datasets.length; j++) {
                            if(config.data.datasets[j].label == cleanText) {
                                config.data.datasets.splice(j, 1);
                                j = config.data.datasets.length;
                            }
                        }
                    }; 
                    
                    window.myLine.update();
		        });              
            }
        }

		document.getElementById('addData').addEventListener('click', function() {
			if (config.data.datasets.length > 0) {
				var month = MONTHS[config.data.labels.length % MONTHS.length];
				config.data.labels.push(month);

				config.data.datasets.forEach(function(dataset) {
					dataset.data.push(randomScalingFactor());
				});

				window.myLine.update();
			}
		});

		document.getElementById('removeData').addEventListener('click', function() {
			config.data.labels.splice(-1, 1); // remove the label first

			config.data.datasets.forEach(function(dataset) {
				dataset.data.pop();
			});

			window.myLine.update();
		});
	</script>
  </div>
</div>

</HTML>