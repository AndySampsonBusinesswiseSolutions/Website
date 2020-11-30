// Code goes here
(() => {
    angular
    .module('timePeriodControlDemo', ['ui.bootstrap', 'rzModule'])
    .controller('timePeriodControl', function timePeriodControl($scope) {
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

      $scope.resetSliders = function () {
        $scope.timePeriodDateRange = {
          minValue: minDate,
          maxValue: maxDate,
          options: {
            id: 'timePeriodDateRange',
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
              // updateCharts();
            } 
          }
        };
  
        $scope.timePeriodCreationDateRange = {
          minValue: minDate,
          maxValue: maxDate,
          options: {
            id: 'timePeriodCreationDateRange',
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
              // updateCharts();
            } 
          }
        };
      };
  
      //Configs
      $scope.timePeriodDateRange = {
        minValue: minDate,
        maxValue: maxDate,
        options: {
          id: 'timePeriodDateRange',
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
            // updateCharts();
          } 
        }
      };

      $scope.timePeriodCreationDateRange = {
        minValue: minDate,
        maxValue: maxDate,
        options: {
          id: 'timePeriodCreationDateRange',
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
            // updateCharts();
          } 
        }
      };
    });
  })();
  