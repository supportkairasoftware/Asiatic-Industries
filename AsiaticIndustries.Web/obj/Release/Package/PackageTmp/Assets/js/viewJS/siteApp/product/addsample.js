controllers.AddProductSampleController = function ($scope, $http, $location) {

    SetPageTitle(pageTitle);
    var dataModel = $.parseJSON($("#ProductSampleModel").val());

    $scope.ProductSampleModel = dataModel.ProductSample;
    $scope.ProductSearchUrl = SiteUrl.ProductSearchUrl;
    $scope.PrepopulateProducts = dataModel.PrepopulateProducts;
    $scope.StrNewProductIds = "";
    //$scope.NewProductItemIds = dataModel.NewProductItemIds;
    //$scope.OldProductItemIds = dataModel.OldProductItemIds;

    //$scope.ListOfRawMaterial = dataModel.ListOfRawMaterial;
    //$scope.ListOfProductItem = dataModel.ListOfProductItem;

    $scope.SaveProductSample = function () {
       
        $("#btnSubmit").attr("disabled", "disabled");
        var param = {};
        param.ProductSample = $scope.ProductSampleModel;
        //param.ListOfProductItem = $scope.ListOfProductItem;
        //param.NewProductItemIds = $scope.NewProductItemIds;
        //param.OldProductItemIds = $scope.OldProductItemIds;
        var jsonData = angular.toJson(param);
        
        if (CheckErrors(jQuery("#frmAddProductSample"))) {
            AngularAjaxCall($http, SiteUrl.SaveProductSample, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        $location.url(SiteUrl.ManageProductSampleUrl);
                    } else {
                        $("#btnSubmit").removeAttr("disabled");
                    }
                    ShowMessages(response);
                });
        } else {
            $("#btnSubmit").removeAttr("disabled");
        }
    };
    

    $scope.AddNewRawMaterilal = function () {
        var newProductItemId = GetGuid();
        $scope.ListOfProductItem.push({ StrProductItemID: newProductItemId });
        $scope.NewProductItemIds.push(newProductItemId);
    };



    //Strat: Product Search Section
    $scope.OnProductAddedCallback = function (item, e) {
        $scope.$apply(function() {
        $scope.ProductSampleModel.ProductID = item.StrProductID;
        });//if (self.ItemModel().NewItemTagIds() == null || self.ItemModel().NewItemTagIds().indexOf(item.StrTagID) == -1) {
        //    self.ItemModel().NewItemTagIds.push(item.StrTagID);
        //    self.ItemModel().StrNewItemTagIds(self.ItemModel().NewItemTagIds().toString());
        //}
    };

    $scope.OnProductDeletedCallback = function (item, e) {
        $scope.$apply(function() {
            $scope.ProductSampleModel.ProductID = 0;
        });
        //if (self.ItemModel().NewItemTagIds().length > 0 && self.ItemModel().NewItemTagIds().indexOf(item.StrTagID) != -1) {
        //    self.ItemModel().NewItemTagIds.remove(item.StrTagID);
        //    self.ItemModel().StrNewItemTagIds(self.ItemModel().NewItemTagIds().toString());
        //}
    };

    $scope.OnProductResultCallback = function (results,data) {
        //results = $.grep(results, function (n, i) {
        //    if (self.ItemModel().NewItemTagIds() == null || self.ItemModel().NewItemTagIds().indexOf(n.StrTagID) == -1)
        //        return n;
        //});
        return results;
    };

    $scope.OnProductResultsFormatter = function (item) {
        return "<li id='" + item.StrProductID + "'>" + item.ProductName + "</div></li>";
    };

    //End:  Product Search Section END
};