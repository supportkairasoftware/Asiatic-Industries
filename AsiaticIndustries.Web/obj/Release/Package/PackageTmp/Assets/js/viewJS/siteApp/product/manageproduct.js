controllers.ManageProductController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.ProductList);
    var modelJson = $.parseJSON($("#SearchProductModel").val());
    $scope.SearchProductModel = modelJson;
    //$scope.SearchProductModel.RawMaterialID = 2;

    $scope.SearchProductRecords = function () {
        
        $scope.GetProductList(1);
        $scope.pagination.current = 1;
        $scope.TotalProductCount = 0;
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
        $scope.GetProductList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.ProductList = [];
    $scope.TotalProductCount = 0;

    $scope.GetProductList = function (pageNumber) {
       
        $scope.pagermodel = {
            searchParams: $scope.SearchProductModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "CreatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetProductList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.ProductList = response.Data.Items;
                    $scope.TotalProductCount = response.Data.TotalItems;
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetProductList(newPage);
    };
    $scope.GetProductList(1);

    //-------------------------------------------------------------------------------------------------------
    
    $scope.EditProduct = function (id) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        var url = SiteUrl.EditProductUrl + id;
        $location.url(url);
    };

    //------------------------------------------------DELETE Product--------------------------------------------
    $scope.EnabledDisabledProduct = function(product) {
        var url = SiteUrl.EnabledDisabledProductUrl + product.EncryptedProductID;
        //SureToEnableProduct
        var title = '', message = '';
        if (product.IsActive) {
            title = window.DisableProduct;
            message = window.SureToDisableProductMessage;
        } else {
            title = window.EnableProduct;
            message = window.SureToEnableProductMessage;
        }
       
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetProductList($scope.pagination.current);
                            //if ((($scope.TotalProductCount - 1) / window.PageSize) == ($scope.pagination.current - 1)) {
                            //    $scope.GetProductList($scope.pagination.current - 1);
                            //} else {
                            //    $scope.GetProductList($scope.pagination.current);
                            //}
                        } else {
                            ShowToaster("errorDialog", response.Message);
                        }
                    });
            }
        }, "Skip", "Yes - Go Ahead!");

    };
};