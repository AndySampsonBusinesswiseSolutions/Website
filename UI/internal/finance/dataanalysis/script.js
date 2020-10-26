// Code goes here
(() => {
  angular
  .module('dateRangeDemo', ['ui.bootstrap', 'rzModule'])
  .controller('dateRangeCtrl', function dateRangeCtrl($scope) {
    // Single Date Slider    
    var timeSpans = [];
    for (var i = 1; i <= 7; i++) {
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
          return 'Five Minutely';
        case 2:
          return 'Half Hourly';
        case 3:
          return 'Daily';
        case 4:
          return 'Weekly';
        case 5:
          return 'Monthly';
        case 6:
          return 'Quarterly';
        case 7:
          return 'Yearly';
        default:
          return '';
      }
    }
    
    // Date Range Slider
    var floorDate = new Date(2021, 0, 1).getTime();
    var ceilDate = new Date(2025, 11, 31).getTime();
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

    $scope.makeTimePeriodOptionsTimeSpanMonthly = function () {
      $scope.timePeriodOptionsDisplayTimeSpan.value = timeSpans[3];
    };

    function checkDateRangeIsWithinTolerances() {
      var granularity = timePeriodOptionsDisplayTimeSpan.children[6].innerHTML;
      var startDate = getStartDate();
      var endDate = getEndDate();
      var toleranceEndDate = getEndDate();
    
      if(granularity == 'Half Hourly') {
        toleranceEndDate = new Date(startDate.setMonth(startDate.getMonth()+1));
      }
      if(granularity == 'Five Minutely') {
        toleranceEndDate = new Date(startDate.setDate(startDate.getDate()+7));
      }
    
      if(endDate > toleranceEndDate) {
        toleranceEndDate = new Date(toleranceEndDate.setDate(toleranceEndDate.getDate()-1));
        if(toleranceEndDate > maxDate) {toleranceEndDate = maxDate}
        setTimePeriodOptionsDisplayDateRange(getStartDate(), toleranceEndDate);
      }  
    }

    function setTimePeriodOptionsDisplayDateRange(newMinDate, newMaxDate)
    {
      $scope.timePeriodOptionsDisplayDateRange = {
        minValue: newMinDate,
        maxValue: newMaxDate,
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
        }
      };
    }

    $scope.resetSliders = function () {
      $scope.timePeriodOptionsDisplayTimeSpan.value = timeSpans[2];
      $scope.timePeriodOptionsFilteredCreated.value = dates[dates.length - 1];
      setTimePeriodOptionsDisplayDateRange(minDate, maxDate);

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
        }
      };
    };

    //Configs
    $scope.timePeriodOptionsDisplayTimeSpan = {
      value: timeSpans[2],
      options: {
        id: 'timePeriodOptionsDisplayTimeSpan',
        stepsArray: timeSpans,
        translate: function(date) {
          if (date !== null)
            return dateToTimePeriod(date);
          return '';
        },
        onEnd: function() {
          checkDateRangeIsWithinTolerances();
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
          checkDateRangeIsWithinTolerances();
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
      }
    };
  });
})();