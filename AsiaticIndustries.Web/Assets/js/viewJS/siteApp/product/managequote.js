controllers.ManageQuoteController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.QuoteList);
    var modelJson = $.parseJSON($("#SearchQuoteModel").val());
    $scope.SearchQuoteModel = modelJson;
    $scope.PrepopulateProductSamples = [];
    $scope.ProductSampleSearchUrl = SiteUrl.ProductSampleSearchUrl;



    $scope.SearchQuoteRecords = function () {
        
        $scope.GetQuoteList(1);
        $scope.pagination.current = 1;
        $scope.TotalQuoteCount = 0;
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
        $scope.GetQuoteList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.QuoteList = [];
    $scope.TotalQuoteCount = 0;

    $scope.GetQuoteList = function (pageNumber) {
       
        $scope.pagermodel = {
            searchParams: $scope.SearchQuoteModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "CreatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetQuoteList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.QuoteList = response.Data.Items;
                    $scope.TotalQuoteCount = response.Data.TotalItems;
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetQuoteList(newPage);
    };
    $scope.GetQuoteList(1);

    //-------------------------------------------------------------------------------------------------------
    
    $scope.EditQuote = function (id) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        var url = SiteUrl.EditQuoteUrl + id;
        $location.url(url);
    };

    //------------------------------------------------DELETE Product--------------------------------------------
    $scope.EnabledDisabledQuote = function (quote) {
        
        var url = SiteUrl.EnabledDisabledQuoteUrl + quote.EncryptedQuoteID;
        //SureToEnableProduct
        var title = '', message = '';
        if (quote.IsActive) {
            title = window.DisableQuote;
            message = window.SureToDisableQuoteMessage;
        } else {
            title = window.EnableQuote;
            message = window.SureToEnableQuoteMessage;
        }
       
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetQuoteList($scope.pagination.current);
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
    





    //Strat: Product Sample Search Section
    $scope.OnProductSampleAddedCallback = function (item, e) {
        $scope.SearchQuoteModel.ProductSampleIDs.push(item.StrProductSampleID);
        $scope.SearchQuoteModel.StrProductSampleIDs = $scope.SearchQuoteModel.ProductSampleIDs.toString();
       
    };

    $scope.OnProductSampleDeletedCallback = function (item, e) {
        var index = $scope.SearchQuoteModel.ProductSampleIDs.indexOf(item.StrProductSampleID);
        $scope.SearchQuoteModel.ProductSampleIDs.splice(index, 1);
        $scope.SearchQuoteModel.StrProductSampleIDs = $scope.SearchQuoteModel.ProductSampleIDs.toString();
       
    };
    
    $scope.OnProductSampleResultCallback = function (results, data) {
        results = $.grep(results, function (n, i) {
            if ($scope.SearchQuoteModel.ProductSampleIDs == null || $scope.SearchQuoteModel.ProductSampleIDs.indexOf(n.StrProductSampleIDs) == -1)
                return n;
        });
        return results;
    };


    $scope.OnProductSampleResultsFormatter = function (item) {
        return "<li id='{0}'><b>AI #: </b> {1}<br/><small><b  style='color:#ad0303;'>Product: </b>{2}</small></li>".format(item.StrProductSampleID, item.ReferenceNumber, item.ProductName);
    };

    $scope.OnProductSampleTokenFormatter = function (item) {
        return "<li id='{0}'><b>AI #: </b> {1}<br/><small><b  style='color:#ad0303;'>Product: </b>{2}</small></li>".format(item.StrProductSampleID, item.ReferenceNumber, item.ProductName);
        //return "<li id='{0}'>{1} {2}</li>".format(item.StrCustomerID, item.FirstName, item.LastName);
    };
    
    //End:  Product  Search Section END

};