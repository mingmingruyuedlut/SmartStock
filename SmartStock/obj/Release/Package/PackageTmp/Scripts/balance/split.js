
function bindSplitTradingOrderPageEvent() {
    $(document).on('click', '#reloadBtn', function () {
        reloadSplitTradingOrder();
    });

    $(document).on('click', '.split-cancel-btn', function () {
        clearAllValues();
        $('.splitOrderDiv').hide();
    });

    $(document).on('click', '.splitAction', function () {
        $('.splitOrderDiv').show();
        splitTradingOrder(this);
    });

    $(document).on('click', '.split-save-btn', function () {
        saveSplitTradingOrder();
    });
}

function reloadSplitTradingOrder() {
    if (validateInputs()) {
        var startDate = $("#StartDate").val();
        var endDate = $("#EndDate").val();
        var clientUserId = $('#ClientUserId').val() === '' ? 0 : +$('#ClientUserId').val();

        $.ajax({
            url: "/Balance/ReloadSplitTradingOrderPartial",
            data: { StartDate: startDate, EndDate: endDate, UserId: clientUserId },
            type: "POST",
            success: function (data) {
                $('.dashboard-partial').html(data);
                initCommonDataTable('#DashboardDataTbl');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Reload split trading order partial error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        // to - do
    }
}

function validateInputs() {
    var valResult = true;
    var startDate = $("#StartDate").val();
    var endDate = $("#StartDate").val();
    var clientUserId = $('#ClientUserId').val() === '' ? 0 : +$('#ClientUserId').val();

    if (startDate == null || startDate == 'Invalid Date') {
        valResult = false;
        alert("请输入正确的起始时间");
    }
    else if (endDate == null || endDate == 'Invalid Date') {
        valResult = false;
        alert("请输入正确的结束时间");
    }

    return valResult;
}

function splitTradingOrder(obj) {
    var rowObj = $(obj).closest('tr');
    $('.tradingorder-id').val($(rowObj).data('tradingorderid'));
    $('.stock-name').text($(rowObj).data('stockname'));
    $('.trading-date').text($(rowObj).data('tradingdate'));
    $('.trading-price').text($(rowObj).data('tradingprice'));
    $('.trading-number').text($(rowObj).data('tradingnumber'));
    var tradingdate = $(rowObj).data('tradingdate').toString().yyyymmddToDate();
    $(".split-date").datepicker("setDate", new Date(tradingdate.setDate(tradingdate.getDate() + 1)));
}

function saveSplitTradingOrder() {
    var tradingOrderId = $('.tradingorder-id').val();
    var splitDate = $('.split-date').val();
    var splitNumber = $('.split-number').val();
    var tradingNumber = $('.trading-number').text();
    if (IsNumber(splitNumber) && +splitNumber <= +tradingNumber && splitDate != null && splitDate != 'Invalid Date') {
        $.ajax({
            url: "/Balance/SaveSplitTradingOrder",
            data: { TOId: tradingOrderId, SplitDate: splitDate, SplitNumber: splitNumber },
            type: "POST",
            success: function (data) {
                if (data == 'success') {
                    reloadSplitTradingOrder();
                    clearAllValues();
                    $('.splitOrderDiv').hide();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Save split trading order error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        alert("请输入不大于交易数量的拆分值以及正确的日期");
    }
}

function clearAllValues() {
    $('.tradingorder-id').val('');
    $('.stock-name').text('');
    $('.trading-date').text('');
    $('.trading-price').text('');
    $('.trading-number').text('');
    $('.split-number').val('');
    $('.split-date').val('');
}