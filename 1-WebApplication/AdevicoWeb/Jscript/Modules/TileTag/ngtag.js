angular.module('ngtags', ['ngfilters'])
  .controller('tagcontroller', ['$scope', '$http', '$location', function ($scope, $http, $location) {
      
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
      $scope.errorMessage = "";
      $scope.errorDialog = false;

      $scope.tag = {};
      $scope.tagLink = {};

      $scope.readonlyDialog = false;

      $(".hidebeforeloading").removeClass(".hidebeforeloading");

      $scope.getTagLink = function (id, idLanguage, readonly) {

          $http.post(tagasmx + "/GetTagLink", { "id": id, "idLanguage": idLanguage }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;

                $scope.errorMessage = "";

                $scope.tagLink = data.d;
                if (readonly) {
                    $(".dlgtaglink").addClass("readonly").removeClass("new").removeClass("edit");
                    $(".dlgtaglink").dialog("option", "title", $(".dlgtaglink").attr("view-title"));
                } else {
                    if (id != 0) {
                        $(".dlgtaglink").addClass("edit").removeClass("new").removeClass("readonly");
                        $(".dlgtaglink").dialog("option", "title", $(".dlgtaglink").attr("edit-title"));
                    } else {
                        $(".dlgtaglink").addClass("new").removeClass("readonly").removeClass("edit");
                        $(".dlgtaglink").dialog("option", "title", $(".dlgtaglink").attr("new-title"));
                    }
                }

                $scope.readonlyDialog = readonly;
                $(".dlgtaglink").dialog("open");



                //$(".chzn-select").chosen();

                //$scope.setFilters();
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;
                $scope.status = status;
                //console.log(data);
            });

      };

      $scope.getTag = function (id, readonly) {
          $http.post(tagasmx + "/GetTag", { "id": id, "disabled": readonly }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;

                $scope.errorMessage = "";

                $scope.tag = data.d;

                //console.log($scope.tag);

                if (readonly) {
                    $(".dlgtagedit").dialog("option", "title", $(".dlgtagedit").attr("view-title"));
                    $(".dlgtagedit").addClass("readonly").removeClass("new").removeClass("edit");
                } else {
                    if (id != 0) {
                        $(".dlgtagedit").addClass("edit").removeClass("new").removeClass("readonly");
                        $(".dlgtagedit").dialog("option", "title", $(".dlgtagedit").attr("edit-title"));
                    } else {
                        $(".dlgtagedit").addClass("new").removeClass("readonly").removeClass("edit");
                        $(".dlgtagedit").dialog("option", "title", $(".dlgtagedit").attr("new-title"));
                    }
                }

                $scope.readonlyDialog = readonly;
                //console.log(data.d);
                $(".dlgtagedit").dialog("open");


                //$(".chzn-select").chosen();

                //$scope.setFilters();
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;
                $scope.status = status;
                //console.log(data);
            });
      }

      $scope.setTag = function (tag) {
          $http.post(tagasmx + "/SetTag", { "dto": tag, "search": searchToObject() }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;
                $scope.errorMessage = "";


                //$scope.tag = data.d;
                //console.log(data.d);


                $(".dlgtagedit").dialog("close");
                $(".hiddensubmit").click();
                //$(".chzn-select").chosen();

                //$scope.setFilters();
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;
                $scope.status = status;
                //console.log(data);
            });
      }

      $scope.setTagLink = function (tagLink) {
          $http.post(tagasmx + "/SetTagLink", { "dto": tagLink }).
            success(function (data, status) {
                $scope.status = status;
                $scope.data = data;
                $scope.errorMessage = "";


                //$scope.tag = data.d;
                //console.log(data.d);


                $(".dlgtaglink").dialog("close");
                $(".hiddensubmit").click();
                //$(".chzn-select").chosen();

                //$scope.setFilters();
            })
            .
            error(function (data, status) {
                $scope.data = data || "Request failed";
                $scope.error = JSON.parse(data.Message);
                $scope.errorMessage = $scope.error.Message;
                $scope.errorDialog = $scope.error.MessageDialog;
                $scope.status = status;
                //console.log(data);
            });
      }


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
    .directive('uiDialog', function () {
        return {
            link: function (scope, elm, attrs) {

                var options = {
                    title: attrs.title,
                    width: attrs.uiWidth || 700,
                    height: attrs.uiHeight || 400,
                    modal: attrs.uiModal == "true" || attrs.uiModal == "True",
                    autoOpen: attrs.uiAutoOpen == "true" || attrs.uiAutoOpen == "True",
                    appendTo: "form",
                    open: function () {
                        $(this).find(".chzn-container").trigger("chosen:updated");
                        $(this).find(".chzn-container").remove();
                        $(this).find(".chzn-done").removeClass("chzn-done");
                        $(this).find(".chzn-container").chosen();
                        $(this).find(".chzn-container").trigger("chosen:updated");
                    },
                    close: function () {
                        //$(this).find(".tabs").tabs("destroy");
                        $(this).find(".chzn-container").trigger("chosen:updated");
                        $(this).find(".chzn-container").remove();
                        $(this).find(".chzn-done").removeClass("chzn-done");

                    }
                }

                var jqueryElm = $(elm[0]);
                $(jqueryElm).dialog(options);
            }
        };
    })

    .directive('uiTabs', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                scope.$watch(
                                    //attrs.ngModel
                                    function () {

                                        return ngModel.$modelValue;

                                    }
                                    , function (newValue, oldValue) {

                                        if (typeof newValue != 'undefined') {

                                            angular.element(element).tabs("destroy");
                                            setTimeout(function () {
                                                angular.element(element).tabs();
                                                angular.element(element).find(".ui-tabs-active.ui-state-active").removeClass("ui-tabs-active").removeClass("ui-state-active");
                                                angular.element(element).tabs("option", "active", 0);
                                                angular.element(element).tabs("refresh");
                                            }, 0);

                                        }
                                        //angular.element(element).chosen();
                                        //angular.element(element).trigger("chosen:updated");
                                    });


                /*scope.$watch(attrs.uiTabs, function (newValue, oldValue) {
                    if (typeof newValue != 'undefined') {
                        
                        setTimeout(function () {
                            angular.element(element).tabs();
                        }, 100);
                    }
                        
                });*/
            }
        }
    })
.directive('chosenTag', function () {
    /*return function (scope, element, attrs) {
        
        angular.element(element).chosen();
    };*/

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            scope.$watch(
                //attrs.ngModel
                function () {

                    return ngModel.$modelValue;

                }
                , function (newValue, oldValue) {

                    if (typeof newValue != 'undefined') {

                        angular.element(element).chosen();
                        ////angular.element(element).trigger("chosen:updated");

                        //setTimeout(function () {
                        //angular.element(element).chosen();
                        //}, 1000);

                    }
                    //angular.element(element).chosen();
                    //angular.element(element).trigger("chosen:updated");
                });

        }
    };
})
;