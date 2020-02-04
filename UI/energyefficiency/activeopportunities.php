<?php 
	$PAGE_TITLE = "Active Opportunities";
	include($_SERVER['DOCUMENT_ROOT']."/includes/header.php");
	include($_SERVER['DOCUMENT_ROOT']."/includes/navigation.php");
?>

<body>
	<div class="section-header section-header-text"><?php echo $PAGE_TITLE ?></div>

	<div class="chart-wrapper">
		<ul class="chart-values">
			<li>sun</li>
			<li>mon</li>
			<li>tue</li>
			<li>wed</li>
			<li>thu</li>
			<li>fri</li>
			<li>sat</li>
		</ul>
		<ul class="chart-bars">
			<li data-duration="tue½-wed" data-color="#b03532">Task</li>
			<li data-duration="wed-sat" data-color="#33a8a5">Task</li>
			<li data-duration="sun-tue" data-color="#30997a">Task</li>
			<li data-duration="tue½-thu" data-color="#6a478f">Task</li>
			<li data-duration="mon-tue½" data-color="#da6f2b">Task</li>
			<li data-duration="wed-wed" data-color="#3d8bb1">Task</li>
			<li data-duration="thu-fri½" data-color="#e03f3f">Task</li>
			<li data-duration="mon½-wed½" data-color="#59a627">Task</li>
			<li data-duration="fri-sat" data-color="#4464a1">Task</li>
		</ul>
	</div>
</body>

<style>
	/* RESET RULES
–––––––––––––––––––––––––––––––––––––––––––––––––– */
:root {
  --white: #fff;
  --divider: lightgrey;
  --body: #f5f7f8;
}

* {
  padding: 0;
  margin: 0;
  box-sizing: border-box;
}

ul {
  list-style: none;
}

a {
  text-decoration: none;
  color: inherit;
}

.chart-wrapper {
  padding: 0 10px;
  margin: 0 auto;
}


/* CHART-VALUES
–––––––––––––––––––––––––––––––––––––––––––––––––– */
.chart-wrapper .chart-values {
  position: relative;
  display: flex;
  margin-bottom: 20px;
  font-weight: bold;
  font-size: 1.2rem;
}

.chart-wrapper .chart-values li {
  flex: 1;
  min-width: 80px;
  text-align: center;
}

.chart-wrapper .chart-values li:not(:last-child) {
  position: relative;
}

.chart-wrapper .chart-values li:not(:last-child)::before {
  content: '';
  position: absolute;
  right: 0;
  height: 510px;
  border-right: 1px solid var(--divider);
}


/* CHART-BARS
–––––––––––––––––––––––––––––––––––––––––––––––––– */
.chart-wrapper .chart-bars li {
  position: relative;
  color: var(--white);
  margin-bottom: 15px;
  font-size: 16px;
  border-radius: 20px;
  padding: 10px 20px;
  width: 0;
  opacity: 0;
  transition: all 0.65s linear 0.2s;
}

@media screen and (max-width: 600px) {
  .chart-wrapper .chart-bars li {
    padding: 10px;
  }
}
</style>

<script>
	function createChart(e) {
  const days = document.querySelectorAll(".chart-values li");
  const tasks = document.querySelectorAll(".chart-bars li");
  const daysArray = [...days];

  tasks.forEach(el => {
    const duration = el.dataset.duration.split("-");
    const startDay = duration[0];
    const endDay = duration[1];
    let left = 0,
      width = 0;

    if (startDay.endsWith("½")) {
      const filteredArray = daysArray.filter(day => day.textContent == startDay.slice(0, -1));
      left = filteredArray[0].offsetLeft + filteredArray[0].offsetWidth / 2;
    } else {
      const filteredArray = daysArray.filter(day => day.textContent == startDay);
      left = filteredArray[0].offsetLeft;
    }

    if (endDay.endsWith("½")) {
      const filteredArray = daysArray.filter(day => day.textContent == endDay.slice(0, -1));
      width = filteredArray[0].offsetLeft + filteredArray[0].offsetWidth / 2 - left;
    } else {
      const filteredArray = daysArray.filter(day => day.textContent == endDay);
      width = filteredArray[0].offsetLeft + filteredArray[0].offsetWidth - left;
    }

    // apply css
    el.style.left = `${left}px`;
    el.style.width = `${width}px`;
    if (e.type == "load") {
      el.style.backgroundColor = el.dataset.color;
      el.style.opacity = 1;
    }
  });
}

window.addEventListener("load", createChart);
window.addEventListener("resize", createChart);
</script>

<?php include($_SERVER['DOCUMENT_ROOT']."/includes/footer.php");?>