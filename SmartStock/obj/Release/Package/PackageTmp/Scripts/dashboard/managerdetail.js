
function initTradingOrderDetailDataTable(tableSelector) {
    var datatbl = $(tableSelector).dataTable();
    var curTabIndex = 0;
    var inputOptions = "input, textarea, select";
    datatbl.$('td').has(inputOptions)
        .each(function (index, item) {
            var td = $(this); //item == td == this
            var currentLabel = $('label.oldvalue', td);
            var currentColumnId = $(td).data('columnid');
            var currentTradingOrderId = $(td).closest('tr').data('tradingorderid');
            td.attr('data-tab-index', index);
            td.on('click.tabIndex', function () {
                curTabIndex = Number($(this).attr("data-tab-index"));
                editCellData(curTabIndex);
            });
            td.find(inputOptions).click(function (event) { event.stopPropagation(); });
            td.find("input,textarea")
                .blur(function () {
                    var ovalue = currentLabel.text().Trim();
                    var nvalue = $(this).val().Trim();

                    if (ovalue != nvalue) {

                        if (!validateTradingOrderInput(nvalue, currentColumnId)) {
                            alert("数据格式不正确，请重新输入.");
                            $(this).focus();
                        }
                        else {
                            saveTradingOrderColumnValue(currentTradingOrderId, currentColumnId, nvalue);
                            currentLabel.text(nvalue).show();
                            $(this).val("").hide();
                        }
                    }
                    else {
                        currentLabel.show();
                        $(this).hide();
                    }
                })
                .keydown(function (event) {
                    var ovalue = currentLabel.text().Trim();
                    var nvalue = $(this).val().Trim();

                    if (event.keyCode == 9) {
                        event.preventDefault();

                        if (ovalue == nvalue || validateTradingOrderInput(nvalue, currentColumnId)) {
                            tabNext();
                        }
                        else {
                            alert("数据格式不正确，请重新输入.");
                            $(this).val($(this).val().Trim().replace('\t', ''));
                            $(this).focus();
                        }
                    } else if (event.keyCode == 13) {
                        event.preventDefault();
                        $(this).blur();
                    }
                    else if (event.keyCode == 27) {
                        $(this).blur();
                    }
                });
            td.find("select")
                .blur(function () {
                    var ovalue = currentLabel.text().Trim();
                    var nvalue = $(this).find('option:selected').text().Trim();

                    if (ovalue != nvalue) {

                        var svalue = $(this).find('option:selected').val().Trim();
                        saveTradingOrderColumnValue(currentTradingOrderId, currentColumnId, svalue);
                        currentLabel.text(nvalue).show();
                        currentLabel.attr("title", nvalue);
                        $(this).hide();
                    }
                    else {
                        currentLabel.show();
                        $(this).hide();
                    }
                })
                .keydown(function (event) {
                    if (event.keyCode == 9) {
                        event.preventDefault();
                        tabNext();
                    } else if (event.keyCode == 13) {
                        event.preventDefault();
                        $(this).blur();
                    }
                    else if (event.keyCode == 27) {
                        $(this).blur();
                    }
                });
        });

    function tabNext() {
        curTabIndex++;
        var maxCurPageTabIndex = datatbl.find(inputOptions).length; // this is for current page
        var maxTabIndex = datatbl.$('td').find(inputOptions).length; // this is for all pages
        var curPageNo = 0;
        var PagesCount = parseInt(maxTabIndex / maxCurPageTabIndex);
        for (var i = 0; i < PagesCount; i++) {
            if (curTabIndex >= maxCurPageTabIndex * i && curTabIndex <= maxCurPageTabIndex * (i + 1)) {
                curPageNo = i;
                break;
            }
        }
        curTabIndex = (curTabIndex < maxCurPageTabIndex * (i + 1) && curTabIndex < maxTabIndex) ? curTabIndex : maxCurPageTabIndex * i;
        editCellData(curTabIndex);

    }

    function getInputElementType(td) {
        var currentCell = td;
        var currentInput = currentCell.find(inputOptions);

        if (currentInput.length > 1) {
            alert("发现多个输入框，不支持");
            return;
        }
        var currentType = (currentInput.is("input, textarea")) ? "input" : (currentInput.is("select")) ? "select" : null;

        return currentType;
    }

    function editCellData(tabIndex) {
        var td = $('#DashboardDataTbl tr td[data-tab-index =' + tabIndex + ']');
        var currentLabel = td.find("label.oldvalue");
        var currentInput = td.find("input, textarea, select");
        var currentInputType = getInputElementType(td);

        switch (currentInputType) {
            case 'input':
                currentInput.val(currentLabel.text().Trim());
                break;
            case 'select':
                currentInput.find("option[value='" + currentLabel.text().Trim() + "']").attr("selected", "selected");
                break;
            default:
                alert("Edit Cell :: Input not found");
                return;
                break;
        }
        currentLabel.hide();
        currentInput.show().focus().mousedown();
    }
}

function validateTradingOrderInput(inputValue, tblColumnId) {
    var vResult = true;
    //if (tblColumnId == "OrderNum") {
    //    var orderRegex = /^[1-9][0-9]{11}$/;
    //    if (inputValue.Trim() != '' && !orderRegex.test(inputValue.Trim())) {
    //        vResult = false;
    //    }
    //}
    return vResult;
}

function saveTradingOrderColumnValue(tradingOrderId, columnId, value) {
    $.ajax({
        url: "/Dashboard/SaveTradingOrderColumnValue",
        data: { ID: tradingOrderId, ColumnId: columnId, Value: value },
        type: "POST",
        success: function (data) {
            //alert(data);
            //to do
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Save trading order column value error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            vResult = false;
        }
    });
}

function bindDashboardPageEvent() {
    $(document).on('click', '#reloadBtn', function () {
        reloadManagerDashboardDetail();
    });
}

function reloadManagerDashboardDetail() {
    var startDate = $("#StartDate").val();
    var endDate = $("#EndDate").val();
    var clientUserId = $('#ClientUserId').val() === '' ? 0 : +$('#ClientUserId').val();
    if (validateDateRange(startDate, endDate)) {
        $.ajax({
            url: "/Dashboard/ReloadManagerDashboardDetailPartial",
            data: { StartDate: startDate, EndDate: endDate, UserId: clientUserId },
            type: "POST",
            success: function (data) {
                $('.dashboard-partial').html(data);
                initTradingOrderDetailDataTable('#DashboardDataTbl');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Reload manager dashboard detail partial error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            }
        });
    }
    else {
        // to - do
    }
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

