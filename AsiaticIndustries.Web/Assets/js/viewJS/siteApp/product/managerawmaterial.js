controllers.ManageRawMaterialController = function ($scope, $http, $templateCache, $location) {

    SetPageTitle(PageTitle.RawMaterialList);
    var modelJson = $.parseJSON($("#SearchRawMaterialModel").val());
    $scope.SearchRawMaterialModel = modelJson;

    $scope.SearchRawMaterialRecords = function () {
        $scope.GetRawMaterialList(1);
        $scope.pagination.current = 1;
        $scope.TotalRawMaterialCount = 0;
        $scope.TotalRawMaterialCountTable1 = 0;
        $scope.TotalRawMaterialCountTable2 = 0;
        $scope.TotalRawMaterialCountTable3 = 0;
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
        $scope.GetRawMaterialList($scope.pagination.current);
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //-----------------------------------------CODE FOR PAGINATION-------------------------------------------
    $scope.currentPage = 1;
    $scope.PageSize = window.PageSize * 5;
    $scope.pagination = {
        current: 1
    };
    $scope.RawMaterialList = [];
    $scope.TotalRawMaterialCount = 0;
    $scope.TotalRawMaterialCountTable1 = 0;
    $scope.TotalRawMaterialCountTable2 = 0;
    $scope.TotalRawMaterialCountTable3 = 0;

    $scope.GetRawMaterialList = function (pageNumber) {
        $scope.pagermodel = {
            searchParams: $scope.SearchRawMaterialModel,
            pageSize: $scope.PageSize,
            pageIndex: pageNumber,
            sortIndex: $scope.predicate != undefined ? $scope.predicate : "CreatedDate",
            sortDirection: $scope.reverse == true ? "DESC" : "ASC"
        };

        var jsonData = angular.toJson($scope.pagermodel);
        AngularAjaxCall($http, SiteUrl.GetRawMaterialList, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.RawMaterialList1 = response.Data.Items.splice(0, Math.round(response.Data.Items.length / 3));
                    $scope.RawMaterialList2 = response.Data.Items.splice(0, Math.round(response.Data.Items.length / 2));
                    $scope.RawMaterialList3 = response.Data.Items;

                    $scope.TotalRawMaterialCount = response.Data.TotalItems;

                    $scope.TotalRawMaterialCountTable1 = $scope.RawMaterialList1.length;
                    $scope.TotalRawMaterialCountTable2 = $scope.RawMaterialList2.length;
                    $scope.TotalRawMaterialCountTable3 = $scope.RawMaterialList3.length;
                    //$scope.NoRecordsFound($scope.TotalRawMaterialCount);
                }
            });
    };

    $scope.PageChanged = function (newPage) {
        $scope.GetRawMaterialList(newPage);
    };
    $scope.GetRawMaterialList(1);

    //-------------------------------------------------------------------------------------------------------


    $scope.ShowGraph = function (rawMaterialId, buttonId, graphEle) {
        
        var isVisible = $('#' + graphEle).is(':hidden');
        var pos = $("#" + buttonId).offset();
        console.log("Top = " + pos.top);
        console.log("Left = " + pos.left);
        console.log("Width = " + screen.width);
        console.log("Height = " + screen.height);

        if ((screen.width - pos.left) <= 350) {
            $("#" + graphEle).css('left', '0px');
        }

        $(".coreJQChart").css('display', 'none');

        

        if (isVisible) {
            $('#' + graphEle).css('display', 'block');

            $scope.pagermodel = {
                encryptedRawMaterialID: rawMaterialId,
                pageSize: 99999,
                pageIndex: 1,
                sortIndex: "LastUpdatedDate",
                sortDirection: "DESC"
            };

            var jsonData = angular.toJson($scope.pagermodel);

            AngularAjaxCall($http, SiteUrl.GetRawMaterialListForGraph, jsonData, "Post", "json", "application/json").
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.RawMaterialGraphList = response.Data.Items;
                        var totalGraphUSDMasterCount = response.Data.TotalItems;
                        //$scope.NoRecordsFound($scope.TotalUSDMasterCount);

                        var data1 = [];
                        var tooltipData = [];

                        for (var i = 0, count = totalGraphUSDMasterCount; i < count; i++) {
                            var changedDate = new Date($scope.RawMaterialGraphList[i].LastUpdatedDate);
                            var usdRate = parseFloat($scope.RawMaterialGraphList[i].RawMaterialPrice);
                            var lastChangedName = $scope.RawMaterialGraphList[i].LastUpdatedName;

                            data1.push([i, usdRate, $scope.RawMaterialGraphList[i]]);
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

                        $('#' + graphEle + '> .jqchart').jqChart({
                            title: 'Raw Material History',
                            font: '12px sans-serif',
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
                                    visible: false,
                                    labels: { // x axes range
                                        visible: false,
                                        //strokeStyle: '#39435C',
                                    }
                                },
                                {
                                    title: { text: 'USD Rate (Rs.)' },
                                }
                            ],
                            series: [
                                {
                                    //title: 'USD History',
                                    type: 'line',
                                    data: data1,
                                    markers: {
                                        visible: true,
                                    }
                                }
                            ]
                        });

                        $('#' + graphEle).bind('tooltipFormat', function (e, data) {
                            if ($.isArray(data) == false) {

                                var date = data.chart.stringFormat(new Date(data.dataItem[2].LastUpdatedDate), "ddd, mmm dS, yyyy, hh:MM:ss TT");

                                var tooltip = '<b>' + date + '</b><br />' +
                                      '<span style="color:' + data.series.fillStyle + '"> Price: </span>' +
                                      '<b>' + data.dataItem[2].RawMaterialPrice + '</b><br />' +
                                '<span style="color:' + data.series.fillStyle + '"> Changed By: </span>' +
                                      '<b>' + data.dataItem[2].LastUpdatedName + '</b><br />';

                                return tooltip;
                            }
                        });

                        //var largeContent = $('#' + graphEle).html(),
                        //    largeSettings = {
                        //        content: largeContent,
                        //        width: 450,
                        //        //height: 350,
                        //        //delay: { show: 300, hide: 1000 },
                        //        closeable: true
                        //    };
                        //$("#" + buttonId).webuiPopover('destroy').webuiPopover($.extend({}, settings, largeSettings));
                    }
                });
        } else {
            $('#' + graphEle).css('display', 'none');
        }
    }

    $scope.CloseGraph = function(graphId) {
        $('#' + graphId).css("display", "none");
    };


    //------------------------------------------------Edit RAW MATERIAL--------------------------------------------
    $scope.EditRawMaterial = function (data) {
        //Cache remove for the update time get updated values.
        $templateCache.removeAll();
        data.IsEditMode = !data.IsEditMode;
    };

    $scope.SaveRawMaterial = function (data, frmname) {
        
        $("#btnSubmit").attr("disabled", "disabled");
        var jsonData = angular.toJson(data);
        $('#' + frmname).removeAttr("novalidate");

        if (CheckErrors(jQuery('#' + frmname))) {
            AngularAjaxCall($http, SiteUrl.SaveRawMaterial, jsonData, "POST", "json", "application/json").
                success(function(response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        data.IsEditMode = !data.IsEditMode;
                        //$scope.GetRawMaterialList($scope.pagination.current);
                    }
                    $("#btnSubmit").removeAttr("disabled");
                });
        } else {
            $("#btnSubmit").removeAttr("disabled");
        }
    };

    //-------------------------------------------------------------------------------------------------------

    //------------------------------------------------Enabled/Disabled RAW MATERIAL--------------------------------------------
    $scope.EnabledDisabledRawMaterial = function (rawMaterial) {
        var url = SiteUrl.EnabledDisabledRawMaterialUrl + rawMaterial.EncryptedRawMaterialID;
        //SureToEnableUser
        var title = '', message = '';
        if (rawMaterial.IsActive) {
            title = window.DisableRawMaterial;
            message = window.SureToDisableRawMaterialMessage;
        } else {
            title = window.EnableRawMaterial;
            message = window.SureToEnableRawMaterialMessage;
        }
        ShowBootstrapConfirmModel(title, message, function (result) {
            if (result) {
                AngularAjaxCall($http, url, "", "Post", "json", "application/json").
                    success(function (response, status, headers, config) {
                        if (response.IsSuccess) {
                            ShowMessages(response);
                            $scope.GetRawMaterialList($scope.pagination.current);
                        } else {
                            ShowMessages(response);
                        }
                    });
            }
        }, "Skip", "Yes - Go Ahead!");
    };

};