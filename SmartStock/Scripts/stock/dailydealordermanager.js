function bindUploadDailyDealOrderEvent() {
    $(document).on('click', '.data-upload-action', function () {
        uploadDailyDealOrderDataFile(this);
    });

    $(document).on('click', '.file-remove-action', function () {
        removeChooseFile(this);
    });
}

function uploadDailyDealOrderDataFile(obj) {
    var fileInputObj = $(obj).parent().find('.trading-order-upload');
    if (fileInputObj.val() == '') {
        alert('请选择一个正确的数据文件');
    }
    else {
        var formData = new FormData();
        formData.append('resourcetype', fileInputObj.data('resourcetype'));
        formData.append('filename', fileInputObj[0].files[0]);
        $.ajax({
            url: "/Stock/UploadDailyDealOrderDataFile",
            data: formData,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data == 'success') {
                    alert('上传成功');
                    removeChooseFile(obj);
                }
                else {
                    alert(data);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Upload daily deal order data file error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
}

function removeChooseFile(obj) {
    $(obj).parent().find('.trading-order-upload').val('');
}


function bindPickupDailyDealOrderEvent() {
    $(document).on('click', '.pickup-save-btn', function () {
        savePickupOrders();
    });
}

function savePickupOrders() {
    var orderIds = getOrderIdsListObj();
    if (orderIds.length > 0) {
        var orderIdsJsonStr = JSON.stringify(orderIds);
        $.ajax({
            url: "/Stock/SavePickupOrders",
            data: { OrderIds: orderIdsJsonStr },
            type: "POST",
            success: function (data) {
                if (data == 'success') {
                    reloadPickupDailyDealOrder();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Save pickup orders error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        alert("请勾选要更新的成交订单");
    }
}

function getOrderIdsListObj() {
    var ids = new Array();
    $('#DailyDealOrderDataTbl input:checkbox:checked').each(function (index, item) {
        var rowObj = $(item).closest('tr');
        ids.push($(rowObj).data('orderid'));
    });
    return ids;
}

function reloadPickupDailyDealOrder() {
    $.ajax({
        url: "/Stock/ReloadPickupDailyDealOrder",
        data: {},
        type: "POST",
        success: function (data) {
            $('.admin-content').html(data);
            initCommonDataTable('#DailyDealOrderDataTbl');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Reload pickup daily deal order error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}


function bindMyPickupDailyDealOrderEvent() {
    $(document).on('click', '.unPickupAction', function () {
        unPickupOrder(this);
    });
}

function unPickupOrder(obj) {
    var rowObj = $(obj).closest('tr');
    var orderId = $(rowObj).data('orderid');
    $.ajax({
        url: "/Stock/UnPickupOrder",
        data: { OrderId: orderId },
        type: "POST",
        success: function (data) {
            if (data == 'success') {
                reloadMyPickupDailyDealOrder();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Unpickup order error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}

function reloadMyPickupDailyDealOrder() {
    $.ajax({
        url: "/Stock/ReloadMyPickupDailyDealOrder",
        data: {},
        type: "POST",
        success: function (data) {
            $('.admin-content').html(data);
            initCommonDataTable('#DailyDealOrderDataTbl');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Reload my pickup daily deal order error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}