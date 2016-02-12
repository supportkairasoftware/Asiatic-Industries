controllers.AddRawMaterialController = function ($scope, $http, $location) {
    
    SetPageTitle(pageTitle);
    var modelJson = $.parseJSON($("#RawMaterialModel").val());
    $scope.RawMaterialModel = modelJson;

    $scope.SaveRawMaterial = function () {
        $("#btnSubmit").attr("disabled", "disabled");
        var jsonData = angular.toJson($scope.RawMaterialModel);
        if (CheckErrors(jQuery("#frmAddRawMaterial"))) {
            AngularAjaxCall($http, SiteUrl.SaveRawMaterial, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                        $location.path(SiteUrl.ManageRawMaterialUrl);
                    } else {
                        ShowMessages(response);
                    }
                    $("#btnSubmit").removeAttr("disabled");
                });
        } else {
            $("#btnSubmit").removeAttr("disabled");
        }
    };
};