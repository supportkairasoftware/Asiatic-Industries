app.config(function ($routeProvider) {
    $routeProvider
        .when("/login", { templateUrl: "/security/index", controller: "LoginController" })
        .when("/dashboard", { templateUrl: "/dashboard/index", controller: "DashboardController" })
        .when("/myaccount", { templateUrl: "/user/myaccount", controller: "AddUserController" })
        .when("/user/:id?",
            {
                controller: "AddUserController",
                templateUrl: function (params) { return '/user/adduser/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/addproduct/:id?",
            {
                controller: "AddProductController",
                templateUrl: function (params) { return '/product/addproduct/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/listproduct", { templateUrl: "/product/manageproduct", controller: "ManageProductController" })
        .when("/addsample/:id?",
            {
                controller: "AddProductSampleController",
                templateUrl: function (params) { return '/product/addsample/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/listsample", { templateUrl: "/product/managesample", controller: "ManageProductSampleController" })
        .when("/addquote/:id?",
            {
                controller: "AddQuoteController",
                templateUrl: function (params) { return '/product/addquote/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/listquote", { templateUrl: "/product/managequote", controller: "ManageQuoteController" })
        .when("/listuser", { templateUrl: "/user/manageuser", controller: "ManageUserController" })
        .when("/addcustomer/:id?",
            {
                controller: "AddCustomerController",
                templateUrl: function (params) { return '/user/addcustomer/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/listcustomer", { templateUrl: "/user/managecustomer", controller: "ManageCustomerController" })
        .when("/addrawmaterial/:id?",
            {
                controller: "AddRawMaterialController",
                templateUrl: function (params) { return '/product/addrawmaterial/' + (params.id == undefined ? "" : params.id); }
            })
        .when("/listrawmaterial", { templateUrl: "/product/managerawmaterial", controller: "ManageRawMaterialController" })
        .when("/usdmaster", { templateUrl: "/user/usdmaster", controller: "USDMasterController" })
        .when("/accessdenied", { templateUrl: "/security/accessdenied", controller: "LoginController" })
        .when("/notfound", { templateUrl: "/security/notfound", controller: "LoginController" })
        .when("/internalerror", { templateUrl: "/security/internalerror", controller: "LoginController" })
        .otherwise({ redirectTo: "/dashboard" });
});


//beforeEach(module('ui.router'));
//app.config(function ($stateProvider) {

//    $stateProvider
//        .state('listcustomer', {
//            url: '/user/managecustomer',
            
//            // ...
//            data: {
//                requireLogin: true
//            }
//        });
//    //.state('app', {
//    //    abstract: true,
//    //    // ...
//    //    data: {
//    //        requireLogin: true // this property will apply to all children of 'app'
//    //    }
//    //})
//    //.state('app.dashboard', {
//    //    // child state of `app`
//    //    // requireLogin === true
//    //})

//});