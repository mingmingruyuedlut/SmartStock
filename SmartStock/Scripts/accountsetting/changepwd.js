/******Account Password Information Page******/
/******Some Common Functions In smartstock.js******/

function bindChangePasswordEvent() {
    $('.pwd-save-btn').bind('click', function () {
        var oldPwd = $('#OldPassword').val();
        var newPwd = $('#NewPassword').val();
        var confirmedNewPwd = $('#ConfirmedNewPassword').val();
        var userId = $('#UserId').val();
        if (newPwd != confirmedNewPwd) {
            alert("输入的新密码与确认密码不相同");
        }
        else if (!checkOldPasswordIsValid(userId, oldPwd)) {
            alert("输入的原始密码不正确");
        }
        else {
            saveAccountPasswordInfo();
        }
    });
}

function saveAccountPasswordInfo() {
    var accountPwd = getAccountPasswordInfoObj()
    var accountPwdJsonStr = JSON.stringify(accountPwd);
    $.ajax({
        url: "/AccountSetting/ChangeAccountPassword",
        data: { AccountPwdJsonStr: accountPwdJsonStr },
        type: "POST",
        success: function (data) {
            if (data == 'success') {
                alert("保存成功");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Save admin account password info error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
        }
    });
}

function getAccountPasswordInfoObj() {
    var accountPwd = new Object();
    accountPwd.UserId = $('#UserId').val();
    accountPwd.OldPassword = $('#OldPassword').val();
    accountPwd.NewPassword = $('#NewPassword').val();
    accountPwd.ConfirmedNewPassword = $('#ConfirmedNewPassword').val();
    return accountPwd;
}

function checkOldPasswordIsValid(userId, oldPwd) {
    var vResult = true;
    $.ajax({
        url: "/AccountSetting/CheckOldPassword",
        data: { UserId: userId, OldPwd: oldPwd },
        type: "POST",
        async: false,
        success: function (data) {
            if (data == false) {
                vResult = false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Check old password is valid error! status:" + XMLHttpRequest.status + " readyState:" + XMLHttpRequest.readyState + " textStatus:" + textStatus);
            vResult = false;
        }
    });
    return vResult;
}

/******Account Password Information Page******/