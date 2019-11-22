<div>
	<br>
	<br>
	<div>
		<div class="group-div">
			<div class="group-by-div">
				<div style="width: 30%; display: inline-block;">
					<span class="fas fa-chart-line" style="padding-right: 5px"></span>
					<span class="chart-header">Electricity Time View</span>
				</div>
				<div style="display: inline-block; float: right;">
					<span style="padding-right: 5px;" class="simple-divider"></span>
					<span>Show By:</span>
					<span class="qwerty-select" style="display: inline-block;">
							<select>
								<option value="0">Energy</option>
								<option value="1">Power</option>
								<option value="2">Current</option>
								<option value="3">Cost</option>
							</select>
						<!-- <span class="arrow-header" title="Show By" style="padding-left: 10px;" id="electricityChartHeaderShowBy">Energy</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderShowByArrow"></span>				 -->
					</span>
					<span style="padding-right: 5px;" class="simple-divider"></span>
					<span>Period:</span>
					<span class="show-pointer">
						<span class="arrow-header" title="Period" style="padding-left: 10px;" id="electricityChartHeaderPeriod">Daily</span>
						<span class="fa fa-angle-double-down" style="padding-left: 10px;" id="electricityChartHeaderPeriodArrow"></span>				
					</span>
					<span class="simple-divider"></span>
					<span title="Previous Period" class="fas fa-caret-left show-pointer"></span>
					<span title="Current Period" class="show-pointer">Wed, 20 Nov 2019</span>
					<span title="Next Period" class="fas fa-caret-right show-pointer"></span>
					<span></span>
					<span title="Calendar" class="fas fa-calendar-alt show-pointer"></span>
					<span class="simple-divider"></span>
					<span title="Chart Type" class="fas fa-chart-bar show-pointer"></span>
					<span class="simple-divider"></span>
					<span title="Layers" class="fas fa-layer-group show-pointer"></span>
					<span class="simple-divider"></span>
					<span title="Download" class="fas fa-download show-pointer"></span>
					<span class="simple-divider"></span>
					<span title="Refresh" class="fas fa-sync show-pointer"></span>
				</div>
			</div>
		</div>
	</div>
	<div class="tree-div">
	</div>
</div>


<script>
var x, i, j, selElmnt, a, b, c;
/*look for any elements with the class "qwerty-select":*/
x = document.getElementsByClassName("qwerty-select");
for (i = 0; i < x.length; i++) {
  selElmnt = x[i].getElementsByTagName("select")[0];
  /*for each element, create a new DIV that will act as the selected item:*/
  a = document.createElement("DIV");
  a.setAttribute("class", "select-selected");
  a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
  x[i].appendChild(a);
  /*for each element, create a new DIV that will contain the option list:*/
  b = document.createElement("DIV");
  b.setAttribute("class", "select-items select-hide");
  for (j = 1; j < selElmnt.length; j++) {
    /*for each option in the original select element,
    create a new DIV that will act as an option item:*/
    c = document.createElement("DIV");
    c.innerHTML = selElmnt.options[j].innerHTML;
    c.addEventListener("click", function(e) {
        /*when an item is clicked, update the original select box,
        and the selected item:*/
        var y, i, k, s, h;
        s = this.parentNode.parentNode.getElementsByTagName("select")[0];
        h = this.parentNode.previousSibling;
        for (i = 0; i < s.length; i++) {
          if (s.options[i].innerHTML == this.innerHTML) {
            s.selectedIndex = i;
            h.innerHTML = this.innerHTML;
            y = this.parentNode.getElementsByClassName("same-as-selected");
            for (k = 0; k < y.length; k++) {
              y[k].removeAttribute("class");
            }
            this.setAttribute("class", "same-as-selected");
            break;
          }
        }
        h.click();
    });
    b.appendChild(c);
  }
  x[i].appendChild(b);
  a.addEventListener("click", function(e) {
      /*when the select box is clicked, close any other select boxes,
      and open/close the current select box:*/
      e.stopPropagation();
      closeAllSelect(this);
      this.nextSibling.classList.toggle("select-hide");
      this.classList.toggle("select-arrow-active");
    });
}
function closeAllSelect(elmnt) {
  /*a function that will close all select boxes in the document,
  except the current select box:*/
  var x, y, i, arrNo = [];
  x = document.getElementsByClassName("select-items");
  y = document.getElementsByClassName("select-selected");
  for (i = 0; i < y.length; i++) {
    if (elmnt == y[i]) {
      arrNo.push(i)
    } else {
      y[i].classList.remove("select-arrow-active");
    }
  }
  for (i = 0; i < x.length; i++) {
    if (arrNo.indexOf(i)) {
      x[i].classList.add("select-hide");
    }
  }
}
/*if the user clicks anywhere outside the select box,
then close all select boxes:*/
document.addEventListener("click", closeAllSelect);
</script>