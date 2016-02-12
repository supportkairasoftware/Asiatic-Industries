controllers.DashboardController = function ($scope) {

    SetPageTitle(PageTitle.Dashboard);
    var modelJson = $.parseJSON($("#USDMasterModel").val());
    $scope.USDMasterList = modelJson;

    if ($scope.USDMasterList.length > 0) {
        var totalGraphUSDMasterCount = $scope.USDMasterList.length;

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
            //colorStops: [
            //        { offset: 0, color: '#d2e6c9' },
            //        { offset: 1, color: 'white' }]
            colorStops: [{ offset: 0, color: '#FFFFFF' },
                         { offset: 1, color: 'white' }]
        };

        $('#jqChart').jqChart({
            title: 'USD Master History',
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
};

$(document).ready(function () {
    ShowPageLoadMessage("LoginSuccessMessage");

});