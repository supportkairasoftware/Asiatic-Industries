controllers.ManageUserController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.UserList);
    var modelJson = $.parseJSON($("#SearchUserModel").val());
    $scope.SearchUserModel = modelJson;

    $scope.SearchUserRecords = function () {
        $scope.GetUserList(1);
        $scope.pagination.current = 1;
        $scope.TotalUserCount = 0;
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    $scope.predicate = 'CreatedDate'; // coulumn name
    $scope.reverse = true; // asc and desc
    $scope.SortColumn = function (predicate) {
        $templateCache.removeAll();
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
        $scope.sortIndex = $scope.predicate != undefined ? $scope.predicate : "CreatedDate";
        $scope.sortDirection = $scope.reverse == true ? "DESC" : "ASC";
        $scope.GetUserList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.UserList = [];
    $scope.TotalUserCount = 0;

    $scope.GetUserList = function (pageNumber) {
       
        $scope.pagermodel = {
            searchParams: $scope.SearchUserModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "LastUpdatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetUserList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.UserList = response.Data.Items;
                    $scope.TotalUserCount = response.Data.TotalItems;
                    //$scope.NoRecordsFound($scope.TotalUserCount);
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetUserList(newPage);
    };
    $scope.GetUserList(1);

    //-------------------------------------------------------------------------------------------------------
    
    // Edit User
    $scope.EditUser = function (id) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        var url = SiteUrl.EditUserUrl + id;
        $location.url(url);
    };
    
    //------------------------------------------------DELETE USER--------------------------------------------
    $scope.EnabledDisabledUser = function (user) {
        var url = SiteUrl.EnabledDisabledUserUrl + user.EncryptedUserID;
        //SureToEnableUser
        var title = '', message = '';
        if (user.IsActive) {
            title = window.DisableUser;
            message = window.SureToDisableUserMessage;
        } else {
            title = window.EnableUser;
            message = window.SureToEnableUserMessage;
        }
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetUserList($scope.pagination.current);
                        } else {
                            ShowMessages(response);
                        }
                    });
            }
        }, "Skip", "Yes - Go Ahead!");
    };
};