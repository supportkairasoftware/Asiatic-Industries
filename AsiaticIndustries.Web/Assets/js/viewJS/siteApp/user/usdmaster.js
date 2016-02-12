controllers.USDMasterController = function ($scope, $http, $location) {

    SetPageTitle(PageTitle.USDMaster);
    var modelJson = $.parseJSON($("#USDMasterModel").val());
    $scope.USDMasterModel = modelJson;

    $scope.EditUSDRate = function (data) {
        //Cache remove for the update time get updated values.
        data.IsEditMode = !data.IsEditMode;
    };

    $scope.SaveUSDRate = function (data) {
        $("#btnUpdateUsd").attr("disabled", "disabled");
        var jsonData = angular.toJson(data);
        if (CheckErrors(jQuery("#frmUSDMaster"))) {
            AngularAjaxCall($http, SiteUrl.SaveUSDMasterUrl, jsonData, "POST", "json", "application/json").
                success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        data.IsEditMode = !data.IsEditMode;
                        $scope.GetUSDMasterList($scope.pagination.current);
                        $scope.GetGraphUSDMasterList();
                    }
                    $("#btnUpdateUsd").removeAttr("disabled");
                });
        } else {
            $("#btnUpdateUsd").removeAttr("disabled");
        }
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------//

    $scope.predicate = 'ChangedDate'; // coulumn name
    $scope.reverse = true; // asc and desc
    $scope.SortColumn = function (predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
        $scope.sortIndex = $scope.predicate != undefined ? $scope.predicate : "ChangedDate";
        $scope.sortDirection = $scope.reverse == true ? "DESC" : "ASC";
        $scope.GetUSDMasterList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize;
    $scope.pagination = {
        current: 1
    };
    $scope.USDMasterList = [];
    $scope.TotalUSDMasterCount = 0;

    $scope.GetUSDMasterList = function (pageNumber) {
        $scope.pagermodel = {
            searchParams: $scope.USDMasterModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "ChangedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetUSDMasterList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.USDMasterList = response.Data.Items;
                    $scope.TotalUSDMasterCount = response.Data.TotalItems;
                    //$scope.NoRecordsFound($scope.TotalUSDMasterCount);
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetUSDMasterList(newPage);
    };
    $scope.GetUSDMasterList(1);

    //-------------------------------------------------------------------------------------------------------

    $scope.GetGraphUSDMasterList = function () {
        $scope.pagermodel = {
            pageSize: 99999,
            pageIndex: 1,
            sortIndex: "ChangedDate",
            sortDirection: "DESC"
        };

        var jsonData = angular.toJson($scope.pagermodel);

        AngularAjaxCall($http, SiteUrl.GetGraphUSDMasterList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.USDMasterList = response.Data.Items;
                    var totalGraphUSDMasterCount = response.Data.TotalItems;

                    var data1 = [];
                    var tooltipData = [];

                    for (var i = 0, count = totalGraphUSDMasterCount; i < count; i++) {
                        var changedDate = new Date($scope.USDMasterList[i].ChangedDate);
                        var usdRate = parseFloat($scope.USDMasterList[i].CurrentUSDRate);
                        var lastChangedName = $scope.USDMasterList[i].LastUpdatedName;

                        data1.push([i, usdRate, $scope.USDMasterList[i]]);
                    }

                    var background = {
                        type: 'linearGradient',
                        x0: 0,
                        y0: 0,
                        x1: 0,
                        y1: 1,
                        colorStops: [{ offset: 0, color: '#FFFFFF' },
                                     { offset: 1, color: 'white' }]
                    };

                    $('#jqChart').jqChart({
                        title: 'USD Master History',
                        legend: { title: '' },
                        border: { strokeStyle: '#39435C' },
                        background: background,
                        animation: { duration: 1 },
                        tooltips: { type: 'shared' },
                        legend: { visible: false },
                        shadows: {
                            enabled: true
                        },
                        crosshairs: {
                            enabled: true,
                            hLine: false,
                            vLine: { strokeStyle: '#39435C' }
                        },
                        axes: [
                            {
                                title: { text: 'Days' },
                                type: 'number',
                                location: 'bottom',
                                zoomEnabled: true,
                                labels: {
                                    visible: false,
                                    strokeStyle: '#39435C',
                                    //strokeStyle: '#FFFFFF',
                                }
                            },
                            {
                                title: { text: 'USD Rate (Rs.)' },
                            }
                        ],
                        series: [
                            {
                                title: 'USD History',
                                type: 'line',
                                data: data1,
                                markers: true
                            }
                        ]
                    });

                    $('#jqChart').bind('tooltipFormat', function (e, data) {

                        if ($.isArray(data) == false) {

                            var date = data.chart.stringFormat(new Date(data.dataItem[2].ChangedDate), "ddd, mmm dS, yyyy, hh:MM:ss TT");

                            var tooltip = '<b>' + date + '</b><br />' +
                                  '<span style="color:' + data.series.fillStyle + '"> USD Rate: </span>' +
                                  '<b>' + data.dataItem[2].CurrentUSDRate + '</b><br />' +
                            '<span style="color:' + data.series.fillStyle + '"> Changed By: </span>' +
                                  '<b>' + data.dataItem[2].LastUpdatedName + '</b><br />';

                            return tooltip;
                        }
                    });
                }
            });
    };
    $scope.GetGraphUSDMasterList();
};
