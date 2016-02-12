controllers.ManageCustomerController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.CustomerList);
    var modelJson = $.parseJSON($("#SearchCustomerModel").val());
    $scope.SearchCustomerModel = modelJson;

    $scope.SearchCustomerRecords = function () {
        $scope.GetCustomerList(1);
        $scope.pagination.current = 1;
        $scope.TotalCustomerCount = 0;
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
        $scope.GetCustomerList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.CustomerList = [];
    $scope.TotalCustomerCount = 0;

    $scope.GetCustomerList = function (pageNumber) {
        $scope.pagermodel = {
            searchParams: $scope.SearchCustomerModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "CreatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetCustomerList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.CustomerList = response.Data.Items;
                    $scope.TotalCustomerCount = response.Data.TotalItems;
                    //$scope.NoRecordsFound($scope.TotalCustomerCount);
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetCustomerList(newPage);
    };
    $scope.GetCustomerList(1);

    //-------------------------------------------------------------------------------------------------------
    
    // Edit Customer
    $scope.EditCustomer = function (id) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        var url = SiteUrl.EditCustomerUrl + id;
        $location.url(url);
    };
    
    //------------------------------------------------DELETE CUSTOMER--------------------------------------------
    $scope.EnabledDisabledCustomer = function (customer) {
        var url = SiteUrl.EnabledDisabledCustomerUrl + customer.EncryptedCustomerID;
        //SureToEnableUser
        var title = '', message = '';
        if (customer.IsActive) {
            title = window.DisableCustomer;
            message = window.SureToDisableCustomerMessage;
        } else {
            title = window.EnableCustomer;
            message = window.SureToEnableCustomerMessage;
        }
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetCustomerList($scope.pagination.current);
                        } else {
                            ShowMessages(response);
                        }
                    });
            }
        }, "Skip", "Yes - Go Ahead!");
    };
};