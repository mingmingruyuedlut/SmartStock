function bindUploadTradingOrderEvent() {
    $('#dataInputFile').change(function () {
        // will log a FileList object, view gifs below
        // console.log(this.files);
        // to-do
    });

    $(document).on('click', '.data-upload-action', function () {
        uploadTradingOrderDataFile();
    });

    $(document).on('click', '.file-remove-action', function () {
        removeChooseFile();
    });
}

function uploadTradingOrderDataFile() {
    var filePath = $('#dataInputFile').val();
    if (filePath == '') {
        alert('请选择一个正确的数据文件');
    }
    else {
        var fileObj = $('#dataInputFile')[0].files[0];
        var formData = new FormData();
        formData.append('filename', fileObj)
        $.ajax({
            url: "/Stock/UploadTradingOrderDataFile",
            data: formData,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data == 'success') {
                    alert('上传成功');
                    removeChooseFile();
                }
                else {
                    alert(data);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Upload trading order data file error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
}

function removeChooseFile() {
    $('#dataInputFile').val('');
}


