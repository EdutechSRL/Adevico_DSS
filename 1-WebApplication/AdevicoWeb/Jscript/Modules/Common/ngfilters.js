angular.module('ngfilters', [])
  .controller('filtercontroller', ['$scope', '$http', '$location', function ($scope, $http, $location) {
      $scope.errorMessage = "";
      $scope.errorDialog = false;
      $scope.filterType = ["text", "radio", "checkbox", "select", "text-select"];

      $(".hidebeforeloading").removeClass(".hidebeforeloading");

      function searchToObject() {
          var pairs = window.location.search.substring(1).split("&"),
            obj = {},
            pair,
            i;

          for (i in pairs) {
              if (pairs[i] === "") continue;

              pair = pairs[i].split("=");
              obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
          }

          return obj;
      }


      $scope.filters = [];

      $scope.getFilters = function () {
          $http.post(filterasmx + "/GetFilters", { "transactionid": transactionid, "filtermodule": filtermodule, "filtermodulescope": filtermodulescope, "filteridLanguage": filteridLanguage, "search": searchToObject(), "requiredpermissions": requiredp }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;



                $scope.filters = data.d;

                //console.log(data.d);

                //$(".chzn-select").chosen();

                //$scope.setFilters();
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.status = status;
                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;
                // alert($scope.errorMessage);
                //console.log(data);
            });
      }

      $scope.setFiltersChange = function (filter, reason) {
          //console.log(filter);
          if (filter.AutoUpdate == true) {

              //console.log(searchToObject());
              //console.log($location.absUrl());
              $http.post(filterasmx + "/SetFiltersChange", { "transactionid": transactionid, "filtermodule": filtermodule, "filtermodulescope": filtermodulescope, "filteridLanguage": filteridLanguage, "filters": $scope.filters, "filter": filter, "reason": reason, "url": $location.absUrl(), "search": searchToObject(), "requiredpermissions": requiredp, "unloadcommunitites": unloadcommunitites, "onlyfromOrganizations": onlyfromOrganizations, "availabilitystring": availabilitystring }).
                success(function (data, status) {
                    $scope.status = status;
                    $scope.data = data;

                    $scope.filters = data.d;

                    //console.log(data.d);
                })
                .
                error(function (data, status) {
                    $scope.data = data || "Request failed";
                    $scope.status = status;

                    $scope.error = JSON.parse(data.Message);
                    $scope.errorMessage = $scope.error.Message;
                    $scope.errorDialog = $scope.error.MessageDialog;

                    //console.log(data);
                });
          }
      }

      $scope.select = function (filter, value) {
          if (!value.Disabled) {
              filter.SelectedId = value.Id;
          }
      }

      $scope.setFilters = function () {
          //console.log(searchToObject());
          //console.log($location.absUrl());
          $http.post(filterasmx + "/SetFilters", { "transactionid": transactionid, "filtermodule": filtermodule, "filtermodulescope": filtermodulescope, "filteridLanguage": filteridLanguage, "filters": $scope.filters, "url": $location.absUrl(), "search": searchToObject(), "requiredpermissions": requiredp }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;

                $scope.filters = data.d;

                //console.log(data.d);
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.status = status;

                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;

                //console.log(data);
            });
      }


      $scope.getFilters();


  }]).directive('ngModelOnblur', function () {
      return {
          restrict: 'A',
          require: 'ngModel',
          priority: 1, // needed for angular 1.2.x
          link: function (scope, elm, attr, ngModelCtrl) {
              if (attr.type === 'radio' || attr.type === 'checkbox') return;

              elm.unbind('input').unbind('keydown').unbind('change');
              elm.bind('blur', function () {
                  scope.$apply(function () {
                      ngModelCtrl.$setViewValue(elm.val());
                  });
              });
          }
      };
  })
.directive('chosen', function () {
    /*return function (scope, element, attrs) {
        
        angular.element(element).chosen();
    };*/

    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.chosen, function (newValue, oldValue) {
                angular.element(element).chosen();
            });
        }
    };
})
;