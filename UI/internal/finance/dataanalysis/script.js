// Code goes here
(() => {
  angular
  .module('dateRangeDemo', ['ui.bootstrap', 'rzModule'])
  .controller('dateRangeCtrl', function dateRangeCtrl($scope) {
    // Single Date Slider    
    var timeSpans = [];
    for (var i = 1; i <= 5; i++) {
      timeSpans.push(new Date(2000, 0, i));
    }

    var dates = [];
    var today = new Date();
    for(var i = -60; i <= 0; i++) {
      dates.push(new Date(today.getFullYear(), today.getMonth(), today.getDate() + i));
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
    $scope.timePeriodOptionsDisplayTimeSpan = {
      value: timeSpans[1],
      options: {
        id: 'timePeriodOptionsDisplayTimeSpan',
        stepsArray: timeSpans,
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

    $scope.timePeriodOptionsDisplayDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        id: 'timePeriodOptionsDisplayDateRange',
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

    $scope.timePeriodOptionsFilterDateRange = {
      minValue: minDate,
      maxValue: maxDate,
      options: {
        id: 'timePeriodOptionsFilterDateRange',
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
          updatePage(timePeriodOptionsFilterDateRange);
        } 
      }
    };

    $scope.timePeriodOptionsFilteredCreated = {
      value: dates[dates.length - 1],
      options: {
        id: 'timePeriodOptionsFilteredCreated',
        stepsArray: dates,
        translate: function(date_millis) {
          if ((date_millis !== null)) {
            var dateFromMillis = new Date(date_millis);
            return formatDate(dateFromMillis);
          }
          return '';
        },
        onEnd: function() {
          updatePage(timePeriodOptionsFilteredCreated);
        } 
      }
    };

    $scope.makeTimePeriodOptionsTimeSpanMonthly = function () {
      $scope.timePeriodOptionsTimeSpan.value = timeSpans[2];
  };

  });
})();
