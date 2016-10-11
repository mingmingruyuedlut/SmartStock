function bindDashboardPageEvent() {
    $(document).on('click', '#reloadBtn', function () {
        reloadDashboardPartial();
    });
}

function initDashboardLineChart(dailyTradingLabels, dailyProfit) {

    var lineChartData = {
        labels: dailyTradingLabels,
        datasets: [
          {
              label: "收益",
              fillColor: "rgba(60,141,188,0.9)",
              strokeColor: "rgba(60,141,188,0.8)",
              pointColor: "#3b8bba",
              pointStrokeColor: "rgba(60,141,188,1)",
              pointHighlightFill: "#fff",
              pointHighlightStroke: "rgba(60,141,188,1)",
              data: dailyProfit
          }
        ]
    };

    var lineChartOptions = {
        //Boolean - If we should show the scale at all
        showScale: true,
        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: false,
        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",
        //Number - Width of the grid lines
        scaleGridLineWidth: 1,
        //Boolean - Whether to show horizontal lines (except X axis)
        scaleShowHorizontalLines: true,
        //Boolean - Whether to show vertical lines (except Y axis)
        scaleShowVerticalLines: true,
        //Boolean - Whether the line is curved between points
        bezierCurve: true,
        //Number - Tension of the bezier curve between points
        bezierCurveTension: 0.3,
        //Boolean - Whether to show a dot for each point
        pointDot: true,
        //Number - Radius of each point dot in pixels
        pointDotRadius: 4,
        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,
        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,
        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,
        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,
        //Boolean - Whether to fill the dataset with a color
        datasetFill: false,
        //String - A legend template
        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>",
        //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
        maintainAspectRatio: true,
        //Boolean - whether to make the chart responsive to window resizing
        responsive: true,
        //Put dataset labels into multiTooltip Template
        multiTooltipTemplate: "<%= datasetLabel %> : <%= value %>",
        // Boolean or a positive integer denoting number of labels to be shown on x axis
        showXLabels: 10,
    };

    //-------------
    //- LINE CHART -
    //--------------
    var lineChartCanvas = $("#tradingOrderDailyLineChart").get(0).getContext("2d");
    var lineChart = new Chart(lineChartCanvas);
    lineChart.Line(lineChartData, lineChartOptions);
}

function reloadDashboardPartial() {
    var startDate = $("#StartDate").val();
    var endDate = $("#EndDate").val();
    if (validateDateRange(startDate, endDate)) {
        $.ajax({
            url: "/Dashboard/ReloadOperatorDashboardPartial",
            data: { StartDate: startDate, EndDate: endDate },
            type: "POST",
            success: function (data) {
                $('.dashboard-partial').html(data);
                initCommonDataTable('#DashboardDataTbl');
                reloadDashboardLineChart(startDate, endDate);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Reload dashboard partial error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        //to-do
    }
}

function reloadDashboardLineChart(startDate, endDate) {
    $.ajax({
        url: "/Dashboard/ReloadOperatorDashboardLineChart",
        data: { StartDate: startDate, EndDate: endDate },
        type: "POST",
        success: function (data) {
            if (data.TradingDateList.length > 0) {
                $('#tradingOrderDailyLineChart').remove();
                $('.trading-order-daily').append('<canvas id="tradingOrderDailyLineChart" style="height:300px"></canvas>');
                initDashboardLineChart(data.TradingDateList, data.DailyProfitList);
            }
            else {
                //to-do
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Reload dashboard line chart error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}

function validateDateRange(startDateStr, endDateStr) {
    var valResult = true;
    var startDate = new Date(startDateStr);
    var endDate = new Date(endDateStr);
    if (startDate == null || startDate == 'Invalid Date') {
        valResult = false;
        alert("请输入正确的起始时间");
    }
    else if (endDate == null || endDate == 'Invalid Date') {
        valResult = false;
        alert("请输入正确的结束时间");
    }
    else if (endDate.getTime() < startDate.getTime()) {
        valResult = false;
        alert("起始时间不能大于结束时间");
    }
    return valResult;
}