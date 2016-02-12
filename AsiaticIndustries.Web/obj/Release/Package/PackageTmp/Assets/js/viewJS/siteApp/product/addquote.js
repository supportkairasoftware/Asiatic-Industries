controllers.AddQuoteController = function ($scope, $http, $location) {

    SetPageTitle(pageTitle);
    var dataModel = $.parseJSON($("#QuoteModel").val());
    
    $scope.NewUSDRate = dataModel.NewUSDRate;
    $scope.CurrentUSDRate = dataModel.CurrentUSDRate;
    $scope.QuoteModel = dataModel.Quote;

    $scope.CustomerID = dataModel.CustomerID;
    $scope.PrepopulateCustomers = dataModel.PrepopulateCustomers;
    $scope.TempPrepopulateCustomers = [];
    $scope.StrPrepopulateCustomers = dataModel.StrPrepopulateCustomers;
    $scope.CustomerSearchUrl = SiteUrl.CustomerSearchUrl;


    $scope.TempProductSampleId = dataModel.TempProductSampleId;
    $scope.PrepopulateProductSamples = dataModel.PrepopulateProductSamples;
    $scope.TempPrepopulateProductSamples = [];
    $scope.OldProductSampleIds = dataModel.OldProductSampleIds == null ? [] : dataModel.OldProductSampleIds;
    $scope.NewProductSampleIds = dataModel.NewProductSampleIds == null ? [] : dataModel.NewProductSampleIds;
    $scope.StrPrepopulateProductSamples = dataModel.StrPrepopulateProductSamples;
    $scope.ProductSampleSearchUrl = SiteUrl.ProductSampleSearchUrl;



    $scope.SaveQuote = function () {

        $("#btnSubmit").attr("disabled", "disabled");
        var param = {};
        
        param.Quote = $scope.QuoteModel;
        param.Quote.CustomerID = $scope.StrPrepopulateCustomers;
        param.StrPrepopulateProductSamples = $scope.StrPrepopulateProductSamples;
        param.OldProductSampleIds = $scope.OldProductSampleIds;
        param.NewProductSampleIds = $scope.NewProductSampleIds;
        param.QuoteDetail = $scope.QuotePriceModel;
        //param.OldProductItemIds = $scope.OldProductItemIds;
        var jsonData = angular.toJson(param);

        if (CheckErrors(jQuery("#frmAddQuote"))) {
            AngularAjaxCall($http, SiteUrl.SaveQuote, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        $location.url(SiteUrl.ManageQuoteUrl);
                    } else {
                        $("#btnSubmit").removeAttr("disabled");
                    }
                    ShowMessages(response);
                });
        } else {
            $("#btnSubmit").removeAttr("disabled");
        }
    };



    //Strat: Customer Search Section
    $scope.OnCustomerAddedCallback = function (item, e) {

        $scope.$apply(function() {

            $scope.PrepopulateCustomers.push(item);
            $scope.TempPrepopulateCustomers.push(item.StrCustomerID);
            $scope.StrPrepopulateCustomers = $scope.TempPrepopulateCustomers.toString();
            $scope.CustomerID = $scope.TempPrepopulateCustomers.toString();

        });

    };

    $scope.OnCustomerDeletedCallback = function (item, e) {
        
        $scope.$apply(function() {
        var index = $scope.PrepopulateCustomers.indexOf(item);
        $scope.PrepopulateCustomers.splice(index, 1);

        var tempIndex = $scope.TempPrepopulateCustomers.indexOf(item.StrCustomerID);
        $scope.TempPrepopulateCustomers.splice(tempIndex, 1);

        $scope.StrPrepopulateCustomers = $scope.TempPrepopulateCustomers.toString();
        $scope.CustomerID = $scope.TempPrepopulateCustomers.toString();
        });
    };

    $scope.OnCustomerResultCallback = function (results, data) {
        results = $.grep(results, function (n, i) {
            if ($scope.PrepopulateCustomers == null || $scope.PrepopulateCustomers.indexOf(n.StrCustomerID) == -1)
                return n;
        });
        return results;
    };

    $scope.OnCustomerResultsFormatter = function (item) {
        
        item.Email = item.Email == undefined ? "NA" : item.Email;
        item.Phone = item.Phone == undefined ? "NA" : item.Phone;
        item.Address = item.Address == undefined ? "NA" : item.Address;
        return "<li id='{0}'>{1} {2} ({3})<br/><small><b  style='color:#ad0303;'>Phone: </b>{4}</small><br/><small ><b style='color:#ad0303;'>Address: </b>{5}</small></li>".format(item.StrCustomerID, item.FirstName, item.LastName, item.Email, item.Phone, item.Address);
    };

    $scope.OnCustomerTokenFormatter = function (item) {
        item.Email = item.Email == undefined ? "NA" : item.Email;
        item.Phone = item.Phone == undefined ? "NA" : item.Phone;
        item.Address = item.Address == undefined ? "NA" : item.Address;
        return "<li id='{0}'>{1} {2} ({3})<br/><small><b  style='color:#ad0303;'>Phone: </b>{4}</small><br/><small ><b style='color:#ad0303;'>Address: </b>{5}</small></li>".format(item.StrCustomerID, item.FirstName, item.LastName, item.Email, item.Phone, item.Address);

        //return "<li id='{0}'>{1} {2}</li>".format(item.StrCustomerID, item.FirstName, item.LastName);
    };


    //End:  Customer Search Section END

    $scope.QuotePriceModel = {};
    $scope.QuoteProductPriceArray = [];

    $scope.$watchCollection("QuoteProductPriceArray", function (newValue, oldValue) {
        if (newValue.length != undefined) {
            $scope.QuotePriceModel.TotalCrudePerkg = 0;
            $scope.QuotePriceModel.TotalMFGOH = 0;
            $scope.QuotePriceModel.TotalADMOH = 0;
            $scope.QuotePriceModel.TotalProfit = 0;
            $scope.QuotePriceModel.TotalProductCost = 0;
            
            $scope.QuotePriceModel.USDRate = 0;
            

            angular.forEach(newValue, function (value, index) {
                value.CrudePerkg = value.CrudePerkg == null ? 0 : value.CrudePerkg;
                value.MFGOH = value.MFGOH == null ? 0 : value.MFGOH;
                value.ADMOH = value.ADMOH == null ? 0 : value.ADMOH;
                value.Profit = value.Profit == null ? 0 : value.Profit;
                value.TotalProductCost = value.TotalProductCost == null ? 0 : value.TotalProductCost;

                $scope.QuotePriceModel.USDRate = 0;
                
                $scope.QuotePriceModel.TotalCrudePerkg = $scope.QuotePriceModel.TotalCrudePerkg == undefined ? parseFloat(0) + parseFloat(value.CrudePerKg) : parseFloat($scope.QuotePriceModel.TotalCrudePerkg) + parseFloat(value.CrudePerKg);
                $scope.QuotePriceModel.TotalMFGOH = $scope.QuotePriceModel.TotalMFGOH == undefined ? parseFloat(0) + parseFloat(value.MFGOH) : parseFloat($scope.QuotePriceModel.TotalMFGOH) + parseFloat(value.MFGOH);
                $scope.QuotePriceModel.TotalADMOH = $scope.QuotePriceModel.TotalADMOH == undefined ? parseFloat(0) + parseFloat(value.ADMOH) : parseFloat($scope.QuotePriceModel.TotalADMOH) + parseFloat(value.ADMOH);
                $scope.QuotePriceModel.TotalProfit = $scope.QuotePriceModel.TotalProfit == undefined ? parseFloat(0) + parseFloat(value.Profit) : parseFloat($scope.QuotePriceModel.TotalProfit) + parseFloat(value.Profit);
                $scope.QuotePriceModel.TotalProductCost = $scope.QuotePriceModel.TotalProductCost == undefined ? parseFloat(0) + parseFloat(value.TotalProductCost) : parseFloat($scope.QuotePriceModel.TotalProductCost) + parseFloat(value.TotalProductCost);

            });

        }
    });

    var isLoad = true;
    $scope.$watch("QuoteModel.IsPriceWillRevised", function (newValue, oldValue) {

        if (isLoad) {

            
            angular.forEach($scope.PrepopulateCustomers, function (item, index) {
                $scope.TempPrepopulateCustomers.push(item.StrCustomerID);
                $scope.StrPrepopulateCustomers = $scope.TempPrepopulateCustomers.toString();
                $scope.CustomerID = $scope.TempPrepopulateCustomers.toString();
            });

            angular.forEach($scope.PrepopulateProductSamples, function (item, index) {
                $scope.TempPrepopulateProductSamples.push(item.StrProductSampleID);
                $scope.StrPrepopulateProductSamples = $scope.TempPrepopulateProductSamples.toString();
                $scope.TempProductSampleId = $scope.TempPrepopulateProductSamples.toString();
            });
            
            
            isLoad = false;
        }

        if (!isLoad) {
            
            $scope.QuoteProductPriceArray.splice(0, $scope.QuoteProductPriceArray.length);
            angular.forEach($scope.PrepopulateProductSamples, function (item, index) {
                var param = {};
                param.ProductSampleID = item.StrProductSampleID;
                param.QuoteID = item.QuoteID;
                param.IsPriceWillRevised = $scope.QuoteModel.IsPriceWillRevised;

                AngularAjaxCall($http, SiteUrl.GetQuoteProductPriceModel, angular.toJson(param), "POST", "json", "application/json").
                    success(function(response) {
                        if (response.IsSuccess) {
                            $scope.QuoteProductPriceArray.push(response.Data);
                            //$scope.$watch(function (scope) { return scope.QuoteProductPriceArray; }, function (newValue, oldValue) {
                            //    
                            //});
                        } else {
                            ShowMessages(response);
                        }
                    });
            });


        }
        
    });

    //$scope.$watchCollection("PrepopulateProductSamples", function (newValue, oldValue) {
    //    if (newValue.length != undefined) {
    //        $scope.QuotePriceModel.TotalCrudePerkg = 0;
    //        $scope.QuotePriceModel.TotalMFGOH = 0;
    //        $scope.QuotePriceModel.TotalADMOH = 0;
    //        $scope.QuotePriceModel.TotalProfit = 0;

    //        angular.forEach(newValue, function (value, index) {
    //            value.CrudePerkg = value.CrudePerkg == null ? 0 : value.CrudePerkg;
    //            value.MFGOH = value.MFGOH == null ? 0 : value.MFGOH;
    //            value.ADMOH = value.ADMOH == null ? 0 : value.ADMOH;
    //            value.Profit = value.Profit == null ? 0 : value.Profit;

    //            $scope.QuotePriceModel.TotalCrudePerkg = $scope.QuotePriceModel.TotalCrudePerkg == undefined ? parseFloat(0) : parseFloat($scope.QuotePriceModel.TotalCrudePerkg) + parseFloat(value.CrudePerKg);
    //            $scope.QuotePriceModel.TotalMFGOH = $scope.QuotePriceModel.TotalMFGOH == undefined ? parseFloat(0) : parseFloat($scope.QuotePriceModel.TotalMFGOH) + parseFloat(value.MFGOH);
    //            $scope.QuotePriceModel.TotalADMOH = $scope.QuotePriceModel.TotalADMOH == undefined ? parseFloat(0) : parseFloat($scope.QuotePriceModel.TotalADMOH) + parseFloat(value.ADMOH);
    //            $scope.QuotePriceModel.TotalProfit = $scope.QuotePriceModel.TotalProfit == undefined ? parseFloat(0) : parseFloat($scope.QuotePriceModel.TotalProfit) + parseFloat(value.Profit);
    //        });

    //    }
    //});


    //Strat: Product Sample Search Section
    //var isFirstLoad = true;
    $scope.OnProductSampleAddedCallback = function(item, e) {
        $scope.$apply(function() {

            $scope.PrepopulateProductSamples.push(item);
            $scope.TempPrepopulateProductSamples.push(item.StrProductSampleID);
            $scope.NewProductSampleIds.push(item.StrProductSampleID);
            $scope.StrPrepopulateProductSamples = $scope.TempPrepopulateProductSamples.toString();
            $scope.TempProductSampleId = $scope.TempPrepopulateProductSamples.toString();

            var param = {};
            param.ProductSampleID = item.StrProductSampleID;
            param.QuoteID = item.QuoteID;
            param.IsPriceWillRevised = $scope.QuoteModel.IsPriceWillRevised;

            AngularAjaxCall($http, SiteUrl.GetQuoteProductPriceModel, angular.toJson(param), "POST", "json", "application/json").
                success(function(response) {
                    if (response.IsSuccess) {
                        $scope.QuoteProductPriceArray.push(response.Data);
                        //$scope.$watch(function (scope) { return scope.QuoteProductPriceArray; }, function (newValue, oldValue) {
                        //    
                        //});
                    } else {
                        ShowMessages(response);
                    }
                });
        });
    };

    $scope.OnProductSampleDeletedCallback = function (item, e) {
        $scope.$apply(function () {
        var index = $scope.PrepopulateProductSamples.indexOf(item);
        $scope.PrepopulateProductSamples.splice(index, 1);
        
        var tempIndex = $scope.TempPrepopulateProductSamples.indexOf(item.StrProductSampleID);
        $scope.TempPrepopulateProductSamples.splice(tempIndex, 1);
        

        $scope.NewProductSampleIds.splice($scope.NewProductSampleIds.indexOf(item.StrProductSampleID),1);


        $scope.StrPrepopulateProductSamples = $scope.TempPrepopulateProductSamples.toString();
        $scope.TempProductSampleId = $scope.TempPrepopulateProductSamples.toString();
        

      


        var results;
          $scope.QuoteProductPriceArray.filter(function (data, index) {
            if (data.ProductSampleID == item.StrProductSampleID)
                results= index;
        });
       
            $scope.QuoteProductPriceArray.splice(results, 1);
        });
        
    };
    
    


    $scope.OnProductSampleResultCallback = function (results, data) {
        results = $.grep(results, function (n, i) {
            if ($scope.TempPrepopulateProductSamples == null || $scope.TempPrepopulateProductSamples.indexOf(n.StrCustomerID) == -1)
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


    //End:  Product Sample Section END

};