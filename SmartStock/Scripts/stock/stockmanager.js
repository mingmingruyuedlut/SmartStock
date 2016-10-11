
var isTradingStockEdit = false;
var tradingStockEditId = 0;

function bindStockManagerEvent() {
    $(document).on('click', '.stock-add-btn', function () {
        clearAllValues();
        $('.addStockDiv').show();
    });

    $(document).on('click', '.stock-cancel-btn', function () {
        clearAllValues();
        $('.addStockDiv').hide();
    });

    $(document).on('click', '.editAction', function () {
        $('.addStockDiv').show();
        editTradingStock(this);
    });

    $(document).on('click', '.deleteAction', function () {
        //deleteTradingStock(this);
    });

    $(document).on('click', '.stock-save-btn', function () {
        saveTradingStock();
    });
}

function saveTradingStock() {
    if (validateInputs()) {
        var tradingStockInfo = getTradingStockInfoObj()
        var tradingStockInfoJsonStr = JSON.stringify(tradingStockInfo);
        $.ajax({
            url: "/Stock/SaveTradingStock",
            data: { TradingStockJsonStr: tradingStockInfoJsonStr, IsEdit: isTradingStockEdit },
            type: "POST",
            success: function (data) {
                if (data == 'success') {
                    reloadTradingStock();
                    clearAllValues();
                    $('.addStockDiv').hide();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Save trading stock error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
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
    var stockCode = $('.stock-code').val();
    var stockName = $('.stock-name').val();
    var originalNumber= $('.original-number').val();
    var closingPrice = $('.closing-price').val();
    var stockHolderCode = $('.stock-holder-code').val();
    var cashAccountNo = $('.cash-account-no').val();

    if (loginUserId.Trim() == "") {
        valResult = false;
        alert("请填选择一个用户名称");
    }
    else if (stockCode.Trim() == "") {
        valResult = false;
        alert("请填写正确的股票代码");
    }
    else if (stockName.Trim() == "") {
        valResult = false;
        alert("请填写正确的股票名称");
    }
    else if (originalNumber.Trim() == "" || !(/^[0-9]*[1-9][0-9]*$/.test(originalNumber.Trim()))) {
        valResult = false;
        alert("请填写正确的股票数量");
    }
    else if (isNaN(closingPrice.Trim())) {
        valResult = false;
        alert("请填写正确的收盘价格");
    }
    else if (stockHolderCode.Trim() == "") {
        valResult = false;
        alert("请填写正确的股东代码");
    }
    else if (cashAccountNo.Trim() == "") {
        valResult = false;
        alert("请填写正确的资金账号");
    }

    return valResult;
}

function getTradingStockInfoObj() {
    var tradingStockInfo = new Object();
    if (isTradingStockEdit) {
        tradingStockInfo.ID = tradingStockEditId;
    }

    tradingStockInfo.UserID = $('#LoginUserId').val();
    tradingStockInfo.StockCode = $('.stock-code').val();
    tradingStockInfo.StockName = $('.stock-name').val();
    tradingStockInfo.OrignalNumber = $('.original-number').val();
    tradingStockInfo.ClosingPrice = $('.closing-price').val();
    tradingStockInfo.StockHolderCode = $('.stock-holder-code').val();
    tradingStockInfo.CashAccountNo = $('.cash-account-no').val();

    // other fields
    return tradingStockInfo;
}

function reloadTradingStock() {
    $.ajax({
        url: "/Stock/ReloadTradingStock",
        data: { },
        type: "POST",
        success: function (data) {
            $('.admin-content').html(data);
            initCommonDataTable('#StockDataTbl');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Reload trading stock by category error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}

function editTradingStock(obj) {
    var rowObj = $(obj).closest('tr');
    isTradingStockEdit = true;
    tradingStockEditId = $(rowObj).data('tradingstockid');

    $('#LoginUserId').attr('disabled', 'disabled');
    $('#LoginUserId').val($(rowObj).data('userid'));
    $('.stock-code').val($(rowObj).data('stockcode'));
    $('.stock-name').val($(rowObj).data('stockname'));
    $('.original-number').val($(rowObj).data('orignalnumber'));
    $('.closing-price').val($(rowObj).data('closingprice'));
    $('.stock-holder-code').val($(rowObj).data('stockholdercode'));
    $('.cash-account-no').val($(rowObj).data('cashaccountno'));
}

function clearAllValues() {
    $('#LoginUserId').removeAttr('disabled');
    $('#LoginUserId').val("");
    $('.stock-code').val("");
    $('.stock-name').val("");
    $('.original-number').val("");
    $('.closing-price').val("");
    $('.stock-holder-code').val("");
    $('.cash-account-no').val("");
    isTradingStockEdit = false;
    tradingStockEditId = 0;
}

