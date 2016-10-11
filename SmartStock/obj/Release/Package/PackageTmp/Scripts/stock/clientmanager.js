
var isTradingClientEdit = false;
var tradingClientEditId = 0;

function bindClientManagerEvent() {
    $(document).on('click', '.stock-add-btn', function () {
        clearAllValues();
        $('.addClientDiv').show();
    });

    $(document).on('click', '.stock-cancel-btn', function () {
        clearAllValues();
        $('.addClientDiv').hide();
    });

    $(document).on('click', '.editAction', function () {
        $('.addClientDiv').show();
        editTradingClient(this);
    });

    $(document).on('click', '.deleteAction', function () {
        //deleteTradingClient(this);
    });

    $(document).on('click', '.stock-save-btn', function () {
        saveTradingClient();
    });
}

function saveTradingClient() {
    if (validateInputs()) {
        var tradingClientInfo = getTradingClientInfoObj()
        var tradingClientInfoJsonStr = JSON.stringify(tradingClientInfo);
        $.ajax({
            url: "/Stock/SaveTradingClient",
            data: { TradingClientJsonStr: tradingClientInfoJsonStr, IsEdit: isTradingClientEdit },
            type: "POST",
            success: function (data) {
                if (data == 'success') {
                    reloadTradingClient();
                    clearAllValues();
                    $('.addClientDiv').hide();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Save trading client info error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        //to-do
    }
}

function validateInputs() {
    var valResult = true;

    var loginUserId = $('#LoginUserId').val();
    var stockAccountLogin = $('.stock-account-login').val();
    var stockAccountPwd = $('.stock-account-pwd').val();
    var originalAmount = $('.original-amount').val();
    var availableAmount = $('.available-amount').val();
    var stockAmount = $('.stock-amount').val();
    var startTradingDate = $('.start-trading-date').val();
    var endTradingDate = $('.end-trading-date').val();
    var signDate = $('.sign-date').val();
    var profitPercent = $('.profit-percent').val();

    if (loginUserId.Trim() == "") {
        valResult = false;
        alert("请选择一个用户名称");
    }
    else if (stockAccountLogin.Trim() == "") {
        valResult = false;
        alert("请填写正确的登陆账号");
    }
    else if (stockAccountPwd.Trim() == "") {
        valResult = false;
        alert("请填写正确的登陆密码");
    }
    else if (originalAmount.Trim() == "" || !IsNumber(originalAmount.Trim())) {
        valResult = false;
        alert("请填写正确的初始资产");
    }
    else if (availableAmount.Trim() == "" || !IsNumber(availableAmount.Trim())) {
        valResult = false;
        alert("请填写正确的可用资金");
    }
    else if (stockAmount.Trim() == "" || !IsNumber(stockAmount.Trim())) {
        valResult = false;
        alert("请填写正确的股票市值");
    }
    else if (startTradingDate.Trim() == "" || !IsyyyyMMddStrFormat(startTradingDate.Trim())) {
        valResult = false;
        alert("请填写正确的开始日期");
    }
    else if (endTradingDate.Trim() == "" || !IsyyyyMMddStrFormat(endTradingDate.Trim())) {
        valResult = false;
        alert("请填写正确的结束日期");
    }
    else if (signDate.Trim() == "" || !IsyyyyMMddStrFormat(signDate.Trim())) {
        valResult = false;
        alert("请填写正确的签约日期");
    }
    else if (profitPercent.Trim() == "" || +(profitPercent.Trim()) > 100 || +(profitPercent.Trim()) < 0) {
        valResult = false;
        alert("请填写正确的收益分比");
    }

    return valResult;
}

function getTradingClientInfoObj() {
    var tradingClientInfo = new Object();
    if (isTradingClientEdit) {
        tradingClientInfo.ID = tradingClientEditId;
    }
    tradingClientInfo.UserID = $('#LoginUserId').val();
    tradingClientInfo.StockAccountLogin = $('.stock-account-login').val();
    tradingClientInfo.StockAccountPassword = $('.stock-account-pwd').val();
    tradingClientInfo.OriginalAmount = $('.original-amount').val();
    tradingClientInfo.StockAmount = $('.available-amount').val();
    tradingClientInfo.AvaliableAmount = $('.stock-amount').val();
    tradingClientInfo.StartTradingDate = $('.start-trading-date').val();
    tradingClientInfo.EndTradingDate = $('.end-trading-date').val();
    tradingClientInfo.SignDate = $('.sign-date').val();
    tradingClientInfo.ProfitPercent = $('.profit-percent').val();
    // other fields
    return tradingClientInfo;
}

function reloadTradingClient() {
    $.ajax({
        url: "/Stock/ReloadTradingClient",
        data: {},
        type: "POST",
        success: function (data) {
            $('.admin-content').html(data);
            initCommonDataTable('#StockDataTbl');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Reload trading client error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}

function editTradingClient(obj) {
    var rowObj = $(obj).closest('tr');
    isTradingClientEdit = true;
    tradingClientEditId = $(rowObj).data('tradingclientid');

    $('#LoginUserId').attr('disabled', 'disabled');
    $('#LoginUserId').val($(rowObj).data('userid'));
    $('.stock-account-login').val($(rowObj).data('stockaccountlogin'));
    $('.stock-account-pwd').val($(rowObj).data('stockaccountpassword'));
    $('.original-amount').val($(rowObj).data('originalamount'));
    $('.available-amount').val($(rowObj).data('avaliableamount'));
    $('.stock-amount').val($(rowObj).data('stockamount'));
    $('.start-trading-date').val($(rowObj).data('starttradingdate'));
    $('.end-trading-date').val($(rowObj).data('endtradingdate'));
    $('.sign-date').val($(rowObj).data('signdate'));
    $('.profit-percent').val($(rowObj).data('profitpercent'));
}

function clearAllValues() {
    $('#LoginUserId').removeAttr('disabled');
    $('#LoginUserId').val("");
    $('.stock-account-login').val("");
    $('.stock-account-pwd').val("");
    $('.original-amount').val("");
    $('.available-amount').val("");
    $('.stock-amount').val("");
    $('.start-trading-date').val("");
    $('.end-trading-date').val("");
    $('.sign-date').val("");
    $('.profit-percent').val("");
    isTradingClientEdit = false;
    tradingClientEditId = 0;
}
