app.controller("LoginController", function ($scope, $http, $templateCache) {

    SetPageTitle(pageTitle);
    $(".wrapper").css("visibility", "hidden");
    $templateCache.removeAll();

    $scope.loginUrl = "/security/login";
    $scope.homeUrl = "/home";
    
    $scope.LoginModel = {
        Email: "",
        Password: "",
        IsRemember: false
    };
    
    $scope.Login = function () {
        var jsonData = angular.toJson($scope.LoginModel);
        var isValid = CheckErrors($("#frmLogin"));
        if (isValid) {
            var retrunUrl = getReturnURL('returnUrl=');
            AngularAjaxCall($http, $scope.loginUrl, jsonData, "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        //$rootScope.loggedInUserId = response.Data.User.UserID;
                        //$rootScope.loggedInUserRole = response.Data.Role;
                        if (response.IsSuccess) {
                            SetMessageForPageLoad(response.Message, "LoginSuccessMessage");
                            if (retrunUrl.length > 0) {
                                window.location = "/"+retrunUrl;
                            } else {
                                window.location = $scope.homeUrl;
                            }
                        } else {
                            ShowMessages(response);
                        }
                });
            }
    };


    $scope.LoginOnEnter = function (data, e) {
        if (e.which == 13) {
            e.preventDefault();
            self.Login(self.LoginModel);
        }
        return true;
    };



    // For disable browser back
    window.history.forward();
});
$(document).ready(function () {
    //if (window.location.href.indexOf("#/") != -1)
    //    window.location = "security";
    $(".wrapper").css("visibility", "visible");
})