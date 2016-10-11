
function bindMergeTradingOrderPageEvent() {
    $(document).on('click', '#reloadBtn', function () {
        reloadMergeTradingOrder();
    });

    $(document).on('click', '.merge-cancel-btn', function () {
        clearAllValues();
        $('.mergeOrderDiv').hide();
    });

    $(document).on('click', '.mergeAction', function () {
        $('.mergeOrderDiv').show();
        mergeTradingOrder(this);
    });

    $(document).on('click', '.merge-save-btn', function () {
        saveMergeTradingOrder();
    });
}

function reloadMergeTradingOrder() {
    if (validateInputs()) {
        var startDate = $("#StartDate").val();
        var endDate = $("#EndDate").val();
        var clientUserId = $('#ClientUserId').val() === '' ? 0 : +$('#ClientUserId').val();

        $.ajax({
            url: "/Balance/ReloadMergeTradingOrderPartial",
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
    var endDate = $("#EndDate").val();
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

function mergeTradingOrder(obj) {
    var rowObj = $(obj).closest('tr');
    $('.tradingorder-id').val($(rowObj).data('tradingorderid'));
    $('.stock-name').text($(rowObj).data('stockname'));
    $('.trading-date').text($(rowObj).data('tradingdate'));
    $('.trading-price').text($(rowObj).data('tradingprice'));
    $('.trading-number').text($(rowObj).data('tradingnumber'));
}

function saveMergeTradingOrder() {
    var tradingOrderId = $('.tradingorder-id').val();
    var mergeNumber = $('.merge-number').val();
    var tradingNumber = $('.trading-number').text();
    if (IsNumber(mergeNumber) && +mergeNumber <= +tradingNumber) {
        $.ajax({
            url: "/Balance/SaveMergeTradingOrder",
            data: { TOId: tradingOrderId, MergeNumber: mergeNumber },
            type: "POST",
            success: function (data) {
                if (data == 'success') {
                    reloadMergeTradingOrder();
                    clearAllValues();
                    $('.mergeOrderDiv').hide();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Save merge trading order error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        alert("请输入不大于交易数量的合并值");
    }
}

function clearAllValues() {
    $('.tradingorder-id').val('');
    $('.stock-name').text('');
    $('.trading-date').text('');
    $('.trading-price').text('');
    $('.trading-number').text('');
    $('.merge-number').val('');
}