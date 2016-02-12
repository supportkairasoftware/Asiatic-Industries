controllers.AddProductController = function ($scope, $http, $location) {

    SetPageTitle(pageTitle);
    var dataModel = $.parseJSON($("#ProductModel").val());

    $scope.ProductModel =  dataModel.Product;
    $scope.NewProductItemIds = dataModel.NewProductItemIds;
    $scope.OldProductItemIds = dataModel.OldProductItemIds;

    $scope.ListOfRawMaterial = dataModel.ListOfRawMaterial;
   
    $scope.ListOfProductItem = dataModel.ListOfProductItem;
    if ($scope.ListOfProductItem == undefined || $scope.ListOfProductItem.length == 0) {
        var tnewProductItemId = GetGuid();
        $scope.ListOfProductItem = [];
        $scope.ListOfProductItem.push({ StrProductItemID: tnewProductItemId });
        $scope.NewProductItemIds.push(tnewProductItemId);
    }

    $scope.SaveProduct = function () {

        $("#btnSubmit").attr("disabled", "disabled");
        var param = {};
        param.Product = $scope.ProductModel;
        param.ListOfProductItem = $scope.ListOfProductItem;
        param.NewProductItemIds = $scope.NewProductItemIds;
        param.OldProductItemIds = $scope.OldProductItemIds;
        var jsonData = angular.toJson(param);
        
        if (CheckErrors(jQuery("#frmAddProduct"))) {
            AngularAjaxCall($http, SiteUrl.SaveProduct, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        $location.url(SiteUrl.ManageProductUrl);
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

    $scope.RemoveRawMaterilal = function(index) {
        var selectedData = $.grep($scope.NewProductItemIds, function(data, i) {
            if ($scope.ListOfProductItem[index].StrProductItemID == data)
                return data;
        });
        $scope.NewProductItemIds.splice($scope.NewProductItemIds.indexOf(selectedData[0]), 1);
        $scope.ListOfProductItem.splice(index, 1);
    };


    $scope.RawMaterialChangeEvent = function(selectedProductItem) {
       
        selectedProductItem.RawMaterialID = $scope.selectedItem.code;
    };


};