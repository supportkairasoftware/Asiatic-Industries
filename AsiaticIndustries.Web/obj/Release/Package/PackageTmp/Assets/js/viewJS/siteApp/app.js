var app = angular.module('myApp', ['ngRoute', 'ngLoadingSpinner', 'angularUtils.directives.dirPagination', 'ui.bootstrap']);

var controllers = {};
app.controller(controllers);

app.run(function ($rootScope, $templateCache) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (typeof (current) !== 'undefined') {
            $templateCache.remove(current.templateUrl);
        }
    });
});

app.config([
    "$httpProvider",
    function ($httpProvider) {
        //$httpProvider
        //    .interceptors.push("httpInterceptor");
        
        $httpProvider.interceptors.push(function ($q, $location, $window) {
            return {

                response: function (response) {
                    return response;
                },
                responseError: function (response) {
                    
                    if (response.data != null || response.data != undefined) {
                        var data = angular.fromJson(response.data);

                        if (data.StatusCode == 401) {
                            var returnUrl = "";

                            if (data.RequestType == "post") {
                                returnUrl = window.location.hash;
                            }

                            $window.location = data.Link + returnUrl;
                        }
                    }
                    else if (response.status == 403)
                        $location.url('/security/login');
                    return $q.reject(response);
                }
            };
        });
    }
]);



