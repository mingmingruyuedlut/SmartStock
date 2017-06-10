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


