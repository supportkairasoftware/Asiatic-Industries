controllers.AddUserController = function ($scope, $http, $location) {
    
    SetPageTitle(pageTitle);
    var modelJson = $.parseJSON($("#UserModel").val());
    $scope.UserModel = modelJson;

    $scope.SaveUser = function () {
        $("#btnSubmit").attr("disabled", "disabled");
        $scope.UserModel.ListOfRole = null;
        var jsonData = angular.toJson($scope.UserModel);
        if (CheckErrors(jQuery("#frmAddUser"))) {
            AngularAjaxCall($http, SiteUrl.SaveUser, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                        if (window.location.toString().indexOf(SiteUrl.Myaccount) == -1)
                            $location.path(SiteUrl.ManageUserUrl);
                        else {
                            $location.path(SiteUrl.Dashboard);
                        }
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