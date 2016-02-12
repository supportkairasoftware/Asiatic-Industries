controllers.AddCustomerController = function ($scope, $http, $location) {
    
    SetPageTitle(pageTitle);
    var modelJson = $.parseJSON($("#CustomerModel").val());
    $scope.CustomerModel = modelJson;

    $scope.SaveCustomer = function () {
        $("#btnSubmit").attr("disabled", "disabled");
        var jsonData = angular.toJson($scope.CustomerModel);
        
        if (CheckErrors(jQuery("#frmAddCustomer"))) {
            AngularAjaxCall($http, SiteUrl.SaveCustomer, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                        $location.path(SiteUrl.ManageCustomerUrl);
                    } else {
                        ShowMessages(response);
                        $("#btnSubmit").removeAttr("disabled");
                    }
                });
        } else {
            $("#btnSubmit").removeAttr("disabled");
        }
    };
};