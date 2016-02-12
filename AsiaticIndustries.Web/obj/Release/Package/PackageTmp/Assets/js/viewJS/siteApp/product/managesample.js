controllers.ManageProductSampleController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.SampleList);
    var modelJson = $.parseJSON($("#SearchProductSampleModel").val());
    $scope.SearchProductSampleModel = modelJson;
    $scope.SearchProductSampleModel.StrProductIDs = "";
    
    $scope.ProductSearchUrl = SiteUrl.ProductSearchUrl;
    $scope.PrepopulateProducts = [];//JSON.parse([]);// new Array();
    
    $scope.SearchProductSampleRecords = function () {
        
        $scope.GetProductSampleList(1);
        $scope.pagination.current = 1;
        $scope.TotalProductSampleCount = 0;
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
        $scope.GetProductSampleList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.ProductSampleList = [];
    $scope.TotalProductSampleCount = 0;

    $scope.GetProductSampleList = function (pageNumber) {
       
        $scope.pagermodel = {
            searchParams: $scope.SearchProductSampleModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "CreatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetProductSampleList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.ProductSampleList = response.Data.Items;
                    $scope.TotalProductSampleCount = response.Data.TotalItems;
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetProductSampleList(newPage);
    };
    $scope.GetProductSampleList(1);

    //-------------------------------------------------------------------------------------------------------
    
    $scope.EditProductSample = function (id) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        var url = SiteUrl.EditProductSampleUrl + id;
        $location.url(url);
    };

    //------------------------------------------------DELETE Product--------------------------------------------
    $scope.EnabledDisabledProductSample = function (product) {
        var url = SiteUrl.EnabledDisabledProductSampleUrl + product.EncryptedProductSampleID;
        //SureToEnableProduct
        var title = '', message = '';
        if (product.IsActive) {
            title = window.DisableProductSample;
            message = window.SureToDisableProductSampleMessage;
        } else {
            title = window.EnableProductSample;
            message = window.SureToEnableProductSampleMessage;
        }
       
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetProductSampleList($scope.pagination.current);
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
    






    //Strat: Product Search Section
    $scope.OnProductAddedCallback = function (item, e) {
        $scope.SearchProductSampleModel.ProductIDs.push(item.StrProductID);
        $scope.SearchProductSampleModel.StrProductIDs = $scope.SearchProductSampleModel.ProductIDs.toString();
        //if (self.ItemModel().NewItemTagIds() == null || self.ItemModel().NewItemTagIds().indexOf(item.StrTagID) == -1) {
        //    self.ItemModel().NewItemTagIds.push(item.StrTagID);
        //    self.ItemModel().StrNewItemTagIds(self.ItemModel().NewItemTagIds().toString());
        //}
    };

    $scope.OnProductDeletedCallback = function (item, e) {
        var index=$scope.SearchProductSampleModel.ProductIDs.indexOf(item.StrProductID);
        $scope.SearchProductSampleModel.ProductIDs.splice(index, 1);
        $scope.SearchProductSampleModel.StrProductIDs = $scope.SearchProductSampleModel.ProductIDs.toString();
        //if (self.ItemModel().NewItemTagIds().length > 0 && self.ItemModel().NewItemTagIds().indexOf(item.StrTagID) != -1) {
        //    self.ItemModel().NewItemTagIds.remove(item.StrTagID);
        //    self.ItemModel().StrNewItemTagIds(self.ItemModel().NewItemTagIds().toString());
        //}
    };

    $scope.OnProductResultCallback = function (results, data) {
        results = $.grep(results, function(n, i) {
            if ($scope.SearchProductSampleModel.ProductIDs == null || $scope.SearchProductSampleModel.ProductIDs.indexOf(n.StrProductID) == -1)
                return n;
        });
        return results;
    };

    $scope.OnProductResultsFormatter = function (item) {
        return "<li id='" + item.StrProductID + "'>" + item.ProductName + "</div></li>";
    };

    //End:  Product  Search Section END
};