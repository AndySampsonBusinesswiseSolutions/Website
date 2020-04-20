// Code goes here
(() => {
  angular
  .module('dateRangeDemo', ['ui.bootstrap', 'rzModule'])
  .controller('dateRangeCtrl', function dateRangeCtrl($scope) {
    // Single Date Slider    
    var dates = [];
    for (var i = 1; i <= 5; i++) {
      dates.push(new Date(2000, 0, i));
    }
    
    var dateToTimePeriod = function (date) {
      var day = date.getDate();
      switch(day) {
        case 1:
          return 'Half Hourly';
        case 2:
          return 'Daily';
        case 3:
          return 'Monthly';
        case 4:
          return 'Quarterly';
        case 5:
          return 'Yearly';
        default:
          return '';
      }
    }
    
    // Date Range Slider
    var floorDate = new Date(2019, 10, 1).getTime();
    var ceilDate = new Date(2020, 0, 4).getTime();
    var minDate = new Date(floorDate).getTime();
    var maxDate = new Date(ceilDate).getTime();
    var millisInDay = 24*60*60*1000;
    
    var monthNames =
    [
      "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"
    ];

    var formatDate = function (date_millis)
    {
      var date = new Date(date_millis);
      return date.getDate()+"-"+monthNames[date.getMonth()]+"-"+date.getFullYear();
    }

    //Configs
    $scope.usageChartOptionsTimeSpan = {
      value: dates[1],
      options: {
        id: 'usageChartOptionsTimeSpan',
        stepsArray: dates,
        translate: function(date) {
          if (date !== null)
            return dateToTimePeriod(date);
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.totalCostChartOptionsTimeSpan = {
      value: dates[1],
      options: {
        stepsArray: dates,
        translate: function(date) {
          if (date !== null)
            return dateToTimePeriod(date);
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.costBreakdownChartOptionsTimeSpan = {
      value: dates[1],
      options: {
        stepsArray: dates,
        translate: function(date) {
          if (date !== null)
            return dateToTimePeriod(date);
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.capacityChartOptionsTimeSpan = {
      value: dates[1],
      options: {
        stepsArray: dates,
        translate: function(date) {
          if (date !== null)
            return dateToTimePeriod(date);
          return '';
        },
        onEnd: function() {
          updateCharts();
        }
      }
    };

    $scope.usageChartOptionsDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        id: 'usageChartOptionsDateRange',
        floor: floorDate,
        ceil: ceilDate,
        step: millisInDay,
        showTicks: false,
        draggableRange: true,
        translate: function(date_millis) {
          if ((date_millis !== null)) {
            var dateFromMillis = new Date(date_millis);
            return formatDate(dateFromMillis);
          }
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.totalCostChartOptionsDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        floor: floorDate,
        ceil: ceilDate,
        step: millisInDay,
        showTicks: false,
        draggableRange: true,
        translate: function(date_millis) {
          if ((date_millis !== null)) {
            var dateFromMillis = new Date(date_millis);
            return formatDate(dateFromMillis);
          }
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.costBreakdownChartOptionsDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        floor: floorDate,
        ceil: ceilDate,
        step: millisInDay,
        showTicks: false,
        draggableRange: true,
        translate: function(date_millis) {
          if ((date_millis !== null)) {
            var dateFromMillis = new Date(date_millis);
            return formatDate(dateFromMillis);
          }
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };

    $scope.capacityChartOptionsDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        floor: floorDate,
        ceil: ceilDate,
        step: millisInDay,
        showTicks: false,
        draggableRange: true,
        translate: function(date_millis) {
          if ((date_millis !== null)) {
            var dateFromMillis = new Date(date_millis);
            return formatDate(dateFromMillis);
          }
          return '';
        },
        onEnd: function() {
          updateCharts();
        } 
      }
    };
    
  });
})();
